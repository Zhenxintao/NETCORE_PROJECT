
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem.ParamDto;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem.ReturnDto;
using THMS.Core.API.Service.DbContext;
using THMS.Core.API.Service.UniformedServices.ComHttpRestSharp;

namespace THMS.Core.API.Service.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 基础信息上传
    /// </summary>
    public class BaseInfoDataUploadService : DbContextMySqlEw
    {
        /// <summary>
        /// 调用华夏api上传基础信息
        /// </summary>
        /// <returns></returns>
        public Response HxUpload()
        {
            var res = new Response() { IsSuccess = true };

            try
            {
                var list = GetSynchroRecord();

                StringBuilder delArr = new StringBuilder();
                foreach (var item in list)
                {
                    delArr.Append(item.Guid_Id + ",");
                    //小区表
                    if (item.TableType == 1)
                    {
                        //新增、更新
                        if (item.SynchroType == 1 || item.SynchroType == 2)
                            addCommunityData(item.Guid_Id, item.SynchroType);
                        //删除
                        if (item.SynchroType == 3)
                            deleteCommunityData(item.Guid_Id);
                    }

                    //楼栋表
                    if (item.TableType == 2)
                    {
                        //新增、更新
                        if (item.SynchroType == 1 || item.SynchroType == 2)
                            addBuildingData(item.Guid_Id, item.SynchroType);
                        //删除
                        if (item.SynchroType == 3)
                            deleteBuildingData(item.Guid_Id);
                    }

                    //单元表
                    if (item.TableType == 3)
                    {
                        //新增、更新
                        if (item.SynchroType == 1 || item.SynchroType == 2)
                        { }
                        //删除
                        if (item.SynchroType == 3)
                        { }
                    }

                    //住户表
                    if (item.TableType == 4)
                    {
                        //新增、更新
                        if (item.SynchroType == 1 || item.SynchroType == 2)
                            addHouseData(item.Guid_Id, item.SynchroType);
                        //删除
                        if (item.SynchroType == 3)
                            deleteHouseData(item.Guid_Id);
                    }
                }

                if (delArr.Length > 0)
                {
                    delArr.Remove(delArr.Length - 1, 1);
                    DeleteSynchroRecord(delArr.ToString().Split(","));
                }
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Msg = e.Message;
            }
            return res;
        }

        /// <summary>
        /// 清空同步记录信息表
        /// </summary>
        /// <returns></returns>
        public bool DeleteSynchroRecord(string[] idArr)
        {
            try
            {
                var con = DbMysql.Deleteable<synchrorecord>().In(s => s.Guid_Id, idArr).ExecuteCommand();
                return con > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取同步记录信息表数据
        /// </summary>
        /// <returns></returns>
        public List<synchrorecord> GetSynchroRecord()
        {
            var list = new List<synchrorecord>();
            try
            {
                list = DbMysql.Queryable<synchrorecord>().ToList();

                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        /// <summary>
        /// 调用华夏api添加/更新小区数据
        /// </summary>
        /// <param name="Community_id">小区id</param>
        /// <param name="synchroType">1:新增，2:更新</param>
        /// <returns></returns>
        public Response addCommunityData(string Community_id, int synchroType)
        {
            var res = new Response() { IsSuccess = true };

            if (string.IsNullOrEmpty(Community_id))
                return new Response() { IsSuccess = false };

            try
            {
                var url = "";
                if (synchroType == 1)
                    url = DqHxConfigSetting.HxCommunityAddAction;
                if (synchroType == 2)
                    url = DqHxConfigSetting.HxCommunityUpdateAction;
                if (string.IsNullOrEmpty(url))
                    return new Response() { IsSuccess = false };

                var list = DbMysql.Queryable<Sys_Community>().Where(c => c.Community_id == Community_id).ToList();
                var param = new CommunityParamDto();
                foreach (var item in list)
                {
                    param.id = item.Id;
                    param.pid = 0;
                    param.name = item.CommunityName;
                    param.layer = "";
                    param.lon = "";
                    param.lat = "";
                    param.address = item.Address;
                    param.country = "中国";
                    param.leader = item.PropertyUserName;
                    param.tel = item.PropertyPhone;
                    param.unknown = "";
                    param.name_pinyin = item.CommunitySabb;
                    param.name_first_pinyin = item.CommunitySabb;
                    param.externalid = "";
                    param.externalpid = "";
                    param.fromsystem = "二网平衡系统";
                    param.chargeablearea = item.BuiltArea;
                    param.peoplenumber = 0;
                }
                if (param.id < 1)
                    return new Response() { IsSuccess = false };

                var result = HttpHelper.HttpClientPost(url, param);
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Msg = e.Message;
            }
            return res;
        }

        /// <summary>
        /// 删除小区
        /// </summary>
        /// <param name="community_id">小区id</param>
        /// <returns></returns>
        public Response deleteCommunityData(string community_id)
        {
            var res = new Response() { IsSuccess = true };
            if (string.IsNullOrEmpty(community_id))
                return new Response() { IsSuccess = false };
            try
            {
                var data = DbMysql.Queryable<Sys_Community>().Where(a => a.Community_id == community_id).First();

                if (data.Id < 1)
                    return new Response() { IsSuccess = false };

                var url = DqHxConfigSetting.HxCommunityDeleteAction;
                var result = HttpHelper.HttpClientPost(url, data.Id);
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Msg = e.Message;
            }
            return res;
        }

        /// <summary>
        /// 调用华夏api添加/更新楼栋数据
        /// </summary>
        /// <param name="Building_id">楼栋id</param>
        /// <param name="synchroType">1:新增，2:更新</param>
        /// <returns></returns>
        public Response addBuildingData(string Building_id, int synchroType)
        {
            var res = new Response() { IsSuccess = true };

            if (string.IsNullOrEmpty(Building_id))
                return new Response() { IsSuccess = false };

            try
            {
                var url = "";
                if (synchroType == 1)
                    url = DqHxConfigSetting.HxBuildingAddAction;
                if (synchroType == 2)
                    url = DqHxConfigSetting.HxBuildingUpdateAction;
                if (string.IsNullOrEmpty(url))
                    return new Response() { IsSuccess = false };

                var list = DbMysql.Queryable<Sys_Building, Sys_Community>((b, c) => new object[] { JoinType.Left, b.Community_id == c.Community_id }).Where(c => c.Building_id == Building_id).Select((b, c) => new BuildingParamDto
                {
                    id = b.Id,
                    pid = c.Id,
                    name = b.BuildingName,
                    layer = "",
                    lon = "",
                    lat = "",
                    address = c.Address + b.BuildingName,
                    leader = c.PropertyUserName,
                    tel = c.PropertyPhone,
                    unknown = "",
                    name_pinyin = "",
                    name_first_pinyin = "",
                    externalid = "",
                    externalpid = "",
                    fromsystem = "二网平衡系统",
                    chargeablearea = b.BuiltArea,
                    peoplenumber = 0,
                    minfloor = 0,
                    maxfloor = 0,
                    lowfloor = "",
                    middlefloor = "",
                    highfloor = ""
                }).First();

                var result = HttpHelper.HttpClientPost(url, list);
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Msg = e.Message;
            }
            return res;
        }

        /// <summary>
        /// 删除楼栋
        /// </summary>
        /// <param name="building_id">楼栋id</param>
        /// <returns></returns>
        public Response deleteBuildingData(string building_id)
        {
            var res = new Response() { IsSuccess = true };
            if (string.IsNullOrEmpty(building_id))
                return new Response() { IsSuccess = false };
            try
            {
                var data = DbMysql.Queryable<Sys_Building>().Where(a => a.Building_id == building_id).First();

                if (data.Id < 1)
                    return new Response() { IsSuccess = false };

                var url = DqHxConfigSetting.HxBuildingDeleteAction;
                var result = HttpHelper.HttpClientPost(url, data.Id);
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Msg = e.Message;
            }
            return res;
        }


        /// <summary>
        /// 调用华夏api添加/更新房产数据
        /// </summary>
        /// <param name="residentuser_id">用户id</param>
        /// <param name="synchroType">1:新增，2:更新</param>
        /// <returns></returns>
        public Response addHouseData(string residentuser_id, int synchroType)
        {
            var res = new Response() { IsSuccess = true };

            if (string.IsNullOrEmpty(residentuser_id))
                return new Response() { IsSuccess = false };

            try
            {
                var url = "";
                if (synchroType == 1)
                    url = DqHxConfigSetting.HxHouseAddAction;
                if (synchroType == 2)
                    url = DqHxConfigSetting.HxHouseUpdateAction;
                if (string.IsNullOrEmpty(url))
                    return new Response() { IsSuccess = false };

                var _id = "000000000";

                var list = DbMysql.Queryable<Sys_ResidentUser, Sys_UnitNo, Sys_Building, Sys_Community,DicDetail,DicDetail>((r, u, b, c,d,d1) => new object[] {
                    JoinType.Left, r.UnitNo_id == u.UnitNo_id,
                    JoinType.Left,u.Building_id==b.Building_id,
                    JoinType.Left,b.Community_id==c.Community_id,
                    JoinType.Left,r.BuildType==d.Id,
                    JoinType.Left,r.HeatingType==d1.Id
                }).Where((r, u, b, c,d,d1) => r.ResidentUser_id == residentuser_id).Select((r, u, b, c,d,d1) => new
                {
                    id = r.Id.ToString() + DateTime.Now.ToString("yyyyMMdd") + _id.Substring(0, _id.Length - r.Id.ToString().Length) + r.Id.ToString(),
                    pid = b.Id,
                    usercode=r.UserCard,
                    annual="",
                    name=r.UserName,
                    cardno_old="",
                    wechatid="",
                    card_type="",
                    idcard="",
                    userkind="",
                    isunit=d.ItemName,
                    tel=r.UserPhone,
                    mobilephone="",
                    fixedtelephone="",
                    street="",
                    address=c.Address+b.BuildingName+u.UnitNoName+r.RoomNum,
                    layer="",
                    building=b.BuildingName,
                    unit=u.UnitNoName,
                    floor=r.LayerNum,
                    room_num=r.RoomNum,
                    building_area=r.BuiltArea,
                    charge_area=r.ChargeArea,
                    themo_id="",
                    heating_type=d1.ItemName,
                    lon="",
                    lat="",
                    taxpayername="",
                    taxpayerno="",
                    taxaddress="",
                    taxtel="",
                    bankname="",
                    bankaccount="",
                    mail="",
                    checkin="",
                    checkindate="",
                    joindate="",
                    joinstate="",
                    joinoperator="",
                    household="",
                    valve_type="",
                    valve_number="",
                    issign="",
                    create_time="",
                    create_people="",
                    update_time="",
                    update_people="",
                    unknown="",
                    bank_card="",
                    remark=r.Remarks,
                    name_first_pinyin="",
                    name_pinyin="",
                    system_only_id="",
                    system_id="",
                    system_only_pid=""
                }).First();

                var result = HttpHelper.HttpClientPost(url, list);
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Msg = e.Message;
            }
            return res;
        }

        /// <summary>
        /// 删除房产
        /// </summary>
        /// <param name="residentuser_id">用户id</param>
        /// <returns></returns>
        public Response deleteHouseData(string residentuser_id)
        {
            var res = new Response() { IsSuccess = true };
            if (string.IsNullOrEmpty(residentuser_id))
                return new Response() { IsSuccess = false };
            try
            {
                var data = DbMysql.Queryable<Sys_ResidentUser>().Where(a => a.ResidentUser_id == residentuser_id).First();

                if (data.Id < 1)
                    return new Response() { IsSuccess = false };

                var url = DqHxConfigSetting.HxHouseDeleteAction;
                var result = HttpHelper.HttpClientPost(url, data.Id);
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Msg = e.Message;
            }
            return res;
        }

    }
}
