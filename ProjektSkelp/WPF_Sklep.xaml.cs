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

namespace ProjektSkelp
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_Sklep.xaml
    /// </summary>
    public partial class WPF_Sklep : Window
    {
        public WPF_Sklep()
        {
            InitializeComponent();

            SklepDBEntities db = new SklepDBEntities();
            var docs = from d in db.Tables
                       select new
                       {
                           NazwaOwocu = d.Nazwa,
                           KrajPochodzenia = d.Kraj_Pochodzenia,
                           Cena =  d.Cena
                       };

            foreach (var item in docs)
            {
                Console.WriteLine(item.NazwaOwocu);
                Console.WriteLine(item.KrajPochodzenia);
                Console.WriteLine(item.Cena);
            }

            this.gridOwoce.ItemsSource = docs.ToList();

        }

        private void btDodaj_Click(object sender, RoutedEventArgs e)
        {
            SklepDBEntities db = new SklepDBEntities();

            Table tableObject = new Table()
            {
                Nazwa = txtName.Text,
                Kraj_Pochodzenia = txtName_Kraj.Text,
                Cena = txtName_Cena.Text
            };

            db.Tables.Add(tableObject);
            db.SaveChanges();

        }

        private void btLoadOwoce_Click(object sender, RoutedEventArgs e)
        {
            SklepDBEntities db = new SklepDBEntities();

            this.gridOwoce.ItemsSource = db.Tables.ToList();
        }

        private int updatingTableID = 0;
        private void gridOwoce_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridOwoce.SelectedIndex >= 0)
            {
                if (this.gridOwoce.SelectedItems.Count >= 0)
                {
                    if (this.gridOwoce.SelectedItems[0].GetType() == typeof(Table)) ;
                    {
                        Table d = (Table)this.gridOwoce.SelectedItems[0];
                        this.txtName_2.Text = d.Nazwa;
                        this.txtName_Kraj_2.Text = d.Kraj_Pochodzenia;
                        this.txtName_Cena_2.Text = d.Cena;

                        this.updatingTableID = d.Id;
                    }
                }
            }
        }

        private void btUpdateOwoce_Click(object sender, RoutedEventArgs e)
        {
            SklepDBEntities db = new SklepDBEntities();

            var r = from d in db.Tables
                where d.Id == this.updatingTableID
                select d;

            Table obj = r.SingleOrDefault();


            if(obj != null)
            {
                obj.Nazwa = this.txtName_2.Text;
                obj.Kraj_Pochodzenia = this.txtName_Kraj_2.Text;
                obj.Cena = this.txtName_Cena_2.Text;
                
                db.SaveChanges();
            }

        }

        private void btDeleteOwoce_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult msgBoxResult = MessageBox.Show("Czy jesteś pewny, że chesz usunąć ten owoc?",
                "Owoc został usunięty",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No
                );

            if (msgBoxResult == MessageBoxResult.Yes)
            {

                SklepDBEntities db = new SklepDBEntities();

                var r = from d in db.Tables
                        where d.Id == this.updatingTableID
                        select d;

                Table obj = r.SingleOrDefault();


                if (obj != null)
                {
                    db.Tables.Remove(obj);
                    db.SaveChanges();
                }
            }
        }
    }
}
