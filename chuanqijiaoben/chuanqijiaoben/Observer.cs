using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chuanqijiaoben
{
    public class Observer
    {
        const string cmd = "";
        private Player player;

        public delegate void PlayerMovedHandler(Coordinate coordnate);
        public delegate void PlayerDeadHandler(bool isDead);
        public delegate void MapChangedHandler(Map map);
        public delegate void MonsterFoundHandler(Monster monster);
        public delegate void GoodThingFoundHandler(Coordinate coordinate);     

        public event PlayerMovedHandler PlayerMovedEvent;
        public event PlayerDeadHandler PlayerDeadEvent;
        public event MapChangedHandler MapChangedEvent;
        public event MonsterFoundHandler MonsterFoundEvent;
        public event GoodThingFoundHandler GoodThingFoundEvent;

        public void CheckIfPlayerMoved()
        {
            Coordinate coordinate = GetPlayerCoordinate();
            if (coordinate == player.Coordinate)
            {
            }
            else
            {
                PlayerMovedEvent(coordinate);
            }
        }
        public void SearchGoodThing()
        {

        }
        private Coordinate 
        public void SearchMonster()
        {
            MonsterFoundEvent(new Monster());
        }
        private Coordinate GetPlayerCoordinate()
        {
            return new Coordinate();
        }
        public void WatchPlayerHealth()
        {
            bool isDead = false;
            if (GetPlayerLife() == 0)
            {
                isDead = true;
            }
            else
            {
                isDead = false;
            }
            PlayerDeadEvent(isDead);
        }
        private int GetPlayerLife()
        {
            return 0;
        }
        public void WatchCurrentMap()
        {
            Map map = GetMapName();
            if (map != player.CurrentMap)
            {
                MapChangedEvent(map);
            }
        }
        private Map GetMapName()
        {
            return new Map();
        }
    }
}
