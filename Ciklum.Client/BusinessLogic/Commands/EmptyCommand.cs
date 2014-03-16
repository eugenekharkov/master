using Ciklum.Types.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciklum.Client.BusinessLogic.Commands
{
    internal class EmptyCommand: ICommand
    {
        public EmptyCommand()
        {
        }

        public void Execute()
        {

        }
    }
}
