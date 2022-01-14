using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using THMS.Core.API.Models.Common;

namespace THMS.Core.API.Service.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetToken(UserInfo user);
    }
    /// <summary>
    /// 
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtSetting _jwtSetting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        public TokenService(IOptions<JwtSetting> option)
        {
            _jwtSetting = option.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetToken(UserInfo user)
        {
            //创建用户身份标识，可按需要添加更多信息
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString(), ClaimValueTypes.Integer32),
                new Claim("name", user.UserName)
                //new Claim("admin", user.IsAdmin.ToString(),ClaimValueTypes.Boolean)
            };

            //创建令牌
            var token = new JwtSecurityToken(
                    issuer: _jwtSetting.Issuer,
                    audience: _jwtSetting.Audience,
                    signingCredentials: _jwtSetting.Credentials,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddSeconds(_jwtSetting.ExpireSeconds)
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
