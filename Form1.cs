using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace Kaenen
{
    public partial class Form1 : Form
    {
        private delegate double MetrykaDelegate(double[] a, double[] b);
        private double[][] dane;
        private string[] etykiety;
        private int k = 3;
        private bool daneZnormalizowane = false;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            btnWczytaj.Click += BtnWczytaj_Click;
            btnOblicz.Click += BtnOblicz_Click;
            btnNormalizuj.Click += BtnNormalizuj_Click;
            nudK.ValueChanged += NudK_ValueChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var metody = typeof(Form1).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(m => m.ReturnType == typeof(double) &&
                           m.GetParameters().Length == 2 &&
                           m.GetParameters()[0].ParameterType == typeof(double[]) &&
                           m.GetParameters()[1].ParameterType == typeof(double[]))
                .ToArray();

            cmbMetryki.DataSource = metody;
            cmbMetryki.DisplayMember = "Name";
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOUSEMENU = 0xF090; // Kliknięcie ikony okna
            const int SC_CLOSE = 0xF060; // Zamknięcie okna (podwójne kliknięcie na ikonie)

            if (m.Msg == WM_SYSCOMMAND)
            {
                int command = m.WParam.ToInt32() & 0xFFF0;

                if (command == SC_MOUSEMENU)
                {
                    return; // Blokuje tylko menu systemowe na ikonie
                }

                if (command == SC_CLOSE && Control.MousePosition.X < this.Left + 40)
                {
                    return; // Blokuje podwójne kliknięcie na ikonie, ale nie blokuje normalnego "X"
                }
            }

            base.WndProc(ref m);
        }

        private void BtnWczytaj_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Pliki tekstowe (*.txt)|*.txt";
                dlg.Title = "Wybierz plik z danymi";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    WczytajDane(dlg.FileName);
                    daneZnormalizowane = false;
                    btnNormalizuj.Enabled = true;
                }
            }
        }

        private void WczytajDane(string sciezka)
        {
            try
            {
                string[] linie = File.ReadAllLines(sciezka);
                dane = new double[linie.Length][];
                etykiety = new string[linie.Length];

                for (int i = 0; i < linie.Length; i++)
                {
                    string[] czesci = linie[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    etykiety[i] = czesci[czesci.Length - 1];
                    dane[i] = new double[czesci.Length - 1];

                    for (int j = 0; j < czesci.Length - 1; j++)
                    {
                        if (double.TryParse(czesci[j].Replace('.', ','), out double wartosc))
                            dane[i][j] = wartosc;
                    }
                }

                lblStatus.Text = $"Wczytano {dane.Length} rekordów, {dane[0].Length} cech";
                btnOblicz.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd wczytywania: {ex.Message}", "Błąd");
            }
        }

        private void BtnNormalizuj_Click(object sender, EventArgs e)
        {
            if (dane == null || dane.Length == 0)
            {
                MessageBox.Show("Najpierw wczytaj dane!", "Błąd");
                return;
            }

            NormalizujDane();
            daneZnormalizowane = true;
            btnNormalizuj.Enabled = false;
            lblStatus.Text = $"Dane znormalizowane. {dane.Length} rekordów, {dane[0].Length} cech";
        }

        private void NormalizujDane()
        {
            int liczbaCech = dane[0].Length;

            for (int i = 0; i < liczbaCech; i++)
            {
                double min = dane.Min(x => x[i]);
                double max = dane.Max(x => x[i]);
                double roznica = max - min;

                if (roznica == 0) continue;

                for (int j = 0; j < dane.Length; j++)
                {
                    dane[j][i] = (dane[j][i] - min) / roznica;
                }
            }
        }

        private void NudK_ValueChanged(object sender, EventArgs e)
        {
            k = (int)nudK.Value;
        }

        private void BtnOblicz_Click(object sender, EventArgs e)
        {
            if (dane == null || dane.Length < 2)
            {
                MessageBox.Show("Najpierw wczytaj dane!", "Błąd");
                return;
            }

            if (!daneZnormalizowane && MessageBox.Show("Dane nie zostały znormalizowane. Czy chcesz kontynuować?",
                                                   "Ostrzeżenie",
                                                   MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            MethodInfo wybranaMetoda = cmbMetryki.SelectedItem as MethodInfo;
            if (wybranaMetoda == null) return;

            int poprawneKlasyfikacje = 0;
            var sb = new StringBuilder();

            sb.AppendLine("=== WALIDACJA 1 vs RESZTA ===");
            sb.AppendLine($"Metryka: {wybranaMetoda.Name.Replace("Metryka", "")}");
            sb.AppendLine($"Wartość k: {k}");
            sb.AppendLine($"Liczba próbek: {dane.Length}");
            sb.AppendLine();

            for (int i = 0; i < dane.Length; i++)
            {
                string przewidzianaKlasa = KlasyfikujKNn(dane[i], wybranaMetoda, i);
                bool poprawna = przewidzianaKlasa == etykiety[i];

                if (poprawna) poprawneKlasyfikacje++;

                sb.AppendLine($"Próbka {i + 1,3}: {(poprawna ? "OK" : "NG")} | " +                  // NG - Not Good  
                              $"Rzeczywista: {etykiety[i],-9} Przewidziana: {przewidzianaKlasa}");
            }

            double dokladnosc = (double)poprawneKlasyfikacje / dane.Length * 100;
            sb.AppendLine();
            sb.AppendLine("=== PODSUMOWANIE ===");
            sb.AppendLine($"Poprawne klasyfikacje: {poprawneKlasyfikacje}/{dane.Length}");
            sb.AppendLine($"Dokładność: {dokladnosc:F2}%");

            txtWyniki.Text = sb.ToString();
        }

        private string KlasyfikujKNn(double[] probkaTestowa, MethodInfo metoda, int indeksPomin)
        {
            var odleglosci = new List<Tuple<double, string>>();

            for (int i = 0; i < dane.Length; i++)
            {
                if (i == indeksPomin) continue;

                double odleglosc = (double)metoda.Invoke(this, new object[] { probkaTestowa, dane[i] });
                odleglosci.Add(new Tuple<double, string>(odleglosc, etykiety[i]));
            }

            var kNajblizszych = odleglosci
                .OrderBy(x => x.Item1)
                .Take(k)
                .GroupBy(x => x.Item2)
                .OrderByDescending(g => g.Count())
                .ToList();

            return kNajblizszych.Count == 1 || kNajblizszych[0].Count() > kNajblizszych[1].Count()
                   ? kNajblizszych[0].Key
                   : "REMIS";
        }

        // Metryki
        private double MetrykaEuklidesowa(double[] a, double[] b)
        {
            double suma = 0.0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
                suma += Math.Pow(a[i] - b[i], 2.0);
            return Math.Sqrt(suma);
        }

        private double MetrykaManhatanska(double[] a, double[] b)
        {
            double suma = 0.0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
                suma += Math.Abs(a[i] - b[i]);
            return suma;
        }

        private double MetrykaMinkowskiego(double[] a, double[] b)
        {
            double suma = 0.0;
            double p = k;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
                suma += Math.Pow(Math.Abs(a[i] - b[i]), p);
            return Math.Pow(suma, 1.0 / p);
        }

        private double MetrykaCzebyszewa(double[] a, double[] b)
        {
            double maks = 0.0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
                maks = Math.Max(maks, Math.Abs(a[i] - b[i]));
            return maks;
        }
    }
}