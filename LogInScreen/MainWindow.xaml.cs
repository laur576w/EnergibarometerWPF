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

namespace LogInScreen {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public int i = 0;

		public MainWindow() {
			InitializeComponent();
		}


		private void Button_CreateAccount_Click(object sender, RoutedEventArgs e) {
			if (i >= 0) {
				StackPass.Visibility = Visibility.Hidden;
				Names.Visibility = Visibility.Visible;
				i++;
			}
			else {
				//create Person
			}
		}
	}
}
