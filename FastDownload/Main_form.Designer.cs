namespace FastDownload
{
    partial class Main_form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_form));
            this.lv_state = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pbox_new = new System.Windows.Forms.PictureBox();
            this.pbox_start = new System.Windows.Forms.PictureBox();
            this.pbox_pause = new System.Windows.Forms.PictureBox();
            this.pbox_delete = new System.Windows.Forms.PictureBox();
            this.pbox_continue = new System.Windows.Forms.PictureBox();
            this.pbox_set = new System.Windows.Forms.PictureBox();
            this.pbox_close = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_new)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_pause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_delete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_continue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_set)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv_state
            // 
            this.lv_state.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lv_state.FullRowSelect = true;
            this.lv_state.GridLines = true;
            this.lv_state.HideSelection = false;
            this.lv_state.Location = new System.Drawing.Point(12, 95);
            this.lv_state.Name = "lv_state";
            this.lv_state.Size = new System.Drawing.Size(1041, 374);
            this.lv_state.TabIndex = 8;
            this.lv_state.UseCompatibleStateImageBehavior = false;
            this.lv_state.View = System.Windows.Forms.View.Details;
            this.lv_state.SelectedIndexChanged += new System.EventHandler(this.lv_state_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "文件名";
            this.columnHeader1.Width = 98;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "文件大小";
            this.columnHeader2.Width = 104;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "下载进度";
            this.columnHeader3.Width = 90;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "下载完成量";
            this.columnHeader4.Width = 170;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "已用时间";
            this.columnHeader5.Width = 132;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "文件类型";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "创建时间";
            this.columnHeader7.Width = 160;
            // 
            // pbox_new
            // 
            this.pbox_new.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbox_new.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbox_new.Image = ((System.Drawing.Image)(resources.GetObject("pbox_new.Image")));
            this.pbox_new.Location = new System.Drawing.Point(12, 12);
            this.pbox_new.Name = "pbox_new";
            this.pbox_new.Size = new System.Drawing.Size(56, 62);
            this.pbox_new.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbox_new.TabIndex = 1;
            this.pbox_new.TabStop = false;
            this.pbox_new.Click += new System.EventHandler(this.pbox_new_Click);
            // 
            // pbox_start
            // 
            this.pbox_start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbox_start.Image = ((System.Drawing.Image)(resources.GetObject("pbox_start.Image")));
            this.pbox_start.Location = new System.Drawing.Point(74, 12);
            this.pbox_start.Name = "pbox_start";
            this.pbox_start.Size = new System.Drawing.Size(57, 62);
            this.pbox_start.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbox_start.TabIndex = 2;
            this.pbox_start.TabStop = false;
            this.pbox_start.Click += new System.EventHandler(this.pbox_start_Click);
            // 
            // pbox_pause
            // 
            this.pbox_pause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbox_pause.Image = ((System.Drawing.Image)(resources.GetObject("pbox_pause.Image")));
            this.pbox_pause.Location = new System.Drawing.Point(137, 12);
            this.pbox_pause.Name = "pbox_pause";
            this.pbox_pause.Size = new System.Drawing.Size(59, 62);
            this.pbox_pause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbox_pause.TabIndex = 3;
            this.pbox_pause.TabStop = false;
            this.pbox_pause.Click += new System.EventHandler(this.pbox_pause_Click);
            // 
            // pbox_delete
            // 
            this.pbox_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbox_delete.Image = ((System.Drawing.Image)(resources.GetObject("pbox_delete.Image")));
            this.pbox_delete.Location = new System.Drawing.Point(202, 12);
            this.pbox_delete.Name = "pbox_delete";
            this.pbox_delete.Size = new System.Drawing.Size(57, 62);
            this.pbox_delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbox_delete.TabIndex = 4;
            this.pbox_delete.TabStop = false;
            this.pbox_delete.Click += new System.EventHandler(this.pbox_delete_Click);
            // 
            // pbox_continue
            // 
            this.pbox_continue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbox_continue.Image = ((System.Drawing.Image)(resources.GetObject("pbox_continue.Image")));
            this.pbox_continue.Location = new System.Drawing.Point(265, 12);
            this.pbox_continue.Name = "pbox_continue";
            this.pbox_continue.Size = new System.Drawing.Size(61, 62);
            this.pbox_continue.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbox_continue.TabIndex = 5;
            this.pbox_continue.TabStop = false;
            this.pbox_continue.Click += new System.EventHandler(this.pbox_continue_Click);
            // 
            // pbox_set
            // 
            this.pbox_set.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbox_set.Image = ((System.Drawing.Image)(resources.GetObject("pbox_set.Image")));
            this.pbox_set.Location = new System.Drawing.Point(332, 12);
            this.pbox_set.Name = "pbox_set";
            this.pbox_set.Size = new System.Drawing.Size(58, 62);
            this.pbox_set.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbox_set.TabIndex = 6;
            this.pbox_set.TabStop = false;
            this.pbox_set.Click += new System.EventHandler(this.pbox_set_Click);
            // 
            // pbox_close
            // 
            this.pbox_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbox_close.Image = ((System.Drawing.Image)(resources.GetObject("pbox_close.Image")));
            this.pbox_close.Location = new System.Drawing.Point(1019, 1);
            this.pbox_close.Name = "pbox_close";
            this.pbox_close.Size = new System.Drawing.Size(45, 39);
            this.pbox_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbox_close.TabIndex = 7;
            this.pbox_close.TabStop = false;
            this.pbox_close.Click += new System.EventHandler(this.pbox_close_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(133, 500);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "[0KB/S]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 500);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "[0KB/S]";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 486);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 29);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(108, 486);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(23, 29);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 28);
            this.contextMenuStrip1.Text = "退出";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "闪电锋下载器";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15F);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(396, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(387, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "一款比迅雷还好用的闪电下载器！\r\n";
            // 
            // Main_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 537);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbox_close);
            this.Controls.Add(this.pbox_set);
            this.Controls.Add(this.pbox_continue);
            this.Controls.Add(this.pbox_delete);
            this.Controls.Add(this.pbox_pause);
            this.Controls.Add(this.pbox_start);
            this.Controls.Add(this.pbox_new);
            this.Controls.Add(this.lv_state);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Main_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "闪电下载器";
            this.Load += new System.EventHandler(this.Main_form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_form_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_form_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pbox_new)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_pause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_delete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_continue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_set)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lv_state;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pbox_new;
        private System.Windows.Forms.PictureBox pbox_start;
        private System.Windows.Forms.PictureBox pbox_pause;
        private System.Windows.Forms.PictureBox pbox_delete;
        private System.Windows.Forms.PictureBox pbox_continue;
        private System.Windows.Forms.PictureBox pbox_set;
        private System.Windows.Forms.PictureBox pbox_close;
        private System.Windows.Forms.Label label3;
    }
}

