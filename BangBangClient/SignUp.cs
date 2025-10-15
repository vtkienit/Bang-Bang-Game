using SplashKitSDK;

namespace BangBang
{
    public class SignUp : Page
    {
        private readonly DatabaseManager _dbManager;

        private InputBox _charNameInput, _usernameInput, _passwordInput, _confirmPasswordInput;

        private string _accepted = "";
        private Color _accepted_color;

        public SignUp(DatabaseManager dbManager)
        {
            _dbManager = dbManager;

            _charNameInput = new InputBox("Character Name", Page.WIDTH / 2 - 135, Page.HEIGHT / 2 - 110, 270, 40);
            _usernameInput = new InputBox("Username", Page.WIDTH / 2 - 135, Page.HEIGHT / 2 - 38, 270, 40);
            _passwordInput = new InputBox("Password", Page.WIDTH / 2 - 135, Page.HEIGHT / 2 + 35, 270, 40, isPassword: true);
            _confirmPasswordInput = new InputBox("Confirm Password", Page.WIDTH / 2 - 135, Page.HEIGHT / 2 + 107, 270, 40, isPassword: true);

        }

        public void Draw()
        {
            float spaceY = 73;

            SplashKit.DrawBitmap("Background2", 0, 0);

            float lenT = TextWidth("Bang Bang", "LuckiestGuy", 50);
            SplashKit.DrawText("Bang Bang", Color.RGBAColor(0, 255, 255, 255), "LuckiestGuy", 50, Page.WIDTH / 2 - lenT / 2, 60);

            // Background box
            float lenX = 330, lenY = 370 + spaceY;
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), Page.WIDTH / 2 - lenX / 2, Page.HEIGHT / 2 - 180, lenX, lenY);

            // Header
            lenT = TextWidth("Register", "RussoOne", 28);
            SplashKit.DrawText("Register", Color.White, "RussoOne", 28, Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 - 170);

            // Input boxes
            _charNameInput.Draw();
            _usernameInput.Draw();
            _passwordInput.Draw();
            _confirmPasswordInput.Draw();

            // Confirm button
            float xInit = 105, lenXB = 120, lenYB = 43, lenTB = TextWidth("Confirm", "RussoOne", 22);
            if (Hover(SplashKit.MousePosition(), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit + spaceY, lenXB, lenYB))
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 120, 50, 255), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit + spaceY, lenXB, lenYB);
                SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 255), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit + spaceY + lenYB - 3, lenXB, 3);
                SplashKit.DrawText("Confirm", Color.White, "RussoOne", 22, Page.WIDTH / 2 - lenTB / 2, Page.HEIGHT / 2 + xInit + spaceY + 6);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 170, 50, 255), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit + spaceY, lenXB, lenYB);
                SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 255), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit + spaceY + lenYB - 3, lenXB, 3);
                SplashKit.DrawText("Confirm", Color.White, "RussoOne", 22, Page.WIDTH / 2 - lenTB / 2, Page.HEIGHT / 2 + xInit + spaceY + 6);
            }

            lenT = TextWidth("Already have an account?", "RussoOne", 15);
            if (Hover(SplashKit.MousePosition(), Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 160 + spaceY, lenT, 40))
            {
                SplashKit.DrawText("Already have an account?", Color.Blue, "RussoOne", 15, Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 160 + spaceY);
            }
            else
            {
                SplashKit.DrawText("Already have an account?", Color.SteelBlue, "RussoOne", 15, Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 160 + spaceY);
            }

            lenT = TextWidth(_accepted, "RussoOne", 15);
            SplashKit.DrawText(_accepted, _accepted_color, "RussoOne", 15, Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 81 + spaceY);
        }

        public void Handle()
        {
            _charNameInput.HandleInput();
            _usernameInput.HandleInput();
            _passwordInput.HandleInput();
            _confirmPasswordInput.HandleInput();

            float spaceY = 73;
            float lenX = 330, lenY = 370 + spaceY;
            float xInit = 105, lenXB = 120, lenYB = 43;
            float lenT = TextWidth("Already have an account?", "RussoOne", 15);

            // Confirm button
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (Hover(SplashKit.MousePosition(), Page.WIDTH / 2 - lenXB / 2, Page.HEIGHT / 2 + xInit + spaceY, lenXB, lenYB))
                {
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");

                    string characterName = _charNameInput.Text.Trim();
                    string username = _usernameInput.Text.Trim();
                    string password = _passwordInput.Text;
                    string confirmPassword = _confirmPasswordInput.Text;

                    if (characterName == "" || username == "" || password == "" || confirmPassword == "")
                    {
                        _accepted = "The input cannot be empty!";
                        _accepted_color = Color.Red;
                    }
                    else if (characterName.Length < 3 || username.Length < 3 || password.Length < 3 || confirmPassword.Length < 3)
                    {
                        _accepted = "At least 3 chars per input!";
                        _accepted_color = Color.Red;
                    }
                    else if (confirmPassword != password)
                    {
                        _accepted = "Passwords do not match!";
                        _accepted_color = Color.Red;
                    }
                    else if (CheckUser(username))
                    {
                        _accepted = "User created successfully!";
                        _accepted_color = Color.Lime;
                        _dbManager.CreateUser(characterName, username, password);
                    }
                    else if (!CheckUser(username))
                    {
                        _accepted = "The user already exists!";
                        _accepted_color = Color.Red;
                    }
                    else
                    {
                        _accepted = "Connection error!";
                        _accepted_color = Color.Red;
                    }
                }
                else if (Hover(SplashKit.MousePosition(), Page.WIDTH / 2 - lenT / 2, Page.HEIGHT / 2 + 160 + spaceY, lenT, 40))
                {
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                    _accepted = "";
                    _charNameInput.Text = "";
                    _usernameInput.Text = "";
                    _passwordInput.Text = "";
                    _confirmPasswordInput.Text = "";
                    GameManager.ScreenType = "SignIn";
                }
            }
        }

        private bool Hover(Point2D mouse, float x, float y, float w, float h)
        {
            if (SplashKit.PointInRectangle(mouse, SplashKit.RectangleFrom(x, y, w, h)))
            {
                return true;
            }
            return false;
        }

        private bool CheckUser(string username)
        {
            var users = _dbManager.ListAllUsers();
            foreach (var user in users)
            {
                if (user.Contains("Username") && user["Username"] == username)
                {
                    return false;
                }
            }
            return true;
        }

        private float TextWidth(string letters, string font, int size_font)
        {
            return SplashKit.TextWidth(letters, font, size_font);
        }
    }
}
