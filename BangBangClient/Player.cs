using SplashKitSDK;

namespace BangBang
{
    public class Player : Character
    {
        private bool _main, _recall;
        private Bitmap _bgCooldowns, _playerMini, _tankOrigin, _tankBush, _gunOrigin, _gunBush;

        protected Bitmap[] _skillCD, _skillImages;
        protected bool[,] _checkMove, _checkBush;
        protected string _lane;

        public float slipYCoin, slipYHealing, healing;
        public int coin = 0, gainCoin;
        public bool InBush;
        public List<ItemBattle> Bag;

        public Player(User User, string Name, float X, float Y, int Side, bool Main, string Lane) : base(User, Name, X, Y, Side)
        {
            _checkMove = new bool[(int)_map.PixelHeight + 2, (int)_map.PixelWidth + 2];
            _checkBush = new bool[(int)_map.PixelHeight + 2, (int)_map.PixelWidth + 2];
            Bag = new List<ItemBattle>();
            _skillCD = new Bitmap[5];
            _skillImages = new Bitmap[5];
            _moveSpeed = _moveSpeedOrigin = 2;
            _lane = Lane;

            if (Main)
                _hpImage = SplashKit.LoadBitmap("HP_Main", "Images/HP_Main.png");

            _tankOrigin = _tank;
            _gunOrigin = _gun;
            _tankBush = User.Bag.LocateItem(User.Bag.Tanks, User.TankUsed).ImageBush;
            _gunBush = User.Bag.LocateItem(User.Bag.Guns, User.GunUsed).ImageBush;

            _bgCooldowns = SplashKit.LoadBitmap("BackgroundCooldowns", "Images/BackgroundCooldowns.png");
            _skillCD[0] = SplashKit.LoadBitmap("Skill1CD_Player", "Images/Skill1CD_Player.png");
            _skillCD[1] = SplashKit.LoadBitmap("Skill2CD_Player", "Images/Skill2CD_Player.png");
            _skillCD[2] = SplashKit.LoadBitmap("Skill3CD_Player", "Images/Skill3CD_Player.png");
            _skillCD[3] = SplashKit.LoadBitmap("Recall", "Images/Recall.png");

            _skillImages[0] = SplashKit.LoadBitmap("BulletPlayer", "Images/BulletPlayer.png");
            _skillImages[1] = SplashKit.LoadBitmap("Skill1_Player", "Images/Skill1_Player.png");
            _skillImages[2] = SplashKit.LoadBitmap("Skill2_Player", "Images/Skill2_Player.png");
            _skillImages[3] = SplashKit.LoadBitmap("Skill3_Player", "Images/Skill3_Player.png");
            
            _playerMini = SplashKit.LoadBitmap("TankColdMini", "Images/TankColdMini.png");

            SplashKit.LoadBitmap("CoinBattle", "Images/CoinBattle.png");
            SplashKit.LoadBitmap("CheckItem", "Images/CheckItem.png");
            SplashKit.LoadBitmap("CoinBattle2", "Images/CoinBattle2.png");
            SplashKit.LoadBitmap("Close_Button", "Images/Close_Button.png");
            SplashKit.LoadBitmap("Close_Button_Hover", "Images/Close_Button_Hover.png");
            SplashKit.LoadBitmap("CloseShopBattle", "Images/CloseShopBattle.png");
            SplashKit.LoadBitmap("CloseShopBattleHover", "Images/CloseShopBattleHover.png");

            SetupPosition();
            CreateVirtualMap();
            CreateVirtualBush();
        }

        protected virtual void SetupPosition()
        {
            if (Side == 1)
            {
                if (_lane == "mid")
                    _y = 39 * _map.TileHeight;
                else if (_lane == "top")
                    _y = 10 * _map.TileHeight;
                else
                    _y = 68 * _map.TileHeight;

                _x = 4 * _map.TileWidth;
            }
            else
            {
                if (_lane == "mid")
                    _y = 39 * _map.TileHeight;
                else if (_lane == "top")
                    _y = 10 * _map.TileHeight;
                else
                    _y = 68 * _map.TileHeight;

                _x = 99 * _map.TileWidth;
            }

            _cameraX = Math.Max(0, Math.Min(_x - _map.ScreenWidth / 2, _map.PixelWidth - _map.ScreenWidth));
            _cameraY = Math.Max(0, Math.Min(_y - _map.ScreenHeight / 2, _map.PixelHeight - _map.ScreenHeight));
        }

        private void CreateVirtualMap()
        {
            for (int i = 0; i < _map.Height; i++)
                for (int j = 0; j < _map.Width; j++)
                    if (_map.MapGame[i][j] != '.' && _map.MapGame[i][j] != 'g' && _map.MapGame[i][j] != 'G' && _map.MapGame[i][j] != 'B')
                    {
                        int kStart = Math.Max(_map.TileHeight * i - 20, 0);
                        int kEnd = Math.Min(_map.TileHeight * (i + 1) + 20, (int)_map.PixelHeight);

                        int lStart = Math.Max(_map.TileWidth * j - 20, 0);
                        int lEnd = Math.Min(_map.TileWidth * (j + 1) + 20, (int)_map.PixelWidth);

                        for (int k = kStart; k < kEnd; k++)
                            for (int l = lStart; l < lEnd; l++)
                                _checkMove[k, l] = true;
                    }
        }

        private void CreateVirtualBush()
        {
            int offsetLength = 15;
            for (int i = 0; i < _map.Height; i++)
                for (int j = 0; j < _map.Width; j++)
                    if (_map.MapGame[i][j] == 'g' || _map.MapGame[i][j] == 'G' || _map.MapGame[i][j] == 'B')
                    {
                        int kStart = Math.Max(_map.TileHeight * i - offsetLength, 0);
                        int kEnd = Math.Min(_map.TileHeight * (i + 1) + offsetLength, (int)_map.PixelHeight);

                        int lStart = Math.Max(_map.TileWidth * j - offsetLength, 0);
                        int lEnd = Math.Min(_map.TileWidth * (j + 1) + offsetLength, (int)_map.PixelWidth);

                        for (int k = kStart; k < kEnd; k++)
                            for (int l = lStart; l < lEnd; l++)
                                _checkBush[k, l] = true;
                    }
        }

        protected override void DrawGun(float targetX, float targetY, float CameraX, float CameraY, float offsetX, float offsetY)
        {
            float Xreal = _x - CameraX;
            float Yreal;
            if (_tankAngle == 90 || _tankAngle == 270)
            {
                _gunYCheck++;
                float count = (float)Math.Min(_gunYCheck / 2, 5f);
                Yreal = _y - CameraY - 15f + count;
            }
            else
            {
                _gunYCheck = 0;
                Yreal = _y - CameraY - 15;
            }

            float dx = targetX - Xreal;
            float dy = targetY - Yreal;

            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length > 0.0f)
            {
                dx /= length;
                dy /= length;
            }

            float angleRad = (float)Math.Atan2(dy, dx) + (float)Math.PI / 2;
            float angleDeg = angleRad * 180 / (float)Math.PI;
            _gunAngle = angleDeg;

            _gunTipX = Xreal + _gun.Height * 0.68f * (float)Math.Sin(angleRad);
            _gunTipY = Yreal - _gun.Height * 0.68f * (float)Math.Cos(angleRad) + 10;

            _gunTipXOrigin = _x + _gun.Height * 0.68f * (float)Math.Sin(angleRad);
            _gunTipYOrigin = _y - 15f + (float)Math.Min(_gunYCheck / 2, 5f) - _gun.Height * 0.68f * (float)Math.Cos(angleRad) + 10;

            //SplashKit.FillCircle(Color.Red, _gunTipXOrigin - CameraX, _gunTipYOrigin - CameraY, 3);
           
            DrawRotation(_gun, Xreal - _gun.Width / 2, Yreal - _gun.Height / 2, angleDeg, offsetX, offsetY);
            //SplashKit.FillCircle(Color.Red, _x - CameraX, _y - CameraY, 3);
            //SplashKit.FillCircle(Color.Black, Xreal, Yreal + 10, 3);
            //SplashKit.FillCircle(Color.Black, _gunTipX, _gunTipY, 3);
        }

        private void DrawCoolDowns()
        {
            SplashKit.DrawBitmap(_bgCooldowns, 900 / 2 - _bgCooldowns.Width / 2, 490);

            float x = 900 / 2 - _bgCooldowns.Width / 2 + 34 + 45 / 2;
            float y = 550 + 45 /2;

            for (int i = 0; i < 3; i++)
                SplashKit.DrawBitmap(_skillCD[i], x + 56 * i - _skillCD[i].Width / 2, y - _skillCD[i].Height / 2);

            SplashKit.DrawBitmap(_skillCD[3], x + 57 * 3 - _skillCD[3].Width / 2 + 2, y - _skillCD[3].Height / 2 + 1);

            x = 900 / 2 - _bgCooldowns.Width / 2 + 35;
            y = 550;
            if (cooldowns[1] > 0.0f)
                DrawCooldown1(cooldowns[1], x, y);
            SplashKit.DrawText("Q", Color.White, "RussoOne", 12, x + 45 - TextWidth("Q", "RussoOne", 12) - 1, y + 44 - 13);

            x += 50;
            if (cooldowns[2] > 0.0f)
                DrawCooldown2and3(cooldowns[2], x, y);
            SplashKit.DrawText("E", Color.White, "RussoOne", 12, x + 52 - TextWidth("E", "RussoOne", 12) - 1, y + 44 - 13);

            x += 58;
            if (cooldowns[3] > 0.0f)
                DrawCooldown2and3(cooldowns[3], x, y);
            SplashKit.DrawText("Space", Color.White, "RussoOne", 12, x + 52 - TextWidth("Space", "RussoOne", 12) - 1, y + 44 - 13);

            x += 58;
            if (cooldowns[4] > 0.0f)
                DrawCooldown4(cooldowns[4], x, y);
            SplashKit.DrawText("B", Color.White, "RussoOne", 12, x + 65 - TextWidth("B", "RussoOne", 12) - 1, y + 44 - 13);

        }

        private void DrawCooldown1(float cooldown, float x, float y)
        {
            Triangle t1 = new Triangle
            {
                Points = new Point2D[]
                {
                    SplashKit.PointAt(x, y),
                    SplashKit.PointAt(x + 45, y),
                    SplashKit.PointAt(x + 45, y + 44)
                }
            };

            Triangle t2 = new Triangle
            {
                Points = new Point2D[]
                {
                    SplashKit.PointAt(x - 1, y),
                    SplashKit.PointAt(x + 45 - 1, y + 44),
                    SplashKit.PointAt(x - 15, y + 44)
                }
            };

            SplashKit.FillTriangle(Color.RGBAColor(0, 0, 0, 128), t1);
            SplashKit.FillTriangle(Color.RGBAColor(0, 0, 0, 128), t2);

            string seconds = ((float)Math.Round(cooldown / FPS, 1)).ToString();
            float secondSize = TextWidth(seconds, "RussoOne", 13);

            SplashKit.DrawText(seconds, Color.White, "RussoOne", 13, x + 45 / 2 - secondSize / 2, y + 45 / 2 - 9);
        }

        private void DrawCooldown2and3(float cooldown, float x, float y)
        {
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 128), x, y, 52, 45);

            string seconds = ((float)Math.Round(cooldown / FPS, 1)).ToString();
            float secondSize = TextWidth(seconds, "RussoOne", 13);

            SplashKit.DrawText(seconds, Color.White, "RussoOne", 13, x + 52 / 2 - secondSize / 2, y + 45 / 2 - 9);
        }

        private void DrawCooldown4(float cooldown, float x, float y)
        {
            Triangle t1 = new Triangle
            {
                Points = new Point2D[]
    {
                    SplashKit.PointAt(x, y),
                    SplashKit.PointAt(x + 51, y),
                    SplashKit.PointAt(x + 65, y + 44)
    }
            };

            Triangle t2 = new Triangle
            {
                Points = new Point2D[]
                {
                    SplashKit.PointAt(x - 1, y),
                    SplashKit.PointAt(x + 65 - 1, y + 44),
                    SplashKit.PointAt(x - 1, y + 44)
                }
            };

            SplashKit.FillTriangle(Color.RGBAColor(0, 0, 0, 128), t1);
            SplashKit.FillTriangle(Color.RGBAColor(0, 0, 0, 128), t2);

            string seconds = ((float)Math.Round(cooldown / FPS, 1)).ToString();
            float secondSize = TextWidth(seconds, "RussoOne", 13);

            SplashKit.DrawText(seconds, Color.White, "RussoOne", 13, x + 51 / 2 - secondSize / 2, y + 45 / 2 - 9);
        }

        private void DrawTimeBack()
        {
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 150), 0, 0, 900, 600);

            int time = (int)(cooldowns[5] / FPS);

            float t1W = TextWidth(time.ToString() + " seconds left to revive", "RussoOne", 25);
            SplashKit.FillRectangle(Color.RGBAColor(0, 184, 255, 130), 450 - t1W / 2 - 20, 260, t1W + 40, 70);
            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 130), 450 - t1W / 2 - 20, 260 + 70 - 5, t1W + 40, 5);
            SplashKit.DrawText(time.ToString() + " seconds left to revive", Color.White, "RussoOne", 25, 450 - t1W / 2, 280);
        }

        protected void DrawInMiniMap()
        {
            float w = 212, h = 148;
            float xinit = 900 - w, yinit = 600 - h;
            int col = (int)(_x / _map.TileWidth);
            int row = (int)(_y / _map.TileHeight);
            float miniX = xinit + col * _map.TileMiniWidth - _playerMini.Width / 2;
            float miniY = yinit + row * _map.TileMiniHeight - _playerMini.Height / 2;
            
            Color color;
            if (_main) color = Color.RGBAColor(0, 255, 0, 200);
            else if (Side == 1) color = Color.RGBAColor(0, 255, 255, 180);
            else color = Color.RGBAColor(255, 0, 0, 200);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), miniX, miniY, _playerMini.Width + 2, _playerMini.Height + 2);
            SplashKit.DrawBitmap(_playerMini, miniX, miniY);
            SplashKit.FillRectangle(color, miniX - 1, miniY - 1, _playerMini.Width + 2, 1);
            SplashKit.FillRectangle(color, miniX - 1 + _playerMini.Width + 1, miniY - 1 + 1, 1, _playerMini.Height + 1);
            SplashKit.FillRectangle(color, miniX - 1 , miniY - 1 + _playerMini.Height + 1, _playerMini.Width + 1, 1);
            SplashKit.FillRectangle(color, miniX - 1, miniY, 1, _playerMini.Height );
        }

        private void DrawGainCoin(float cameraX, float cameraY)
        {
            if (cooldowns[7] == 0.00f) return;
            float length = TextWidth(" + " + gainCoin.ToString() + "c", "RussoOne", 13);
            SplashKit.DrawText(" + " + gainCoin.ToString() + "c", Color.Yellow, "RussoOne", 13, _x - cameraX - length / 2, _y - cameraY - slipYCoin);
            slipYCoin += 0.7f;
        }

        private void DrawRecallVisible(float cameraX, float cameraY)
        {
            if (cooldowns[4] == 0.00f || !_recall) return;
            float percent = (timeSkills[4] - cooldowns[4]) / timeSkills[4];

            if (Side == 1)
            {
                SplashKit.FillCircle(Color.RGBAColor(0, 0, 255, 40), X - cameraX, Y - cameraY, 30 + percent * 40);
                SplashKit.DrawCircle(Color.RGBAColor(0, 0, 255, 200), X - cameraX, Y - cameraY, 30 + percent * 40);
            }
            else
            {
                SplashKit.FillCircle(Color.RGBAColor(255, 0, 0, 40), X - cameraX, Y - cameraY, 30 + percent * 40);
                SplashKit.DrawCircle(Color.RGBAColor(255, 0, 0, 200), X - cameraX, Y - cameraY, 30 + percent * 40);
            }
        }

        private void DrawRecallInvisible(float cameraX, float cameraY)
        {
            if (cooldowns[4] == 0.00f || !_recall) return;
            float percent = (timeSkills[4] - cooldowns[4]) / timeSkills[4];

            float lenBar = 80;
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 150), X - cameraX - lenBar / 2, Y - cameraY + 75, lenBar, 12);

            float lenText = TextWidth("Recall", "RussoOne", 16);
            if (Side == 1)
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 50, 255, 180), X - cameraX - lenBar / 2, Y - cameraY + 75, percent * lenBar, 12);
                SplashKit.DrawText("Recall", Color.RGBAColor(0, 50, 255, 255), "RussoOne", 16, X - cameraX - lenText / 2, Y - cameraY + 90);
            }
            else
            {
                SplashKit.FillRectangle(Color.RGBAColor(255, 0, 0, 180), X - cameraX - lenBar / 2, Y - cameraY + 75, percent * lenBar, 12);
                SplashKit.DrawText("Recall", Color.RGBAColor(255, 0, 0, 255), "RussoOne", 16, X - cameraX - lenText / 2, Y - cameraY + 90);
            }
        }

        private void DrawHealing(float cameraX, float cameraY)
        {
            if (cooldowns[9] == 0.00f) return;
            float len1 = TextWidth("+", "RussoOne", 15);
            float len2 = TextWidth(healing.ToString(), "RussoOne", 15);
            SplashKit.DrawText("+" + healing.ToString(), Color.RGBAColor(0, 255, 0, 255), "RussoOne", 15, _x - cameraX - len2 / 2 - len1, _y - cameraY - slipYHealing);
            slipYHealing += 0.7f;
        }

        public override void Draw(float targetX, float targetY, float cameraX, float cameraY)
        {
            if (destroyed) return;
            DrawRecallVisible(cameraX, cameraY);
            DrawHpBar(cameraX, cameraY);
            DrawName(cameraX, cameraY);
            base.Draw(targetX, targetY, cameraX, cameraY);
            DrawGun(targetX, targetY, cameraX, cameraY, 0, 10);
            DrawInMiniMap();
            DrawDameTaken(cameraX, cameraY);
            DrawGainCoin(cameraX, cameraY);
            DrawHealing(cameraX, cameraY);
        }

        public void DrawOthers(float cameraX, float cameraY)
        {
            if (destroyed) DrawTimeBack();
            DrawCoolDowns();
            DrawRecallInvisible(cameraX, cameraY);
        }

        public virtual void Handle()
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
            }
        }

        protected virtual void HandleSkills()
        {
            if (SplashKit.KeyDown(KeyCode.QKey))
                Attack(this, 1, 1, _skillImages[1]);

            if (SplashKit.KeyDown(KeyCode.EKey))
                Attack(this, 1, 2, _skillImages[2]);

            if (SplashKit.KeyDown(KeyCode.SpaceKey))
                Attack(this, 1, 3, _skillImages[3]);

            if (SplashKit.KeyDown(KeyCode.BKey))
            {
                HandleRecall();
            }
        }

        public override void Handle(float moveX, float moveY)
        {
            HandleRecalling();
        }

        protected virtual void HandleMovingControl()
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
                HandleMoving(moveX, moveY);
        }

        public void HandleMoving(float moveX, float moveY)
        {
            _recall = false;
            cooldowns[4] = 0.0f;
            if (!GameManager.MuteSound && !this.mute)
                SplashKit.StopSoundEffect("Recall");

            float _targetAngle = 0.0f;

            if (moveX != 0 || moveY != 0)
            {
                float length = (float)Math.Sqrt(moveX * moveX + moveY * moveY);
                moveX = moveX / length * _moveSpeed;
                moveY = moveY / length * _moveSpeed;

                _targetAngle = (float)(Math.Atan2(moveY, moveX) * 180 / Math.PI) + 90;

                float angleDiff = _targetAngle - _tankAngle;
                if (angleDiff > 180) angleDiff -= 360;
                if (angleDiff < -180) angleDiff += 360;

                float absDiff = Math.Abs(angleDiff);
                float RotationSpeed = 5;

                if (absDiff <= 47)
                    RotationSpeed = 10.0f;
                else if (absDiff <= 92)
                    RotationSpeed = 20.0f;
                else
                    RotationSpeed = 30.0f;

                if (Math.Abs(angleDiff) > RotationSpeed)
                    _tankAngle += Math.Sign(angleDiff) * RotationSpeed;
                else
                    _tankAngle = _targetAngle;

                _tankAngle = ((_tankAngle % 360) + 360) % 360;
            }

            if (Math.Abs(_tankAngle - _targetAngle) == 0.0f || Math.Abs((_tankAngle - _targetAngle + 360) % 360) == 0.0f)
            {
                _x += moveX;
                _y += moveY;
            }

            _x = Math.Max(0 + _tank.Width / 2, Math.Min(_x, _map.PixelWidth - _tank.Width));
            _y = Math.Max(0 + _tank.Height / 2, Math.Min(_y, _map.PixelHeight - _tank.Height));
            _cameraX = Math.Max(0, Math.Min(_x - _map.ScreenWidth / 2, _map.PixelWidth - _map.ScreenWidth));
            _cameraY = Math.Max(0, Math.Min(_y - _map.ScreenHeight / 2, _map.PixelHeight - _map.ScreenHeight));
            if (_checkBush[(int)_y, (int)_x])
            {
                _tank = _tankBush;
                _gun = _gunBush;
                InBush = true;
            }
            else
            {
                _tank = _tankOrigin;
                _gun = _gunOrigin;
                InBush = false;
            }
        }

        public virtual void HandleRecall()
        {
            cooldowns[4] = timeSkills[4];
            _recall = true;
            if (!GameManager.MuteSound && !this.mute)
                SplashKit.PlaySoundEffect("Recall");
        }

        protected void HandleRecalling()
        {
            if (cooldowns[4] == 0.00f && _recall)
            {
                _recall = false;
                if (Side == 1)
                    _x = 3 * _map.TileWidth;
                else
                    _x = 103 * _map.TileWidth;

                _y = 36 * _map.TileHeight;
                _cameraX = Math.Max(0, Math.Min(_x - _map.ScreenWidth / 2, _map.PixelWidth - _map.ScreenWidth));
                _cameraY = Math.Max(0, Math.Min(_y - _map.ScreenHeight / 2, _map.PixelHeight - _map.ScreenHeight));
            }
        }

        public void AddPower(ItemBattle item)
        {
            if (item.Type == "dame") _dame += _dame * item.Value / 100;
            else if (item.Type == "atk") timeSkills[0] -= timeSkills[0] * item.Value / 100;
            else if (item.Type == "speed") _moveSpeed += _moveSpeed * item.Value / 100;
            else if (item.Type == "blood") _maxHp += _maxHp * item.Value / 100;
            else if (item.Type == "armor") _armor += _armor * item.Value / 100;
            else if (item.Type == "piercing") _piercing += _piercing * item.Value / 100;
        }

        public void RemovePower(ItemBattle item)
        {
            if (item.Type == "dame") _dame -= _dameOrigin * item.Value / 100;
            else if (item.Type == "atk") timeSkills[0] += _atkOrigin * item.Value / 100;
            else if (item.Type == "speed") _moveSpeed -= _moveSpeedOrigin * item.Value / 100;
            else if (item.Type == "blood") _maxHp -= _maxHpOrigin * item.Value / 100;
            else if (item.Type == "armor") _armor -= _armorOrigin * item.Value / 100;
            else if (item.Type == "piercing") _piercing -= _piercingOrigin * item.Value / 100;
        }

        public Bitmap[] SkillImages
        {
            get { return _skillImages; }
        }

        public Bitmap TankOrigin
        {
            get { return _tankOrigin; }
            set { _tankOrigin = value; }
        }

        public Bitmap TankBush
        {
            get { return _tankBush; }
            set { _tankBush = value; }
        }

        public Bitmap GunOrigin
        {
            get { return _gunOrigin; }
            set { _gunOrigin = value; }
        }

        public Bitmap GunBush
        {
            get { return _gunBush; }
            set { _gunBush = value; }
        }

        public Bitmap PlayerMini
        {
            get { return _playerMini; }
            set { _playerMini = value; }
        }

        public string Lane
        {
            get { return _lane; }
        }
    }
}