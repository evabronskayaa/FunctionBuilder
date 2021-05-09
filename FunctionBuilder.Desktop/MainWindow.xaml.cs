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

namespace FunctionBuilder.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void btnCalculate_click(object sender, RoutedEventArgs e) 
        {
            if (RPN.IsExpressionCorrectly(((TextBox) FindName("tbFunction")).Text)) 
            {
                Drawer.SetFunction(((TextBox)FindName("tbFunction")).Text);
                Drawer.DrawField();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            Drawer.MainWindow = this;
            Drawer.GraphCanvas = (Canvas)FindName("Canvas");
            Drawer.SetControls();
        }
    }
}
