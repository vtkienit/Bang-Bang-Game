using SplashKitSDK;
using static SplashKitSDK.SplashKit;

namespace BangBang
{
    public class Bag:Page
    {
        private DatabaseManager _dbManager;
        private User _user;
        private int _tankSelected, _frameSelected, _avatarSelected, _itemSelected;
        private int _tankUsed, _frameUsed, _avatarUsed, _itemUsed;

        public static string ScreenType = "Tanks";

        public Bag(DatabaseManager dbManager, User user)
        {
            _dbManager = dbManager;
            _user = user;

            // Buttons
            SplashKit.LoadBitmap("Return1_Button", "Images/Return1_Button.png");
            SplashKit.LoadBitmap("Return1_Button_Hover", "Images/Return1_Button_Hover.png");
            SplashKit.LoadBitmap("BackgroundFrame", "Images/BackgroundFrame.png");
            SplashKit.LoadBitmap("Check_Button", "Images/Check_Button.png");
        }

        public void LoadResources()
        {
            _tankSelected = _tankUsed = OrderItem(_user.Bag.Tanks, _user.TankUsed);
            _frameSelected = _frameUsed = OrderItem(_user.Bag.Frames, _user.FrameUsed);
            _avatarSelected = _avatarUsed = OrderItem(_user.Bag.Avatars, _user.AvatarUsed);
            _itemSelected = _itemUsed = OrderItem(_user.Bag.Items, _user.ItemUsed);
        }

        private int OrderItem(List<Item> items, string ID)
        {
            for (int i = 0; i < items.Count; i++)
                if (items[i].ID == ID) return i;
            return -1;
        }

        private void DrawMenu()
        {
            SplashKit.DrawBitmap("BackgroundShop", 0, 0);
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), 0, 10, 60, 40);

            if (Hover(MousePosition(), 15, 15, 30, 30))
                SplashKit.DrawBitmap("Return1_Button_Hover", 15, 15);
            else
                SplashKit.DrawBitmap("Return1_Button", 15, 15);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), 30, 80, 500, 500);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 50), 30, 80, 500, 45);

            int menu_width = (int)TextWidth("Tanks", "RussoOne", 19);
            if (Bag.ScreenType == "Tanks")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), 50, 80, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, 50, 125 - 3, menu_width + 50, 3);
                SplashKit.DrawText("Tanks", Color.Black, "RussoOne", 19, 75, 90);
            }
            else if (Hover(SplashKit.MousePosition(), 50, 80, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, 50, 125 - 3, menu_width + 50, 3);
                SplashKit.DrawText("Tanks", Color.Yellow, "RussoOne", 19, 75, 90);
            }
            else
                SplashKit.DrawText("Tanks", Color.White, "RussoOne", 19, 75, 90);

            float locate_menu = 75 + (int)TextWidth("Tanks", "RussoOne", 19) + 50;
            menu_width = (int)TextWidth("Frames", "RussoOne", 19);
            if (Bag.ScreenType == "Frames")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), locate_menu - 25, 80, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, locate_menu - 25, 125 - 3, menu_width + 50, 3);
                SplashKit.DrawText("Frames", Color.Black, "RussoOne", 19, locate_menu, 90);
            }
            else if (Hover(SplashKit.MousePosition(), locate_menu - 25, 80, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, locate_menu - 25, 125 - 3, menu_width + 50, 3);
                SplashKit.DrawText("Frames", Color.Yellow, "RussoOne", 19, locate_menu, 90);
            }
            else
                SplashKit.DrawText("Frames", Color.White, "RussoOne", 19, locate_menu, 90);

            locate_menu += (int)TextWidth("Frames", "RussoOne", 19) + 50;
            menu_width = (int)TextWidth("Avatars", "RussoOne", 19);
            if (Bag.ScreenType == "Avatars")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), locate_menu - 25, 80, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, locate_menu - 25, 125 - 3, menu_width + 50, 3);
                SplashKit.DrawText("Avatars", Color.Black, "RussoOne", 19, locate_menu, 90);
            }
            else if (Hover(SplashKit.MousePosition(), locate_menu - 25, 80, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, locate_menu - 25, 125 - 3, menu_width + 50, 3);
                SplashKit.DrawText("Avatars", Color.Yellow, "RussoOne", 19, locate_menu, 90);
            }
            else
                SplashKit.DrawText("Avatars", Color.White, "RussoOne", 19, locate_menu, 90);

            locate_menu += (int)TextWidth("Avatars", "RussoOne", 19) + 50;
            menu_width = (int)TextWidth("Items", "RussoOne", 19);
            if (Bag.ScreenType == "Items")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), locate_menu - 25, 80, menu_width + 50, 45);
                SplashKit.FillRectangle(Color.White, locate_menu - 25, 125 - 3, menu_width + 50, 3);
                SplashKit.DrawText("Items", Color.Black, "RussoOne", 19, locate_menu, 90);
            }
            else if (Hover(SplashKit.MousePosition(), locate_menu - 25, 80, menu_width + 50, 45))
            {
                SplashKit.FillRectangle(Color.Yellow, locate_menu - 25, 125 - 3, menu_width + 50, 3);
                SplashKit.DrawText("Items", Color.Yellow, "RussoOne", 19, locate_menu, 90);
            }
            else
                SplashKit.DrawText("Items", Color.White, "RussoOne", 19, locate_menu, 90);

        }

        private void DrawHeader()
        {
            SplashKit.DrawText("Bag", Color.White, "LuckiestGuy", 55, 450 - TextWidth("Bag", "LuckiestGuy", 55) / 2, 18);
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 120), 833, 19, 15 + TextWidth(_user.Coin.ToString(), "RussoOne", 18) + 8, 22);
            SplashKit.DrawBitmap("Coin", 815, 15);
            SplashKit.DrawText(_user.Coin.ToString(), Color.White, "RussoOne", 18, 850, 19);
        }

        private void DrawUseButton()
        {
            int x0 = 530;
            if (Hover(SplashKit.MousePosition(), x0 + 370 / 2 - 110 / 2, 450, 110, 45))
            {
                SplashKit.FillRectangle(Color.RGBAColor(230, 177, 0, 255), x0 + 370 / 2 - 110 / 2, 450, 110, 45);
                SplashKit.FillRectangle(Color.White, x0 + 370 / 2 - 110 / 2, 450 + 45 - 4, 110, 4);
                SplashKit.DrawText("Use", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 18, x0 + 370 / 2 - TextWidth("Use", "RussoOne", 18) / 2 + 1, 460 + 1);
                SplashKit.DrawText("Use", Color.White, "RussoOne", 18, x0 + 370 / 2 - TextWidth("Use", "RussoOne", 18) / 2, 460);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(255, 204, 50, 255), x0 + 370 / 2 - 110 / 2, 450, 110, 45);
                SplashKit.FillRectangle(Color.White, x0 + 370 / 2 - 110 / 2, 450 + 45 - 4, 110, 4);
                SplashKit.DrawText("Use", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 18, x0 + 370 / 2 - TextWidth("Use", "RussoOne", 18) / 2 + 1, 460 + 1);
                SplashKit.DrawText("Use", Color.White, "RussoOne", 18, x0 + 370 / 2 - TextWidth("Use", "RussoOne", 18) / 2, 460);
            }
        }

        private void DrawItems(List<Item> items, int selectedID, int itemUsed, string typeItem)
        {
            int x, y = 150;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (i * 3 + j + 1 > items.Count) return;
                    x = 50 + j * 170;
                    
                    if (typeItem == "Tank")
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(0, 153, 153, 255), x, y, 113, 113);
                        SplashKit.DrawBitmap(items[i * 3 + j].ImageShop, x + 115 / 2 - items[i * 3 + j].ImageShop.Width / 2, y + 113 / 2 - items[i * 3 + j].ImageShop.Height / 2 + 8);
                        SplashKit.DrawBitmap(_user.Bag.Guns[i * 3 + j].ImageShop, x + 115 / 2 - _user.Bag.Guns[i * 3 + j].ImageShop.Width / 2, y + 113 / 2 - _user.Bag.Guns[i * 3 + j].ImageShop.Height / 2 - 17);
                    }
                    else if (typeItem == "Frame")
                    {
                        SplashKit.DrawBitmap("BackgroundFrame", x, y);
                        SplashKit.DrawBitmap(items[i * 3 + j].ImageShop, x + 115 / 2 - items[i * 3 + j].ImageShop.Width / 2, y + 113 / 2 - items[i * 3 + j].ImageShop.Height / 2);
                    }
                    else if (typeItem == "Avatar")
                    {
                        SplashKit.DrawBitmap("BackgroundAvatar", x, y);
                        SplashKit.DrawBitmap(items[i * 3 + j].ImageShop, x + 115 / 2 - items[i * 3 + j].ImageShop.Width / 2, y + 113 / 2 - items[i * 3 + j].ImageShop.Height / 2);
                        SplashKit.FillRectangle(Color.RGBAColor(140, 0, 0, 200), x + 110 / 2 - 70 / 2, y + 110 / 2 - 70 / 2 - 2, 75, 3);
                        SplashKit.FillRectangle(Color.RGBAColor(140, 0, 0, 200), x + 3 + 110 / 2 - 70 / 2 + 70 - 1, y + 110 / 2 - 70 / 2, 3, 73);
                        SplashKit.FillRectangle(Color.RGBAColor(140, 0, 0, 200), x + 110 / 2 - 70 / 2, y + 110 / 2 - 70 / 2 + 70, 73, 3);
                        SplashKit.FillRectangle(Color.RGBAColor(140, 0, 0, 200), x + 110 / 2 - 70 / 2, y + 110 / 2 - 70 / 2, 3, 70);
                    }
                    else
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(255, 153, 153, 255), x, y, 113, 113);
                        SplashKit.DrawBitmap(items[i * 3 + j].ImageShop, x + 115 / 2 - items[i * 3 + j].ImageShop.Width / 2, y + 113 / 2 - items[i * 3 + j].ImageShop.Height / 2);
                    }


                    if (itemUsed == i * 3 + j)
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), x, y, 113, 3);
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), x + 113, y, 3, 113);
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), x, y + 113, 116, 3);
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), x, y, 3, 113);
                        SplashKit.FillCircle(Color.RGBAColor(0, 0, 0, 230), x + 115 / 2, y + 115, 12);
                        SplashKit.DrawBitmap("Check_Button", x + 115 / 2 - 15, y + 101);
                    }
                    else if (selectedID == i * 3 + j)
                    {
                        SplashKit.FillRectangle(Color.Lime, x, y, 113, 3);
                        SplashKit.FillRectangle(Color.Lime, x + 113, y, 3, 113);
                        SplashKit.FillRectangle(Color.Lime, x, y + 113, 116, 3);
                        SplashKit.FillRectangle(Color.Lime, x, y, 3, 113);
                    }
                    else if (Hover(SplashKit.MousePosition(), x, y, 113, 113))
                    {
                        SplashKit.FillRectangle(Color.Yellow, x, y, 113, 3);
                        SplashKit.FillRectangle(Color.Yellow, x + 113, y, 3, 113);
                        SplashKit.FillRectangle(Color.Yellow, x, y + 113, 116, 3);
                        SplashKit.FillRectangle(Color.Yellow, x, y, 3, 113);
                    }
                    else
                    {
                        SplashKit.FillRectangle(Color.White, x, y, 113, 3);
                        SplashKit.FillRectangle(Color.White, x + 113, y, 3, 113);
                        SplashKit.FillRectangle(Color.White, x, y + 113, 116, 3);
                        SplashKit.FillRectangle(Color.White, x, y, 3, 113);
                    }
                }
                y += 150;
            }
        }

        private void DrawSubItems(List<Item> items, int selectedID, int itemUsed, string typeItem)
        {
            if (typeItem == "Tank")
            {
                SplashKit.DrawBitmap(items[selectedID].ImageBag, 530 + 370 / 2 - items[selectedID].ImageBag.Width / 2, 150);
                SplashKit.DrawBitmap(_user.Bag.Guns[selectedID].ImageBag, 530 + 370 / 2 - _user.Bag.Guns[selectedID].ImageBag.Width / 2, 125);
            }
            else
            {
                SplashKit.DrawBitmap(items[selectedID].ImageBag, 530 + 370 / 2 - items[selectedID].ImageBag.Width / 2, 150);
            }

            SplashKit.DrawText(items[selectedID].Name, Color.White, "RussoOne", 25, 530 + 370 / 2 - TextWidth(items[selectedID].Name, "RussoOne", 25) / 2, 380);

            if (itemUsed != selectedID) DrawUseButton();
        }

        public void Draw()
        {
            DrawMenu();
            DrawHeader();
            if (Bag.ScreenType == "Tanks")
            {
                DrawItems(_user.Bag.Tanks, _tankSelected, _tankUsed, "Tank");
                DrawSubItems(_user.Bag.Tanks, _tankSelected, _tankUsed, "Tank");
            }
            else if (Bag.ScreenType == "Frames")
            {
                DrawItems(_user.Bag.Frames, _frameSelected, _frameUsed, "Frame");
                DrawSubItems(_user.Bag.Frames, _frameSelected, _frameUsed, "Frame");
            }
            else if (Bag.ScreenType == "Avatars")
            {
                DrawItems(_user.Bag.Avatars, _avatarSelected, _avatarUsed, "Avatar");
                DrawSubItems(_user.Bag.Avatars, _avatarSelected, _avatarUsed, "Avatar");
            }
            else if (Bag.ScreenType == "Items")
            {
                DrawItems(_user.Bag.Items, _itemSelected, _itemUsed, "Item");
                DrawSubItems(_user.Bag.Items, _itemSelected, _itemUsed, "Item");
            }
        }

        private void HandleMenu()
        {
            if (Hover(SplashKit.MousePosition(), 15, 15, 30, 30))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                GameManager.ScreenType = "Home";
                Bag.ScreenType = "Tanks";

                _tankSelected = _tankUsed;
                _frameSelected = _frameUsed;
                _avatarSelected = _avatarUsed;
                _itemSelected = _itemUsed;
            }

            int menu_width = (int)TextWidth("Tanks", "RussoOne", 19);
            if (Hover(SplashKit.MousePosition(), 52.5f, 80, menu_width + 45, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                Bag.ScreenType = "Tanks";

                _tankSelected = _tankUsed;
                _frameSelected = _frameUsed;
                _avatarSelected = _avatarUsed;
                _itemSelected = _itemUsed;
            }

            float locate_menu = 75 + TextWidth("Tanks", "RussoOne", 19) + 45;
            menu_width = (int)TextWidth("Frames", "RussoOne", 19);

            if (Hover(SplashKit.MousePosition(), locate_menu - 22.5f, 80, menu_width + 45, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                Bag.ScreenType = "Frames";

                _tankSelected = _tankUsed;
                _frameSelected = _frameUsed;
                _avatarSelected = _avatarUsed;
                _itemSelected = _itemUsed;
            }

            locate_menu += TextWidth("Frames", "RussoOne", 19) + 45;
            menu_width = (int)TextWidth("Avatars", "RussoOne", 19);
            if (Hover(SplashKit.MousePosition(), locate_menu - 22.5f, 80, menu_width + 45, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                Bag.ScreenType = "Avatars";

                _tankSelected = _tankUsed;
                _frameSelected = _frameUsed;
                _avatarSelected = _avatarUsed;
                _itemSelected = _itemUsed;
            }

            locate_menu += TextWidth("Avatars", "RussoOne", 19) + 45;
            menu_width = (int)TextWidth("Items", "RussoOne", 19);
            if (Hover(SplashKit.MousePosition(), locate_menu - 22.5f, 80, menu_width + 45, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                Bag.ScreenType = "Items";

                _tankSelected = _tankUsed;
                _frameSelected = _frameUsed;
                _avatarSelected = _avatarUsed;
                _itemSelected = _itemUsed;
            }

        }

        private void HandleItems(List<Item> items, ref int selectedID, ref int itemUsed, string typeItem)
        {
            int x0 = 530;
            if ((itemUsed != selectedID) && Hover(SplashKit.MousePosition(), x0 + 370 / 2 - 110 / 2, 450, 110, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                HandleUseButton(typeItem + " Used", items[selectedID].ID, ref itemUsed, selectedID);
                if (typeItem == "Tank")
                {
                    _user.TankUsed = items[selectedID].ID;
                    HandleUseButton("Gun Used", _user.Bag.Guns[selectedID].ID, ref itemUsed, selectedID);
                    _user.GunUsed = _user.Bag.Guns[selectedID].ID;
                }
                else if (typeItem == "Frame")
                    _user.FrameUsed = items[selectedID].ID;
                else if (typeItem == "Avatar")
                    _user.AvatarUsed = items[selectedID].ID;
                else if (typeItem == "Item")
                    _user.ItemUsed = items[selectedID].ID;
            }

            int x, y = 150;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (i * 3 + j + 1 > items.Count) return;
                    x = 50 + j * 170;
                    if (Hover(SplashKit.MousePosition(), x, y, 113, 113))
                    {
                        if (!GameManager.MuteSound)
                            SplashKit.PlaySoundEffect("Click");
                        selectedID = i * 3 + j;
                        return;
                    }
                }
                y += 150;
            }
        }

        public void Handle()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                HandleMenu();
                if (Bag.ScreenType == "Tanks")
                {
                    HandleItems(_user.Bag.Tanks, ref _tankSelected, ref _tankUsed, "Tank");
                }
                else if (Bag.ScreenType == "Frames")
                {
                    HandleItems(_user.Bag.Frames, ref _frameSelected, ref _frameUsed, "Frame");
                }
                else if (Bag.ScreenType == "Avatars")
                {
                    HandleItems(_user.Bag.Avatars, ref _avatarSelected, ref _avatarUsed, "Avatar");
                }
                else if (Bag.ScreenType == "Items")
                {
                    HandleItems(_user.Bag.Items, ref _itemSelected, ref _itemUsed, "Item");
                }
            }
        }

        private void HandleUseButton(string field, string ID, ref int objectUsed, int order)
        {
            objectUsed = order;
            _dbManager.UpdateUserField2(_user.Username, field, ID);
        }

        public void DrawImage(string name, Bitmap img, float x, float y, float width, float height)
        {
            var opts = OptionScaleBmp(width / img.Width, height / img.Height);
            SplashKit.DrawBitmap(name, x, y, opts);
        }

        private bool Hover(Point2D mouse, float x, float y, int w, int h)
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
