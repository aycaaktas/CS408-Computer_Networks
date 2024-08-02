

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ClientHandler
    {
        public TcpClient client;
        private Server server;
        public string clientName;
        public int score;
        public string gesture;

        public ClientHandler(TcpClient tcpClient, Server server)
        {
            client = tcpClient;
            this.server = server;
            clientName = "";
            gesture = "";
            score = 0;
        }

        public void HandleClient()
        {

            // Receive client name
            NetworkStream networkStream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
            string clientname = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Check if the client name is unique
            bool isNameUnique = IsNameUnique(clientname);

            if (!isNameUnique)
            {
                // Notify the client that the name is already in use
                byte[] errorMessage = Encoding.UTF8.GetBytes("Name already in use. Please choose a different name.");
                networkStream.Write(errorMessage, 0, errorMessage.Length);
                networkStream.Close();
                CloseConnection();
                return;
            }
            this.clientName=clientname.ToLower();
            server.BroadcastMessage($"Player {clientName} joined the game.");
            server.ShowNotification($"Client {clientName} came in.");


            // Start a new thread to handle message reading
            Thread readThread = new Thread(() =>
            {
                while (true)
                {
                    // Read messages from the client's network stream
                    bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        string lowercaseMessage = message.ToLower();

                        // Check if the client wants to leave the game
                        if (lowercaseMessage == "leave")
                        {
                            // Notify the server and remove the client from the game
                            server.HandleClientDisconnection(this,"l");
                            
                        }
                        if(lowercaseMessage == "rock" || lowercaseMessage == "paper" || lowercaseMessage == "scissors")
                        {
                            gesture = lowercaseMessage;
                        }
                        if (lowercaseMessage == "terminate")
                        {
                            // Notify the server and remove the client from the game
                            server.HandleClientDisconnection(this,"t");
                            CloseConnection();
                            break;

                        }
                    }
                   
                }
            });

            // Start the read thread
            readThread.Start();
        }

    


        private bool IsNameUnique(string name)
        {
            // Check if the name is already in use
            foreach (ClientHandler client in server.clients)
            {
                if (client.clientName == name)
                {
                    return false;
                }
            }
            return true;
        }

        // Method to send message to client
        public void SendMessage(string message)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to close client connection
        public void CloseConnection()
        {
            client.Close();
        }

        public void IncrementScore()
        {
            score = score + 1;
        }




    }
}

