using SplashKitSDK;
using System.Net.Sockets;
using System.Text;

namespace BangBang
{
    public class Match
    {
        private Battle? _offlineBattle, _onlineBattle;
        private DatabaseManager _dbManager;
        private User _user;

        public static int Side, Level;
        public static bool Start;
        public static string EnemyUsm = "", EnemyTank = "", EnemyGun = "";
        public static TcpClient? Client;
        public static string Mode = "Offline";

        public Match(DatabaseManager dbManager, User user)
        {
            _dbManager = dbManager;
            _user = user;
            Level = 1;
        }

        public void Draw()
        {
            if (Mode == "Online" && Start)
            {
                _onlineBattle.Draw();
            }
            else if (Start)
            {
                _offlineBattle.Draw();
            }
        }

        public void Handle()
        {
            if (Mode == "Online" && Start)
            {
                _onlineBattle.Handle();
            }
            else if (Start)
            {
                _offlineBattle.Handle();
            }
        }

        public void Update()
        {
            if (Mode == "Online")
            {
                if (!Start)
                {
                    _onlineBattle = new OnlineBattle(_dbManager, _user, Client, EnemyUsm, EnemyTank, EnemyGun, 2, Side);
                    Start = true;
                }
                else
                    _onlineBattle.Update();
            }
            else
            {
                if (!Start)
                {
                    _offlineBattle = new OfflineBattle(_dbManager, _user, Client, 1, 1, Level);
                    Start = true;
                }
                else
                    _offlineBattle.Update();
            }
        }

        private void SendMessage(string msg)
        {       
            NetworkStream _stream = Client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
            _stream.Write(data, 0, data.Length);
        }

        private string GetMessage()
        {
            NetworkStream _stream = Client.GetStream();
            byte[] buffer = new byte[4096];
            int len = _stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, len);
        }
    }
}
