using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UsbSerialNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TruthName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PasswordFormat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PasswordType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PasswordSalt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastLogintime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LastLoginIp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CreateUser_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FailedPasswordAttemptCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Organization_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
