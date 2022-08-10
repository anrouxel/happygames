using happygames.Data.MartianChess;

namespace happygames.Models.MartianChess
{
    public abstract class Pawn
    {
        public abstract PawnData Clone();
        public abstract int getScore();
        public abstract List<Coordinate> getDisplacement(Displacement displacement);
    }
}