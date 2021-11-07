using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliminacjaGaussa
{
    class Krawedz
    {
        public int id { get; set; }
        public int id1 { get; set; }
        public int id2 { get; set; }

        public Krawedz(int id, int id1, int id2)
        {
            this.id = id;
            this.id1 = id1;
            this.id2 = id2;
        }
    }
}
