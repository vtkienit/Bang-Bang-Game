using SplashKitSDK;

namespace BangBang
{
    public abstract class Character
    {
        private string _name;
        private int _side;

        protected float _dame, _dameOrigin, _moveSpeed, _moveSpeedOrigin, _armor, _armorOrigin, _piercing, _piercingOrigin, _atkOrigin;
        protected float _x, _y, _cameraX, _cameraY, _tankAngle, _hp, _maxHp, _maxHpOrigin;
        protected float _gunAngle, _gunYCheck, _gunTipX, _gunTipY, _gunTipXOrigin, _gunTipYOrigin;
        protected Map _map;
        protected Bitmap _tank, _gun, _hpImage, _hpBorder;
        protected Random _rnd = new Random();
        protected User _user;
        private Bitmap[] _explosion;

        public const float FPS = 60;
        public string enemyUsername = "";
        public bool destroyed, mute;
        public int NumberOfDied, NumberOfKills, typeNoti = 1;
        public float dameTaken, slipY;
        public float[] cooldowns, speedSkills, timeSkills, rangeSkills;
        public List<Bullet> Bullets;

        public Character(User User, string Name, float X, float Y, int Side)
        {
            _map = new Map("map.txt");

            LoadInitialPosition(X, Y);

            _user = User;
            destroyed = false;
            _name = Name;
            _side = Side;
            rangeSkills = new float[4];
            cooldowns = new float[12];
            timeSkills = new float[12];
            speedSkills = new float[4];
            Bullets = new List<Bullet>();
            _explosion = new Bitmap[12];
            mute = false;

            _tank = User.Bag.LocateItem(User.Bag.Tanks, User.TankUsed).ImageBattle;
            _gun = User.Bag.LocateItem(User.Bag.Guns, User.GunUsed).ImageBattle;

            if (Side == 1)
                _hpImage = SplashKit.LoadBitmap("HP_Player", "Images/HP_Player.png");
            else
                _hpImage = SplashKit.LoadBitmap("HP_Enemy", "Images/HP_Enemy.png");

            _hpBorder = SplashKit.LoadBitmap("HP_Border", "Images/HP_Border.png");

            _explosion[0] = SplashKit.LoadBitmap("boom1", "Images/boom1.png");
            for (int i = 1; i < 12; i++)
                _explosion[i] = SplashKit.LoadBitmap($"boom{i}", $"Images/boom{i}.png");
        }

        protected void LoadInitialPosition(float x, float y)
        {
            _x = x * _map.TileWidth;
            _y = y * _map.TileHeight;
            _cameraX = Math.Max(0, Math.Min(_x - _map.ScreenWidth / 2, _map.PixelWidth - _map.ScreenWidth));
            _cameraY = Math.Max(0, Math.Min(_y - _map.ScreenHeight / 2, _map.PixelHeight - _map.ScreenHeight));
        }

        public void SetUpSkill(float MaxHp, float dame, float armor, float piercing, float TimeSkill0, float TimeSkill1, float TimeSkill2, float TimeSkill3, float RangeSkill0, float RangeSkill1, float RangeSkill2, float RangeSkill3, float SpeedSkill0)
        {
            timeSkills[0] = _atkOrigin = TimeSkill0 * FPS;
            timeSkills[1] = TimeSkill1 * FPS;
            timeSkills[2] = TimeSkill2 * FPS;
            timeSkills[3] = TimeSkill3 * FPS;
            timeSkills[4] = 5 * FPS;
            timeSkills[5] = 30 * FPS;
            timeSkills[6] = 0.6f * FPS;
            timeSkills[7] = 0.6f * FPS;
            timeSkills[8] = 1.5f * FPS;
            timeSkills[9] = 0.6f * FPS;
            timeSkills[11] = 11 * 10;

            rangeSkills[0] = RangeSkill0;
            rangeSkills[1] = RangeSkill1;
            rangeSkills[2] = RangeSkill2;
            rangeSkills[3] = RangeSkill3;

            speedSkills[0] = SpeedSkill0;
            speedSkills[1] = SpeedSkill0;
            speedSkills[2] = SpeedSkill0;
            speedSkills[3] = SpeedSkill0;

            _hp = _maxHp = MaxHp;
            _maxHpOrigin = MaxHp;
            _dame = _dameOrigin = dame;
            _armor = _armorOrigin = armor;
            _piercing = _piercingOrigin = piercing;
        }

        public void SetUpSkill(float MaxHp, float dame, float armor, float piercing, float TimeSkill0, float RangeSkill0, float SpeedSkill0)
        {
            timeSkills[0] = _atkOrigin = TimeSkill0 * FPS;

            if(this is Monster)
                timeSkills[5] = 60 * FPS;
            else
                timeSkills[5] = 30 * FPS;

            timeSkills[6] = 0.6f * FPS;
            timeSkills[8] = 1.5f * FPS;
            timeSkills[9] = 0.6f * FPS;
            timeSkills[11] = 11 * 10;

            rangeSkills[0] = RangeSkill0;
            speedSkills[0] = SpeedSkill0;

            _hp = _maxHp = MaxHp;
            _maxHpOrigin = MaxHp;
            _dame = _dameOrigin = dame;
            _armor = _armorOrigin = armor;
            _piercing = _piercingOrigin = piercing;
        }

        protected virtual void DrawGun(float targetX, float targetY, float cameraX, float cameraY, float offsetX, float offsetY) { }

        protected virtual void DrawTank(float cameraX, float cameraY)
        {
            float Xreal = _x - cameraX;
            float Yreal = _y - cameraY;

            DrawingOptions options = SplashKit.OptionRotateBmp(_tankAngle);
            SplashKit.DrawBitmap(_tank, Xreal - _tank.Width / 2, Yreal - _tank.Height / 2, options);
            //SplashKit.FillCircle(Color.Red, Xreal, Yreal, 3);
        }

        protected virtual void DrawHpBar(float cameraX, float cameraY)
        {
            float barWidth = 50 * _hp / _maxHp;
            float xOffset = 50 - barWidth - 1;

            DrawImage(_hpImage, (_x - cameraX - 86) - xOffset / 2, _y - cameraY + _tank.Height / 2 - 2, barWidth, 8);
            DrawImage(_hpBorder, _x - cameraX - 86, _y - cameraY + _tank.Height / 2 - 2, 50, 8);
        }

        protected void DrawName(float cameraX, float cameraY)
        {
            SplashKit.DrawText(_name, Color.White, "RussoOne", 11, _x - cameraX - TextWidth(_name, "RussoOne", 11) / 2 + 1, _y - cameraY + _tank.Height / 2 + 12);
        }

        protected void DrawBullets(float cameraX, float cameraY)
        {
            if (Bullets.Count > 0)
            {
                foreach (var b in Bullets)
                {
                    b.Draw(cameraX, cameraY);
                }
            }
        }

        public virtual void DrawDameTaken(float cameraX, float cameraY)
        {
            if (cooldowns[6] == 0.00f) return;
            float length = TextWidth(dameTaken.ToString(), "RussoOne", 13);
            SplashKit.DrawText(dameTaken.ToString(), Color.Red, "RussoOne", 13, _x - cameraX - length / 2, _y - cameraY - slipY);
            slipY += 0.7f;
        }

        public void DrawNoti()
        {
            if (cooldowns[8] == 0.0f) return;
            string p2 = "Enemy";
            float x = 450, y = 120, lenX = 150, lenC = 60;
            string msg = "";

            if (typeNoti == 1) msg = "Kill";
            else msg = "is Killed by";

            int fontsize2 = 22, fontsize3 = 23;
            float lenText2 = TextWidth(msg, "RussoOne", fontsize2);
            float lenText3 = TextWidth(p2, "RussoOne", fontsize3);

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 100, 80), x - lenX, y, lenX, 45);
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 150, 200), x - lenC, y - 2, lenC, 5);
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 150, 200), x - lenX, y, lenX - lenC, 1);
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 150, 200), x - lenC, y + 45 - 3, lenC, 5);
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 150, 200), x - lenX, y + 44, lenX - lenC, 1);

            SplashKit.FillRectangle(Color.RGBAColor(100, 0, 0, 80), x, y, lenX, 45);
            SplashKit.FillRectangle(Color.RGBAColor(150, 0, 0, 200), x, y - 2, lenC, 5);
            SplashKit.FillRectangle(Color.RGBAColor(150, 0, 0, 200), x + lenC, y, lenX - lenC, 1);
            SplashKit.FillRectangle(Color.RGBAColor(150, 0, 0, 200), x, y + 45 - 3, lenC, 5);
            SplashKit.FillRectangle(Color.RGBAColor(150, 0, 0, 200), x + lenC, y + 44, lenX - lenC, 1);
            
            if (_side == 1)
            {
                SplashKit.DrawText(Name, Color.Blue, "RussoOne", fontsize3, x - lenX + 3, y + 9);
                SplashKit.DrawText(enemyUsername, Color.Red, "RussoOne", fontsize3, x + lenX - lenText3 - 3, y + 9);
            }
            else
            {
                SplashKit.DrawText(enemyUsername, Color.Blue, "RussoOne", fontsize3, x - lenX + 3, y + 9);
                SplashKit.DrawText(Name, Color.Red, "RussoOne", fontsize3, x + lenX - lenText3 - 3, y + 9);
            }


            SplashKit.DrawText(msg, Color.Gold, "RussoOne", fontsize2, x - lenText2 / 2, y + 10);
        }

        public void DrawExplosion(float cameraX, float cameraY)
        {
            if (cooldowns[11] == 0.0f) return;

            int frame = 11 - (int)(cooldowns[11] / 10);

            if ((this is Tower) && !(this is Monster))
                SplashKit.DrawBitmap(_explosion[frame], _x + 13.5f - _explosion[frame].Width / 2 - cameraX, _y + 13.5f - _explosion[frame].Height / 2 - cameraY);
            else
                SplashKit.DrawBitmap(_explosion[frame], _x - _explosion[frame].Width / 2 - cameraX, _y - _explosion[frame].Height / 2 - cameraY);
        }

        public virtual void Draw(float targetX, float targetY, float cameraX, float cameraY)
        {
            if (destroyed) return;
            DrawTank(cameraX, cameraY);
            DrawBullets(cameraX, cameraY);
        }

        public abstract void Handle(float moveX, float moveY);

        public virtual void Update(List<Character> Enemies1, List<Character> Enemies2, List<Character> Teammates)
        {
            UpdateBullets(Enemies1, Enemies2, Teammates);
            UpdateCooldowns();
        }

        public virtual void Update(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4, List<Character> Teammates)
        {
            UpdateBullets(Enemies1, Enemies2, Enemies3, Enemies4, Teammates);
            UpdateCooldowns();
        }

        protected virtual void UpdateBullets(List<Character> Enemies1, List<Character> Enemies2, List<Character> Teammates)
        {
            if (Bullets.Count > 0)
            {
                foreach (var b in Bullets)
                {
                    b.Update(Enemies1, Enemies2, Teammates);
                }

                Bullets.RemoveAll(b => b.OutRange(b.X, b.Y, b.ShotRange) || b.destroyed);
            }
        }

        protected virtual void UpdateBullets(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4, List<Character> Teammates)
        {
            if (Bullets.Count > 0)
            {
                foreach (var b in Bullets)
                {
                    b.Update(Enemies1, Enemies2, Enemies3, Enemies4, Teammates);
                }

                Bullets.RemoveAll(b => b.OutRange(b.X, b.Y, b.ShotRange) || b.destroyed);
            }
        }

        protected void UpdateCooldowns()
        {
            for (int i = 0; i < cooldowns.Length; i++)
            {
                cooldowns[i] = Math.Max(cooldowns[i] - 1, 0);
            }

            if ((this is Player) && cooldowns[5] == 0 && destroyed)
            {
                destroyed = false;
                _hp = _maxHp;

                if (Side == 1)
                    _x = 3 * _map.TileWidth;
                else
                    _x = 103 * _map.TileWidth;

                if (((Player)this).Lane == "top")
                    _y = 10 * _map.TileHeight;
                else if (((Player)this).Lane == "mid")
                    _y = 39 * _map.TileHeight;
                else
                    _y = 68 * _map.TileHeight;

                _cameraX = Math.Max(0, Math.Min(_x - _map.ScreenWidth / 2, _map.PixelWidth - _map.ScreenWidth));
                _cameraY = Math.Max(0, Math.Min(_y - _map.ScreenHeight / 2, _map.PixelHeight - _map.ScreenHeight));
            }else if ((this is Monster) && cooldowns[5] == 0 && destroyed)
            {
                destroyed = false;
                _hp = _maxHp;
            }
        }

        public void Attack(Character Owner, int typeCharacter, int TypeAttack, Bitmap BulletImage)
        {
            if (cooldowns[TypeAttack] != 0) return;

            if (!GameManager.MuteSound && Owner is Player && !Owner.mute)
            {
                if (TypeAttack == 0)
                    SplashKit.PlaySoundEffect("PlayerShoot");
                else if (TypeAttack == 1)
                    SplashKit.PlaySoundEffect("PlayerSkill1");
                else if (TypeAttack == 2)
                    SplashKit.PlaySoundEffect("PlayerSkill2");
                else
                    SplashKit.PlaySoundEffect("PlayerSkill3");
            }
            else if (!GameManager.MuteSound && Owner is Soldier && !Owner.mute)
                SplashKit.PlaySoundEffect("SoldierShoot");
            else if (!GameManager.MuteSound && Owner is Monster && !Owner.mute)
                SplashKit.PlaySoundEffect("MonsterShoot");
            else if (!GameManager.MuteSound && Owner is Tower && !Owner.mute)
                SplashKit.PlaySoundEffect("TowerShoot");

            float dame;

            if (TypeAttack == 0)
                dame = Owner.Dame;
            else if (TypeAttack == 1)
                dame = Owner.Dame * 1.2f;
            else if (TypeAttack == 2)
                dame = Owner.Dame * 1.5f;
            else
                dame = Owner.Dame * 2;

            float centerX, centerY;
            if (typeCharacter == 2)
            {
                centerX = _x + 13.5f;
                centerY = _y + 13.5f;
            }
            else
            {
                centerX = _x;
                centerY = _y;
            }

            float lenGunX = _gunTipXOrigin - centerX;
            float lenGunY = _gunTipYOrigin - centerY;
            float lenGun = (float)Math.Sqrt(lenGunX * lenGunX + lenGunY * lenGunY);

            float dx = _gunTipXOrigin - centerX;
            float dy = _gunTipYOrigin - centerY;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            if (length > 0.0f)
            {
                dx /= length;
                dy /= length;

                cooldowns[TypeAttack] = timeSkills[TypeAttack];
                Bullets.Add(new Bullet(Owner, BulletImage, _gunAngle, _gunTipXOrigin, _gunTipYOrigin, dx, dy, speedSkills[TypeAttack], rangeSkills[TypeAttack] - lenGun, dame));
            }
        }

        public void DrawImage(Bitmap img, float x, float y, float width, float height)
        {
            var options = SplashKit.OptionScaleBmp(width / img.Width, height / img.Height);
            SplashKit.DrawBitmap(img, x, y, options);
        }

        public void DrawRotation(Bitmap img, float x, float y, float angle, float offsetX, float offsetY)
        {
            DrawingOptions options = SplashKit.OptionRotateBmp(angle);
            options.AnchorOffsetX = offsetX;
            options.AnchorOffsetY = offsetY;
            SplashKit.DrawBitmap(img, x, y, options);
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

        protected int RandomInt(int x, int y)
        {
            return _rnd.Next(x, y + 1);
        }

        public float X
        {
            get { return _x; }
        }

        public float Y
        {
            get { return _y; }
        }

        public float CameraX
        {
            get { return _cameraX; }
        }

        public float CameraY
        {
            get { return _cameraY; }
        }

        public Bitmap Tank
        {
            get { return _tank; }
            set { _tank = value; }
        }

        public Bitmap Gun
        {
            get { return _gun; }
            set { _gun = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public float HP
        {
            get { return _hp; }
            set { _hp = value; }
        }

        public float MaxHP
        {
            get { return _maxHp; }
        }

        public int Side
        {
            get { return _side; }
        }

        public float Dame
        {
            get { return _dame; }
        }

        public float MoveSpeed
        {
            get { return _moveSpeed; }
        }

        public float Armor
        {
            get { return _armor; }
        }

        public float Piercing
        {
            get { return _piercing; }
        }
    }
}