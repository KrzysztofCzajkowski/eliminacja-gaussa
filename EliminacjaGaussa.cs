using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliminacjaGaussa
{
    /// <summary>
    /// Klasa statyczna przechowująca modele macierzy niezbędnych do obliczenia eliminacji Gaussa.
    /// </summary>
    static class EliminacjaGaussa
    {
        /// <summary>
        /// Rozmiar N macierzy wejściowej.
        /// </summary>
        public static int N;
        /// <summary>
        /// Macierz rozszerzona A|B
        /// </summary>
        public static double[,] a;
        /// <summary>
        /// Macierz trójkątna górna A* stanowiąca część wyniku eliminacji Gaussa.
        /// </summary>
        public static double[,] a_gwiazdka;
        /// <summary>
        /// Macierz B* wyrazów wolnych będąca częścią wyniku rozkładu Gaussa.
        /// </summary>
        public static double[] b_gwiazdka;
        /// <summary>
        /// Macierz trójkątna dolna M potrzebna przy obliczeniach macierzy wynikowych.
        /// </summary>
        public static double[,] m;
        /// <summary>
        /// Macierz D.
        /// </summary>
        public static int[,] D = new int[3, 4]
        {
            {1,0,0,1},
            {0,1,0,1},
            {0,0,1,0}
        };
        public static int[,] Fs1 = new int[2, 3]
        {
            {1, 0, 0},
            {0, 1, 0}
        };
        public static int[,] Fs1D = new int[2, 3];

        public static int[,] Fs2 = new int[2, 3]
        {
            {1, 0, 0},
            {0, 0, 1}
        };
        public static int[,] Fs2D = new int[2, 3];
        public static int[,] Fs3 = new int[1, 3]
        {
            {-1, 0, 1}
        };
        public static int[,] Fs3D = new int[2, 3];
        public static int[,] Ft = new int[1, 3]
        {
            {1, 1, 1}
        };
        public static List<Element> dataFlowElements { get; set; }
        public static List<Wierzcholek> dataFlowVertices { get; set; }
        public static List<Krawedz> dataFlowEdges { get; set; }

        public static List<OperacjaEP> operacjeMP1 { get; set; }
        public static List<OperacjaEP> operacjeMP2 { get; set; }
        public static List<OperacjaEP> operacjeMP3 { get; set; }

        public static List<EP> MP1 { get; set; }
        public static List<EP> MP2 { get; set; }
        public static List<EP> MP3 { get; set; }

        public static void init()
        {
            dataFlowElements = new List<Element>();
            dataFlowVertices = new List<Wierzcholek>();
            dataFlowEdges = new List<Krawedz>();
            operacjeMP1 = new();
            operacjeMP2 = new();
            operacjeMP3 = new();
            MP1 = new();
            MP2 = new();
            MP3 = new();
        }

        public static void clear()
        {
            dataFlowElements.Clear();
            dataFlowVertices.Clear();
            dataFlowEdges.Clear();
            operacjeMP1.Clear();
            operacjeMP2.Clear();
            operacjeMP3.Clear();
            MP1.Clear();
            MP2.Clear();
            MP3.Clear();
        }

        /// <summary>
        /// Metoda inicjalizująca macierze wejściowe po podaniu rozmiaru N.
        /// </summary>
        public static void utworzMacierze()
        {
            a = new double[N, N + 1]; // macierz rozszerzona A|B
            a_gwiazdka = new double[N, N];
            b_gwiazdka = new double[N];
            m = new double[N, N];
        }
        /// <summary>
        /// Metoda obliczająca macierze wyjściowe A* oraz B* eliminacji Gaussa.
        /// </summary>
        public static void oblicz()
        {
            int id = 1;
            
            for (int i = 0; i < N-1; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    if (a[i, i] != 0)
                    {
  
                        m[j, i] = -a[j, i] / a[i, i];
                        dataFlowElements.Add(new Element(id, i, j, i, Operacja.DZIELENIE, j, i, j, i, i, i, null, null, null, null));
                        id++;
                    }
                    else
                        m[j, i] = 0;
                }
                for (int j = i + 1; j < N; j++)
                {
                    for (int k = i + 1; k < N + 1; k++)
                    {
                        a[j, k] += m[j, i] * a[i, k];
                        dataFlowElements.Add(new Element(id, i, j, k, Operacja.MNOZENIE, null, null, null, null, j, i, j, k, i, k));
                        id++;
                        if (i <= j)
                            a_gwiazdka[i, j] = a[i, j];
                    }
                }
                b_gwiazdka[i] = a[i, N-1];
            }
            b_gwiazdka[N-1] = a[N-1, N-1];
            for (int k = 0; k < N; k++)
                a_gwiazdka[k, k] = a[k, k];
        }

        public static void sortuj()
        {
            // Sortowanie data flow elements
            dataFlowElements = dataFlowElements.OrderBy(sorted => sorted.i1).ThenBy(sorted => sorted.i2).ThenBy(sorted => sorted.i3).ToList();

            // Nadanie poprawnych indeksów
            for (int i = 1; i < dataFlowElements.Count + 1; i++)
            {
                dataFlowElements.ElementAt(i - 1).id = i;
            }

            int id = 1;
            Element elementA = null;
            Element elementB = null;

            // Znalezienie krawędzi
            for (int i = 1; i <= dataFlowElements.Count; i++)
            {
                for (int j = 1; j <= dataFlowElements.Count; j++)
                {
                    elementA = dataFlowElements[i - 1];
                    elementB = dataFlowElements[j - 1];
                    // krawędź w kierunku i1 i2
                    if ((elementA.i1 == elementB.i1 - 1) && (elementA.i2 == elementB.i2 - 1) && (elementA.i3 == elementB.i3) && (elementA.o4Prop.Equals(elementB.o3Prop) || elementA.o4Prop.Equals(elementB.o5Prop)))
                    {
                        dataFlowEdges.Add(new Krawedz(id, i, j, Kierunek.I1I2));
                        id++;
                    }
                    // krawędź w kierunku i1
                    else if ((elementA.i1 == elementB.i1 - 1) && (elementA.i2 == elementB.i2) && (elementA.i3 == elementB.i3) && (elementA.o4Prop.Equals(elementB.o2Prop) || elementA.o4Prop.Equals(elementB.o4Prop)))
                    {
                        dataFlowEdges.Add(new Krawedz(id, i, j, Kierunek.I1));
                        id++;
                    }
                    // krawędź w kierunku i2
                    else if ((elementA.i2 == elementB.i2 - 1) && (elementA.i1 == elementB.i1) && (elementA.i3 == elementB.i3) && (elementA.o3Prop.Equals(elementB.o3Prop) || elementA.o5Prop.Equals(elementB.o5Prop)))
                    {
                        dataFlowEdges.Add(new Krawedz(id, i, j, Kierunek.I2));
                        id++;
                    }
                    // krawędź w kierunku i3
                    else if ((elementA.i3 == elementB.i3 - 1) && (elementA.i1 == elementB.i1) && (elementA.i2 == elementB.i2) && (elementA.o2Prop.Equals(elementB.o3Prop) || elementA.o3Prop.Equals(elementB.o3Prop)))
                    {
                        dataFlowEdges.Add(new Krawedz(id, i, j, Kierunek.I3));
                        id++;
                    }
                }
            }
        }

        public static void wyznaczMacierzProcesorowe()
        {
            operacjeMP1 = mnozenieFs2D(Fs1);
            operacjeMP2 = mnozenieFs2D(Fs2);
            operacjeMP3 = mnozenieFs1D(Fs3);

            int id = 1;
            foreach (var item in operacjeMP1.Distinct(new EPComparator()))
            {
                MP1.Add(new EP(id, item.X, item.Y, Operacja.MNOZENIE));
                id++;
            }
            foreach (var item in MP1)
            {
                foreach (var item2 in operacjeMP1.Where(x => (x.X == item.X && x.Y == item.Y)))
                {
                    item.Operacje.Add(item2);
                    if (item2.OP == Operacja.DZIELENIE)
                        item.OP = Operacja.DZIELENIE;
                }
            }

            id = 1;
            foreach (var item in operacjeMP2.Distinct(new EPComparator()))
            {
                MP2.Add(new EP(id, item.X, item.Y, Operacja.MNOZENIE));
                id++;
            }
            foreach (var item in MP2)
            {
                foreach (var item2 in operacjeMP2.Where(x => (x.X == item.X && x.Y == item.Y)))
                {
                    item.Operacje.Add(item2);
                    if (item2.OP == Operacja.DZIELENIE)
                        item.OP = Operacja.DZIELENIE;
                }
            }

            id = 1;
            foreach (var item in operacjeMP3.Distinct(new EPComparator()))
            {
                MP3.Add(new EP(id, item.X, item.Y, Operacja.MNOZENIE));
                id++;
            }
            foreach (var item in MP3)
            {
                foreach (var item2 in operacjeMP3.Where(x => (x.X == item.X && x.Y == item.Y)))
                {
                    item.Operacje.Add(item2);
                    if (item2.OP == Operacja.DZIELENIE)
                        item.OP = Operacja.DZIELENIE;
                }
            }
        }

        public static int[,] mnozeniePrzezD(int[,] macierz)
        {
            int[,] wynik = new int[macierz.GetLength(0), 4];
            for (int i = 0; i < macierz.GetLength(0); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < macierz.GetLength(1); k++)
                    {
                        wynik[i,j] += macierz[i, k] * D[k, j];
                    }
                }
            }
            return wynik;
        }

        public static bool sprawdzMacierz(int[,] macierz)
        {
            int suma;
            for (int i = 0; i < macierz.GetLength(0); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    suma = 0;
                    for (int k = 0; k < macierz.GetLength(1); k++)
                    {
                        suma += macierz[i, k] * D[k, j];
                        if (suma != -1 && suma != 0 && suma != 1)
                            return false;
                    }
                }
            }
            return true;
        }

        public static List<OperacjaEP> mnozenieFs2D(int[,] fs)
        {
            List<OperacjaEP> wynik = new();

            int id = 1;
            foreach (var item in dataFlowElements)
            {
                int x = 0;
                int y = 0;

                x += fs[0, 0] * item.i1;
                x += fs[0, 1] * item.i2;
                x += fs[0, 2] * item.i3;

                y += fs[1, 0] * item.i1;
                y += fs[1, 1] * item.i2;
                y += fs[1, 2] * item.i3;

                wynik.Add(new OperacjaEP(id, x, y, item.i1 + item.i2 + item.i3, item.i1, item.i2, item.i3, item.typOperacji));
                id++;
            }
            return wynik;
        }

        public static List<OperacjaEP> mnozenieFs1D(int[,] fs)
        {
            List<OperacjaEP> wynik = new();

            int id = 1;
            foreach (var item in dataFlowElements)
            {
                int x = 0;

                x += fs[0,0] * item.i1;
                x += fs[0,1] * item.i2;
                x += fs[0,2] * item.i3;

                wynik.Add(new OperacjaEP(id, x, item.i1 + item.i2 + item.i3, item.i1, item.i2, item.i3, item.typOperacji));
                id++;
            }
            return wynik;
        }
    }
}
