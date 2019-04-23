using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dm;

namespace chuanqijiaoben
{
    public class DM : Singleton<DM>, IDisposable
    {
        int WM_LBUTTONDOWN = 0x0201;
        int WM_LBUTTONUP = 0x0202;
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam,  int lParam);
        /// <summary>
        /// 此函数用以将屏幕坐标x及y组合成为向PostMessage函数lParam传递的实参
        /// </summary>
        /// <param name="lWord"></param>
        /// <param name="hWord"></param>
        /// <returns></returns>
        private int MakeLParam(int lWord, int hWord)
        {
            return ((hWord << 16) | (lWord & 0xffff));
        }
        private static string cmd;
        private bool disposed = false;
        private dmsoft dm;
        private int hwnd;
        public DM()
        {
            try
            {
                AutoRegCom(cmd);
                dm = new dmsoft();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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
        public void LeftClick(int x, int y)
        {
            PostMessage(hwnd, WM_LBUTTONDOWN, x, y);
            PostMessage(hwnd, WM_LBUTTONUP, x, y);
        }
        public void LeftDoubleClick(int x, int y)
        {
            LeftClick(x, y);
            Thread.Sleep(100);
            LeftClick(x, y);
        }
        public void BindWindow(int hwnd, string display, string mouse, string keypad, int mode)
        {
            dm.BindWindow(hwnd, display, mouse, keypad, mode);
            this.hwnd = hwnd;
        }
        #region 继承释放接口方法
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }
        public void Close()
        {
            Dispose();
        }
        ~DM()
        {
            //必须为false
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                //if (managedResource != null)
                //{
                //    managedResource.Dispose();
                //    managedResource = null;
                //}
            }
            //让类型知道自己已经被释放
            disposed = true;
        }
        #endregion
    }
}
