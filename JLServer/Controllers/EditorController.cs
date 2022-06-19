using JL_ApiModels.Request.Editor;
using JL_ApiModels.Response.Editor;
using JL_MSSQLServer.PersistModels;
using JL_Service.Abstraction.Editor;
using JL_Utility.Models;
using JLServer.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JLServer.Controllers
{
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuthController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserSettings _userSettings;

        public EditorController(ILogger<AuthController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _serviceProvider = provider;
            _httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            _userSettings = new UserSettings((User)_httpContextAccessor.HttpContext.Items["User"]);
        }

        [HttpPost]
        [JwtAuthentication(role: "Editor")]
        [Route("/editor/material")]
        public async Task<NewMaterialResponse> NewMaterial([FromBody] NewMaterialRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<INewMaterialPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new NewMaterialResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPut]
        [JwtAuthentication(role: "Editor")]
        [Route("/editor/material")]
        public async Task<UpdateMaterialResponse> UpdateMaterial([FromBody] UpdateMaterialRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IUpdateMaterialPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new UpdateMaterialResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/editor/material/{fileId}")]
        public async Task<GetMaterialResponse> GetMaterial([FromRoute][Required] int fileId)
        {
            try
            {
                var point = _serviceProvider.GetService<IGetMaterialDataPoint>();
                return await point.Start(fileId, _userSettings);
            }
            catch (Exception er)
            {
                return new GetMaterialResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/editor/course-material/{id}")]
        public async Task<GetCourseManualResponse> GetCourseMaterial([FromRoute][Required] int id)
        {
            try
            {
                var point = _serviceProvider.GetService<IGetMaterialByIdPoint>();
                return await point.Start(id, _userSettings);
            }
            catch (Exception er)
            {
                return new GetCourseManualResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }


        [HttpGet]
        [JwtAuthentication(role: "Editor")]
        [Route("/editor/my-materials")]
        public async Task<GetMyMaterialsResponse> MyMaterials()
        {
            try
            {
                var point = _serviceProvider.GetService<IGetMyMaterialsPoint>();
                return await point.Start(null, _userSettings);
            }
            catch (Exception er)
            {
                return new GetMyMaterialsResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }

        }
    }
}
