namespace happygames.Data.MartianChess
{
    public class CoordinateData
    {
        public int x { get; set; } // Coordinate x
        public int y { get; set; } // Coordinate y

        public CoordinateData(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}