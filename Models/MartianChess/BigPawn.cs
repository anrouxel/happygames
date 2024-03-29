using happygames.Data.MartianChess;

namespace happygames.Models.MartianChess
{
    public class BigPawn : Pawn
    {
        public override int getScore()
        {
            return 3;
        }

        public override List<Coordinate> getDisplacement(Displacement displacement)
        {
            if (displacement.isDiagonal())
            {
                return displacement.getDiagonalPath();
            }
            else if (displacement.isHorizontal())
            {
                return displacement.getHorizontalPath();
            }
            else if (displacement.isVertical())
            {
                return displacement.getVerticalPath();
            }

            throw new DisplacementException("Déplacement impossible pour le grand pion");
        }

        public override PawnData Clone()
        {
            return new PawnData { pawn = "G", score = getScore() };
        }
    }
}