using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chuanqijiaoben
{
    class Player : Singleton<Player>
    {
        public delegate void MoveHandler();
        public event MoveHandler MoveEvent;

        public Coordinate Coordinate { get; set; }
        public bool IsAlive { get; set; }
        public Map CurrentMap { get; set; }

        public void Move(Coordinate coordinate)
        {
            MoveEvent();
        }
        public void MoveToMap(Map map)
        {

        }
        public void Attack(Monster monster)
        {
            DM.GetInstance().Controller.KeyPress(27);
        }
    }
}
