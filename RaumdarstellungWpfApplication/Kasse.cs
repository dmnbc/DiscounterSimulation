using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DiscounterActor_ConsoleApplication
{
    class Kasse  // soll den Umsatz berechnen für jeden Kunden
                 // für den ganzen Tag 
                 // Bestandsabfragen ( pro Regal) ermöglichen
                 // Schwund ermitteln 
    {
        // Attribute
        private double kunde_umsatz;
        private double tages_umsatz;
        private double _schwund;
        private double _warenwert;
        public bool offen;
        private Warenkatalog wk;
        public Label Umsatz;
        public Label schwund_lbl;
        public Label warenwert;

        // Methoden
        public double umsatz
        {
            get
            {
                return tages_umsatz;
            }
            set
            {
                tages_umsatz = value;
                Umsatz.Content = "Tagesumsatz: " + tages_umsatz.ToString("C");
            }
        }

        public double Schwund
        {
            get
            {
                return _schwund;
            }

            set
            {
                _schwund = value;
                schwund_lbl.Content = "Schwund: " + _schwund.ToString("C");
            }
        }

        public double Warenwert
        {
            get
            {
                return _warenwert;
            }

            set
            {
                _warenwert = value;
                warenwert.Content = "Gesamtwert der Regale: " + _warenwert.ToString("C");
            }
        }

        public Kasse(ref Warenkatalog wk)
        {
            offen = true;
            kunde_umsatz = 0.0;
            tages_umsatz = 0.0;
        Console.WriteLine("Kasse ist geöffnet");
        }

        public double kunde_abrechnen( Einkaufszettel ekw )
        {
            kunde_umsatz = 0.0;
            for (int i = 0; i < ekw.liste.Count;i++)
            {
                Console.WriteLine("Artikel {0,3}, {1,3} mal a {2,4:F2} Euro", ekw.liste[i].artikel, ekw.liste[i].anzahl, Warenkatalog.warenkatalog[ekw.liste[i].artikel].art_einzelpreis);
                kunde_umsatz += ekw.liste[i].anzahl * Warenkatalog.warenkatalog[ekw.liste[i].artikel].art_einzelpreis;
            }
            umsatz += kunde_umsatz;     
                   
            return kunde_umsatz; 
        }

        public void nachfuellen_anfordern(ref Discounter_ConsoleApplication.Verkauf v,ref Discounter_ConsoleApplication.Lager l, Lagerist heinz)
        {
            // Auftrag an Personal, die Regale auf der Fehlliste nachzufüllen

            // dazu braucht mann das Resultat von fehlbestand_anzeigen(), einen Lagerspezi der angsprochen wird
            Einkaufszettel auftrag = new Einkaufszettel("Auftrag");            
            Console.WriteLine("Der Lagerist geht zum Lager....");
            auftrag = heinz.wareEntnehmen(l, fehlbestand_anzeigen(v));
            Console.WriteLine("Der Lagerist füllt nach...");
            heinz.wareAuffuellen(v, auftrag);           
        }

        public Einkaufszettel  fehlbestand_anzeigen(Discounter_ConsoleApplication.Raum v)
        {            
            Einkaufszettel fehlliste = new Einkaufszettel("Auftrag");
            
            var zeile = from item in v.regale where item.nachfuellen == true select new {artikel = item.regal_id, anzahl = item.kapazität - item.aktuellerInhalt };
            foreach(var element in zeile)
            {
                fehlliste.liste.Add(new Einkaufszettel.zeile(element.artikel, element.anzahl));
            }    
                       
            return fehlliste;
        }

        public double regalWert(Discounter_ConsoleApplication.Raum v)
        {
            double summe = 0.0;
            var wert = from item in v.regale select item.aktuellerWarenwert;            
         /* foreach(var item in wert)
            {
                summe += item;
            }*/            
            summe = wert.Sum();   // ersetzt die foreach schleife   
            Warenwert = summe;         
            return summe;
        }     
        
        public double schwund(Discounter_ConsoleApplication.Verkauf v, Discounter_ConsoleApplication.Lager l)
        {
            double schwund = 0.0, sumSollWert = 0.0;
            //var sollWertVerkauf = from regal in v.regale select regal.kapazität * regal.artikelpreis;
            //var sollWertLager = from regal in l.regale select regal.kapazität * regal.artikelpreis;
            // direktes abfragen und summe berechnen ohne zwischenspeichern
            sumSollWert = (from regal in v.regale select regal.kapazität * regal.artikelpreis).Sum() + 
                            (from regal in l.regale select regal.kapazität * regal.artikelpreis).Sum();            
            schwund = sumSollWert - regalWert(v) - regalWert(l) - tages_umsatz;
            Schwund = schwund;
            return schwund;
        }
    }
}
/*   for (int i = 0; i < v.regale.Length; i++)
               {
                   if (v.regale[i].nachfuellen == true)
                   {
                       Console.WriteLine(" Regal {0} muss aufgefüllt werden ", i);
                       Console.WriteLine(" Es fehlen zum Maximalbestand {0} Einheiten", v.regale[i].kapazität - v.regale[i].aktuellerInhalt);
                       fehlliste.liste.Add(new Einkaufszettel.zeile() { artikel = i, anzahl = v.regale[i].kapazität - v.regale[i].aktuellerInhalt });
                   }
               } */
