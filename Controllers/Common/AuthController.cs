using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.Common;

namespace THMS.Core.API.Controllers.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    [EnableCors("any")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        private readonly UserInfoService _UserInfoService = new UserInfoService();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenService"></param>
        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// 登入验证
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetToken(string UserName, string Password)
        {
            var user = await _UserInfoService.CheckUserInfo(UserName, Password);

            if (user == null)
            {
                return "Login Failed";
            }
            else
            {
                var token = _tokenService.GetToken(user);

                var response = new
                {
                    Status = true,
                    Token = token,
                    Type = "Bearer"
                };

                return JsonConvert.SerializeObject(response);

            }
        }
    }
}