using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using RaumdarstellungWpfApplication;
using Discounter_ConsoleApplication;
using DiscounterActor_ConsoleApplication;
using System.Windows;
using System.Threading;

namespace RaumdarstellungWpfApplication
{
    class ActorAnimiert
    {
        protected AnimationClock _taktgeber = null;
        protected AnimationClock _taktquer = null;
        protected double ersterGang;
        protected double regalBreite = MainWindow.gangbei[0].Width;
        protected double nextRegal;
        protected int regalNr = 0;
        protected int posten = 0;
        protected bool nachRechts = true;
        protected Kasse kasse;
        protected Label bottom;
        protected Verkauf verkauf;
        protected Lager lager;
        protected Kunde kunde;

        public ActorAnimiert(ref Button pause, ref Button weiter, ref Kasse k, Verkauf r, Lager _lager)
        {
            pause.Click += Pause_Click;
            weiter.Click += Weiter_Click;
            kasse = k;
            verkauf = r;
            lager = _lager;
        }

        public void gehEinkaufen( ref Label l, Kunde _kunde )
        {            
            bottom = l;
            kunde = _kunde;
            ersterGang = MainWindow.gangbeiY[0];
            nextRegal = MainWindow.gangbeiX[0] + regalBreite;
            DoubleAnimation x1 = new DoubleAnimation();
            DoubleAnimation y = new DoubleAnimation();
            x1.To = MainWindow.gangbeiX[0];
            y.To = MainWindow.gangbeiY[MainWindow.gangbeiY.Count - 1] + 90;
            x1.Duration = TimeSpan.Parse("0:0:2");
            x1.BeginTime = TimeSpan.Parse("0:0:" + kunde.ID);
            y.Duration = TimeSpan.Parse("0:0:10");
            y.BeginTime = TimeSpan.Parse("0:0:" + (kunde.ID + 2));
            _taktgeber = y.CreateClock();
            _taktgeber.CurrentTimeInvalidated += _taktgeber_CurrentTimeInvalidated;
            _taktgeber.Completed += _taktgeber_Completed;
            kunde.Bild.ApplyAnimationClock(Canvas.TopProperty, _taktgeber);
            kunde.Bild.BeginAnimation(Canvas.LeftProperty, x1);
        }


        protected void _taktgeber_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            double aktuellePosition;
            Double.TryParse((kunde.Bild.GetValue(Canvas.TopProperty).ToString()), out aktuellePosition);

            if (aktuellePosition > ersterGang)
            {
                _taktgeber.Controller.Pause();
                durchdengang();
            }
        }

        private void durchdengang()
        {
            double rorl = 1600;
            ersterGang = ersterGang + MainWindow.gangbei[0].Height * 3;

            Point aufenthalt = kunde.Bild.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            if (aufenthalt.X > 1500)
            {
                rorl = MainWindow.gangbeiX[0];
                nachRechts = false;
            }
            else
            {
                rorl = MainWindow.gangbeiX[1];
                nachRechts = true;
            }
            DoubleAnimation quer = new DoubleAnimation();
            quer.To = rorl;
            quer.Duration = TimeSpan.Parse("0:0:25");
            _taktquer = quer.CreateClock();
            _taktquer.CurrentTimeInvalidated += _taktquer_CurrentTimeInvalidated;
            _taktquer.Completed += _taktquer_Completed; //(s, x) => _taktgeber.Controller.Resume();
            kunde.Bild.ApplyAnimationClock(Canvas.LeftProperty, _taktquer);
        }

        private void _taktquer_CurrentTimeInvalidated(object sender, EventArgs e)
        {

            double position, a, b;
            double.TryParse(kunde.Bild.GetValue(Canvas.LeftProperty).ToString(), out position);

            if (nachRechts)
            {
                a = position;
                b = nextRegal;
            }
            else
            {
                b = position;
                a = nextRegal;
            }

            if (a > b)
            {
                if(posten < kunde.einkaufsliste.liste.Count())
                {
                    if (kunde.einkaufsliste.liste[posten].artikel == regalNr)
                    {
                        _taktquer.Controller.Pause();
                        kunde.einkaufswagen.liste.Add(kunde.wareEntnehmenEinzel(verkauf, posten));
                        wareEntnehmenAnimation();
                        posten++;
                    }
                }
                if (posten < kunde.einkaufsliste.liste.Count())
                {
                    if (kunde.einkaufsliste.liste[posten].artikel == regalNr + 1)
                    {
                        _taktquer.Controller.Pause();
                        kunde.einkaufswagen.liste.Add(kunde.wareEntnehmenEinzel(verkauf, posten));
                        wareEntnehmenAnimation();
                        posten++;
                    }
                }
                regalNr += 2;
                nextRegal += regalBreite;
            }
        }

        private void wareEntnehmenAnimation()
        {
            DoubleAnimation drehen = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.Parse("0:0:2")
            };
            drehen.Completed += (s, x) => _taktquer.Controller.Resume();
            RotateTransform rotate = new RotateTransform();
            kunde.Bild.RenderTransform = rotate;
            kunde.Bild.RenderTransformOrigin = new Point(0.5, 0.5);
            rotate.BeginAnimation(RotateTransform.AngleProperty, drehen);
        }

        private void _taktquer_Completed(object sender, EventArgs e)
        {
            regalBreite *= -1;
            _taktgeber.Controller.Resume();
        }


        private void _taktgeber_Completed(object sender, EventArgs e)
        {
            kunde.bezahlen(ref kasse, ref bottom);
            kasse.regalWert(verkauf);
            kasse.schwund(verkauf, lager);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _taktquer.Controller.Pause();
        }

        private void Weiter_Click(object sender, RoutedEventArgs e)
        {
            _taktquer.Controller.Resume();
        }

    }
}
