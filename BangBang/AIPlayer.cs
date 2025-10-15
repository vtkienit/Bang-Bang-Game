using SplashKitSDK;

namespace BangBang
{
    public class AIPlayer : Player
    {
        private bool[] _itemsOwned;
        private ItemBattle[] _itemBattles;

        protected float _gunAngleInit;
        protected Character? _currentTarget;
        protected List<Character> _playersInRange, _towersInRange, _soldiersInRange;

        public int level;

        public AIPlayer(User User, string Name, float X, float Y, int Side, bool Main, string Lane) : base(User, Name, X, Y, Side, Main, Lane)
        {
            _moveSpeed = _moveSpeedOrigin = 1.4f;
            _gunAngleInit = (float)Math.PI / 2;
            _currentTarget = null;
            _soldiersInRange = new List<Character>();
            _playersInRange = new List<Character>();
            _towersInRange = new List<Character>();
            _itemsOwned = new bool[6];
            _itemBattles = new ItemBattle[6];
            timeSkills[10] = 0.8f * 60;

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

        public override void Handle(float moveX, float moveY)
        {
            if (destroyed) return;
            if (_currentTarget != null && !_currentTarget.destroyed && (_playersInRange.Count > 0 || _soldiersInRange.Count > 0 || _towersInRange.Count > 0))
            {
                if (cooldowns[10] > 0.0f) return;

                if (level > 1)
                {
                    if (cooldowns[3] == 0.0f)
                        Attack(this, 1, 3, _skillImages[3]);
                    else if (cooldowns[2] == 0.0f)
                        Attack(this, 1, 2, _skillImages[2]);
                    else if (cooldowns[1] == 0.0f)
                        Attack(this, 1, 1, _skillImages[1]);
                    else if (cooldowns[0] == 0.0f)
                        Attack(this, 1, 0, _skillImages[0]);
                }
                else
                {
                    if (cooldowns[0] == 0.0f)
                        Attack(this, 1, 0, _skillImages[0]);
                }

                cooldowns[10] = timeSkills[10];
            }
            else
                HandleMovingControl();
        }

        protected override void HandleMovingControl()
        {
            if (Side == 1)
            {
                if (_lane == "mid")
                {
                    HandleMoving(1, 0);
                }
                else if (_lane == "top")
                {
                    if (_x >= 93 * _map.TileWidth)
                        HandleMoving(0, 1);
                    else
                        HandleMoving(1, 0);
                }
                else
                {
                    if (_x >= 93 * _map.TileWidth)
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
                    if (_x >= 9 * _map.TileWidth)
                        HandleMoving(0, 1);
                    else
                        HandleMoving(-1, 0);
                }
                else
                {
                    if (_x >= 9 * _map.TileWidth)
                        HandleMoving(0, -1);
                    else
                        HandleMoving(-1, 0);
                }
            }
        }

        public override void Update(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4, List<Character> Teammates)
        {
            base.Update(Enemies1, Enemies2, Enemies3, Enemies4, Teammates);
            if (level > 2) UpdateBuyItems();
            UpdateTarget(Enemies1, Enemies2, Enemies3, Enemies4);
        }

        protected virtual void UpdateTarget(List<Character> Enemies1, List<Character> Enemies2, List<Character> Enemies3, List<Character> Enemies4)
        {
            UpdateSingleTarget(Enemies3, _playersInRange);

            if (_currentTarget != null && !_currentTarget.destroyed && _currentTarget is Player)
            {
                _soldiersInRange.Clear();
                _towersInRange.Clear();
                return;
            }

            UpdateSingleTarget(Enemies2, _soldiersInRange);

            if (_currentTarget != null && !_currentTarget.destroyed && _currentTarget is Soldier)
            {
                _towersInRange.Clear();
                return;
            }

            UpdateSingleTarget(Enemies1, _towersInRange);
        }

        private void UpdateSingleTarget(List<Character> Enemies, List<Character> TargetsInRange)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy is Player && ((Player)enemy).InBush)
                {
                    _currentTarget = null;
                    _playersInRange.Clear();
                    return;
                }

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

        private bool IsInRange(Character Enemy)
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

        private int CheckOrderItem(ItemBattle item)
        {
            for (int i = 0; i < _itemBattles.Length; i++)
                if (_itemBattles[i].ID == item.ID) return i;
            return -1;
        }

        public void AddItemAndPower(ItemBattle item, int order)
        {
            if (order >= 0 && !_itemsOwned[order])
            {
                _itemsOwned[order] = true;
                Bag.Add(_itemBattles[order]);
                coin = Math.Max(0, coin - item.Price);
                AddPower(item);
            }
        }

        public void RemoveItemAndPower(ItemBattle item, int order)
        {
            if (order >= 0 && _itemsOwned[order])
            {
                _itemsOwned[order] = false;
                Bag.RemoveAll(it => it.ID == item.ID);
                coin += item.Price / 2;
                RemovePower(item);
            }
        }

        private void UpdateBuyItems()
        {
            for (int i = 0; i < _itemBattles.Length; i++)
                if (!_itemsOwned[i] && _itemBattles[i].Price <= coin)
                    AddItemAndPower(_itemBattles[i], i);
        }
    }
}
