using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    ///<summary>
    ///住户信息表
    ///</summary>
    public class Sys_ResidentUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        ///<summary>
        ///住户id（guid）
        ///</summary>
        public string ResidentUser_id { get; set; }

        ///<summary>
        ///所属单元ID
        ///</summary>
        public string UnitNo_id { get; set; }

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

        ///<summary>
        ///建筑面积
        ///</summary>
        public decimal BuiltArea { get; set; }

        ///<summary>
        ///收费面积
        ///</summary>
        public decimal ChargeArea { get; set; }

        ///<summary>
        ///供热状态
        ///</summary>
        public int HeatingState { get; set; }

        ///<summary>
        ///删除
        ///</summary>
        public bool IsDelete { get; set; } = false;

        ///<summary>
        ///收费状态
        ///</summary>
        public bool ChargeState { get; set; }

        ///<summary>
        ///建筑类型
        ///</summary>
        public int BuildType { get; set; }

        ///<summary>
        ///收费类型
        ///</summary>
        public int PaymentType { get; set; }

        ///<summary>
        ///采暖类型
        ///</summary>
        public int HeatingType { get; set; }

        ///<summary>
        ///保温类型
        ///</summary>
        public int IsHeatPreservationType { get; set; }

        ///<summary>
        ///户型类别
        ///</summary>
        public int HouseType { get; set; }

        ///<summary>
        ///备注
        ///</summary>
        public string Remarks { get; set; }


    }
}