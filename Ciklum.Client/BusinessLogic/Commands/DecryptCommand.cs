using Ciklum.Types;
using Ciklum.Types.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ciklum.Client.BusinessLogic.Commands
{
    internal class DecryptCommand : BaseCommand,  ICommand
    {
        private MainWindow windowsForm;

        public DecryptCommand(string filePath, string urlAddress, object data)
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
            TransferFileResult result = transfer.DecodeFile();

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
                 data.Add("remove", filePath);
             };

             windowsForm.RefreshList(data);
         }
            
    }
}
