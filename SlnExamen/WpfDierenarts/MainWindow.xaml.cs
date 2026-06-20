using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CLDierenarts;

namespace WpfDierenarts
{
    public partial class MainWindow : Window
    {
        private List<Dier> alleDieren = new List<Dier>();
        private List<Eigenaar> alleEigenaars = new List<Eigenaar>();
        private DierValidator validator = new DierValidator();
        private bool isBezigMetLaden = false;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                isBezigMetLaden = true;
                VulKeuzelijsten();
                LaadDieren();
                MaakFormulierLeeg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "De gegevens konden niet geladen worden.\n\n" + ex.Message,
                    "Fout bij het opstarten",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                isBezigMetLaden = false;
            }
        }

        private void VulKeuzelijsten()
        {
            alleEigenaars = Eigenaar.GetAll();

            CmbFilterUrgentie.Items.Clear();
            CmbFilterUrgentie.Items.Add(new ComboBoxItem()
            {
                Content = "Alle urgenties",
                Tag = null
            });

            foreach (Urgentie urgentie in Enum.GetValues(typeof(Urgentie)))
            {
                CmbFilterUrgentie.Items.Add(new ComboBoxItem()
                {
                    Content = urgentie.ToString(),
                    Tag = urgentie
                });
            }
            CmbFilterUrgentie.SelectedIndex = 0;

            CmbFilterEigenaar.Items.Clear();
            CmbFilterEigenaar.Items.Add(new ComboBoxItem()
            {
                Content = "Alle eigenaars",
                Tag = ""
            });

            CmbNieuweEigenaar.Items.Clear();
            foreach (Eigenaar eigenaar in alleEigenaars)
            {
                CmbFilterEigenaar.Items.Add(new ComboBoxItem()
                {
                    Content = eigenaar,
                    Tag = eigenaar.Id
                });

                CmbNieuweEigenaar.Items.Add(new ComboBoxItem()
                {
                    Content = eigenaar,
                    Tag = eigenaar.Id
                });
            }
            CmbFilterEigenaar.SelectedIndex = 0;

            CmbNieuweUrgentie.Items.Clear();
            foreach (Urgentie urgentie in Enum.GetValues(typeof(Urgentie)))
            {
                CmbNieuweUrgentie.Items.Add(new ComboBoxItem()
                {
                    Content = urgentie.ToString(),
                    Tag = urgentie
                });
            }
            CmbNieuweUrgentie.SelectedIndex = 1;

            CmbNieuwType.Items.Clear();
            CmbNieuwType.Items.Add(new ComboBoxItem() { Content = "Hond" });
            CmbNieuwType.Items.Add(new ComboBoxItem() { Content = "Kat" });
            CmbNieuwType.SelectedIndex = 0;
        }

        private void LaadDieren(int teSelecterenId = 0)
        {
            alleDieren = Dier.GetAll();
            PasFiltersToe(teSelecterenId);
        }

        private void PasFiltersToe(int teSelecterenId = 0)
        {
            if (LbxDieren == null)
            {
                return;
            }

            bool filterOpUrgentie = false;
            Urgentie gekozenUrgentie = Urgentie.Normaal;
            ComboBoxItem urgentieItem = CmbFilterUrgentie.SelectedItem as ComboBoxItem;
            if (urgentieItem != null && urgentieItem.Tag != null)
            {
                filterOpUrgentie = true;
                gekozenUrgentie = (Urgentie)urgentieItem.Tag;
            }

            string gekozenEigenaarId = "";
            ComboBoxItem eigenaarItem = CmbFilterEigenaar.SelectedItem as ComboBoxItem;
            if (eigenaarItem != null && eigenaarItem.Tag != null)
            {
                gekozenEigenaarId = eigenaarItem.Tag.ToString();
            }

            bool alleenOpgenomen = ChkAlleenOpgenomen.IsChecked == true;

            LbxDieren.Items.Clear();

            foreach (Dier dier in alleDieren)
            {
                bool tonen = true;

                if (filterOpUrgentie && dier.Urgentie != gekozenUrgentie)
                {
                    tonen = false;
                }

                if (gekozenEigenaarId != "" && dier.EigenaarId != gekozenEigenaarId)
                {
                    tonen = false;
                }

                if (alleenOpgenomen && !dier.IsOpgenomen)
                {
                    tonen = false;
                }

                if (tonen)
                {
                    LbxDieren.Items.Add(dier);
                }
            }

            TxtAantalDieren.Text = LbxDieren.Items.Count == 1
                ? "1 dier"
                : LbxDieren.Items.Count + " dieren";

            if (LbxDieren.Items.Count == 0)
            {
                ToonGeenDetails();
                return;
            }

            int indexTeSelecteren = 0;
            if (teSelecterenId > 0)
            {
                for (int i = 0; i < LbxDieren.Items.Count; i++)
                {
                    Dier dier = (Dier)LbxDieren.Items[i];
                    if (dier.Id == teSelecterenId)
                    {
                        indexTeSelecteren = i;
                        break;
                    }
                }
            }

            LbxDieren.SelectedIndex = indexTeSelecteren;
        }

        private void FilterSelectie_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!isBezigMetLaden)
            {
                PasFiltersToe();
            }
        }

        private void FilterOpgenomen_Click(object sender, RoutedEventArgs e)
        {
            if (!isBezigMetLaden)
            {
                PasFiltersToe();
            }
        }

        private void LbxDieren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dier dier = LbxDieren.SelectedItem as Dier;

            if (dier == null)
            {
                ToonGeenDetails();
                return;
            }

            TxtDetails.Text = dier.GeefInfo();
            TxtDierType.Text = dier.Type;

            if (dier is Hond)
            {
                ImgDier.Source = new BitmapImage(new Uri("Images/hond.png", UriKind.Relative));
            }
            else
            {
                ImgDier.Source = new BitmapImage(new Uri("Images/kat.png", UriKind.Relative));
            }

            BtnDierOpnemen.IsEnabled = !dier.IsOpgenomen;
            BtnDierOpnemen.Content = dier.IsOpgenomen
                ? "Dier is opgenomen"
                : "Dier opnemen";
        }

        private void ToonGeenDetails()
        {
            ImgDier.Source = new BitmapImage(new Uri("Images/poot.png", UriKind.Relative));
            TxtDierType.Text = "Geen selectie";
            TxtDetails.Text = "Selecteer links een dier om alle details te bekijken.";
            BtnDierOpnemen.Content = "Dier opnemen";
            BtnDierOpnemen.IsEnabled = false;
        }

        private void BtnDierOpnemen_Click(object sender, RoutedEventArgs e)
        {
            Dier dier = LbxDieren.SelectedItem as Dier;

            if (dier == null || dier.IsOpgenomen)
            {
                return;
            }

            try
            {
                dier.NeemOp();
                LaadDieren(dier.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Het dier kon niet opgenomen worden.\n\n" + ex.Message,
                    "Opslagfout",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CmbNieuwType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = CmbNieuwType.SelectedItem as ComboBoxItem;
            if (item == null || PnlRas == null || PnlGevaccineerd == null)
            {
                return;
            }

            bool isHond = item.Content.ToString() == "Hond";
            PnlRas.Visibility = isHond ? Visibility.Visible : Visibility.Collapsed;
            PnlGevaccineerd.Visibility = isHond ? Visibility.Collapsed : Visibility.Visible;
            TxtFoutmelding.Text = "";
        }

        private void BtnDierToevoegen_Click(object sender, RoutedEventArgs e)
        {
            string fouten = ValideerFormulier();

            if (fouten != "")
            {
                TxtFoutmelding.Foreground = Brushes.Firebrick;
                TxtFoutmelding.Text = fouten;
                MessageBox.Show(
                    fouten,
                    "Ongeldige invoer",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            ComboBoxItem typeItem = (ComboBoxItem)CmbNieuwType.SelectedItem;
            ComboBoxItem eigenaarItem = (ComboBoxItem)CmbNieuweEigenaar.SelectedItem;
            ComboBoxItem urgentieItem = (ComboBoxItem)CmbNieuweUrgentie.SelectedItem;

            Dier nieuwDier;

            if (typeItem.Content.ToString() == "Hond")
            {
                Hond hond = new Hond();
                hond.Ras = TxtRas.Text.Trim();
                nieuwDier = hond;
            }
            else
            {
                Kat kat = new Kat();
                kat.IsGevaccineerd = ChkGevaccineerd.IsChecked == true;
                nieuwDier = kat;
            }

            Eigenaar eigenaar = (Eigenaar)eigenaarItem.Content;
            double gewicht;
            LeesGewicht(TxtGewicht.Text, out gewicht);

            nieuwDier.Naam = TxtNieuweNaam.Text.Trim();
            nieuwDier.EigenaarId = eigenaar.Id;
            nieuwDier.Eigenaar = eigenaar;
            nieuwDier.Geboortedatum = DtpGeboortedatum.SelectedDate.Value;
            nieuwDier.Gewicht = gewicht;
            nieuwDier.Urgentie = (Urgentie)urgentieItem.Tag;
            nieuwDier.IsOpgenomen = false;
            nieuwDier.DatumOpgenomen = null;

            try
            {
                int nieuwId = nieuwDier.InsertInDb();

                isBezigMetLaden = true;
                CmbFilterUrgentie.SelectedIndex = 0;
                CmbFilterEigenaar.SelectedIndex = 0;
                ChkAlleenOpgenomen.IsChecked = false;
                isBezigMetLaden = false;

                MaakFormulierLeeg();
                TxtFoutmelding.Foreground = Brushes.SeaGreen;
                TxtFoutmelding.Text = $"{nieuwDier.Naam} werd toegevoegd als ticket #{nieuwId}.";
                LaadDieren(nieuwId);
            }
            catch (Exception ex)
            {
                TxtFoutmelding.Foreground = Brushes.Firebrick;
                TxtFoutmelding.Text = "Het dier kon niet opgeslagen worden.";
                MessageBox.Show(
                    "Het dier kon niet opgeslagen worden.\n\n" + ex.Message,
                    "Opslagfout",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private string ValideerFormulier()
        {
            string fouten = "";

            if (CmbNieuwType.SelectedItem == null)
            {
                fouten += "- Kies een type dier.\n";
            }

            if (!validator.IsGeldigeNaam(TxtNieuweNaam.Text))
            {
                fouten += "- De naam mag alleen letters, spaties en koppeltekens bevatten.\n";
            }

            if (CmbNieuweEigenaar.SelectedItem == null)
            {
                fouten += "- Kies een eigenaar.\n";
            }

            if (!DtpGeboortedatum.SelectedDate.HasValue)
            {
                fouten += "- Kies een geboortedatum.\n";
            }
            else if (DtpGeboortedatum.SelectedDate.Value.Date > DateTime.Today)
            {
                fouten += "- De geboortedatum mag niet in de toekomst liggen.\n";
            }

            double gewicht;
            if (!LeesGewicht(TxtGewicht.Text, out gewicht) || gewicht <= 0)
            {
                fouten += "- Geef een geldig gewicht groter dan 0 kg in.\n";
            }

            if (CmbNieuweUrgentie.SelectedItem == null)
            {
                fouten += "- Kies een urgentie.\n";
            }

            ComboBoxItem typeItem = CmbNieuwType.SelectedItem as ComboBoxItem;
            if (typeItem != null && typeItem.Content.ToString() == "Hond")
            {
                if (!validator.IsGeldigRas(TxtRas.Text))
                {
                    fouten += "- Het ras moet minstens " +
                               validator.MinimaalAantalTekensRas +
                               " tekens bevatten.\n";
                }
            }

            return fouten.TrimEnd();
        }

        private bool LeesGewicht(string tekst, out double gewicht)
        {
            if (double.TryParse(tekst, out gewicht))
            {
                return true;
            }

            string aangepast = tekst.Trim().Replace(',', '.');
            return double.TryParse(
                aangepast,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out gewicht);
        }

        private void MaakFormulierLeeg()
        {
            CmbNieuwType.SelectedIndex = 0;
            TxtNieuweNaam.Text = "";
            CmbNieuweEigenaar.SelectedIndex = -1;
            DtpGeboortedatum.SelectedDate = null;
            TxtGewicht.Text = "";
            CmbNieuweUrgentie.SelectedIndex = 1;
            TxtRas.Text = "";
            ChkGevaccineerd.IsChecked = false;
            TxtFoutmelding.Text = "";
            PnlRas.Visibility = Visibility.Visible;
            PnlGevaccineerd.Visibility = Visibility.Collapsed;
        }
    }
}
