using System;
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
        public Operacja OP { get; set; }

        private List<OperacjaEP> operacje;
        public List<OperacjaEP> Operacje
        {
            get 
            {
                if(operacje == null)
                    operacje = new List<OperacjaEP>();
                return operacje;
            }
            set { operacje = value; }
        }

        public EP(int id, int x, int y, Operacja op)
        {
            ID = id;
            X = x;
            Y = y;
            OP = op;
        }
        public EP(int id, int x, Operacja op)
        {
            ID = id;
            X = x;
            Y = 0;
            OP = op;
        }
    }
}
