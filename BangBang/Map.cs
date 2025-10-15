using SplashKitSDK;

namespace BangBang
{
    public class Map
    {
        public int Width, Height, ScreenWidth, ScreenHeight, PixelWidth, PixelHeight, TileWidth, TileHeight, TileMiniWidth, TileMiniHeight;

        public List<string> MapGame;

        private Bitmap grass, grass2, bush;

        public Map(string filePath)
        {
            ScreenWidth = 900;
            ScreenHeight = 600;
            TileWidth = TileHeight = 27;
            TileMiniWidth = TileMiniHeight = 2;
            MapGame = new List<string>(File.ReadAllLines(filePath));
            Height = MapGame.Count;
            Width = MapGame[0].Length;
            PixelWidth = Width * TileWidth;
            PixelHeight = Height * TileHeight;

            grass = SplashKit.LoadBitmap("grass", "Images/Grass.png");
            grass2 = SplashKit.LoadBitmap("grass2", "Images/Grass2.png");
            bush = SplashKit.LoadBitmap("bush", "Images/bush.png");
            SplashKit.LoadBitmap("Wall1", "Images/Wall1.png");
            SplashKit.LoadBitmap("Wall4", "Images/Wall4.png");
            SplashKit.LoadBitmap("Wall5", "Images/Wall5.png");
            SplashKit.LoadBitmap("Wall6", "Images/Wall6.png");
            SplashKit.LoadBitmap("Wall7", "Images/Wall7.png");
            SplashKit.LoadBitmap("Wall8", "Images/Wall8.png");
            SplashKit.LoadBitmap("TowerDestroyedBlue1", "Images/TowerDestroyedBlue1.png");
            SplashKit.LoadBitmap("TowerDestroyedBlue2", "Images/TowerDestroyedBlue2.png");
            SplashKit.LoadBitmap("TowerDestroyedRed1", "Images/TowerDestroyedRed1.png");
            SplashKit.LoadBitmap("TowerDestroyedRed2", "Images/TowerDestroyedRed2.png");
            SplashKit.LoadBitmap("TankMonsterDestroyed", "Images/TankMonsterDestroyed.png");
            SplashKit.LoadBitmap("BackgroundMap", "Images/BackgroundBattle.png");
        }

        public void Draw(float cameraX, float cameraY)
        {
            SplashKit.DrawBitmap("BackgroundMap", 0, 0);
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    float x = col * TileWidth - cameraX;
                    float y = row * TileHeight - cameraY;

                    if (MapGame[row][col] == '#' && ((row - 1 >=0 && MapGame[row - 1][col] == '#') || (row + 1 < Height && MapGame[row + 1][col] == '#')))
                    {
                        SplashKit.DrawBitmap("Wall5", x, y);
                    }
                    else if (MapGame[row][col] == '#' && ((col - 1 >= 0 && MapGame[row][col - 1] == '#') || (col + 1 < Width && MapGame[row][col + 1] == '#')))
                    {
                        SplashKit.DrawBitmap("Wall7", x, y);
                    }
                    else if (MapGame[row][col] == 'w' && MapGame[row - 1][col] == '_')
                    {
                        SplashKit.DrawBitmap("Wall5", x, y);
                    }
                    else if (MapGame[row][col] == 'w' && MapGame[row + 1][col] == '_')
                    {
                        SplashKit.DrawBitmap("Wall6", x, y);
                    }
                    else if (MapGame[row][col] == 'w' && MapGame[row][col - 1] == '_')
                    {
                        SplashKit.DrawBitmap("Wall7", x, y);
                    }
                    else if (MapGame[row][col] == 'w' && MapGame[row][col + 1] == '_')
                    {
                        SplashKit.DrawBitmap("Wall8", x, y);
                    }
                    else if (MapGame[row][col] == 'W' && MapGame[row - 1][col] == '_' && MapGame[row + 1][col] == '_')
                    {
                        SplashKit.DrawBitmap("Wall4", x, y - 27);
                    }
                    else if (MapGame[row][col] == 'W' && MapGame[row][col - 1] == '_' && MapGame[row][col + 1] == '_')
                    {
                        SplashKit.DrawBitmap("Wall1", x - 27, y);
                    }
                    else if (MapGame[row][col] == 't')
                    {
                        SplashKit.DrawBitmap("TowerDestroyedBlue2", x - 27, y - 27);
                    }
                    else if (MapGame[row][col] == 'T')
                    {
                        SplashKit.DrawBitmap("TowerDestroyedBlue1", x - 50, y - 50);
                    }
                    else if (MapGame[row][col] == 'r')
                    {
                        SplashKit.DrawBitmap("TowerDestroyedRed2", x - 27, y - 27);                    
                    }
                    else if (MapGame[row][col] == 'R')
                    {
                        SplashKit.DrawBitmap("TowerDestroyedRed1", x - 54, y - 54);
                    }
                    else if (MapGame[row][col] == 'M')
                    {
                        SplashKit.DrawBitmap("TankMonsterDestroyed", x - 54, y - 54);
                    }
                }
            }
            DrawGrassAndBush(cameraX, cameraY);
        }

        private void DrawGrassAndBush(float cameraX, float cameraY)
        {
            for (int row = 0; row < Height; row++)
                for (int col = 0; col < Width; col++)
                {
                    float x = col * TileWidth - cameraX;
                    float y = row * TileHeight - cameraY;

                    if (MapGame[row][col] == 'G')
                        SplashKit.DrawBitmap(grass2, x + 13.5f - grass2.Width / 2, y + 13.5f - grass2.Height / 2);
                    else if (MapGame[row][col] == 'g')
                        SplashKit.DrawBitmap(grass, x + 13.5f - grass.Width / 2, y + 13.5f - grass.Height / 2);
                    else if (MapGame[row][col] == 'B')
                        SplashKit.DrawBitmap(bush, x + 13.5f - grass.Width / 2, y + 13.5f - grass.Height / 2 - 13);
                }
        }

        public void DrawMiniMap()
        {
            float w = 212, h = 148;
            float xinit = 900 - w, yinit = 600 - h;

            SplashKit.FillRectangle(Color.RGBAColor(0, 184, 255, 40), xinit, yinit, w, h);

            for (int row = 0; row < Height; row++)
                for (int col = 0; col < Width; col++)
                {
                    float x = xinit + col * TileMiniWidth;
                    float y = yinit + row * TileMiniHeight;

                    if (MapGame[row][col] == '#')
                        SplashKit.FillRectangle(Color.RGBAColor(0, 184, 255, 230), x, y, TileMiniWidth, TileMiniHeight);
                    else if (MapGame[row][col] == 'w')
                        SplashKit.FillRectangle(Color.RGBAColor(128, 128, 128, 200), x, y, TileMiniWidth, TileMiniHeight);
                    else if (MapGame[row][col] == 'W' && MapGame[row - 1][col] == '_' && MapGame[row + 1][col] == '_')
                        SplashKit.FillRectangle(Color.RGBAColor(128, 128, 128, 200), x, y - TileMiniHeight, TileMiniWidth, TileMiniHeight * 3);
                    else if (MapGame[row][col] == 'W' && MapGame[row][col - 1] == '_' && MapGame[row][col + 1] == '_')
                        SplashKit.FillRectangle(Color.RGBAColor(128, 128, 128, 200), x - TileMiniWidth, y, TileMiniWidth * 3, TileMiniHeight);
                    else if (MapGame[row][col] == 'H')
                        SplashKit.FillRectangle(Color.Yellow, x, y - TileMiniHeight, TileMiniWidth, TileMiniHeight * 3);
                }
        }
    }
}
