using SplashKitSDK;
using System.Net.Sockets;
using System.Text;

namespace BangBang
{
    public class OnlineBattle : Battle
    {
        private float _enemyMouseX, _enemyMouseY, _enemyCameraX, _enemyCameraY;
        private ItemBattle[] _itemBattles;

        public OnlineBattle(DatabaseManager dbManager, User User, TcpClient Client, string EnemyUsm, string EnemyTank, string EnemyGun, int TypeBattle, int Side) : base(dbManager, User, Client, TypeBattle, Side)
        {
            _itemBattles = new ItemBattle[6];
            mainPlayer.Name = User.Name;
            var enemyUser = dbManager.GetUserByUsername(EnemyUsm);
            enemyPlayer.Name = enemyUser["CharacterName"].ToString();
            ((Player)enemyPlayer).TankOrigin = enemyPlayer.Tank = SplashKit.LoadBitmap(EnemyTank + "Battle", "Images/" + EnemyTank + "Battle.png");
            ((Player)enemyPlayer).GunOrigin = enemyPlayer.Gun = SplashKit.LoadBitmap(EnemyGun + "Battle", "Images/" + EnemyGun + "Battle.png");
            ((Player)enemyPlayer).PlayerMini = SplashKit.LoadBitmap(EnemyTank + "Mini", "Images/" + EnemyTank + "Mini.png");
            Thread listener = new Thread(ListenEnemyData);
            listener.Start();

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

        public override void Draw()
        {
            float cameraX = mainPlayer.CameraX, cameraY = mainPlayer.CameraY;

            _map.Draw(cameraX, cameraY);

            // Aid Kid
            foreach (var aidkid in AidKidBlue)
                aidkid.Draw(cameraX, cameraY);

            foreach (var aidkid in AidKidRed)
                aidkid.Draw(cameraX, cameraY);

            // Player
            if (_side == 1)
            {
                for (int i = 0; i < PlayerRed.Count; i++)
                {
                    if (IsVisible(PlayerRed[i], PlayerBlue, SoldierBlue, TowerBlue))
                        PlayerRed[i].Draw(_enemyMouseX + _enemyCameraX - cameraX, _enemyMouseY + _enemyCameraY - cameraY, cameraX, cameraY);

                    if (!CheckVision(PlayerRed[i], PlayerBlue, 300))
                        PlayerRed[i].mute = true;
                    else
                        PlayerRed[i].mute = false;
                }
            }
            else
            {
                for (int i = 0; i < PlayerBlue.Count; i++)
                {
                    if (IsVisible(PlayerBlue[i], PlayerRed, SoldierRed, TowerRed))
                        PlayerBlue[i].Draw(_enemyMouseX + _enemyCameraX - cameraX, _enemyMouseY + _enemyCameraY - cameraY, cameraX, cameraY);

                    if (!CheckVision(PlayerBlue[i], PlayerRed, 300))
                        PlayerBlue[i].mute = true;
                    else
                        PlayerBlue[i].mute = false;
                }
            }

            // Tower
            foreach (var tower in TowerBlue)
            {
                tower.Draw(0, 0, cameraX, cameraY);
                if (_side == 1)
                {
                    if (!CheckVision(tower, PlayerBlue, 300))
                        tower.mute = true;
                    else
                        tower.mute = false;

                    tower.DrawNoti();
                }
                else
                {
                    if (!CheckVision(tower, PlayerRed, 300))
                        tower.mute = true;
                    else
                        tower.mute = false;
                }

                tower.DrawExplosion(cameraX, cameraY);
            }

            foreach (var tower in TowerRed)
            {
                tower.Draw(0, 0, cameraX, cameraY);
                if (_side == 1)
                {
                    if (!CheckVision(tower, PlayerBlue, 300))
                        tower.mute = true;
                    else
                        tower.mute = false;
                }
                else
                {
                    if (!CheckVision(tower, PlayerRed, 300))
                        tower.mute = true;
                    else
                        tower.mute = false;

                    tower.DrawNoti();
                }

                tower.DrawExplosion(cameraX, cameraY);
            }

            // Soldier
            if (_side == 1)
            {
                foreach (var soldier in SoldierBlue)
                {
                    soldier.Draw(0, 0, cameraX, cameraY);

                    if (!CheckVision(soldier, PlayerBlue, 300))
                        soldier.mute = true;
                    else
                        soldier.mute = false;

                    if (_side == 1) soldier.DrawNoti();

                    soldier.DrawExplosion(cameraX, cameraY);
                }

                foreach (var soldier in SoldierRed)
                {
                    if (IsVisible(soldier, PlayerBlue, SoldierBlue, TowerBlue))
                        soldier.Draw(0, 0, cameraX, cameraY);

                    if (!CheckVision(soldier, PlayerBlue, 300))
                        soldier.mute = true;
                    else
                        soldier.mute = false;

                    soldier.DrawExplosion(cameraX, cameraY);
                }
            }
            else
            {
                foreach (var soldier in SoldierBlue)
                {
                    if (IsVisible(soldier, PlayerRed, SoldierRed, TowerRed))
                        soldier.Draw(0, 0, cameraX, cameraY);

                    if (!CheckVision(soldier, PlayerRed, 300))
                        soldier.mute = true;
                    else
                        soldier.mute = false;

                    soldier.DrawExplosion(cameraX, cameraY);
                }

                foreach (var soldier in SoldierRed)
                {
                    soldier.Draw(0, 0, cameraX, cameraY);

                    if (!CheckVision(soldier, PlayerRed, 300))
                        soldier.mute = true;
                    else
                        soldier.mute = false;

                    if (_side == 2) soldier.DrawNoti();

                    soldier.DrawExplosion(cameraX, cameraY);
                }
            }

            // Monster
            foreach (var monster in Monsters)
            {
                monster.Draw(0, 0, cameraX, cameraY);

                if (_side == 1)
                {
                    if (!CheckVision(monster, PlayerBlue, 300))
                        monster.mute = true;
                    else
                        monster.mute = false;
                }
                else
                {
                    if (!CheckVision(monster, PlayerRed, 300))
                        monster.mute = true;
                    else
                        monster.mute = false;
                }

                monster.DrawExplosion(cameraX, cameraY);
            }

            enemyPlayer.DrawExplosion(cameraX, cameraY);

            // Draw Main Player
            mainPlayer.Draw(SplashKit.MouseX(), SplashKit.MouseY(), cameraX, cameraY);
            mainPlayer.DrawNoti();
            mainPlayer.DrawExplosion(cameraX, cameraY);


            // Draw others
            ((Player)mainPlayer).DrawOthers(cameraX, cameraY);
            DrawHeader();
            _shop.Draw();
            _map.DrawMiniMap();
            DrawEndMatch();
        }

        public override void Handle()
        {
            //Others
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                HandleInformationPlayer();
                HandleStatistic();
                HandleEndMatch();
                HandleSetting();
            }

            _shop.Handle();

            //Handle Players
            ((Player)mainPlayer).Handle();

            if (_side == 1)
            {
                for (int i = 0; i < PlayerRed.Count; i++)
                    PlayerRed[i].Handle(0, 0);
            }
            else
            {
                for (int i = 0; i < PlayerBlue.Count; i++)
                    PlayerBlue[i].Handle(0, 0);
            }

            // Blue Team
            foreach (var tower in TowerBlue)
                tower.Handle(0, 0);

            foreach (var soldier in SoldierBlue)
                soldier.Handle(0, 0);


            // Red Team
            foreach (var tower in TowerRed)
                tower.Handle(0, 0);

            foreach (var soldier in SoldierRed)
                soldier.Handle(0, 0);


            // Monsters
            foreach (var monster in Monsters)
                monster.Handle(0, 0);
        }

        private void ListenEnemyData()
        {
            NetworkStream _stream = _client.GetStream();
            byte[] buffer = new byte[4096];
            StringBuilder messageBuilder = new StringBuilder();
            try
            {
                while (true)
                {
                    int len = _stream.Read(buffer, 0, buffer.Length);
                    if (len == 0) continue;

                    string chunk = Encoding.UTF8.GetString(buffer, 0, len);

                    messageBuilder.Append(chunk);

                    string allMsg = messageBuilder.ToString();
                    string[] messages = allMsg.Split('\n');

                    for (int i = 0; i < messages.Length - 1; i++)
                    {
                        ProcessMessage(messages[i]);
                    }

                    messageBuilder.Clear();
                    messageBuilder.Append(messages[^1]);
                }
            }
            catch
            {
                _matchEnd = true;
            }

        }

        private void ProcessMessage(string msg)
        {
            if (msg.StartsWith("Gun:"))
            {
                string[] parts = msg.Substring(4).Split(',');
                float x = float.Parse(parts[0]);
                float y = float.Parse(parts[1]);
                float cameraX = float.Parse(parts[2]);
                float cameraY = float.Parse(parts[3]);
                _enemyMouseX = x;
                _enemyMouseY = y;
                _enemyCameraX = cameraX;
                _enemyCameraY = cameraY;
            }
            else if (msg.StartsWith("Move:"))
            {
                string[] parts = msg.Substring(5).Split(',');
                float x = float.Parse(parts[0]);
                float y = float.Parse(parts[1]);
                
                ((Player)enemyPlayer).HandleMoving(x, y);
            }
            else if (msg.StartsWith("Skill"))
            {
                int typeSkill = int.Parse(msg[5].ToString());

                enemyPlayer.Attack(enemyPlayer, 1, typeSkill, ((Player)enemyPlayer).SkillImages[typeSkill]);
            }
            else if (msg.StartsWith("Recall"))
            {
                ((Player)enemyPlayer).HandleRecall();
            }
            else if (msg.StartsWith("Buy:"))
            {
                string idItem = msg.Substring(4);

                for (int i = 0; i < _itemBattles.Length; i++)
                    if (_itemBattles[i].ID == idItem)
                    {
                        ((Player)enemyPlayer).coin = Math.Max(0, ((Player)enemyPlayer).coin - _itemBattles[i].Price);
                        ((Player)enemyPlayer).Bag.Add(_itemBattles[i]);
                        ((Player)enemyPlayer).AddPower(_itemBattles[i]);
                        break;
                    }
            }
            else if (msg.StartsWith("Sell:"))
            {
                string idItem = msg.Substring(5);

                for (int i = 0; i < _itemBattles.Length; i++)
                    if (_itemBattles[i].ID == idItem)
                    {
                        ((Player)enemyPlayer).coin = Math.Max(0, ((Player)enemyPlayer).coin + _itemBattles[i].Price / 2);
                        ((Player)enemyPlayer).Bag.RemoveAll(item => item.ID == _itemBattles[i].ID);
                        ((Player)enemyPlayer).RemovePower(_itemBattles[i]);
                        break;
                    }
            }
            else if (msg == "opponent_disconnected")
            {
                Console.WriteLine("The opponent has exited!");
                result = 1;
                _matchEnd = true;
                if (!GameManager.MuteMusic)
                    SplashKit.StopMusic();

                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Winner");
            }
            else if (msg == "Surrender")
            {
                Console.WriteLine("The opponent surrendered!");
                result = 1;
                _matchEnd = true;
                if (!GameManager.MuteMusic)
                    SplashKit.StopMusic();

                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Winner");
            }
        }

        protected override void HandleSetting()
        {
            if (Hover(SplashKit.MousePosition(), 900 - 35 + 6, 35 / 2 - 25 / 2, 25, 25))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                _openSetting = !_openSetting;
            }

            if (!_openSetting) return;

            float x = 450, y = 300, lenX = 400, lenY = 270, lenXB = 120, lenYB = 45;

            if (Hover(SplashKit.MousePosition(), x + lenX / 2 - 28, y - lenY / 2 + 12, 20, 20))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                _openSetting = false;
            }
            else if (Hover(SplashKit.MousePosition(), x - lenX / 2 + 80, y - lenY / 2 + 70, 30, 30))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                GameManager.MuteMusic = !GameManager.MuteMusic;
                if (GameManager.MuteMusic)
                    SplashKit.StopMusic();
                else
                    SplashKit.PlayMusic("BackgroundBattle", -1);
            }
            else if (Hover(SplashKit.MousePosition(), x - lenX / 2 + 80, y - lenY / 2 + 130, 30, 30))
            {
                GameManager.MuteSound = !GameManager.MuteSound;
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
            }
            else if (Hover(SplashKit.MousePosition(), x - lenXB / 2, y + lenY / 2 - lenYB - 30, lenXB, lenYB))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                SendMessage("Surrender");
                _openSetting = false;
                result = 2;
                _matchEnd = true;
                if (!GameManager.MuteMusic)
                    SplashKit.StopMusic();

                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Defeat");
            }
        }

        protected override void HandleEndMatch()
        {
            if (!_matchEnd) return;

            int xB = 100, yB = 50, y = 250;
            if (Hover(SplashKit.MousePosition(), 450 - xB / 2, 300 - y / 2 + 180, xB, yB))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");

                if (result == 1)
                {
                    int totalCoin;
                    if (_typeBattle == 1) totalCoin = _user.Coin + 10;
                    else totalCoin = _user.Coin + 20;

                    _dbManager.UpdateUserField2(_user.Username, "Coin", totalCoin);
                    _user.Coin = totalCoin;
                }

                if (!GameManager.MuteMusic)
                {
                    SplashKit.StopMusic();
                    SplashKit.PlayMusic("Background", -1);
                }

                SendMessage("MatchEnd");
                Home.ConnectServer = false;
                Match.Start = false;
                GameManager.ScreenType = "Home";
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
            catch { }
        }

    }
}
