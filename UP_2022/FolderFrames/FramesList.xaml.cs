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
using Word = Microsoft.Office.Interop.Word;


namespace UP_2022.FolderFrames
{

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
            FolderClasses.ChangePropertyClass.listview = LVProductList;
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
            if (FinalFilter.Count ==0)
            {
                MessageBox.Show("Элементов 0", "Сообщение", MessageBoxButton.OK);
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
            List<Material> OurList = new List<Material>();
            foreach (Material item in LVProductList.SelectedItems)
            {
                OurList.Add(item);
            }
            FolderWindows.ChangeMinWindow changeMinWindow = new FolderWindows.ChangeMinWindow(OurList);
            changeMinWindow.Show();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button OurB = (Button)sender;
            int ind = Convert.ToInt32(OurB.Uid);
            FolderWindows.UpdateAddWindow upada = new FolderWindows.UpdateAddWindow(ind);
            upada.Show();
        }

        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            FolderWindows.UpdateAddWindow upada = new FolderWindows.UpdateAddWindow();
            upada.Show();
        }

        private void BExport_Click(object sender, RoutedEventArgs e)
        {
            FolderWindows.ImportMenuWindow importMenu = new FolderWindows.ImportMenuWindow();
            importMenu.Show();
        }

        private void Pagin1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock TBLOCK = (TextBlock)sender;
            switch (TBLOCK.Uid)
            {
                case "prev":
                    FolderClasses.PaginatiaClass.CurrentPage--;
                    break;
                case "next":
                    FolderClasses.PaginatiaClass.CurrentPage++;
                    break;
                default:
                    FolderClasses.PaginatiaClass.CurrentPage = Convert.ToInt32(TBLOCK.Text);
                    break;
            }
            LVProductList.ItemsSource = FinalFilter.Skip(FolderClasses.PaginatiaClass.CurrentPage * FolderClasses.PaginatiaClass.CountOrder - FolderClasses.PaginatiaClass.CountOrder).Take(FolderClasses.PaginatiaClass.CountOrder).ToList();
            FolderClasses.PaginatiaClass.sketch();
        }
    }
}
