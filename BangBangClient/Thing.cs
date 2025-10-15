using SplashKitSDK;

namespace BangBang
{
    public class Thing
    {
        private string _id, _name, _shortDesc, _fullDesc;
        private int _price;

        private Bitmap _image;

        public Thing(string id, string ImageName, string name, string shortDesc, string fullDesc, int price)
        {
            _image = SplashKit.LoadBitmap(ImageName, "Images/" + ImageName + ".png");
            _id = id;
            _name = name;
            _shortDesc = shortDesc;
            _fullDesc = fullDesc;
            _price = price;
        }

        public string ID {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string ShortDesc
        {
            get { return _shortDesc; }
        }

        public string FullDesc
        {
            get { return _fullDesc; }
        }

        public int Price
        {
            get { return _price; }
        }

        public Bitmap Image
        {
            get { return _image; }
        }
    }
}
