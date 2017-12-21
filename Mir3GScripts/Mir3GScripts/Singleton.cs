using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mir3GScripts
{
    public class Singleton<T> where T: class, new()
    {
        private static T instance;
        private static readonly object sysLock = new object();

        public static T GetInstance()
        {
            if (instance == null)
            {
                lock (sysLock)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}
