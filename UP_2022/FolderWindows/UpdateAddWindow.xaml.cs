using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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

namespace UP_2022.FolderWindows
{
    /// <summary>
    /// Логика взаимодействия для UpdateAddWindow.xaml
    /// </summary>
    public partial class UpdateAddWindow : Window
    {
        int _ind =-1;
        List<MaterialType> materialTypesList = FolderClasses.BD.Data.MaterialType.ToList();
        List<Material> materials = FolderClasses.BD.Data.Material.ToList();
        string _PhotoPath = null;

        public void AlwaysWork() // метод, который будет выполнятся в обоих конструкторах
        {            
            for (int i = 0; i < materialTypesList.Count; i++)
            {
                CBTypeMat.Items.Add(materialTypesList[i].Title);
            }
            if (_ind != -1)
            {
                BAddOrUp.Content = "Редактировать";
            }
            else
            {
                BAddOrUp.Content = "Добавить";
            }
        }
        public UpdateAddWindow()
        {
             InitializeComponent();
             AlwaysWork();
        }

        public UpdateAddWindow(int ind)
        {
            InitializeComponent();
            _ind = ind;
            AlwaysWork();

            List<Material> NeededMaterials;
            NeededMaterials = materials.Where(x => x.ID == _ind).ToList();
            TBOXTitle.Text = NeededMaterials[0].Title +"";
            TBOXUnit.Text = NeededMaterials[0].Unit+ "";
            TBOXMinCount.Text = NeededMaterials[0].MinCount + "";
            TBOXDescription.Text = NeededMaterials[0].Description + "";
            TBOXCountInStock.Text = NeededMaterials[0].CountInStock + "";
            TBOXCountInPack.Text = NeededMaterials[0].CountInPack + "";
            CBTypeMat.SelectedIndex = NeededMaterials[0].MaterialTypeID-1;
            TBOXCostPerUnit.Text = Convert.ToDouble(NeededMaterials[0].Cost)/ Convert.ToDouble(NeededMaterials[0].CountInStock)+ "";
            IPhoto.Source = new BitmapImage(new Uri("UP_2022\\" + NeededMaterials[0].Image, UriKind.RelativeOrAbsolute));/*\UP_2022\materials\material_9.jpeg*/
        }

        public void Add()
        {
            try
            {
                Material TemporaryList = new Material()
                {
                    Title = TBOXTitle.Text,
                    Unit = TBOXUnit.Text,
                    MinCount = Convert.ToDouble(TBOXMinCount.Text),
                    Description = TBOXDescription.Text,
                    CountInStock = Convert.ToDouble(TBOXCountInStock.Text),
                    CountInPack = Convert.ToInt32(TBOXCountInPack.Text),
                    MaterialTypeID = CBTypeMat.SelectedIndex + 1,
                    Image = null,
                    Cost = Convert.ToDecimal(TBOXCostPerUnit.Text) * Convert.ToDecimal(TBOXCountInStock.Text)
                };
                FolderClasses.BD.Data.Material.Add(TemporaryList);
                FolderClasses.BD.Data.SaveChanges();
                MessageBox.Show("Данные сохранены в БД", "Сообщение", MessageBoxButton.OK);
            }
            catch (OptimisticConcurrencyException)
            {
                MessageBox.Show("Данные не сохранены в БД", "Ошибка", MessageBoxButton.OK);
            }
            
        }



        private void BAddOrUp_Click(object sender, RoutedEventArgs e)
        {
            Add();
        }

        private void BChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                _PhotoPath = fileDialog.FileName;
                int index = _PhotoPath.IndexOf("materials");
                _PhotoPath = _PhotoPath.Substring(index);
            }
            else
            {
                MessageBox.Show("Не возможно открыть диалоговое окно", "Ошибка", MessageBoxButton.OK);
            }

        }
    }
}
