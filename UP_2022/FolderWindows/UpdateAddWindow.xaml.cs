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

            BDell.Visibility = Visibility.Visible;

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
            if (NeededMaterials[0].Image == null)
            {
                IPhoto.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\materials\\picture.png" + NeededMaterials[0].Image, UriKind.RelativeOrAbsolute));                
            }
            else
            {
                IPhoto.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\.." + NeededMaterials[0].Image, UriKind.RelativeOrAbsolute));
            }
            _PhotoPath = IPhoto.Source.ToString();
        }


        public bool Add(int ind)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TBOXTitle.Text) || string.IsNullOrWhiteSpace(TBOXCountInStock.Text) || string.IsNullOrWhiteSpace(TBOXUnit.Text) || string.IsNullOrWhiteSpace(TBOXMinCount.Text) || string.IsNullOrWhiteSpace(TBOXCountInPack.Text) || string.IsNullOrWhiteSpace(TBOXCostPerUnit.Text))
                {
                    return false;
                }
                if (ind < 0)
                {
                    if (Convert.ToDecimal(TBOXCostPerUnit.Text) >= 0 || Convert.ToDouble(TBOXMinCount.Text) >= 0)
                    {
                        return false;
                    }
                    Material TemporaryList = new Material()
                    {
                        Title = TBOXTitle.Text,
                        Unit = TBOXUnit.Text,
                        MinCount = Convert.ToDouble(TBOXMinCount.Text),
                        Description = TBOXDescription.Text,
                        CountInStock = Convert.ToDouble(TBOXCountInStock.Text),
                        CountInPack = Convert.ToInt32(TBOXCountInPack.Text),
                        MaterialTypeID = CBTypeMat.SelectedIndex + 1,
                        Image = _PhotoPath,
                        Cost = Convert.ToDecimal(TBOXCostPerUnit.Text) * Convert.ToDecimal(TBOXCountInStock.Text)
                    };
                    FolderClasses.BD.Data.Material.Add(TemporaryList);
                }
                else
                {
                    materials[ind - 1].Title = TBOXTitle.Text;
                    materials[ind - 1].Unit = TBOXUnit.Text;
                    materials[ind - 1].MinCount = Convert.ToDouble(TBOXMinCount.Text);
                    materials[ind - 1].Description = TBOXDescription.Text;
                    materials[ind - 1].CountInStock = Convert.ToDouble(TBOXCountInStock.Text);
                    materials[ind - 1].CountInPack = Convert.ToInt32(TBOXCountInPack.Text);
                    materials[ind - 1].MaterialTypeID = CBTypeMat.SelectedIndex + 1;
                    materials[ind - 1].Image = _PhotoPath;                 
                    materials[ind - 1].Cost = Convert.ToDecimal(TBOXCostPerUnit.Text) * Convert.ToDecimal(TBOXCountInStock.Text);
                }
                    FolderClasses.BD.Data.SaveChanges();
                    return true;               
            }
            catch (OptimisticConcurrencyException)
            {
                return false;
            }            
        }



        private void BAddOrUp_Click(object sender, RoutedEventArgs e)
        {
            if (!Add(_ind))
            {
                MessageBox.Show("Данные не сохранены в БД", "Ошибка", MessageBoxButton.OK);
            }
            else 
            {
                MessageBox.Show("Данные сохранены в БД", "Сообщение", MessageBoxButton.OK);
            }
            FolderClasses.ChangePropertyClass.listview.Items.Refresh();
            this.Close();
        }

        private void BChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    _PhotoPath = fileDialog.FileName;
                    int index = _PhotoPath.IndexOf("materials");
                    _PhotoPath = _PhotoPath.Substring(index);
                    _PhotoPath = "\\" + _PhotoPath;
                    IPhoto.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\.."+_PhotoPath, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    MessageBox.Show("Не возможно открыть диалоговое окно", "Ошибка", MessageBoxButton.OK);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK);
            }
            
        }

        private void BDell_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MR = MessageBox.Show("вы точно хотите удалить", "Сообщение", MessageBoxButton.YesNo);
            Material Del = FolderClasses.BD.Data.Material.FirstOrDefault(x => x.ID == _ind);
            switch (MR)
            {
                case MessageBoxResult.Yes:
                    FolderClasses.BD.Data.Material.Remove(Del);
                    FolderClasses.BD.Data.SaveChanges();
                    MessageBox.Show("Данные удалены", "Сообщение", MessageBoxButton.OK);
                    FolderClasses.ChangePropertyClass.listview.Items.Refresh();
                    this.Close();
                    break;
                default:
                    break;
            }
        }
    }
}
