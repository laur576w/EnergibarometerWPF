using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Energibarometer {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void NumberOneToFive(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^1-5]");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void Button_UploadPerson_Click(object sender, RoutedEventArgs e) { }
		private void Button_UploadIncident_Click(object sender, RoutedEventArgs e) { }

		private void Button_UpdatePerson_Click(object sender, RoutedEventArgs e) { }
		private void Button_UpdateIncident_Click(object sender, RoutedEventArgs e) { }

		private void Button_DeletePerson_Click(object sender, RoutedEventArgs e) { }
		private void Button_DeleteIncident_Click(object sender, RoutedEventArgs e) { }
	}

}
