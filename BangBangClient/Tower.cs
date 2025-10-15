using SplashKitSDK;

namespace BangBang
{
    public class Tower : Character
    {
        private int _typeTower;

        protected bool EnemyCloseToTower;
        protected float _gunAngleInit;
        protected Bitmap[] _skillImages;
        protected Character? _currentTarget;
        protected List<Character> _playersInRange, _AIsInRange;

        public Tower(User User, string Name, float X, float Y, int Side, int typeTower) : base(User, Name, X, Y, Side)
        {
            _skillImages = new Bitmap[1];
            _gunAngleInit = (float)Math.PI / 2;
            _typeTower = typeTower;
            _currentTarget = null;
            _playersInRange = new List<Character>(); 
            _AIsInRange = new List<Character>();

            if (Side == 1)
            {
                if (typeTower == 1)
                {
                    _tank = SplashKit.LoadBitmap("TowerTankBlue1", "Images/" + "TowerTankBlue1" + ".png");
                    _gun = SplashKit.LoadBitmap("TowerGunBlue1", "Images/" + "TowerGunBlue1" + ".png");
                }
                else
                {
                    _tank = SplashKit.LoadBitmap("TowerTankBlue2", "Images/" + "TowerTankBlue2" + ".png");
                    _gun = SplashKit.LoadBitmap("TowerGunBlue2", "Images/" + "TowerGunBlue2" + ".png");
                }
            }
            else
            {
                if (typeTower == 1)
                {
                    _tank = SplashKit.LoadBitmap("TowerTankRed1", "Images/" + "TowerTankRed1" + ".png");
                    _gun = SplashKit.LoadBitmap("TowerGunRed1", "Images/" + "TowerGunRed1" + ".png");
                }
                else
                {
                    _tank = SplashKit.LoadBitmap("TowerTankRed2", "Images/" + "TowerTankRed2" + ".png");
                    _gun = SplashKit.LoadBitmap("TowerGunRed2", "Images/" + "TowerGunRed2" + ".png");
                }
            }

            _skillImages[0] = SplashKit.LoadBitmap("BulletTower", "Images/BulletTower.png");
        }

        protected override void DrawGun(float targetX, float targetY, float CameraX, float CameraY, float offsetX, float offsetY)
        {
            float Xreal = _x - CameraX + 13.5f;
            float Yreal = _y - CameraY + 4.0f;

            float dx, dy;

            if (Side == 1)
            {
                dx = 1; dy = 0;
            }
            else
            {
                dx = -1; dy = 0;
            }

            if (_currentTarget != null && (_playersInRange.Count > 0 || _AIsInRange.Count > 0))
            {
                dx = _currentTarget.X - (_x + 13.5f);
                dy = _currentTarget.Y - (_y + 13.5f);

                float length = (float)Math.Sqrt(dx * dx + dy * dy);
                if (length > 0.0f)
                {
                    dx /= length;
                    dy /= length;
                }
            }

            float angleRad = (float)Math.Atan2(dy, dx) + _gunAngleInit;
            float angleDeg = angleRad * 180 / (float)Math.PI;
            _gunAngle = angleDeg;

            _gunTipX = Xreal + _gun.Height * 0.68f * (float)Math.Sin(angleRad);
            _gunTipY = Yreal - _gun.Height * 0.68f * (float)Math.Cos(angleRad) + 9.5f;

            _gunTipXOrigin = _x + 13.5f + _gun.Height * 0.68f * (float)Math.Sin(angleRad);
            _gunTipYOrigin = _y + 13.5f - _gun.Height * 0.68f * (float)Math.Cos(angleRad);

            DrawRotation(_gun, Xreal - _gun.Width / 2, Yreal - _gun.Height / 2, angleDeg, offsetX, offsetY);

            //SplashKit.FillCircle(Color.Black, Xreal, Yreal + 10, 3);
            //SplashKit.FillCircle(Color.Aqua, _gunTipX, _gunTipY, 3);
        }

        protected override void DrawTank(float CameraX, float CameraY)
        {
            if (_typeTower == 1)
            {
                SplashKit.DrawBitmap(_tank, _x - CameraX - 54, _y - CameraY - 54);
            }
            else
            {
                SplashKit.DrawBitmap(_tank, _x - CameraX - 27, _y - CameraY - 27);
            }
        }

        protected virtual void DrawOutline(float CameraX, float CameraY)
        {
            if (_currentTarget != null && _currentTarget is Soldier && _AIsInRange.Count > 0)
            {
                SplashKit.FillCircle(Color.RGBAColor(0, 250, 0, 20), _x - CameraX + 13.5f, _y - CameraY + 13.5f, rangeSkills[0]);
                SplashKit.DrawCircle(Color.RGBAColor(0, 250, 0, 120), _x - CameraX + 13.5f, _y - CameraY + 13.5f, rangeSkills[0]);
            }
            else if (_currentTarget != null && _playersInRange.Count > 0)
            {
                SplashKit.FillCircle(Color.RGBAColor(255, 0, 0, 20), _x - CameraX + 13.5f, _y - CameraY + 13.5f, rangeSkills[0]);
                SplashKit.DrawCircle(Color.RGBAColor(255, 0, 0, 120), _x - CameraX + 13.5f, _y - CameraY + 13.5f, rangeSkills[0]);
            }
            else if (EnemyCloseToTower)
            {
                SplashKit.FillCircle(Color.RGBAColor(255, 255, 0, 40), _x - CameraX + 13.5f, _y - CameraY + 13.5f, rangeSkills[0]);
                SplashKit.DrawCircle(Color.RGBAColor(255, 255, 0, 120), _x - CameraX + 13.5f, _y - CameraY + 13.5f, rangeSkills[0]);
            }
        }

        protected override void DrawHpBar(float cameraX, float cameraY)
        {
            if (_typeTower == 1)
            {
                float barWidth = 90 * _hp / _maxHp;
                float xOffset = 90 - barWidth - 2.00001f;
                DrawImage(_hpImage, _x - cameraX - 74 - xOffset / 2, _y - cameraY + _tank.Height / 2 - 2 + 20, barWidth, 8);
                DrawImage(_hpBorder, _x - cameraX - 74, _y - cameraY + _tank.Height / 2 - 2 + 20, 90, 8);
            }
            else
            {
                float barWidth = 50 * _hp / _maxHp;
                float xOffset = 50 - barWidth - 1;
                DrawImage(_hpImage, _x - cameraX - 87 + 13.5f - xOffset / 2, _y - cameraY + _tank.Height / 2 - 2 + 18.5f, barWidth, 8);
                DrawImage(_hpBorder, _x - cameraX - 87 + 13.5f, _y - cameraY + _tank.Height / 2 - 2 + 18.5f, 50, 8);
            }
        }

        protected virtual void DrawInMiniMap()
        {
            float w = 212, h = 148;
            float xinit = 900 - w, yinit = 600 - h;
            int col = (int)(_x / _map.TileWidth);
            int row = (int)(_y / _map.TileHeight);
            float miniX = xinit + col * _map.TileMiniWidth;
            float miniY = yinit + row * _map.TileMiniHeight;

            Color color;
            if (Side == 1) color = Color.Blue;
            else
                color = Color.Red;

            if (_typeTower == 1)
                SplashKit.FillRectangle(color, miniX - _map.TileMiniWidth * 2, miniY - _map.TileMiniHeight * 2, _map.TileMiniWidth * 5, _map.TileMiniHeight * 5);
            else
                SplashKit.FillRectangle(color, miniX - _map.TileMiniWidth, miniY - _map.TileMiniHeight, _map.TileMiniWidth * 3, _map.TileMiniHeight * 3);
        }

        public override void DrawDameTaken(float cameraX, float cameraY)
        {
            if (cooldowns[6] == 0.00f) return;
            float length = TextWidth(dameTaken.ToString(), "RussoOne", 17);
            SplashKit.DrawText(dameTaken.ToString(), Color.Red, "RussoOne", 17, _x + 13.5f - cameraX - length / 2, _y + 13.5f - cameraY - slipY);
            slipY += 0.7f;
        }

        public override void Draw(float targetX, float targetY, float CameraX, float CameraY)
        {
            if (destroyed) return;
            DrawOutline(CameraX, CameraY);
            DrawTank(CameraX, CameraY);
            DrawGun(targetX, targetY, CameraX, CameraY, 0, 10);
            DrawHpBar(CameraX, CameraY);
            DrawBullets(CameraX, CameraY);
            DrawInMiniMap();
            DrawDameTaken(CameraX, CameraY);
        }

        public override void Handle(float targetX, float targetY)
        {
            if (destroyed) return;
            if (_currentTarget != null && !_currentTarget.destroyed && (_playersInRange.Count > 0 || _AIsInRange.Count > 0))
                Attack(this, 2, 0,_skillImages[0]);
        }

        public override void Update(List<Character> Enemies1, List<Character> Enemies2, List<Character> Teammates)
        {
            base.Update(Enemies1, Enemies2, Teammates);
            UpdateTarget(Enemies1, Enemies2);
        }

        private void UpdateTarget(List<Character> Enemies1, List<Character> Enemies2)
        {
            CheckCloseToTower(Enemies2[0]);

            UpdateSingleTarget(Enemies1, _AIsInRange);

            if (_currentTarget != null && _currentTarget is Soldier)
            {
                _playersInRange.Clear();
                return;
            }

            UpdateSingleTarget(Enemies2, _playersInRange);
        }

        protected void UpdateSingleTarget(List<Character> Enemies, List<Character> TargetsInRange)
        {
            foreach (var enemy in Enemies)
            {
                if (!enemy.destroyed && IsInRange(enemy))
                {
                    if (!TargetsInRange.Contains(enemy))
                        TargetsInRange.Add(enemy);
                }
                else
                    TargetsInRange.Remove(enemy);

                if (_currentTarget == null || !TargetsInRange.Contains(_currentTarget))
                    _currentTarget = TargetsInRange.Count > 0 ? TargetsInRange[0] : null;
            }
        }

        protected virtual bool IsInRange(Character Enemy)
        {
            float dx = Enemy.X - (_x + 13.5f);
            float dy = Enemy.Y - (_y + 13.5f);
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance - Enemy.Tank.Width / 2 <= rangeSkills[0];
        }

        protected virtual void CheckCloseToTower(Character Enemy)
        {
            if (Enemy.destroyed)
            {
                EnemyCloseToTower = false;
                return;
            }

            float dx = Enemy.X - (_x + 13.5f);
            float dy = Enemy.Y - (_y + 13.5f);
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            EnemyCloseToTower =  distance - Enemy.Tank.Width / 2 - 25 <= rangeSkills[0] ? true : false;
        }

        public float TypeTower
        {
            get {  return _typeTower; }
        }
    }
}
