using happygames.Data.MartianChess;

namespace happygames.Models.MartianChess
{
    public class Board
    {
        private int horizontalSize;
        private int verticalSize;
        private Box[,] boxes;

        public Board()
        {
            this.horizontalSize = 4;
            this.verticalSize = 8;
            this.boxes = new Box[verticalSize, horizontalSize];
            initialize();
        }

        public BoardData Clone()
        {
            Console.WriteLine("board début");
            BoxData[][] boxesData = new BoxData[verticalSize][];
            for (int i = 0; i < verticalSize; i++)
            {
                boxesData[i] = new BoxData[horizontalSize];
            }
            for (int y = 0; y < verticalSize; y++)
            {
                for (int x = 0; x < horizontalSize; x++)
                {
                    Console.WriteLine($"===== {y}, {x}");
                    boxesData[y][x] = boxes[y, x].Clone();
                }
            }
            Console.WriteLine("board fin");
            return new BoardData(boxesData, horizontalSize, verticalSize);
        }

        public void initialize()
        {
            for (int y = 0; y < verticalSize; y++)
            {
                for (int x = 0; x < horizontalSize; x++)
                {
                    boxes[y, x] = new Box();
                }
            }
            boxes[0, 0].setPawn(new BigPawn());
            boxes[1, 0].setPawn(new BigPawn());
            boxes[0, 1].setPawn(new BigPawn());
            boxes[1, 1].setPawn(new MediumPawn());
            boxes[0, 2].setPawn(new MediumPawn());
            boxes[2, 0].setPawn(new MediumPawn());
            boxes[2, 2].setPawn(new SmallPawn());
            boxes[1, 2].setPawn(new SmallPawn());
            boxes[2, 1].setPawn(new SmallPawn());
            boxes[7, 3].setPawn(new BigPawn());
            boxes[6, 3].setPawn(new BigPawn());
            boxes[7, 2].setPawn(new BigPawn());
            boxes[6, 2].setPawn(new MediumPawn());
            boxes[7, 1].setPawn(new MediumPawn());
            boxes[5, 3].setPawn(new MediumPawn());
            boxes[5, 2].setPawn(new SmallPawn());
            boxes[6, 1].setPawn(new SmallPawn());
            boxes[5, 1].setPawn(new SmallPawn());
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
            return boxes;
        }

        public override string ToString()
        {
            string content = "";
            for (int y = 0; y < verticalSize; y++)
            {
                for (int x = 0; x < horizontalSize; x++)
                {
                    content += "|";
                    switch (boxes[y, x].getPawn())
                    {
                        case SmallPawn smallPawn:
                            content += "P";
                            break;
                        case MediumPawn mediumPawn:
                            content += "M";
                            break;
                        case BigPawn bigPawn:
                            content += "G";
                            break;
                        default:
                            content += " ";
                            break;
                    }
                }
                content += "|\n";
            }
            return content;
        }
    }
}