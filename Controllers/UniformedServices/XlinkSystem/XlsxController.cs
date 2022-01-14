using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.UniformedServices.ComHttpRestSharp;

namespace THMS.Core.API.Controllers.UniformedServices.XlinkSystem
{
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "XlinkSystem")]
    [ApiController]
    [EnableCors("any")]
    public class XlsxController : ControllerBase
    {
        //用来获取路径相关
        private IHostingEnvironment _hostingEnvironment;

        public XlsxController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //导入功能
        [HttpPost]
        public void Import(IFormFile excelfile)
        {
            #region 废弃代码
            //string sWebRootFolder = _hostingEnvironment.WebRootPath;
            //string sFileName = $@"FileData\{Guid.NewGuid()}.xlsx";
            //FileInfo file = new FileInfo(sFileName);
            //try
            //{
            //    //把excelfile中的数据复制到file中
            //    using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
            //    {
            //        excelfile.CopyTo(fs);
            //        fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
            //    }

            //    using (ExcelPackage package = new ExcelPackage(file))
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
            //        int rowCount = worksheet.Dimension.Rows;
            //        int ColCount = worksheet.Dimension.Columns;

            //        for (int row = 1; row <= rowCount; row++)
            //        {
            //            for (int col = 1; col <= ColCount; col++)
            //            {
            //                //sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t"); //这种写法遇到为null的为报错
            //                sb.Append(worksheet.Cells[row, col].Value + "\t");
            //            }
            //            sb.Append(Environment.NewLine);
            //        }
            //        return Content(sb.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Content(ex.Message);
            //}
            #endregion
          new FileDataCreateCommon().CreateXlinkDb(excelfile);
        }
    }
}