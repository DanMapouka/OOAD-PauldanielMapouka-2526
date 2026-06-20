using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CLDierenarts
{
    /// <summary>
    /// Klasse die overeenkomt met de tabel eigenaars.
    /// </summary>
    public class Eigenaar
    {
        private static readonly string connString =
            ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public string Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }

        public Eigenaar()
        {
            Id = "";
            Voornaam = "";
            Achternaam = "";
        }

        public static List<Eigenaar> GetAll()
        {
            List<Eigenaar> eigenaars = new List<Eigenaar>();

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand comm = new SqliteCommand(
                    "SELECT id, voornaam, achternaam FROM eigenaars ORDER BY achternaam, voornaam",
                    conn);
                SqliteDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    Eigenaar eigenaar = new Eigenaar();
                    eigenaar.Id = reader["id"].ToString();
                    eigenaar.Voornaam = reader["voornaam"].ToString();
                    eigenaar.Achternaam = reader["achternaam"].ToString();
                    eigenaars.Add(eigenaar);
                }
            }

            return eigenaars;
        }

        public static Eigenaar GetById(string id)
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand comm = new SqliteCommand(
                    "SELECT id, voornaam, achternaam FROM eigenaars WHERE id = $id",
                    conn);
                comm.Parameters.AddWithValue("$id", id);
                SqliteDataReader reader = comm.ExecuteReader();

                if (reader.Read())
                {
                    Eigenaar eigenaar = new Eigenaar();
                    eigenaar.Id = reader["id"].ToString();
                    eigenaar.Voornaam = reader["voornaam"].ToString();
                    eigenaar.Achternaam = reader["achternaam"].ToString();
                    return eigenaar;
                }
            }

            return null;
        }

        public void InsertInDb()
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand comm = new SqliteCommand(
                    "INSERT INTO eigenaars (id, voornaam, achternaam) " +
                    "VALUES ($id, $voornaam, $achternaam)",
                    conn);
                comm.Parameters.AddWithValue("$id", Id);
                comm.Parameters.AddWithValue("$voornaam", Voornaam);
                comm.Parameters.AddWithValue("$achternaam", Achternaam);
                comm.ExecuteNonQuery();
            }
        }

        public void UpdateInDb()
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand comm = new SqliteCommand(
                    "UPDATE eigenaars SET voornaam = $voornaam, " +
                    "achternaam = $achternaam WHERE id = $id",
                    conn);
                comm.Parameters.AddWithValue("$id", Id);
                comm.Parameters.AddWithValue("$voornaam", Voornaam);
                comm.Parameters.AddWithValue("$achternaam", Achternaam);
                comm.ExecuteNonQuery();
            }
        }

        public void DeleteFromDb()
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand comm = new SqliteCommand(
                    "DELETE FROM eigenaars WHERE id = $id",
                    conn);
                comm.Parameters.AddWithValue("$id", Id);
                comm.ExecuteNonQuery();
            }
        }

        public override string ToString()
        {
            return $"{Voornaam} {Achternaam}";
        }
    }
}
