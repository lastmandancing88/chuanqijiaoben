using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mir3GScripts
{
    public class Monster
    {
        private string name;
        public Monster()
        {
        }
        public Monster(string name)
        {
            this.name = name;
        }
        public Coordinate Coordinate { get; set; }
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}
