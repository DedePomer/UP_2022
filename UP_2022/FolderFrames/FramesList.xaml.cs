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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace UP_2022.FolderFrames
{
    /// <summary>
    /// Логика взаимодействия для FramesList.xaml
    /// </summary>
    /// 

    public partial class FramesList : Page
    {
        List<Material> StartFilter = FolderClasses.BD.Data.Material.ToList();
        List<Material> FinalFilter ;
        int _UpAndDown = 0;

        public FramesList()
        {
            InitializeComponent();
            LVProductList.ItemsSource = StartFilter;
            FinalFilter = StartFilter;
            CBFilt.SelectedIndex = 0;
            CBSort.SelectedIndex = 0;
            TBLOCKConst.Text = StartFilter.Count + " из ";
        }

        private void CBFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filt();
            LVProductList.Items.Refresh();
        }

        private void CBSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sort();
            LVProductList.Items.Refresh();
        }

        public void filt()
        {
            string SearchStroke = TBOXSearch.Text;
            switch (CBFilt.SelectedIndex)
            {
                case 0:
                    FinalFilter = StartFilter;
                    break;
                case 1:
                    FinalFilter = StartFilter.Where(x => x.MaterialTypeID == 1).ToList();
                    break;
                case 2:
                    FinalFilter = StartFilter.Where(x => x.MaterialTypeID == 2).ToList();
                    break;
                case 3:
                    FinalFilter = StartFilter.Where(x => x.MaterialTypeID == 3).ToList();
                    break;
                case 4:
                    FinalFilter = StartFilter.Where(x => x.MaterialTypeID == 4).ToList();
                    break;
                default:
                    MessageBox.Show("Ошибка", "Ошибка сортировки", MessageBoxButton.OK);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(TBOXSearch.Text))
            {
                FinalFilter = FinalFilter.Where(x => x.Title.Contains(SearchStroke) || x.Title.ToLower().Contains(SearchStroke)
                 || x.Title.ToUpper().Contains(SearchStroke)).ToList();
            }
            if (_UpAndDown == 1)
            {
                sort();
                sortRevers();
            }
            else
            {
                sort();
            }           
            LVProductList.ItemsSource = FinalFilter;
        }

        public void sort()
        { 
                switch (CBSort.SelectedIndex)
                {
                    case 0:
                        FinalFilter.Sort((x, y) => x.Title.CompareTo(y.Title));
                        break;
                    case 1:
                        FinalFilter.Sort((x, y) => x.CountInStock.CompareTo(y.CountInStock));
                        break;
                    case 2:
                        FinalFilter.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                        break;
                    case -1:
                        break;
                    default:
                        MessageBox.Show("Ошибка", "Ошибка сортировки", MessageBoxButton.OK);
                        break;
                }
            TBLOCKChange.Text = FinalFilter.Count + "";
        }

        public void sortRevers()
        {
            FinalFilter.Reverse();
        }

        private void BUp_Click(object sender, RoutedEventArgs e)
        {
            if (_UpAndDown == 1)
            {
                sortRevers();
                _UpAndDown = 0;
                LVProductList.Items.Refresh();
            }
        }

        private void BDown_Click(object sender, RoutedEventArgs e)
        {
            if (_UpAndDown == 0)
            {
                sortRevers();
                _UpAndDown = 1;
                LVProductList.Items.Refresh();
            }    
        }

        private void TBOXSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            filt();
        }

        private void LVProductList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (LVProductList.SelectedIndex != -1)
            {
                BchangeMin.Visibility = Visibility.Visible;
            }
            else
            {
                BchangeMin.Visibility = Visibility.Collapsed;
            }
        }

        
        private void BchangeMin_Click(object sender, RoutedEventArgs e)
        {

            FolderWindows.ChangeMinWindow changeMinWindow = new FolderWindows.ChangeMinWindow();
            changeMinWindow.Show();
        }
    }
}
