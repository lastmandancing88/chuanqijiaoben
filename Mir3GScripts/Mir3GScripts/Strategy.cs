using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mir3GScripts
{
    public class Strategy : Singleton<Strategy>
    {
        private List<Map> viaMaps;
        public MoveStrategy MoveStrategy { get; set; }
        public Map BattleMap { get; set; }
        public List<Buff> KeepBuffs { get; set; }
        public int RecoverViaTime { get; set; }
    }
}
