using ApiModel;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.Logs;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.UniformedServices.ComHttpRestSharp
{
    /// <summary>
    /// 创建文件应用
    /// </summary>
    public class FileDataCreateCommon : DbContextSqlSugar
    {
        /// <summary>
        /// 创建文件并写入数据
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="content">写入内容</param>
        static public void CreateEnergyFile(string fileName, string content)
        {
            try
            {
                //var filePath = AppDomain.CurrentDomain.BaseDirectory + @"FileData\Demo.txt";
                var filePath = $@"FileData\{fileName}.txt";
                //如果文本文件fileName.txt已经存在，则将其删除
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
                {
                    byte[] data = Encoding.ASCII.GetBytes(content);//使用ASCII码将字符串转换为字节数据，所以一个字符占用一个字节
                    fileStream.Write(data, 0, data.Length);
                    Logger.Info($"{filePath}.txt文件创建成功;创建时间:{DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
                throw;
            }

        }

        public List<XlinkDbDto> CreateXlinkDbFilee(IFormFile excelfile)
        {
            string sFileName = $@"FileData\{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(sFileName);

            //把excelfile中的数据复制到file中
            using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
            {
                excelfile.CopyTo(fs);
                fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
            }
            var xlinkDbList = new List<XlinkDbDto>();

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                //获取表格的列数和行数
                int rowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;

                for (int row = 1; row <= rowCount; row++)
                {
                    XlinkDbDto xlinkDb = new XlinkDbDto();
                    xlinkDb.VpnUser_id = worksheet.Cells[row, 1].Value.ToString();
                    xlinkDb.AiValue = worksheet.Cells[row, 2].Value.ToString();
                    xlinkDb.NarrayNo = worksheet.Cells[row, 3].Value.ToString();
                    xlinkDb.AiDesc = worksheet.Cells[row, 4].Value.ToString();
                    xlinkDb.AiType = worksheet.Cells[row, 5].Value.ToString();
                    xlinkDb.TagName = worksheet.Cells[row, 6].Value.ToString();
                    xlinkDb.Unit = worksheet.Cells[row, 7].Value + "";
                    xlinkDb.PvssTagName = worksheet.Cells[row, 8].Value + "";
                    xlinkDbList.Add(xlinkDb);
                }
                return xlinkDbList;
            }

            //CreateXlinkDb(xlinkDbList);


        }

        public void CreateXlinkDb(IFormFile excelfile)
        {
            var resultList = CreateXlinkDbFilee(excelfile);
            if (resultList != null)
            {
                var xlinkDb = resultList.First();
                var xlinkDbList = Db.Queryable<ValueDesc>().Where(s => s.VpnUser_id.ToString() == xlinkDb.VpnUser_id && s.NarrayNo == 0 || s.NarrayNo == 1).ToList();
                var con = Db.Deleteable<ValueDesc>().Where(s => s.VpnUser_id.ToString() == xlinkDb.VpnUser_id).ExecuteCommand();
                if (XlinkDbType == "1")
                {
                    List<ValueDesc> valueDescs = new List<ValueDesc>();
                    foreach (var xlinkdb in resultList)
                    {
                        ValueDesc value = new ValueDesc();
                        value.VpnUser_id = Convert.ToInt32(xlinkdb.VpnUser_id);
                        value.AiValue = xlinkdb.AiValue;
                        value.NarrayNo = Convert.ToInt32(xlinkdb.NarrayNo);
                        value.AiDesc = xlinkdb.AiDesc;
                        value.AiType = xlinkdb.AiType;
                        value.TagName = xlinkdb.TagName;
                        value.Unit = xlinkdb.Unit;
                        value.PvssTagName = xlinkdb.PvssTagName;
                        var results = xlinkDbList.Where(s => s.NarrayNo.ToString() == xlinkdb.NarrayNo && s.TagName == xlinkdb.TagName).FirstOrDefault();
                        if (results == null)
                        {
                            value.HiHi = 0;
                            value.Hi = 0;
                            value.LoLo = 0;
                            value.Lo = 0;
                            value.RealValue = "";
                            value.ValueSeq = 1;
                        }
                        else
                        {
                            value.HiHi = results.HiHi;
                            value.Hi = results.Hi;
                            value.LoLo = results.LoLo;
                            value.Lo = results.Lo;
                            value.RealValue = results.RealValue;
                            value.ValueSeq = results.ValueSeq;
                        }
                        if (xlinkdb.Unit != "T" && xlinkdb.AiType == "AI")
                        {
                            value.IsImage = true;
                        }
                        else
                        {
                            value.IsImage = false;
                        }
                        valueDescs.Add(value);
                    }
                    var narrayList = resultList.Where(s => s.NarrayNo == "1").ToList();
                    foreach (var item in narrayList)
                    {
                        for (int i = 2; i <= Convert.ToInt32(XlinkDbNarrayNumber); i++)
                        {
                            ValueDesc value = new ValueDesc();
                            value.VpnUser_id = Convert.ToInt32(item.VpnUser_id);
                            value.AiValue = item.AiValue;
                            value.NarrayNo = i;
                            value.AiDesc = item.AiDesc;
                            value.AiType = item.AiType;
                            string yTagName = item.TagName;
                            value.TagName = item.TagName.Substring(0, item.TagName.Length - 1) + i;
                            value.Unit = item.Unit;
                            value.PvssTagName = item.PvssTagName;
                            var results = xlinkDbList.Where(s => s.NarrayNo == 1 && s.TagName == yTagName).FirstOrDefault();
                            if (results == null)
                            {
                                value.HiHi = 0;
                                value.Hi = 0;
                                value.LoLo = 0;
                                value.Lo = 0;
                                value.RealValue = "";
                                value.ValueSeq = 1;
                            }
                            else
                            {
                                value.HiHi = results.HiHi;
                                value.Hi = results.Hi;
                                value.LoLo = results.LoLo;
                                value.Lo = results.Lo;
                                value.RealValue = results.RealValue;
                                value.ValueSeq = results.ValueSeq;
                            }
                            if (item.Unit != "T" && item.AiType == "AI")
                            {
                                value.IsImage = true;
                            }
                            else
                            {
                                value.IsImage = false;
                            }
                            valueDescs.Add(value);
                        }
                    }

                    int conResult = Db.Insertable<ValueDesc>(valueDescs).ExecuteCommand();
                }
                else
                {
                    List<ValueDesc> valueDescs = new List<ValueDesc>();
                    foreach (var xlinkdb in resultList)
                    {
                        ValueDesc value = new ValueDesc();
                        value.VpnUser_id = Convert.ToInt32(xlinkdb.VpnUser_id);
                        value.AiValue = xlinkdb.AiValue;
                        value.NarrayNo = Convert.ToInt32(xlinkdb.NarrayNo);
                        value.AiDesc = xlinkdb.AiDesc;
                        value.AiType = xlinkdb.AiType;
                        value.TagName = xlinkdb.TagName;
                        value.Unit = xlinkdb.Unit;
                        value.PvssTagName = xlinkdb.PvssTagName;
                        var results = xlinkDbList.Where(s => s.NarrayNo.ToString() == xlinkdb.NarrayNo && s.TagName == xlinkdb.TagName).FirstOrDefault();
                        if (results == null)
                        {
                            value.HiHi = 0;
                            value.Hi = 0;
                            value.LoLo = 0;
                            value.Lo = 0;
                            value.RealValue = "";
                            value.ValueSeq = 1;
                        }
                        else
                        {
                            value.HiHi = results.HiHi;
                            value.Hi = results.Hi;
                            value.LoLo = results.LoLo;
                            value.Lo = results.Lo;
                            value.RealValue = results.RealValue;
                            value.ValueSeq = results.ValueSeq;
                        }
                        if (xlinkdb.Unit != "T" && xlinkdb.AiType == "AI")
                        {
                            value.IsImage = true;
                        }
                        else
                        {
                            value.IsImage = false;
                        }
                        valueDescs.Add(value);
                    }
                    int conResult = Db.Insertable<ValueDesc>(valueDescs).ExecuteCommand();
                }
            }

        }
    }
}
