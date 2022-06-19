using JL_ApiModels.Request.Auth;
using JL_ApiModels.Response.Auth;
using JL_Service.Abstraction.Auth;
using JL_Utility.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JLServer.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _serviceProvider = provider;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/auth/login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<ILoginPoint>();
                return await point.Start(request, null);
            }
            catch (Exception er)
            {
                return new LoginResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }
    }
}
