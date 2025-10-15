using MongoDB.Bson;
using MongoDB.Driver;

namespace BangBang
{
    public class DatabaseManager
    {
        private MongoClient? _client;
        private IMongoDatabase? _database;
        private IMongoCollection<BsonDocument>? _userCollection, _tankCollection, _gunCollection, _frameCollection, _avatarCollection, _itemCollection;

        private readonly string _connectionString = "mongodb+srv://vtkienit:Kienvu%2319@vutrungkien.y67x5vo.mongodb.net/?retryWrites=true&w=majority&appName=VuTrungKien";

        public void Connect()
        {
            _client = new MongoClient(_connectionString);
            _database = _client.GetDatabase("BangBangGame");
            _userCollection = _database.GetCollection<BsonDocument>("users");
            _tankCollection = _database.GetCollection<BsonDocument>("tanks");
            _gunCollection = _database.GetCollection<BsonDocument>("guns");
            _frameCollection = _database.GetCollection<BsonDocument>("frames");
            _avatarCollection = _database.GetCollection<BsonDocument>("avatars");
            _itemCollection = _database.GetCollection<BsonDocument>("items");
        }

        public void Disconnect()
        {
            _client = null;
            _database = null;
            _userCollection = null;
            _tankCollection = null;
            _gunCollection = null;
            _frameCollection = null;
            _avatarCollection = null;
            _itemCollection = null;
        }

        // Handle Users
        public void CreateUser(string CharacterName, string Username, string Password)
        {
            if (_userCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return;
            }

            var user = new BsonDocument
            {
                {"CharacterName", CharacterName},
                { "Username", Username },
                { "Password", Password },
                { "Tanks", new BsonArray(new[] { "Tank0" }) },
                { "Guns", new BsonArray(new[] { "Gun0" }) },
                { "Frames", new BsonArray(new[] { "Frame1" }) },
                { "Avatars", new BsonArray(new[] { "Avatar1" }) },
                { "Items", new BsonArray(new[] { "Item1" }) },
                { "Tank Used", "Tank0" },
                { "Gun Used", "Gun0" },
                { "Frame Used", "Frame1" },
                { "Avatar Used", "Avatar1" },
                { "Item Used", "Item1" },
                { "Coin", 0 },
                { "CreatedAt", DateTime.UtcNow }
            };

            _userCollection.InsertOne(user);
        }

        public void CreateUser(string CharacterName, string Username, string Password, string[] Tanks, string[] Guns, string[] Frames, string[] Avatars, string[] Items)
        {
            if (_userCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return;
            }

            var user = new BsonDocument
            {
                {"CharacterName", CharacterName},
                { "Username", Username },
                { "Password", Password },
                { "Tanks", new BsonArray(Tanks ?? new string[0]) },
                { "Guns", new BsonArray(Guns ?? new string[0]) },
                { "Frames", new BsonArray(Frames ?? new string[0]) },
                { "Avatars", new BsonArray(Avatars ?? new string[0]) },
                { "Items", new BsonArray(Items ?? new string[0]) },
                { "Tank Used", "Tank0" },
                { "Gun Used", "Gun0" },
                { "Frame Used", "Frame1" },
                { "Avatar Used", "Avatar1" },
                { "Item Used", "Item1" },
                { "Coin", 0 },
                { "CreatedAt", DateTime.UtcNow }
            };

            _userCollection.InsertOne(user);
        }

        public void UpdateUserField(string username, string field, string[] values)
        {
            if (_userCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var update = Builders<BsonDocument>.Update.Set(field, values);

            var result = _userCollection.UpdateOne(filter, update);

            if (result.MatchedCount == 0)
                Console.WriteLine($"No user found with Username: {username}");
            else
                Console.WriteLine($"Updated {field} for user {username}");
        }

        public void UpdateUserField(string username, string field, string itemToAdd)
        {
            if (_userCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var update = Builders<BsonDocument>.Update.Push(field, itemToAdd);

            var result = _userCollection.UpdateOne(filter, update);

            if (result.ModifiedCount > 0)
                Console.WriteLine($"Successfully added '{itemToAdd}' to {field} for user {username}.");
            else
                Console.WriteLine($"No update made. Either user not found or '{itemToAdd}' already exists in {field}.");
        }

        public void UpdateUserField2(string username, string field, int value)
        {
            if (_userCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var update = Builders<BsonDocument>.Update.Set(field, value);

            var result = _userCollection.UpdateOne(filter, update);

            if (result.MatchedCount == 0)
                Console.WriteLine($"No user found with Username: {username}");
            else
                Console.WriteLine($"Updated {field} for user {username}");
        }

        public void UpdateUserField2(string username, string field, string value)
        {
            if (_userCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var update = Builders<BsonDocument>.Update.Set(field, value);

            var result = _userCollection.UpdateOne(filter, update);

            if (result.MatchedCount == 0)
                Console.WriteLine($"No user found with Username: {username}");
            else
                Console.WriteLine($"Updated {field} for user {username}");
        }

        public List<BsonDocument>? ListAllUsers()
        {
            if (_userCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return null;
            }

            var users = _userCollection.Find(new BsonDocument()).ToList();
            return users;
        }

        public BsonDocument? GetUserByUsername(string username)
        {
            if (_userCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return null;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = _userCollection.Find(filter).FirstOrDefault();

            if (user == null)
                Console.WriteLine("User not found with Username: " + username);

            return user;
        }

        // Handle Items
        public void CreateItem(string id, string name, string image, string desc, int price, string type)
        {
            IMongoCollection<BsonDocument>? itemCollection;

            if (type == "Tank") itemCollection = _tankCollection;
            else if (type == "Gun") itemCollection = _gunCollection;
            else if (type == "Frame") itemCollection = _frameCollection;
            else if (type == "Avatar") itemCollection = _avatarCollection;
            else itemCollection = _itemCollection;

            if (itemCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return;
            }

            var newItem = new BsonDocument
            {
                { "ID", id },
                { "Name", name },
                { "Image", image },
                { "Desc", desc },
                { "Price", price },
                { "CreatedAt", DateTime.UtcNow }
            };

            itemCollection.InsertOne(newItem);
        }

        public void DeleteAllItems(string type)
        {
            IMongoCollection<BsonDocument>? itemCollection;

            if (type == "Tank") itemCollection = _tankCollection;
            else if (type == "Gun") itemCollection = _gunCollection;
            else if (type == "Frame") itemCollection = _frameCollection;
            else if (type == "Avatar") itemCollection = _avatarCollection;
            else itemCollection = _itemCollection;

            if (itemCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return;
            }

            var deleteResult = itemCollection.DeleteMany(FilterDefinition<BsonDocument>.Empty);
            Console.WriteLine($"Deleted {deleteResult.DeletedCount} item(s) from the database.");
        }

        public BsonDocument? FindItemById(string id, string type)
        {
            IMongoCollection<BsonDocument>? itemCollection;

            if (type == "Tank") itemCollection = _tankCollection;
            else if (type == "Gun") itemCollection = _gunCollection;
            else if (type == "Frame") itemCollection = _frameCollection;
            else if (type == "Avatar") itemCollection = _avatarCollection;
            else itemCollection = _itemCollection;

            if (itemCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return null;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("ID", id);
            var item = itemCollection.Find(filter).FirstOrDefault();

            if (item == null)
            {
                Console.WriteLine($"No item found with ID: {id}");
                return null;
            }

            return item;
        }

        public List<BsonDocument>? ListAllItems(string type)
        {
            IMongoCollection<BsonDocument>? itemCollection;

            if (type == "Tank") itemCollection = _tankCollection;
            else if (type == "Gun") itemCollection = _gunCollection;
            else if (type == "Frame") itemCollection = _frameCollection;
            else if (type == "Avatar") itemCollection = _avatarCollection;
            else itemCollection = _itemCollection;

            if (itemCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return null;
            }

            var items = itemCollection.Find(new BsonDocument()).ToList();
            return items;
        }

        public List<BsonDocument>? GetUserItems(string username, string type)
        {
            IMongoCollection<BsonDocument>? itemCollection;

            if (type == "Tank") itemCollection = _tankCollection;
            else if (type == "Gun") itemCollection = _gunCollection;
            else if (type == "Frame") itemCollection = _frameCollection;
            else if (type == "Avatar") itemCollection = _avatarCollection;
            else itemCollection = _itemCollection;

            if (_userCollection == null || itemCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return null;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = _userCollection.Find(filter).FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine($"User with username '{username}' not found.");
                return null;
            }

            var itemIds = user.GetValue(type + "s").AsBsonArray.Select(t => t.AsString).ToList();
            var ownedItems = new List<BsonDocument>();

            foreach (var itemId in itemIds)
            {
                var item = FindItemById(itemId, type);
                if (item != null)
                    ownedItems.Add(item);
            }
            return ownedItems;
        }

        public BsonDocument? GetUserItemUsed(string username, string type)
        {
            IMongoCollection<BsonDocument>? itemCollection;

            if (type == "Tank") itemCollection = _tankCollection;
            else if (type == "Gun") itemCollection = _gunCollection;
            else if (type == "Frame") itemCollection = _frameCollection;
            else if (type == "Avatar") itemCollection = _avatarCollection;
            else itemCollection = _itemCollection;

            if (_userCollection == null || itemCollection == null)
            {
                Console.WriteLine("Not connected to database yet. Call Connect() first.");
                return null;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = _userCollection.Find(filter).FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine($"User with username '{username}' not found.");
                return null;
            }

            var itemId = user.GetValue(type + " Used").AsString;
            var ownedItem = FindItemById(itemId, type);

            return ownedItem;
        }

        public string GetUserItem(string username, string field)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = _userCollection.Find(filter).FirstOrDefault();

            string item = user.GetValue(field).AsString;

            return item;
        }
    }
}
