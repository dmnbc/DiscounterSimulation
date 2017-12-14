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
    /// Interaktionslogik für _display.xaml
    /// </summary>
    public partial class RegalInfo : Window
    {
        public RegalInfo(int nr, int kap, int akt_inh, int min, double akt_wert)
        {
            InitializeComponent();
            R_nr.Content = nr;
            kapazität.Content = kap;
            aktuellerInhalt.Content = akt_inh;
            mindestBestand.Content = min;
            aktuellerWarenwert.Content = akt_wert.ToString("C");
            ArtikelEinzelpreis.Content = Warenkatalog.warenkatalog[nr].art_einzelpreis.ToString("C");
            
        }
    }
}
