using System;
using System.Windows.Forms;
using System.Threading;

namespace chuanqijiaoben
{
    public partial class Form1 : Form
    {
        private static Game game;
        private static bool flag = true;
        private Script script;
        public Form1()
        {
            InitializeComponent();
            game = new Game();
            script = new Script();
            Initial();
        }
        private void Initial()
        {
            this.cbAutoUseStatusPotion.Checked = script.AutoList[cbAutoUseStatusPotion.Text];
        }
        private void InitialMiscList()
        {
        }
        private void bStart_Click(object sender, EventArgs e)
        {
            game.Initial();
            //Thread t1 = new Thread(new ThreadStart(RemainWeightWatcherThread));
            //t1.Start();
            //while (!game.role.MovetoCoordinate(new Coordinate(284,143)))
            //{

            //}
            Monster monster = game.role.SearchMonster(7);
            game.role.PhysicalAttack(monster.Position);
        }
        public static void RemainWeightWatcherThread()
        {
            Thread.Sleep(1000 * 5);
            while (true)
            {
                flag = false;
            }
        }
        public void BattleMode()
        {

        }

        private void cbMend_CheckedChanged(object sender, EventArgs e)
        {
            string name = ((CheckBox)sender).Text;
        }
        private void tcMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            script.Mode = tcMode.SelectedTab.Text;
        }
        private void tcMoveMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            script.MoveMethod = tcMoveMode.SelectedTab.Text;
        }
        private void checkbox_CheckedChanged(object sender, EventArgs e)
        {
            script.AutoList[((CheckBox)sender).Text] = ((CheckBox)sender).Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            script.SaveSetting(script);
        }
    }
}
