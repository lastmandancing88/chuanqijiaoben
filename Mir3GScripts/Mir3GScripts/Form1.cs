using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dm;

namespace Mir3GScripts
{
    public partial class Form1 : Form
    {
        private Strategy strategy;
        private Updater updater;
        private Player player;
        private Observer observer;
        public Form1()
        {
            InitializeComponent();
            this.rbOnFoot.CheckedChanged += new EventHandler(updater.UpdateStrategyMove);
            this.rbRandomScroll.CheckedChanged += new EventHandler(updater.UpdateStrategyMove);
            this.rbTransport.CheckedChanged += new EventHandler(updater.UpdateStrategyMove);
            this.rbStay.CheckedChanged += new EventHandler(updater.UpdateStrategyMove);
            this.tbRecoverViaTime.ValueChanged += new EventHandler(updater.UpdateStrategyRecoverViaTime);
            Initial();
        }
        void Initial()
        {
            InitialMiscList();
        }
        void InitialMiscList()
        {
            lvMiscs.GridLines = true; //显示表格线
            lvMiscs.View = View.Details;//显示表格细节
            lvMiscs.LabelEdit = true; //是否可编辑,ListView只可编辑第一列。
            lvMiscs.Scrollable = true;//有滚动条
            lvMiscs.HeaderStyle = ColumnHeaderStyle.Clickable;//对表头进行设置
            lvMiscs.FullRowSelect = true;//是否可以选择行

            //this.lvMiscs.HotTracking = true;// 当选择此属性时则HoverSelection自动为true和Activation属性为oneClick
            //this.listView1.HoverSelection = true;
            //this.listView1.Activation = ItemActivation.Standard; //
            //添加表头
            lvMiscs.Columns.Add("", 20);
            lvMiscs.Columns.Add("Name", 100);
            lvMiscs.Columns.Add("Count", 80);
            //添加各项
            ListViewItem[] p = new ListViewItem[2];
            p[0] = new ListViewItem(new string[] { "", "万年雪霜", "1000" });
            p[1] = new ListViewItem(new string[] { "", "回城卷", "50" });
            //p[0].SubItems[0].BackColor = Color.Red; //用于设置某行的背景颜色

            this.lvMiscs.Items.AddRange(p);
            //也可以用this.listView1.Items.Add();不过需要在使用的前后添加Begin... 和End...防止界面自动刷新
            // 添加分组
            //this.lvMiscs.Groups.Add(new ListViewGroup("tou"));
            //this.lvMiscs.Groups.Add(new ListViewGroup("wei"));

            //this.lvMiscs.Items[0].Group = this.lvMiscs.Groups[0];
            //this.lvMiscs.Items[1].Group = this.lvMiscs.Groups[1];
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            observer.MonsterFoundEvent += player.Attack;
            while (player.IsAlive)
            {
                
            }
        }
    }
}
