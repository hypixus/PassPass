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

namespace PassPassClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for (var i = 0; i < 50; i++)
            {
                CollectionListBox.Items.Add(new ListBoxItem()
                {
                    Content = "pepega"
                });
                EntriesListBox.Items.Add(new ListBoxItem()
                {
                    Content = "omegalul"
                });
            }
        }
        
        private void CollectionListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        
        private void EntriesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
