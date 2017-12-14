using RaumdarstellungWpfApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Discounter_ConsoleApplication
{
    internal class Regal
    {
        private int _regal_id;
        private double _volumen = 1;
        private Artikel _artikel;
        private int _kapazitaet;
        private int _mindestBestand;
        private bool _nachfuellen;
        private int _aktuellerInhalt;
        private double _aktuellerWarenwert;
        public Label lbl;

        public int regal_id
        {
            get { return _regal_id;  }
        }

        public int aktuellerInhalt
        {
            get
            {
                return _aktuellerInhalt;
            }

            set
            {
                _aktuellerInhalt = value;
            }
        }

        public double aktuellerWarenwert
        {
            get
            {
                return _aktuellerWarenwert;
            }

            set
            {
                _aktuellerWarenwert = value;
            }
        }

        public int mindestbestand
        {
            get { return _mindestBestand; }
        }

        public bool nachfuellen
        {
            get
            {
                return _nachfuellen;
            }

            set
            {
                _nachfuellen = value;
            }
        }
        public int kapazität
        {
            get
            {
                return _kapazitaet;
            }
        }
        public double artikelpreis
        {
            get
            {
                return _artikel.art_einzelpreis;
            }
        }
        
        public Regal()
        {
            Console.WriteLine("Regal erstellt");
        }
        public Regal(int id)
        {
            _regal_id = id;
        }
        public Regal(int id, Artikel[] wk)
        {
            _regal_id = id;
            _artikel = new Artikel(_regal_id,wk);
            _kapazitaet = (int)(_volumen / _artikel.art_volumen);
            _mindestBestand =(int)( _kapazitaet * .3);
            aktuellerInhalt = _kapazitaet;
            nachfuellen = aktuellerInhalt <= _mindestBestand;
            aktuellerWarenwert = aktuellerInhalt * _artikel.art_einzelpreis;
            lbl = new Label();
            lbl.Content = _regal_id.ToString("D3");
            lbl.MouseDown += Lbl_MouseDown;
        }

        private void Lbl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window info = new RaumdarstellungWpfApplication.RegalInfo(_regal_id, _kapazitaet, _aktuellerInhalt, _mindestBestand, _aktuellerWarenwert);
            info.Owner = Application.Current.MainWindow;
            info.Show();
        }
    }
}