using Energi;
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
		public EnergiDatabase Database;
		public Person Person;
		public List<Incident> Incidents;
		public MainWindow(Person person, List<Incident> incidents) {
			Database = new EnergiDatabase();
			this.Person = person;
			this.Incidents = incidents;
			InitializeComponent();
		}


		private void OnInitialized(object sender, EventArgs e) {
			Name.Text = Person.Name;
		}

		public void UpdateLabels() {
			Error.Text = "";
		}

		private void NumberOneToFive(object sender, TextCompositionEventArgs e) {
			Regex regex = new Regex("[^1-5]");
			e.Handled = regex.IsMatch(e.Text);
		}

		public void LogInTest() {

			Person TestPerson = Database.GetPersonsById((int)Person.ID, (string)Person.Name);
			if (Person.ID != (int)TestPerson.ID) {
				throw new AggregateException("Invalid unilogin.");
			}

		}

		public void Button_UploadIncident_Click(object sender, RoutedEventArgs e) {
			try {
				Database.TryOpen();
				LogInTest();
				int.TryParse(Energy.Text, out int energy);
				int.TryParse(EnergyEffect.Text, out int energyEffect);
				Incident incident = new Incident(Person.ID, (int)energy, (int)energyEffect, (string)Description.Text, DateTime.Now);
				Database.UploadIncident(incident);
				Incidents = Database.GetIncidentsById((int)Person.ID, (string)Person.Name);
				IncidentList.ItemsSource = Incidents;
				UpdateLabels();
				Database.TryClose();
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


		private void Button_UpdatePerson_Click(object sender, RoutedEventArgs e) {
			try {
				Database.TryOpen();
				LogInTest();
				Person.Name = (string)Name.Text.Trim();
				Database.UpdatePerson(Person);
				Database.TryClose();
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
		private void Button_UpdateIncident_Click(object sender, RoutedEventArgs e) {
			try {
				Database.TryOpen();
				LogInTest();
				Incident SelectedOreder = (Incident)IncidentList.SelectedItem;
				Incident incident = new Incident(SelectedOreder.ID, Person.ID, SelectedOreder.Energi, SelectedOreder.IncidentEffect, SelectedOreder.ActivityAndCauses, SelectedOreder.Time);
				Database.UpdateIncident(incident);
				Database.TryClose();
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

		private void Button_DeletePerson_Click(object sender, RoutedEventArgs e) {
			try {
				Database.TryOpen();
				LogInTest();
				Database.DeletePerson(Person);
				Person = new Person(0, "", "", "", "", "", "", "");
				Incidents = new List<Incident>();
				Database.TryClose();
				MessageBox.Show("Du har slettet din konto.", "Slettet konto");
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
		private void Button_DeleteIncident_Click(object sender, RoutedEventArgs e) {
			try {
				Database.TryOpen();
				LogInTest();
				Database.DeleteIncident((Incident)IncidentList.SelectedItem);
				Database.TryClose();
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
		public void Button_LogOut_Click(object sender, RoutedEventArgs e) {
			SplashScreen log = new SplashScreen();
			log.Show();
			Close();
		}
	}


}


