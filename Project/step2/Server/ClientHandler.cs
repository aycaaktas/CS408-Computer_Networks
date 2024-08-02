

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
        public bool leftthegame=false;
        public int draws;
        public int losses;
        public ManualResetEvent gestureReceivedEvent;

        public ClientHandler(TcpClient tcpClient, Server server, string name)
        {


            client = tcpClient;
            this.server = server;
            clientName = name.ToLower();
            gesture = "";
            score = 0;
            losses = 0;
            draws = 0;
            gestureReceivedEvent = new ManualResetEvent(false);
        }

        public void HandleClient()
        {

            
            server.BroadcastMessage($"Player {clientName} joined the game.");
            server.ShowNotification($"Client {clientName} came in.");

            // Receive client name
            NetworkStream networkStream = client.GetStream();
            byte[] buffer = new byte[1024];


            
            // Start a new thread to handle message reading
            Thread readThread = new Thread(() =>
            {
                while (true)
                {
                    // Read messages from the client's network stream
                    int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        string lowercaseMessage = message.ToLower();

                        // Check if the client wants to leave the game
                        if (lowercaseMessage == "leave")
                        {
                            // Notify the server and remove the client from the game
                            HandleClientDisconnection("l");
                            
                        }
                        if(lowercaseMessage == "rock" || lowercaseMessage == "paper" || lowercaseMessage == "scissors")
                        {
                            gesture = lowercaseMessage;
                            gestureReceivedEvent.Set(); // Signal that gesture is received
                        }
                        if (lowercaseMessage == "terminate")
                        {
                            // Notify the server and remove the client from the game
                            HandleClientDisconnection("t");
                            break;

                        }
                    }
                   
                }
            });

            // Start the read thread
            readThread.Start();
        }
        // Method to handle client disconnection
        public void HandleClientDisconnection( string mes)
        {
            gestureReceivedEvent.Reset();
            // Remove the disconnected client from the list
            if (mes == "l")
            {
                leftthegame = true;
                server.BroadcastMessage($"Player {clientName} left the game.");
                server.ShowNotification($"Player {clientName} left the game.");
            }
            else
            {
                server.clients.Remove(this);
                CloseConnection();
                server.BroadcastMessage($"Player {clientName} disconnected.");
                server.ShowNotification($"Player {clientName} disconnected.");
            }
            


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

