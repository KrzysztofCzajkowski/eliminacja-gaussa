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
        public static double[,] b_gwiazdka;
        /// <summary>
        /// Macierz trójkątna dolna M potrzebna przy obliczeniach macierzy wynikowych.
        /// </summary>
        public static double[,] m;
        public static List<Element> dataFlowElements { get; set; }
        public static List<Wierzcholek> dataFlowVertices { get; set; }
        public static List<Krawedz> dataFlowEdges { get; set; }
        private static List<Element> sortedDataFlowElements { get; set;}

        public static void init()
        {
            dataFlowElements = new List<Element>();
            dataFlowVertices = new List<Wierzcholek>();
            dataFlowEdges = new List<Krawedz>();
            sortedDataFlowElements = new List<Element>();
        }

        /// <summary>
        /// Metoda inicjalizująca macierze wejściowe po podaniu rozmiaru N.
        /// </summary>
        public static void utworzMacierze()
        {
            a = new double[N, N + 1]; // macierz rozszerzona A|B
            a_gwiazdka = new double[N, N];
            b_gwiazdka = new double[1, N];
            m = new double[N, N];
        }
        /// <summary>
        /// Metoda obliczająca macierze wyjściowe A* oraz B* eliminacji Gaussa.
        /// </summary>
        public static void oblicz()
        {
            int id = 1;
            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    if (a[i, i] != 0)
                    {
                        m[j, i] = -a[j, i] / a[i, i];
                        dataFlowElements.Add(new Element(id, i, j, 1, j, i, j, i, i, i, null, null, null, null));
                        id++;
                    }
                    else
                        m[j, i] = 0;
                }
                for (int j = i; j < N; j++)
                {
                    for (int k = i + 1; k < N + 1; k++)
                    {
                        a[j, k] += m[j, i] * a[i, k];
                        dataFlowElements.Add(new Element(id, i, j, k, null, null, null, null, j, i, j, k, i, k));
                        id++;
                        if (i <= j)
                            a_gwiazdka[i, j] = a[i, j];
                    }
                }
                b_gwiazdka[0, i] = a[i, N];
            }
            ///<summary>
            ///Sortowanie data flow elements
            /// </summary>
            dataFlowElements = dataFlowElements.OrderBy(sorted => sorted.i1).ThenBy(sorted => sorted.i2).ThenBy(sorted => sorted.i3).ToList();

            ///<summary>
            ///Nadanie poprawnych indeksów
            /// </summary>
            for (id = 0; id < dataFlowElements.Count; id++)
            {
                dataFlowElements.ElementAt(id).id = id;
            }
        }
    }
}
