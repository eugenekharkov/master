using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciklum.Types
{
    public class TransferFileResult
    {
        public bool IsError { get; private set; }
        public string ErrorMessage { get; private set; }

        public TransferFileResult(bool isError, string errorMessage)
        {
            IsError = isError;
            ErrorMessage = errorMessage;
        }
    }
}
