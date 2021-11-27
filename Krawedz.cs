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
        public int poczatek { get; set; }
        public int koniec { get; set; }
        public Kierunek kierunek { get; set; }

        public Krawedz(int id, int poczatek, int koniec, Kierunek kierunek)
        {
            this.id = id;
            this.poczatek = poczatek;
            this.koniec = koniec;
            this.kierunek = kierunek;
        }
    }
}
