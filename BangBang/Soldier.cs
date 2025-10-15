using SplashKitSDK;

namespace BangBang
{
    public class Soldier : Character
    {
        protected string _lane;
        protected float _gunAngleInit;

        protected Character? _currentTarget;
        protected List<Character> _playersInRange, _towersInRange, _soldiersInRange;
        protected Bitmap[] _skillImages;

        public Soldier(User User, string Name, float X, float Y, int Side, string lane) : base(User, Name, X, Y, Side)
        {
            _lane = lane;
            _moveSpeed = _moveSpeedOrigin = 1.5f;
            _gunAngleInit = (float)Math.PI / 2;
            _currentTarget = null;
            _soldiersInRange = new List<Character>();
            _playersInRange = new List<Character>();
            _towersInRange = new List<Character>();
            _skillImages = new Bitmap[1];
            _skillImages[0] = SplashKit.LoadBitmap("BulletSoldier", "Images/BulletSoldier.png");

            if (Side == 1)
            {
                _tank = SplashKit.LoadBitmap("TankSoldierBlue", "Images/" + "TankSoldierBlue" + ".png");
                _gun = SplashKit.LoadBitmap("GunSoldierBlue", "Images/" + "GunSoldierBlue" + ".png");
            }
            else
            {
                _tank = SplashKit.LoadBitmap("TankSoldierRed", "Images/" + "TankSoldierRed" + ".png");
                _gun = SplashKit.LoadBitmap("GunSoldierRed", "Images/" + "GunSoldierRed" + ".png");
            }

            SetupPosition();
        }

        protected virtual void SetupPosition()
        {
            if (Side == 1)
            {
                if (_lane == "mid")
                    _y = 36 * _map.TileHeight;
                else if(_lane == "top")
                    _y = 7 * _map.TileHeight;
                else
                    _y = 65 * _map.TileHeight;

                _x = 4 * _map.TileWidth;
            }
            else
            {
                if (_lane == "mid")
                    _y = 36 * _map.TileHeight;
                else if (_lane == "top")
                    _y = 7 * _map.TileHeight;
                else
                    _y = 65 * _map.TileHeight;

                _x = 99 * _map.TileWidth;
            }
        }

        protected override void DrawGun(float targetX, float targetY, float targetCameraX, float targetCameraY, float offsetX, float offsetY)
        {
            float Xreal = _x - targetCameraX;
            float Yreal = _y - targetCameraY - 10;

            float dx, dy;
            if (Side == 1)
            {
                dx = 1; dy = 0;
            }
            else
            {
                dx = -1; dy = 0;
            }

            if (_currentTarget != null && (_playersInRange.Count > 0 || _soldiersInRange.Count > 0 || _towersInRange.Count > 0))
            {
                float offset = 0;
                if (_currentTarget is Tower) offset = 13.5f;
                dx = _currentTarget.X + offset - _x;
                dy = _currentTarget.Y + offset - _y;

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

            //SplashKit.FillCircle(Color.Black, Xreal, Yreal + 10, 3);
            //SplashKit.FillCircle(Color.Aqua, _gunTipX, _gunTipY, 3);
        }

        protected override void DrawHpBar(float cameraX, float cameraY)
        {
            float barWidth = 46 * _hp / _maxHp;
            float xOffset = 46 - barWidth - 1;

            DrawImage(_hpImage, (_x - cameraX - 87) - xOffset / 2, _y - cameraY + _tank.Height / 2 + 2, barWidth, 6);
            DrawImage(_hpBorder, _x - cameraX - 87, _y - cameraY + _tank.Height / 2 + 2, 46, 6);
        }

        protected void DrawInMiniMap()
        {
            float w = 212, h = 148;
            float xinit = 900 - w, yinit = 600 - h;
            int col = (int)(_x / _map.TileWidth);
            int row = (int)(_y / _map.TileHeight);
            float miniX = xinit + col * _map.TileMiniWidth ;
            float miniY = yinit + row * _map.TileMiniHeight;
            
            if (Side == 1)
                SplashKit.FillCircle(Color.RGBAColor(0, 255, 255, 200), miniX, miniY, 2);
            else
                SplashKit.FillCircle(Color.RGBAColor(255, 0, 0, 200), miniX, miniY, 2);
        }

        public override void Draw(float targetX, float targetY, float targetCameraX, float targetCameraY)
        {
            if (destroyed) return;
            DrawHpBar(targetCameraX, targetCameraY);
            base.Draw(targetX, targetY, targetCameraX, targetCameraY);
            DrawGun(targetX, targetY, targetCameraX, targetCameraY, 0, 10);
            DrawInMiniMap();
            DrawDameTaken(targetCameraX, targetCameraY);
        }

        public override void Handle(float moveX, float moveY)
        {
            if (destroyed) return;
            if (_currentTarget != null && !_currentTarget.destroyed && (_playersInRange.Count > 0 || _soldiersInRange.Count > 0 || _towersInRange.Count > 0))
            {
                Attack(this, 1, 0, _skillImages[0]);
            }
            else
                HandleMovingControl();
        }

        protected void HandleMovingControl()
        {
            if (Side == 1)
            {
                if (_lane == "mid")
                {
                    HandleMoving(1, 0);
                }
                else if (_lane == "top")
                {
                    if (_x >= 95 * _map.TileWidth)
                        HandleMoving(0, 1);
                    else
                        HandleMoving(1, 0);
                }
                else
                {
                    if (_x >= 95 * _map.TileWidth)
                        HandleMoving(0, -1);
                    else
                        HandleMoving(1, 0);
                }
            }
            else
            {
                if (_lane == "mid")
                {
                    HandleMoving(-1, 0);
                }
                else if (_lane == "top")
                {
                    if (_x <= 9 * _map.TileWidth)
                        HandleMoving(0, 1);
                    else
                        HandleMoving(-1, 0);
                }
                else
                {
                    if (_x <= 9 * _map.TileWidth)
                        HandleMoving(0, -1);
                    else
                        HandleMoving(-1, 0);
                }
            }
        }

        protected virtual void HandleMoving(float moveX, float moveY)
        {
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
        }

        public override void Update(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4, List<Character> Teammates)
        {
            base.Update(Enemies1, Enemies2, Enemies3, Enemies4, Teammates);
            UpdateTarget(Enemies1, Enemies2, Enemies3, Enemies4);
        }

        protected virtual void UpdateTarget(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4)
        {
            UpdateSingleTarget(Enemies1, _towersInRange);

            if (_currentTarget != null && _currentTarget is Tower)
            {
                _playersInRange.Clear();
                _soldiersInRange.Clear();
                return;
            }

            UpdateSingleTarget(Enemies2, _soldiersInRange);

            if (_currentTarget != null && _currentTarget is Soldier)
            {
                _playersInRange.Clear();
                return;
            }

            UpdateSingleTarget(Enemies3, _playersInRange);
        }

        protected void UpdateSingleTarget(List<Character> Enemies, List<Character> TargetsInRange)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy is Player && ((Player)enemy).InBush) return;

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

        protected bool IsInRange(Character Enemy)
        {
            float dx, dy;
            if (Enemy is Tower)
            {
                dx = (Enemy.X + 13.5f) - _x;
                dy = (Enemy.Y + 13.5f) - _y;
            }
            else
            {
                dx = Enemy.X - _x;
                dy = Enemy.Y - _y;
            }

            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance - Enemy.Tank.Width / 2 <= rangeSkills[0];
        }
    }
}
