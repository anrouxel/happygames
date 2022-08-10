namespace happygames.Data.MartianChess
{
    public class BoardData
    {
        public int horizontalSize {get; set;}
        public int verticalSize {get; set;}
        public BoxData[,] boxes {get; set;}

        public BoardData(BoxData[,] boxes, int horizontalSize, int verticalSize)
        {
            this.boxes = boxes;
            this.horizontalSize = horizontalSize;
            this.verticalSize = verticalSize;
        }
    }
}