using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Reflection;
using System.Xml.Linq;

namespace Server
{
    public class GameRoom
    {
        List<ClientHandler> clientsPlay;
        int numPlayers;
        Server server;
        public string Winner { get; private set; }
        private object lockObject = new object();
        private Dictionary<string, string> NameGesture = new Dictionary<string, string>();


        public GameRoom(int numPlayers, List<ClientHandler> clients, Server server)
        {
            this.numPlayers = numPlayers;
            this.clientsPlay = new List<ClientHandler>(clients); 
            this.server = server;
        }

        public void StartGame()
        {
            server.BroadcastMessage("We started the game\n");
            bool gameEnded = false;
            int numRounds = 1;
            string theWinner = "";
            

            while (!gameEnded)
            {
                foreach (ClientHandler client in server.clients)
                {
                    client.gesture = "";
                    client.leftthegame = false;
                    client.gestureReceivedEvent.Reset(); // Reset the event
                }

                NameGesture.Clear(); // Reset gestures
                server.BroadcastMessage("Waiting for 10 seconds. Players please enter your gesture.\n");
                server.ShowNotification("Waiting for 10 seconds. Players please enter your gesture.\n");


                // Wait for all gestures to be received or timeout after 10 seconds
                WaitHandle[] waitHandles = clientsPlay.Select(c => c.gestureReceivedEvent).ToArray();
                bool allGesturesReceived = WaitHandle.WaitAll(waitHandles, 20000);

                if (!allGesturesReceived)
                {
                    server.BroadcastMessage("Time is up! Processing received gestures.\n");
                    server.ShowNotification("Time is up! Processing received gestures.\n");
                }
                //Thread.Sleep(20000); // wait for 10 seconds
                server.ShowNotification($"Round {numRounds}\n");
                server.BroadcastMessage($"Round {numRounds}\n");


                lock (server.clients)
                {
                    foreach (ClientHandler client in server.clients)
                    {
                        if (client.leftthegame)
                        {
                            lock (clientsPlay)
                            {
                                if (clientsPlay.Contains(client))
                                {
                                    clientsPlay.Remove(client); // Remove the clients that left the game
                                    client.losses++;
                                    server.BroadcastMessage($"Player {client.clientName} is out of the game because they left the game .\n");
                                    server.ShowNotification($"Player {client.clientName} is out of the game because they left the game .\n");
                                }
                            }
                        }
                    }
                    lock (clientsPlay)
                    {
                        foreach (ClientHandler client in clientsPlay)
                        {
                            if (!server.clients.Contains(client))
                            {
                                server.BroadcastMessage($"Player {client.clientName} is out of the game because they disconnected.\n");
                                client.losses++;
                                server.ShowNotification($"Player {client.clientName} is out of the game because they disconnected.\n");
                                clientsPlay.Remove(client);
                            }
                        }
                    }
                }

                
                foreach (ClientHandler client in server.clients)
                {
                    if (clientsPlay.Contains(client) && client.gesture != "")
                    {
                        NameGesture[client.clientName] = client.gesture.ToLower();
                        server.BroadcastMessage(client.clientName + " chose " + client.gesture.ToLower() + "\n");
                        server.ShowNotification(client.clientName + " chose " + client.gesture.ToLower() + "\n");
                    }
                    else if (clientsPlay.Contains(client) && client.gesture == "")
                    {
                        server.BroadcastMessage(client.clientName + " lost as they did not enter the gesture on time.\n");
                        server.ShowNotification(client.clientName + " lost as they did not enter the gesture on time.\n");

                        lock (clientsPlay)
                        {
                            clientsPlay.Remove(client);
                        }
                    }
                }
    

                if (clientsPlay.Count > 1)
                {

                    List<string> NewWinners = WhoWon(NameGesture);

                    if (NewWinners.Count == 1)
                    {
                        theWinner = NewWinners[0];
                        gameEnded = true;
                  
                    }
                    else if (NewWinners.Count == clientsPlay.Count)
                    {
                        foreach(ClientHandler client in clientsPlay)
                        {
                            if (NewWinners.Contains(client.clientName))
                            {
                                server.BroadcastMessage($"Player {client.clientName} moves on to the next round.\n");
                                server.ShowNotification($"Player {client.clientName} moves on to the next round.\n");
                            }
                            client.draws++;
                            client.draws++;
                        }
                       
                    }
                    else
                    {
                        foreach (ClientHandler client in clientsPlay) // Iterate over a copy of the players list
                        {
                            string name= client.clientName;
                            if (!NewWinners.Contains(name))
                            {
                                client.SendMessage("e");
                                server.BroadcastMessage(name + " lost this round. They were eliminated.\n");
                                

                                lock (clientsPlay)
                                {
                                   clientsPlay.Remove(client);
                                }
                            }
                        }

                    }

                    numRounds += 1;


                }
                else if (clientsPlay.Count == 1)
                {
                    theWinner = clientsPlay[0].clientName;
                    gameEnded = true;
                }
                else
                {
                    server.BroadcastMessage("No players left in the game room. Terminating the game. \n");
                    server.ShowNotification("No players left in the game room.. Terminating the game. \n");
                    gameEnded = true;
                }
            }

            lock (lockObject)
            {
                Winner = theWinner;
                
            }
        }



        public List<string> WhoWon(Dictionary<string, string> NameGesture)
        {
            List<string> winners = new List<string>();
            List<string> names = new List<string>(NameGesture.Keys);
            List<string> gestures = new List<string>(NameGesture.Values);

            if (names.Count == 1)
            {
                winners.Add(names[0]);
            }
            else if (names.Count == 2)
            {
                if (CanBeat(gestures[0], gestures[1]))
                {
                    winners.Add(names[0]);
                }
                else if (CanBeat(gestures[1], gestures[0]))
                {
                    winners.Add(names[1]);
                }
                else
                {
                    winners = names; // ties
                }
            }
            else if (names.Count == 3)
            {
                if (gestures[0] == gestures[1] && gestures[1] == gestures[2])
                {
                    winners = names;
                }
                else if (gestures[0] != gestures[1] && gestures[1] == gestures[2])
                {
                    if (CanBeat(gestures[0], gestures[1]))
                    {
                        winners.Add(names[0]);
                    }
                    else
                    {
                        winners.Add(names[1]);
                        winners.Add(names[2]);
                    }
                }
                else if (gestures[1] != gestures[0] && gestures[0] == gestures[2])
                {
                    if (CanBeat(gestures[1], gestures[0]))
                    {
                        winners.Add(names[1]);
                    }
                    else
                    {
                        winners.Add(names[0]);
                        winners.Add(names[2]);
                    }
                }
                else if (gestures[2] != gestures[1] && gestures[0] == gestures[1])
                {
                    if (CanBeat(gestures[2], gestures[1]))
                    {
                        winners.Add(names[2]);
                    }
                    else
                    {
                        winners.Add(names[0]);
                        winners.Add(names[1]);
                    }
                }
                else if (gestures[0] != gestures[1] && gestures[0] != gestures[2] && gestures[1] != gestures[2])
                {
                    winners = names;
                }
            }
            else if (names.Count == 4)
            {
                if (gestures[0] == gestures[1] && gestures[1] == gestures[2] && gestures[2] == gestures[3])
                {
                    winners = names;
                }
                else if ((gestures.Count(item => item == "rock") == 3) || (gestures.Count(item => item == "paper") == 3) || (gestures.Count(item => item == "scissors") == 3)) // Three and one case
                {
                    if (gestures[0] != gestures[1] && gestures[1] == gestures[2] && gestures[2] == gestures[3])
                    {
                        if (CanBeat(gestures[0], gestures[1]))
                        {
                            winners.Add(names[0]);
                        }
                        else
                        {
                            winners.Add(names[1]);
                            winners.Add(names[2]);
                            winners.Add(names[3]);
                        }
                    }
                    else if (gestures[1] != gestures[0] && gestures[0] == gestures[2] && gestures[2] == gestures[3])
                    {
                        if (CanBeat(gestures[1], gestures[0]))
                        {
                            winners.Add(names[1]);
                        }
                        else
                        {
                            winners.Add(names[0]);
                            winners.Add(names[2]);
                            winners.Add(names[3]);
                        }
                    }
                    else if (gestures[2] != gestures[0] && gestures[0] == gestures[1] && gestures[1] == gestures[3])
                    {
                        if (CanBeat(gestures[2], gestures[0]))
                        {
                            winners.Add(names[2]);
                        }
                        else
                        {
                            winners.Add(names[0]);
                            winners.Add(names[1]);
                            winners.Add(names[3]);
                        }
                    }
                    else if (gestures[3] != gestures[0] && gestures[0] == gestures[1] && gestures[1] == gestures[2])
                    {
                        if (CanBeat(gestures[3], gestures[0]))
                        {
                            winners.Add(names[3]);
                        }
                        else
                        {
                            winners.Add(names[0]);
                            winners.Add(names[1]);
                            winners.Add(names[2]);
                        }
                    }
                }
                else if (((gestures.Count(item => item == "rock") == 2) && ((gestures.Count(item => item == "paper") == 2) || (gestures.Count(item => item == "scissors") == 2))) || ((gestures.Count(item => item == "paper") == 2) && ((gestures.Count(item => item == "scissors") == 2) || (gestures.Count(item => item == "rock") == 2))) || ((gestures.Count(item => item == "scissors") == 2) && ((gestures.Count(item => item == "paper") == 2) || (gestures.Count(item => item == "rock") == 2)))) // Two and two case
                {
                    if (gestures.Count(item => item == "rock") == 2 && gestures.Count(item => item == "paper") == 2)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (gestures[i] == "paper")
                            {
                                winners.Add(names[i]);
                            }
                        }
                    }
                    else if (gestures.Count(item => item == "rock") == 2 && gestures.Count(item => item == "scissors") == 2)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (gestures[i] == "rock")
                            {
                                winners.Add(names[i]);
                            }
                        }
                    }
                    else if (gestures.Count(item => item == "scissors") == 2 && gestures.Count(item => item == "paper") == 2)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (gestures[i] == "scissors")
                            {
                                winners.Add(names[i]);
                            }
                        }
                    }
                }
                else if (gestures.Count(item => item == "rock") == 2 || gestures.Count(item => item == "paper") == 2 || gestures.Count(item => item == "scissors") == 2) // Two alike, two different case
                {
                    if (gestures.Count(item => item == "rock") == 2)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (gestures[i] == "rock")
                            {
                                winners.Add(names[i]);
                            }
                        }
                    }
                    else if (gestures.Count(item => item == "paper") == 2)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (gestures[i] == "paper")
                            {
                                winners.Add(names[i]);
                            }
                        }
                    }
                    else if (gestures.Count(item => item == "scissors") == 2)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (gestures[i] == "scissors")
                            {
                                winners.Add(names[i]);
                            }
                        }
                    }
                }
            }

            return winners;
        }

        bool CanBeat(string gestureA, string gestureB)
        {
            if (gestureA == "rock" && gestureB == "scissors")
            {
                return true;
            }
            else if (gestureA == "scissors" && gestureB == "paper")
            {
                return true;
            }
            else if (gestureA == "paper" && gestureB == "rock")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}