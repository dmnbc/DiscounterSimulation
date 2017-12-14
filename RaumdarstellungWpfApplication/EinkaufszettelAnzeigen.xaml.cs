using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RaumdarstellungWpfApplication
{
    /// <summary>
    /// Interaction logic for EinkaufszettelAnzeigen.xaml
    /// </summary>
    public partial class EinkaufszettelAnzeigen : Window
    {
        public EinkaufszettelAnzeigen(int id, DiscounterActor_ConsoleApplication.Einkaufszettel zettel)
        {
            InitializeComponent();
            K_ID.Content = id;
            for(int i = 0; i < zettel.liste.Count; i++)
            {
                Label art = new Label { Content = "Artikel Nr. " + zettel.liste[i].artikel };
                artikeln.Children.Add(art);
                Label anz = new Label { Content = zettel.liste[i].anzahl + " mal" };
                anzahl.Children.Add(anz);
            }
        }
    }
}
