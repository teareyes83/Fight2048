using System;

namespace Fight2048.Game.Model
{
    public struct Coord
    {
        public Coord(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; set; }
        public int Col { get; set; }

        public override string ToString()
        {
            return $"{{{Row}, {Col}}}";
        }

        public static int Distance(Coord from, Coord to)
        {
            return Math.Abs(from.Row - to.Row) + Math.Abs(from.Col - to.Col);
        }
    }
}