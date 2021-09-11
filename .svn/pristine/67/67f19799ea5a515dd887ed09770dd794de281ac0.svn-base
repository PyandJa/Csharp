using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FastDownload
{
    public partial class LoadStart : Form
    {
        public LoadStart()
        {
            InitializeComponent();
        }
        private Point p1;      //记录鼠标坐标，用于移动窗体
        private Main_form bs2; //获取主窗体对象

        private void LoadStart_Load(object sender, EventArgs e)
        {
            cbox_count.SelectedIndex = 5; //默认使用6条线程下载
            tb_savepath.Text = Set.Path;  //默认下载路径
            bs2 = Owner as Main_form;     //得到主窗体实例的引用
        }
        //浏览按扭
        private void button1_Click(object sender, EventArgs e)
        { 
           
        }
        //自动获取下载文件名
        private void tb_url_TextChanged(object sender, EventArgs e)
        {
            string strUrl = tb_url.Text; //获取下载地址
            if (strUrl.IndexOf("/")>0)   //自动获取下载文件名
            {
                tb_filename.Text = strUrl.Substring(strUrl.LastIndexOf("/") + 1);
            }
        }
        //立即下载实现
        private void pbox_true_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_url.Text)||String.IsNullOrEmpty(tb_filename.Text))
            {
                MessageBox.Show("请输入下载地址及路径！");
            }
            else
            {
                if (!System.IO.Directory.Exists(tb_savepath.Text))//判断是否存在目录
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(tb_savepath.Text);//创建路径
                    }
                    catch 
                    {

                        MessageBox.Show("默认磁盘不存在,请重新选择保存路径！");
                        btn_browse_Click_1(sender, e);
                    }
                }
                bs2.downloadUrl = tb_url.Text;//设置下载地址
                bs2.filename = bs2.downloadUrl.Substring(bs2.downloadUrl.LastIndexOf("/") + 1,
                    bs2.downloadUrl.Length - (bs2.downloadUrl.LastIndexOf("/") + 1));//设置文件名称
                tb_filename.Text = bs2.filename;
                bs2.xiancheng = cbox_count.SelectedIndex + 1;//设置下载文件使用的线程数量
                if (tb_savepath.Text.EndsWith("\\"))
                {
                    bs2.fileNameAndPath = tb_savepath.Text + bs2.filename;
                }
                else
                {
                    bs2.fileNameAndPath = tb_savepath.Text + @"\" + bs2.filename;
                }
                if (tb_savepath.Text!=string.Empty)//如果文件路径不等于空字符串
                {
                    Set.WritePrivateProfileString(Set.strNode, "Path", 
                        tb_savepath.Text, Set.strPath);

                    DownLoad dl1 = new DownLoad(bs2.filename, tb_savepath.Text,
                        bs2.downloadUrl, bs2.fileNameAndPath, bs2.xiancheng);
                    bs2.dl.Add(dl1); //将下载类型的实例放入下载列表
                    this.Close();    //关闭当前窗体
                }
                else
                {
                    MessageBox.Show("请选择下载文件的保存位置");
                }
            }
        }
        //浏览事件
       
        private void btn_browse_Click_1(object sender, EventArgs e)
        {

            DialogResult dr = folderBrowserDialog1.ShowDialog();      //选择现在文件保存的文件夹
            if (dr == DialogResult.OK)
            {
                tb_savepath.Text = folderBrowserDialog1.SelectedPath; //显示下载路径
                bs2.filepath = tb_savepath.Text;                      //得到下载路径
                Set.WritePrivateProfileString(Set.strNode, "Path", tb_savepath.Text, Set.strPath);
            }
        }
        //取消按钮
        private void pbox_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //移动窗体事件
        private void LoadStart_MouseDown(object sender, MouseEventArgs e)
        {
            p1 = new Point(-e.X, -e.Y);
        }

        private void LoadStart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                Point p2 = MousePosition;
                p2.Offset(p1);
                DesktopLocation = p2;
            }
        }
    }
}
