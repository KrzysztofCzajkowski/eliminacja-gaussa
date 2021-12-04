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
        public static double[,] D = new double[3, 4]
        {
            {1,0,0,1},
            {0,1,0,1},
            {0,0,1,0}
        };
        public static double[,] Fs1 = new double[2, 3]
        {
            {-1,  0, 1},
            {1, -1, 1}
        };
        public static double[,] Fs1D = new double[2, 3];

        public static double[,] Fs2 = new double[2, 3]
        {
            { -1,  1, 1},
            {-1, 1, 0}
        };
        public static double[,] Fs2D = new double[2, 3];
        public static double[,] Fs3 = new double[1, 3]
        {
            {-1, 0, 1}
        };
        public static double[,] Fs3D = new double[2, 3];
        public static double[,] Ft = new double[1, 3]
        {
            {1, 1, 1}
        };
        public static List<Element> dataFlowElements { get; set; }
        public static List<Wierzcholek> dataFlowVertices { get; set; }
        public static List<Krawedz> dataFlowEdges { get; set; }

        public static void init()
        {
            dataFlowElements = new List<Element>();
            dataFlowVertices = new List<Wierzcholek>();
            dataFlowEdges = new List<Krawedz>();
        }

        public static void clear()
        {
            dataFlowElements.Clear();
            dataFlowVertices.Clear();
            dataFlowEdges.Clear();
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
                    if ((elementA.i1 == elementB.i1 - 1) && (elementA.i2 == elementB.i2 - 1) && (elementA.i3 == elementB.i3) && elementA.o4Prop.Equals(elementB.o3Prop))
                    {
                        dataFlowEdges.Add(new Krawedz(id, i, j, Kierunek.I1I2));
                        id++;
                    }
                    // krawędź w kierunku i1
                    else if ((elementA.i1 == elementB.i1 - 1) && (elementA.i2 == elementB.i2) && (elementA.i3 == elementB.i3) && elementA.o4Prop.Equals(elementB.o2Prop))
                    {
                        dataFlowEdges.Add(new Krawedz(id, i, j, Kierunek.I1));
                        id++;
                    }
                    // krawędź w kierunku i2
                    else if ((elementA.i2 == elementB.i2 - 1) && (elementA.i1 == elementB.i1) && (elementA.i3 == elementB.i3) && elementA.o5Prop.Equals(elementB.o5Prop))
                    {
                        dataFlowEdges.Add(new Krawedz(id, i, j, Kierunek.I2));
                        id++;
                    }
                    // krawędź w kierunku i3
                    else if ((elementA.i3 == elementB.i3 - 1) && (elementA.i1 == elementB.i1) && (elementA.i2 == elementB.i2) && elementA.o2Prop.Equals(elementB.o3Prop))
                    {
                        dataFlowEdges.Add(new Krawedz(id, i, j, Kierunek.I3));
                        id++;
                    }
                }
            }
        }

        public static double[,] mnozeniePrzezD(double[,] macierz)
        {
            double[,] wynik = new double[macierz.GetLength(0), 4];
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

    public static bool sprawdzMacierz(double[,] macierz)
        {
            double suma;
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
    }
}
