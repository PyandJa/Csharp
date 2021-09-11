using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FastDownload
{
    /// <summary>
    /// 系统设置类
    /// </summary>
   public class Set
    {
        public static string Start;       //是否开机自动启动
        public static string Auto;        //是否自动开始未完成任务
        public static string Path;        //默认下载路径
        public static string Net;         //网络限制下载速度
        public static string NetValue;    //网络限速值
        public static string DClose;      //是否下载完自动关机
        public static string TClose;      //是否定时关机
        public static string TCloseValue; //定时关机时间
        public static string SNotify;     //是否下载完成显示提示
        public static string Play;        //是否下载完成播放提示音
        public static string Continue;    //是否在有未完成的下载时显示继续
        public static string ShowFlow;    //是否显示流量控制
        public static string strNode = "SET";                                   //ini文件中要读取的节点
        public static string strPath = Application.StartupPath + "\\Set.ini";   //获取ini配置文件

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);//读取ini文件
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string mpAppName, string mpKeyName,string mpDefault,string mpFileName);//向ini写入数据

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool ExitWindowsEx(int uFlags, int dwReserved);//定时关机

        [DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool RtlAdjustPrivilege(int htok, bool disall, bool newst, ref int len);//关闭/重启系统（拥有所有权限）

        /// <summary>
        ///读取指定ini节点信息
        /// </summary>
        /// <param name="section">ini节点</param>
        /// <param name="key">节点下的项</param>
        /// <param name="def">没有找到内容时返回的默认值</param>
        /// <param name="filePaht">要读取的ini文件</param>
        /// <returns></returns>
        public static string GetIniFileString(string section,string key,string def,string filePath)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, temp, 1024, filePath);
            return temp.ToString();
        }
        /// <summary>
        /// 开机自动运行程序
        /// </summary>
        /// <param name="auto">是否自动运行</param>
        public void AutoRun(string auto)
        {
            string strName = Application.ExecutablePath;  //获取可执行文件路径
            if (!System.IO.File.Exists(strName))          //判断文件是否存在
            {
                return;
            }
            string strnewName = strName.Substring(strName.LastIndexOf("\\") + 1);//获取文件名
            RegistryKey RKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);//打开开机自动运行的注册表
            if (RKey==null)
            {
                RKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }
            if (auto=="0")
            {
                RKey.DeleteValue(strnewName, false);
            }
            else
            {
                RKey.SetValue(strnewName, strName);
            }
        }
        private const int EWX_SHUTDOWN = 0x00000001;     //关闭参数
        private const int SE_SHUTDOWN_PRIVILEGE = 0X13;  //关机特权
        /// <summary>
        /// 关闭计算机
        /// </summary>
        public void Shutdown()                           //关机
        {
            int i = 0;
            //提权，否则权限不足以关闭计算机
            RtlAdjustPrivilege(SE_SHUTDOWN_PRIVILEGE, true, false, ref i);   //获得关机特权
            ExitWindowsEx(EWX_SHUTDOWN, 0);                                  //关闭计算机
        }
        /// <summary>
        /// 获取ini配置文件中各字段的值
        /// </summary>
        public void GetConfig()
        {
            Start = GetIniFileString(strNode, "Start", "", strPath);         //是否开机自动启动
            Auto = GetIniFileString(strNode, "Auto", "", strPath);           //是否自动开始未完成任务
            Path = GetIniFileString(strNode, "Path", "", strPath);           //默认下载路径
            string netTemp = GetIniFileString(strNode, "Net", "", strPath);  //网络限制
            Net = netTemp.Split(' ')[0];                                     //是否继续网络限制
            NetValue = netTemp.Split(' ')[1];                                //网络限制的值
            DClose = GetIniFileString(strNode, "DClose", "", strPath);       //是否下载完成自动关机
            string closeTemp = GetIniFileString(strNode, "TClose", "", strPath);//定时关机
            TClose = closeTemp.Split(' ')[0];                                //是否定时关机
            TCloseValue = closeTemp.Split(' ')[1];                           //定时关机事件
            SNotify = GetIniFileString(strNode, "SNotify", "", strPath);     //是否下载完成提示
            Play = GetIniFileString(strNode, "Play", "", strPath);           //是否下载完成播放提示音
            Continue = GetIniFileString(strNode, "Continue", "", strPath);   //是否有未完成的下载时显示继续提示
            ShowFlow = GetIniFileString(strNode, "ShowFlow", "", strPath);   //是否显示流量监控
        }
        /// <summary>
        /// 获取指定驱动器的剩余空间，并转换为以GB为单位的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public string GetSpace(string path)
        {
            System.IO.DriveInfo[] drive = System.IO.DriveInfo.GetDrives();   //检索所有驱动器
            int i;
            for ( i = 0; i < drive.Length; i++)                              //遍历驱动器
            {
                if (path==drive[i].Name)                                     //判断遍历到的项是否与下拉列表的项相同
                {
                    break;                                                   //跳出循环
                }
            }
            return (drive[i].TotalFreeSpace / 1024 / 1024 / 1024.0).ToString("0.00") + "G";     //显示剩余空间GB格式
        }
    }
}
