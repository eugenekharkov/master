using Ciklum.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ciklum.Client.BusinessLogic.Commands
{
    internal abstract class BaseCommand
    {
        protected string filePath;
        protected string urlAddress;

        public BaseCommand(string filePath, string urlAddress)
        {
            this.filePath = filePath;
            this.urlAddress = urlAddress;
        }

        protected abstract void ShowResult(TransferFileResult result);
    }
}
