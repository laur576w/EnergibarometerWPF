using Energi;
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

namespace Energibarometer {
	/// <summary>
	/// Interaction logic for SplashScreen.xaml
	/// </summary>
	public partial class SplashScreen : Window {
		public EnergiDatabase Database;
		public Person Person;
		public List<Incident> Incidents;
		public SplashScreen() {
			InitializeComponent();
		}

		private void OnInitialized(object sender, EventArgs e) {
			Database = new EnergiDatabase();

		}


		private void Button_CreateAccount_Click(object sender, RoutedEventArgs e) {
			if (MainStack.Visibility == Visibility.Visible) {
				MainStack.Visibility = Visibility.Collapsed;
				SecondaryStack.Visibility = Visibility.Visible;
				btnCreate.Content = "Opret konto";
			}
			else {
				// creating account
				int.TryParse(createId.Text.Trim(), out int id);
				try {
					Database.TryOpen();
					Person = new(id, Name.Text.Trim(), "", "", "", "", "", "");
					Database.UploadPerson(Person);
					Database.TryClose();
					MainWindow win1 = new MainWindow(Person, Incidents);
					
					win1.Show();
					Close();

				}
				catch (InvalidOperationException m) {
					Error.Text = m.Message;
					Database.TryClose();
				}
				catch (Exception) {
					Error.Text = "Unknown error.";
					Database.TryClose();
				}

			}

		}

		public void LogInTest() {

			int.TryParse(Id.Password.ToString(), out int id);
			Person TestPerson = Database.GetPersonsById((int)id, (string)Password.Password.ToString());
			if (Person.ID != (int)id) {
				throw new AggregateException("Invalid unilogin.");
			}

		}

		public void Button_LogIn_Click(object sender, RoutedEventArgs e) {
			int.TryParse(Id.Password.ToString(), out int id);

			try {
				Database.TryOpen();
				Person = Database.GetPersonsById((int)id, (string)Password.Password.ToString());
				Incidents = Database.GetIncidentsById((int)id, (string)Password.Password.ToString());
				Database.TryClose();
				MainWindow win1 = new MainWindow(Person, Incidents);
				win1.Show();
				Close();
			}
			catch (InvalidOperationException m) {
				Error.Text = m.Message;
				Database.TryClose();
			}
			catch (Exception) {
				Error.Text = "Unknown error.";
				Database.TryClose();
			}
		}
	}


}
