
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;




namespace Server
{
    public class Server
    {
        private TcpListener listener;
        private static readonly object gameStartLock = new object();
        private static bool gameStarted = false;
        public List<ClientHandler> clients = new List<ClientHandler>();
        private ServerForm serverForm;
        public Queue<ClientHandler> clientQueue = new Queue<ClientHandler>();
        private const int MaxPlayers = 4;
        private volatile bool gameProgress = false;
        public int gameNumber = 0;
        public Dictionary<string,int>LeaderboardDictionary = new Dictionary<string,int>();
        public static IPAddress address = IPAddress.Parse("10.51.56.31");



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
            listener.Stop();
            // Close all client connections
            lock (clients)
            {
                foreach (ClientHandler client in clients)
                {
                    client.CloseConnection();
                }
            }
            clients.Clear();
            clientQueue.Clear();
            gameProgress = false;
        }
        
       

        // Method to accept client connections
        private void AcceptClients()
        {

            while (true)
            {
                try
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    ClientHandler clientHandler = new ClientHandler(tcpClient, this);
                    

                    // Lock access to the clients list to prevent race conditions
                    lock (clients)
                    {
                        if (clients.Count < MaxPlayers)
                        {
                            clients.Add(clientHandler);
                            //ThreadPool.QueueUserWorkItem(clientHandler.HandleClient);
                            Thread clientThread = new Thread(clientHandler.HandleClient);
                            clientThread.Start();
                            clientThread.IsBackground = true;
      
                        }
                        else
                        {
                            clientQueue.Enqueue(clientHandler);
                            clientHandler.SendMessage("The room is full. You have been placed in a queue.");
                            ShowNotification("New Client came in but the room is full.");
                        }
                    }


                    // Check if we can start the game
                    if (clients.Count == MaxPlayers)
                    {
                        
                        bool lockAcquired = Monitor.TryEnter(gameStartLock);

                        if (lockAcquired)
                        {
                            // Start the game if the lock is acquired
                            Thread gameThread = new Thread(StartGame);
                            gameThread.Start();
                            gameThread.IsBackground = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowNotification($"An exception occurred while accepting clients: {ex.Message}");
                    // Decide how to handle the exception, whether to log or stop the server
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
            //GameRoom gameRoom = new GameRoom(clients.Count,clients,this);
            //Thread gameThread = new Thread(new ThreadStart(gameRoom.StartGame));
            //gameThread.Start();
            //gameThread.IsBackground = true;

            foreach (ClientHandler client in clients)
            {
                if (!LeaderboardDictionary.ContainsKey(client.clientName))
                {
                    LeaderboardDictionary.Add(client.clientName, client.score);

                }

            }
            System.IO.File.WriteAllLines("leaderboard.txt", LeaderboardDictionary.Select(kv => $"{kv.Key} {kv.Value}"));
            UpdateLeaderboard();
            ShowNotification("Leaderboard updated.\n");

            gameProgress = true;
            gameNumber++;

            //lock (gameStartLock)
            //{
               // while (gameThread.IsAlive) { }

                //if (!gameThread.IsAlive)
                //{
                //    Monitor.Exit(gameStartLock);
              //  }
            //}

        }

        public void RestartGame()
        {
             

            foreach(ClientHandler client in clients)
            {
                clientQueue.Enqueue(client);

            }
            clients.Clear();

            
             while(clients.Count !=4)
             {
                if(clientQueue.Count > 0)
                {
                    clients.Add(clientQueue.Dequeue());
                }  
                   
             }
             StartGame();

        }

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



        // Method to broadcast message to all clients
        public void BroadcastMessage(string message)
        {
            foreach (ClientHandler client in clients)
            {
                client.SendMessage(message);
            }
        }

        // Method to update leaderboard and notify clients
        public void UpdateLeaderboard()
        {
            serverForm.Invoke((MethodInvoker)delegate {
                serverForm.UpdateLeaderboard(LeaderboardDictionary);
            });
            BroadcastMessage("Leaderboard updated.");
        }

        // Method to show notification on the server GUI
        public void ShowNotification(string message)
        {
            serverForm.Invoke((MethodInvoker)delegate {
                serverForm.ShowNotification(message);
            });
        }

        // Method to handle client disconnection
        public void HandleClientDisconnection(ClientHandler client,string mes)
        {
            // Remove the disconnected client from the list
            if( mes == "l")
            {
                clients.Remove(client);
                clientQueue.Enqueue(client);
                BroadcastMessage($"Player {client.clientName} left the game.");
            }
            else
            {
                clients.Remove(client);
                BroadcastMessage($"Player {client.clientName} disconnected.");

            }
            
            
        }


















    }




}