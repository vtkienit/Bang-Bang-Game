using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server
{
    private Queue<string> waitingQueue;
    private Queue<string> roomWaitingQueue;
    private Dictionary<string, Dictionary<string, TcpClient>> userClients;
    private Dictionary<string, bool> IsInMatch;
    private object _lock = new object();
    private TcpListener? _listener;

    public Server()
    {
        waitingQueue = new Queue<string>();
        roomWaitingQueue = new Queue<string>();
        userClients = new Dictionary<string, Dictionary<string, TcpClient>>();
        IsInMatch = new Dictionary<string, bool>();
    }

    public void Start(int port = 5000)
    {
        try
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            Console.WriteLine("Server started on port " + port);

            Thread matcher = new Thread(MatchmakingLoop);
            matcher.Start();

            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();

                Thread handleThread = new Thread(() => HandleClient(client));
                handleThread.Start();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void HandleClient(TcpClient client)
    {
        try
        {
            string msg = GetMessage(client);

            if (msg.Length == 0) return;

            if (msg.StartsWith("DISCONNECT:"))
            {
                string removedUsername = msg.Substring("DISCONNECT:".Length);
                RemoveUser(removedUsername);
                client.Close();
                return;
            }

            // Format: username|desiredOpponent
            string[] parts = msg.Split('|');
            string username = parts[0];
            string desiredOpponent = parts.Length > 1 ? parts[1] : "";

            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Empty username. Connection rejected.");
                client.Close();
                return;
            }

            lock (_lock)
            {
                if (!userClients.ContainsKey(username))
                {
                    userClients[username] = new Dictionary<string, TcpClient>
                    {
                        { desiredOpponent, client }
                    };

                    if (string.IsNullOrEmpty(desiredOpponent))
                    {
                        waitingQueue.Enqueue(username);
                        Console.WriteLine($"{username} added to general matchmaking queue.");
                    }
                    else
                    {
                        roomWaitingQueue.Enqueue(username);
                        Console.WriteLine($"{username} is waiting for a match with {desiredOpponent}.");
                    }

                    IsInMatch[username] = false;
                    Thread l = new Thread(() => ListenClient(client, username));
                    l.Start();
                }
                else
                {
                    Console.WriteLine($"Username {username} already connected.");
                    client.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error handling client: " + ex.Message);
        }
    }

    private void MatchmakingLoop()
    {
        try
        {
            while (true)
            {
                lock (_lock)
                {
                    // 1. Room-based matching
                    Queue<string> newRoomQueue = new Queue<string>();

                    while (roomWaitingQueue.Count > 0)
                    {
                        string user = roomWaitingQueue.Dequeue();

                        // Nếu user không còn trong danh sách, bỏ qua
                        if (!userClients.ContainsKey(user))
                            continue;

                        var pair = userClients[user];

                        // Nếu không có ai để ghép hoặc có lỗi, thì bỏ qua
                        if (pair.Count == 0) continue;

                        string desiredOpponent = pair.Keys.First();

                        // Nếu đối thủ không còn trong danh sách thì giữ user lại
                        if (!userClients.ContainsKey(desiredOpponent))
                        {
                            newRoomQueue.Enqueue(user);
                            continue;
                        }

                        // Nếu đối thủ cũng đang muốn đấu với user
                        if (userClients[desiredOpponent].ContainsKey(user))
                        {
                            TcpClient p1 = pair[desiredOpponent];
                            TcpClient p2 = userClients[desiredOpponent][user];

                            if (IsInMatch.ContainsKey(user)) IsInMatch[user] = true;
                            if (IsInMatch.ContainsKey(desiredOpponent)) IsInMatch[desiredOpponent] = true;

                            userClients.Remove(user);
                            userClients.Remove(desiredOpponent);

                            Thread matchThread = new Thread(() => StartMatch(user, p1, desiredOpponent, p2));
                            matchThread.Start();
                        }
                        else
                        {
                            newRoomQueue.Enqueue(user);
                        }
                    }

                    roomWaitingQueue = newRoomQueue;


                    // 2. Auto matching
                    while (waitingQueue.Count >= 2)
                    {
                        string user1 = waitingQueue.Dequeue();
                        string user2 = waitingQueue.Dequeue();

                        if (userClients.ContainsKey(user1) && userClients[user1].ContainsKey("") &&
                            userClients.ContainsKey(user2) && userClients[user2].ContainsKey(""))
                        {
                            TcpClient p1 = userClients[user1][""];
                            TcpClient p2 = userClients[user2][""];

                            if (IsInMatch.ContainsKey(user1)) IsInMatch[user1] = true;
                            if (IsInMatch.ContainsKey(user2)) IsInMatch[user2] = true;

                            userClients.Remove(user1);
                            userClients.Remove(user2);

                            Thread matchThread = new Thread(() => StartMatch(user1, p1, user2, p2));
                            matchThread.Start();
                        }
                    }
                }

                Thread.Sleep(500);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private void StartMatch(string user1, TcpClient p1, string user2, TcpClient p2)
    {
        try
        {
            SendTo(p1, $"SideTeam:1\nEnemyUsername:{user2}\n");
            SendTo(p2, $"SideTeam:2\nEnemyUsername:{user1}\n");

            Console.WriteLine($"Match started: {user1} vs {user2}");

            Thread t1 = new Thread(() => Relay(user1, p1, user2, p2));
            Thread t2 = new Thread(() => Relay(user2, p2, user1, p1));
            t1.Start();
            t2.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private void Relay(string userFrom, TcpClient client1, string userTo, TcpClient client2)
    {
        NetworkStream from = client1.GetStream();
        NetworkStream to = client2.GetStream();

        byte[] buffer = new byte[4096];
        try
        {
            while (true)
            {
                int len = from.Read(buffer, 0, buffer.Length);
                if (len == 0) break;
                string msg = Encoding.UTF8.GetString(buffer, 0, len).Trim();

                if (msg == "MatchEnd")
                {
                    break; 
                }
                else if (msg == "Surrender")
                {
                    to.Write(buffer, 0, len);
                    Console.WriteLine($"User {userFrom} surrendered!");
                    break;
                }
                else
                {
                    to.Write(buffer, 0, len);
                }
            }
        }
        catch
        {
            Console.WriteLine($"Player {userFrom} disconnected. Cleaning up match.");
            SendTo(client2, "opponent_disconnected\n");
        }
        finally 
        {
            if (IsInMatch.ContainsKey(userFrom)) IsInMatch.Remove(userFrom);
            if (IsInMatch.ContainsKey(userTo)) IsInMatch.Remove(userTo);
            RemoveUser(userFrom);
            RemoveUser(userTo);
            Console.WriteLine($"Match ended for {userFrom} and {userTo}");
        }
    }

    private void SendTo(TcpClient client, string message)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ListenClient(TcpClient client, string username)
    {
        try
        {
            while (IsInMatch.ContainsKey(username) && IsInMatch[username] == false)
            {
                string msg = GetMessage(client);
                if (msg.Length == 0)
                {
                    throw new Exception("User disconnected in finding match!");
                }

                if (msg.StartsWith("CancelFindMatch"))
                {
                    RemoveUser(username);
                    Console.WriteLine($"User {username} cancelled matchmaking.");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ListenClient error: {ex.Message}");
            RemoveUser(username);
        }
    }

    private string GetMessage(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4096];
            int len = stream.Read(buffer, 0, buffer.Length);
            if (len == 0)
            {
                client.Close();
                return "";
            }
            return Encoding.UTF8.GetString(buffer, 0, len).Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "";
        }
    }

    public void RemoveUser(string username)
    {
        try
        {
            lock (_lock)
            {
                if (userClients.ContainsKey(username))
                {
                    userClients.Remove(username);
                    Console.WriteLine($"Removed {username} from userClients");
                }

                waitingQueue = new Queue<string>(waitingQueue.Where(u => u != username));

                roomWaitingQueue = new Queue<string>(roomWaitingQueue.Where(u => u != username));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
