using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.PvssDSSystem
{
    /// <summary>
    /// PVSS原API同步基础数据参数
    /// </summary>
    public static class PvssDSDto
    {
        /// <summary>
        ///  同步PVSS与XLINK 换热站数据
        /// </summary>
        public class Station_PVSS
        {
            //pvss中的站点id
            public string Pvss_id { get; set; }
            //站点在热源上的位置
            public int StationNumber { get; set; }
            //站点名称
            public string StationName { get; set; }
            //组织结构编码
            public int? OrganizationCode { get; set; }
            //站点地址
            public string StationAddress { get; set; }
            //供热面积
            public decimal? StationHotArea { get; set; }
            //机组总数
            public int? StationArrayCount { get; set; }
            //pvss中所属热源的id
            public string Pvss_PowerId { get; set; }
            //供热方式
            public int HeatType { get; set; }
            //X坐标
            public string Xaxis { get; set; }
            //Y坐标
            public string Yaxis { get; set; }

            // 热站类型（住宅区/工业区）
            public int StationType { get; set; }
            // 是否为自管站
            public int IsSelfManage { get; set; }

            public int HeatNetID { get; set; }

            public string DisignPower { get; set; }

            public List<StationBranchInsertPvss> StationBranch;
        }

        /// <summary>
        /// 换热站机组信息（PVSS）
        /// </summary>
        public class StationBranchInsertPvss
        {
            //机组号
            public int StationBranchArrayNumber { get; set; }
            //机组名称
            public string StationBranchName { get; set; }
            //机组面积
            public decimal StationBranchArea { get; set; }

            // 用热类型（挂暖/地暖）
            public int HeatType { get; set; }

            // 是否节能建筑
            public int IsEnergySave { get; set; }

            // 父级ID
            public int ParentId { get; set; }
        }

        /// <summary>
        /// 同步PVSS与Xlink热网数据
        /// </summary>
        public class MdjRw_PVSS
        {
            //pvss中的站点id
            public string Pvss_id { get; set; }
            //站点在热源上的位置
            public int StationNumber { get; set; }
            //站点名称
            public string StationName { get; set; }
            //组织结构编码
            public int? OrganizationCode { get; set; }
            //站点地址
            public string StationAddress { get; set; }
            //供热面积
            public decimal? StationHotArea { get; set; }
            //机组总数
            public int? StationArrayCount { get; set; }
            public int HeatNetType { get; set; }
        }

        public class MdjPower_PVSS
        {
            //pvss中的站点id
            public string Pvss_id { get; set; }
            //站点在热源上的位置
            public int StationNumber { get; set; }
            //站点名称
            public string StationName { get; set; }
            //组织结构编码
            public int? OrganizationCode { get; set; }
            //站点地址
            public string StationAddress { get; set; }
            //供热面积
            public decimal? StationHotArea { get; set; }
            //机组总数
            public int? StationArrayCount { get; set; }

            //pvss中的父级id
            public string Pvss_ParentId { get; set; }
            //供热方式
            public int HeatType { get; set; }
            //X坐标
            public string  Xaxis { get; set; }
            //Y坐标
            public string Yaxis { get; set; }

            // 热源类型（电厂/燃气锅炉房/煤气锅炉房）
            public int SourceType { get; set; }

            //供热类型（集中/区域）
            public int HeatNetType { get; set; }

            public string DisignPower { get; set; }

            public List<PowerStationBranchInsertPvss> StationBranch;
        }

        /// <summary>
        /// 热源机组信息（PVSS）
        /// </summary>
        public class PowerStationBranchInsertPvss
        {
            //机组号
            public int StationBranchArrayNumber { get; set; }
            //机组名称
            public string StationBranchName { get; set; }
            //机组面积
            public decimal StationBranchArea { get; set; }

            // 父级ID
            public int ParentId { get; set; }
        }
        /// <summary>
        /// 热网信息（PVSS）
        /// </summary>
        public class PowerRW_PVSS
        {
            //热网编号
            public string PcName { get; set; }
            //热网名称
            public string StationName { get; set; }
            //组织结构ID
            public int Organization_id { get; set; }
            //热网地址
            public string StationAddress { get; set; }
            //供热面积
            public decimal? StationHotArea { get; set; }
            //机组数量
            public int StationArrayCount { get; set; }
            //供热方式
            public int HeatType { get; set; }

        }

        /// <summary>
        /// 热源信息（PVSS）
        /// </summary>
        public class PowerRY_PVSS
        {
            //热源编号
            public string PcName { get; set; }
            //热源名称
            public string StationName { get; set; }
            //组织结构ID
            public int Organization_id { get; set; }
            //热源地址
            public string StationAddress { get; set; }
            //供热面积
            public decimal? StationHotArea { get; set; }
            //机组数量
            public int StationArrayCount { get; set; }
            //供热方式
            public int HeatType { get; set; }
            //所属热网
            public string PcName_RW { get; set; }
            //机组
            public List<StationBranchInsert> StationBranch;

        }
        /// <summary>
        /// 机组信息（PVSS）
        /// </summary>
        public class StationBranchInsert
        {
            //机组号
            public int StationBranchArrayNumber { get; set; }
            //机组名称
            public string StationBranchName { get; set; }
            //机组面积
            public decimal StationBranchArea { get; set; }
        }

        /// <summary>
        /// 公司信息新增内容(PVSS)
        /// </summary>
        public partial class Organization
        {
            /// <summary>
            /// 组织结构ID
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// 组织结构编码
            /// </summary>
            public int OrganizationCode { get; set; }

            /// <summary>
            /// 组织结构名称
            /// </summary>
            public string OrganizationName { get; set; }

            /// <summary>
            /// 组织机构描述
            /// </summary>
            public string OrganizationDesc { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime? CreateTime { get; set; }

            /// <summary>
            /// 是否有效
            /// </summary>
            public bool IsValid { get; set; }

            /// <summary>
            /// 公司面积
            /// </summary>
            public decimal HeatArea { get; set; }

            /// <summary>
            /// 部门的父部门ID(用于部门管理的递归树)
            /// </summary>
            public int ParentDepID { get; set; }

            /// <summary>
            /// 部门的级别
            /// </summary>
            public int DepLevel { get; set; }

            /// <summary>
            /// 创建人
            /// </summary>
            public string CreateUser { get; set; }
        }

        /// <summary>
        /// 修改公司组织结构参数(PVSS)
        /// </summary>
        public class UpdateOrgPvss
        {
            /// <summary>
            /// 组织结构编码
            /// </summary>
            public int OrganizationCode { get; set; }

            /// <summary>
            /// 组织结构名称
            /// </summary>
            public string OrganizationName { get; set; }

            /// <summary>
            /// 组织机构描述
            /// </summary>
            public string OrganizationDesc { get; set; }

            /// <summary>
            /// 公司面积
            /// </summary>
            public decimal HeatArea { get; set; }

            /// <summary>
            /// 部门的父部门ID(用于部门管理的递归树)
            /// </summary>
            public int ParentDepID { get; set; }

            /// <summary>
            /// 部门的级别
            /// </summary>
            public int DepLevel { get; set; }

            /// <summary>
            /// 是否有效
            /// </summary>
            public bool IsValid { get; set; } = true;
        }
    }
}
