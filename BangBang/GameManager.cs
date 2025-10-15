using SplashKitSDK;

namespace BangBang
{
    public class GameManager:Page
    {
        private DatabaseManager _dbManager;
        private User _user;
        private SignIn _signIn;
        private Home _home;
        private SignUp _signUp;
        private Shop _shop;
        private Bag _bag;
        private Match _match;

        public static bool _shopLoadResources = false, _bagLoadResources = false;
        public static string ScreenType = "SignIn";
        public static bool MuteMusic, MuteSound;

        public GameManager(DatabaseManager dbManager)
        {
            _dbManager = dbManager;
            _user = new User();
            MuteMusic = MuteSound = false;
            _signIn = new SignIn(dbManager, _user);
            _signUp = new SignUp(dbManager);
            _home = new Home(dbManager, _user);
            _shop = new Shop(dbManager, _user);
            _bag = new Bag(dbManager, _user);
            _match = new Match(dbManager, _user);

            SplashKit.LoadBitmap("Background", "Images/Background.png");
            SplashKit.LoadBitmap("Background2", "Images/Background2.png");
            SplashKit.LoadBitmap("Avatar", "Images/Avatar.png");

            // Fonts
            SplashKit.LoadFont("Arial", "Arial.ttf");
            SplashKit.LoadFont("LuckiestGuy", "LuckiestGuy.ttf");
            SplashKit.LoadFont("Orbitron", "Orbitron.ttf");
            SplashKit.LoadFont("Audiowide", "Audiowide.ttf");
            SplashKit.LoadFont("RussoOne", "RussoOne.ttf");
            SplashKit.LoadFont("Oxanium", "Oxanium.ttf");

            // Musics and Sounds
            SplashKit.LoadMusic("Background", "Sounds/Background.mp3");
            SplashKit.LoadMusic("BackgroundBattle", "Sounds/BackgroundBattle.mp3");
            SplashKit.LoadSoundEffect("Click", "Sounds/Click.mp3");
            SplashKit.LoadSoundEffect("PlayerShoot", "Sounds/PlayerShoot.mp3");
            SplashKit.LoadSoundEffect("PlayerSkill1", "Sounds/PlayerSkill1.mp3");
            SplashKit.LoadSoundEffect("PlayerSkill2", "Sounds/PlayerSkill2.mp3");
            SplashKit.LoadSoundEffect("PlayerSkill3", "Sounds/PlayerSkill3.mp3");
            SplashKit.LoadSoundEffect("SoldierShoot", "Sounds/SoldierShoot.mp3");
            SplashKit.LoadSoundEffect("MonsterShoot", "Sounds/MonsterShoot.mp3");
            SplashKit.LoadSoundEffect("TowerShoot", "Sounds/TowerShoot.mp3");
            SplashKit.LoadSoundEffect("Winner", "Sounds/Winner.mp3");
            SplashKit.LoadSoundEffect("Defeat", "Sounds/Defeat.mp3");
            SplashKit.LoadSoundEffect("Notice", "Sounds/Notice.mp3");
            SplashKit.LoadSoundEffect("Boom", "Sounds/Boom.mp3");
            SplashKit.LoadSoundEffect("Recall", "Sounds/Recall.mp3");

            SplashKit.PlayMusic("Background", -1);
        }

        public void Draw()
        {
            if (GameManager.ScreenType == "SignIn")
            {
                _signIn.Draw();
            }
            else if (GameManager.ScreenType == "SignUp")
            {
                _signUp.Draw();
            }
            else if (GameManager.ScreenType == "Home")
            {

                if (_bagLoadResources == false)
                {
                    _bag.LoadResources();
                    _bagLoadResources = true;
                }

                if (_shopLoadResources == false)
                {
                    _shop.LoadResources();
                    _shopLoadResources = true;
                }

                _home.Draw();
            }
            else if (GameManager.ScreenType == "Shop")
            {
                _shop.Draw();
            }
            else if (GameManager.ScreenType == "Bag")
            {
                _bag.Draw();
            }
            else if (GameManager.ScreenType == "Tutorial")
            {
                _home.Draw();
            }
            else if (GameManager.ScreenType == "Setting")
            {
                _home.Draw();
            }
            else if (GameManager.ScreenType == "Match")
            {
                _match.Draw();
            }
        }

        public void Handle()
        {
            if (GameManager.ScreenType == "SignIn")
            {
                _signIn.Handle();
            }
            else if (GameManager.ScreenType == "SignUp")
            {
                _signUp.Handle();
            }
            else if (GameManager.ScreenType == "Home")
            {
                _home.Handle();
            }
            else if (GameManager.ScreenType == "Shop")
            {
                _shop.Handle();
            }
            else if (GameManager.ScreenType == "Bag")
            {
                _bag.Handle();
            }
            else if (GameManager.ScreenType == "Tutorial")
            {
                _home.Handle();
            }
            else if (GameManager.ScreenType == "Setting")
            {
                _home.Handle();
            }
            else if (GameManager.ScreenType == "Match")
            {
                _match.Handle();
            }
        }

        public void Update()
        {
            if (GameManager.ScreenType == "Home")
            {
                _home.Update();
            }
            else if (GameManager.ScreenType == "Tutorial")
            {
                _home.Update();
            }
            else if (GameManager.ScreenType == "Setting")
            {
                _home.Update();
            }
            else if (GameManager.ScreenType == "Match")
            {
                _match.Update();
            }
        }

        public DatabaseManager DbManager
        {
            get { return _dbManager; }
        }

        public User User
        {
            get { return _user; }
        }
    }
}
