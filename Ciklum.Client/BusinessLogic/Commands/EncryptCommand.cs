using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ciklum.Types;
using System.Windows.Forms;
using Ciklum.Types.Commands;

namespace Ciklum.Client.BusinessLogic.Commands
{
    internal class EncryptCommand: BaseCommand,  ICommand
    {
        private MainWindow windowsForm;

        public EncryptCommand(string filePath, string urlAddress, object data)
            : base(filePath, urlAddress)
        {
            windowsForm = (MainWindow) data;
        }

        /// <summary>
        /// Command execution
        /// </summary>
        public void Execute()
        {
            FileTransfer transfer = new FileTransfer(urlAddress, filePath);
            TransferFileResult result = transfer.EncodeFile();

            ShowResult(result);
        }

        /// <summary>
        /// This method runs in defferent thread from UI thread
        /// </summary>
        /// <param name="result">Result message</param>
        protected override void ShowResult(TransferFileResult result)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            if(result.IsError)
            { 
                data.Add("error", result.ErrorMessage);
            }
            else 
            { 
                data.Add("add", filePath);
            };

            windowsForm.RefreshList(data);
        }
    }
}
