using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Common
{
    /// <summary>
    /// 系统用户信息表
    /// </summary>
    public class UserInfoService
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        DbContextSqlSugar DbContext = new DbContextSqlSugar();

        /// <summary>
        /// 登入验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">登入密码</param>
        /// <returns></returns>
        public async Task<UserInfo> CheckUserInfo(string username, string password)
        {
            var lowerName = username == null ? "" : username.ToLowerInvariant();

            var list = new List<UserInfo>();

            string sql = @"SELECT * FROM UserInfo WHERE 1=1";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                if (!string.IsNullOrEmpty(username))
                {
                    sql += @" AND  UserName = '" + username + "'";
                }

                list = await DbContext.Db.Ado.SqlQueryAsync<UserInfo>(sql);

                var state = ValidatePasswordHashed(list, password);

                if (state)
                {
                    return list.First();
                }
                else
                {
                    return null;
                }
            }
        }

        private bool ValidatePasswordHashed(List<UserInfo> userPart, string password)
        {

            var saltBytes = Convert.FromBase64String(userPart.First().PasswordSalt);

            var passwordBytes = Encoding.Unicode.GetBytes(password);

            var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

            byte[] hashBytes = new byte[] { };
            using (var hashAlgorithm = HashAlgorithm.Create(userPart.First().PasswordType))
            {
                if (hashAlgorithm != null) hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }

            return userPart.First().Password == Convert.ToBase64String(hashBytes);
        }

    }
}
