using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeeEnDee
{
    class Monster : Entity
    {
        public string type {get; set;}
        public int challenge { get; set; }
        public string appearance { get; set; }
        public string actions { get; set; }
        public string page { get; private set; }

        public override string ToString()
        {
            return "Entity: " + Name + '(' + Race + ')';
        }
    }
}
