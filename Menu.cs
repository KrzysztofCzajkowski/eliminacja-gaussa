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
                    element.o5Prop,
                    element.typOperacji == Operacja.MNOZENIE ? "* +" : "/"
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
                    element.i3.ToString(),
                    element.typOperacji == Operacja.MNOZENIE ? "* +" : "/"
                };
                vertexDataGridView.Rows.Add(row);
            }
        }

        public void UpdateEdgeList()
        {
            krawedzDataGridView.Rows.Clear();
            foreach (var element in EliminacjaGaussa.dataFlowEdges)
            {
                
                string[] row =
                {
                    element.id.ToString(),
                    element.poczatek.ToString(),
                    element.koniec.ToString(),
                    element.kierunek switch
                    {
                        Kierunek.I1 => "i1",
                        Kierunek.I2 => "i2",
                        Kierunek.I3 => "i3",
                        Kierunek.I1I2 => "i1 i2",
                        _ => ""
                    }
                };
                krawedzDataGridView.Rows.Add(row);
            }
        }

        public void UpdateMP1List()
        {
            mp1DataGridView.Rows.Clear();
            foreach (var element in EliminacjaGaussa.operacjeMP1)
            {
                mp1DataGridView.Rows.Add(new string[]{
                    element.ID.ToString(),
                    element.X.ToString(),
                    element.Y.ToString(),
                    element.T.ToString(),
                    element.I1.ToString(),
                    element.I2.ToString(),
                    element.I3.ToString()
                });
            }
        }

        public void UpdateMP2List()
        {
            mp2DataGridView.Rows.Clear();
            foreach (var element in EliminacjaGaussa.operacjeMP2)
            {
                mp2DataGridView.Rows.Add(new string[]{
                    element.ID.ToString(),
                    element.X.ToString(),
                    element.Y.ToString(),
                    element.T.ToString(),
                    element.I1.ToString(),
                    element.I2.ToString(),
                    element.I3.ToString()
                });
            }
        }

        public void UpdateMP3List()
        {
            mp3DataGridView.Rows.Clear();
            foreach (var element in EliminacjaGaussa.operacjeMP3)
            {
                mp3DataGridView.Rows.Add(new string[]{
                    element.ID.ToString(),
                    element.X.ToString(),
                    element.Y.ToString(),
                    element.T.ToString(),
                    element.I1.ToString(),
                    element.I2.ToString(),
                    element.I3.ToString()
                });
            }
        }

        public void UpdateMP1Table()
        {
            int maxTakt = 0;
            foreach (var item in EliminacjaGaussa.operacjeMP1)
            {
                if(item.T > maxTakt)
                    maxTakt = item.T;
            }
            var liczbaEP = EliminacjaGaussa.MP1.Count();
            long liczbaMnozen = EliminacjaGaussa.MP1.Where(x => x.OP == Operacja.MNOZENIE).Count();
            long liczbaDzielen = EliminacjaGaussa.MP1.Where(x => x.OP == Operacja.DZIELENIE).Count();
            float T = (maxTakt + 24 * (2 * (float)EliminacjaGaussa.N - 3)) / 354;
            float P = (float)EliminacjaGaussa.dataFlowElements.Count / (maxTakt * liczbaEP) * 100;
            mp1DetailsDataGridView.Rows.Clear();
            mp1DetailsDataGridView.Rows.Add(new string[] { "Liczba EP", $"{liczbaEP}" });
            mp1DetailsDataGridView.Rows.Add(new string[] { "Czas CPU", stopwatchTimeLabel.Text });
            mp1DetailsDataGridView.Rows.Add(new string[] { "Czas MP1", $"{T.ToString()} \u00B5s" });
            mp1DetailsDataGridView.Rows.Add(new string[] { "Średnie obciążenie EP", $"{P} %" });
            mp1DetailsDataGridView.Rows.Add(new string[] { "Liczba LUT", $"{liczbaMnozen * 1311 + liczbaDzielen * 1175}" });
            mp1DetailsDataGridView.Rows.Add(new string[] { "Liczba DSP", $"{liczbaDzielen * 3}" });
        }

        public void UpdateMP2Table()
        {
            int maxTakt = 0;
            foreach (var item in EliminacjaGaussa.operacjeMP2)
            {
                if (item.T > maxTakt)
                    maxTakt = item.T;
            }
            var liczbaEP = EliminacjaGaussa.MP2.Count();
            long liczbaMnozen = EliminacjaGaussa.MP2.Where(x => x.OP == Operacja.MNOZENIE).Count();
            long liczbaDzielen = EliminacjaGaussa.MP2.Where(x => x.OP == Operacja.DZIELENIE).Count();
            float T = (maxTakt + 24 * (2 * (float)EliminacjaGaussa.N - 1)) / 354;
            float P = (float)EliminacjaGaussa.dataFlowElements.Count / (maxTakt * liczbaEP) * 100;
            mp2DetailsDataGridView.Rows.Clear();
            mp2DetailsDataGridView.Rows.Add(new string[] { "Liczba EP", $"{liczbaEP}" });
            mp2DetailsDataGridView.Rows.Add(new string[] { "Czas CPU", stopwatchTimeLabel.Text });
            mp2DetailsDataGridView.Rows.Add(new string[] { "Czas MP2", $"{T.ToString()} \u00B5s" });
            mp2DetailsDataGridView.Rows.Add(new string[] { "Średnie obciążenie EP", $"{P} %" });
            mp2DetailsDataGridView.Rows.Add(new string[] { "Liczba LUT", $"{liczbaMnozen * 1311 + liczbaDzielen * 1175}" });
            mp2DetailsDataGridView.Rows.Add(new string[] { "Liczba DSP", $"{liczbaDzielen * 3}" });
        }

        public void UpdateMP3Table()
        {
            int maxTakt = 0;
            foreach (var item in EliminacjaGaussa.operacjeMP3)
            {
                if (item.T > maxTakt)
                    maxTakt = item.T;
            }
            var liczbaEP = EliminacjaGaussa.MP3.Count();
            long liczbaMnozen = EliminacjaGaussa.MP3.Where(x => x.OP == Operacja.MNOZENIE).Count();
            long liczbaDzielen = EliminacjaGaussa.MP3.Where(x => x.OP == Operacja.DZIELENIE).Count();
            long taktyMnozenia = liczbaMnozen * 16;
            long taktyDzielenia = liczbaDzielen * 2;
            float T = (maxTakt + 24 * ((float)liczbaEP)) / 354;
            float P = ((float)EliminacjaGaussa.dataFlowElements.Count / ((maxTakt) * liczbaEP)) * 100;
            mp3DetailsDataGridView.Rows.Clear();
            mp3DetailsDataGridView.Rows.Add(new string[] { "Liczba EP", $"{liczbaEP}" });
            mp3DetailsDataGridView.Rows.Add(new string[] { "Czas CPU", stopwatchTimeLabel.Text });
            mp3DetailsDataGridView.Rows.Add(new string[] { "Czas MP3", $"{T.ToString()} \u00B5s" });
            mp3DetailsDataGridView.Rows.Add(new string[] { "Średnie obciążenie EP", $"{P} %" });
            mp3DetailsDataGridView.Rows.Add(new string[] { "Liczba LUT", $"{liczbaMnozen * 1311 + liczbaDzielen * 1175}" });
            mp3DetailsDataGridView.Rows.Add(new string[] { "Liczba DSP", $"{liczbaDzielen * 3}" });
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
            macierzADataGridView.ReadOnly = false;
            macierzMDataGridView.ReadOnly = false;
            macierzAGwiazdkaDataGridView.ReadOnly = false;
            macierzBGwiazdkaDataGridView.ReadOnly = false;

            EliminacjaGaussa.N = int.Parse(nTextBox.Text);
            EliminacjaGaussa.utworzMacierze();
            EliminacjaGaussa.clear();
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
            macierzADataGridView.ReadOnly = true;
            macierzMDataGridView.ReadOnly = true;
            macierzAGwiazdkaDataGridView.ReadOnly = true;
            macierzBGwiazdkaDataGridView.ReadOnly = true;
            EliminacjaGaussa.clear();
            Stopwatch stoper = Stopwatch.StartNew();
            EliminacjaGaussa.oblicz();
            stoper.Stop();
            stopwatchTimeLabel.Text = (stoper.ElapsedMilliseconds.ToString() + " ms");
            if(listCheckbox.Checked)
            {
                macierzAGwiazdkaDataGridView.Refresh();
                macierzBGwiazdkaDataGridView.Refresh();
                macierzMDataGridView.Refresh();
                macierzAGwiazdkaDataGridView.AutoResizeColumns();
                macierzBGwiazdkaDataGridView.AutoResizeColumns();
                macierzMDataGridView.AutoResizeColumns();
                elementDataGridView.AutoResizeColumns();
                EliminacjaGaussa.sortuj();
                UpdateElementList();
                UpdateVertexList();
                UpdateEdgeList();
            }
            
            EliminacjaGaussa.Fs1D = EliminacjaGaussa.mnozeniePrzezD(EliminacjaGaussa.Fs1);
            EliminacjaGaussa.Fs2D = EliminacjaGaussa.mnozeniePrzezD(EliminacjaGaussa.Fs2);
            EliminacjaGaussa.Fs3D = EliminacjaGaussa.mnozeniePrzezD(EliminacjaGaussa.Fs3);
            if(listCheckbox.Checked)
                wypiszMacierze();
            if (EliminacjaGaussa.sprawdzMacierz(EliminacjaGaussa.Fs1) && EliminacjaGaussa.sprawdzMacierz(EliminacjaGaussa.Fs2) && EliminacjaGaussa.sprawdzMacierz(EliminacjaGaussa.Fs3))
            {
                EliminacjaGaussa.wyznaczMacierzProcesorowe();
                if(listCheckbox.Checked)
                {
                    UpdateMP1List();
                    UpdateMP2List();
                    UpdateMP3List();
                }
                UpdateMP1Table();
                UpdateMP2Table();
                UpdateMP3Table();
            }
            else
            {
                MessageBox.Show("Ups coś poszło nie tak");
            }
        }

        private void wczytajAButton_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string linia = string.Empty;
            string[] wiersz = null;

            macierzADataGridView.ReadOnly = true;
            macierzMDataGridView.ReadOnly = true;
            macierzAGwiazdkaDataGridView.ReadOnly = true;
            macierzBGwiazdkaDataGridView.ReadOnly = true;

            EliminacjaGaussa.clear();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\Users\\Cornelius Filmore\\Documents\\MATLAB";
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
                            writer.Write("{0}", EliminacjaGaussa.b_gwiazdka[i]);
                            writer.WriteLine(EliminacjaGaussa.N);
                        }
                    }
                }
            }
        }

        private void wypiszMacierze()
        {
            fs1ValueLabel.Text = "";
            fs2ValueLabel.Text = "";
            fs3ValueLabel.Text = "";
            fs1DValueLabel.Text = "";
            fs2DValueLabel.Text = "";
            fs3DValueLabel.Text = "";
            dValueLabel.Text = "";
            for (int i=0;i<EliminacjaGaussa.Fs1.GetLength(0);i++)
            {
                for (int j = 0; j < EliminacjaGaussa.Fs1.GetLength(1); j++)
                    fs1ValueLabel.Text += EliminacjaGaussa.Fs1[i, j].ToString() + "  ";
                fs1ValueLabel.Text += "\n";
            }

            for (int i = 0; i < EliminacjaGaussa.Fs2.GetLength(0); i++)
            {
                for (int j = 0; j < EliminacjaGaussa.Fs2.GetLength(1); j++)
                    fs2ValueLabel.Text += EliminacjaGaussa.Fs2[i, j].ToString() + "  ";
                fs2ValueLabel.Text += "\n";
            }

            for (int i = 0; i < EliminacjaGaussa.Fs3.GetLength(0); i++)
            {
                for (int j = 0; j < EliminacjaGaussa.Fs3.GetLength(1); j++)
                    fs3ValueLabel.Text += EliminacjaGaussa.Fs3[i, j].ToString() + "  ";
                fs3ValueLabel.Text += "\n";
            }

            for (int i = 0; i < EliminacjaGaussa.Fs1D.GetLength(0); i++)
            {
                for (int j = 0; j < EliminacjaGaussa.Fs1D.GetLength(1); j++)
                    fs1DValueLabel.Text += EliminacjaGaussa.Fs1D[i, j].ToString() + "  ";
                fs1DValueLabel.Text += "\n";
            }

            for (int i = 0; i < EliminacjaGaussa.Fs2D.GetLength(0); i++)
            {
                for (int j = 0; j < EliminacjaGaussa.Fs2D.GetLength(1); j++)
                    fs2DValueLabel.Text += EliminacjaGaussa.Fs2D[i, j].ToString() + "  ";
                fs2DValueLabel.Text += "\n";
            }

            for (int i = 0; i < EliminacjaGaussa.Fs3D.GetLength(0); i++)
            {
                for (int j = 0; j < EliminacjaGaussa.Fs3D.GetLength(1); j++)
                    fs3DValueLabel.Text += EliminacjaGaussa.Fs3D[i, j].ToString() + "  ";
                fs3DValueLabel.Text += "\n";
            }

            for (int i = 0; i < EliminacjaGaussa.D.GetLength(0); i++)
            {
                for (int j = 0; j < EliminacjaGaussa.D.GetLength(1); j++)
                    dValueLabel.Text += EliminacjaGaussa.D[i, j].ToString() + "  ";
                dValueLabel.Text += "\n";
            }

        }
    }
}
