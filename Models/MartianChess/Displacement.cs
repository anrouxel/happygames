namespace happygames.Models.MartianChess
{
    public class Displacement
    {
        private Coordinate origin;
        private Coordinate destination;

        public Displacement(Coordinate origin, Coordinate destination)
        {
            this.origin = origin;
            this.destination = destination;
        }

        public bool isHorizontal()
        {
            return origin.getY() == destination.getY() && origin.getX() != destination.getX();
        }

        public bool isVertical()
        {
            return origin.getX() == destination.getX() && origin.getY() != destination.getY();
        }

        public bool isDiagonal()
        {
            return Math.Abs(origin.getX() - destination.getX()) == Math.Abs(origin.getY() - destination.getY());
        }

        public int length()
        {
            if (isHorizontal())
            {
                return Math.Abs(origin.getX() - destination.getX());
            }
            else
            {
                return Math.Abs(origin.getY() - destination.getY());
            }
        }

        public bool isHorizontalPositive()
        {
            return isHorizontal() && origin.getX() < destination.getX();
        }

        public bool isVerticalPositive()
        {
            return isVertical() && origin.getY() < destination.getY();
        }

        public bool isDiagonalPositiveXPositiveY()
        {
            return isDiagonal() && origin.getX() < destination.getX() && origin.getY() < destination.getY();
        }

        public bool isDiagonalPositiveXNegativeY()
        {
            return isDiagonal() && origin.getX() < destination.getX() && origin.getY() > destination.getY();
        }

        public bool isDiagonalNegativeXPositiveY()
        {
            return isDiagonal() && origin.getX() > destination.getX() && origin.getY() < destination.getY();
        }

        public bool isDiagonalNegativeXNegativeY()
        {
            return isDiagonal() && origin.getX() > destination.getX() && origin.getY() > destination.getY();
        }

        public List<Coordinate> getHorizontalPath()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            int x = 1;
            if (!isHorizontalPositive())
            {
                x = -x;
            }
            for (int i = 0; i <= length(); i++)
            {
                coordinates.Add(new Coordinate(origin.getX() + i * x, origin.getY()));
            }
            return coordinates;
        }

        public List<Coordinate> getVerticalPath()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            int y = 1;
            if (!isVerticalPositive())
            {
                y = -y;
            }
            for (int i = 0; i <= length(); i++)
            {
                coordinates.Add(new Coordinate(origin.getX(), origin.getY() + i * y));
            }
            return coordinates;
        }

        public List<Coordinate> getDiagonalPath()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            int x = 1;
            int y = 1;
            if (isDiagonalPositiveXNegativeY())
            {
                y = -y;
            }
            else if (isDiagonalNegativeXPositiveY())
            {
                x = -x;
            }
            else if (isDiagonalNegativeXNegativeY())
            {
                x = -x;
                y = -y;
            }
            for (int i = 0; i <= length(); i++)
            {
                coordinates.Add(new Coordinate(origin.getX() + i * x, origin.getY() + i * y));
            }
            return coordinates;
        }
    }
}