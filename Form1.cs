//
////I, Camilo Becerra, 000035320 certify that this material is my original work.  No other person's work has been used without due acknowledgement.using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace lab5b
{
    /// <summary>
    /// The main window for the Doctor Who application. 
    /// Handles database connections, data loading, and user interactions.
    /// </summary>
    public partial class DoctorWho : Form
    {
        // Connection string for the local SQL Express database
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=COMP10204_Lab5;Integrated Security=True";

        // Lists to hold data in memory for the LINQ operations
        List<Doctor> doctors = new List<Doctor>();
        List<Companion> companions = new List<Companion>();
        List<Episode> episodes = new List<Episode>();

        public DoctorWho()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs when the form first opens. 
        /// Connects to the database, downloads all data into lists, and populates the Doctor dropdown menu.
        /// </summary>
        private void DoctorWho_Load(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            // Loads all episode data from the database into the list
            SqlCommand episodeQuery = new SqlCommand("SELECT * FROM episode", connection);
            SqlDataReader epReader = episodeQuery.ExecuteReader();

            while (epReader.Read())
            {
                string storyId = (string)epReader["STORYID"];
                int season = (int)epReader["SEASON"];
                int seasonYear = (int)epReader["SEASONYEAR"];
                string title = (string)epReader["TITLE"];

                Episode ep = new Episode(storyId, season, seasonYear, title);
                episodes.Add(ep);
            }
            epReader.Close();

            // Loads all doctor data from the database into the list
            SqlCommand doctorQuery = new SqlCommand("SELECT * FROM doctor", connection);
            SqlDataReader docReader = doctorQuery.ExecuteReader();

            while (docReader.Read())
            {
                int doctorId = (int)docReader["DOCTORID"];
                string actor = (string)docReader["ACTOR"];
                string debut = (string)docReader["DEBUT"];
                int series = (int)docReader["SERIES"];
                int age = (int)docReader["AGE"];

                // Images are stored as byte arrays in the database
                byte[] photo = (byte[])docReader["Picture"];

                Doctor doc = new Doctor(doctorId, actor, series, age, debut, photo);
                doctors.Add(doc);
            }
            docReader.Close();

            // Loads all companion data from the database into the list
            SqlCommand compQuery = new SqlCommand("SELECT * FROM companion", connection);
            SqlDataReader compReader = compQuery.ExecuteReader();

            while (compReader.Read())
            {
                string name = (string)compReader["NAME"];
                string actor = (string)compReader["ACTOR"];
                string episode = (string)compReader["STORYID"];
                int doctorId = (int)compReader["DOCTORID"];

                Companion comp = new Companion(name, actor, episode, doctorId);
                companions.Add(comp);
            }
            compReader.Close();
            connection.Close();

            // Adds all Doctor IDs to the dropdown box (ComboBox)
            foreach (Doctor doctor in doctors)
            {
                CboDoctor.Items.Add(doctor.DoctorId);
            }
        }

        /// <summary>
        /// Triggered when the user selects a Doctor from the dropdown list.
        /// Updates the screen with that Doctor's details using either LINQ or SQL.
        /// </summary>
        private void CboDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedDoctor = (int)CboDoctor.SelectedItem;

            // Checks which method the user has selected: LINQ or SQL
            if (radLINQ.Checked)
            {
                // Solves using LINQ
                // Searches the in-memory lists created during the Load event

                // Finds the specific doctor object
                Doctor doc = doctors.FirstOrDefault(d => d.DoctorId == selectedDoctor);

                // Finds the episode object that matches the doctor's debut
                Episode ep = episodes.FirstOrDefault(epi => epi.StoryId == doc.Debut);

                // Finds all companions linked to this doctor ID
                IEnumerable<Companion> comp = companions.Where(c => c.DoctorId == doc.DoctorId);

                // Converts the raw bytes back into an Image for display
                MemoryStream stream = new MemoryStream(doc.Photo);
                Image docPicture = Image.FromStream(stream);

                // Updates text boxes
                txtPlayedBy.Text = doc.Actor;
                txtYear.Text = ep.SeasonYear.ToString();
                txtSeries.Text = doc.Series.ToString();
                txtFirstEpisode.Text = ep.Title;
                txtAge.Text = doc.Age.ToString();
                picDoctor.Image = docPicture;

                // Updates the companion list
                lstCompanions.Items.Clear();
                foreach (var c in comp)
                {
                    // Finds the episode title for this specific companion
                    Episode compEpisode = episodes.FirstOrDefault(episode => episode.StoryId == c.Episode);

                    lstCompanions.Items.Add($"{c.Name} ({c.Actor})");
                    lstCompanions.Items.Add($"\"{compEpisode.Title}\" ({compEpisode.SeasonYear})");
                    lstCompanions.Items.Add(" ");
                }
            }
            else
            {
                // Solves using SQL
                // Connects to the database again and retrieves only the specific data needed

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                // Retrieves Doctor Info
                SqlCommand doctorQuery = new SqlCommand($"SELECT * FROM doctor WHERE DOCTORID = {selectedDoctor}", connection);
                SqlDataReader docReader = doctorQuery.ExecuteReader();

                string debutStoryId = "";

                if (docReader.Read())
                {
                    txtPlayedBy.Text = (string)docReader["ACTOR"];
                    txtSeries.Text = docReader["SERIES"].ToString();
                    txtAge.Text = docReader["AGE"].ToString();
                    debutStoryId = (string)docReader["DEBUT"]; // Saves this ID to look up the episode next

                    // Checks if an image exists and loads it
                    if (docReader["Picture"] != DBNull.Value)
                    {
                        byte[] photo = (byte[])docReader["Picture"];
                        MemoryStream stream = new MemoryStream(photo);
                        picDoctor.Image = Image.FromStream(stream);
                    }
                }
                docReader.Close();

                // Retrieves Doctor's Debut Episode Info using the ID just saved
                SqlCommand epQuery = new SqlCommand($"SELECT * FROM episode WHERE STORYID = '{debutStoryId}'", connection);
                SqlDataReader epReader = epQuery.ExecuteReader();
                if (epReader.Read())
                {
                    txtFirstEpisode.Text = (string)epReader["TITLE"];
                    txtYear.Text = epReader["SEASONYEAR"].ToString();
                }
                epReader.Close();

                // Retrieves Companions and their debut Episodes
                // Uses a JOIN statement to combine Companion and Episode tables in one query
                string compSql = "SELECT C.NAME, C.ACTOR, E.TITLE, E.SEASONYEAR " +
                                 "FROM COMPANION C " +
                                 "JOIN EPISODE E ON C.STORYID = E.STORYID " +
                                 $"WHERE C.DOCTORID = {selectedDoctor}";

                SqlCommand compQuery = new SqlCommand(compSql, connection);
                SqlDataReader compReader = compQuery.ExecuteReader();

                lstCompanions.Items.Clear();

                while (compReader.Read())
                {
                    string cName = (string)compReader["NAME"];
                    string cActor = (string)compReader["ACTOR"];
                    string eTitle = (string)compReader["TITLE"];
                    int eYear = (int)compReader["SEASONYEAR"];

                    lstCompanions.Items.Add($"{cName} ({cActor})");
                    lstCompanions.Items.Add($"\"{eTitle}\" ({eYear})");
                    lstCompanions.Items.Add(" ");
                }
                compReader.Close();
                connection.Close();
            }
        }
    }
}

   