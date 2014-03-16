using Ciklum.Client.BusinessLogic.Commands;
using Ciklum.Types.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciklum.Client.BusinessLogic
{
    /// <summary>
    /// Singletone class for queue of commands
    /// </summary>
    internal class CommandQueue
    {
        private static object s_lock = new object();
        private static ConcurrentQueue<ICommand> queueCommands;

        private CommandQueue()
        {
        }

        public static ConcurrentQueue<ICommand> Queue
        {
            get
            {
                if (queueCommands == null)
                {
                    lock (s_lock)
                    {
                        if (queueCommands == null)
                        {
                            queueCommands = new ConcurrentQueue<ICommand>();
                        }
                    }
                }

                return queueCommands;
            }
        }
    }
}
