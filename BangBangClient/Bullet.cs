using SplashKitSDK;

namespace BangBang
{
    public class Bullet
    {
        private float _mapPixelWidth, _mapPixelHeight;
        private float _x, _y, _xInit, _yInit, _dx, _dy, _angle;
        private float _speed, _shotRange, _dame;
        private Character _owner;
        private Bitmap _image;

        public bool destroyed;

        public Bullet(Character owner, Bitmap image, float angle, float x, float y, float dx, float dy, float speed, float shotRange, float dame)
        {
            _mapPixelWidth = 2835;
            _mapPixelHeight = 1971;
            _image = image;
            _angle = angle;
            _x = _xInit = x;
            _y = _yInit = y;
            _dx = dx;
            _dy = dy;
            _speed = speed;
            _shotRange = shotRange;
            destroyed = false;
            _owner = owner;
            _dame = dame;
        }

        public bool OutRange(float x, float y, float range)
        {
            if (x < 0 || x > _mapPixelWidth || y < 0 || y > _mapPixelHeight)
                return true;

            float a = (x - _xInit);
            float b = (y - _yInit);
            float length = (float)Math.Sqrt(a * a + b * b);
            if (length > range) return true;
            return false;
        }

        public void Update(List<Character> Enemies1, List<Character> Enemies2, List<Character> Teammates)
        {
            X = X + DX * Speed;
            Y = Y + DY * Speed;

            UpdateCharacter(Enemies1, Teammates);
            UpdateCharacter(Enemies2, Teammates);
        }

        public void Update(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4, List<Character> Teammates)
        {
            X = X + DX * Speed;
            Y = Y + DY * Speed;

            UpdateCharacter(Enemies1, Teammates);
            UpdateCharacter(Enemies2, Teammates);
            UpdateCharacter(Enemies3, Teammates);
            UpdateCharacter(Enemies4, Teammates);
        }

        private void UpdateCharacter(List<Character> Enemies, List<Character> Teammates)
        {
            float offsetX = 0, offsetY = 0;

            if (Enemies.Count > 0 && (Enemies[0] is Tower && !(Enemies[0] is Monster)))
            {
                offsetX = 13.5f;
                offsetY = 13.5f;
            }

            foreach (var enemy in Enemies)
            {
                if(enemy.destroyed) continue;
                float length = Distance(X, Y, enemy.X + offsetX, enemy.Y + offsetY);

                if (length <= enemy.Tank.Width / 2)
                {
                    float finalDame = Math.Max(_dame - Math.Max(enemy.Armor - _owner.Piercing, 0), 0);
                    enemy.HP = Math.Max(enemy.HP - finalDame, 0);

                    if (enemy.HP == 0 && enemy.destroyed == false)
                    {
                        if (!GameManager.MuteSound && !enemy.mute)
                            SplashKit.PlaySoundEffect("Boom");

                        // (Player) Kill (Player, Tower)            => (Player, Tower) is Killed by (Player)
                        // (Soldier, Monster, Tower) Kill (Player)  => (Player) is Killed by (Soldier, Monster, Tower) 
                        // (Soldier) Kill (Tower)                   => (Tower) is Killed by (Soldier)
                        if ( ( (_owner is Player) && ((enemy is Player) || ((enemy is Tower) && !(enemy is Monster))) )
                             || (!(_owner is Player) && (enemy is Player)) 
                             || ((_owner is Soldier) && (enemy is Tower) && !(enemy is Monster)) )
                        {
                            UpdateNotiCharacter(_owner, 1, enemy.Name);
                            UpdateNotiCharacter(enemy, 2, _owner.Name);
                        }
                        
                        UpdateCoinPlayer(enemy, Teammates);
                        enemy.destroyed = true;
                        enemy.NumberOfDied += 1;
                        enemy.cooldowns[5] = enemy.timeSkills[5];
                        enemy.cooldowns[11] = enemy.timeSkills[11];
                        if((_owner is Player) && (enemy is Player))
                            _owner.NumberOfKills += 1;
                    }
                    enemy.cooldowns[6] = enemy.timeSkills[6];
                    enemy.dameTaken = finalDame;
                    enemy.slipY = 0.0f;
                    destroyed = true;
                    return;
                }
            }
        }

        private void UpdateCoinPlayer(Character Enemy, List<Character> Teammates)
        {
            if (Teammates.Count == 0 || (Teammates[0].Side == Enemy.Side)) return;

            int coin = 0;
            if (Enemy is Soldier) coin = 20;
            else if (Enemy is Monster) coin = 30;
            else if (Enemy is Player) coin = 50;
            else if (Enemy is Tower)
            {
                coin = 60;
                foreach (var teammate in Teammates)
                    if (teammate is Player)
                    {
                        ((Player)teammate).coin += coin;
                        ((Player)teammate).gainCoin = coin;
                        ((Player)teammate).slipYCoin = 0.0f;
                        ((Player)teammate).cooldowns[7] = ((Player)teammate).timeSkills[7];
                    }
                return;
            }

            foreach (var teammate in Teammates)
                if (teammate is Player)
                {
                    float d = Distance(Enemy.X, Enemy.Y, teammate.X, teammate.Y);
                    if (d <= Enemy.rangeSkills[0] + 50)
                    {
                        ((Player)teammate).coin += coin;
                        ((Player)teammate).gainCoin = coin;
                        ((Player)teammate).slipYCoin = 0.0f;
                        ((Player)teammate).cooldowns[7] = ((Player)teammate).timeSkills[7];
                    }

                }
                else
                    return;
        }

        private void UpdateNotiCharacter(Character character, int type, string EnemyUsm)
        {
            character.typeNoti = type;
            character.enemyUsername = EnemyUsm;
            character.cooldowns[8] = character.timeSkills[8];
            if (!GameManager.MuteSound)
                SplashKit.PlaySoundEffect("Notice");
        }

        public void Draw(float cameraX, float cameraY)
        {
            DrawRotation(_image, X - cameraX - (_image.Width - 4) / 2 , Y - cameraY - (_image.Height - 4) / 2, _angle, 0, 0);
        }

        public void DrawRotation(Bitmap img, float x, float y, float angle, float offsetX, float offsetY)
        {
            DrawingOptions options = SplashKit.OptionRotateBmp(angle);
            options.AnchorOffsetX = offsetX;
            options.AnchorOffsetY = offsetY;
            SplashKit.DrawBitmap(img, x, y, options);
        }

        private float Distance(float a, float b, float c, float d)
        {
            float x = c - a;
            float y = d - b;

            float length = (float)Math.Sqrt(x * x + y * y);
            return length;
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float DX
        {
            get { return _dx; }
            set { _dx = value; }
        }

        public float DY
        {
            get { return _dy; }
            set { _dy = value; }
        }

        public float Speed
        {
            get { return _speed; }
        }

        public float ShotRange
        {
            get { return _shotRange; }
        }
    }

}
