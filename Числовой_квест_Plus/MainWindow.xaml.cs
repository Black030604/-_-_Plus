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

namespace Числовой_квест_Plus
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Random random = new Random();
        private int playerHealth = 100;
        private int potions = 3;
        private int gold = 0;
        private int arrows = 5;
        private List<string> inventory = new List<string>();
        private string[] dungeonMap = new string[10];

        public MainWindow()
        {
            InitializeComponent();
        }

       
        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
