using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace FastDownload
{
    public partial class Main_form : Form
    {
        public Main_form()
        {
            InitializeComponent();
            netList = new List<NetworkInterface>();
            foreach (NetworkInterface t in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (t.OperationalStatus.ToString() == "Up")
                {
                    if (t.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        netList.Add(t);
                    }
                }
            }
        }

        public List<DownLoad> dl = new List<DownLoad>();//下载队列的集合
        public List<xuchuan> jc = new List<xuchuan>();  //续传队列的集合
        public string filename = string.Empty;          //文件名称
        public string filepath = string.Empty;          //文件路径
        public string fileNameAndPath = string.Empty;   //文件的名称及路径
        public string downloadUrl = string.Empty;       //文件下载路径
        public int xiancheng;                           //文件下载所使用的线程数量
        private int RowProcess = -1;                    //下载或续传列表中选择行的索引
        private Point p1;                                       //记录鼠标坐标点变量

        private Set set = new Set();
        private List<NetworkInterface> netList;         //存储网卡列表
        private long receivedBytes;                     //记录上一次总接收字节数
        private long sentBytes;                         //记录上一次总发送字节数

        //按钮状态图片
        private Image imagenew1 = global::FastDownload.Properties.Resources.pbox_new;

        private Image imagenew2 = global::FastDownload.Properties.Resources.pbox_new2;
        private Image imagebegin1 = global::FastDownload.Properties.Resources.pbox_start;
        private Image imagebegin2 = global::FastDownload.Properties.Resources.pbox_start2;
        private Image imagepause1 = global::FastDownload.Properties.Resources.pbox_pause;
        private Image imagepause2 = global::FastDownload.Properties.Resources.pbox_pause2;
        private Image imagedel1 = global::FastDownload.Properties.Resources.pbox_delete;
        private Image imagedel2 = global::FastDownload.Properties.Resources.pbox_delete2;
        private Image imageopen1 = global::FastDownload.Properties.Resources.pbox_continue;
        private Image imageopen2 = global::FastDownload.Properties.Resources.pbox_continue2;

        private void Main_form_Load(object sender, EventArgs e)
        {
            set.GetConfig();                                       //获取配置信息
            Thread th = new Thread(new ThreadStart(BeginDisplay)); //该线程用于显示任务状态
            th.IsBackground = true;                                //后台线程
            th.Start();                                            //线程开始
            SetToolTip();                                          //设置提示组件
            InitialListViewMenu();                                 //初始化listview控件菜单
            Thread th2 = new Thread(new ThreadStart(DisplayListView));//重绘listview控件
            th2.IsBackground = true;                //设置为后台线程
            th2.Start();                            //开始执行线程
            if (Set.ShowFlow == "1")                  //是否显示流量监控
            {
                pictureBox1.Visible = pictureBox2.Visible = label1.Visible = label2.Visible = true;
            }
            if (Set.Auto == "1")                       //是否自动开始未完成任务
            {
                DirectoryInfo dir = new DirectoryInfo(Set.Path);   //指定路径
                if (dir.Exists)
                {
                    FileInfo[] files = dir.GetFiles();//获取所有文件列表
                    foreach (FileInfo file in files)
                    {
                        if (file.Extension == ".cfg")   //派生是否有未下载完的文件
                        {
                            Stream sm = file.Open(FileMode.Open, FileAccess.ReadWrite);//得到续传文件流对象
                            string s = file.Name;                 //得到续传文件的文件名
                            xuchuan jcc = new xuchuan();          //实例化处理续传文件下载类的实例
                            jcc.Begin(sm, s);                     //开始处理续传信息
                            jc.Add(jcc);                          //添加续传对象到续传处理队列中
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化listview控件菜单
        /// </summary>
        private void InitialListViewMenu()
        {
            //设置listview的鼠标右键设置
            MenuItem mi = new MenuItem("开始");      //定义菜单的开始项
            mi.Click += new EventHandler(mi_Click);  //开始项的事件
            MenuItem mi2 = new MenuItem("暂停");     //定义菜单的暂停项
            mi2.Click += new EventHandler(mi2_Click);//暂停项的事件
            MenuItem mi3 = new MenuItem("删除");     //定义菜单的删除项
            mi3.Click += new EventHandler(mi3_Click);//删除项的事件
            lv_state.ContextMenu = new ContextMenu(new MenuItem[] { mi, mi2, mi3 });//为listview控件添加菜单
        }

        /// <summary>
        /// 定时重绘listview控件
        /// </summary>
        private void DisplayListView()
        {
            while (true)
            {
                this.Invoke((MethodInvoker)delegate ()  //定义匿名方法
                {
                    if (lv_state.Items.Count < 28)        //lv_state发送改变，则执行下面的内容
                    {
                        for (int j = 0; j < 28 - lv_state.Items.Count; j++)
                        {
                            //初始化lv_state的状态
                            lv_state.Items.Add(new ListViewItem(new string[] { string.Empty,
                            string.Empty,string.Empty,string.Empty,string.Empty,
                            string.Empty,string.Empty}));
                        }
                    }
                    for (int i = 0; i < lv_state.Items.Count; i++)//循环遍历所有行
                    {
                        if (i % 2 == 0)
                        {
                            lv_state.Items[i].BackColor = Color.FromArgb(225, 238, 225);//背景为浅蓝色
                        }
                        else
                        {
                            lv_state.Items[i].BackColor = Color.White;//设置文件为白色
                        }
                    }
                });
                Thread.Sleep(1000);//线程挂起1秒
            }
        }

        /// <summary>
        /// 删除按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mi3_Click(object sender, EventArgs e)
        {
            delete();
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        private void delete()
        {
            if (RowProcess != -1)
            {
                if (lv_state.Items[RowProcess].Text != string.Empty)
                {
                    if (RowProcess + 1 > dl.Count)
                    {
                        jc[RowProcess - dl.Count > 0 ? RowProcess - dl.Count : 0].stop = false;//状态为暂停
                        jc[RowProcess - dl.Count > 0 ? RowProcess - dl.Count : 0].stop2 = true;//状态为删除
                    }
                    else
                    {
                        dl[RowProcess].stop = false;//状态为暂停
                        dl[RowProcess].stop2 = true;//状态为删除
                    }
                }
            }
        }

        /// <summary>
        /// listview右键菜单暂停事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mi2_Click(object sender, EventArgs e)
        {
            pause();
        }

        /// <summary>
        /// 点击暂停按钮时,暂停下载进程或续传任务
        /// </summary>
        private void pause()
        {
            if (RowProcess != -1)
            {
                if (lv_state.Items[RowProcess].Text != string.Empty)
                {
                    if (RowProcess + 1 > dl.Count)
                    {
                        //设置任务状态为暂停
                        jc[RowProcess - dl.Count > 0 ? RowProcess - dl.Count : 0].stop = true;
                    }
                    else
                    {
                        //设置任务状态为暂停
                        dl[RowProcess].stop = true;
                    }
                }
            }
        }

        /// <summary>
        /// listview右键菜单开始事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mi_Click(object sender, EventArgs e)
        {
            start();//调用start()方法开始下载或续传任务
        }

        /// <summary>
        /// 点击开始按钮时，终止暂停动作，开始下载
        /// </summary>
        private void start()
        {
            if (RowProcess != -1)//判断lv_state是否选中行
            {
                if (lv_state.Items[RowProcess].Text != string.Empty)//判断选中行是否有效
                {
                    if (RowProcess + 1 > dl.Count)
                    {
                        jc[RowProcess - dl.Count > 0 ? RowProcess - dl.Count : 0].stop = false;//设置任务状态为开始
                    }
                    else
                    {
                        dl[RowProcess].stop = false;//设置任务状态为开始
                    }
                }
            }
        }

        /// <summary>
        /// 设置提示组件
        /// </summary>
        private void SetToolTip()
        {
            ToolTip ttnew = new ToolTip();//创建ToolTip对象
            ttnew.InitialDelay = 10;//设置延迟为10毫秒
            ttnew.SetToolTip(pbox_new, "新建");//为控件添加提示信息
            ToolTip ttbegin = new ToolTip();//创建ToolTip对象
            ttbegin.InitialDelay = 10;//设置延迟为10毫秒
            ttbegin.SetToolTip(pbox_start, "开始");//为控件添加提示信息
            ToolTip ttpause = new ToolTip();//创建ToolTip对象
            ttpause.InitialDelay = 10;//设置延迟为10毫秒
            ttpause.SetToolTip(pbox_pause, "暂停");//为控件添加提示信息
            ToolTip ttdel = new ToolTip();//创建ToolTip对象
            ttdel.InitialDelay = 10;//设置延迟为10毫秒
            ttdel.SetToolTip(pbox_delete, "删除");//为控件添加提示信息
            ToolTip ttopen = new ToolTip();//创建ToolTip对象
            ttopen.InitialDelay = 10;//设置延迟为10毫秒
            ttopen.SetToolTip(pbox_continue, "续传");//为控件添加提示信息
            ToolTip ttset = new ToolTip();//创建ToolTip对象
            ttset.InitialDelay = 10;//设置延迟为10毫秒
            ttset.SetToolTip(pbox_set, "设置");//为控件添加提示信息
            ToolTip ttclose = new ToolTip();//创建ToolTip对象
            ttclose.InitialDelay = 10;//设置延迟为10毫秒
            ttclose.SetToolTip(pbox_close, "关闭");//为控件添加提示信息
        }

        /// <summary>
        /// 显示下载窗体或续传文件的状态
        /// </summary>
        private void BeginDisplay()
        {
            List<string[]> ls1 = new List<string[]>();//字符串集合1，用于对listview控件中的数据项进行对比
            List<string[]> ls2 = new List<string[]>();//字符串集合2，用于对listview控件中的数据项进行对比
            while (true)//使用whilea循环，重复监控下载或续传状态
            {
                //检测是否有异常
                try
                {
                    if (dl.Count > 0)//如果下载队列中有数据则向下执行
                    {
                        for (int j = 0; j < dl.Count + jc.Count; j++)//下载和续传队列数量的和
                        {
                            this.Invoke((MethodInvoker)delegate ()//在窗体主线程中listview控件中添加新的空数据项
                            {
                                if (lv_state.Items.Count < dl.Count + jc.Count)
                                {
                                    lv_state.Items.Add(new ListViewItem(new string[] { string.Empty,string.Empty,
                                        string.Empty,string.Empty,string.Empty,string.Empty,string.Empty}));
                                }
                            });
                        }
                        //遍历下载列表
                        for (int i = 0; i < dl.Count; i++)
                        {
                            if (dl[i].state == true)//检查下载列表中每一个下载进程的状态，如果为true继续执行
                            {
                                if (dl[i].complete)//如果下载列表中的下载线程的状态为：已完成
                                {
                                    if (Set.Play == "1")//自动播放声音
                                    {
                                        SoundPlayer player = new SoundPlayer("msg.wav");
                                        player.Play();
                                    }
                                    if (Set.SNotify == "1")//下载完成显示提示
                                    {
                                        MessageBox.Show("任务下载完成!");
                                    }
                                    dl.RemoveAt(i);//将已经完成的下载线程从队列中删除
                                    this.Invoke((MethodInvoker)delegate ()//将已完成的线程从listview中删除
                                    {
                                        lv_state.Items.RemoveAt(i);
                                    });
                                    ls1.Clear();//清空字符串集合1
                                    ls2.Clear();//清空字符串集合2
                                    break;//跳出循环
                                }
                                this.Invoke((MethodInvoker)delegate ()//进入主窗体线程,开始对listview1控件进行操作
                                {
                                    if (ls1.Count < dl.Count)//添加新的控数据项
                                    {
                                        ls1.Add(new string[] { string.Empty,
                                            string.Empty, string.Empty, string.Empty,
                                            string.Empty, string.Empty, string.Empty });
                                    }
                                    //得到新的下载状态信息
                                    ls1[i] = (dl[i].showmessage());

                                    if (ls2.Count < ls1.Count) //添加新的数据项
                                    {
                                        ls2.Add(new string[] { string.Empty,
                                            string.Empty, string.Empty, string.Empty,
                                            string.Empty, string.Empty, string.Empty });
                                    }
                                    for (int j = 0; j < 7; j++)
                                    {
                                        if (ls1[i][j] != ls2[i][j])
                                        {
                                            ls2[i][j] = ls1[i][j];
                                            ListViewItem lvi = lv_state.Items[i];
                                            lvi.SubItems[j] = new ListViewItem.ListViewSubItem(lvi, ls1[i][j]);
                                        }
                                    }
                                });
                            }
                            else
                            {
                                dl[i].state = true;//将下载进程的状态设置为true
                                dl[i].StartLoad();//执行下载进程中的开始下载方法
                            }
                        }
                    }
                    //续传
                    //如果续传队列中有数据，则向下执行
                    if (jc.Count > 0)
                    {
                        for (int j = 0; j < jc.Count + dl.Count; j++)//下载和续传队列数量的和
                        {
                            this.Invoke((MethodInvoker)delegate ()//在窗体主线程中listview控件添加新的空数据项
                            {
                                if (lv_state.Items.Count < jc.Count + dl.Count)
                                {
                                    lv_state.Items.Add(new ListViewItem(new string[] { string.Empty,string.Empty,
                                    string.Empty,string.Empty,string.Empty,string.Empty,string.Empty}));
                                }
                            });
                        }
                        //遍历续传队列
                        for (int i = 0; i < jc.Count; i++)
                        {
                            if (jc[i].state == true)//如果续传列表中的进程状态为true则向下执行
                            {
                                if (jc[i].complete)//如果续传列表中的续传进程的状态为：已完成
                                {
                                    if (Set.Play == "1")//自动播放声音
                                    {
                                        SoundPlayer player = new SoundPlayer("msg.wav");
                                        player.Play();
                                    }
                                    if (Set.SNotify == "1")//下载完成提示
                                    {
                                        MessageBox.Show("任务下载完成!");
                                    }
                                    jc.RemoveAt(i);//将已经完成续传的进程从队列中删除
                                    this.Invoke((MethodInvoker)delegate () //将已经完成的续传进程从listview控件中删除
                                    {
                                        lv_state.Items.RemoveAt(i);
                                    });
                                    //清空字符串集合1
                                    ls1.Clear();
                                    //清空字符串集合2
                                    ls2.Clear();
                                    //跳出循环
                                    break;
                                }
                                //进入窗体主线程，开始对listview控件进行操作
                                this.Invoke((MethodInvoker)delegate ()
                                {
                                    try
                                    {
                                        if (ls1.Count < jc.Count + dl.Count)
                                        {
                                            ls1.Add(new string[] { string.Empty,string.Empty,
                                        string.Empty,string.Empty,string.Empty,string.Empty,string.Empty});
                                        }
                                        //得到新的续传状态信息
                                        ls1[dl.Count + i] = (jc[i].showmessage());
                                        //添加新的空数据项
                                        if (ls2.Count < ls1.Count + dl.Count)
                                        {
                                            ls2.Add(new string[] { string.Empty,string.Empty,
                                        string.Empty,string.Empty,string.Empty,string.Empty,string.Empty});
                                        }
                                        //只更新新的数据项，不会造成listview控件的闪烁
                                        for (int j = 0; j < 7; j++)
                                        {
                                            if (ls1[i + dl.Count][j] != ls2[i + dl.Count][j])
                                            {
                                                ls2[i + dl.Count][j] = ls1[i + dl.Count][j];
                                                ListViewItem lvi = lv_state.Items[i + dl.Count];
                                                lvi.SubItems[j] = new ListViewItem.ListViewSubItem(lvi, ls1[i + dl.Count][j]);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //将异常写入日志
                                        writelog(ex.Message);
                                    }
                                });
                            }
                            else
                            {
                                jc[i].state = true;//将续传进程的状态设为true
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    //将出现的异常写入日志文件
                    if (ex.Message == "未能找到文件下载服务器或下载文件，请输入正确的下载地址！")
                    {
                        writelog(ex.Message);//写入日志
                        if (dl.Count > 0)
                        {
                            dl.RemoveAt(dl.Count - 1);
                        }
                        MessageBox.Show(ex.Message, "出错！");
                    }
                }
                catch (Exception ex2)
                {
                    writelog(ex2.Message);//写入日志
                    if (dl.Count > 0)
                    {
                        dl.RemoveAt(dl.Count - 1);
                    }
                    MessageBox.Show(ex2.Message, "出错！");
                }
                Thread.Sleep(1000);//每隔一秒重复检查一次
            }
        }

        /// <summary>
        /// 将异常信息写入日志文件，并标明异常出现的时间和日期
        /// </summary>
        /// <param name="s">异常信息</param>
        private void writelog(string s)
        {
            //创建文件流操作对象,将文件写入方式设置为追加
            StreamWriter fs = new StreamWriter(Application.StartupPath + "\\DownLoad.log", true);
            //向日志文件中写入出现的异常信息及出现异常的时间
            fs.Write(string.Format(s + DateTime.Now.ToString("yy-MM-dd  hh:mm:ss")));
            //将数据压入流
            fs.Flush();
            //关闭流对象
            fs.Close();
        }

        /// <summary>
        /// 退出应用程序并保存续传信息
        /// </summary>
        private void Exit()
        {
            if (Set.Continue == "1")//有续传文件时
            {
                if (dl.Count > 0 || jc.Count > 0)//如果下载或续传队列有任务则继续执行
                {
                    DialogResult dr = MessageBox.Show("当前有未完成的下载，请确认继续下载(是),还是关闭应用程序(否)!", "提示",
                        MessageBoxButtons.YesNo);//是否关闭应用程序
                    if (dr == DialogResult.Yes)//点击确定
                    {
                        if (dl.Count > 0)//如果下载队列中有下载进程
                        {
                            for (int i = 0; i < dl.Count; i++)//遍历下载队列中所有下载进程，并操作下载进程保存续传数据信息
                            {
                                dl[i].stop = true;  //暂停下载进程的下载动作
                                Thread.Sleep(3000); //线程挂起3秒
                                dl[i].SaveState();  //保存下载数据的续传信息
                                dl[i].AborThread(); //关闭下载进程
                            }
                        }
                        if (jc.Count > 0)//如果队列中有续传进程
                        {
                            //遍历续传队列中的所有续传进程，并操作续传进程保存续传数据信息
                            for (int j = 0; j < jc.Count; j++)
                            {
                                jc[j].stop = true;  //暂停续传进程的下载动作
                                Thread.Sleep(3000); //挂起3秒
                                jc[j].SaveState();  //保存续传数据的续传信息
                                jc[j].AbortThread();//关闭续传进程
                            }
                        }
                        Environment.Exit(0);//强制退出应用程序
                    }
                }
                else
                {
                    Close();//退出应用程序
                }
            }
            else
            {
                Close();//退出应用程序
            }
        }

        /// <summary>
        /// 显示网络流量
        /// </summary>
        private void ShowSpeed()
        {
            long totalReceivedbytes = 0;               //记录本次总接收字节数
            long totalSentbytes = 0;                   //记录本次总发送字节数
            foreach (NetworkInterface net in netList)  //遍历网卡列表
            {
                IPv4InterfaceStatistics interfaceStats = net.GetIPv4Statistics(); //获取ipv4的统计信息
                totalReceivedbytes += interfaceStats.BytesReceived;               //获取接收的字节数，并累计
                totalSentbytes += interfaceStats.BytesSent;                       //获取发送的字节数，并累计
            }
            long recivedSpeed = totalReceivedbytes - receivedBytes;  //获取本次接收的字节数(本次-上次)
            long sentSpeed = totalSentbytes - sentBytes;             //计算本次发送字节数(本次-上次)
            if (receivedBytes == 0 && sentBytes == 0)                      //如果上一次的接收和发送都为0，则将上传和下载速度设为0
            {
                recivedSpeed = 0;
                sentSpeed = 0;
            }
            label1.Text = "[" + recivedSpeed / 1024 + "KB/s]"; //下载速度
            label2.Text = "[" + sentSpeed / 1024 + "KB/s]";    //上传速度
            receivedBytes = totalReceivedbytes;                //记录上一次总接收字节数
            sentBytes = totalSentbytes;                        //记录上一次总发送字节数
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            set.GetConfig();//获取配置文件的值
            if (Set.TClose == "1")//定时关机
            {
                string nowTime = DateTime.Now.ToLongTimeString();//获取当前时间
                if (Set.TCloseValue.Equals(nowTime))
                {
                    set.Shutdown();//关闭计算机
                }
            }
            if (Set.ShowFlow == "1")//显示网络流量
            {
                pictureBox1.Visible = pictureBox2.Visible = label1.Visible = label2.Visible = true;
                ShowSpeed();
            }
            else
            {
                pictureBox1.Visible = pictureBox2.Visible = label1.Visible = label2.Visible = false;
            }
        }

        /// <summary>
        /// 拖动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_form_MouseDown(object sender, MouseEventArgs e)
        {
            p1 = new Point(-e.X, -e.Y);//当鼠标点击左键时，开始记录鼠标位置
        }

        private void Main_form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)      //当鼠标在窗体中移动，并按下鼠标左键时
            {
                Point p2 = Control.MousePosition; //得到鼠标在当前操作系统工作区域的坐标
                p2.Offset(p1);                    //得到现在窗体所在的坐标
                DesktopLocation = p2;             //设置当前窗体所在的坐标
            }
        }

        /// <summary>
        /// 添加任务按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbox_new_Click(object sender, EventArgs e)
        {
            LoadStart ls = new LoadStart();
            ls.Owner = this;
            ls.Show();//显示下载页面
        }

        /// <summary>
        /// 开始按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbox_start_Click(object sender, EventArgs e)
        {
            start();//开始下载或续传
            if (Set.DClose == "1")
            {
                set.Shutdown();//下载完成自动关闭计算机
            }
        }

        /// <summary>
        /// 暂停按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbox_pause_Click(object sender, EventArgs e)
        {
            pause();//调用暂停方法，暂停下载或续传任务
        }

        /// <summary>
        /// 删除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbox_delete_Click(object sender, EventArgs e)
        {
            delete();//调用方法，删除下载或续传任务
        }

        /// <summary>
        /// 续传按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbox_continue_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;                 //重置续传文件的名称
            openFileDialog1.Filter = string.Format("cfg文件|*.cfg"); //续传文件类型筛选
            DialogResult dr = openFileDialog1.ShowDialog();          //打开文件浏览，选择续传文件
            if (dr == DialogResult.OK)                   //判断是否按下确定按钮
            {
                Stream sm = openFileDialog1.OpenFile();//获取续传文件流对象
                string s = openFileDialog1.FileName;   //获取续传文件的文件名
                xuchuan jcc = new xuchuan();           //实例化续传类
                jcc.Begin(sm, s);                      //处理续传信息
                jc.Add(jcc);                           //将续传对象添加到续传处理队列
            }
        }

        /// <summary>
        /// 系统设置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbox_set_Click(object sender, EventArgs e)
        {
            Setting set = new Setting();
            set.ShowDialog();
        }

        /// <summary>
        /// 退出程序事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbox_close_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//最小化窗体
            this.ShowInTaskbar = false;
        }

        /// <summary>
        /// 系统托盘还原事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)//先判断窗体是否为最小化
            {
                this.Show();//显示
                this.WindowState = FormWindowState.Normal;//还原窗体
            }
        }

        /// <summary>
        /// 右键退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();//调用方法，实现续传信息的保存
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void lv_state_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RowProcess = lv_state.SelectedIndices.Count;
        }
    }
}