using SplashKitSDK;

namespace BangBang
{
    public class AidKid
    {
        private float _x, _y, _side, _lenX, _lenY, _healing;
        private Bitmap _image;
        private Character? _currentTarget;
        private float[] cooldowns;
        private List<Character> _playersInRange;

        public AidKid(float X, float Y, string Image, int Side)
        {
            _x = X * 27;
            _y = Y * 27;
            _side = Side;
            _healing = 20;
            cooldowns = new float[5];
            _playersInRange = new List<Character>();
            _image = SplashKit.LoadBitmap(Image, "Images/" + Image + ".png");

            if (_side == 1)
            {
                _lenX = _image.Width + 70;
                _lenY = _image.Height / 2 + 70;
            }
            else
            {
                _lenX = _image.Width / 2 + 70;
                _lenY = _image.Height / 2 + 70;
            }
        }

        private void DrawTank(float cameraX, float cameraY)
        {
            if (_side == 1)
                SplashKit.DrawBitmap(_image, _x - cameraX, _y - _image.Height / 2 - cameraY);
            else
                SplashKit.DrawBitmap(_image, _x - _image.Width / 2 - cameraX, _y - _image.Height / 2 - cameraY);  
        }

        private void DrawRange(float cameraX, float cameraY)
        {
            if (_currentTarget == null) return;
            if (_side == 1)
            {
                SplashKit.FillRectangle(Color.RGBAColor(0, 100, 0, 30), _x - cameraX, _y - _lenY - cameraY, _lenX, _lenY * 2);

                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), _x - cameraX, _y - _lenY - cameraY, 1, _lenY * 2);
                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), _x - cameraX + 1, _y - _lenY - cameraY, _lenX - 2, 1);
                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), _x + _lenX - 1 - cameraX, _y - _lenY - cameraY, 1, _lenY * 2);
                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), _x - cameraX + 1, _y + _lenY - 1 - cameraY, _lenX - 2, 1);
            }
            else
            {
                float halfW = _image.Width / 2;
                SplashKit.FillRectangle(Color.RGBAColor(0, 100, 0, 30), _x - _lenX - cameraX, _y - _lenY - cameraY, _lenX + halfW, _lenY * 2);

                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), _x + halfW - 1 - cameraX, _y - _lenY - cameraY, 1, _lenY * 2);
                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), _x - _lenX + 1 - cameraX, _y - _lenY - cameraY, _lenX + _image.Width / 2 - 2, 1);
                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), _x - _lenX - cameraX, _y - _lenY - cameraY, 1, _lenY * 2);
                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), _x - _lenX - cameraX + 1, _y + _lenY - 1 - cameraY, _lenX + _image.Width / 2 - 2, 1);
            }
        }

        public void Draw(float cameraX, float cameraY)
        {
            DrawRange(cameraX, cameraY);
            DrawTank(cameraX, cameraY);
            
        }

        public void Update(List<Character> Players)
        {
            UpdateTargets(Players);

            for (int i = 0; i < Players.Count; i++)
                UpdateCharacter(Players[i], i);

            UpdateCooldowns();
        }

        private void UpdateCharacter(Character Player, int order)
        {
            if (cooldowns[order] > 0.00f) return;
            if (_side == 1 && IsInRange(Player, _x, _x + _lenX, _y - _lenY, _y + _lenY))
            {
                float number = Math.Min(_healing, Math.Max(0, Player.MaxHP - Player.HP));
                if (number == 0) return;

                ((Player)Player).slipYHealing = 0.0f;
                ((Player)Player).healing = number;
                Player.HP += number;
                Player.cooldowns[9] = Player.timeSkills[9];
                cooldowns[order] = 60;
            }
            else if (_side == 2 && IsInRange(Player, _x - _lenX, _x + _image.Width / 2, _y - _lenY, _y + _lenY))
            {
                float number = Math.Min(_healing, Math.Max(0, Player.MaxHP - Player.HP));
                if (number == 0) return;

                ((Player)Player).slipYHealing = 0.0f;
                ((Player)Player).healing = number;
                Player.HP += number;
                Player.cooldowns[9] = Player.timeSkills[9];
                cooldowns[order] = 60;
            }
        }

        private void UpdateTargets(List<Character> Players)
        {
            UpdateSingleTarget(Players, _playersInRange);
        }

        protected void UpdateSingleTarget(List<Character> Players, List<Character> TargetsInRange)
        {
            foreach (var player in Players)
            {
                if (_side == 1 && !player.destroyed && IsInRange(player, _x, _x + _lenX, _y - _lenY, _y + _lenY))
                {
                    if (!TargetsInRange.Contains(player))
                        TargetsInRange.Add(player);
                }
                else if (_side == 2 && !player.destroyed && IsInRange(player, _x - _lenX, _x + _image.Width / 2, _y - _lenY, _y + _lenY))
                {
                    if (!TargetsInRange.Contains(player))
                        TargetsInRange.Add(player);
                }
                else
                    TargetsInRange.Remove(player);

                if (_currentTarget == null || !TargetsInRange.Contains(_currentTarget))
                    _currentTarget = TargetsInRange.Count > 0 ? TargetsInRange[0] : null;
            }
        }

        private void UpdateCooldowns()
        {
            for (int i = 0; i < cooldowns.Length; i++)
                cooldowns[i] = Math.Max(0, cooldowns[i] - 1);
        }

        private bool IsInRange(Character Enemy, float x1, float x2, float y1, float y2)
        {
            if (Enemy.X >= x1 && Enemy.X <= x2 && Enemy.Y >= y1 && Enemy.Y <= y2)
                return true;
            return false;
        }
    }
}
