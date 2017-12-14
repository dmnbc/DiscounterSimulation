using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RaumdarstellungWpfApplication;
using System.Windows;
using System.Windows.Media.Animation;
using Discounter_ConsoleApplication;
using System.Threading;

namespace DiscounterActor_ConsoleApplication
{
    class Kunde : Actor
    {
        public int ID
        {
            get
            {
                return id;
            }
        }

        public Kunde()
        {
            this.id = Actor.lfrNr;
            _bild.Source = new BitmapImage(new Uri("shopping-cart.png",UriKind.Relative));
            Canvas.SetTop(_bild, 10);
            Canvas.SetLeft(_bild, 10);
            _bild.Width = 26;
            _bild.Height = 26;
            _bild.MouseDown += einkaufszettelAnzeigen;
        }

        private void einkaufszettelAnzeigen(object sender, MouseButtonEventArgs e)
        {
            Window zeigeZettel = new EinkaufszettelAnzeigen(id, einkaufsliste);
            zeigeZettel.Owner = Application.Current.MainWindow;
            zeigeZettel.Show();
        }

        public override void bezahlen(ref Kasse ks,ref Label lbl)
        {
            Console.WriteLine("Der Kunde {0} zahlt den offiziellen Preis ",id);
            Console.WriteLine("für {0} Artikel ", einkaufswagen.liste.Count);
           
            lbl.Content = "Der Kunde " + id + " zahlt den offiziellen Preis von " + ks.kunde_abrechnen(einkaufswagen) + " Euro";
        }

         ~Kunde()
        {
        }
    }
}
