using SplashKitSDK;

namespace BangBang
{
    public class ItemBattle : Thing
    {
        private string _type;
        private int _value;

        public ItemBattle(string id, string ImageName, string name, string shortDesc, string fullDesc, int price, string Type, int Value) : base(id, ImageName, name, shortDesc, fullDesc, price)
        {
            _type = Type;
            _value = Value;
        }

        public string Type
        {
            get { return _type; }
        }

        public int Value
        {
            get { return _value; }
        }
    }
}
