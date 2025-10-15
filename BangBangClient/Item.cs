using SplashKitSDK;

namespace BangBang
{
    public class Item:Thing
    {
        private Bitmap _imageShop, _imageBag, _imageBattle, _imageBush, _imageMini; 
        public Item(string id, string ImageName, string name, string shortDesc, string fullDesc, int price, int type) : base(id, ImageName, name, shortDesc, fullDesc, price)
        {
            if (type == 1)
            {
                _imageShop = SplashKit.LoadBitmap(ImageName + "Shop", "Images/" + ImageName + "Shop.png");
                _imageBag = SplashKit.LoadBitmap(ImageName + "Bag", "Images/" + ImageName + "Bag.png");
                _imageBattle = SplashKit.LoadBitmap(ImageName + "Battle", "Images/" + ImageName + "Battle.png");
                _imageBush = SplashKit.LoadBitmap(ImageName + "Bush", "Images/" + ImageName + "Bush.png");
                _imageMini = SplashKit.LoadBitmap(ImageName + "Mini", "Images/" + ImageName + "Mini.png");
            }
            else if (type == 2)
            {
                _imageShop = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
                _imageBag = SplashKit.LoadBitmap(ImageName + "Bag", "Images/" + ImageName + "Bag.png");
                _imageBattle = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
                _imageBush = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
                _imageMini = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
            }
            else if (type == 3)
            {
                _imageShop = SplashKit.LoadBitmap(ImageName + "Shop", "Images/" + ImageName + "Shop.png");
                _imageBag = SplashKit.LoadBitmap(ImageName + "Bag", "Images/" + ImageName + "Bag.png");
                _imageBattle = SplashKit.LoadBitmap(ImageName + "Battle", "Images/" + ImageName + "Battle.png");
                _imageBush = SplashKit.LoadBitmap(ImageName + "Bush", "Images/" + ImageName + "Bush.png");
                _imageMini = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
            }
            else
            {
                _imageShop = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
                _imageBag = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
                _imageBattle = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
                _imageBush = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
                _imageMini = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
            }
        }

        public Bitmap ImageShop
        {
            get { return _imageShop; }
        }

        public Bitmap ImageBag
        {
            get { return _imageBag; }
        }

        public Bitmap ImageBattle
        {
            get { return _imageBattle; }
        }

        public Bitmap ImageBush
        {
            get { return _imageBush; }
        }

        public Bitmap ImageMini
        {
            get { return _imageMini; }
        }
    }
}
