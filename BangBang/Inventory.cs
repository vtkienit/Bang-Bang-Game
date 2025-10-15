
namespace BangBang
{
    public class Inventory
    {
        public List<Item> Tanks, Guns, Frames, Avatars, Items;

        public Inventory()
        {
            Tanks = new List<Item>();
            Guns = new List<Item>();
            Frames = new List<Item>();
            Avatars = new List<Item>();
            Items = new List<Item>();
        }

        public void AddItem(string TypeAdd, string id, string ImageName, string name, string shortDesc, string fullDesc, int price, int typeItem)
        {
            if (TypeAdd == "Tank")
                Tanks.Add(new Item(id, ImageName, name, shortDesc, fullDesc, price, typeItem));
            else if (TypeAdd == "Gun")
                Guns.Add(new Item(id, ImageName, name, shortDesc, fullDesc, price, typeItem));
            else if (TypeAdd == "Frame")
                Frames.Add(new Item(id, ImageName, name, shortDesc, fullDesc, price, typeItem));
            else if (TypeAdd == "Avatar")
                Avatars.Add(new Item(id, ImageName, name, shortDesc, fullDesc, price, typeItem));
            else
                Items.Add(new Item(id, ImageName, name, shortDesc, fullDesc, price, typeItem));
        }

        public Item? LocateItem(List<Item> items, string id)
        {
            foreach (Item item in items)
                if (item.ID == id) return item;

            return null;
        }
    }
}
