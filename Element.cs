using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliminacjaGaussa
{
    /// <summary>
    /// Klasa opisująca pojedynczy wiersz w tablicy pomocniczej wierzchołków i krawędzi. Pola o1,o2,o3,o4,o5 odpowiadają kolejnym
    /// odwołaniom do tablic występujących w algorytmie, a ich kolejne elementy odpowiadają kolejnym współrzędnym w tablicach.
    /// </summary>
    class Element
    {
        public int id { get; set; }
        public int i1 { get; set; }
        public int i2 { get; set; }
        public int i3 { get; set; }

        private Tuple<int?, int?> o1;

        public String o1Prop
        {
            get { return new String(o1.Item1.ToString() + "   " + o1.Item2.ToString());}
        }

        private Tuple<int?, int?> o2;

        public String o2Prop
        {
            get { return new String(o2.Item1.ToString() + "   " + o2.Item2.ToString()); }
        }

        private Tuple<int?, int?> o3;

        public String o3Prop
        {
            get { return new String(o3.Item1.ToString() + "   " + o3.Item2.ToString()); }
        }

        private Tuple<int?, int?> o4;

        public String o4Prop
        {
            get { return new String(o4.Item1.ToString() + "   " + o4.Item2.ToString()); }
        }

        private Tuple<int?, int?> o5;

        public String o5Prop
        {
            get { return new String(o5.Item1.ToString() + "   " + o5.Item2.ToString()); }
        }

        public Element(int id, int i1, int i2, int i3)
        {
            this.id = id;
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
        }

        public Element(int id, int i1,int i2, int i3, int? o11, int? o12, int? o21, int? o22, int? o31, int? o32, int? o41, int? o42, int? o51, int? o52)
        {
            this.id = id;
            this.i1 = i1;
            this.i2 = i2;
            this.i3 = i3;
            o1 = new Tuple<int?, int?>(o11, o12);
            o2 = new Tuple<int?, int?>(o21, o22);
            o3 = new Tuple<int?, int?>(o31, o32);
            o4 = new Tuple<int?, int?>(o41, o42);
            o5 = new Tuple<int?, int?>(o51, o52);
        }
    }
}
