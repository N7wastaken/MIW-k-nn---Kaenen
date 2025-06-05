using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Kaenen
{
    public partial class Form1 : Form
    {
        private delegate double MetrykaDelegate(double[] a, double[] b);
        private double[][] dane;
        private string[] etyk;
        private int k = 3;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            btnWczytaj.Click += BtnWczytaj_Click;
            btnOblicz.Click += BtnOblicz_Click;
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

        private void BtnWczytaj_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Pliki tekstowe (*.txt)|*.txt";
                dlg.Title = "Wybierz plik z danymi";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    WczytajDane(dlg.FileName);
                }
            }
        }

        private void WczytajDane(string sciezka)
        {
            try
            {
                string[] linie = File.ReadAllLines(sciezka);
                dane = new double[linie.Length][];
                etyk = new string[linie.Length];

                for (int i = 0; i < linie.Length; i++)
                {
                    string[] czesci = linie[i].Split(' ');
                    etyk[i] = czesci[czesci.Length - 1];
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

            MethodInfo wybMetoda = cmbMetryki.SelectedItem as MethodInfo;
            if (wybMetoda == null) return;

            string wynik = $"═══ WYNIKI OBLICZEŃ - {wybMetoda.Name.Replace("Metryka", "")} (k={k}) ═══\n\n";

            var klasy = new[] { "1", "2", "3" };
            foreach (string klasa in klasy)
            {
                wynik += $"▬▬▬ KLASA {klasa} ▬▬▬\n";
                var objKlasy = dane.Select((d, i) => new { dane = d, idx = i, etyk = etyk[i] })
                                  .Where(x => x.etyk == klasa)
                                  .Take(k)
                                  .ToArray();

                for (int i = 0; i < objKlasy.Length - 1; i++)
                {
                    for (int j = i + 1; j < objKlasy.Length; j++)
                    {
                        double odl = (double)wybMetoda.Invoke(this, new object[] { objKlasy[i].dane, objKlasy[j].dane });
                        wynik += $"  Obj[{objKlasy[i].idx + 1:D3}] ↔ Obj[{objKlasy[j].idx + 1:D3}]: {odl:F4}\n";
                    }
                }
                wynik += "\n";
            }

            wynik += $"▬▬▬ MIĘDZYKLASOWE (pierwsze {k * 2}) ▬▬▬\n";
            var obj1 = dane.Select((d, i) => new { dane = d, idx = i, etyk = etyk[i] })
                          .Where(x => x.etyk == "1").Take(k).ToArray();
            var obj2 = dane.Select((d, i) => new { dane = d, idx = i, etyk = etyk[i] })
                          .Where(x => x.etyk == "2").Take(k).ToArray();
            var obj3 = dane.Select((d, i) => new { dane = d, idx = i, etyk = etyk[i] })
                          .Where(x => x.etyk == "3").Take(k).ToArray();

            int licz = 0;
            foreach (var o1 in obj1)
            {
                foreach (var o2 in obj2)
                {
                    if (licz++ >= k * 2) break;
                    double odl = (double)wybMetoda.Invoke(this, new object[] { o1.dane, o2.dane });
                    wynik += $"  Kl1[{o1.idx + 1:D3}] ↔ Kl2[{o2.idx + 1:D3}]: {odl:F4}\n";
                }
                if (licz >= k * 2) break;
            }

            txtWyniki.Text = wynik;
        }

        private double MetrykaEuklidesowa(double[] a, double[] b)
        {
            double suma = 0.0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                suma += Math.Pow(a[i] - b[i], 2.0);
            }
            return Math.Sqrt(suma);
        }

        private double MetrykaManhatanska(double[] a, double[] b)
        {
            double suma = 0.0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                suma += Math.Abs(a[i] - b[i]);
            }
            return suma;
        }

        private double MetrykaMinkowskiego(double[] a, double[] b)
        {
            double suma = 0.0;
            double p = k;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                suma += Math.Pow(Math.Abs(a[i] - b[i]), p);
            }
            return Math.Pow(suma, 1.0 / p);
        }

        private double MetrykaCzebyszewa(double[] a, double[] b)
        {
            double maks = 0.0;
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                double rozn = Math.Abs(a[i] - b[i]);
                if (rozn > maks) maks = rozn;
            }
            return maks;
        }
    }
}