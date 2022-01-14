using SqlSugar;
using System;

namespace Sugar.Enties
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("da_realvalue")]
    public partial class da_realvalue
    {
        public da_realvalue()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true)]
        public string DeviceNum { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public decimal TValue { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public decimal Humidity { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime RecordTime { get; set; }

    }
}
