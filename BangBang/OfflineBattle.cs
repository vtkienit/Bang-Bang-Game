using SplashKitSDK;
using System.Net.Sockets;

namespace BangBang
{
    public class OfflineBattle : Battle
    {

        public OfflineBattle(DatabaseManager dbManager, User User, TcpClient Client, int TypeBattle, int Side, int level) : base(dbManager, User, Client, TypeBattle, Side)
        {
            mainPlayer.Name = User.Name;
            ((AIPlayer)enemyPlayer).level = level;

            if (level == 1)
            {
                ((Player)mainPlayer).coin = 420;
            }
            else if (level == 2)
            {
                ((Player)mainPlayer).coin = 210;
            }
            else
            {
                ((Player)enemyPlayer).coin = 420;
            }
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
                        PlayerRed[i].Draw(mainPlayer.X - cameraX, mainPlayer.Y - cameraY, cameraX, cameraY);

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
                        PlayerBlue[i].Draw(mainPlayer.X - cameraX, mainPlayer.Y - cameraY, cameraX, cameraY);

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
    }
}
