using Ciklum.Client.BusinessLogic.Commands;
using Ciklum.Types;
using Ciklum.Types.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ciklum.Client.BusinessLogic
{
    internal class CommandManager
    {
        private static object s_lock = new object();

        private static int numberOfThreads = 0;

        public static int NumberOfThreads
        {
            get
            {
                lock (s_lock)
                {
                    return numberOfThreads;
                }
            }
            private set
            {
                lock (s_lock)
                {
                     numberOfThreads = value;
                }
            }
        }

        public void AddFilesToProcessQueue(IEnumerable<string> files, string commandType, IDictionary<string,object> parameters)
        {
            foreach (string file in files)
            {
                Dictionary<string, object> data = new Dictionary<string, object>(parameters);

                data.Add("filePath", file);

                ICommand command = CommandBuilder.Build(commandType, data);

                CommandQueue.Queue.Enqueue(command);
            }

            EnforceCommandProcess();
        }

        public static void EnforceCommandProcess()
        {
            while (CommandQueue.Queue.Count > 0 && NumberOfThreads < ConfigurationManager.GetIntAppSettings("threadsNumber"))
            {
                ThreadPool.QueueUserWorkItem((data) => {
                    NumberOfThreads++;
                    ICommand commandToExecute = null;

                    if (CommandQueue.Queue.TryDequeue(out commandToExecute))
                    {
                        commandToExecute.Execute();
                    }

                    EnforceCommandProcess();
                    NumberOfThreads--;
                });
            }
        }
    }
}
