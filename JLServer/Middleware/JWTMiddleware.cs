using JL_MSSQLServer.PersistModels;
using JL_Service.Abstraction.Auth;
using JL_Utility.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JLServer.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationSettings _appSettings;

        public JWTMiddleware(RequestDelegate next, IOptions<ApplicationSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IGetRolesByUserIdPoint getRolesPoint, IGetUserByIdPoint getUserPoint)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var user = attachUserToContext(context, getUserPoint, token);
                var roles = attachRolesToContext(context, getRolesPoint, user);
            }
            await _next(context);
        }

        private User attachUserToContext(HttpContext context, IGetUserByIdPoint point, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.SecretWord);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var userData = point.Execute(userId, null).Result;
                context.Items["User"] = userData;
                return userData;
            }
            catch (Exception er) { throw new Exception(nameof(token)); }
        }

        private Role[] attachRolesToContext(HttpContext context, IGetRolesByUserIdPoint point, User user)
        {
            var rolesData = point.Execute(user.Id, null).Result.ToArray();
            context.Items["Roles"] = rolesData;
            return rolesData;
        }
    }
}
