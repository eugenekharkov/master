using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Ciklum.Types
{
    public class FileTransfer
    {
        private static object s_lock = new object();

        private static volatile ConcurrentQueue<byte[]> dataBuffer = new ConcurrentQueue<byte[]>();

        private static bool isJobReady;

        private static TransferFileResult outputMessage;

        public static bool IsJobReady
        {
            get
            {
                return isJobReady;
            }
            private set
            {
                lock (s_lock)
                {
                    isJobReady = value;
                }
            }
        }

        public static TransferFileResult Message
        {
            get
            {
                return outputMessage;
            }
            private set
            {
                if (outputMessage == null)
                {
                    lock (s_lock)
                    {
                        if (outputMessage == null)
                        {
                            outputMessage = value;
                        }
                    }
                }
            }
        }

        public string UrlAddress { get; private set; }

        public string FilePath { get; private set; }

        public FileTransfer(string urlAddress,string filePath)
        {
            UrlAddress = urlAddress;
            FilePath = filePath;
            IsJobReady = false;
        }

        public TransferFileResult EncodeFile()
        {
            // Sending file to the server for encrypting
            PostFileToServer("encrypt");

            while (!IsJobReady)
            {
                Thread.Sleep(1000);
            }

            // There was not error
            if (!Message.IsError)
            {
                // Let's do new job
                IsJobReady = false;

                // Checking when file is ready to be downloaded
                GetEncryptedFile();

                while (!IsJobReady)
                {
                    Thread.Sleep(1000);
                }
            }

            return Message;
        }

        private void PostFileToServer(string command)
        {
            EnforesFillingBuffer();
            EnforsSendingBuffer(command);
        }

        public TransferFileResult DecodeFile()
        {
            // Sending file to the server for encrypting
            PostFileToServer("decrypt");

            while (!IsJobReady)
            {
                Thread.Sleep(1000);
            }

            // There was not error
            if (!Message.IsError)
            {
                // Let's do new job
                IsJobReady = false;

                // Checking when file is ready to be downloaded
                GetEncryptedFile();

                while (!IsJobReady)
                {
                    Thread.Sleep(1000);
                }
            }

            return Message;
        }

        private void EnforesFillingBuffer()
        {
             ThreadPool.QueueUserWorkItem((data) => {
                 FillingBuffer(FilePath);
             });
        }

        private void EnforsSendingBuffer(string command)
        {
            ThreadPool.QueueUserWorkItem((data) =>
            {
                Thread.Sleep(1000);

                SendingFile(UrlAddress, Path.GetFileName(FilePath), command);
            });
        }

        private void FillingBuffer(string filePath)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(filePath);
                byte[] package = new byte[ConfigurationManager.GetIntAppSettings("bufferSize")];

                while (fs.Read(package, 0, package.Length) > 0)
                {
                    // End of the file where length of package is less then DEFAULT_PACKAGE_SIZE
                    if (fs.Position == fs.Length)
                    {
                        var lastPackageSize = fs.Length % ConfigurationManager.GetIntAppSettings("bufferSize");
                        package = package.Take(Convert.ToInt32(lastPackageSize)).ToArray();
                    }
                    
                    dataBuffer.Enqueue(package);

                    if (dataBuffer.Count > 100)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = new TransferFileResult(true, ex.Message);
                IsJobReady = true;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        private void SendingFile(string urlAddress, string fileName, string command)
        {
            Guid sessionId = Guid.NewGuid();
            string mimType = MimeMapping.GetMimeMapping(fileName);
            try
            {
                SendingBuffer(urlAddress, sessionId, mimType);

                SendingFileNameMarker(urlAddress, fileName, sessionId, command);

                Message = new TransferFileResult(false, string.Empty);

            }
            catch (Exception ex)
            {
                Message = new TransferFileResult(true, ex.Message);
            }
            finally
            {
                IsJobReady = true;
            }
        }

        private void SendingFileNameMarker(string urlAddress, string fileName, Guid sessionId, string command)
        {
            WebRequest request = WebRequest.Create(urlAddress);

            string requestParameters = @"sessionGuid=" + sessionId
                + "&fileName=" + fileName + "&command=" + command;

            var data = Encoding.UTF8.GetBytes(requestParameters);
            request.ContentLength = data.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();

            using (WebResponse response = request.GetResponse())
            {

            }
        }

        private void SendingBuffer(string urlAddress, Guid sessionId,string mimType)
        {
            while (dataBuffer.Count > 0)
            {
                byte[] package = null;

                if (!dataBuffer.TryDequeue(out package))
                {
                    continue;
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);

                string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                request.Method = "POST";
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                request.KeepAlive = true;


                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");


                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: " + mimType + "\r\n\r\n";

                string header = string.Format(headerTemplate, "file0", sessionId.ToString());
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
     

                request.ContentLength = boundarybytes.LongLength * 2 + headerbytes.LongLength + package.LongLength;
                
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(boundarybytes, 0, boundarybytes.Length);
                dataStream.Write(headerbytes, 0, headerbytes.Length);
                dataStream.Write(package, 0, package.Length);
                dataStream.Write(boundarybytes, 0, boundarybytes.Length);

                // old code
                //WebRequest request = WebRequest.Create(urlAddress);
                //string requestParameters = @"sessionGuid=" + sessionId
                //    + "&data=" + HttpUtility.UrlEncode(Convert.ToBase64String(package));

                //var data = Encoding.ASCII.GetBytes(requestParameters);
                //request.ContentLength = data.Length;
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.Method = "POST";
                //Stream dataStream = request.GetRequestStream();
                //dataStream.Write(data, 0, data.Length);
                //dataStream.Close();

                using (WebResponse response = request.GetResponse())
                {
                }

       



            }
        }

        private void GetEncryptedFile()
        {
            ThreadPool.QueueUserWorkItem((data) =>
            {
                int counter = 0;

                while (!DownloadFile(UrlAddress, FilePath))
                {
                    Thread.Sleep(1000);
                    counter++;
                    if (counter > ConfigurationManager.GetIntAppSettings("encryptTimeout"))
                    {
                        Message = new TransferFileResult(true, "Enceyption is too slow (time is out)");
                        break;
                    }
                }
                IsJobReady = true;
            });
        }

        private bool DownloadFile(string urlAddress, string fileName)
        {
            bool isDownloaded = false;

            try
            {
                
                string requestParameters = "fileName=" + WebUtility.UrlEncode(Path.GetFileName(fileName));

                WebRequest request = WebRequest.Create(urlAddress + "?" + requestParameters);

                request.ContentType = MimeMapping.GetMimeMapping(fileName);
                request.Method = "GET";

                using (WebResponse response = request.GetResponse())
                {
                    if (!string.IsNullOrWhiteSpace(response.ContentType))
                    {
                        WebClient client = new WebClient();
                        client.DownloadFile(urlAddress + "?" + requestParameters, fileName);
                        isDownloaded = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Message = new TransferFileResult(true, exception.Message);
                IsJobReady = true;
            }

            return isDownloaded;
        }

        private bool DownloadFile(string fileName , WebResponse response)
        {
            bool isDownloaded = false;

            using (Stream resStream = response.GetResponseStream())
            {
                using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[1024];

                    while (resStream.Read(buffer, 0, 1024) > 0)
                    {
                        //if (resStream.Position == resStream.Length)
                        //{
                        //    var lastPackageSize = resStream.Length % 1024;
                        //    buffer = buffer.Take(Convert.ToInt32(lastPackageSize)).ToArray();
                        //}
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Flush(true);
                    }
                    isDownloaded = true;
                }

            }
            return isDownloaded;
        }

    }
}
