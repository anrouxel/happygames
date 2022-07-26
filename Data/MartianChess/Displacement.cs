namespace happygames.Data
{
    class Displacement
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
            return isHorizontal() && isVertical() && Math.Abs(origin.getX() - destination.getX()) == Math.Abs(origin.getY() - destination.getY());
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
            return isHorizontal() && length() > 0;
        }

        public bool isVerticalPositive()
        {
            return isVertical() && length() > 0;
        }

        public bool isDiagonalPositiveXPositiveY()
        {
            return isDiagonal() && isHorizontalPositive() && isVerticalPositive();
        }

        public bool isDiagonalPositiveXNegativeY()
        {
            return isDiagonal() && isHorizontalPositive() && !isVerticalPositive();
        }

        public bool isDiagonalNegativeXPositiveY()
        {
            return isDiagonal() && !isHorizontalPositive() && isVerticalPositive();
        }

        public bool isDiagonalNegativeXNegativeY()
        {
            return isDiagonal() && !isHorizontalPositive() && !isVerticalPositive();
        }

        public List<Coordinate> getHorizontalPath()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int i = 0; i < length(); i++)
            {
                coordinates.Add(new Coordinate(origin.getX() + i, origin.getY()));
            }
            return coordinates;
        }

        public List<Coordinate> getVerticalPath()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int i = 0; i < length(); i++)
            {
                coordinates.Add(new Coordinate(origin.getX(), origin.getY() + i));
            }
            return coordinates;
        }

        public List<Coordinate> getDiagonalPath()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int i = 0; i < length(); i++)
            {
                coordinates.Add(new Coordinate(origin.getX() + i, origin.getY() + i));
            }
            return coordinates;
        }
    }
}