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
using System.Windows;

namespace DiscounterActor_ConsoleApplication
{
    class Actor
    {
        public struct zeile {
            int art;
            int anz;

            public zeile(int i, int j)
            {
                art = i;
                anz = j;
            }
        }
        static public int lfrNr;
        protected Einkaufszettel _einkaufsliste;
        protected Einkaufszettel _einkaufswagen;
        protected int id;
        protected int rolle;
        protected Image _bild = new Image();
        
        public Image Bild
        {
            get
            {
                return _bild;
            }
        }

        public Einkaufszettel einkaufsliste
        {
            get
            {
                return _einkaufsliste;
            }

            set
            { 

            }
        }

        public Einkaufszettel einkaufswagen
        {
            get
            {
                return _einkaufswagen;
            }

            set
            {
                _einkaufswagen = value;
            }
        }

        public Actor()
        {
            lfrNr++;
            _einkaufsliste = new Einkaufszettel();
            _einkaufswagen = new Einkaufszettel("einkaufswagen");
            _einkaufsliste.liste.Sort();
            Console.WriteLine("Es wurde zuerst ein Einkaufszettel erstellt");

        }

        ~Actor()
        {
            
        }

        public void Liste_zeigen()
        {
            _einkaufsliste.anzeigen();
        }

        public virtual void bezahlen(ref Kasse ks,ref Label lbl)
        {
            Console.WriteLine("Der Actor zahlt für");
           // this.Liste_zeigen();
        }

      //  public void wareEntnehmen(Discounter_ConsoleApplication.Raum r)
        public Einkaufszettel wareEntnehmen(Discounter_ConsoleApplication.Raum r,Einkaufszettel alt_liste)
        {
            Einkaufszettel einkaufswagen = new Einkaufszettel("Einkaufswagen");
            for (int i = 0; i < alt_liste.liste.Count; i++)
            {

                Console.WriteLine(" Auf dem Zettel : Artikel {0,3} soll {1,3} mal gekauft werden", alt_liste.liste[i].artikel, alt_liste.liste[i].anzahl);
                if (r.regale[alt_liste.liste[i].artikel].aktuellerInhalt >= alt_liste.liste[i].anzahl)
                { // genug im Regal
                    Console.WriteLine("genug da");
                    r.regale[alt_liste.liste[i].artikel].aktuellerInhalt -= alt_liste.liste[i].anzahl;  // einkaufswagen.liste.Add = // wunsch

                    einkaufswagen.liste.Add(alt_liste.liste[i]);
                }
                else
                { // zu wenig im Regal, alles was noch da ist
                    //Console.WriteLine("zu wenig da, Regal wird leer gemacht");
                    Console.WriteLine("Es sind noch {0,3} da, Regal wird leer gemacht.", r.regale[alt_liste.liste[i].artikel].aktuellerInhalt);
                    einkaufswagen.liste.Add(new Einkaufszettel.zeile(alt_liste.liste[i].artikel, r.regale[alt_liste.liste[i].artikel].aktuellerInhalt));
                    r.regale[alt_liste.liste[i].artikel].aktuellerInhalt = 0;
                    //  einkaufswagen.liste.Add(new Einkaufszettel.zeile(i, r.regale[alt_liste.liste[i].artikel].aktuellerInhalt));

                }
                //    Console.WriteLine("Im Wagen lfdNr: {0},ArtikelNr:{1}, Anzahl:{2}", i, alt_liste.liste[i].artikel, alt_liste.liste[i].anzahl);
                r.regale[alt_liste.liste[i].artikel].nachfuellen = r.regale[alt_liste.liste[i].artikel].aktuellerInhalt <= r.regale[alt_liste.liste[i].artikel].mindestbestand;
                if(r.regale[alt_liste.liste[i].artikel].nachfuellen) { r.regale[alt_liste.liste[i].artikel].lbl.Background = Brushes.Red ; }
                r.regale[alt_liste.liste[i].artikel].aktuellerWarenwert = r.regale[alt_liste.liste[i].artikel].aktuellerInhalt * r.regale[alt_liste.liste[i].artikel].artikelpreis;

            }
            Console.WriteLine("Im Wagen sind {0} verschiedene Artikel ", einkaufswagen.liste.Count);
            return einkaufswagen;   // wegen Änderung der Rückgabe von void auf Einkaufszettel
        }

        public Einkaufszettel.zeile wareEntnehmenEinzel(Discounter_ConsoleApplication.Verkauf raum, int i)
        {
            Einkaufszettel.zeile temp;
            if (raum.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt >= einkaufsliste.liste[i].anzahl)
            { // genug im Regal                
                raum.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt -= einkaufsliste.liste[i].anzahl;  // einkaufswagen.liste.Add = // wunsch
                temp = einkaufsliste.liste[i];
            }
            else
            { // zu wenig im Regal, alles was noch da ist                
                temp =  new Einkaufszettel.zeile(einkaufsliste.liste[i].artikel, raum.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt);
                raum.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt = 0;
            }
            raum.regale[einkaufsliste.liste[i].artikel].nachfuellen = raum.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt <= raum.regale[einkaufsliste.liste[i].artikel].mindestbestand;
            if (raum.regale[einkaufsliste.liste[i].artikel].nachfuellen)
            {
                raum.regale[einkaufsliste.liste[i].artikel].lbl.Background = Brushes.Red;
                raum.leereRegale++;
            }
            raum.regale[einkaufsliste.liste[i].artikel].aktuellerWarenwert = raum.regale[einkaufsliste.liste[i].artikel].aktuellerInhalt * raum.regale[einkaufsliste.liste[i].artikel].artikelpreis;
            return temp;
        }
    }
}
