
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;




namespace Server
{
    public class Server
    {
        private TcpListener listener;
        private object gameStartLock = new object();
        private static bool gameStarted = false;
        public List<ClientHandler> clients = new List<ClientHandler>();
        private ServerForm serverForm;
        public Queue<ClientHandler> clientQueue = new Queue<ClientHandler>();
        private const int MaxPlayers = 4;
        private volatile bool gameProgress = false;
        public int gameNumber = 0;
        public static IPAddress address = IPAddress.Parse("10.51.117.80");


        public Server(ServerForm form)
        {
            serverForm = form;
            listener = new TcpListener(address, 8888);
            
        }

        // Start the server
        public void Start()
        {
            listener.Start();
            ShowNotification(address.ToString());
            ShowNotification("Server started. Waiting for clients...");
            //ThreadPool.QueueUserWorkItem(AcceptClients);
            Thread acceptThread = new Thread(new ThreadStart(AcceptClients));
            acceptThread.Start();
            acceptThread.IsBackground = true;
        }

        // Stop the server
        public void Stop()
        {
            BroadcastMessage("Server is shutting down.\n");
            ShowNotification("Server is shutting down.\n");
            listener.Stop();
            // Close all client connections
            lock (clients)
            {
                foreach (ClientHandler client in clients)
                {
                    client.CloseConnection();
                }
            }
            lock(clientQueue)
            {
                foreach (ClientHandler client in clientQueue)
                {
                    client.CloseConnection();
                }
            }
            

            // Exit the application or stop the server
            
            clients.Clear();
            clientQueue.Clear();
            gameNumber = 0;
            gameProgress = false;
            Environment.Exit(0);
        }



        // Method to accept client connections
        private void AcceptClients()
        {
            while (true)
            {
                try
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();

                    // Receive client name
                    NetworkStream networkStream = tcpClient.GetStream();
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
                        tcpClient.Close();
                        ShowNotification($"Client with duplicate name '{clientname}' tried to connect.");
                        continue;
                    }
                    ClientHandler clientHandler = new ClientHandler(tcpClient, this,clientname);
                   
           
                    

                    lock (clients)
                    {
                        if (clients.Count < 4)
                        {
                            clients.Add(clientHandler);
                            Thread clientThread = new Thread(clientHandler.HandleClient);
                            clientThread.IsBackground = true;
                            clientThread.Start();
                        }
                        else
                        {
                            clientQueue.Enqueue(clientHandler);
                            clientHandler.SendMessage("The room is full. You have been placed in a queue.");
                            ShowNotification("New Client came in but the room is full.");
                        }
                    }

                    if (clients.Count == 4)
                    {
                        lock (gameStartLock)
                        {
                            Thread gameThread = new Thread(StartGame);
                            gameThread.IsBackground = true;
                            gameThread.Start();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowNotification($"An exception occurred while accepting clients: {ex.Message}");
                }
            }
        }




        public void StartGame()
        {
            BroadcastMessage("Game will start in 5 seconds.\n");
            ShowNotification("Game will start in 5 seconds.\n");

            // Start the countdown
            for (int i = 5; i > 0; i--)
            {
                Thread.Sleep(1000); // Sleep for 1 second
                BroadcastMessage(i.ToString());
                ShowNotification(i.ToString());


                // Check if the number of clients is less than 4
                while (clients.Count < 4)
                {
                    // Check if there are clients in the waiting queue
                    if (clientQueue.Count > 0)
                    {
                        // Add the next client from the queue to the clients list
                        ClientHandler nextClient = clientQueue.Dequeue();
                        clients.Add(nextClient);

                        // Inform players about the new player joining
                        BroadcastMessage($"Player {nextClient.clientName} joined the game.");

                        // Check if the total count of clients is now 4
                        if (clients.Count == 4)
                        {
                            i = 4;
                            break;
                            // Exit the loop if there are 4 clients
                        }
                    }
                    i = 4;

                }


            }

            // Send "Go" message to indicate the start of the game
            BroadcastMessage("Go");
            ShowNotification("Go");

            lock (LeaderboardDictionary)
            {
                foreach (ClientHandler client in clients)
                {
                    if (!LeaderboardDictionary.ContainsKey(client.clientName))
                    {
                        LeaderboardDictionary[client.clientName] = client.score;
                    }
                }
            }
            System.IO.File.WriteAllLines("leaderboard.txt", LeaderboardDictionary.Select(kv => $"{kv.Key} {kv.Value}"));
            UpdateLeaderboard();
            ShowNotification("Leaderboard updated.\n");

            GameRoom gameRoom = new GameRoom(clients.Count, clients, this);
            Thread gameRoomThread = new Thread(new ThreadStart(gameRoom.StartGame));
            gameRoomThread.IsBackground = true;
            gameRoomThread.Start();

            gameRoomThread.Join(); // This blocks until the gameRoomThread completes
            if (gameRoom.Winner != "")
            {
                BroadcastMessage(gameRoom.Winner + " is the winner!\n");
                ShowNotification(gameRoom.Winner + " is the winner!\n");
            }
            else
            {
                BroadcastMessage(" No one is the winner!\n");
                ShowNotification("No one is the winner!\n");

            }
            

            lock (LeaderboardDictionary)
            {
                foreach (ClientHandler client in clients)
                {
                    if (client.clientName == gameRoom.Winner)
                    {
                        client.score += 1;
                    }

                    if (!LeaderboardDictionary.ContainsKey(client.clientName))
                    {
                        LeaderboardDictionary[client.clientName] = client.score;
                    }
                    else
                    {
                        LeaderboardDictionary[client.clientName] = client.score;
                    }

                }
                System.IO.File.WriteAllLines("leaderboard.txt", LeaderboardDictionary.Select(kv => $"{kv.Key} {kv.Value}"));
            }

            UpdateLeaderboard();
            ShowNotification("Leaderboard updated.\n");

            gameNumber++;

            BroadcastMessage("The new game will start in 20 seconds");
            ShowNotification("The new game will start in 20 seconds");
            Thread.Sleep(20000);
            RestartGame();
        }

        public void RestartGame()
        {
            lock (clients)
            {
                foreach (ClientHandler client in clients)
                {
                    clientQueue.Enqueue(client);
                    client.gesture = "";
                }
                clients.Clear();

                while (clients.Count < 4 && clientQueue.Count > 0)
                {
                    clients.Add(clientQueue.Dequeue());
                }
            }

            if (clients.Count == 4)
            {
                BroadcastMessage("Restarting game with the next set of players...\n");
                ShowNotification("Restarting game with the next set of players...\n");
                StartGame();
            }
            else
            {
                BroadcastMessage("Not enough players to restart the game.\n");
                ShowNotification("Not enough players to restart the game.\n");
                Stop();
            }
        }

        public void BroadcastMessage(string message)
        {
            lock (clients)
            {
                foreach (ClientHandler client in clients)
                {
                    client.SendMessage(message);
                }
            }
        }

        public Dictionary<string, int> LeaderboardDictionary = new Dictionary<string, int>();

        public  void UpdateDictionary( string name)
        {
            // Check if the name exists in the dictionary
            if (LeaderboardDictionary.ContainsKey(name))
            {
                // Increment the value associated with the name by one
                LeaderboardDictionary[name]++;
            }
            UpdateLeaderboard();


        }


        // Method to update leaderboard and notify clients
        public void UpdateLeaderboard()
        {
            serverForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                serverForm.UpdateLeaderboard(LeaderboardDictionary);
            });
            BroadcastMessage("Leaderboard updated.");
        }

        // Method to show notification on the server GUI
        public void ShowNotification(string message)
        {
            serverForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                serverForm.ShowNotification(message);
            });
        }

        

        public bool IsNameUnique(string name)
        {
            // Check if the name is already in use
            foreach (ClientHandler client in clients)
            {
                if (client.clientName == name)
                {
                    return false;
                }
            }
            return true;
        }


















    }




}
public class ClientStats
{
    public int Score { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
}