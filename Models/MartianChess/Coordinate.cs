using happygames.Data.MartianChess;

namespace happygames.Models.MartianChess
{
    public class Coordinate
    {
        private int x; // Coordinate x
        private int y; // Coordinate y

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public CoordinateData Clone()
        {
            return new CoordinateData { x = x, y = y };
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }
    }
}