using SplashKitSDK;

namespace BangBang
{
    public class Monster : Tower
    {
        protected List<Character> _playerOthersInRange, _AIOthersInRange;

        public Monster(User User, string Name, float X, float Y, string Tank, string Gun, int Side, int typeTower) : base(User, Name, X, Y, Side, typeTower)
        {
            _playerOthersInRange = new List<Character>();
            _AIOthersInRange = new List<Character>();
            _tank = SplashKit.LoadBitmap(Tank, "Images/" + Tank + ".png");
            _gun = SplashKit.LoadBitmap(Gun, "Images/" + Gun + ".png");
            _hpImage = SplashKit.LoadBitmap("HP_Enemy", "Images/HP_Enemy.png");
            _skillImages[0] = SplashKit.LoadBitmap("BulletMonster", "Images/BulletMonster.png");
        }

        protected override void DrawGun(float targetX, float targetY, float CameraX, float CameraY, float offsetX, float offsetY)
        {
            float Xreal = _x - CameraX;
            float Yreal = _y - CameraY - 10;

            float dx, dy;

            if (Side == 1)
            {
                dx = 1; dy = 0;
            }
            else
            {
                dx = -1; dy = 0;
            }

            if (_currentTarget != null && (_AIsInRange.Count > 0 || _AIOthersInRange.Count > 0 || _playersInRange.Count > 0 || _playerOthersInRange.Count > 0))
            {
                dx = _currentTarget.X - _x;
                dy = _currentTarget.Y - _y;

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
            _gunTipY = Yreal - _gun.Height * 0.68f * (float)Math.Cos(angleRad) + 10;

            _gunTipXOrigin = _x + _gun.Height * 0.68f * (float)Math.Sin(angleRad);
            _gunTipYOrigin = _y - _gun.Height * 0.68f * (float)Math.Cos(angleRad);

            DrawRotation(_gun, Xreal - _gun.Width / 2, Yreal - _gun.Height / 2, angleDeg, offsetX, offsetY);

            //SplashKit.FillCircle(Color.Aqua, Xreal, Yreal + 10, 3);
            //SplashKit.FillCircle(Color.Aqua, _gunTipX, _gunTipY, 3);
        }

        protected override void DrawHpBar(float cameraX, float cameraY)
        {
            float barWidth = 60 * _hp / _maxHp;
            float xOffset = 60 - barWidth - 1;
            DrawImage(_hpImage, _x - cameraX - 87 - xOffset / 2, _y - cameraY + _tank.Height / 2 - 2 + 10, barWidth, 8);
            DrawImage(_hpBorder, _x - cameraX - 87, _y - cameraY + _tank.Height / 2 - 2 + 10, 60, 8);
        }

        protected override void DrawOutline(float targetCameraX, float targetCameraY)
        {
            if (_currentTarget != null && _currentTarget is Soldier && (_AIsInRange.Count > 0 || _AIOthersInRange.Count > 0))
            {
                SplashKit.FillCircle(Color.RGBAColor(0, 250, 0, 20), _x - targetCameraX, _y - targetCameraY, rangeSkills[0]);
                SplashKit.DrawCircle(Color.RGBAColor(0, 250, 0, 120), _x - targetCameraX, _y - targetCameraY, rangeSkills[0]);
            }
            else if (_currentTarget != null && (_playersInRange.Count > 0 || _playerOthersInRange.Count > 0))
            {
                SplashKit.FillCircle(Color.RGBAColor(255, 0, 0, 20), _x - targetCameraX, _y - targetCameraY, rangeSkills[0]);
                SplashKit.DrawCircle(Color.RGBAColor(255, 0, 0, 120), _x - targetCameraX, _y - targetCameraY, rangeSkills[0]);
            }
            else if (EnemyCloseToTower)
            {
                SplashKit.FillCircle(Color.RGBAColor(255, 255, 0, 40), _x - targetCameraX, _y - targetCameraY, rangeSkills[0]);
                SplashKit.DrawCircle(Color.RGBAColor(255, 255, 0, 120), _x - targetCameraX, _y - targetCameraY, rangeSkills[0]);
            }
        }

        protected override void DrawInMiniMap()
        {
            float w = 212, h = 148;
            float xinit = 900 - w, yinit = 600 - h;
            int col = (int)(_x / _map.TileWidth);
            int row = (int)(_y / _map.TileHeight);
            float miniX = xinit + col * _map.TileMiniWidth;
            float miniY = yinit + row * _map.TileMiniHeight;

            SplashKit.FillCircle(Color.RGBAColor(255, 167, 38, 200), miniX, miniY, 3);
        }

        public override void DrawDameTaken(float cameraX, float cameraY)
        {
            if (cooldowns[6] == 0.00f) return;
            float length = TextWidth(dameTaken.ToString(), "RussoOne", 17);
            SplashKit.DrawText(dameTaken.ToString(), Color.Red, "RussoOne", 17, _x - cameraX - length / 2, _y- cameraY - slipY);
            slipY += 0.7f;
        }

        public override void Handle(float targetCameraX, float targetCameraY)
        {
            if (destroyed) return;
            if (_currentTarget != null && !_currentTarget.destroyed && (_AIsInRange.Count > 0 || _AIOthersInRange.Count > 0 || _playersInRange.Count > 0 || _playerOthersInRange.Count > 0))
                Attack(this, 1, 0, _skillImages[0]);
        }

        public override void Update(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4, List<Character> Teammates)
        {
            base.Update(Enemies1, Enemies2, Enemies3, Enemies4, Teammates);
            UpdateTarget(Enemies1, Enemies2, Enemies3, Enemies4);
        }

        private void UpdateTarget(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4)
        {
            CheckCloseToTower(Enemies3[0]);

            if (_currentTarget != null && !_currentTarget.destroyed && IsInRange(_currentTarget)) return;

            UpdateSingleTarget(Enemies1, _AIsInRange);

            if (_currentTarget != null && _currentTarget is Soldier)
            {
                _AIOthersInRange.Clear();
                _playersInRange.Clear();
                _playerOthersInRange.Clear();
                return;
            }

            UpdateSingleTarget(Enemies2, _AIOthersInRange);

            if (_currentTarget != null && _currentTarget is Soldier)
            {
                _playersInRange.Clear();
                _playerOthersInRange.Clear();
                return;
            }

            UpdateSingleTarget(Enemies3, _playersInRange);

            if (_currentTarget != null)
            {
                _playerOthersInRange.Clear();
                return;
            }

            UpdateSingleTarget(Enemies4, _playerOthersInRange);
        }

        protected override bool IsInRange(Character Enemy)
        {
            float dx = Enemy.X - _x;
            float dy = Enemy.Y - _y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance - Enemy.Tank.Width / 2 <= rangeSkills[0];
        }

        protected override void CheckCloseToTower(Character Enemy)
        {
            if (Enemy.destroyed)
            {
                EnemyCloseToTower = false;
                return;
            }

            float dx = Enemy.X - _x;
            float dy = Enemy.Y - _y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            EnemyCloseToTower = distance - Enemy.Tank.Width / 2 - 25 <= rangeSkills[0] ? true : false;
        }
    }
}
