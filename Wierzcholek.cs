using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliminacjaGaussa
{
    class Wierzcholek
    {
        public int id { get; set; }
        public int i1 { get; set; }
        public int i2 { get; set; }
        public int i3 { get; set; }
        public Operacja typOperacji { get; set; }

        public Wierzcholek(int id, int i1, int i2, int i3, Operacja typOperacji)
        {
            this.id = id;
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
            this.typOperacji = typOperacji;
        }
    }
}
