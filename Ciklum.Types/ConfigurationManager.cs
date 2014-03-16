using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciklum.Types
{
    public static class ConfigurationManager
    {

        public static string GetAppSettings(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        public static int GetIntAppSettings(string key)
        {
   
                int number = DefaultIntValues(key);

                int.TryParse(System.Configuration.ConfigurationManager.AppSettings[key], out number);

                return number;
            
        }

        private static int DefaultIntValues(string key)
        {
            if (key.Equals("threadsNumber", StringComparison.InvariantCultureIgnoreCase))
            {
                return Constants.DEFAULT_THREADS_NUMBER;
            }

            if (key.Equals("bufferSize", StringComparison.InvariantCultureIgnoreCase))
            {
                return Constants.DEFAULT_PACKAGE_SIZE;
            }

            if (key.Equals("encryptTimeout", StringComparison.InvariantCultureIgnoreCase))
            {
                return Constants.DEFAULT_ENCRYPT_TIMEOUT;
            }

            return 0;
        }
    }
}
