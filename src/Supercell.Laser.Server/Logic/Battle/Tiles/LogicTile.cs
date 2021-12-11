namespace Supercell.Laser.Server.Logic.Battle.Tiles
{
    using Supercell.Laser.Titan.Logic.Math;
    using System.Collections.Generic;

    internal class LogicTile
    {
        internal LogicTile(char code, int x, int y)
        {
            Code = code;
            Position = new LogicVector2(x, y);
        }

        internal static readonly List<char> Destructibles = new List<char>(new char[]{'M', 'X', 'C', 'F', 'R',
                'T', 'B', 'N', 'Y', 'W', 'a'});

        internal static readonly List<char> Walls = new List<char>(new char[]{'M', 'X', 'C', 'Y', 'I', 'T', 'B', 'N', 'J'});

        internal char Code;
        internal LogicVector2 Position;

        internal int X => Position.X;
        internal int Y => Position.Y;

        internal bool IsBox()
        {
            return Code == '4';
        }

        /*internal int GetXTile()
        {

        }

        internal int GetYTile()
        {

        }*/

        internal bool IsDestructible()
        {
            return Destructibles.Contains(Code);
        }

        internal bool DestroysProjectile()
        {
            return Walls.Contains(Code);
        }
    }
}
