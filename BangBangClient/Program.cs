using SplashKitSDK;

namespace BangBang
{
    public class Program
    {
        public static void Main()
        {
            DatabaseManager dbManager = new DatabaseManager();
            dbManager.Connect();
            
            Window window = SplashKit.OpenWindow("Bang Bang", 900, 600);

            GameManager game = new GameManager(dbManager);

            while (!window.CloseRequested)
            {
                SplashKit.ProcessEvents();

                game.Draw();
                game.Handle();
                game.Update();

                SplashKit.RefreshScreen(60);
            }

            game.DbManager.UpdateUserField2(game.User.Username, "Status", "Offline");
            dbManager.Disconnect();
        }
    }
}
