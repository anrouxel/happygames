namespace happygames.Data
{
    abstract class Pawn
    {
        public abstract int getScore();
        public abstract List<Coordinate> getDisplacement(Displacement displacement);
    }
}