using JL_ApiModels.Request.Auth;
using JL_ApiModels.Response.Auth;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Auth;
using JL_Service.Exceptions;
using JL_Utility.Logger;
using JL_Utility.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JL_Service.Implementation.Auth
{
    public class LoginPoint : PointBase<LoginRequest, LoginResponse>, ILoginPoint
    {
        private readonly IAuthDataRepository _authDataRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGetRolesByUserIdPoint _getRolesByUserIdPoint;
        private readonly IJLLogger _logger;
        private readonly ApplicationSettings _appSettings;

        public LoginPoint(
            IAuthDataRepository _authDataRepository,
            IUserRepository _userRepository,
            IGetRolesByUserIdPoint _getRolesByUserIdPoint,
            IJLLogger _logger,
            IOptions<ApplicationSettings> appSettings,
            ApplicationContext _context) : base(_context)
        {
            this._authDataRepository = _authDataRepository;
            this._userRepository = _userRepository;
            this._getRolesByUserIdPoint = _getRolesByUserIdPoint;
            this._appSettings = appSettings.Value;
            this._logger = _logger;
        }

        public override async Task<LoginResponse> Execute(LoginRequest req, UserSettings userSettings)
        {
            var response = new LoginResponse();

            var passwordHash = ComputeSHA256Hash(req.Password);

            var authData = _authDataRepository.Get().FirstOrDefault(x => x.Login == req.Login && x.PasswordHash == passwordHash)
                ?? throw new PointException("Логин и/или пароль не найден(ы)", _logger);

            var user = _userRepository.Get().FirstOrDefault(x => x.Id == authData.UserId)
                ?? throw new PointException("Данные пользователя не найдены", _logger);

            var roles = await _getRolesByUserIdPoint.Execute(user.Id, userSettings)
                ?? throw new PointException("У пользователя нет ролей", _logger);

            response.JWT = GenerateJwtToken(user);
            response.Roles = roles;
            response.User = user;
            response.Message = $"{user.FirstName} {user.ThirdName}, Добро пожаловать в JointLesson";

            return response;
        }

        private string GenerateJwtToken(JL_MSSQLServer.PersistModels.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretWord);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string ComputeSHA256Hash(string text)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(text));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
