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
    public partial class FramesList : Page
    {
        List<Material> StartFilter = FolderClasses.BD.Data.Material.ToList();
        public FramesList()
        {
            InitializeComponent();
            LVProductList.ItemsSource = StartFilter;          
        }

        private void CBFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CBSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
