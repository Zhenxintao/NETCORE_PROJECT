using CoreFtp;
using CoreFtp.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.ModelDto;

namespace THMS.Core.API.Service.UniformedServices.ComHttpRestSharp
{
    /// <summary>
    /// FTP传输类
    /// </summary>
    public class CoreFtpOpFiel
    {
        private FtpClient _ftpClient;
        /// <summary>
        /// 构造FtpClient实例
        /// </summary>
        public CoreFtpOpFiel()
        {
            //通过构造函数传入配置信息获得ftpClient
            FtpClient ftpClient = new FtpClient(new FtpClientConfiguration
            {
                Host = ConfigAppsetting.FTPHxInterFaceIp,
                Username = ConfigAppsetting.FTPHxUserName,
                Password = ConfigAppsetting.FTPHxPassWord,
                Port =Convert.ToInt32(ConfigAppsetting.FTPHxInterFaceProt),
                EncryptionType = FtpEncryption.Implicit,
                IgnoreCertificateErrors = true
            });
            _ftpClient = ftpClient;
        }
        /// <summary>
        /// 传送一个文件
        /// </summary>
        /// <param name="fileName">文件名称（例：heatStation_2020081722.txt）</param>
        /// <param name="sourcePath">文件路径（例：FileData\heatStation_2020081722.txt）</param>
        /// <returns></returns>
        public async Task<string> Upload(string fileName, string sourcePath)
        {
            try
            {
                //string fileName = @"heatStation_2020081722.txt";
                //string sourcePath = $@"FileData\heatStation_2020081722.txt";
                //获取本地文件信息
                var fileinfo = new FileInfo(sourcePath);
                await _ftpClient.LoginAsync();
                //判断连接状态
                if (!_ftpClient.IsConnected)
                {
                    return "could not connect ftp";
                }
                using (var writeStream = await
                //ftp服务器
                _ftpClient.OpenFileWriteStreamAsync(fileName))
                {
                    var fileReadStream = fileinfo.OpenRead();
                    //写入
                    await fileReadStream.CopyToAsync(writeStream);
                    return ResultMessageInfo.SuccessMessage;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                _ftpClient.Dispose();
            }
        }
        /// <summary>
        /// 获取文件夹下所有文件并发送
        /// </summary>
        /// <param name="folder">文件夹路径（例：FileData\）</param>
        /// <returns></returns>
        public async Task<string> UploadAllFile(string folder)
        {
            try
            {
                //string path = @"FileData\";
                DirectoryInfo di = new DirectoryInfo(folder);
                //找到该目录下的所有文件 
                FileInfo[] fis = di.GetFiles();
                string message = "";
                await _ftpClient.LoginAsync();
                //判断连接状态
                if (!_ftpClient.IsConnected)
                {
                    return "could not connect ftp";
                }
                foreach (var fi in fis)
                {
                    using (var writeStream = await
                                 //ftp服务器
                                 _ftpClient.OpenFileWriteStreamAsync(fi.Name))
                    {
                        var fileReadStream = fi.OpenRead();
                        //写入
                        await fileReadStream.CopyToAsync(writeStream);
                        message = "success";
                    }
                }
                return message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                _ftpClient.Dispose();
            }
        }
    }
}
