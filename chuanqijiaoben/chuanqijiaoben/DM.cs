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
using dm;

namespace chuanqijiaoben
{
    public class DM : Singleton<DM>, IDisposable
    {
        int WM_LBUTTONDOWN = 0x0201;
        int WM_LBUTTONUP = 0x0202;
        [DllImport("User32.dll", EntryPoint = "SetCursorPos")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
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
        public string Ver()
        {
            return dm.Ver();
        }
        public int GetLastError()
        {
            return dm.GetLastError();
        }
        public int SetMouseDelay(string type, int delay)
        {
            return dm.SetMouseDelay(type, delay);
        }
        public int FindWindow(string windowClass, string title)
        {
            return dm.FindWindow(windowClass, title);
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
        public int RightClick()
        {
            return dm.RightClick();
        }
        public int MoveTo(int x, int y)
        {
            return dm.MoveTo(x, y);
        }
        public string Ocr(int x1, int y1, int x2, int y2, string color, double sim)
        {
            return dm.Ocr(x1, y1, x2, y2, color, sim);
        }
        public int FindColor(int x1, int y1, int x2, int y2, string color, double sim, int dir, out object x, out object y)
        {
            return dm.FindColor(x1, y1, x2, y2, color, sim, dir, out x, out y);
        }
        public string FindColorE(int x1, int y1, int x2, int y2, string color, double sim, int dir)
        {
            return dm.FindColorE(x1, y1, x2, y2, color, sim, dir);
        }
        public string FindStrFastE(int x1, int y1, int x2, int y2, string str, string color, double sim)
        {
            return dm.FindStrFastE(x1, y1, x2, y2, str, color, sim);
        }
        public int FindStr(int x1, int y1, int x2, int y2, string str, string color, double sim, out object x, out object y)
        {
            return dm.FindStr(x1, y1, x2, y2, str, color, sim, out x, out y);
        }
        public string FindMultiColorE(int x1, int y1, int x2, int y2, string first_color, string offset_color,double sim, int dir)
        {
            return dm.FindMultiColorE(x1, y1, x2, y2, first_color, offset_color, sim, dir);
        }
        public void BindWindow(int hwnd, string display, string mouse, string keypad, int mode)
        {
            dm.BindWindow(hwnd, display, mouse, keypad, mode);
            this.hwnd = hwnd;
        }
        public int CmpColor(int x, int y, string color, int sim)
        {
            return dm.CmpColor(x, y, color, sim);
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
