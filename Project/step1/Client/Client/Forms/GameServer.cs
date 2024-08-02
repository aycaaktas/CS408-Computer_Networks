using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;


namespace RPSGame.Client.Forms
{
    public partial class GameServer : Form
    {
        public GameServer()
        {
            InitializeComponent();
        }

        private void connect_button_Click(object sender, EventArgs e)
        {
            try
            {
                // Convert the port from string to int
                int port = int.Parse(port_text.Text);

                // Create a TCP client
                TcpClient client = new TcpClient();

                // Connect to the server
                client.Connect(IPAddress.Parse(ip_text.Text), port);

                // Send the username to the server immediately after connecting
                var stream = client.GetStream();
                byte[] usernameBytes = System.Text.Encoding.ASCII.GetBytes(name_text.Text);
                stream.Write(usernameBytes, 0, usernameBytes.Length);

                // Update the UI or show a message that the connection is successful
                MessageBox.Show("Connected to server!");

                //Connecting the main game page
                this.Hide(); // Hide the current form
                GameMain gameMainForm = new GameMain(client); // Assuming you have a form named GameMain for the main game
                gameMainForm.FormClosed += (s, args) => this.Close(); // Close the GameServer form when GameMain is closed
                gameMainForm.Show(); // Show the GameMain form
            }
            catch (Exception ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show("Error connecting to server: " + ex.Message);
            }
        }
    }
}
