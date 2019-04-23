using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dm;

namespace chuanqijiaoben
{
    public class DM
    {
        private static string cmd;
        private static DM instance;
        private dmsoft controller;
        private static readonly object sysLock = new object();

        public dmsoft Controller
        {
            get
            {
                return controller;
            }
        }
        private DM()
        {
            try
            {
                AutoRegCom(cmd);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static DM GetInstance()
        {
            if (instance == null)
            {
                lock (sysLock)
                {
                    if (instance == null)
                    {
                        instance = new DM();
                    }
                }
            }
            return instance;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private void AutoRegCom(string cmd)
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.RedirectStandardOutput = true;
                process.StartInfo = processStartInfo;
                processStartInfo.Arguments = "/c " + cmd;
                process.Start();
                StreamReader streamReader = process.StandardOutput;
                streamReader.ReadToEnd();
                process.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
