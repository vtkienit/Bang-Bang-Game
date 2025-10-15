using SplashKitSDK;
using System.Net.Sockets;
using System.Text;

namespace BangBang
{
    public class OnlinePlayer : Player
    {
        private TcpClient _client;

        public OnlinePlayer(User User, TcpClient Client, string Name, float X, float Y, int Side, bool Main, string Lane) : base(User, Name, X, Y, Side, Main, Lane)
        {
            _client = Client;
        }

        public override void Draw(float targetX, float targetY, float cameraX, float cameraY)
        {
            if (destroyed) return;
            base.Draw(targetX, targetY, cameraX, cameraY);
            SendMessage($"Gun:{targetX},{targetY},{_cameraX},{_cameraY}");
        }

        public override void Handle()
        {
            if (destroyed) return;
            if (SplashKit.AnyKeyPressed())
            {
                HandleMovingControl();
                HandleSkills();
            }

            HandleRecalling();

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Attack(this, 1, 0, _skillImages[0]);
                SendMessage("Skill0");
            }
        }

        protected override void HandleMovingControl()
        {
            float moveX = 0, moveY = 0;
            if (SplashKit.KeyDown(KeyCode.DKey))
            {
                moveX = 1;
                int x = (int)(_x + moveX);
                int y = (int)_y;
                if (_checkMove[y, x + 2]) moveX = 0;
            }
            if (SplashKit.KeyDown(KeyCode.AKey))
            {
                moveX = -1;
                int x = (int)(_x + moveX);
                int y = (int)_y;
                if (_checkMove[y, x - 2]) moveX = 0;
            }
            if (SplashKit.KeyDown(KeyCode.WKey))
            {
                moveY = -1;
                int x = (int)_x;
                int y = (int)(_y + moveY);
                if (_checkMove[y - 2, x]) moveY = 0;
            }
            if (SplashKit.KeyDown(KeyCode.SKey))
            {
                moveY = 1;
                int x = (int)_x;
                int y = (int)(_y + moveY);
                if (_checkMove[y + 2, x]) moveY = 0;
            }

            if (moveX != 0 || moveY != 0)
            {
                SendMessage($"Move:{moveX},{moveY}");
                HandleMoving(moveX, moveY);
            }
        }

        protected override void HandleSkills()
        {
            if (SplashKit.KeyDown(KeyCode.QKey))
            {
                Attack(this, 1, 1, _skillImages[1]);
                SendMessage("Skill1");
            }
                
            if (SplashKit.KeyDown(KeyCode.EKey))
            {
                Attack(this, 1, 2, _skillImages[2]);
                SendMessage("Skill2");
            }
                
            if (SplashKit.KeyDown(KeyCode.SpaceKey))
            {
                Attack(this, 1, 3, _skillImages[3]);
                SendMessage("Skill3");
            }

            if (SplashKit.KeyDown(KeyCode.BKey))
            {
                HandleRecall();
                SendMessage("Recall");
            }
        }

        private void SendMessage(string msg)
        {
            try
            {
                NetworkStream _stream = _client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
                _stream.Write(data, 0, data.Length);
            }
            catch {}
        }
    }
}
