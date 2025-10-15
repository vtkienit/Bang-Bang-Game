using SplashKitSDK;
using System.Net.Sockets;
using System.Text;

namespace BangBang
{
    public class ShopBattle:Page
    {
        private int _itemSelected, _buyStatus, _typeBattle;
        private bool _openShop, _buyProcess;
        private bool[] _itemsOwned;
        private ItemBattle[] _itemBattles;
        private Bitmap _shopImages;
        private Player _player;
        private TcpClient _client;

        public ShopBattle(Player player, TcpClient client, int TypeBattle)
        {
            _player = player;
            _client = client;
            _itemBattles = new ItemBattle[6];
            _itemsOwned = new bool[6];
            _typeBattle = TypeBattle;

            _shopImages = SplashKit.LoadBitmap("ShopBattle", "Images/ShopBattle.png");

            SetUpItemBattles();
        }

        private void SetUpItemBattles()
        {
            _itemBattles[0] = new ItemBattle("d1", "Dame", "Blood Bullet", "Increase Damage", "Increase Damage by 80%", 240, "dame", 80);
            _itemBattles[1] = new ItemBattle("a1", "ATK", "Haunted Arrow", "Increase Attack Speed", "Increase Attack Speed by 40%", 120, "atk", 40);
            _itemBattles[2] = new ItemBattle("s1", "Speed", "Swan Shoe", "Increase Movement Speed", "Increase Movement Speed by 20%", 60, "speed", 20);
            _itemBattles[3] = new ItemBattle("b1", "Blood", "Blood of God", "Increase Max HP", "Increase Max HP by 60%", 240, "blood", 60);
            _itemBattles[4] = new ItemBattle("m1", "Armor", "Guardian Armor", "Increase Armor", "Increase Armor by 60%", 180, "armor", 60);
            _itemBattles[5] = new ItemBattle("p1", "Piercing", "Penetrating Bow", "Increase Piercing", "Increase Piercing by 60%", 200, "piercing", 60);
        }

        private void DrawShop()
        {
            if (Hover(SplashKit.MousePosition(), 1, 600 / 2 - 30, 40, 60))
            {
                SplashKit.FillRectangle(Color.RGBAColor(255, 193, 7, 100), 1, 600 / 2 - 30, 40, 60);
                SplashKit.DrawBitmap(_shopImages, 1 + 40 / 2 - _shopImages.Width / 2 - 2, 600 / 2 - _shopImages.Height / 2 - 10);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(255, 193, 7, 60), 1, 600 / 2 - 30, 40, 60);
                SplashKit.DrawBitmap(_shopImages, 1 + 40 / 2 - _shopImages.Width / 2 - 2, 600 / 2 - _shopImages.Height / 2 - 10);
            }

            SplashKit.DrawText(_player.coin.ToString(), Color.White, "RussoOne", 14, 1 + 40 / 2 - TextWidth(_player.coin.ToString(), "RussoOne", 14) / 2, 600 / 2 + 10);
            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 230), 1, 600 / 2 + 30, 40, 3);

            if (_openShop) DrawInsideShop();
        }

        private void DrawInsideShop()
        {
            float x = 900 / 2 - 720 / 2;
            float y = 600 / 2 - 420 / 2;

            SplashKit.FillRectangle(Color.RGBAColor(44, 44, 110, 220), x, y, 720, 420);
            SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 200), x, y, 5, 420);

            SplashKit.DrawBitmap("CoinBattle", x + 26, y + 10);
            SplashKit.DrawText(_player.coin.ToString(), Color.RGBAColor(255, 248, 220, 200), "RussoOne", 15, x + 49, y + 11);

            SplashKit.DrawText("Shop", Color.RGBAColor(255, 215, 0, 255), "RussoOne", 25, 900 / 2 - TextWidth("Shop", "RussoOne", 25) / 2, y + 10);


            if ((!_buyProcess && _buyStatus == 0) && Hover(SplashKit.MousePosition(), (int)x + 720 - 35, (int)y + 10, 25, 25))
                SplashKit.DrawBitmap("CloseShopBattleHover", x + 720 - 35, y + 10);
            else
                SplashKit.DrawBitmap("CloseShopBattle", x + 720 - 35, y + 10);

            SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 200), x + 5, y + 340, 715, 1);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int space = 230;
                    SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), x + 30 + j * space, y + 100, 62, 62);

                    SplashKit.DrawBitmap(_itemBattles[i * 3 + j].Image, x + 30 + 1 + j * space, y + 100 + 1);
                    SplashKit.DrawText(_itemBattles[i * 3 + j].Price.ToString(), Color.RGBAColor(255, 255, 255, 200), "RussoOne", 13, x + 88 - TextWidth(_itemBattles[i * 3 + j].Price.ToString(), "RussoOne", 13) + j * space, y + 143);
                    SplashKit.DrawText(_itemBattles[i * 3 + j].Name, Color.RGBAColor(255, 255, 255, 200), "RussoOne", 14, x + 92 + 5 + j * space, y + 100 + 5);
                    SplashKit.DrawText(_itemBattles[i * 3 + j].ShortDesc, Color.RGBAColor(200, 200, 200, 200), "RussoOne", 11, x + 92 + 5 + j * space, y + 100 + 40);

                    if (_itemsOwned[i * 3 + j])
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 235), x + 30 + j * space, y + 100, 62, 2);
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 235), x + 30 + 60 + j * space, y + 100 + 2, 2, 60);
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 235), x + 30 + j * space, y + 100 + 60, 24, 2);
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 235), x + 30 + j * space + 38, y + 100 + 60, 23, 2);
                        SplashKit.FillRectangle(Color.RGBAColor(0, 255, 255, 235), x + 30 + j * space, y + 100 + 2, 2, 58);
                        SplashKit.DrawBitmap("CheckItem", x + 92 - 39 + j * space, y + 162 - 8);
                    }
                    else if ((!_buyProcess && _buyStatus == 0) && Hover(SplashKit.MousePosition(), (int)x + 30 + j * space, (int)y + 100, 62, 62))
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 255), x + 30 + j * space, y + 100, 62, 2);
                        SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 255), x + 30 + 60 + j * space, y + 100 + 2, 2, 60);
                        SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 255), x + 30 + j * space, y + 100 + 60, 60, 2);
                        SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 255), x + 30 + j * space, y + 100 + 2, 2, 58);
                    }
                    else
                    {
                        SplashKit.FillRectangle(Color.RGBAColor(200, 200, 200, 150), x + 30 + j * space, y + 100, 62, 2);
                        SplashKit.FillRectangle(Color.RGBAColor(200, 200, 200, 150), x + 30 + 60 + j * space, y + 100 + 2, 2, 60);
                        SplashKit.FillRectangle(Color.RGBAColor(200, 200, 200, 150), x + 30 + j * space, y + 100 + 60, 60, 2);
                        SplashKit.FillRectangle(Color.RGBAColor(200, 200, 200, 150), x + 30 + j * space, y + 100 + 2, 2, 58);
                    }
                }
                y += 130;
            }


            for (int i = 0; i < 6; i++)
            {
                int space = 100, spaceH = 90, initX = 75;
                SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), x + initX + i * space, y + spaceH, 62, 62);

                if ((!_buyProcess && _buyStatus == 0) && Hover(SplashKit.MousePosition(), (int)x + initX + i * space, (int)y + spaceH, 62, 62))
                {
                    SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 150), x + initX + i * space, y + spaceH, 62, 2);
                    SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 150), x + initX + 60 + i * space, y + spaceH + 2, 2, 60);
                    SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 150), x + initX + i * space, y + spaceH + 60, 60, 2);
                    SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 150), x + initX + i * space, y + spaceH + 2, 2, 58);
                }
                else
                {
                    SplashKit.FillRectangle(Color.RGBAColor(100, 100, 100, 200), x + initX + i * space, y + spaceH, 62, 2);
                    SplashKit.FillRectangle(Color.RGBAColor(100, 100, 100, 200), x + initX + 60 + i * space, y + spaceH + 2, 2, 60);
                    SplashKit.FillRectangle(Color.RGBAColor(100, 100, 100, 200), x + initX + i * space, y + spaceH + 60, 60, 2);
                    SplashKit.FillRectangle(Color.RGBAColor(100, 100, 100, 200), x + initX + i * space, y + spaceH + 2, 2, 58);
                }

                if (i < _player.Bag.Count)
                    SplashKit.DrawBitmap(_player.Bag[i].Image, x + initX + 1 + i * space, y + spaceH + 1);
            }
        }

        private void DrawBuyProcess()
        {
            int x0 = 450, y0 = 300;
            int x = 400, y = 260;
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), 0, 0, 900, 600);
            SplashKit.FillRectangle(Color.RGBAColor(214, 234, 252, 255), x0 - x / 2, y0 - y / 2, x, y);
            if (_itemsOwned[_itemSelected])
            {
                SplashKit.FillRectangle(Color.RGBAColor(200, 0, 0, 255), x0 - x / 2, y0 - y / 2, x, 45);
                SplashKit.DrawText("Selling", Color.White, "RussoOne", 20, x0 - x / 2 + 15, y0 - y / 2 + 10);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 255), x0 - x / 2, y0 - y / 2, x, 45);
                SplashKit.DrawText("Purchase", Color.White, "RussoOne", 20, x0 - x / 2 + 15, y0 - y / 2 + 10);
            }
                

            if (Hover(SplashKit.MousePosition(), x0 - x / 2 + x - 40, y0 - y / 2 + 7, 30, 30))
                SplashKit.DrawBitmap("Close_Button_Hover", x0 - x / 2 + x - 40, y0 - y / 2 + 7);
            else
                SplashKit.DrawBitmap("Close_Button", x0 - x / 2 + x - 40, y0 - y / 2 + 7);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 40), x0 - x / 2 + 13, y0 - y / 2 + 58, 374, 129);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), x0 - x / 2 + 20, y0 - y / 2 + 65, 113, 113);
            SplashKit.FillRectangle(Color.White, x0 - x / 2 + 20, y0 - y / 2 + 65, 113, 3);
            SplashKit.FillRectangle(Color.White, x0 - x / 2 + 20 + 113, y0 - y / 2 + 65, 3, 113);
            SplashKit.FillRectangle(Color.White, x0 - x / 2 + 20, y0 - y / 2 + 65 + 113, 116, 3);
            SplashKit.FillRectangle(Color.White, x0 - x / 2 + 20, y0 - y / 2 + 65, 3, 113);

            SplashKit.DrawText(_itemBattles[_itemSelected].Name, Color.White, "RussoOne", 20, x0 - x / 2 + 20 + 123, y0 - y / 2 + 70);
            SplashKit.DrawText(_itemBattles[_itemSelected].FullDesc, Color.Gray, "RussoOne", 14, x0 - x / 2 + 20 + 123, y0 - y / 2 + 105);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 50), x0 - x / 2 + 20 + 123, y0 - y / 2 + 142, 40 + TextWidth(_itemBattles[_itemSelected].Price.ToString(), "Arial", 18) + 12, 31);
            SplashKit.DrawBitmap("CoinBattle2", x0 - x / 2 + 20 + 126, y0 - y / 2 + 142.5);
            int coin = _itemBattles[_itemSelected].Price;
            if (_itemsOwned[_itemSelected])
                SplashKit.DrawText((coin / 2).ToString(), Color.White, "RussoOne", 18, x0 - x / 2 + 20 + 160, y0 - y / 2 + 146.5);
            else
                SplashKit.DrawText(coin.ToString(), Color.White, "RussoOne", 18, x0 - x / 2 + 20 + 160, y0 - y / 2 + 146.5);


            if (Hover(SplashKit.MousePosition(), x0 - 120 / 2, y0 + 93 - 45 / 2, 120, 45))
            {
                if (_itemsOwned[_itemSelected])
                    SplashKit.FillRectangle(Color.RGBAColor(130, 0, 0, 255), x0 - 120 / 2, y0 + 93 - 45 / 2, 120, 45);
                else
                    SplashKit.FillRectangle(Color.RGBAColor(218, 165, 32, 255), x0 - 120 / 2, y0 + 93 - 45 / 2, 120, 45);
            }
            else
            {
                if (_itemsOwned[_itemSelected])
                    SplashKit.FillRectangle(Color.RGBAColor(230, 0, 0, 255), x0 - 120 / 2, y0 + 93 - 45 / 2, 120, 45);
                else
                    SplashKit.FillRectangle(Color.RGBAColor(255, 215, 0, 255), x0 - 120 / 2, y0 + 93 - 45 / 2, 120, 45);
            }
                
            SplashKit.FillRectangle(Color.White, x0 - 120 / 2, y0 + 93 - 45 / 2 + 45 - 4, 120, 4);

            if (_itemsOwned[_itemSelected])
            {
                SplashKit.DrawText("Sell", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 18, x0 - TextWidth("Sell", "RussoOne", 18) / 2 + 1, y0 + 93 - 45 / 2 + 10 + 1);
                SplashKit.DrawText("Sell", Color.White, "RussoOne", 18, x0 - TextWidth("Sell", "RussoOne", 18) / 2, y0 + 93 - 45 / 2 + 10);
            }
            else
            {
                SplashKit.DrawText("Buy", Color.RGBAColor(0, 0, 0, 100), "RussoOne", 18, x0 - TextWidth("Buy", "RussoOne", 18) / 2 + 1, y0 + 93 - 45 / 2 + 10 + 1);
                SplashKit.DrawText("Buy", Color.White, "RussoOne", 18, x0 - TextWidth("Buy", "RussoOne", 18) / 2, y0 + 93 - 45 / 2 + 10);
            }

            int x2 = x0 - x / 2 + 20, y2 = y0 - y / 2 + 65;
            SplashKit.DrawBitmap(_itemBattles[_itemSelected].Image, x2 + 113 / 2 - _itemBattles[_itemSelected].Image.Width / 2, y2 + 113 / 2 - _itemBattles[_itemSelected].Image.Height / 2 + 8);
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
            DrawShop();
            if (_buyProcess) DrawBuyProcess();

            if (_buyStatus == 1)
                DrawBuyStatus("Your purchase was successful!", Color.RGBAColor(39, 174, 96, 255), Color.RGBAColor(46, 204, 113, 255),
                               Color.RGBAColor(0, 200, 83, 255), Color.RGBAColor(0, 153, 63, 255));
            else if (_buyStatus == 2)
                DrawBuyStatus("Purchase failed: Not enough coins!", Color.RGBAColor(183, 28, 28, 255), Color.RGBAColor(220, 53, 69, 255),
                               Color.RGBAColor(229, 57, 53, 255), Color.RGBAColor(198, 40, 40, 255));
            else if (_buyStatus == 3)
                DrawBuyStatus("Item sold successfully!", Color.RGBAColor(39, 174, 96, 255), Color.RGBAColor(46, 204, 113, 255),
                               Color.RGBAColor(0, 200, 83, 255), Color.RGBAColor(0, 153, 63, 255));
        }

        public void Handle()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                HandleShop();
            }
        }

        private void HandleShop()
        {
            if (Hover(SplashKit.MousePosition(), 1, 600 / 2 - 30, 40, 60))
            {
                _player.mute = true;
                _openShop = true;
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
            }
                

            if (_openShop && (!_buyProcess && _buyStatus == 0)) HandleInSideShop();

            if (_buyProcess) HandleBuyProcess();
            else if (_buyStatus > 0) HandleBuyStatus();
        }

        private void HandleInSideShop()
        {
            float x = 900 / 2 - 720 / 2;
            float y = 600 / 2 - 420 / 2;
            if (Hover(SplashKit.MousePosition(), (int)x + 720 - 35, (int)y + 10, 25, 25))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");

                _player.mute = false;
                _openShop = false;
            }
                

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int space = 230;
                    if (Hover(SplashKit.MousePosition(), (int)x + 30 + j * space, (int)y + 100, 62, 62))
                    {
                        if (!GameManager.MuteSound)
                            SplashKit.PlaySoundEffect("Click");
                        _itemSelected = i * 3 + j;
                        _buyProcess = true;
                    }
                }
                y += 130;
            }
        }

        private void HandleBuyProcess()
        {
            int x0 = 450, y0 = 300;
            int x = 400, y = 260;

            if (Hover(SplashKit.MousePosition(), x0 - x / 2 + x - 40, y0 - y / 2 + 7, 30, 30))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                _buyProcess = false;
            }

            if (Hover(SplashKit.MousePosition(), x0 - 120 / 2, y0 + 93 - 45 / 2, 120, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");

                if (_itemsOwned[_itemSelected])
                {
                    _buyStatus = 3;
                    _player.Bag.RemoveAll(item => item.Name == _itemBattles[_itemSelected].Name);
                    _player.RemovePower(_itemBattles[_itemSelected]);
                    _player.coin += _itemBattles[_itemSelected].Price / 2;
                    _itemsOwned[_itemSelected] = false;
                    if (_typeBattle == 2)
                        SendMessage($"Sell:{_itemBattles[_itemSelected].ID}");
                }
                else if (_player.coin < _itemBattles[_itemSelected].Price)
                    _buyStatus = 2;
                else
                {
                    _buyStatus = 1;
                    _player.Bag.Add(_itemBattles[_itemSelected]);
                    _player.AddPower(_itemBattles[_itemSelected]);
                    _player.coin -= _itemBattles[_itemSelected].Price;
                    _itemsOwned[_itemSelected] = true;
                    if (_typeBattle == 2)
                        SendMessage($"Buy:{_itemBattles[_itemSelected].ID}");
                }

                _buyProcess = false;
            }
        }

        private void HandleBuyStatus()
        {
            int x0 = 450, y0 = 300;
            if (Hover(SplashKit.MousePosition(), x0 - 120 / 2, y0 + 100 - 45 / 2 - 25, 120, 45))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                _buyStatus = 0;
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

        public bool Hover(Point2D mouse, int x, int y, int w, int h)
        {
            if (SplashKit.PointInRectangle(mouse, SplashKit.RectangleFrom(x, y, w, h)))
            {
                return true;
            }
            return false;
        }

        public float TextWidth(string letters, string font, int size_font)
        {
            return SplashKit.TextWidth(letters, font, size_font);
        }
    }
}
