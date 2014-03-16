using Ciklum.Client.BusinessLogic.Commands;
using Ciklum.Types.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciklum.Client.BusinessLogic
{
    internal static class CommandBuilder
    {
        public static ICommand Build(string commandType, IDictionary<string, object> parameters)
        {
            switch (commandType.ToUpperInvariant())
            {
                case "ENCRYPT":
                    return new EncryptCommand(parameters["filePath"].ToString(), parameters["urlAddress"].ToString(), parameters["UI"]);

                case "DECRYPT":
                    return new DecryptCommand(parameters["filePath"].ToString(), parameters["urlAddress"].ToString(), parameters["UI"]);

                default: 
                    return new EmptyCommand();
            }
        }
    }
}
