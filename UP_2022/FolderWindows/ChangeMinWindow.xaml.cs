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
using System.Collections;

namespace UP_2022.FolderWindows
{
    /// <summary>
    /// Логика взаимодействия для ChangeMinWindow.xaml
    /// </summary>
    public partial class ChangeMinWindow : Window
    {
        double maxNum = 0;
        List<Material> _AllProperty;

        public ChangeMinWindow(List<Material> AllProperty)
        {
            InitializeComponent();
            if (!CalcMaxProperty(AllProperty))
            {
                var Mesnum = MessageBox.Show("Ошибка вычисления максимального числа", "Ошибка", MessageBoxButton.OK);
                if (Mesnum == MessageBoxResult.OK)
                {
                    this.Close();
                }
                else if (Mesnum == MessageBoxResult.Cancel)
                {
                    this.Close();
                }
                FolderClasses.ChangePropertyClass.listview.Items.Refresh();
            }
            else 
            {
                _AllProperty = AllProperty;
                TBOXNumber.Text = maxNum.ToString();
            }            
        }
        

        public bool CalcMaxProperty(List<Material> AllProperty)
        {            
            try
            {
                if (AllProperty.Count != 0)
                {
                    for (int i = 0; i < AllProperty.Count; i++)
                    {
                        if (maxNum < AllProperty[i].MinCount)
                        {
                            maxNum = AllProperty[i].MinCount;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
                       
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TBOXNumber.Text))
            {
                for (int i = 0; i < _AllProperty.Count; i++)
                {
                    _AllProperty[i].MinCount = Convert.ToInt32(TBOXNumber.Text);
                }
                FolderClasses.BD.Data.SaveChanges();
                MessageBox.Show("Данные обновленны", "Изменение информации", MessageBoxButton.OK);
                FolderClasses.ChangePropertyClass.listview.Items.Refresh();
                this.Close();
            }
            else
            {
                MessageBox.Show("Поле пустое", "Ошибка", MessageBoxButton.OK);
            }
        }
    }
}
