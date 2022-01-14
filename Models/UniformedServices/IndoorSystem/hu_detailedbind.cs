using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.IndoorSystem
{
    /// <summary>
    /// 用户绑定表
    /// </summary>
    public class hu_detailedbind
    {
        /// <summary>
        /// 
        /// </summary>
        public hu_detailedbind()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String UserCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32 CompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32 LineId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? SourceId { get; set; }

        /// <summary>
        /// 换热站id
        /// </summary>
        public System.Int32 StationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? BranchId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Village { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Floor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Layer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String House { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String HolderChargeCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String HolderPhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String HolderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String LayerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? InstallationPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? HouseOrientation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MountingHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? SocketLocation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? HouseDirection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? BindTime { get; set; }
    }
}
