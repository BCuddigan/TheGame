using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.TopDown
{
    public class Map
    {
        public Vector2[][] MapArray;
        public Vector2 TileSize;
        public string MapName;
        public Texture2D sprite;

        public Map(String name)
        {
            MapName = name;
            MapArray = getMap(name,out TileSize);
        }

        public static Vector2[][] getMap(string mapName, out Vector2 tileSize)
        {
            System.IO.TextReader tr = new System.IO.StreamReader("Content/TopDown/Maps/" + mapName + "/map.txt");
            string tempTileSize = tr.ReadLine();
            string[] tempTileSizes = tempTileSize.Split(',');
            tileSize = new Vector2(float.Parse(tempTileSizes[0]), float.Parse(tempTileSizes[1]));
            string tempMapSize = tr.ReadLine();
            string[] tempMapSizes = tempMapSize.Split(',');
            Vector2 mapSize = new Vector2(float.Parse(tempMapSizes[0]), float.Parse(tempMapSizes[1]));
            string[] mapRows = new string[(int)mapSize.Y];
            Vector2[][] returnMap = new Vector2[(int)mapSize.X][];
            for (int i = 0; i < mapSize.Y; i++)
            {
                mapRows[i] = tr.ReadLine();
            }
            for (int j = 0; j < mapSize.X; j++)
            {
                returnMap[j] = new Vector2[(int)mapSize.Y];
            }

            for (int y = 0; y < mapSize.Y; y++)
            {
                string[] currentRow = mapRows[y].Split(';');
                for (int x = 0; x < mapSize.X; x++)
                {
                    string[] xy = currentRow[x].Split(',');
                    returnMap[x][y] = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                }
            }


            return returnMap;
        }
    }
}