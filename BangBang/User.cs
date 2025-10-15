namespace BangBang
{
    public class User
    {
        public string Name, Username;
        public int Coin;
        public string TankUsed, GunUsed, FrameUsed, AvatarUsed, ItemUsed;
        public Inventory Bag;

        public User()
        {
            Bag = new Inventory();
            Name = string.Empty;
            Username = string.Empty;
            Coin = 0;
            TankUsed = string.Empty;
            GunUsed = string.Empty;
            FrameUsed = string.Empty;
            AvatarUsed = string.Empty;
            ItemUsed = string.Empty;
        }
    }
}
