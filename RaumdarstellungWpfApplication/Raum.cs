using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.ConsoleColor;

namespace Discounter_ConsoleApplication
{
    internal class Raum
    {
        protected int _anzahlRegale;
        protected string _bezeichnung;
        protected double _flaeche;
        protected bool _kundenErlaubt;
        protected Regal[] _regale;
        protected int _leereRegale = 0;

        public Raum()
        {
           
        }

        public Raum(string b, double f)
        {
            bezeichnung = b;
            _flaeche = f;
            
        }

        public int anzahlRegale
        {
            get
            {
                return _anzahlRegale;
            }

            set
            {
                _anzahlRegale = value;
            }
        }

        public string bezeichnung
        {
            get
            {
                return _bezeichnung;
            }
            set
            {
                _bezeichnung = value;
            }
        }

        public Regal[] regale
        {
            get
            {
                return _regale;
            }
           
        }

        public int leereRegale
        {
            get
            {
                return _leereRegale;
            }
            set
            {
                _leereRegale = value;
            }
        }

        public void anzeigen(Regal[] r)
        {anzeigen(r, 0, r.Length-1);}

        public void anzeigen(Regal[] r, int x)
        {anzeigen(r, x, x);}

        public void anzeigen(Regal[] r,int x, int y)
        {
         // Display.darstellen(ref r,x,y );
            
        }

        public void anzeigen(ref StackPanel v)
        {
            Label headline = new Label
            {
                Content = _bezeichnung,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = Brushes.BurlyWood,
                Width = 300
            };
            v.Children.Add(headline);
            for (int i = 0; i < 10; i++)
            {
                StackPanel gang = new StackPanel();
                gang.Name = "Gang" + i;
                gang.Orientation = System.Windows.Controls.Orientation.Horizontal;
                if (i % 2 != 0)
                {
                    gang.FlowDirection = FlowDirection.RightToLeft;
                }
                StackPanel gangVor = new StackPanel();
                gangVor.Orientation = System.Windows.Controls.Orientation.Vertical;
                Label leerVorO = new Label { Content = " " }; leerVorO.Background = Brushes.LightGray;
                Label leerVorM = new Label { Content = " ", Name = "GA" + i }; leerVorM.Background = Brushes.LightGray;
                RaumdarstellungWpfApplication.MainWindow.gangbei.Add(leerVorM);
                Label leerVorU = new Label { Content = " " }; leerVorU.Background = Brushes.LightGray;
                gangVor.Children.Add(leerVorO);
                gangVor.Children.Add(leerVorM);
                gangVor.Children.Add(leerVorU);
                gang.Children.Add(gangVor);
                for (int j = 0; j < 40; j++)
                {
                    StackPanel rgr = new StackPanel();
                    rgr.Orientation = System.Windows.Controls.Orientation.Vertical;
                    rgr.Children.Add(regale[(i * 40 + j) * 2].lbl);
                    rgr.Children.Add(new Label { Content = " ", Width = 40, Background = Brushes.LightGray });
                    rgr.Children.Add(regale[((i * 40 + j) * 2) + 1].lbl);

                    gang.Children.Add(rgr);
                }
                StackPanel gangNach = new StackPanel();
                gangNach.Orientation = System.Windows.Controls.Orientation.Vertical;
                Label leerNachO = new Label { Content = " " }; leerNachO.Background = Brushes.LightGray;
                Label leerNachM = new Label { Content = " " }; leerNachM.Background = Brushes.LightGray;
                Label leerNachU = new Label { Content = " " }; leerNachU.Background = Brushes.LightGray;
                gangNach.Children.Add(leerNachO);
                gangNach.Children.Add(leerNachM);
                gangNach.Children.Add(leerNachU);
                gang.Children.Add(gangNach);
                v.Children.Add(gang);
            }
        }
    }
    internal class Verkauf:Raum
    {
        private int _anzahlKunden;

        public Verkauf()
        {
            
        }

        public Verkauf(string b,double f,ref Artikel[] wk):base(b,f)
        {
            _kundenErlaubt = true;
            anzahlRegale = (int)(f / .5);
            _regale = new Regal[wk.Length];
            for (int i = 0; i < wk.Length /*anzahlRegale */; i++)
            { _regale[i] = new Regal(i, wk); }

            /*
            foreach(Regal r in _regale)
            {

                Console.WriteLine("Regal aufgestellt mit einer Kapazität: {0} Einheiten ", r.aktuellerInhalt);
                Console.WriteLine("Das Regal hat einen Wert von {0} Euro\n", r.aktuellerWarenwert);

            } */

        }



        public int anzahlKunden
        {
            get
            {
                return _anzahlKunden;
            }

            set
            {
            }
        }
    }
    internal class Lager : Raum
    {
        public Lager()
        {
            
        }
        public Lager(string b,double f, ref Artikel[] wk) :base(b,f)
        {
            _kundenErlaubt = false;
            anzahlRegale = (int)(f / .5);
            _regale = new Regal[/*wk.Length*/anzahlRegale];            
            for (int i = 0; i < /*wk.Length*/ anzahlRegale ; i++)
            {
                if(i < wk.Length)
                {
                    _regale[i] = new Regal(i, wk);
                    _regale[i].lbl.Background = Brushes.Blue;
                }
                else
                {
                    _regale[i] = new Regal(i);
                }
            }
        }
    }



    
}
