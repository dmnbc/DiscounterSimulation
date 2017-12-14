using DiscounterActor_ConsoleApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RaumdarstellungWpfApplication
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Label> gangbei = new List<Label>(); // soll das Abbiegen in die Gänge steuern
        public static List<double> gangbeiY = new List<double>();
        public static List<double> gangbeiX = new List<double>();
        Discounter_ConsoleApplication.Verkauf verkaufsraum = new Discounter_ConsoleApplication.Verkauf("VERKAUF", 400, ref Warenkatalog.warenkatalog);
        Discounter_ConsoleApplication.Lager lager = new Discounter_ConsoleApplication.Lager("Lager", 400, ref Warenkatalog.warenkatalog);
        List<DiscounterActor_ConsoleApplication.Kunde> meineKunden = new List<Kunde>(); 
        static Warenkatalog wk = new Warenkatalog();
        DiscounterActor_ConsoleApplication.Kasse kasse = new Kasse(ref wk);

        public MainWindow()
        {
            InitializeComponent();
            raum.Children.Remove(Lager); // Das Lager wurde nur im XAML erstellt damit es den gleichen Style hat Wie Verkauf
            // Das Objekt verkaufsraum im Fenster anzeigen
            verkaufsraum.anzeigen(ref Verkauf);
            for (int i = 0; i < 5; i++)
            {
                Kunde kunde = new Kunde();
                meineKunden.Add(kunde);
                canvas.Children.Add(kunde.Bild);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double x = 0.0;
            for(int i= 0; i < gangbei.Count; i++)
            {
                Point relativePoint;
                if (i % 2 == 0)
                {
                    relativePoint = gangbei[i].TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
                }
                else
                {
                    relativePoint = gangbei[i].TransformToAncestor(Application.Current.MainWindow).Transform(new Point(gangbei[i].Width, 0));
                }
                Ellipse kundenAlsKreis = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Black                                                       
                };
                canvas.Children.Add(kundenAlsKreis);
                Canvas.SetTop(kundenAlsKreis, relativePoint.Y);
                Canvas.SetLeft(kundenAlsKreis, relativePoint.X);
                gangbeiY.Add(relativePoint.Y);
                gangbeiX.Add(relativePoint.X);
                x = relativePoint.X;
            }
            kasse.schwund_lbl = Schwund;
            kasse.warenwert = gesamtWert;
            kasse.Umsatz = Umsatz;
            gesamtWert.Content = "Gesamtwert der Regale: " + kasse.regalWert(verkaufsraum).ToString("C");
            Schwund.Content = "Schwund: " + kasse.schwund(verkaufsraum, lager).ToString("C");
            // Lager erstellen, noch nicht anzeigen
            lager = new Discounter_ConsoleApplication.Lager("LAGER", 400, ref Warenkatalog.warenkatalog);
            lager.anzeigen(ref Lager);
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            foreach(Kunde kunde in meineKunden)
            {
                ActorAnimiert animation = new ActorAnimiert(ref pause, ref weiter, ref kasse, verkaufsraum, lager);
                animation.gehEinkaufen( ref bottom, kunde );
            }                                              
        }

        private void headerVerkauf_Click(object sender, RoutedEventArgs e)
        {
            raum.Children.RemoveAt(raum.Children.Count - 1);
            raum.Children.Add(Verkauf);
        }

        private void headerLager_Click(object sender, RoutedEventArgs e)
        {
            raum.Children.RemoveAt(raum.Children.Count - 1);
            raum.Children.Add(Lager);
        }

        //public Einkaufszettel wareEntnehmen(Discounter_ConsoleApplication.Raum r)
        //{
        //    DoubleAnimation suchen = new DoubleAnimation
        //    {
        //        By = 40,
        //        Duration = TimeSpan.Parse("0:0:1")
        //    };
        //    for (int i = 0; i < Warenkatalog.warenkatalog.Count(); i++)
        //    {
        //        if (i == einkaufsliste.liste[i].artikel)
        //        {
        //            if (r.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt >= einkaufsliste.liste[i].anzahl)
        //            {
        //                r.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt -= einkaufsliste.liste[i].anzahl;  // einkaufswagen.liste.Add = // wunsch

        //                einkaufswagen.liste.Add(einkaufsliste.liste[i]);
        //            }
        //            else
        //            {
        //                einkaufswagen.liste.Add(new Einkaufszettel.zeile(einkaufsliste.liste[i].artikel, r.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt));
        //                r.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt = 0;
        //            }
        //            r.regale[einkaufsliste.liste[i].artikel].nachfuellen = r.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt <= r.regale[einkaufsliste.liste[i].artikel].mindestbestand;
        //            if (r.regale[einkaufsliste.liste[i].artikel].nachfuellen) { r.regale[einkaufsliste.liste[i].artikel].lbl.Background = Brushes.Red; }
        //            r.regale[einkaufsliste.liste[i].artikel].aktuellerWarenwert = r.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt * r.regale[einkaufsliste.liste[i].artikel].artikelpreis;
        //        }
        //        if (i + 1 == einkaufsliste.liste[i + 1].artikel)
        //        {
        //            if (r.regale[einkaufsliste.liste[i + 1].artikel].aktuellerInhalt >= einkaufsliste.liste[i + 1].anzahl)
        //            {
        //                r.regale[einkaufsliste.liste[i + 1].artikel].aktuellerInhalt -= einkaufsliste.liste[i + 1].anzahl;  // einkaufswagen.liste.Add = // wunsch

        //                einkaufswagen.liste.Add(einkaufsliste.liste[i + 1]);
        //            }
        //            else
        //            {
        //                einkaufswagen.liste.Add(new Einkaufszettel.zeile(einkaufsliste.liste[i + 1].artikel, r.regale[einkaufsliste.liste[i + 1].artikel].aktuellerInhalt));
        //                r.regale[einkaufsliste.liste[i + 1].artikel].aktuellerInhalt = 0;
        //            }
        //            r.regale[einkaufsliste.liste[i + 1].artikel].nachfuellen = r.regale[einkaufsliste.liste[i + 1].artikel].aktuellerInhalt <= r.regale[einkaufsliste.liste[i + 1].artikel].mindestbestand;
        //            if (r.regale[einkaufsliste.liste[i + 1].artikel].nachfuellen) { r.regale[einkaufsliste.liste[i + 1].artikel].lbl.Background = Brushes.Red; }
        //            r.regale[einkaufsliste.liste[i + 1].artikel].aktuellerWarenwert = r.regale[einkaufsliste.liste[i + 1].artikel].aktuellerInhalt * r.regale[einkaufsliste.liste[i + 1].artikel].artikelpreis;
        //        }
        //        if (i % 40 == 0)
        //        {
        //            DoubleAnimation runter = new DoubleAnimation
        //            {
        //                By = 90,
        //                Duration = TimeSpan.Parse("0:0:1")
        //            };
        //            runter.Completed += Runter_Completed;

        //        }
        //    }


        //    return einkaufswagen;
        //}

        //private void Runter_Completed(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
