namespace happygames.Data
{
    class Board
    {
        private int horizontalSize = 4;
        private int verticalSize = 8;
        private Box[,]? boxes;

        public void initialize()
        {
            boxes = new Box[verticalSize, horizontalSize];
        }

        public int getHorizontalSize()
        {
            return horizontalSize;
        }

        public int getVerticalSize()
        {
            return verticalSize;
        }

        public Box[,] getBoxes()
        {
            return boxes!;
        }
    }
}