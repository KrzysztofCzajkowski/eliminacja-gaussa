﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliminacjaGaussa
{
    public class EP
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int T { get; set; }
        public int I1 { get; set; }
        public int I2 { get; set; }
        public int I3 { get; set; }

        public EP(int id, int x, int y, int t, int i1, int i2, int i3)
        {
            ID = id;
            X = x;
            Y = y;
            T = t;
            I1 = i1;
            I2 = i2;
            I3 = i3;
        }
        public EP(int id, int x, int t, int i1, int i2, int i3)
        {
            ID = id;
            X = x;
            Y = 0;
            T = t;
            I1 = i1;
            I2 = i2;
            I3 = i3;
        }
    }
}
