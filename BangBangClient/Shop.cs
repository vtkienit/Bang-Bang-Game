using MongoDB.Bson;
using SplashKitSDK;
using static SplashKitSDK.SplashKit;

namespace BangBang
{
    public class Shop:Page
    {
        private DatabaseManager _dbManager;
        private User _user;

        private List<Item> tanks, guns, frames, avatars, items;

        private bool[] _checkOwnedTanks, _checkOwnedFrames, _checkOwnedAvatars, _checkOwnedItems;

        private int _selectedTanks = -1, _selectedFrames = -1, _selectedAvatars = -1, _selectedItems = -1, _buyStatus = -1;

        public static string ScreenType = "Tanks";

        public Shop(DatabaseManager dbManager, User user)
        {
            _dbManager = dbManager;
            _user = user;

            tanks = new List<Item>();
            guns = new List<Item>();
            frames = new List<Item>();
            avatars = new List<Item>();
            items = new List<Item>();

            _checkOwnedTanks = new bool[10];
            _checkOwnedFrames = new bool[10];
            _checkOwnedAvatars = new bool[10];
            _checkOwnedItems = new bool[10];

            // Backgrounds
            SplashKit.LoadBitmap("BackgroundTank", "Images/BackgroundTank.png");
            SplashKit.LoadBitmap("BackgroundFrame", "Images/BackgroundFrame.png");
            SplashKit.LoadBitmap("BackgroundAvatar", "Images/BackgroundAvatar.png");
            SplashKit.LoadBitmap("BackgroundAvatar1", "Images/BackgroundAvatar1.png");

            // Items
            SplashKit.LoadBitmap("Coin", "Images/Coin.png");

            // Buttons
            SplashKit.LoadBitmap("Return_Button", "Images/Return_Button.png");
            SplashKit.LoadBitmap("Return_Button_Hover", "Images/Return_Button_Hover.png");
        }

        private void LoadItems(List<BsonDocument>? VirtualItems, List<Item> items, int typeItem)
        {
            if (VirtualItems == null || VirtualItems.Count == 0) return;

            foreach (var VirtualItem in VirtualItems)
                items.Add(new Item(VirtualItem["ID"].ToString(), VirtualItem["Image"].ToString(), VirtualItem["Name"].ToString(), "", VirtualItem["Desc"].ToString(), (int)VirtualItem["Price"], typeItem));
        }

        public void LoadResources()
        {
            tanks = new List<Item>();
            guns = new List<Item>();
            frames = new List<Item>();
            avatars = new List<Item>();
            items = new List<Item>();

            _checkOwnedTanks = new bool[10];
            _checkOwnedFrames = new bool[10];
            _checkOwnedAvatars = new bool[10];
            _checkOwnedItems = new bool[10];

            LoadItems(_dbManager.ListAllItems("Tank"), tanks, 1);
            DivideResources(ref _checkOwnedTanks, tanks, _user.Bag.Tanks);

            LoadItems(_dbManager.ListAllItems("Gun"), guns, 3);

            LoadItems(_dbManager.ListAllItems("Frame"), frames, 2);
            DivideResources(ref _checkOwnedFrames, frames, _user.Bag.Frames);

            LoadItems(_dbManager.ListAllItems("Avatar"), avatars, 2);
            DivideResources(ref _checkOwnedAvatars, avatars, _user.Bag.Avatars);

            LoadItems(_dbManager.ListAllItems("Item"), items, 2);
            DivideResources(ref _checkOwnedItems, items, _user.Bag.Items);
        }

        public void DivideResources(ref bool[] CheckObjects, List<Item> AllItems, List<Item> OwnedItems)
        {
            for(int i = 0; i < AllItems.Count; i++)
                for(int j = 0; j < OwnedItems.Count; j++)
                    if (AllItems[i].ID == OwnedItems[j].ID)
                    {
                        CheckObjects[i] = true;
                        break;
                    }
        }

        private bool CheckOnProcessing()
        {
            if (_selectedTanks >= 0 || _selectedFrames >= 0 || _selectedAvatars >= 0 || _selectedItems >= 0 || _buyStatus >= 0)
                return true;
            return false;
        }

        private void DrawMenu()
        {
            SplashKit.DrawBitmap("BackgroundShop", 0, 0);
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), 18, 0, 69, 600);

            if (!CheckOnProcessing() && Hover(SplashKit.MousePosition(), 30, 20, 45, 45))
                SplashKit.DrawBitmap("Return_Button_Hover", 30, 20);
            else
                SplashKit.DrawBitmap("Return_Button", 30, 20);


            if (Shop.ScreenType == "Tanks")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), 18, 120, 69, 45);
                SplashKit.FillRectangle(Color.White, 18, 120, 4, 45);
                SplashKit.DrawText("Tanks", Color.Black, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Tanks", "RussoOne", 16) / 2, 130);
            }
            else if (!CheckOnProcessing() && Hover(SplashKit.MousePosition(), 18, 120, 69, 45))
                SplashKit.DrawText("Tanks", Color.Yellow, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Tanks", "RussoOne", 16) / 2, 130);
            else
                SplashKit.DrawText("Tanks", Color.White, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Tanks", "RussoOne", 16) / 2, 130);

            SplashKit.FillRectangle(Color.Gray, 30, 180, 45, 2);


            if (Shop.ScreenType == "Frames")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), 18, 200, 69, 45);
                SplashKit.FillRectangle(Color.White, 18, 200, 4, 45);
                SplashKit.DrawText("Frames", Color.Black, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Frames", "RussoOne", 16) / 2 + 2, 210);
            }
            else if (!CheckOnProcessing() && Hover(SplashKit.MousePosition(), 18, 200, 69, 45))
                SplashKit.DrawText("Frames", Color.Yellow, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Frames", "RussoOne", 16) / 2, 210);
            else
                SplashKit.DrawText("Frames", Color.White, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Frames", "RussoOne", 16) / 2, 210);

            SplashKit.FillRectangle(Color.Gray, 30, 260, 45, 2);


            if (Shop.ScreenType == "Avatars")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), 18, 280, 69, 45);
                SplashKit.FillRectangle(Color.White, 18, 280, 4, 45);
                SplashKit.DrawText("Avatars", Color.Black, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Avatars", "RussoOne", 16) / 2 + 2, 290);
            }
            else if (!CheckOnProcessing() && Hover(SplashKit.MousePosition(), 18, 280, 69, 45))
                SplashKit.DrawText("Avatars", Color.Yellow, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Avatars", "RussoOne", 16) / 2, 290);
            else
                SplashKit.DrawText("Avatars", Color.White, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Avatars", "RussoOne", 16) / 2, 290);

            SplashKit.FillRectangle(Color.Gray, 30, 340, 45, 2);


            if (Shop.ScreenType == "Items")
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 255), 18, 360, 69, 45);
                SplashKit.FillRectangle(Color.White, 18, 360, 4, 45);
                SplashKit.DrawText("Items", Color.Black, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Items", "RussoOne", 16) / 2, 370);
            }
            else if (!CheckOnProcessing() && Hover(SplashKit.MousePosition(), 18, 360, 69, 45))
                SplashKit.DrawText("Items", Color.Yellow, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Items", "RussoOne", 16) / 2, 370);
            else
                SplashKit.DrawText("Items", Color.White, "RussoOne", 16, 20 + 65 / 2 - TextWidth("Items", "RussoOne", 16) / 2, 370);

            SplashKit.FillRectangle(Color.Gray, 30, 420, 45, 2);
        }

        private void DrawHeader()
        {
            SplashKit.DrawText("Shop", Color.White, "LuckiestGuy", 55, 450 - TextWidth("Shop", "LuckiestGuy", 55) / 2, 18);
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 120), 833, 19, 15 + TextWidth(_user.Coin.ToString(), "Arial", 19) + 8, 22);
            SplashKit.DrawBitmap("Coin", 815, 15);
            SplashKit.DrawText(_user.Coin.ToString(), Color.White, "RussoOne", 18, 850, 19);
        }

        private void DrawItems(List<Item> items1, List<Item> items2, bool[] OwnedItems, int selectedID)
        {
            int x = 150, y = 120;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (i * 3 + j + 1 > items1.Count) break;
                    x = 150 + j * 250;
                    SplashKit.DrawBitmap("BackgroundTank", x, y);

                    if (selectedID == i * 3 + j)
                    {
                        SplashKit.FillRectangle(Color.Cyan, x, y, 150, 3);
                        SplashKit.FillRectangle(Color.Cyan, x + 150, y, 3, 180);
                        SplashKit.FillRectangle(Color.Cyan, x, y + 180, 153, 3);
                        SplashKit.FillRectangle(Color.Cyan, x, y, 3, 180);
                    }
                    else if ((selectedID < 0 && _buyStatus < 0) && Hover(SplashKit.MousePosition(), x, y, 150, 180))
                    {
                        SplashKit.FillRectangle(Color.Yellow, x, y, 150, 3);
                        SplashKit.FillRectangle(Color.Yellow, x + 150, y, 3, 180);
                        SplashKit.FillRectangle(Color.Yellow, x, y + 180, 153, 3);
                        SplashKit.FillRectangle(Color.Yellow, x, y, 3, 180);
                    }
                    else
                    {
                        SplashKit.FillRectangle(Color.White, x, y, 150, 3);
                        SplashKit.FillRectangle(Color.White, x + 150, y, 3, 180);
                        SplashKit.FillRectangle(Color.White, x, y + 180, 153, 3);
                        SplashKit.FillRectangle(Color.White, x, y, 3, 180);
                    }

                    SplashKit.DrawBitmap(items1[i * 3 + j].ImageShop, x + 150 / 2 - items1[i * 3 + j].ImageShop.Width / 2, y + 150 / 2 - items1[i * 3 + j].ImageShop.Height / 2);
                    SplashKit.DrawBitmap(items2[i * 3 + j].ImageShop, x + 150 / 2 - items2[i * 3 + j].ImageShop.Width / 2, y + 150 / 2 - items2[i * 3 + j].ImageShop.Height / 2 - 22);
                    
                    if (!OwnedItems[i * 3 + j])
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(224, 255, 255, 140), x + 3, y + 150, 147, 30);
                        SplashKit.DrawBitmap("Coin", x + 150 / 2 - 35, y + 150);
                        SplashKit.DrawText(items1[i * 3 + j].Price.ToString(), Color.White, "RussoOne", 20, x + 150 / 2 - 5, y + 153);
                    }
                    else
                    {
                        SplashKit.FillRectangle(Color.Gray, x + 3, y + 150, 147, 30);
                        SplashKit.DrawText("Owned", Color.White, "RussoOne", 18, x + 150 / 2 - TextWidth("Owned", "RussoOne", 18) / 2, y + 153);
                    }
                }
                y += 240;
            }
        }

        private void DrawItems(List<Item> items, bool[] OwnedItems, int selectedID, string typeItem)
        {
            int x = 150, y = 120;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (i * 4 + j + 1 > items.Count) return;
                    x = 150 + j * 200;

                    if (typeItem == "Frame")
                        SplashKit.DrawBitmap("BackgroundFrame", x, y);
                    else if (typeItem == "Avatar")
                    {
                        SplashKit.DrawBitmap("BackgroundAvatar", x, y);
                        SplashKit.FillRectangle(Color.RGBAColor(224, 255, 255, 140), x, y + 113, 113, 30);
                    }
                    else
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(255, 153, 153, 255), x, y, 113, 113);
                        SplashKit.FillRectangle(Color.RGBAColor(224, 255, 255, 140), x, y + 113, 113, 30);
                    }

                    if (selectedID == i * 4 + j)
                    {
                        SplashKit.FillRectangle(Color.Cyan, x, y, 113, 3);
                        SplashKit.FillRectangle(Color.Cyan, x + 113, y, 3, 143);
                        SplashKit.FillRectangle(Color.Cyan, x, y + 143, 116, 3);
                        SplashKit.FillRectangle(Color.Cyan, x, y, 3, 143);
                    }
                    else if ((selectedID < 0 && _buyStatus < 0) && Hover(SplashKit.MousePosition(), x, y, 113, 143))
                    {
                        SplashKit.FillRectangle(Color.Yellow, x, y, 113, 3);
                        SplashKit.FillRectangle(Color.Yellow, x + 113, y, 3, 143);
                        SplashKit.FillRectangle(Color.Yellow, x, y + 143, 116, 3);
                        SplashKit.FillRectangle(Color.Yellow, x, y, 3, 143);
                    }
                    else
                    {
                        SplashKit.FillRectangle(Color.White, x, y, 113, 3);
                        SplashKit.FillRectangle(Color.White, x + 113, y, 3, 143);
                        SplashKit.FillRectangle(Color.White, x, y + 143, 116, 3);
                        SplashKit.FillRectangle(Color.White, x, y, 3, 143);
                    }

                    SplashKit.DrawBitmap(items[i * 4 + j].ImageShop, x + 3 + 110 / 2 - items[i * 4 + j].ImageShop.Width / 2, y + 110 / 2 - items[i * 4 + j].ImageShop.Height / 2);
                    if (typeItem == "Avatar")
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(140, 0, 0, 200), x + 110 / 2 - 70 / 2, y + 110 / 2 - 70 / 2 - 3, 76, 3);
                        SplashKit.FillRectangle(Color.RGBAColor(140, 0, 0, 200), x + 3 + 110 / 2 - 70 / 2 + 70, y + 110 / 2 - 70 / 2, 3, 73);
                        SplashKit.FillRectangle(Color.RGBAColor(140, 0, 0, 200), x + 110 / 2 - 70 / 2, y + 110 / 2 - 70 / 2 + 70, 73, 3);
                        SplashKit.FillRectangle(Color.RGBAColor(140, 0, 0, 200), x + 110 / 2 - 70 / 2, y + 110 / 2 - 70 / 2, 3, 70);
                    }

                    if (!OwnedItems[i * 4 + j])
                    {
                        if (typeItem == "Frame")
                            SplashKit.FillRectangle(Color.RGBAColor(224, 255, 255, 140), x + 3, y + 113, 110, 30);

                        SplashKit.DrawBitmap("Coin", x + 113 / 2 - 35, y + 113);
                        SplashKit.DrawText(items[i * 4 + j].Price.ToString(), Color.White, "RussoOne", 20, x + 110 / 2 + 2, y + 115.5);
                    }
                    else
                    {
                        SplashKit.FillRectangle(Color.Gray, x + 3, y + 113, 110, 30);
                        SplashKit.DrawText("Owned", Color.White, "RussoOne", 18, x + 116 / 2 - TextWidth("Owned", "RussoOne", 18) / 2, y + 116);
                    }
                }
                y += 240;
            }
        }

        private void DrawPurchaseProcessing(Item item, Item item2, string typeItem)
        {
            int x0 = 450, y0 = 300;
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), 0, 0, 900, 600);
            SplashKit.FillRectangle(Color.RGBAColor(214, 234, 252, 250), x0 - 400 / 2, y0 - 300 / 2, 400, 300);
            SplashKit.FillRectangle(Color.RGBAColor(0, 56, 168, 250), x0 - 400 / 2, y0 - 300 / 2, 400, 45);
            SplashKit.DrawText("Purchase", Color.White, "RussoOne", 20, x0 - 400 / 2 + 15, y0 - 300 / 2 + 10);

            if (Hover(SplashKit.MousePosition(), x0 - 400 / 2 + 400 - 40, y0 - 300 / 2 + 7, 30, 30))
                SplashKit.DrawBitmap("Close_Button_Hover", x0 - 400 / 2 + 400 - 40, y0 - 300 / 2 + 7);
            else
                SplashKit.DrawBitmap("Close_Button", x0 - 400 / 2 + 400 - 40, y0 - 300 / 2 + 7);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 40), x0 - 400 / 2 + 13, y0 - 300 / 2 + 58, 374, 129);
            SplashKit.FillRectangle(Color.RGBAColor(255, 174, 107, 150), x0 - 400 / 2 + 20, y0 - 300 / 2 + 65, 113, 113);
            SplashKit.FillRectangle(Color.White, x0 - 400 / 2 + 20, y0 - 300 / 2 + 65, 113, 3);
            SplashKit.FillRectangle(Color.White, x0 - 400 / 2 + 20 + 113, y0 - 300 / 2 + 65, 3, 113);
            SplashKit.FillRectangle(Color.White, x0 - 400 / 2 + 20, y0 - 300 / 2 + 65 + 113, 116, 3);
            SplashKit.FillRectangle(Color.White, x0 - 400 / 2 + 20, y0 - 300 / 2 + 65, 3, 113);

            SplashKit.DrawText(item.Name, Color.RGBAColor(255, 221, 0, 255), "RussoOne", 20, x0 - 400 / 2 + 20 + 123, y0 - 300 / 2 + 70);
            if(typeItem == "Item")
                SplashKit.DrawText(item.FullDesc, Color.Gray, "RussoOne", 14, x0 - 400 / 2 + 20 + 123, y0 - 300 / 2 + 70 + 30);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 50), x0 - 400 / 2 + 20 + 123, y0 - 300 / 2 + 142, 40 + TextWidth(item.Price.ToString(), "Arial", 18) + 12, 31);
            SplashKit.DrawBitmap("Coin", x0 - 400 / 2 + 20 + 126, y0 - 300 / 2 + 142.5);
            SplashKit.DrawText(item.Price.ToString(), Color.White, "RussoOne", 18, x0 - 400 / 2 + 20 + 160, y0 - 300 / 2 + 146.5);

            if (Hover(SplashKit.MousePosition(), x0 - 120 / 2, y0 + 100 - 45 / 2, 120, 45))
            {
                SplashKit.FillRectangle(Color.RGBAColor(38, 79, 180, 255), x0 - 120 / 2, y0 + 100 - 45 / 2, 120, 45);
                SplashKit.FillRectangle(Color.White, x0 - 120 / 2, y0 + 100 - 45 / 2 + 45 - 4, 120, 4);
                SplashKit.DrawText("Buy", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 18, x0 - TextWidth("Buy", "RussoOne", 18) / 2 + 1, y0 + 100 - 45 / 2 + 10 + 1);
                SplashKit.DrawText("Buy", Color.White, "RussoOne", 18, x0 - TextWidth("Buy", "RussoOne", 18) / 2, y0 + 100 - 45 / 2 + 10);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(65, 105, 225, 255), x0 - 120 / 2, y0 + 100 - 45 / 2, 120, 45);
                SplashKit.FillRectangle(Color.White, x0 - 120 / 2, y0 + 100 - 45 / 2 + 45 - 4, 120, 4);
                SplashKit.DrawText("Buy", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 18, x0 - TextWidth("Buy", "RussoOne", 18) / 2 + 1, y0 + 100 - 45 / 2 + 10 + 1);
                SplashKit.DrawText("Buy", Color.White, "RussoOne", 18, x0 - TextWidth("Buy", "RussoOne", 18) / 2, y0 + 100 - 45 / 2 + 10);
            }

            int x = 450 - 400 / 2 + 20, y = 300 - 300 / 2 + 65;

            if (typeItem == "Tank")
            {
                SplashKit.DrawBitmap(item.ImageShop, x + 113 / 2 - item.ImageShop.Width / 2, y + 113 / 2 - item.ImageShop.Height / 2 + 8);
                SplashKit.DrawBitmap(item2.ImageShop, x + 113 / 2 - item2.ImageShop.Width / 2, y + 113 / 2 - item2.ImageShop.Height / 2 - 17);
            }
            else
                SplashKit.DrawBitmap(item.ImageShop, x + 113 / 2 - item.ImageShop.Width / 2, y + 113 / 2 - item.ImageShop.Height / 2);
        }

        private void DrawBuyStatus(string message, Color title, Color msg, Color cf_button, Color cf_button_hover)
        {
            int x0 = 450, y0 = 300;
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), 0, 0, 900, 600);
            SplashKit.FillRectangle(Color.RGBAColor(214, 234, 252, 250), x0 - 340 / 2, y0 - 220 / 2, 340, 220);
            SplashKit.FillRectangle(title, x0 - 340 / 2, y0 - 220 / 2, 340, 45);
            SplashKit.DrawText("Status", Color.White, "RussoOne", 23, x0 - TextWidth("Status", "RussoOne", 23) / 2, y0 - 220 / 2 + 8);
            SplashKit.DrawText(message, msg, "RussoOne", 18, x0 - TextWidth(message, "RussoOne", 18) / 2, y0 - 220 / 2 + 80);

            if (Hover(SplashKit.MousePosition(), x0 - 120 / 2, y0 + 100 - 45 / 2 - 25, 120, 45))
            {
                SplashKit.FillRectangle(cf_button_hover, x0 - 120 / 2, y0 + 100 - 45 / 2 - 25, 120, 45);
                SplashKit.FillRectangle(Color.White, x0 - 120 / 2, y0 + 100 - 45 / 2 + 45 - 29, 120, 4);
                SplashKit.DrawText("Confirm", Color.White, "RussoOne", 18, x0 - TextWidth("Confirm", "RussoOne", 18) / 2, y0 + 100 - 45 / 2 - 15);
            }
            else
            {
                SplashKit.FillRectangle(cf_button, x0 - 120 / 2, y0 + 100 - 45 / 2 - 25, 120, 45);
                SplashKit.FillRectangle(Color.White, x0 - 120 / 2, y0 + 100 - 45 / 2 + 45 - 29, 120, 4);
                SplashKit.DrawText("Confirm", Color.White, "RussoOne", 18, x0 - TextWidth("Confirm", "RussoOne", 18) / 2, y0 + 100 - 45 / 2 - 15);
            }
        }

        public void Draw()
        {
            DrawMenu();
            DrawHeader();

            if (Shop.ScreenType == "Tanks")
            {
                DrawItems(tanks, guns, _checkOwnedTanks, _selectedTanks);
                if (_selectedTanks >= 0)
                    DrawPurchaseProcessing(tanks[_selectedTanks], guns[_selectedTanks], "Tank");
            }
            else if (Shop.ScreenType == "Frames")
            {
                DrawItems(frames, _checkOwnedFrames, _selectedFrames, "Frame");
                if (_selectedFrames >= 0)
                    DrawPurchaseProcessing(frames[_selectedFrames], frames[_selectedFrames], "Frame");
            }
            else if (Shop.ScreenType == "Avatars")
            {
                DrawItems(avatars, _checkOwnedAvatars, _selectedAvatars, "Avatar");
                if (_selectedAvatars >= 0)
                    DrawPurchaseProcessing(avatars[_selectedAvatars], avatars[_selectedAvatars], "Avatar");
            }
            else if (Shop.ScreenType == "Items")
            {
                DrawItems(items, _checkOwnedItems, _selectedItems, "Item");
                if (_selectedItems >= 0)
                    DrawPurchaseProcessing(items[_selectedItems], items[_selectedItems], "Item");
            }

            if (_buyStatus == 1)
                DrawBuyStatus("Your purchase was successful!", Color.RGBAColor(39, 174, 96, 255), Color.RGBAColor(46, 204, 113, 255),
                               Color.RGBAColor(0, 200, 83, 255), Color.RGBAColor(0, 153, 63, 255));
            else if (_buyStatus == 0)
                DrawBuyStatus("Purchase failed: Not enough coins!", Color.RGBAColor(183, 28, 28, 255), Color.RGBAColor(220, 53, 69, 255),
                               Color.RGBAColor(229, 57, 53, 255), Color.RGBAColor(198, 40, 40, 255));
        }

        private void HandleMenu()
        {
            if (CheckOnProcessing()) return;

            if (Hover(SplashKit.MousePosition(), 30, 20, 45, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                GameManager.ScreenType = "Home";
                Shop.ScreenType = "Tanks";
            }
            else if (Hover(SplashKit.MousePosition(), 18, 120, 69, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                Shop.ScreenType = "Tanks";
            }
            else if (Hover(SplashKit.MousePosition(), 18, 200, 69, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                Shop.ScreenType = "Frames";
            }
            else if (Hover(SplashKit.MousePosition(), 18, 280, 69, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                Shop.ScreenType = "Avatars";
            }
            else if (Hover(SplashKit.MousePosition(), 18, 360, 69, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                Shop.ScreenType = "Items";
            }
        }

        private void HandleItems()
        {
            if (_selectedTanks >= 0 || _buyStatus >= 0) return;
            int x = 150, y = 120;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if (i * 3 + j + 1 > tanks.Count) break;
                    x = 150 + j * 250;
                    if (!_checkOwnedTanks[i * 3 + j] && Hover(SplashKit.MousePosition(), x, y, 150, 180))
                    {
                        if (!GameManager.MuteSound)
                            SplashKit.PlaySoundEffect("Click");
                        _selectedTanks = i * 3 + j;
                        return;
                    }
                }
                y += 240;
            }
        }

        private void HandleItems(List<Item> items, bool[] OwnedItems, ref int selectedID)
        {
            if (selectedID >= 0 || _buyStatus >= 0) return;

            int x = 150, y = 120;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (i * 4 + j + 1 > items.Count) return;
                    x = 150 + j * 200;
                    if (!OwnedItems[i * 4 + j] && Hover(SplashKit.MousePosition(), x, y, 113, 143))
                    {
                        if (!GameManager.MuteSound)
                            SplashKit.PlaySoundEffect("Click");
                        selectedID = i * 4 + j;
                        return;
                    }
                }
                y += 240;
            }
        }

        private void HandlePurchaseProcessing(List<Item> objects, ref int selectedID)
        {
            int x0 = 450, y0 = 300;
            if (Hover(SplashKit.MousePosition(), x0 - 400 / 2 + 400 - 40, y0 - 300 / 2 + 7, 30, 30))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                selectedID = -1;
            }
            else if (Hover(SplashKit.MousePosition(), x0 - 120 / 2, y0 + 100 - 45 / 2, 120, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                if (objects[selectedID].Price <= _user.Coin)
                {
                    _user.Coin = _user.Coin - objects[selectedID].Price;
                    _dbManager.UpdateUserField2(_user.Username, "Coin", _user.Coin);

                    string typeAdd = objects[selectedID].ID.Substring(0, 4);
                    if (typeAdd == "Tank")
                    {
                        _dbManager.UpdateUserField(_user.Username, "Tanks", objects[selectedID].ID);
                        _dbManager.UpdateUserField(_user.Username, "Guns", guns[selectedID].ID);
                        int pos = OrderItem(tanks, objects[selectedID].ID);
                        if (pos != -1)
                        {
                            _checkOwnedTanks[pos] = true;
                            _user.Bag.Tanks.Add(tanks[pos]);
                            _user.Bag.Guns.Add(guns[pos]);
                        }
                    }
                    else if (typeAdd == "Fram")
                    {
                        _dbManager.UpdateUserField(_user.Username, "Frames", objects[selectedID].ID);
                        int pos = OrderItem(frames, objects[selectedID].ID);
                        if (pos != -1)
                        {
                            _checkOwnedFrames[pos] = true;
                            _user.Bag.Frames.Add(frames[pos]);
                        }
                    }
                    else if (typeAdd == "Avat")
                    {
                        _dbManager.UpdateUserField(_user.Username, "Avatars", objects[selectedID].ID);
                        int pos = OrderItem(avatars, objects[selectedID].ID);
                        if (pos != -1)
                        {
                            _checkOwnedAvatars[pos] = true;
                            _user.Bag.Avatars.Add(avatars[pos]);
                        }
                    }
                    else if (typeAdd == "Item")
                    {
                        _dbManager.UpdateUserField(_user.Username, "Items", objects[selectedID].ID);
                        int pos = OrderItem(items, objects[selectedID].ID);
                        if (pos != -1)
                        {
                            _checkOwnedItems[pos] = true;
                            _user.Bag.Items.Add(items[pos]);
                        }
                    }

                    selectedID = -1;
                    _buyStatus = 1;
                }
                else
                {
                    selectedID = -1;
                    _buyStatus = 0;
                }
            }
        }

        private void HandleBuyStatus()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                int x0 = 450, y0 = 300;
                if (Hover(SplashKit.MousePosition(), x0 - 120 / 2, y0 + 100 - 45 / 2 - 25, 120, 45))
                {
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Click");
                    _buyStatus = -1;
                }
            }
        }

        public void Handle()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                HandleMenu();

                if (_buyStatus >= 0)
                {
                    HandleBuyStatus();
                    return;
                }

                if (Shop.ScreenType == "Tanks")
                {
                    if (_selectedTanks >= 0)
                        HandlePurchaseProcessing(tanks, ref _selectedTanks);
                    else
                        HandleItems();
                }
                else if (Shop.ScreenType == "Frames")
                {
                    if (_selectedFrames >= 0)
                        HandlePurchaseProcessing(frames, ref _selectedFrames);
                    else
                        HandleItems(frames, _checkOwnedFrames, ref _selectedFrames);
                }
                else if (Shop.ScreenType == "Avatars")
                {
                    if (_selectedAvatars >= 0)
                        HandlePurchaseProcessing(avatars, ref _selectedAvatars);
                    else
                        HandleItems(avatars, _checkOwnedAvatars, ref _selectedAvatars);
                }
                else if (Shop.ScreenType == "Items")
                {
                    if (_selectedItems >= 0)
                        HandlePurchaseProcessing(items, ref _selectedItems);
                    else
                        HandleItems(items, _checkOwnedItems, ref _selectedItems);
                }
            }
        }

        private int OrderItem(List<Item> items, string ID)
        {
            for (int i = 0; i < items.Count; i++)
                if (items[i].ID == ID) return i;
            return -1;
        }

        public void DrawImage(string name, Bitmap img, float x, float y, float width, float height)
        {
            var opts = OptionScaleBmp(width / img.Width, height / img.Height);
            SplashKit.DrawBitmap(name, x, y, opts);
        }

        private bool Hover(Point2D mouse, int x, int y, int w, int h)
        {
            if (SplashKit.PointInRectangle(mouse, SplashKit.RectangleFrom(x, y, w, h)))
                return true;
            return false;
        }

        private float TextWidth(string letters, string font, int size_font)
        {
            return SplashKit.TextWidth(letters, font, size_font);
        }
    }
}
