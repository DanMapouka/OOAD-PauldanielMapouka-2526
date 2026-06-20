using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace CLDierenarts
{
    /// <summary>
    /// Basisklasse voor Hond en Kat. De klasse bevat ook alle communicatie
    /// met de tabel dieren, zodat de WPF-app niet weet waar de data vandaan komt.
    /// </summary>
    public abstract class Dier
    {
        private static readonly string connString =
            ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public int Id { get; set; }
        public string Naam { get; set; }
        public string EigenaarId { get; set; }
        public Eigenaar Eigenaar { get; set; }
        public DateTime Geboortedatum { get; set; }
        public double Gewicht { get; set; }
        public Urgentie Urgentie { get; set; }
        public bool IsOpgenomen { get; set; }
        public DateTime? DatumOpgenomen { get; set; }

        public abstract string Type { get; }

        public Dier()
        {
            Naam = "";
            EigenaarId = "";
            Urgentie = Urgentie.Normaal;
        }

        /// <summary>
        /// Geeft de gemeenschappelijke informatie voor de details-weergave.
        /// Hond en Kat breiden deze methode uit met base.GeefInfo().
        /// </summary>
        public virtual string GeefInfo()
        {
            string eigenaarNaam = Eigenaar == null ? EigenaarId : Eigenaar.ToString();
            string opnameTekst = "Nee";

            if (IsOpgenomen)
            {
                opnameTekst = "Ja";
                if (DatumOpgenomen.HasValue)
                {
                    opnameTekst += $" sinds {DatumOpgenomen.Value:dd/MM/yyyy HH:mm}";
                }
            }

            return $"Ticketnummer: {Id}{Environment.NewLine}" +
                   $"Naam: {Naam}{Environment.NewLine}" +
                   $"Type: {Type}{Environment.NewLine}" +
                   $"Eigenaar: {eigenaarNaam}{Environment.NewLine}" +
                   $"Geboortedatum: {Geboortedatum:dd/MM/yyyy}{Environment.NewLine}" +
                   $"Gewicht: {Gewicht:0.##} kg{Environment.NewLine}" +
                   $"Urgentie: {Urgentie}{Environment.NewLine}" +
                   $"Opgenomen: {opnameTekst}";
        }

        /// <summary>
        /// Tekst voor de ListBox. Een opname is altijd zichtbaar.
        /// </summary>
        public override string ToString()
        {
            string resultaat = $"#{Id} - {Naam} ({Type}) - {Urgentie}";

            if (IsOpgenomen)
            {
                resultaat += " - OPGENOMEN";
            }

            return resultaat;
        }

        public static List<Dier> GetAll()
        {
            List<Dier> dieren = new List<Dier>();

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                string sql = @"
                    SELECT d.id, d.naam, d.eigenaarId, d.geboortedatum,
                           d.gewicht, d.urgentie, d.type, d.ras,
                           d.isGevaccineerd, d.isOpgenomen, d.datumOpgenomen,
                           e.voornaam, e.achternaam
                    FROM dieren d
                    INNER JOIN eigenaars e ON e.id = d.eigenaarId
                    ORDER BY d.id";

                SqliteCommand comm = new SqliteCommand(sql, conn);
                SqliteDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    Dier dier = MaakDier(reader);
                    dieren.Add(dier);
                }
            }

            return dieren;
        }

        public static Dier GetById(int id)
        {
            List<Dier> dieren = GetAll();

            foreach (Dier dier in dieren)
            {
                if (dier.Id == id)
                {
                    return dier;
                }
            }

            return null;
        }

        public int InsertInDb()
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO dieren
                        (naam, eigenaarId, geboortedatum, gewicht, urgentie,
                         type, ras, isGevaccineerd, isOpgenomen, datumOpgenomen)
                    VALUES
                        ($naam, $eigenaarId, $geboortedatum, $gewicht, $urgentie,
                         $type, $ras, $isGevaccineerd, $isOpgenomen, $datumOpgenomen)";

                SqliteCommand comm = new SqliteCommand(sql, conn);
                VulParameters(comm);
                comm.ExecuteNonQuery();

                SqliteCommand idComm = new SqliteCommand("SELECT last_insert_rowid()", conn);
                Id = Convert.ToInt32(idComm.ExecuteScalar());
            }

            return Id;
        }

        public void UpdateInDb()
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                string sql = @"
                    UPDATE dieren
                    SET naam = $naam,
                        eigenaarId = $eigenaarId,
                        geboortedatum = $geboortedatum,
                        gewicht = $gewicht,
                        urgentie = $urgentie,
                        type = $type,
                        ras = $ras,
                        isGevaccineerd = $isGevaccineerd,
                        isOpgenomen = $isOpgenomen,
                        datumOpgenomen = $datumOpgenomen
                    WHERE id = $id";

                SqliteCommand comm = new SqliteCommand(sql, conn);
                VulParameters(comm);
                comm.Parameters.AddWithValue("$id", Id);
                comm.ExecuteNonQuery();
            }
        }

        public void DeleteFromDb()
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand comm = new SqliteCommand(
                    "DELETE FROM dieren WHERE id = $id",
                    conn);
                comm.Parameters.AddWithValue("$id", Id);
                comm.ExecuteNonQuery();
            }
        }

        public void NeemOp()
        {
            if (!IsOpgenomen)
            {
                IsOpgenomen = true;
                DatumOpgenomen = DateTime.Now;
                UpdateInDb();
            }
        }

        private void VulParameters(SqliteCommand comm)
        {
            comm.Parameters.AddWithValue("$naam", Naam.Trim());
            comm.Parameters.AddWithValue("$eigenaarId", EigenaarId);
            comm.Parameters.AddWithValue(
                "$geboortedatum",
                Geboortedatum.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            comm.Parameters.AddWithValue("$gewicht", Gewicht);
            comm.Parameters.AddWithValue("$urgentie", Urgentie.ToString());
            comm.Parameters.AddWithValue("$type", Type);

            if (this is Hond)
            {
                Hond hond = (Hond)this;
                comm.Parameters.AddWithValue("$ras", hond.Ras.Trim());
                comm.Parameters.AddWithValue("$isGevaccineerd", DBNull.Value);
            }
            else
            {
                Kat kat = (Kat)this;
                comm.Parameters.AddWithValue("$ras", DBNull.Value);
                comm.Parameters.AddWithValue(
                    "$isGevaccineerd",
                    kat.IsGevaccineerd ? 1 : 0);
            }

            comm.Parameters.AddWithValue("$isOpgenomen", IsOpgenomen ? 1 : 0);

            if (DatumOpgenomen.HasValue)
            {
                comm.Parameters.AddWithValue(
                    "$datumOpgenomen",
                    DatumOpgenomen.Value.ToString(
                        "yyyy-MM-ddTHH:mm:ss",
                        CultureInfo.InvariantCulture));
            }
            else
            {
                comm.Parameters.AddWithValue("$datumOpgenomen", DBNull.Value);
            }
        }

        private static Dier MaakDier(SqliteDataReader reader)
        {
            string type = reader["type"].ToString();
            Dier dier;

            if (type == "Hond")
            {
                Hond hond = new Hond();
                hond.Ras = reader["ras"] == DBNull.Value
                    ? ""
                    : reader["ras"].ToString();
                dier = hond;
            }
            else
            {
                Kat kat = new Kat();
                kat.IsGevaccineerd = reader["isGevaccineerd"] != DBNull.Value &&
                                     Convert.ToInt32(reader["isGevaccineerd"]) == 1;
                dier = kat;
            }

            dier.Id = Convert.ToInt32(reader["id"]);
            dier.Naam = reader["naam"].ToString();
            dier.EigenaarId = reader["eigenaarId"].ToString();
            dier.Geboortedatum = DateTime.Parse(
                reader["geboortedatum"].ToString(),
                CultureInfo.InvariantCulture);
            dier.Gewicht = Convert.ToDouble(
                reader["gewicht"],
                CultureInfo.InvariantCulture);

            Urgentie urgentie;
            if (!Enum.TryParse(reader["urgentie"].ToString(), out urgentie))
            {
                urgentie = Urgentie.Normaal;
            }
            dier.Urgentie = urgentie;

            dier.IsOpgenomen = Convert.ToInt32(reader["isOpgenomen"]) == 1;

            if (reader["datumOpgenomen"] != DBNull.Value)
            {
                dier.DatumOpgenomen = DateTime.Parse(
                    reader["datumOpgenomen"].ToString(),
                    CultureInfo.InvariantCulture);
            }

            Eigenaar eigenaar = new Eigenaar();
            eigenaar.Id = dier.EigenaarId;
            eigenaar.Voornaam = reader["voornaam"].ToString();
            eigenaar.Achternaam = reader["achternaam"].ToString();
            dier.Eigenaar = eigenaar;

            return dier;
        }
    }
}
