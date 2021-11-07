using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; //StreamReader
using System.Diagnostics; //Stopwatch
using System.Windows.Forms;

using Accord.Controls; // ArrayDataView

namespace EliminacjaGaussa
{
    public partial class Menu : Form
    {
        /// <summary>
        /// Główne okno aplikacji.
        /// </summary>
        public Menu()
        {
            InitializeComponent();
            // Inicjalizacja list wierzchołków i krawędzi.
            EliminacjaGaussa.init();
        }

        /// <summary>
        /// Metoda odświeżająca wyświetlaną listę wierzchołków i krawędzi obiektu elementDataGridView.
        /// </summary>
        public void UpdateElementList()
        {
            elementDataGridView.Rows.Clear();
            foreach (var element in EliminacjaGaussa.dataFlowElements)
            {
                elementDataGridView.Rows.Add(new string[]{
                    element.id.ToString(),
                    element.i1.ToString(),
                    element.i2.ToString(),
                    element.i3.ToString(),
                    element.o1Prop,
                    element.o2Prop,
                    element.o3Prop,
                    element.o4Prop,
                    element.o5Prop
                });
            }
        }

        public void UpdateVertexList()
        {
            vertexDataGridView.Rows.Clear();
            foreach (var element in EliminacjaGaussa.dataFlowElements)
            {
                string[] row =
                {
                    element.id.ToString(),
                    element.i1.ToString(),
                    element.i2.ToString(),
                    element.i3.ToString()
                };
                vertexDataGridView.Rows.Add(row);
            }
        }

        /// <summary>
        /// Metoda inicjalizująca Menu.
        /// </summary>
        private void Menu_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Zdarzenie kliknięcia przycisku "Ustaw". Ustawia rozmiar macierzy A.
        /// </summary>
        private void ustawButton_Click(object sender, EventArgs e)
        {
            EliminacjaGaussa.N = int.Parse(nTextBox.Text);
            EliminacjaGaussa.utworzMacierze();
            try
            {
                macierzADataGridView.DataSource = new ArrayDataView(EliminacjaGaussa.a);
                macierzMDataGridView.DataSource = new ArrayDataView(EliminacjaGaussa.m);
                macierzAGwiazdkaDataGridView.DataSource = new ArrayDataView(EliminacjaGaussa.a_gwiazdka);
                macierzBGwiazdkaDataGridView.DataSource = new ArrayDataView(EliminacjaGaussa.b_gwiazdka);
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Macierz ma zbyt duży rozmiar aby ją wyświetlić");
            }
        }
        /// <summary>
        /// Zdarzenie kliknięcia przycisku "Oblicz". Oblicza macierze M, A* oraz B* algorytmu eliminacji Gaussa i podaje czas obliczeń.
        /// </summary>
        private void obliczButton_Click(object sender, EventArgs e)
        {
            Stopwatch stoper = Stopwatch.StartNew();
            EliminacjaGaussa.oblicz();
            stoper.Stop();
            stopwatchTimeLabel.Text = (stoper.ElapsedMilliseconds.ToString() + " ms");
            macierzAGwiazdkaDataGridView.Refresh();
            macierzBGwiazdkaDataGridView.Refresh();
            macierzMDataGridView.Refresh();
            macierzAGwiazdkaDataGridView.AutoResizeColumns();
            macierzBGwiazdkaDataGridView.AutoResizeColumns();
            macierzMDataGridView.AutoResizeColumns();
            elementDataGridView.AutoResizeColumns();
            EliminacjaGaussa.dataFlowElements.OrderBy(e => i1).ThenBy(e => i2).ThenBy(e => i3);
            UpdateElementList();
            UpdateVertexList();
        }

        private void wczytajAButton_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string linia = string.Empty;
            string[] wiersz = null;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        // wczytaj rozmiar N macierzy A|B
                        EliminacjaGaussa.N = int.Parse(reader.ReadLine());
                        // zainicjalizuj macierze o odpowiednim rozmiarze
                        EliminacjaGaussa.utworzMacierze();
                        // następnie czytaj wiersz po wierszu do macierzy A|B
                        for (int i = 0; i < EliminacjaGaussa.N; i++)
                        {
                            linia = reader.ReadLine();
                            wiersz = linia.Split(',');
                            for (int j = 0; j < (EliminacjaGaussa.N + 1); j++)
                            {
                                EliminacjaGaussa.a[i, j] = double.Parse(wiersz[j]);
                            }
                        }
                    }
                }
            }
            try
            {
                macierzADataGridView.DataSource = new ArrayDataView(EliminacjaGaussa.a);
                macierzMDataGridView.DataSource = new ArrayDataView(EliminacjaGaussa.m);
                macierzAGwiazdkaDataGridView.DataSource = new ArrayDataView(EliminacjaGaussa.a_gwiazdka);
                macierzBGwiazdkaDataGridView.DataSource = new ArrayDataView(EliminacjaGaussa.b_gwiazdka);
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Macierz ma zbyt duży rozmiar aby ją wyświetlić");
            }
        }

        private void zapiszAButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Plik tekstowy (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = saveFileDialog.OpenFile();
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        // zapis rozmiaru macierzy A*
                        writer.WriteLine(EliminacjaGaussa.N);
                        for (int i = 0; i < EliminacjaGaussa.N; i++)
                        {
                            int j;
                            // zapis kolejnych elementów macierzy A*
                            for (j = 0; j < (EliminacjaGaussa.N - 1); j++)
                                writer.Write("{0},", EliminacjaGaussa.a_gwiazdka[i, j]);
                            writer.Write("{0}", EliminacjaGaussa.a_gwiazdka[i, j]);
                            writer.WriteLine();
                        }
                    }
                }
            }
        }

        private void zapiszBButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Plik tekstowy (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = saveFileDialog.OpenFile();
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        // zapis rozmiaru macierzy B*
                        writer.WriteLine(EliminacjaGaussa.N);
                        int i;
                        for (i = 0; i < EliminacjaGaussa.N; i++)
                        {
                            writer.Write("{0}", EliminacjaGaussa.b_gwiazdka[0, i]);
                            writer.WriteLine(EliminacjaGaussa.N);
                        }
                    }
                }
            }
        }
    }
}
