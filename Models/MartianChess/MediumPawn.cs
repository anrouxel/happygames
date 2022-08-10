namespace happygames.Models.MartianChess
{
    public class MediumPawn : BigPawn
    {
        public override int getScore()
        {
            return 2;
        }

        public override List<Coordinate> getDisplacement(Displacement displacement)
        {
            if (displacement.length() >= 1 && displacement.length() <= 2)
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
            }

            throw new DisplacementException("Déplacement impossible pour le moyen pion");
        }
    }
}