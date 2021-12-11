namespace Supercell.Laser.Server.Logic.Battle.Tiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LogicTileMap
    {
        private LogicTileMap()
        {
            Tiles = new LogicTile[HEIGHT, WIDTH];
        }

        internal LogicTile[,] Tiles;

        internal const int WIDTH = 60;
        internal const int HEIGHT = 60;

        internal LogicTile GetTile(int xTile, int yTile)
        {
            try {
            return Tiles[yTile, xTile];
            } catch (IndexOutOfRangeException)
            {
                return new LogicTile('X', 0, 0);
            }
        }

        internal static LogicTileMap GenerateTileMap(string map)
        {
            LogicTileMap tilemap = new LogicTileMap();

            char[] chars = map.ToCharArray();

            int idx = 0;

            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    tilemap.Tiles[i, j] = new LogicTile(chars[idx], 150 + 300 * j, 50 + 300 * i);
                    idx++;
                }
            }

            return tilemap;
        }
    }
}
