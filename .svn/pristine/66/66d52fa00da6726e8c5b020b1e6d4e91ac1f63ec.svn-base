
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using FastDownload;
using Locations;

namespace FastDownload
{    
    /// <summary>
    /// 文件下载类
    /// </summary>
   public class DownLoad
    {
        public DateTime dtbegin;              //文件开始下载的时间
        public bool complete = false;         //文件下载是否完成
        public long filesize;                 //下载文件的大小
        public bool state = false;            //下载文件的状态,是否为已执行
        public string downloadUrl;            //下载地址
        public bool stop = false;             //暂停下载开关
        public string filename;               //下载文件的名称
        public string fileNameAndPath;        //下载文件的路径及文件名
        public string filepath;               //下载文件的路径
        public List<bool> lbo = new List<bool>();                                //判断所有线程是否全部下载完成
        public int xiancheng;                                                    //下载文件所使用的线程数量
        public List<Locations.Locations> lli = new List<Locations.Locations>();  //记录续传信息的可序列化类的集合
        public bool stop2 = false;                                               //停止下载，并删除下载的文件
        public List<Thread> G_thread_Collection = new List<Thread>();            //存放线程的集合，用于停止下载任务
        private AutoResetEvent are = new AutoResetEvent(true);                   //线程事件
        private bool b_thread = false;                                           //是否支持多线程下载

        /// <summary>
        /// DownLoad类的构造方法
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="downloadurl">下载资源地址</param>
        /// <param name="FileNameAndPath">文件完整路径</param>
        /// <param name="xiancheng">使用线程数量</param>
        public DownLoad(string filename,string filepath,string downloadurl,string FileNameAndPath,int xiancheng)
        {
            //给类的公有成员赋值
            this.filename = filename;
            this.filepath = filepath;
            this.downloadUrl = downloadurl;
            this.fileNameAndPath = FileNameAndPath;
            this.xiancheng = xiancheng;
            dtbegin = DateTime.Now;  //系统时间
        }
        /// <summary>
        /// 开始下载网络资源
        /// </summary>
        public void StartLoad()
        {
            long filelong = 0;
            try
            {
                //创建HttpWebRequest对象
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(downloadUrl);
                //根据HttpWebRequest对象得到HttpWebResponse对象
                HttpWebResponse hwp = (HttpWebResponse)hwr.GetResponse();
                //得到要下载的文件的长度
                filelong = hwp.ContentLength;
                b_thread = GetBool(downloadUrl);
            }
            catch (WebException we)
            {
                //向上一层抛出异常
                throw new WebException("未能找到服务器或下载文件，请检查下载地址是否正确!");
            }
            catch (Exception ex)
            {
                //抛出应用程序异常
                throw new Exception(ex.Message);
            }
            filesize = filelong;                      //得到文件长度值
            int meitiao = (int)filelong / xiancheng;  //开始计算每条线程要下载多少字节
            int yitiao=(int)filelong % xiancheng;     //开始计算每条线程分配自字节后余出的字节
            Locations.Locations ll = new Locations.Locations(0, 0);//新建一个续传信息对象
            lbo = new List<bool>();                                //初始化布尔集合
            for (int i = 0; i < xiancheng; i++)                    //开始为每条线程分配下载区间
            {
                ll.Start = i != 0 ? ll.End + 1 : ll.End;           //分配下载区间
                ll.End = i == xiancheng - 1 ? ll.End + meitiao + yitiao : ll.End + meitiao;//分配下载区间
                Thread th = new Thread(GetData);                   //为每一条线程分配下载区间
                th.Name = i.ToString();                            //线程名称为下载区间排序的索引
                th.IsBackground = true;                            //设置线程为后台线程
                th.Start(ll);                                      //线程开始，并为线程所执行的方法传递参数,参数为当前线程下载的区间
                lli.Add(new Locations.Locations(ll.Start, ll.End, downloadUrl,//续传状态列表添加新的续传空间
                    filename, filesize, new Locations.Locations(ll.Start,ll.End)));
                ll = new Locations.Locations(ll.Start, ll.End);                //得到新的区间对象
                G_thread_Collection.Add(th);                                   //将线程添加到线程集合中
                lbo.Add(false);                                                //设置每条线程的完成状态为false
            }
            hebinfile();//合并文件线程开始启动
        }
        /// <summary>
        /// 下载网络资源方法
        /// </summary>
        /// <param name="l">下载资源区间</param>
        public void GetData(object l)
        {
            Locations.Locations ll = (Locations.Locations)l;   //得到续传信息对象(文件下载或续传的开始点和结束点)
            if (!b_thread)                                      //判断是否支持多线程下载
            {
                are.WaitOne();
            }
            else
            {
                are.Set();
            }
            HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(downloadUrl);//根据下载地址创建HttpWebRequest对象
            hwr.Timeout = 15000;                                                    //设置下载请求超时为200s
            hwr.AddRange(ll.Start, ll.End);                                         //设置当前线程续传的开始点与结束点
            HttpWebResponse hwp = (HttpWebResponse)hwr.GetResponse();               //得到HttpWebResponse对象
            Stream ss = hwp.GetResponseStream();                                    //根据HttpWebResponse对象的GetResponseStream方法得到用于下载数据的网络对象
            new Set().GetConfig();                                       //获取文件下载的缓冲区
            byte[] buffer = new byte[Convert.ToInt32(Set.NetValue) * 8]; //获取网络速值:byte*8=1B
            FileStream fs = new FileStream(string.Format(filepath + @"\" + filename + Thread.CurrentThread.Name), FileMode.Create);//新建文件流对象，用于存放当前每个线程下载的文件
            try
            {
                int i;                                                   //用于记录每次下载的有效字节数
                int nns = Convert.ToInt32(Thread.CurrentThread.Name);    //当前线程的索引
                while ((i=ss.Read(buffer,0,buffer.Length))>0)            //开始将下载的数据放入缓冲中
                {
                    fs.Write(buffer, 0, i);                              //将缓冲数据写入到本地文件
                    lli[nns].Start += i;                                 //计算现在的下载位置，用于续传
                    while (stop)                                         //用户点击暂定按钮后，线程暂时挂起
                    {
                        Thread.Sleep(100);                               //设置挂起线程时间为100ms
                    }
                    if (stop2)                                           //点击删除按钮后，使下载过程强行停止
                    {
                        break;                                           //跳出循环，停止下载。
                    }
                    Thread.Sleep(10);
                }
                fs.Close();//关闭文件流对象
                ss.Close();//关闭网络流对象
                lbo[Convert.ToInt32(Thread.CurrentThread.Name)] = true;  //状态为已完成
            }
            catch (Exception ex)
            {

                writelog(ex.Message);                                    //将异常写入日志
                SaveState();                                             //保存断点续传状态
                
            }
            finally
            {
                fs.Close();//关闭文件流对象
                ss.Close();//关闭网络流对象
                if (!b_thread) are.Set(); else are.Set();                 //将线程状态设置为终止
            }
        }
        /// <summary>
        /// 保存断点续传状态
        /// </summary>
        public void SaveState()
        {
            BinaryFormatter bf = new BinaryFormatter();     //实例化二进制格式对象
            MemoryStream ms = new MemoryStream();           //实例化内存流对象
            bf.Serialize(ms, lli);                          //将续传信息序列化到内存流中
            ms.Seek(0, SeekOrigin.Begin);                   //将内存流中的指针位置置为零
            byte[] bt = ms.GetBuffer();                     //从内存流中得到字节数组
            FileStream fs = new FileStream(fileNameAndPath +
                ".cfg", FileMode.Create);                   //创建文件流对象
            fs.Write(bt, 0, bt.Length);                     //向文件流写入数据（字节数组）
            fs.Close();                                     //关闭流对象
        }
        /// <summary>
        /// 监控文件是否下载完成
        /// </summary>
        public void hebinfile()
        {
            //在新线程中执行
            Thread th2 = new Thread(
            delegate() //使用匿名方法
            {
                while (true)//每隔1s，检查一次是否所有线程都完成了任务
                {
                    if (!lbo.Contains(false))//如果所有线程都完成了下载任务
                    {
                        GetFile(); //开始文件合并
                        break;     //跳出循环，停止检查
                    }
                    else
                    {
                        if (this.stop2)   //停止下载
                        {
                            DeleteFile(); //删除文件
                        }
                    }
                    Thread.Sleep(1000);   //线程挂起1秒
                }
             });
            th2.IsBackground = true;      //设置为后台线程
            th2.Start();                  //线程开始
        }
        /// <summary>
        /// 把异常写入日志
        /// </summary>
        /// <param name="s">异常信息</param>
        private void writelog(string s)
        {
            try
            {
                StreamWriter fs = new StreamWriter(@"c:\DownLoda.log", true);                //新建文件流对象
                fs.Write(string.Format(s + DateTime.Now.ToString("yy-MM-dd hh:mm:ss")));     //将异常信息及时间写入
                fs.Flush();      //清空缓存区,使缓存区内数据压入流
                fs.Close();      //关闭文件流对象
            }
            catch { }
            
        }
        /// <summary>
        /// 文件合并
        /// </summary>
        private void GetFile()
        {
            new Set().GetConfig();
            if (Set.Path.EndsWith("\\"))
            {
                fileNameAndPath = Set.Path + filename;
            }
            else
            {
                fileNameAndPath = Set.Path + "\\" + filename;
            }
            if (stop2)//如果此文件是点击删除按钮后跳转到当前方法的，那么就直接删除文件
            {
                DeleteFile();//删除文件
            }
            else
            {
                FileStream fs = new FileStream(fileNameAndPath, FileMode.Create);//新建文件流对象，用于生成下载后得到的文件
                byte[] buffer = new byte[2000];//创建缓冲区对象

                for (int i = 0; i < xiancheng; i++)//开始遍历每条线程下载后得到的文件,并从每个文件中读取内容，放入一个文件夹中
                {
                    //新建文件流对象,此流对象用于引用每个线程下载的文件
                    //并将所有文件按照顺序放入一个文件中去,此文件就是多线程下载的文件
                    FileStream fs2 = new FileStream(string.Format(filepath + @"\" + filename + i.ToString()), FileMode.Open);
                    int i2;//记数器
                    while ((i2=fs2.Read(buffer,0,buffer.Length))>0)
                    {
                        fs.Write(buffer, 0, i2);//读取文件中所有数据
                    }
                    fs2.Close();//关闭流对象          
                }
                fs.Close();  //关闭流对象
                DeleteFile();//调用删除方法
            }
        }
        /// <summary>
        /// 删除文件的方法
        /// </summary>
        private void DeleteFile()
        {
            foreach (var item in G_thread_Collection) //关闭所有下载或续传线程
            {
                if (item.Name!=Thread.CurrentThread.Name)
                {
                    item.Abort();                     //终止线程
                }
            }
            if (stop2)                               //删除所有生成的文件
            {
                for (int i = 0; i < xiancheng; i++)
                {
                    File.Delete(string.Format(filepath + @"\" + filename + i.ToString()));
                }
                if (File.Exists(fileNameAndPath+".cfg"))//判断指定的文件是否存在
                {
                    string ssname = string.Format(fileNameAndPath + ".cfg");
                    File.Delete(ssname);             //存在则删除
                }
                clear();                             //重置字段信息
            }
            else
            {
                for (int i = 0; i < xiancheng; i++)
                {
                    File.Delete(string.Format(filepath + @"\" + filename + i.ToString()));
                }
                if (File.Exists(fileNameAndPath+".cfg"))
                {
                    string ssname = string.Format(fileNameAndPath + ".cfg");
                    File.Delete(ssname);
                }
                clear();                            //重置所有字段信息
            }

        }
        /// <summary>
        /// 返回资源下载状态信息
        /// </summary>
        /// <returns></returns>
        public string[] showmessage()
        {
            TimeSpan dt2 = DateTime.Now - dtbegin;
            return new string[] {filename,((filesize/1024)/1024).ToString()+"MB",
                (Process()).ToString()+"%",
                (Procese2()/1024).ToString()+"KB"+@"/"+(filesize/1024).ToString()+"KB",
                string.Format("{0}小时{1}分{2}秒",dt2.Hours.ToString(),dt2.Minutes.ToString(),dt2.Seconds.ToString()),
                filename.Substring(filename.LastIndexOf("."),4),
                dtbegin.ToString("yy-MM-dd hh:mm:ss")
            };
        }
        /// <summary>
        /// 返回资源下载的百分比
        /// </summary>
        /// <returns></returns>
        private int Process()
        {
            try
            {
                long ll = 0;
                for (int i = 0; i < lli.Count; i++)
                {
                    ll += lli[i].Start - lli[i].Ls.Start;
                }
                int il = (int)(ll * 100 / filesize);
                return il;
            }
            catch (Exception ex)
            {

                writelog(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 返回文件已经下载的长度
        /// </summary>
        /// <returns></returns>
        private int Procese2()
        {
            long ll = 0;
            for (int i = 0; i < lli.Count; i++)
            {
                ll += lli[i].Start - lli[i].Ls.Start;
            }
            return (int)ll;
        }
        /// <summary>
        /// 重置所有字段信息
        /// </summary>
        private void clear()
        {
            downloadUrl = string.Empty;    //重置下载地址
            stop = false;                  //标记下载状态为未暂停
            filename = string.Empty;       //重置文件名称
            fileNameAndPath = string.Empty;//重置文件路径名称
            filepath = string.Empty;       //重置文件路径
            lbo = new List<bool>();        //重置每条线程的下载状态
            xiancheng = 0;                 //重置线程数量
            lli = new List
                <Locations.Locations>();   //重置续传信息
            complete = true;               //标记当前对象,下载状态为 已完成
        }
        /// <summary>
        /// 关闭所有下载线程
        /// </summary>
        public void AborThread()
        {
            foreach (var item in G_thread_Collection)
            {
                item.Abort();//关闭线程
            }
        }
        /// <summary>
        /// 测试是否支持多线程下载
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <returns></returns>
        bool GetBool(string url)
        {
            List<Thread> lth = new List<Thread>();//创建线程集合
            int count = 0;//临时变量
            for (int i = 0; i < 3; i++)
            {
                Thread th = new Thread(delegate ()
                  {
                      try
                      {
                          //使用下载地址创建HttpWebRequest对象
                          HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
                          hwr.Timeout = 3000;//设置超时时间为3秒
                          HttpWebResponse hwp = (HttpWebResponse)hwr.GetResponse();//获取网络资源响应
                          hwr.Abort();//取消响应
                      }
                      catch 
                      {

                          count++;//给临时变量加1
                      }
                  });
                th.Name = i.ToString();//设置线程名称
                th.IsBackground = true;//设置为后台线程
                th.Start();            //开始线程
                lth.Add(th);           //将当前线程添加到线程集合中
            }
            foreach (var item in lth)  //遍历线程集合
            {
                item.Join();           //顺序执行线程
            }
            return count == 0;         //判断是否支持多线程
        }
    }
}
