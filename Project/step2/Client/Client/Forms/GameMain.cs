using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPSGame.Client.Forms
{
    public partial class GameMain : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread listenThread; // Thread to listen for server messages
        delegate void UpdateLeaderboardCallback(Dictionary<string, int> leaderboard);

        public GameMain(TcpClient client)
        {
            InitializeComponent();
            this.client = client;
            stream = client.GetStream();

            // Start listening to the server messages immediately after initializing the form
            listenThread = new Thread(new ThreadStart(ListenForMessages));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenForMessages()
        {
            try
            {
                byte[] receivedBytes = new byte[1024];
                int byteCount;

                while ((byteCount = stream.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(receivedBytes, 0, byteCount);

                    if (IsDictionaryMessage(message))
                    {
                        Dictionary<string, int> leaderboard = ConvertStringToDictionary(message);
                        UpdateLeaderboard(leaderboard);
                    }
                    else
                    {
                        DisplayMessage(message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving data from server: " + ex.Message);
            }
        }

        private bool IsDictionaryMessage(string message)
        {
            string[] lines = message.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (!line.Contains(","))
                {
                    return false;
                }
            }
            return true;
        }

        private void UpdateLeaderboard(Dictionary<string, int> leaderboard)
        {
            if (this.score_list.InvokeRequired)
            {
                UpdateLeaderboardCallback callback = new UpdateLeaderboardCallback(UpdateLeaderboard);
                this.Invoke(callback, new object[] { leaderboard });
            }
            else
            {
                score_list.Rows.Clear();
                foreach (var kvp in leaderboard)
                {
                    score_list.Rows.Add(kvp.Key, kvp.Value);
                }
            }
        }

        private Dictionary<string, int> ConvertStringToDictionary(string leaderboardData)
        {
            Dictionary<string, int> leaderboard = new Dictionary<string, int>();
            string[] lines = leaderboardData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    string playerName = parts[0];
                    int score = int.Parse(parts[1]);
                    leaderboard.Add(playerName, score);
                }
            }
            return leaderboard;
        }

        private void DisplayMessage(string message)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(message + "\n");
                }));
            }
            else
            {
                richTextBox1.AppendText(message + "\n");
            }
        }

        private void SendChoice(string choice)
        {
            try
            {
                byte[] choiceData = Encoding.ASCII.GetBytes(choice);
                stream.Write(choiceData, 0, choiceData.Length);
                UpdateRichTextBox($"You chose: {choice}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending choice: " + ex.Message);
            }
        }

        private void UpdateRichTextBox(string message)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(message + "\n");
                }));
            }
            else
            {
                richTextBox1.AppendText(message + "\n");
            }
        }

        private void button_rock_Click(object sender, EventArgs e)
        {
            SendChoice("Rock");
        }

        private void button_paper_Click(object sender, EventArgs e)
        {
            SendChoice("Paper");
        }

        private void button_scissor_Click(object sender, EventArgs e)
        {
            SendChoice("Scissor");
        }

        private void button_terminate_Click(object sender, EventArgs e)
        {
            try
            {
                SendChoice("terminate");
                if (stream != null)
                {
                    UpdateRichTextBox($"You pressed terminate");
                    stream.Close();
                }
                if (client != null)
                {
                    client.Close();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error terminating connection: " + ex.Message);
            }
        }

        private void button_leave_Click(object sender, EventArgs e)
        {
            SendChoice("Leave");
        }
    }
}
