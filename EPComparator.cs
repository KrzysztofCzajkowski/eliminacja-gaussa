using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliminacjaGaussa
{
    internal class EPComparator : IEqualityComparer<OperacjaEP>
    {
        public bool Equals(OperacjaEP x, OperacjaEP y)
        {
            if (x.X == y.X && x.Y == y.Y) return true;
            else return false;
        }

        public int GetHashCode(OperacjaEP ep)
        {
            int hCode = ep.X ^ ep.Y;
            return hCode.GetHashCode();
        }
    }
}
