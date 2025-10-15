using SplashKitSDK;
using System.Net.Sockets;

namespace BangBang
{
    public class Battle
    {
        protected DatabaseManager _dbManager;
        protected bool _openInformPlayer, _openStatistic, _matchEnd, _openSetting;
        protected int _side, cooldownRevive, result, _typeBattle;
        protected Character mainPlayer, enemyPlayer;
        protected List<Character> PlayerBlue, PlayerRed, TowerBlue, TowerRed;
        protected List<Character> SoldierBlue, SoldierRed, Monsters;
        protected List<AidKid> AidKidBlue, AidKidRed; 
        protected ShopBattle _shop;
        protected Map _map;
        protected TcpClient _client;
        protected User _user;

        public Clock Time;

        public Battle(DatabaseManager dbManager, User User, TcpClient Client, int TypeBattle, int Side)
        {
            // Initial
            _map = new Map("map.txt");
            PlayerBlue = new List<Character>();
            PlayerRed = new List<Character>();
            TowerBlue = new List<Character>();
            TowerRed = new List<Character>();
            SoldierBlue = new List<Character>();
            SoldierRed = new List<Character>();
            Monsters = new List<Character>();
            AidKidBlue = new List<AidKid>();
            AidKidRed = new List<AidKid>();
            _user = User;
            _side = Side;
            _client = Client;
            _dbManager = dbManager;
            _typeBattle = TypeBattle;

            // Load buttons
            SplashKit.LoadBitmap("CloseBattle", "Images/CloseBattle.png");
            SplashKit.LoadBitmap("CloseBattleHover", "Images/CloseBattleHover.png");
            SplashKit.LoadBitmap("Close", "Images/Close.png");
            SplashKit.LoadBitmap("CloseHover", "Images/CloseHover.png");
            SplashKit.LoadBitmap("TowerNumber", "Images/TowerNumber.png");
            SplashKit.LoadBitmap("BladeCount", "Images/BladeCount.png");
            SplashKit.LoadBitmap("DeadCount", "Images/DeadCount.png");
            SplashKit.LoadBitmap("Para", "Images/Para.png");
            SplashKit.LoadBitmap("ParaHover", "Images/ParaHover.png");
            SplashKit.LoadBitmap("Setting", "Images/Setting.png");
            SplashKit.LoadBitmap("SettingHover", "Images/SettingHover.png");
            SplashKit.LoadBitmap("Statistic", "Images/Statistic.png");
            SplashKit.LoadBitmap("StatisticHover", "Images/StatisticHover.png");

            if (TypeBattle == 1)
            {
                if (Side == 1)
                {
                    PlayerBlue.Add(new Player(User, "Kien", 50, 35, 1, true, "mid"));
                    PlayerRed.Add(new AIPlayer(User, "Enemy", 52, 30, 2, false, "mid"));
                }
                else
                {
                    PlayerBlue.Add(new AIPlayer(User, "Enemy", 50, 35, 1, false, "mid"));
                    PlayerRed.Add(new Player(User, "Kien", 52, 30, 2, true, "mid"));
                }
            }
            else
            {
                if (Side == 1)
                {
                    PlayerBlue.Add(new OnlinePlayer(User, Client, "Kien", 50, 35, 1, true, "mid"));
                    PlayerRed.Add(new Player(User, "Enemy", 52, 30, 2, false, "mid"));
                }
                else
                {
                    PlayerBlue.Add(new Player(User, "Enemy", 50, 35, 1, false, "mid"));
                    PlayerRed.Add(new OnlinePlayer(User, Client, "Enemy", 52, 30, 2, true, "mid"));
                }
            }


            // Blue Team
            // Aid Kid
            AidKidBlue.Add(new AidKid(1, 22, "AidKidBlue", 1));
            AidKidBlue.Add(new AidKid(1, 51, "AidKidBlue", 1));

            // Tower
            TowerBlue.Add(new Tower(User, "Tower", 9, 36, 1, 1));
            TowerBlue.Add(new Tower(User, "Tower", 40, 36, 1, 2)); // Mid
            TowerBlue.Add(new Tower(User,"Tower", 25, 36, 1, 2)); // Mid
            TowerBlue.Add(new Tower(User, "Tower", 40, 7, 1, 2)); // Top
            TowerBlue.Add(new Tower(User, "Tower", 25, 7, 1, 2)); // Top
            TowerBlue.Add(new Tower(User, "Tower", 40, 65, 1, 2)); // Bot
            TowerBlue.Add(new Tower(User, "Tower", 25, 65, 1, 2)); // Bot


            // Red Team
            // Aid Kid
            AidKidRed.Add(new AidKid(103, 22, "AidKidRed", 2));
            AidKidRed.Add(new AidKid(103, 51, "AidKidRed", 2));

            TowerRed.Add(new Tower(User, "Tower", 95, 36, 2, 1));
            TowerRed.Add(new Tower(User, "Tower", 79, 36, 2, 2)); // Mid
            TowerRed.Add(new Tower(User, "Tower", 64, 36, 2, 2)); // Mid
            TowerRed.Add(new Tower(User, "Tower", 79, 7, 2, 2)); // Top
            TowerRed.Add(new Tower(User, "Tower", 64, 7, 2, 2)); // Top
            TowerRed.Add(new Tower(User, "Tower", 79, 65, 2, 2)); // Bot
            TowerRed.Add(new Tower(User, "Tower", 64, 65, 2, 2)); // Bot


            // Monsters
            Monsters.Add(new Monster(User, "Monster", 33, 22, "TankMonster", "GunGreenMonster", 2, 1));
            Monsters.Add(new Monster(User, "Monster", 33, 51, "TankMonster", "GunBlueMonster", 2, 1));

            Monsters.Add(new Monster(User, "Monster", 72, 22, "TankMonster", "GunGreenMonster", 1, 1));
            Monsters.Add(new Monster(User, "Monster", 72, 51, "TankMonster", "GunBlueMonster", 1, 1));


            // Setup Skills for Blue Team
            for (int i = 0; i < PlayerBlue.Count; i++)
                PlayerBlue[i].SetUpSkill(150, 10, 10, 10, 0.7f, 2, 4, 8, 150, 180, 200, 230, 10);

            TowerBlue[0].SetUpSkill(400, 20, 18, 18, 1, 230, 10);
            for (int i = 1; i < TowerBlue.Count; i++)
                TowerBlue[i].SetUpSkill(300, 14, 14, 14, 1, 175, 10);

            for (int i = 0; i < SoldierBlue.Count; i++)
                SoldierBlue[i].SetUpSkill(200, 8, 8, 9, 1, 140, 10);


            // Setup Skills for Red Team
            for (int i = 0; i < PlayerRed.Count; i++)
                PlayerRed[i].SetUpSkill(150, 10, 10, 10, 0.7f, 2, 4, 8, 150, 180, 200, 230, 10);

            TowerRed[0].SetUpSkill(400, 20, 18, 18, 1, 230, 10);
            for (int i = 1; i < TowerRed.Count; i++)
                TowerRed[i].SetUpSkill(300, 14, 14, 14, 1, 175, 10);

            for (int i = 0; i < SoldierRed.Count; i++)
                SoldierRed[i].SetUpSkill(200, 8, 8, 9, 1, 140, 10);


            // Set up skills of Monsters
            for (int i = 0; i < Monsters.Count; i++)
                Monsters[i].SetUpSkill(100, 9, 9, 9, 1, 180, 10);

            //Others
            Time = new Clock();
            if (Side == 1)
            {
                mainPlayer = PlayerBlue[0];
                enemyPlayer = PlayerRed[0];
            }
            else
            {
                mainPlayer = PlayerRed[0];
                enemyPlayer = PlayerBlue[0];
            }

            _shop = new ShopBattle((Player)mainPlayer, _client, TypeBattle);

            SplashKit.StopMusic();
            if (!GameManager.MuteMusic)
                SplashKit.PlayMusic("BackgroundBattle", -1);
        }

        protected void DrawHeader()
        {
            int x = 450, y = 0;
            DrawTrapezium(Color.RGBAColor(0, 207, 230, 110), x - 60, y, 60, x - 30, y + 40, 30);
            DrawTrapezium(Color.RGBAColor(255, 60, 60, 110), x + 1, y, 60, x + 1, y + 40, 30);
            Time.Draw(13, 450, 1);
            DrawScore();
            DrawTrapezium(Color.RGBAColor(0, 207, 230, 30), x - 90, y, 30, x - 90, y + 40, 60);
            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 100), x - 90, y, 1, 40);

            DrawTrapezium(Color.RGBAColor(255, 60, 60, 30), x + 1 + 60, y, 30, x + 1 + 30, y + 40, 60);
            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 100), x + 1 + 89, y, 1, 40);

            SplashKit.DrawBitmap("TowerNumber", x - 65, y + 20);
            SplashKit.DrawBitmap("TowerNumber", x + 50, y + 20);

            DrawInformationPlayer();

            DrawStatistic();

            DrawSetting();
        }

        protected void DrawInformationPlayer()
        {
            SplashKit.FillRectangle(Color.RGBAColor(0, 184, 255, 150), 0, 0, 150, 35);
            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 180), 4 + 4 + 25, 32, 142 - 25, 3);
            SplashKit.FillRectangle(Color.RGBAColor(0, 184, 255, 150), 0, 0, 33, 35);

            if (Hover(SplashKit.MousePosition(), 4, 35 / 2 - 25 / 2, 25, 25))
                SplashKit.DrawBitmap("ParaHover", 4, 35 / 2 - 25 / 2);
            else
                SplashKit.DrawBitmap("Para", 4, 35 / 2 - 25 / 2);

            SplashKit.DrawBitmap("BladeCount", 45, 10);
            SplashKit.DrawText(mainPlayer.NumberOfKills.ToString(), Color.White, "RussoOne", 14, 65, 10);

            SplashKit.DrawBitmap("DeadCount", 100, 10);
            SplashKit.DrawText(mainPlayer.NumberOfDied.ToString(), Color.White, "RussoOne", 14, 120, 10);

            // Detail Information
            if (!_openInformPlayer) return;

            float y = 340;
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), 900 / 2 - 400 / 2, 600 / 2 - y / 2, 400, y);
            SplashKit.FillRectangle(Color.RGBAColor(0, 184, 255, 200), 900 / 2 - 400 / 2, 600 / 2 - y / 2, 5, y);

            float lenText = TextWidth("Information", "RussoOne", 26);
            SplashKit.DrawText("Information", Color.RGBAColor(0, 184, 255, 255), "RussoOne", 26, 900 / 2 - lenText / 2, 600 / 2 - y / 2 + 5);

            if (Hover(SplashKit.MousePosition(), 900 / 2 + 400 / 2 - 30, 600 / 2 - (int)y / 2 + 9, 25, 25))
                SplashKit.DrawBitmap("CloseBattleHover", 900 / 2 + 400 / 2 - 30, 600 / 2 - y / 2 + 9);
            else
                SplashKit.DrawBitmap("CloseBattle", 900 / 2 + 400 / 2 - 30, 600 / 2 - y / 2 + 9);

            SplashKit.DrawText("HP:", Color.White, "RussoOne", 18, 900 / 2 - 400 / 2 + 15, 600 / 2 - y / 2 + 60);
            lenText = TextWidth("HP:", "RussoOne", 18);
            SplashKit.DrawText(mainPlayer.HP.ToString() + "/" + mainPlayer.MaxHP.ToString(), Color.LightGray, "RussoOne", 18, 900 / 2 - 400 / 2 + 15 + lenText + 10, 600 / 2 - y / 2 + 60);

            SplashKit.DrawText("Dame:", Color.White, "RussoOne", 18, 900 / 2 - 400 / 2 + 15, 600 / 2 - y / 2 + 100);
            lenText = TextWidth("Dame:", "RussoOne", 18);
            SplashKit.DrawText(mainPlayer.Dame.ToString(), Color.LightGray, "RussoOne", 18, 900 / 2 - 400 / 2 + 15 + lenText + 10, 600 / 2 - y / 2 + 100);

            SplashKit.DrawText("Attack Speed:", Color.White, "RussoOne", 18, 900 / 2 - 400 / 2 + 15, 600 / 2 - y / 2 + 140);
            lenText = TextWidth("Attack Speed:", "RussoOne", 18);
            SplashKit.DrawText(Math.Round((mainPlayer.timeSkills[0] / 60.0f), 3).ToString() + "s per hit", Color.LightGray, "RussoOne", 18, 900 / 2 - 400 / 2 + 15 + lenText + 10, 600 / 2 - y / 2 + 140);

            SplashKit.DrawText("Move Speed:", Color.White, "RussoOne", 18, 900 / 2 - 400 / 2 + 15, 600 / 2 - y / 2 + 180);
            lenText = TextWidth("Move Speed:", "RussoOne", 18);
            SplashKit.DrawText(mainPlayer.MoveSpeed.ToString() + " pixel per frame", Color.LightGray, "RussoOne", 18, 900 / 2 - 400 / 2 + 15 + lenText + 10, 600 / 2 - y / 2 + 180);

            SplashKit.DrawText("Armor:", Color.White, "RussoOne", 18, 900 / 2 - 400 / 2 + 15, 600 / 2 - y / 2 + 220);
            lenText = TextWidth("Armor:", "RussoOne", 18);
            SplashKit.DrawText(mainPlayer.Armor.ToString(), Color.LightGray, "RussoOne", 18, 900 / 2 - 400 / 2 + 15 + lenText + 10, 600 / 2 - y / 2 + 220);

            SplashKit.DrawText("Armor Piercing:", Color.White, "RussoOne", 18, 900 / 2 - 400 / 2 + 15, 600 / 2 - y / 2 + 260);
            lenText = TextWidth("Armor Piercing:", "RussoOne", 18);
            SplashKit.DrawText(mainPlayer.Piercing.ToString(), Color.LightGray, "RussoOne", 18, 900 / 2 - 400 / 2 + 15 + lenText + 10, 600 / 2 - y / 2 + 260);
        }

        protected void DrawScore()
        {
            int countPLayerBlueDied = 0, countPLayerRedDied = 0;
            foreach (var x in PlayerBlue)
                countPLayerBlueDied += x.NumberOfDied;

            foreach (var x in PlayerRed)
                countPLayerRedDied += x.NumberOfDied;

            SplashKit.DrawText(countPLayerBlueDied.ToString(), Color.Red, "RussoOne", 19, 451 + 10, 19);
            float t1W = TextWidth(countPLayerBlueDied.ToString(), "RussoOne", 19);
            SplashKit.DrawText(countPLayerRedDied.ToString(), Color.Blue, "RussoOne", 19, 450 - 8 - t1W, 19);

            
            int countTowerBlueDied = 0, countTowerRedDied = 0;
            foreach (var tower in TowerBlue)
                if (tower.destroyed) countTowerBlueDied += 1;

            foreach (var tower in TowerRed)
                if (tower.destroyed) countTowerRedDied += 1;

            SplashKit.DrawText(countTowerBlueDied.ToString(), Color.LightGray, "Arial", 18, 516, 20);
            float t2W = TextWidth(countTowerRedDied.ToString(), "Arial", 18);
            SplashKit.DrawText(countTowerRedDied.ToString(), Color.LightGray, "Arial", 18, 392 - 8 - t2W, 20);
        }

        private void DrawSetting()
        {
            SplashKit.FillRectangle(Color.RGBAColor(0, 184, 255, 200), 900 - 35, 0, 35, 35);

            if (Hover(SplashKit.MousePosition(), 900 - 35 + 6, 35 / 2 - 25 / 2, 25, 25))
                SplashKit.DrawBitmap("SettingHover", 900 - 35 + 6, 35 / 2 - 25 / 2);
            else
                SplashKit.DrawBitmap("Setting", 900 - 35 + 6, 35 / 2 - 25 / 2);

            if (!_openSetting) return;

            float x = 450, y = 300, lenX = 400, lenY = 270;

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 210), x - lenX / 2, y - lenY / 2, lenX, lenY);

            float lenT = TextWidth("Setting", "RussoOne", 30);
            SplashKit.DrawText("Setting", Color.White, "RussoOne", 30, x - lenT / 2, y - lenY / 2 + 8);

            if (Hover(SplashKit.MousePosition(), x + lenX / 2 - 28, y - lenY / 2 + 12, 20, 20))
                SplashKit.DrawBitmap("CloseHover", x + lenX / 2 - 28, y - lenY / 2 + 12);
            else 
                SplashKit.DrawBitmap("Close", x + lenX / 2 - 28, y - lenY / 2 + 12);

            if (GameManager.MuteMusic)
                SplashKit.DrawBitmap("MusicHover", x - lenX / 2 + 80, y - lenY / 2 + 70);
            else
                SplashKit.DrawBitmap("Music", x - lenX / 2 + 80, y - lenY / 2 + 70);

            SplashKit.DrawText("Background Music", Color.LightGray, "RussoOne", 20, x - lenX / 2 + 120, y - lenY / 2 + 73);

            if (GameManager.MuteSound)
                SplashKit.DrawBitmap("MusicHover", x - lenX / 2 + 80, y - lenY / 2 + 130);
            else
                SplashKit.DrawBitmap("Music", x - lenX / 2 + 80, y - lenY / 2 + 130);

            SplashKit.DrawText("Sound Effect", Color.LightGray, "RussoOne", 20, x - lenX / 2 + 120, y - lenY / 2 + 133);

            float lenXB = 120, lenYB = 45;
            if (Hover(SplashKit.MousePosition(), x - lenXB / 2, y + lenY / 2 - lenYB - 30, lenXB, lenYB))
                SplashKit.FillRectangle(Color.RGBAColor(180, 0, 50, 200), x - lenXB / 2, y + lenY / 2 - lenYB - 30, lenXB, lenYB);
            else
                SplashKit.FillRectangle(Color.RGBAColor(255, 0, 50, 200), x - lenXB / 2, y + lenY / 2 - lenYB - 30, lenXB, lenYB);

            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 255), x - lenXB / 2, y + lenY / 2 - 3 - 30, lenXB, 3);

            lenT = TextWidth("Surrender", "RussoOne", 20);
            SplashKit.DrawText("Surrender", Color.White, "RussoOne", 20, x - lenT / 2, y + lenY / 2 - lenYB - 30 + 10);
        }

        private void DrawStatistic()
        {
            SplashKit.FillRectangle(Color.RGBAColor(0, 184, 255, 130), 900 - 70, 0, 35, 35);

            if (Hover(SplashKit.MousePosition(), 900 - 70 + 5, 35 / 2 - 25 / 2, 25, 25))
                SplashKit.DrawBitmap("StatisticHover", 900 - 70 + 5, 35 / 2 - 25 / 2);
            else
                SplashKit.DrawBitmap("Statistic", 900 - 70 + 5, 35 / 2 - 25 / 2);

            if (!_openStatistic) return;

            int x = 810, y = 240; 
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 200), 450 - x / 2, 300 - y / 2, x, y);

            float lenT = TextWidth("Statistic", "RussoOne", 25);
            SplashKit.DrawText("Statistic", Color.White, "RussoOne", 25, 450 - lenT / 2, 300 - y / 2 + 10);

            if (Hover(SplashKit.MousePosition(), 450 + x / 2 - 30, 300 - y / 2 + 15, 20, 20))
                SplashKit.DrawBitmap("CloseHover", 450 + x / 2 - 30, 300 - y / 2 + 15);
            else
                SplashKit.DrawBitmap("Close", 450 + x / 2 - 30, 300 - y / 2 + 15);

            SplashKit.FillRectangle(Color.RGBAColor(70, 106, 179, 200), 450 - x / 2, 300 - y / 2 + 50, x / 2, 45);
            SplashKit.FillRectangle(Color.RGBAColor(38, 44, 84, 150), 450 - x / 2, 300 - y / 2 + 50, x / 2, y - 50);

            SplashKit.FillRectangle(Color.RGBAColor(168, 61, 85, 200), 450, 300 - y / 2 + 50, x / 2, 45);
            SplashKit.FillRectangle(Color.RGBAColor(70, 27, 40, 150), 450, 300 - y / 2 + 50, x / 2, y - 50);

            if(mainPlayer.Side == 1)
            {
                DrawPlayerStatistic(mainPlayer, 450 - x / 2, 300 - y / 2 + 50);
                DrawPlayerStatistic(enemyPlayer, 450, 300 - y / 2 + 50);
            }
            else
            {
                DrawPlayerStatistic(enemyPlayer, 450 - x / 2, 300 - y / 2 + 50);
                DrawPlayerStatistic(mainPlayer, 450, 300 - y / 2 + 50);
            }
        }

        private void DrawPlayerStatistic(Character Player, int x, int y)
        {
            if (Player.Side == 1)
            {
                SplashKit.DrawText(Player.Name + ":", Color.Blue, "RussoOne", 22, x + 10, y + 60);
            }
            else
            {
                SplashKit.DrawText(Player.Name + ":", Color.Red, "RussoOne", 22, x + 10, y + 60);
            }

            List<ItemBattle> items = ((Player)Player).Bag;
            for (int i = 0; i < 6; i++)
            {
                int space = 65, spaceH = 90, initX = 10;
                SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 100), x + initX + i * space, y + spaceH, 62, 62);

                SplashKit.FillRectangle(Color.RGBAColor(100, 100, 100, 200), x + initX + i * space, y + spaceH, 62, 2);
                SplashKit.FillRectangle(Color.RGBAColor(100, 100, 100, 200), x + initX + 60 + i * space, y + spaceH + 2, 2, 60);
                SplashKit.FillRectangle(Color.RGBAColor(100, 100, 100, 200), x + initX + i * space, y + spaceH + 60, 60, 2);
                SplashKit.FillRectangle(Color.RGBAColor(100, 100, 100, 200), x + initX + i * space, y + spaceH + 2, 2, 58);

                if (i < items.Count)
                    SplashKit.DrawBitmap(items[i].Image, x + initX + 1 + i * space, y + spaceH + 1);
            }
        }

        private void DrawTrapezium(Color clr, float x, float y, float len1, float k, float l, float len2)
        {
            Triangle t1 = new Triangle
            {
                Points = new Point2D[]
                {
                    SplashKit.PointAt(x, y),
                    SplashKit.PointAt(k, l),
                    SplashKit.PointAt(k + len2 - 1, l)
                }
            };
            SplashKit.FillTriangle(clr, t1);
            Triangle t2 = new Triangle
            {
                Points = new Point2D[]
                {
                    SplashKit.PointAt(x + 1, y),
                    SplashKit.PointAt(x + len1, y),
                    SplashKit.PointAt(k + len2, l)
                }
            };
            SplashKit.FillTriangle(clr, t2);
        }
        
        protected void DrawEndMatch()
        {
            if (!_matchEnd) return;

            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 180), 0, 0, 900, 600);

            int x = 550, y = 250, coin;

            Color clr1, clr2, clr3;
            string text;
            if (result == 1)
            {
                clr1 = Color.RGBAColor(124, 98, 59, 180);
                clr2 = Color.Gold;
                clr3 = Color.LightGoldenrodYellow;
                text = "Victory";
                coin = 20;
            }
            else
            {
                clr1 = Color.RGBAColor(44, 38, 94, 200);
                clr2 = Color.LightGray;
                clr3 = Color.Gray;
                text = "Defeat";
                coin = 0;
            }

            SplashKit.FillRectangle(clr1, 450 - x / 2, 300 - y / 2, x, y);

            float lenT = TextWidth(text, "RussoOne", 80);
            SplashKit.DrawText(text, clr2, "RussoOne", 80, 450 - lenT / 2, 300 - y / 2 + 10);

            lenT = TextWidth($"You gain {coin} coins", "RussoOne", 20);
            SplashKit.DrawText($"You gain {coin} coins", clr3, "RussoOne", 20, 450 - lenT / 2, 300 - y / 2 + 120);

            int xB = 100, yB = 50;
            if (Hover(SplashKit.MousePosition(), 450 - xB / 2, 300 - y / 2 + 180, xB, yB))
                SplashKit.FillRectangle(Color.RGBAColor(0, 150, 0, 200), 450 - xB / 2, 300 - y / 2 + 180, xB, yB);
            else
                SplashKit.FillRectangle(Color.RGBAColor(0, 200, 0, 200), 450 - xB / 2, 300 - y / 2 + 180, xB, yB);

            SplashKit.FillRectangle(Color.RGBAColor(255, 255, 255, 210), 450 - xB / 2, 300 - y / 2 + 180 + yB - 3, xB, 3);
            lenT = TextWidth("Home", "RussoOne", 22);
            SplashKit.DrawText("Home", Color.White, "RussoOne", 22, 450 - lenT / 2, 300 - y / 2 + 180 + 10);
        }

        public virtual void Draw()
        {
        }

        public virtual void Handle()
        {
        }

        protected void HandleInformationPlayer()
        {
            if (Hover(SplashKit.MousePosition(), 4, 35 / 2 - 25 / 2, 25, 25))
            {
                mainPlayer.mute = true;
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");

                _openInformPlayer = true;
            }
                
            float y = 340;
            if (Hover(SplashKit.MousePosition(), 900 / 2 + 400 / 2 - 30, 600 / 2 - (int)y / 2 + 9, 25, 25))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");

                mainPlayer.mute = false;
                _openInformPlayer = false;
            } 
        }

        protected virtual void HandleSetting()
        {
            if (Hover(SplashKit.MousePosition(), 900 - 35 + 6, 35 / 2 - 25 / 2, 25, 25))
            {
                mainPlayer.mute = !mainPlayer.mute;
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

                mainPlayer.mute = false;
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
                _openSetting = false;
                result = 2;
                _matchEnd = true;
                if (!GameManager.MuteMusic)
                    SplashKit.StopMusic();

                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Defeat");
            }
        }

        protected void HandleStatistic()
        {
            if (Hover(SplashKit.MousePosition(), 900 - 70 + 5, 35 / 2 - 25 / 2, 25, 25))
            {
                mainPlayer.mute = true;
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");
                _openStatistic = true;
            }

            int x = 810, y = 240;

            if (Hover(SplashKit.MousePosition(), 450 + x / 2 - 30, 300 - y / 2 + 15, 20, 20))
            {
                if (!GameManager.MuteSound)
                    SplashKit.PlaySoundEffect("Click");

                mainPlayer.mute = false;
                _openStatistic = false;
            }
        }

        public virtual void Update()
        {
            if (_matchEnd) return;

            // Others
            UpdateLife();

            Time.Update();


            //Blue Team
            foreach (var aidKid in AidKidBlue)
                aidKid.Update(PlayerBlue);

            for (int i = 0; i < PlayerBlue.Count; i++)
                PlayerBlue[i].Update(TowerRed, SoldierRed, PlayerRed, Monsters, PlayerBlue);

            foreach (var tower in TowerBlue)
                tower.Update(SoldierRed, PlayerRed, PlayerBlue);

            for (int i = 0; i < SoldierBlue.Count; i++)
                SoldierBlue[i].Update(TowerRed, SoldierRed, PlayerRed, Monsters, PlayerBlue);


            // Red Team
            foreach (var aidKid in AidKidRed)
                aidKid.Update(PlayerRed);

            for (int i = 0; i < PlayerRed.Count; i++)
                PlayerRed[i].Update(TowerBlue, SoldierBlue, PlayerBlue, Monsters, PlayerRed);

            foreach (var tower in TowerRed)
                tower.Update(SoldierBlue, PlayerBlue, PlayerRed);

            for (int i = 0; i < SoldierRed.Count; i++)
                SoldierRed[i].Update(TowerBlue, SoldierBlue, PlayerBlue, Monsters, PlayerRed);


            // Monsters
            for (int i = 0; i < Monsters.Count; i++)
                Monsters[i].Update(SoldierBlue, SoldierRed, PlayerBlue, PlayerRed, PlayerBlue);
        }

        protected void UpdateLife()
        {
            // Status Match
            if (mainPlayer.Side == 1)
            {
                if (TowerBlue[0].destroyed && !_matchEnd)
                {
                    result = 2;
                    _matchEnd = true;
                    if (!GameManager.MuteMusic)
                        SplashKit.StopMusic();

                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Defeat");
                }else if (TowerRed[0].destroyed && !_matchEnd)
                {
                    result = 1;
                    _matchEnd = true;
                    if (!GameManager.MuteMusic)
                        SplashKit.StopMusic();

                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Winner");
                }
            }
            else
            {
                if (TowerBlue[0].destroyed && !_matchEnd)
                {
                    result = 1;
                    _matchEnd = true;
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Winner");
                }
                else if (TowerRed[0].destroyed && !_matchEnd)
                {
                    result = 2;
                    _matchEnd = true;
                    if (!GameManager.MuteSound)
                        SplashKit.PlaySoundEffect("Defeat");
                }
            }

            cooldownRevive = Math.Max(cooldownRevive - 1, 0);

            //Revive character
            if (Time.Seconds == 0 && cooldownRevive == 0)
            {
                //Blue Team
                SoldierBlue.Add(new Soldier(_user, "Soldier", 47, 34, 1, "top"));
                SoldierBlue[SoldierBlue.Count - 1].SetUpSkill(200, 8, 8, 9, 1, 140, 10);

                SoldierBlue.Add(new Soldier(_user, "Soldier", 47, 34, 1, "mid"));
                SoldierBlue[SoldierBlue.Count - 1].SetUpSkill(200, 8, 8, 9, 1, 140, 10);

                SoldierBlue.Add(new Soldier(_user, "Soldier", 47, 34, 1, "bot"));
                SoldierBlue[SoldierBlue.Count - 1].SetUpSkill(200, 8, 8, 9, 1, 140, 10);


                //Red Team
                SoldierRed.Add(new Soldier(_user, "Soldier", 49, 34, 2, "top"));
                SoldierRed[SoldierRed.Count - 1].SetUpSkill(200, 8, 8, 9, 1, 140, 10);

                SoldierRed.Add(new Soldier(_user, "Soldier", 49, 34, 2, "mid"));
                SoldierRed[SoldierRed.Count - 1].SetUpSkill(200, 8, 8, 9, 1, 140, 10);

                SoldierRed.Add(new Soldier(_user, "Soldier", 49, 34, 2, "bot"));
                SoldierRed[SoldierRed.Count - 1].SetUpSkill(200, 8, 8, 9, 1, 140, 10);

                cooldownRevive = 60;
            }

        }

        protected virtual void HandleEndMatch()
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

                Home.ConnectServer = false;
                Match.Start = false;
                GameManager.ScreenType = "Home";
            }
        }

        protected bool IsVisible(Character Enemy, List<Character> Players, List<Character> Soldiers, List<Character> Towers)
        {
            if (CheckVision(Enemy, Players, 300))
                return true;
            else if (CheckVision(Enemy, Soldiers, 170))
                return true;
            else if (CheckVision(Enemy, Towers, 195))
                return true;
            else
                return false;
        }

        protected bool CheckVision(Character Enemy, List<Character> Objects, float viewRadius)
        {
            foreach (var teammate in Objects)
            {
                float offsetX = 0, offsetY = 0;

                if (teammate is Tower)
                {
                    offsetX = 13.5f;
                    offsetY = 13.5f;
                }

                double dx = Enemy.X - (teammate.X + offsetX);
                double dy = Enemy.Y - (teammate.Y + offsetY);
                double distance = Math.Sqrt(dx * dx + dy * dy);

                if (teammate is Tower)
                {
                    if (((Tower)teammate).TypeTower == 1) viewRadius = 260;
                    else viewRadius = 195;
                }

                if (distance <= viewRadius)
                {
                    if ((Enemy is Player))
                    {
                        if (((Player)Enemy).InBush)
                        {
                            if ((teammate is Player) && ((Player)teammate).InBush)
                                return true;
                        }
                        else
                            return true;
                    }
                    else
                        return true;
                }
            }
            return false;
        }

        public float TextWidth(string letters, string font, int size_font)
        {
            return SplashKit.TextWidth(letters, font, size_font);
        }

        public bool Hover(Point2D mouse, float x, float y, float w, float h)
        {
            if (SplashKit.PointInRectangle(mouse, SplashKit.RectangleFrom(x, y, w, h)))
            {
                return true;
            }
            return false;
        }
    }
}
