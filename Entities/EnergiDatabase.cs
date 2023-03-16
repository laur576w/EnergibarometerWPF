﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Energi {
	public class EnergiDatabase {
		private SqlConnection Connection;

		public EnergiDatabase() {
			string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EnergiDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
			Connection = new SqlConnection(ConnectionString);
		}

		public void TryOpen() {
			try {
				Connection.Open();
			}
			catch (Exception) {
				throw new AggregateException("Could’t open NorthWind.");
			}
		}
		public void TryClose() {
			try {
				Connection.Close();
			}
			catch (Exception) {
				throw new AggregateException("Could’t close NorthWind.");
			}
		}

		private List<Person> GetPersons() {
			string SQL = $"SELECT * FROM Person;";
			SqlCommand Command = new SqlCommand(SQL, Connection);
			SqlDataReader Reader = Command.ExecuteReader();

			List<Person> Persons = new List<Person>();
			while (Reader.Read()) {
				Person Person = new Person((int)Reader["Id"], (string)Reader["Name"], (string)Reader["EnergiLevel1"], (string)Reader["EnergiLevel2"], (string)Reader["EnergiLevel3"], (string)Reader["EnergiLevel4"], (string)Reader["EnergiLevel5"], (string)Reader["EnergiLevel5"]);
				Persons.Add(Person);
			}
			Reader.Close();

			return Persons;
		}
		private List<Incident> GetIncidents() {
			string SQL = $"SELECT * FROM Incident;";
			SqlCommand Command = new SqlCommand(SQL, Connection);
			SqlDataReader Reader = Command.ExecuteReader();

			List<Incident> Incidents = new List<Incident>();
			while (Reader.Read()) {
				Incident Incident = new Incident((int)Reader["Id"], (int)Reader["PersonID"], (int)Reader["Energi"], (int)Reader["IncidentEffect"], (string)Reader["ActivityAndCauses"], (DateTime)Reader["Time"]);
				Incidents.Add(Incident);
			}
			Reader.Close();

			return Incidents;
		}

		public Person GetPersonsById(int ID, string Name) {
			Person ThePerson = null;
			List<Person> peoples = GetPersons();

			bool PersonFound = false;
			foreach (Person person in peoples) {
				if (person.ID == ID && person.Name == Name) {
					ThePerson = person;
					PersonFound = true;
				}
			}

			switch (PersonFound) {
				case true:
					return ThePerson;
				case false:
					throw new AggregateException("Invalid unilogin.");
			}
		}
		public List<Incident> GetIncidentsById(int ID, string Name) {
			Person ThePerson = this.GetPersonsById(ID, Name);
			List<Incident> TheIncidents = new List<Incident>();
			List<Incident> Incidents = GetIncidents();

			foreach (Incident incident in Incidents) {
				if (incident.PersonID == ID) {
					TheIncidents.Add(incident);
				}
			}

			return TheIncidents;
		}
		public void UploadPerson(Person NewPerson) {
			string SQL = $"INSERT INTO Person (Id, Name, EnergiLevel1, EnergiLevel2, EnergiLevel3, EnergiLevel4, EnergiLevel5, GeneralStrategy) VALUES ({NewPerson.ID} , '{NewPerson.Name}', '{NewPerson.EnergiLevel1}', '{NewPerson.EnergiLevel2}', '{NewPerson.EnergiLevel3}', '{NewPerson.EnergiLevel4}', '{NewPerson.EnergiLevel5}', '{NewPerson.GeneralStrategy}');";
			SqlCommand Command = new SqlCommand(SQL, Connection);

			List<Person> Persons = this.GetPersons();
			foreach (Person Person in Persons) {
				if (Person.ID == NewPerson.ID) {
					throw new AggregateException("You can't uplode a person that doesn't have a unique ID");
				}
			}

			Command.ExecuteNonQuery();
		}
		public void UploadIncident(Incident NewIncident) {
			string SQL = $"INSERT INTO Incident (Id, PersonID, Energi, IncidentEffect, Time) VALUES ({NewIncident.ID}, {NewIncident.PersonID}, {NewIncident.Energi}, {NewIncident.IncidentEffect}, '{NewIncident.Time}');";
			SqlCommand Command = new SqlCommand(SQL, Connection);

			List<Incident> Incidents = this.GetIncidents();
			foreach (Incident Incident in Incidents) {
				if (Incident.ID == NewIncident.ID) {
					throw new AggregateException("You can't uplode an incident that doesn't have a unique ID");
				}
			}

			List<Person> Persons = this.GetPersons();
			bool ForeignKeyMatch = false;
			foreach (Person Person in Persons) {
				if (Person.ID == NewIncident.PersonID) {
					ForeignKeyMatch = true;
				}
			}
			if (ForeignKeyMatch == false) {
				throw new AggregateException("You can't uplode an incident that has a unique person's ID");
			}

			Command.ExecuteNonQuery();
		}
		public void UpdatePerson(Person UpdatedPerson) {
			string SQL = $"UPDATE Person SET Name = '{UpdatedPerson.Name}', EnergiLevel1 = '{UpdatedPerson.EnergiLevel1}', EnergiLevel2 = '{UpdatedPerson.EnergiLevel2}', EnergiLevel3 = '{UpdatedPerson.EnergiLevel3}', EnergiLevel4 = '{UpdatedPerson.EnergiLevel4}', EnergiLevel5 = '{UpdatedPerson.EnergiLevel5}', GeneralStrategy = '{UpdatedPerson.GeneralStrategy}' WHERE Id = {UpdatedPerson.ID};";
			SqlCommand Command = new SqlCommand(SQL, Connection);

			List<Person> Persons = this.GetPersons();
			bool IdMatch = false;
			foreach (Person Person in Persons) {
				if (Person.ID == UpdatedPerson.ID) {
					IdMatch = true;
				}
			}
			if (IdMatch == false) {
				throw new AggregateException("You can't update a person without a matching ID");
			}

			Command.ExecuteNonQuery();
		}
		public void UpdateIncident(Incident UpdatedIncident) {
			string SQL = $"UPDATE Incident SET PersonID = {UpdatedIncident.PersonID}, Energi = {UpdatedIncident.Energi}, IncidentEffect = {UpdatedIncident.IncidentEffect}, Time = {UpdatedIncident.Time} WHERE Id = {UpdatedIncident.ID};";
			SqlCommand Command = new SqlCommand(SQL, Connection);

			List<Incident> Incidents = this.GetIncidents();
			bool IdMatch = false;
			foreach (Incident Incident in Incidents) {
				if (Incident.ID == UpdatedIncident.ID) {
					IdMatch = true;
				}
			}
			if (IdMatch == false) {
				throw new AggregateException("You can't update an incident without a matching ID");
			}

			List<Person> Persons = this.GetPersons();
			bool ForeignKeyMatch = false;
			foreach (Person Person in Persons) {
				if (Person.ID == UpdatedIncident.PersonID) {
					ForeignKeyMatch = true;
				}
			}
			if (ForeignKeyMatch == false) {
				throw new AggregateException("You can't update an incident that with a unique person's ID");
			}

			Command.ExecuteNonQuery();
		}
		public void DeletePerson(Person WrongPerson) {
			string SQL = $"DELETE FROM Person WHERE Id = {WrongPerson.ID};";
			string SqlForIncident = $"DELETE FROM Incident WHERE PersonID = {WrongPerson.ID};";
			SqlCommand Command = new SqlCommand(SQL, Connection);
			SqlCommand CommandForInciden = new SqlCommand(SqlForIncident, Connection);

			List<Person> Persons = this.GetPersons();
			bool IdMatch = false;
			foreach (Person Person in Persons) {
				if (Person.ID == WrongPerson.ID) {
					IdMatch = true;
				}
			}
			if (IdMatch == false) {
				throw new AggregateException("You can't delete a person without a matching ID");
			}

			CommandForInciden.ExecuteNonQuery();
			Command.ExecuteNonQuery();
		}
		public void DeleteIncident(Incident WrongIncident) {
			string SQL = $"DELETE FROM Incident WHERE Id = {WrongIncident.ID};";
			SqlCommand Command = new SqlCommand(SQL, Connection);

			List<Incident> Incidents = this.GetIncidents();
			bool IdMatch = false;
			foreach (Incident Incident in Incidents) {
				if (Incident.ID == WrongIncident.ID) {
					IdMatch = true;
				}
			}
			if (IdMatch == false) {
				throw new AggregateException("You can't delete an Incident without a matching ID");
			}

			Command.ExecuteNonQuery();
		}
	}
}