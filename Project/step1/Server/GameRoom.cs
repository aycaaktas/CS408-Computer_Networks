using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Server
{
    public class GameRoom
    {
        List<ClientHandler> clients;
        int numPlayers;
        Server server;
        Dictionary<string, int> map = new Dictionary<string, int>()
            {
                { "rock", 1},
                { "paper", 2},
                {"scissors", 3 }
            };

        Dictionary<int, string> reverseMap = new Dictionary<int, string>()
                {
                    { 1, "rock"},
                    { 2,  "paper"},
                    { 3, "scissors"}
                };

        public GameRoom(int numPlayers, List<ClientHandler> clients, Server server)
        {
            this.numPlayers = numPlayers;
            clients.Clear();
            this.server = server;
            this.clients = clients;
        }

        public void StartGame()
        {


            bool gameEnded = false;

            while (!gameEnded)
            {
                Dictionary<string, int> gestures = new Dictionary<string, int>();

                Thread.Sleep(10000);

				foreach (ClientHandler client in clients)
                {
                    if (!server.clients.Contains(client))
                    {
                        clients.Remove(client);
                    }
                }

                foreach (ClientHandler client in clients)
                {
                    if (client.gesture != "")
                    {
                        gestures[client.clientName] = map[client.gesture.ToLower()];
                    }
                    else
                    {   
                       server.BroadcastMessage(client.clientName + " lost as they did not enter the gesture on time.\n");
                        clients.Remove(client);
                    }
                }

                if (!gameEnded && clients.Count == 1)
                {
                    string name = clients[0].clientName;
                    clients[0].IncrementScore();
                    server.BroadcastMessage(name + " won the game.");
                    gameEnded = true;
                    break;
                }

                if (gameEnded)
                {
                    server.UpdateDictionary(clients[0].clientName);
                }

                Dictionary<string, int> leaderboardDictionary = server.LeaderboardDictionary;

                System.IO.File.WriteAllLines("leaderboard.txt", leaderboardDictionary.Select(kv => $"{kv.Key} {kv.Value}"));


                gameEnded = RunRound(gestures);

            }

            server.RestartGame();

        }



        private bool RunRound(Dictionary<string, int> gestures)
        {

            server.BroadcastMessage("Rules of the game: rock crushes scissors, scissors cuts paper, and paper covers rock.\n");

            List<int> gesturesList = gestures.Values.ToList();

            foreach (ClientHandler client in clients)
            {
                server.BroadcastMessage(client.clientName + " chose " + client.gesture + ".\n");
            }

            if (AllTheSame(gestures) && gesturesList.Count > 1)
            {

                server.BroadcastMessage("All choices are alike. Everyone moves to the next round.\n");

            }
            else if (AllUnique(gestures) && gesturesList.Count > 1)

            {

                server.BroadcastMessage("All choices were unique. No one wins. Everyone moves to the next.");

            }
            else if (gesturesList.Count(x => x == 1) == 3 || gesturesList.Count(x => x == 2) == 3 || gesturesList.Count(x => x == 3) == 3)
            {
                // Three Rocks:
                if (gesturesList.Count(x => x == 1) == 3)
                {
                    int nonRockGesture = -1, nonRockIndex = -1;

                    foreach (int gesture in gesturesList)
                    {
                        if (gesture != 1)
                        {
                            nonRockGesture = gesture;
                            nonRockIndex = gesturesList.IndexOf(gesture);
                            break;
                        }
                    }

                    if (nonRockGesture == 2)
                    {
                        string clientName = gestures.Keys.ToList()[nonRockIndex];
                        clients[nonRockIndex].IncrementScore();
                        server.BroadcastMessage(clientName + " won this round.\n");

                        foreach (ClientHandler client in clients)
                        {
                            if (client.clientName == clientName)
                            {
                                client.IncrementScore();
                                break;
                            }
                        }

                        return true;
                    }

                    if (nonRockGesture == 3)
                    {
                        string clientName = gestures.Keys.ToList()[nonRockIndex];
                        foreach (ClientHandler client in clients)
                        {
                            if (client.clientName != clientName)
                            {
                                server.BroadcastMessage(clientName + " moves on to next round.\n");
                            }
                            else
                            {
                                clients.Remove(client);
                            }
                        }
                        server.BroadcastMessage(clientName + " was eliminated in this round.\n");
                    }
                }
                else if (gesturesList.Count(x => x == 2) == 3)
                {
                    int nonPaperGesture = -1 , nonPaperIndex = -1;
                    foreach (int gesture in gesturesList)
                    {
                        if (gesture != 2)
                        {
                            nonPaperGesture = gesture;
                            nonPaperIndex = gesturesList.IndexOf(gesture);
                            break;
                        }
                    }

                    if (nonPaperGesture == 2)
                    {
                        string clientName = gestures.Keys.ToList()[nonPaperIndex];
                        clients[nonPaperIndex].IncrementScore();
                        server.BroadcastMessage(clientName + " won this round.\n");
                        return true;
                    }

                    if (nonPaperGesture == 1)
                    {
                        string clientName = gestures.Keys.ToList()[nonPaperIndex];
                        foreach (ClientHandler client in clients)
                        {
                            if (client.clientName != clientName)
                            {
                                server.BroadcastMessage(clientName + " moves on to next round.\n");

                            }
                            else
                            {
                                clients.Remove(client);
                            }
                        }
                        server.BroadcastMessage(clientName + " was eliminated in this round.\n");
                    }
                }
                else if (gesturesList.Count(x => x == 3) == 3)
                {
                    int nonScissorGesture = 0, nonScissorIndex = 0;
                    foreach (int gesture in gesturesList)
                    {
                        if (gesture != 2)
                        {
                            nonScissorGesture = gesture;
                            nonScissorIndex = gesturesList.IndexOf(gesture);
                            break;
                        }
                    }

                    if (nonScissorGesture == 1)
                    {
                        string clientName = gestures.Keys.ToList()[nonScissorIndex];
                        clients[nonScissorIndex].IncrementScore();
                        server.BroadcastMessage(clientName + " won this round.\n");
                        return true;
                    }

                    if (nonScissorGesture == 2)
                    {
                        string clientName = gestures.Keys.ToList()[nonScissorIndex];
                        foreach (ClientHandler client in clients)
                        {
                            if (client.clientName != clientName)
                            {
                                server.BroadcastMessage(clientName + " moves on to next round.\n");
                                    
                            }
                            else
                            {
                                clients.Remove(client);
                            }
                        }
                        server.BroadcastMessage(clientName + " was eliminated in this round.\n");
                    }
                }

            }
            else if (gesturesList.Count(x => x == 1) == 2 || gesturesList.Count(x => x == 2) == 2 ||
                gesturesList.Count(x => x == 3) == 2)
            {
                // 2 cases: the other two options are the same or the other two options are differnet
                if (gesturesList.Count(x => x == 1) == 2 && gesturesList.Count(x => x == 2) == 1 &&
                    gesturesList.Count(x => x == 3) == 1)
                {
                    List<string> clientNames = gestures.Keys.ToList();

                    for (int i = gesturesList.Count - 1; i >= 0; i--)
                    {
                        if (gesturesList[i] == 1)
                        {
                            server.BroadcastMessage(clientNames[i] + " is moving to the next round.\n");
                        }
                        else
                        {
                            server.BroadcastMessage(clientNames[i] + " is eliminated.\n");
                            clients.RemoveAt(i);
                        }
                    }
                }

                else if (gesturesList.Count(x => x == 1) == 2 && gesturesList.Count(x => x == 2) == 2)
                {
                    List<string> clientNames = gestures.Keys.ToList();

                    List<int> indicesToRemove = new List<int>();

                    for (int i = 0; i < gesturesList.Count; i++)
                    {
                        if (gesturesList[i] == 2)
                        {
                            server.BroadcastMessage(clientNames[i] + " is moving to the next round.\n");
                        }
                        else
                        {
                            server.BroadcastMessage(clientNames[i] + " is eliminated.\n");
                            indicesToRemove.Add(i);
                        }
                    }

                    // Remove elements from the clients list in reverse order
                    for (int i = indicesToRemove.Count - 1; i >= 0; i--)
                    {
                        clients.RemoveAt(indicesToRemove[i]);
                    }

                }
                else if (gesturesList.Count(x => x == 1) == 2 && gesturesList.Count(x => x == 3) == 2)
                {
                    List<string> clientNames = gestures.Keys.ToList();

                    List<int> indicesToRemove = new List<int>();

                    for (int i = 0; i < gesturesList.Count; i++)
                    {
                        if (gesturesList[i] == 1)
                        {
                            server.BroadcastMessage(clientNames[i] + " is moving to the next round.\n");
                        }
                        else
                        {
                            server.BroadcastMessage(clientNames[i] + " is eliminated.\n");
                            indicesToRemove.Add(i);
                        }
                    }

                    // Remove elements from the clients list in reverse order
                    for (int i = indicesToRemove.Count - 1; i >= 0; i--)
                    {
                        int indexToRemove = indicesToRemove[i];
                        clients.RemoveAt(indexToRemove);
                    }

                }
                else if (gesturesList.Count(x => x == 2) == 2 && gesturesList.Count(x => x == 1) == 1 &&
                    gesturesList.Count(x => x == 3) == 1) // 2 papers, 1 rock, 1 scissor
                {
                    List<string> clientNames = gestures.Keys.ToList();

                    List<int> indicesToRemove = new List<int>();

                    for (int i = 0; i < gesturesList.Count; i++)
                    {
                        if (gesturesList[i] == 2)
                        {
                            server.BroadcastMessage(clientNames[i] + " is moving to the next round.\n");
                        }
                        else
                        {
                            server.BroadcastMessage(clientNames[i] + " is eliminated.\n");
                            indicesToRemove.Add(i);
                        }
                    }

                    // Remove elements from the clients list in reverse order
                    for (int i = indicesToRemove.Count - 1; i >= 0; i--)
                    {
                        int indexToRemove = indicesToRemove[i];
                        clients.RemoveAt(indexToRemove);
                    }

                }
                else if (gesturesList.Count(x => x == 2) == 2 && gesturesList.Count(x => x == 3) == 2)
                {
                    List<string> clientNames = gestures.Keys.ToList();

                    List<int> indicesToRemove = new List<int>();

                    for (int i = 0; i < gesturesList.Count; i++)
                    {
                        if (gesturesList[i] == 3)
                        {
                            server.BroadcastMessage(clientNames[i] + " is moving to the next round.\n");
                        }
                        else
                        {
                            server.BroadcastMessage(clientNames[i] + " is eliminated.\n");
                            indicesToRemove.Add(i);
                        }
                    }

                    // Remove elements from the clients list in reverse order
                    for (int i = indicesToRemove.Count - 1; i >= 0; i--)
                    {
                        int indexToRemove = indicesToRemove[i];
                        clients.RemoveAt(indexToRemove);
                    }

                }
                else if (gesturesList.Count(x => x == 3) == 2 && gesturesList.Count(x => x == 1) == 1 &&
                    gesturesList.Count(x => x == 2) == 1) // 2 papers, 1 rock, 1 scissor
                {
                    List<string> clientNames = gestures.Keys.ToList();

                    List<int> indicesToRemove = new List<int>();

                    for (int i = 0; i < gesturesList.Count; i++)
                    {
                        if (gesturesList[i] == 3)
                        {
                            server.BroadcastMessage(clientNames[i] + " is moving to the next round.\n");
                        }
                        else
                        {
                            server.BroadcastMessage(clientNames[i] + " is eliminated.\n");
                            indicesToRemove.Add(i);
                        }
                    }

                    // Remove elements from the clients list in reverse order
                    for (int i = indicesToRemove.Count - 1; i >= 0; i--)
                    {
                        int indexToRemove = indicesToRemove[i];
                        clients.RemoveAt(indexToRemove);
                    }

                }
            }
            if (gesturesList.Count == 2)
            {
                if (gesturesList.Contains(1) && gesturesList.Contains(2))
                {
                    foreach (ClientHandler client in clients)
                    {
                        if (map[client.gesture] == 2)
                        {
                            client.IncrementScore();
                            server.BroadcastMessage(client.clientName + " won the game.");
                        }
                    }
                }
                else if (gesturesList.Contains(1) && gesturesList.Contains(3))
                {
                    foreach (ClientHandler client in clients)
                    {
                        if (map[client.gesture] == 1)
                        {
                            client.IncrementScore();
                            server.BroadcastMessage(client.clientName + " won the game.");
                            return true;
                        }
                    }
                }
                else if (gesturesList.Contains(2) && gesturesList.Contains(3))
                {
                    foreach (ClientHandler client in clients)
                    {
                        if (map[client.gesture] == 3)
                        {
                            client.IncrementScore();
                            server.BroadcastMessage(client.clientName + " won the game.");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool AllTheSame(Dictionary<string, int> gestures)
        {
            List<int> gesturesList = gestures.Values.ToList();

            for (int i = 0; i < gesturesList.Count - 1; i++)
            {
                for (int j = i + 1; j < gesturesList.Count; j++)
                {
                    if (gesturesList[i] != gesturesList[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool AllUnique(Dictionary<string, int> gestures)
        {
            List<int> gesturesList = gestures.Values.ToList();

            for (int i = 0; i < gesturesList.Count - 1; i++)
            {
                for (int j = i + 1; j < gesturesList.Count; j++)
                {
                    if (gesturesList[i] == gesturesList[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}