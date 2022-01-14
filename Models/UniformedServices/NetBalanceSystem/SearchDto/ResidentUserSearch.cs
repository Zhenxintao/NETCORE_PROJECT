using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto
{
    /// <summary>
    /// 住户查询类
    /// </summary>
    public class ResidentUserSearch : UnitNoSearch
    {
        ///<summary>
        ///住户id（guid）
        ///</summary>
        public string ResidentUser_id { get; set; }

        ///<summary>
        ///用户卡号
        ///</summary>
        public string UserCard { get; set; }

        ///<summary>
        ///用户名称
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        ///入网用户名
        ///</summary>
        public string NetworkUserName { get; set; }

        ///<summary>
        ///房间号
        ///</summary>
        public string RoomNum { get; set; }

        ///<summary>
        ///楼层编号
        ///</summary>
        public int LayerNum { get; set; }

        ///<summary>
        ///联系电话
        ///</summary>
        public string UserPhone { get; set; }
    }
}
