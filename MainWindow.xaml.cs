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
using ActualFlashcardApp.ViewModels;

namespace ActualFlashcardApp
{

    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
        //handler for github button. Eventually needs to be changed to a hyperlink button since you can't set one via the C# class
        public void github_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //Handler for closing application
        public void closeApplication_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
