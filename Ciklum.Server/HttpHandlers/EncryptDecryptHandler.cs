using Ciklum.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ciklum.Server.HttpHandlers
{
    public class EncryptDecryptHandler: IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            DirectoriesInitialization(context);

            if (context.Request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                FileUploading(context);
            }
            else
            {
                FileDownloading(context);
            }
        }

        private void FileDownloading(HttpContext context)
        {
            string fileName = context.Request.QueryString["fileName"];
            string publicPath = context.Server.MapPath(ConfigurationManager.GetAppSettings("publicFolder"));

            if (File.Exists(publicPath + "\\" + fileName))
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = MimeMapping.GetMimeMapping(fileName);
                //var data = File.ReadAllBytes(publicPath + "\\" + fileName);
                //context.Response.OutputStream.Write(data, 0, data.Length);
                //context.Response.OutputStream.Flush();
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
                context.Response.TransmitFile(publicPath + "\\" + fileName);
                context.Response.Flush();
            }
            else
            {
                context.Response.StatusCode = 200;
                context.Response.StatusDescription = "File Not Found";
            }
        }

        private void FileUploading(HttpContext context)
        {
            string sessionGuid = context.Request.Params["sessionGuid"];
            string fileName = context.Request.Form["fileName"];

            if (context.Request.Files.Count > 0)
            {
                HttpPostedFile file = context.Request.Files[0];
                byte[] buffer = new byte[file.InputStream.Length];

                file.InputStream.Read(buffer, 0, buffer.Length);

                if (buffer != null)
                {
                    SaveFile(Path.GetFileName(file.FileName), buffer);
                }
            }

            if (!(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(sessionGuid)))
            {
                string oldPath = context.Server.MapPath(ConfigurationManager.GetAppSettings("workFolder")) + "\\" + sessionGuid;
                string newPath = context.Server.MapPath(ConfigurationManager.GetAppSettings("workFolder")) + "\\" + fileName;

                if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                }

                if (File.Exists(oldPath))
                {
                    File.Move(oldPath, newPath);
                }

                ExecuteEncryptionDecryption(context, fileName, newPath);

            }
        }

        private void ExecuteEncryptionDecryption(HttpContext context, string fileName, string newPath)
        {
            string command = context.Request.Form["command"] ?? string.Empty;

            string publicPath = context.Server.MapPath(ConfigurationManager.GetAppSettings("publicFolder")) + "\\" + fileName;
            byte[] encodedData = null;

            if (string.Equals(command, "encrypt", StringComparison.InvariantCultureIgnoreCase))
            {
                encodedData = Encrypter.Encrypt(File.ReadAllBytes(newPath),
                    ConfigurationManager.GetAppSettings("DesPrivateKey")
                    );
            }
            else
                if (string.Equals(command, "decrypt", StringComparison.InvariantCultureIgnoreCase))
                {
                    encodedData = Encrypter.Decrypt(File.ReadAllBytes(newPath),
                                ConfigurationManager.GetAppSettings("DesPrivateKey")
                                );
                }

            File.WriteAllBytes(newPath, encodedData);

            if (File.Exists(publicPath))
            {
                File.Delete(publicPath);
            }

            if (File.Exists(newPath))
            {
                File.Move(newPath, publicPath);
            }
        }

        private void SaveFile(string fileName, byte[] buffer)
        {
            string path = HttpContext.Current.Server.MapPath(ConfigurationManager.GetAppSettings("workFolder")) + "\\" + fileName;
            FileStream writer = new FileStream(path, File.Exists(path) ? FileMode.Append : FileMode.Create, FileAccess.Write);

            writer.Write(buffer, 0, buffer.Length);
            writer.Close();
        }

        private void DirectoriesInitialization(HttpContext context)
        {
            string publicPath = context.Server.MapPath(ConfigurationManager.GetAppSettings("publicFolder"));
            string workPath = context.Server.MapPath(ConfigurationManager.GetAppSettings("workFolder"));

            if (!Directory.Exists(publicPath))
            {
                Directory.CreateDirectory(publicPath);
            }

            if (!Directory.Exists(workPath))
            {
                Directory.CreateDirectory(workPath);
            }
        }
    }
}