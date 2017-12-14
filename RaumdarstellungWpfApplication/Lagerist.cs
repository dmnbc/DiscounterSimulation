using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscounterActor_ConsoleApplication
{
    internal class Lagerist:Personal
    {
        // Attribute
        //Einkaufszettel _auftragsliste;
        double tragkraft = 0.0;

        // Methoden
        public void wareAuffuellen(Discounter_ConsoleApplication.Raum r, Einkaufszettel alt_liste)
        {
            //Einkaufszettel einkaufswagen = new Einkaufszettel("Einkaufswagen");
            double volumen = 0.0;
            for (int i = 0; i < alt_liste.liste.Count; i++)
            {

                Console.WriteLine(" Vom Artikel {0,3} werden {1,3} nachgefüllt", alt_liste.liste[i].artikel, alt_liste.liste[i].anzahl);
                
                r.regale[alt_liste.liste[i].artikel].aktuellerInhalt += alt_liste.liste[i].anzahl;                    // einkaufswagen.liste.Add = // wunsch
                // einkaufswagen.liste.Add(alt_liste.liste[i]);
                
                r.regale[alt_liste.liste[i].artikel].nachfuellen = r.regale[alt_liste.liste[i].artikel].aktuellerInhalt <= r.regale[alt_liste.liste[i].artikel].mindestbestand;
                r.regale[alt_liste.liste[i].artikel].aktuellerWarenwert = r.regale[alt_liste.liste[i].artikel].aktuellerInhalt * r.regale[alt_liste.liste[i].artikel].artikelpreis;
                volumen += alt_liste.liste[i].anzahl * Warenkatalog.warenkatalog[alt_liste.liste[i].artikel].art_volumen;
                
            }
            Console.WriteLine("Da muß ich {0} cbm schleppen!", volumen);
        }
        // Auftragsliste abarbeiten
        // - Artikel aus Lager entnehmen ( dazu muß es ein Lager geben! )
        // - in Verkauf einfüllen 
    }
}