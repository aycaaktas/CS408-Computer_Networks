using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    public partial class ServerForm : Form
    {
        private Server server;
        private List<string> leaderboard = new List<string>();

        public ServerForm()
        {
            InitializeComponent();
            server = new Server(this);
        }

        // Method to update the leaderboard on the server GUI
        public void UpdateLeaderboard(Dictionary<string, int> LeaderboardDictionary)
        {
            var sortedEntries = LeaderboardDictionary.OrderByDescending(x => x.Value);

            // Clear existing rows in the DataGridView
            dataGridView1.Rows.Clear();

            // Add sorted entries to the DataGridView
            foreach (var entry in sortedEntries)
            {
                dataGridView1.Rows.Add(entry.Key, entry.Value);
            }
            foreach (ClientHandler cli in server.clients)
            {
                NetworkStream stream = cli.client.GetStream();
                string leaderboardString = ConvertDictionaryToString(LeaderboardDictionary);
                byte[] buffer = Encoding.UTF8.GetBytes(leaderboardString);
                stream.Write(buffer, 0, buffer.Length);
            }

        }
        private string ConvertDictionaryToString(Dictionary<string, int> dictionary)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in dictionary)
            {
                sb.AppendLine($"{kvp.Key},{kvp.Value}");
            }
            return sb.ToString();
        }

        // Method to display notifications on the server GUI
        public void ShowNotification(string message)
        {
            notificationsTextBox.AppendText(message + Environment.NewLine);
        }

        // Start button click event handler to start the server
        private void startButton_Click(object sender, EventArgs e)
        {
            server.Start();
            startButton.Enabled = false;
            stopButton.Enabled = true;

        }

        // Stop button click event handler to stop the server
        private void stopButton_Click(object sender, EventArgs e)
        {
            server.Stop();
            startButton.Enabled = true;
            stopButton.Enabled = false;
            ShowNotification("Server stopped.");
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void notificationsTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }




}


