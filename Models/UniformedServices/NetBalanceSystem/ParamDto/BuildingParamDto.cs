using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem.ParamDto
{
    /// <summary>
    /// 调用华夏api参数类
    /// </summary>
    public class BuildingParamDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 上级id（动态）
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 层级描述
        /// </summary>
        public string layer { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string lon { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string leader { get; set; }

        /// <summary>
        /// 负责人电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 最低层
        /// </summary>
        public int minfloor { get; set; }

        /// <summary>
        /// 最高层
        /// </summary>
        public int maxfloor { get; set; }

        /// <summary>
        /// 底层范围
        /// </summary>
        public string lowfloor { get; set; }

        /// <summary>
        /// 中层范围
        /// </summary>
        public string middlefloor { get; set; }

        /// <summary>
        /// 高层范围
        /// </summary>
        public string highfloor { get; set; }

        /// <summary>
        /// 是否未知
        /// </summary>
        public string unknown { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        public string name_pinyin { get; set; }

        /// <summary>
        /// 拼音首字母
        /// </summary>
        public string name_first_pinyin { get; set; }

        /// <summary>
        /// 外部id
        /// </summary>
        public string externalid { get; set; }

        /// <summary>
        /// 外部pid
        /// </summary>
        public string externalpid { get; set; }

        /// <summary>
        /// 外部系统名称
        /// </summary>
        public string fromsystem { get; set; }

        /// <summary>
        /// 收费面积
        /// </summary>
        public decimal chargeablearea { get; set; }

        /// <summary>
        /// 服务人数
        /// </summary>
        public int peoplenumber { get; set; }
    }
}
