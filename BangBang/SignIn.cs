using MongoDB.Bson;
using SplashKitSDK;

namespace BangBang
{
    public class SignIn: Page
    {
        private DatabaseManager _dbManager;
        private User _user;

        private InputBox _usernameInput;
        private InputBox _passwordInput;

        private bool _logined = false;
        private string _accepted = "";
        private Color _accepted_color;

        public SignIn(DatabaseManager dbManager, User user)
        {
            _dbManager = dbManager;
            _user = user;

            _usernameInput = new InputBox("Username", Page.WIDTH / 2 - 135, Page.HEIGHT / 2 - 110, 270, 40);
            _passwordInput = new InputBox("Password", Page.WIDTH / 2 - 135, Page.HEIGHT / 2 - 35, 270, 40, isPassword: true);
        }

        public void Draw()
        {
            SplashKit.DrawBitmap("Background2", 0, 0);

            float lenT = TextWidth("Bang Bang", "LuckiestGuy", 50);
            SplashKit.DrawText("Bang Bang", Color.RGBAColor(0, 255, 255, 255), "LuckiestGuy", 50, Page.WIDTH / 2 - lenT / 2, 60);

            // Background box
            float lenX = 330;
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), Page.WIDTH / 2 - lenX / 2, Page.HEIGHT / 2 - 180, lenX, 310);

            // Header
            lenT = TextWidth("Sign In", "RussoOne", 26);
            SplashKit.DrawText("Sign In", Color.White, "RussoOne", 26, Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 - 170);

            _usernameInput.Draw();
            _passwordInput.Draw();

            float xInit = 37, lenXB = 120, lenYB = 43, lenTB = TextWidth("Login", "RussoOne", 22);
            if (Hover(SplashKit.MousePosition(), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit, lenXB, lenYB))
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 50, 120, 255), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit, lenXB, lenYB);
                SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 255), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit + lenYB - 3, lenXB, 3);
                SplashKit.DrawText("Login", Color.White, "RussoOne", 22, Page.WIDTH / 2 - lenTB / 2, Page.HEIGHT / 2 + xInit + 6);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 50, 220, 255), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit, lenXB, lenYB);
                SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 255), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit + lenYB - 3, lenXB, 3);
                SplashKit.DrawText("Login", Color.White, "RussoOne", 22, Page.WIDTH / 2 - lenTB / 2, Page.HEIGHT / 2 + xInit + 6);
            }

            lenT = TextWidth("Don't have an account?", "RussoOne", 15);
            if (Hover(SplashKit.MousePosition(), Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 95, lenT, 40))
            {
                SplashKit.DrawText("Don't have an account?", Color.Blue, "RussoOne", 15, Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 95);
            }
            else
            {
                SplashKit.DrawText("Don't have an account?", Color.SteelBlue, "RussoOne", 15, Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 95);
            }

            lenT = TextWidth(_accepted, "RussoOne", 14);
            SplashKit.DrawText(_accepted, _accepted_color, "RussoOne", 14, Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 13);
        }

        public void Handle()
        {
            _usernameInput.HandleInput();
            _passwordInput.HandleInput();

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                // Handle login button
                float xInit = 37, lenXB = 120, lenYB = 43;
                float lenT = TextWidth("Don't have an account?", "RussoOne", 15);

                if (Hover(SplashKit.MousePosition(), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit, lenXB, lenYB))
                {
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");

                    string username = _usernameInput.Text;
                    string password = _passwordInput.Text;

                    if (username == "" || password == "")
                    {
                        _logined = false;
                        _accepted = "The input cannot be empty!";
                        _accepted_color = Color.Red;
                    }
                    else if (CheckUserLogin(username, password))
                    {
                        _dbManager.UpdateUserField2(username, "Status", "Online");
                        LoadResources(username);

                        _logined = true;
                        _accepted = "Login successful!";
                        _accepted_color = Color.Green;
                        _usernameInput.Text = "";
                        _passwordInput.Text = "";
                        GameManager.ScreenType = "Home";
                        _accepted = "";
                    }
                    else
                    {
                        _logined = false;
                        _accepted = "Username or Password is incorrect!";
                        _accepted_color = Color.Red;
                    }
                }
                else if (Hover(SplashKit.MousePosition(), Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 95, lenT, 40))
                {
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");

                    _accepted = "";
                    _usernameInput.Text = "";
                    _passwordInput.Text = "";
                    GameManager.ScreenType = "SignUp";
                }
            }
        }

        private void LoadResources(string Username)
        {
            var DataUser = _dbManager.GetUserByUsername(Username);
            _user.Name = DataUser["CharacterName"].ToString();
            _user.Username = Username;
            _user.TankUsed = DataUser["Tank Used"].ToString();
            _user.GunUsed = DataUser["Gun Used"].ToString();
            _user.FrameUsed = DataUser["Frame Used"].ToString();
            _user.AvatarUsed = DataUser["Avatar Used"].ToString();
            _user.ItemUsed = DataUser["Item Used"].ToString();
            _user.Coin = (int)DataUser["Coin"];

            LoadItems(_dbManager.GetUserItems(Username, "Tank"), "Tank", 1);
            LoadItems(_dbManager.GetUserItems(Username, "Gun"), "Gun", 3);
            LoadItems(_dbManager.GetUserItems(Username, "Frame"), "Frame", 2);
            LoadItems(_dbManager.GetUserItems(Username, "Avatar"), "Avatar", 2);
            LoadItems(_dbManager.GetUserItems(Username, "Item"), "Item", 2);
        }

        private void LoadItems(List<BsonDocument>? items, string typeAdd, int typeItem)
        {
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
                _user.Bag.AddItem(typeAdd, item["ID"].ToString(), item["Image"].ToString(), item["Name"].ToString(), "", item["Desc"].ToString(), (int)item["Price"], typeItem);
        }

        private bool Hover(Point2D mouse, float x, float y, float w, float h)
        {
            if (SplashKit.PointInRectangle(mouse, SplashKit.RectangleFrom(x, y, w, h)))
            {
                return true;
            }
            return false;
        }

        private bool CheckUserLogin(string username, string password)
        {
            var users = _dbManager.ListAllUsers();
            foreach (var user in users)
            {
                if (user.Contains("Username") && user["Username"] == username && user.Contains("Password") && user["Password"] == password)
                {
                    return true;
                }
            }
            return false;
        }

        private float TextWidth(string letters, string font, int size_font)
        {
            return SplashKit.TextWidth(letters, font, size_font);
        }

        public bool Logined
        {
            get {  return _logined; }
        }

    }
}
