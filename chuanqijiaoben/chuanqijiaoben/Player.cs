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
            switch (game.Role.verb)
            {
                case Verb.战士:
                    dm.LeftClick(coordinate.x, coordinate.y);
                    break;
                case Verb.法师:
                    break;
            }
            return false;
        }
        public Player()
        {
            dm = DM.GetInstance();
        }
        public bool HasTarget()
        {
            string targetInfo = dm.Ocr(Game.target_status_area[0], Game.target_status_area[0], Game.target_status_area[0], Game.target_status_area[0], Game.kTargetStatusColor, 1.0);
            if (targetInfo == "")
            {
                return false;
            }
            else
            {
                
            }
            return false;
        }
    }
}
