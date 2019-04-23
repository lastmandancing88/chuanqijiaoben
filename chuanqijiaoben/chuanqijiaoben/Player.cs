using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chuanqijiaoben
{
    public sealed class Player : Singleton<Player>
    {
        private static volatile Player instance;
        private static object syncRoot = new Object();
        private DM dm;
        private Game game;
        public bool Attack(Coordinate coordinate)
        {
            return false;
        }
        public Player()
        {
            dm = DM.GetInstance();
            game = Game.GetInstance();
        }
    }
}
