using SplashKitSDK;
using System.Net.Sockets;
using System.Text;

namespace BangBang
{
    public class Home:Page
    {
        private DatabaseManager _dbManager;
        private User _user;
        private Bitmap? _frame, _avatar;
        private bool _openMode, _findingMatch;
        private TcpClient? _client;
        private Clock _clock;

        public static bool ConnectServer;

        public Home(DatabaseManager dbManager, User user) 
        { 
            _dbManager = dbManager;
            _user = user;
            _clock = new Clock();

            // Backgrounds
            SplashKit.LoadBitmap("BackgroundHome", "Images/BackgroundHome.png");
            SplashKit.LoadBitmap("BackgroundShop", "Images/BackgroundShop.png");

            // Buttons
            SplashKit.LoadBitmap("Music", "Images/Music.png");
            SplashKit.LoadBitmap("MusicHover", "Images/MusicHover.png");
            SplashKit.LoadBitmap("Coin", "Images/Coin.png");
            SplashKit.LoadBitmap("Return_Button", "Images/Return_Button.png");
            SplashKit.LoadBitmap("Return_Button_Hover", "Images/Return_Button_Hover.png");
            SplashKit.LoadBitmap("HomeTank", "Images/HomeTank.png");
            SplashKit.LoadBitmap("Logout_Button", "Images/Logout_Button.png");
            SplashKit.LoadBitmap("Logout_Button_Hover", "Images/Logout_Button_Hover.png");
            SplashKit.LoadBitmap("Friends_Button", "Images/Friends_Button.png");
            SplashKit.LoadBitmap("Friends_Button_Hover", "Images/Friends_Button_Hover.png");
            SplashKit.LoadBitmap("GiftBox", "Images/GiftBox.png");
            SplashKit.LoadBitmap("GiftBox_Hover", "Images/GiftBox_Hover.png");
            SplashKit.LoadBitmap("Club", "Images/Club.png");
            SplashKit.LoadBitmap("Club_Hover", "Images/Club_Hover.png");
            SplashKit.LoadBitmap("LV_Bar1", "Images/LV_Bar1.png");
            SplashKit.LoadBitmap("Close_Button", "Images/Close_Button.png");
            SplashKit.LoadBitmap("Close_Button_Hover", "Images/Close_Button_Hover.png");
            SplashKit.LoadBitmap("Close", "Images/Close.png");
            SplashKit.LoadBitmap("CloseHover", "Images/CloseHover.png");

        }

        private void DrawMenu()
        {
            SplashKit.DrawBitmap("BackgroundHome", 0, 0);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 140), 144, 15, 552, 50);

            int menu_width = (int)TextWidth("Home", "RussoOne", 20);
            if (GameManager.ScreenType == "Home")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), 145, 18, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, 145, 60, 109, 3);
                SplashKit.DrawText("Home", Color.Black, "RussoOne", 20, 170, 28);
            }
            else if (Hover(SplashKit.MousePosition(), 145, 18, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, 145, 60, 110, 3);
                SplashKit.DrawText("Home", Color.Yellow, "RussoOne", 20, 170, 28);
            }
            else
                SplashKit.DrawText("Home", Color.White, "RussoOne", 20, 170, 28);

            int locate_menu = 170 + (int)TextWidth("Home", "RussoOne", 20) + 50;
            menu_width = (int)TextWidth("Shop", "RussoOne", 20);
            if (GameManager.ScreenType == "Shop")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), locate_menu - 25, 18, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, locate_menu - 25, 60, menu_width + 50, 3);
                SplashKit.DrawText("Shop", Color.Black, "RussoOne", 20, locate_menu, 28);
            }
            else if (Hover(SplashKit.MousePosition(), locate_menu - 25, 18, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, locate_menu - 25, 60, menu_width + 50, 3);
                SplashKit.DrawText("Shop", Color.Yellow, "RussoOne", 20, locate_menu, 28);
            }
            else
                SplashKit.DrawText("Shop", Color.White, "RussoOne", 20, locate_menu, 28);

            locate_menu += (int)TextWidth("Shop", "RussoOne", 20) + 50;
            menu_width = (int)TextWidth("Bag", "RussoOne", 20);
            if (GameManager.ScreenType == "Bag")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), locate_menu - 25, 18, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, locate_menu - 25, 60, menu_width + 50, 3);
                SplashKit.DrawText("Bag", Color.Black, "RussoOne", 20, locate_menu, 28);
            }
            else if (Hover(SplashKit.MousePosition(), locate_menu - 25, 18, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, locate_menu - 25, 60, menu_width + 50, 3);
                SplashKit.DrawText("Bag", Color.Yellow, "RussoOne", 20, locate_menu, 28);
            }
            else
                SplashKit.DrawText("Bag", Color.White, "RussoOne", 20, locate_menu, 28);

            locate_menu += (int)TextWidth("Bag", "RussoOne", 20) + 50;
            menu_width = (int)TextWidth("Tutorial", "RussoOne", 20);
            if (GameManager.ScreenType == "Tutorial")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), locate_menu - 25, 18, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, locate_menu - 25, 60, menu_width + 50, 3);
                SplashKit.DrawText("Tutorial", Color.Black, "RussoOne", 20, locate_menu, 28);
            }
            else if (Hover(SplashKit.MousePosition(), locate_menu - 25, 18, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, locate_menu - 25, 60, menu_width + 50, 3);
                SplashKit.DrawText("Tutorial", Color.Yellow, "RussoOne", 20, locate_menu, 28);
            }
            else
                SplashKit.DrawText("Tutorial", Color.White, "RussoOne", 20, locate_menu, 28);

            locate_menu += (int)TextWidth("Tutorial", "RussoOne", 20) + 50;
            menu_width = (int)TextWidth("Setting", "RussoOne", 20);
            if (GameManager.ScreenType == "Setting")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), locate_menu - 25, 18, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, locate_menu - 25, 60, menu_width + 50, 3);
                SplashKit.DrawText("Setting", Color.Black, "RussoOne", 20, locate_menu, 28);
            }
            else if (Hover(SplashKit.MousePosition(), locate_menu - 25, 18, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, locate_menu - 25, 60, menu_width + 50, 3);
                SplashKit.DrawText("Setting", Color.Yellow, "RussoOne", 20, locate_menu, 28);
            }
            else
                SplashKit.DrawText("Setting", Color.White, "RussoOne", 20, locate_menu, 28);


            if (Hover(SplashKit.MousePosition(), 843, 15, 46, 45))
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), 843, 15, 46, 45);
                SplashKit.DrawBitmap("Logout_Button_Hover", 845, 20);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), 843, 15, 46, 45);
                SplashKit.DrawBitmap("Logout_Button", 845, 20);
            }

            if (Hover(SplashKit.MousePosition(), 10, 150, 30 + (int)TextWidth("Friends", "RussoOne", 18), 30))
            {
                SplashKit.DrawBitmap("Friends_Button_Hover", 10, 150);
                SplashKit.DrawText("Friends", Color.RGBAColor(0, 255, 255, 255), "RussoOne", 18, 46, 153);
            }
            else
            {
                SplashKit.DrawBitmap("Friends_Button", 10, 150);
                SplashKit.DrawText("Friends", Color.White, "RussoOne", 18, 46, 153);
            }

            if (Hover(SplashKit.MousePosition(), 8, 210, 30 + (int)TextWidth("Club", "RussoOne", 18), 30))
            {
                SplashKit.DrawBitmap("Club_Hover", 8, 210);
                SplashKit.DrawText("Club", Color.RGBAColor(0, 255, 255, 255), "RussoOne", 18, 46, 213);
            }
            else
            {
                SplashKit.DrawBitmap("Club", 8, 210);
                SplashKit.DrawText("Club", Color.White, "RussoOne", 18, 46, 213);
            }

            if (Hover(SplashKit.MousePosition(), 10, 270, 30 + (int)TextWidth("Gift Box", "RussoOne", 18), 30))
            {
                SplashKit.DrawBitmap("GiftBox_Hover", 10, 270);
                SplashKit.DrawText("Gift Box", Color.RGBAColor(0, 255, 255, 255), "RussoOne", 18, 46, 273);
            }
            else
            {
                SplashKit.DrawBitmap("GiftBox", 10, 270);
                SplashKit.DrawText("Gift Box", Color.White, "RussoOne", 18, 46, 273);
            }

        }

        private void DrawAvatar()
        {
            _avatar = _user.Bag.LocateItem(_user.Bag.Avatars, _user.AvatarUsed).Image;
            if (_avatar != null) 
                SplashKit.DrawBitmap(_avatar, 18, 20);

            _frame = _user.Bag.LocateItem(_user.Bag.Frames, _user.FrameUsed).Image;
            if (_frame != null)
                SplashKit.DrawBitmap(_frame, 3, 3);

            // Level Bar
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 255), 18, 100, 70, 18);
            SplashKit.FillRectangle(Color.RGBAColor(76, 175, 80, 255), 18, 100, 1, 18);
            float x_lv = SplashKit.TextWidth("1", "RussoOne", 15);
            SplashKit.DrawText("19", Color.White, "RussoOne", 15, 18 + 70 / 2 - x_lv / 2, 100);

            // Coin
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), 760, 17, 70, 30);
            SplashKit.DrawBitmap("Coin", 765, 17);
            SplashKit.DrawText(_user.Coin.ToString(), Color.White, "RussoOne", 18, 800, 20);
        }

        private void DrawTank()
        {
            SplashKit.DrawBitmap("HomeTank", 450 - 270, 100);
        }

        private void DrawNews()
        {
            if (Hover(SplashKit.MousePosition(), 725, 150, 160, 70))
            {
                SplashKit.FillRectangle(Color.RGBAColor(225, 100, 40, 255), 725, 150, 160, 70);

                SplashKit.FillRectangle(Color.White, 725, 146, 160, 4);

                SplashKit.FillTriangle(
                    Color.RGBAColor(255, 255, 255, 30),
                    725, 150,
                    885, 150,
                    725, 220
                );

                SplashKit.DrawText("News", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 35, 727 + 160 / 2 - TextWidth("News", "RussoOne", 35) / 2, 164);

                SplashKit.DrawText("News", Color.White, "RussoOne", 35, 725 + 160 / 2 - TextWidth("News", "RussoOne", 35) / 2, 162);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(255, 130, 60, 255), 725, 150, 160, 70);

                SplashKit.FillRectangle(Color.White, 725, 146, 160, 4);

                SplashKit.FillTriangle(
                    Color.RGBAColor(255, 255, 255, 50),
                    725, 150,
                    885, 150,
                    725, 220
                );

                SplashKit.DrawText("News", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 35, 727 + 160 / 2 - TextWidth("News", "RussoOne", 35) / 2, 164);

                SplashKit.DrawText("News", Color.White, "RussoOne", 35, 725 + 160 / 2 - TextWidth("News", "RussoOne", 35) / 2, 162);
            }

            if (Hover(SplashKit.MousePosition(), 725, 240, 160, 70))
            {
                SplashKit.FillRectangle(Color.RGBAColor(126, 55, 225, 255), 725, 240, 160, 70);
                SplashKit.FillRectangle(Color.White, 725, 236, 160, 4);
                SplashKit.FillEllipse(Color.RGBAColor(255, 255, 255, 45), 725 + 20, 285, 120, 25);

                SplashKit.DrawText("Events", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 35, 727 + 160 / 2 - TextWidth("Events", "RussoOne", 35) / 2, 254);
                SplashKit.DrawText("Events", Color.White, "RussoOne", 35, 725 + 160 / 2 - TextWidth("Events", "RussoOne", 35) / 2, 252);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(156, 85, 255, 255), 725, 240, 160, 70);
                SplashKit.FillRectangle(Color.White, 725, 236, 160, 4);
                SplashKit.FillEllipse(Color.RGBAColor(255, 255, 255, 25), 730, 285, 110, 20);

                SplashKit.DrawText("Events", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 35, 727 + 160 / 2 - TextWidth("Events", "RussoOne", 35) / 2, 254);
                SplashKit.DrawText("Events", Color.White, "RussoOne", 35, 725 + 160 / 2 - TextWidth("Events", "RussoOne", 35) / 2, 252);
            }

        }

        private void DrawStart()
        {
            if (Hover(SplashKit.MousePosition(), 725, 470, 160, 35))
            {
                SplashKit.FillRectangle(Color.RGBAColor(25, 118, 210, 255), 725, 470, 160, 35);

                if (Match.Mode == "Offline")
                {
                    int fsize = 17;

                    string text = $"Mode: {Match.Mode} ({Match.Level})";
                    SplashKit.DrawText(text, Color.RGBAColor(0, 0, 0, 100), "RussoOne", fsize, 727 + 160 / 2 - TextWidth(text, "RussoOne", fsize) / 2, 479);
                    SplashKit.DrawText(text, Color.White, "RussoOne", fsize, 725 + 160 / 2 - TextWidth(text, "RussoOne", fsize) / 2, 477);
                }
                else
                {
                    SplashKit.DrawText($"Mode: {Match.Mode}", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 18, 727 + 160 / 2 - TextWidth($"Mode: {Match.Mode}", "RussoOne", 18) / 2, 478);
                    SplashKit.DrawText($"Mode: {Match.Mode}", Color.White, "RussoOne", 18, 725 + 160 / 2 - TextWidth($"Mode: {Match.Mode}", "RussoOne", 18) / 2, 476);
                }
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(33, 150, 243, 255), 725, 470, 160, 35);

                if (Match.Mode == "Offline")
                {
                    int fsize = 17;

                    string text = $"Mode: {Match.Mode} ({Match.Level})";
                    SplashKit.DrawText(text, Color.RGBAColor(0, 0, 0, 100), "RussoOne", fsize, 727 + 160 / 2 - TextWidth(text, "RussoOne", fsize) / 2, 479);
                    SplashKit.DrawText(text, Color.White, "RussoOne", fsize, 725 + 160 / 2 - TextWidth(text, "RussoOne", fsize) / 2, 477);
                }
                else
                {
                    SplashKit.DrawText($"Mode: {Match.Mode}", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 18, 727 + 160 / 2 - TextWidth($"Mode: {Match.Mode}", "RussoOne", 18) / 2, 478);
                    SplashKit.DrawText($"Mode: {Match.Mode}", Color.White, "RussoOne", 18, 725 + 160 / 2 - TextWidth($"Mode: {Match.Mode}", "RussoOne", 18) / 2, 476);
                }
            }

            if (_findingMatch)
            {
                SplashKit.FillRectangle(Color.RGBAColor(126, 87, 194, 255), 725, 515, 160, 70);
                SplashKit.FillRectangle(Color.White, 725, 585 - 5, 160, 5);

                SplashKit.FillEllipse(Color.RGBAColor(255, 255, 200, 30), 725 + 45, 532, 70, 25);

                if (Hover(SplashKit.MousePosition(), 725 + 160 - 20, 515, 20, 20))
                    SplashKit.DrawBitmap("CloseHover", 725 + 160 - 20, 515);
                else
                    SplashKit.DrawBitmap("Close", 725 + 160 - 20, 515);

                _clock.Draw(30, 725 + 160 / 2, 525);
                SplashKit.DrawText("Finding Match", Color.LightGray, "RussoOne", 17, 725 + 160 / 2 - TextWidth("Finding Match", "RussoOne", 17) / 2, 555);
            }
            else
            {
                if (Hover(SplashKit.MousePosition(), 725, 515, 160, 70))
                {
                    SplashKit.FillRectangle(Color.RGBAColor(212, 160, 25, 255), 725, 515, 160, 70);
                    SplashKit.FillRectangle(Color.White, 725, 585 - 5, 160, 5);
                    SplashKit.FillEllipse(Color.RGBAColor(255, 255, 200, 55), 725 + 40, 530, 80, 30);

                    SplashKit.FillTriangle(
                        Color.RGBAColor(255, 255, 255, 30),
                        725 + 10, 515 + 10,
                        725 + 150, 515 + 10,
                        725 + 80, 515 + 30
                    );

                    SplashKit.DrawText("Start", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 45, 727 + 160 / 2 - TextWidth("Start", "RussoOne", 45) / 2, 523);
                    SplashKit.DrawText("Start", Color.White, "RussoOne", 45, 725 + 160 / 2 - TextWidth("Start", "RussoOne", 45) / 2, 521);
                }
                else
                {
                    SplashKit.FillRectangle(Color.RGBAColor(251, 192, 45, 255), 725, 515, 160, 70);
                    SplashKit.FillRectangle(Color.White, 725, 585 - 5, 160, 5);

                    SplashKit.FillEllipse(Color.RGBAColor(255, 255, 200, 30), 725 + 45, 532, 70, 25);

                    SplashKit.DrawText("Start", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 45, 727 + 160 / 2 - TextWidth("Start", "RussoOne", 45) / 2, 523);
                    SplashKit.DrawText("Start", Color.White, "RussoOne", 45, 725 + 160 / 2 - TextWidth("Start", "RussoOne", 45) / 2, 521);
                }
            }
        }

        private void DrawMode()
        {
            if (!_openMode) return;
            float x0 = 450, y0 = 300, x = 300, y = 320;
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 220), x0 - x / 2, y0 - y / 2, x, y);
            SplashKit.DrawText("Mode", Color.Gold, "RussoOne", 25, x0 - TextWidth("Mode", "RussoOne", 25) / 2, y0 - y / 2 + 10);

            int typeM;
            if (Match.Mode == "Offline") typeM = Match.Level - 1;
            else typeM = 3;

            for (int i = 0; i < 4; i++)
            {
                float lenX = 210, lenY = 50, spaceY = 65;
                if (i == typeM)
                {
                    SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 230), x0 - lenX / 2, y0 - y / 2 + 50 + spaceY * i, lenX, lenY);
                    SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 230), x0 - lenX / 2, y0 - y / 2 + 50 + spaceY * i + lenY - 3, lenX, 3);
                }
                else if (Hover(SplashKit.MousePosition(), x0 - lenX / 2, y0 - y / 2 + 50 + spaceY * i, lenX, lenY))
                    SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 100), x0 - lenX / 2, y0 - y / 2 + 50 + spaceY * i, lenX, lenY);
                else
                    SplashKit.FillRectangle(Color.RGBAColor(211, 211, 211, 100), x0 - lenX / 2, y0 - y / 2 + 50 + spaceY * i, lenX, lenY);

                string text;
                if (i == 0) text = "Offline 1 (Easy)";
                else if (i == 1) text = "Offline 2 (Medium)";
                else if (i == 2) text = "Offline 3 (Hard)";
                else text = "Online";

                SplashKit.DrawText(text, Color.White, "RussoOne", 19, x0 - TextWidth(text, "RussoOne", 19) / 2, y0 - y / 2 + 63 + spaceY * i);
            }
        }

        private void DrawTutorial()
        {
            if (GameManager.ScreenType != "Tutorial") return;

            float x = 450, y = 300, lenX = 500, lenY = 380; 
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), x - lenX / 2, y - lenY / 2, lenX, lenY);

            float lenT = TextWidth("Tutorial", "RussoOne", 30);
            SplashKit.DrawText("Tutorial", Color.White, "RussoOne", 30, x - lenT / 2, y - lenY / 2 + 8);

            if (Hover(SplashKit.MousePosition(), x + lenX / 2 - 30, y - lenY / 2 + 10, 20, 20))
                SplashKit.DrawBitmap("CloseHover", x + lenX / 2 - 30, y - lenY / 2 + 10);
            else 
                SplashKit.DrawBitmap("Close", x + lenX / 2 - 30, y - lenY / 2 + 10);

            lenT = TextWidth("Move:", "RussoOne", 17);
            SplashKit.DrawText("Move:", Color.Gold, "RussoOne", 17, x - lenX / 2 + 20, y - lenY / 2 + 60 + 5);
            SplashKit.DrawText("Press", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 10, y - lenY / 2 + 62 + 5);
            SplashKit.DrawText("W A S D", Color.Red, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 59, y - lenY / 2 + 62 + 5);
            SplashKit.DrawText("to move Up, Left, Down, Right", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 130, y - lenY / 2 + 62 + 5);

            lenT = TextWidth("Normal Attack:", "RussoOne", 17);
            SplashKit.DrawText("Normal Attack:", Color.Gold, "RussoOne", 17, x - lenX / 2 + 20, y - lenY / 2 + 100 + 5);
            SplashKit.DrawText("Click", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 10, y - lenY / 2 + 102 + 5);
            SplashKit.DrawText("Right Mouse", Color.Red, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 55, y - lenY / 2 + 102 + 5);
            SplashKit.DrawText("to shoot", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 160, y - lenY / 2 + 102 + 5);

            lenT = TextWidth("Skill 1:", "RussoOne", 17);
            SplashKit.DrawText("Skill 1:", Color.Gold, "RussoOne", 17, x - lenX / 2 + 20, y - lenY / 2 + 140 + 5);
            SplashKit.DrawText("Press", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 10, y - lenY / 2 + 142 + 5);
            SplashKit.DrawText("Q", Color.Red, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 59, y - lenY / 2 + 142 + 5);
            SplashKit.DrawText("to activate", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 75, y - lenY / 2 + 142 + 5);

            lenT = TextWidth("Skill 2:", "RussoOne", 17);
            SplashKit.DrawText("Skill 2:", Color.Gold, "RussoOne", 17, x - lenX / 2 + 20, y - lenY / 2 + 180 + 5);
            SplashKit.DrawText("Press", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 10, y - lenY / 2 + 182 + 5);
            SplashKit.DrawText("E", Color.Red, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 59, y - lenY / 2 + 182 + 5);
            SplashKit.DrawText("to activate", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 75, y - lenY / 2 + 182 + 5);

            lenT = TextWidth("Skill 3:", "RussoOne", 17);
            SplashKit.DrawText("Skill 3:", Color.Gold, "RussoOne", 17, x - lenX / 2 + 20, y - lenY / 2 + 220 + 5);
            SplashKit.DrawText("Press", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 10, y - lenY / 2 + 222 + 5);
            SplashKit.DrawText("Space", Color.Red, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 59, y - lenY / 2 + 222 + 5);
            SplashKit.DrawText("to activate", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 110, y - lenY / 2 + 222 + 5);

            lenT = TextWidth("Recall:", "RussoOne", 17);
            SplashKit.DrawText("Recall:", Color.Gold, "RussoOne", 17, x - lenX / 2 + 20, y - lenY / 2 + 260 + 5);
            SplashKit.DrawText("Press", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 10, y - lenY / 2 + 262 + 5);
            SplashKit.DrawText("B", Color.Red, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 59, y - lenY / 2 + 262 + 5);
            SplashKit.DrawText("to activate", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 75, y - lenY / 2 + 262 + 5);

            lenT = TextWidth("Rules of Game:", "RussoOne", 17);
            SplashKit.DrawText("Rules of Game:", Color.Gold, "RussoOne", 17, x - lenX / 2 + 20, y - lenY / 2 + 300 + 5);
            SplashKit.DrawText("Destroy", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 10, y - lenY / 2 + 302 + 5);
            SplashKit.DrawText("Main Tower", Color.Red, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 75, y - lenY / 2 + 302 + 5);
            SplashKit.DrawText("to win", Color.White, "RussoOne", 15, x - lenX / 2 + 20 + lenT + 170, y - lenY / 2 + 302 + 5);

            lenT = TextWidth("Game created by Vu Trung Kien (ID: 104849906)", "RussoOne", 13);
            SplashKit.DrawText("Game created by Vu Trung Kien (ID: 104849906)", Color.Red, "RussoOne", 13, x - lenT / 2, y + lenY / 2 - 30);
        }

        private void DrawSetting()
        {
            if (GameManager.ScreenType != "Setting") return;

            float x = 450, y = 300, lenX = 400, lenY = 270;

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 210), x - lenX / 2, y - lenY / 2, lenX, lenY);

            float lenT = TextWidth("Setting", "RussoOne", 30);
            SplashKit.DrawText("Setting", Color.White, "RussoOne", 30, x - lenT / 2, y - lenY / 2 + 8);

            if (GameManager.MuteMusic)
                SplashKit.DrawBitmap("MusicHover", x - lenX / 2 + 80, y - lenY / 2 + 70);
            else 
                SplashKit.DrawBitmap("Music", x - lenX / 2 + 80, y - lenY / 2 + 70);

            SplashKit.DrawText("Background Music", Color.LightGray, "RussoOne", 20, x - lenX / 2 + 120, y - lenY / 2 + 73);

            if (GameManager.MuteSound)
                SplashKit.DrawBitmap("MusicHover", x - lenX / 2 + 80, y - lenY / 2 + 130);
            else 
                SplashKit.DrawBitmap("Music", x - lenX / 2 + 80, y - lenY / 2 + 130);

            SplashKit.DrawText("Sound Effect", Color.LightGray, "RussoOne", 20, x - lenX / 2 + 120, y - lenY / 2 + 133);

            float lenXB = 120, lenYB = 45;
            if (Hover(SplashKit.MousePosition(), x - lenXB / 2, y + lenY / 2 - lenYB - 30, lenXB, lenYB))
                SplashKit.FillRectangle(Color.RGBAColor(0, 120, 200, 200), x - lenXB / 2, y + lenY / 2 - lenYB - 30, lenXB, lenYB);
            else
                SplashKit.FillRectangle(Color.RGBAColor(0, 170, 255, 200), x - lenXB / 2, y + lenY / 2 - lenYB - 30, lenXB, lenYB);

            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 255), x - lenXB / 2, y + lenY / 2 - 3 - 30, lenXB, 3);

            lenT = TextWidth("Confirm", "RussoOne", 20);
            SplashKit.DrawText("Confirm", Color.White, "RussoOne", 20, x - lenT / 2, y + lenY / 2 - lenYB - 30 + 10);
        }

        public void Draw()
        {
            DrawMenu();
            DrawAvatar();
            DrawTank();
            DrawStart();
            DrawNews();
            DrawTutorial();
            DrawSetting();
            DrawMode();
        }

        public void Handle()
        {
            HandleMenu();
            HandleMode();
            HandleStart();
            HandleTutorial();
            HandleSetting();
        }

        public void Update()
        {
            if (_findingMatch) _clock.Update();
        }

        private void HandleMenu()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                int menu_width = (int)TextWidth("Home", "RussoOne", 20);
                if (Hover(SplashKit.MousePosition(), 145, 18, menu_width + 50, 45))
                {
                    GameManager.ScreenType = "Home";
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                }

                int locate_menu = 170 + (int)TextWidth("Home", "RussoOne", 20) + 50;
                menu_width = (int)TextWidth("Shop", "RussoOne", 20);
                if (Hover(SplashKit.MousePosition(), locate_menu - 25, 18, menu_width + 50, 45))
                {
                    GameManager.ScreenType = "Shop";
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                }

                locate_menu += (int)TextWidth("Shop", "RussoOne", 20) + 50;
                menu_width = (int)TextWidth("Bag", "RussoOne", 20);
                if (Hover(SplashKit.MousePosition(), locate_menu - 25, 18, menu_width + 50, 45))
                {
                    GameManager.ScreenType = "Bag";
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                }

                locate_menu += (int)TextWidth("Bag", "RussoOne", 20) + 50;
                menu_width = (int)TextWidth("Tutorial", "RussoOne", 20);
                if (Hover(SplashKit.MousePosition(), locate_menu - 25, 18, menu_width + 50, 45))
                {
                    GameManager.ScreenType = "Tutorial";
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                }

                locate_menu += (int)TextWidth("Tutorial", "RussoOne", 20) + 50;
                menu_width = (int)TextWidth("Setting", "RussoOne", 20);
                if (Hover(SplashKit.MousePosition(), locate_menu - 25, 18, menu_width + 50, 45))
                {
                    GameManager.ScreenType = "Setting";
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                }

                if (Hover(SplashKit.MousePosition(), 843, 15, 46, 45))
                {
                    _dbManager.UpdateUserField2(_user.Username, "Status", "Offline");
                    _user.Bag.Tanks.Clear();
                    _user.Bag.Guns.Clear();
                    _user.Bag.Frames.Clear();
                    _user.Bag.Avatars.Clear();
                    _user.Bag.Items.Clear();
                    GameManager._shopLoadResources = false;
                    GameManager._bagLoadResources = false;
                    GameManager.ScreenType = "SignIn";
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                }
            }
        }

        private void HandleMode()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (Hover(SplashKit.MousePosition(), 725, 470, 160, 35))
                {
                    _openMode = !_openMode;
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                }

                if (!_openMode) return;

                float x0 = 450, y0 = 300, y = 320;
                for (int i = 0; i < 4; i++)
                {
                    float lenX = 210, lenY = 50, spaceY = 65;
                    if (Hover(SplashKit.MousePosition(), x0 - lenX / 2, y0 - y / 2 + 50 + spaceY * i, lenX, lenY))
                    {
                        if (!GameManager.MuteSound)
                            SplashKit.PlaySoundEffect("Click");
                        if (i == 3)
                        {
                            Match.Mode = "Online";
                            _openMode = !_openMode;
                        }
                        else
                        {
                            Match.Level = i + 1;
                            Match.Mode = "Offline";
                            _openMode = !_openMode;
                        }
                        return;
                    }
                }
            }
        }

        private void HandleSetting()
        {
            if (GameManager.ScreenType != "Setting") return;

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                float x = 450, y = 300, lenX = 400, lenY = 270;

                if (Hover(SplashKit.MousePosition(), x - lenX / 2 + 80, y - lenY / 2 + 70, 30, 30))
                {
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                    GameManager.MuteMusic = !GameManager.MuteMusic;
                    if (GameManager.MuteMusic)
                        SplashKit.StopMusic();
                    else
                        SplashKit.PlayMusic("Background", -1);
                }

                if (Hover(SplashKit.MousePosition(), x - lenX / 2 + 80, y - lenY / 2 + 130, 30, 30))
                {
                    GameManager.MuteSound = !GameManager.MuteSound;
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                }

                float lenXB = 120, lenYB = 45;
                if (Hover(SplashKit.MousePosition(), x - lenXB / 2, y + lenY / 2 - lenYB - 30, lenXB, lenYB))
                {
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                    GameManager.ScreenType = "Home";
                }
            }
        }

        private void HandleStart()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (!_findingMatch)
                {
                    if (Hover(SplashKit.MousePosition(), 725, 515, 160, 70))
                    {
                        if (!GameManager.MuteSound)
                            SplashKit.PlaySoundEffect("Click");
                        if (Match.Mode == "Offline")
                            GameManager.ScreenType = "Match";
                        else
                        {
                            _findingMatch = true;
                            Thread m = new Thread(() => FindMatch());
                            m.Start();
                        }
                    }
                }
                else
                {
                    if (Hover(SplashKit.MousePosition(), 725 + 160 - 20, 515, 20, 20)){
                        if (!GameManager.MuteSound)
                            SplashKit.PlaySoundEffect("Click");
                        SendMessage(_client, "CancelFindMatch");
                        _clock.Reset();
                        ConnectServer = false;
                        _findingMatch = false;
                    }
                }
            }
        }

        private void FindMatch()
        {
            while (_findingMatch)
            {
                try
                {
                    if (!ConnectServer)
                    {
                        _client = new TcpClient("192.168.1.100", 5000);
                        ConnectServer = true;
                    }

                    if (ConnectServer)
                    {
                        if (!_findingMatch) return;
                        SendMessage(_client, $"{_user.Username}|");
                        int side = 0;
                        string msg = GetMessage(_client), EnemyUsm = "", EnemyTank = "", EnemyGun = "";
                        string[] lines = msg.Split('\n');

                        foreach (string line in lines)
                            if (line.StartsWith("SideTeam:"))
                                side = int.Parse(line.Substring(9));
                            else if (line.StartsWith("EnemyUsername:"))
                                EnemyUsm = line.Substring(14);

                        EnemyTank = _dbManager.GetUserItemUsed(EnemyUsm, "Tank")["Image"].ToString();
                        EnemyGun = _dbManager.GetUserItemUsed(EnemyUsm, "Gun")["Image"].ToString();

                        if (side != 0)
                        {
                            _findingMatch = false;
                            Match.Client = _client;
                            Match.EnemyUsm = EnemyUsm;
                            Match.EnemyTank = EnemyTank;
                            Match.EnemyGun = EnemyGun;
                            Match.Side = side;
                            GameManager.ScreenType = "Match";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can't connect to Server!");
                }
            }
        }

        private void HandleTutorial()
        {
            if (GameManager.ScreenType != "Tutorial") return;

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                float x = 450, y = 300, lenX = 500, lenY = 380;
                if (Hover(SplashKit.MousePosition(), x + lenX / 2 - 30, y - lenY / 2 + 10, 20, 20))
                {
                    if (!GameManager.MuteMusic)
                        SplashKit.PlaySoundEffect("Click");

                    GameManager.ScreenType = "Home";
                }
            }
        }

        private void SendMessage(TcpClient client, string msg)
        {
            try
            {
                NetworkStream _stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
                _stream.Write(data, 0, data.Length);
            }
            catch { }
        }

        private string GetMessage(TcpClient client)
        {
            try
            {
                NetworkStream _stream = client.GetStream();
                byte[] buffer = new byte[4096];
                int len = _stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, len);
            }
            catch
            {
                return "";
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

        private float TextWidth(string letters, string font, int size_font)
        {
            return SplashKit.TextWidth(letters, font, size_font);
        }
    }
}
