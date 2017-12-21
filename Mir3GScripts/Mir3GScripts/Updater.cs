using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mir3GScripts
{
    public class Updater : Singleton<Updater>
    {
        public delegate void ConfigurationHandler(object sender, EventArgs e);

        public void UpdatePlayerCoordinate(Coordinate coordinate)
        {
            Player.GetInstance().Coordinate = coordinate;
        }
        public void UpdatePlayerIsAlive(bool alive)
        {
            Player.GetInstance().IsAlive = alive;
        }
        public void UpdatePlayerCurrentMap(Map map)
        {
            Player.GetInstance().CurrentMap = map;
        }
        public void UpdateStrategyMove(object sender, EventArgs e)
        {
            Strategy.GetInstance().MoveStrategy = (MoveStrategy)Enum.Parse(typeof(MoveStrategy), ((RadioButton)sender).Text);
        }
        public void UpdateStrategyKeepBuffs(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
                Strategy.GetInstance().KeepBuffs.Add((Buff)Enum.Parse(typeof(Buff), ((CheckBox)sender).Text));
            else
                Strategy.GetInstance().KeepBuffs.Remove((Buff)Enum.Parse(typeof(Buff), ((CheckBox)sender).Text));
        }
        public void UpdateStrategyRecoverViaTime(object sender, EventArgs e)
        {
            Strategy.GetInstance().RecoverViaTime = ((TrackBar)sender).Value;
        }
    }
}
