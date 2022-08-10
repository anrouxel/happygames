using happygames.Data.MartianChess;

namespace happygames.Models.MartianChess
{
    public class SmallPawn : Pawn
    {
        public override int getScore()
        {
            return 1;
        }

        public override List<Coordinate> getDisplacement(Displacement displacement)
        {
            if (displacement.isDiagonal() && displacement.length() == 1)
            {
                return displacement.getDiagonalPath();
            }

            throw new DisplacementException("Déplacement impossible pour le petit pion");
        }

        public override PawnData Clone()
        {
            return new SmallPawnData();
        }
    }
}