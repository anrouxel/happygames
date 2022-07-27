namespace happygames.Data
{
    class SmallPawn : Pawn
    {
        public override int getScore()
        {
            return 1;
        }

        public override List<Coordinate> getDisplacement(Displacement displacement)
        {
            Console.WriteLine("A");
            Console.WriteLine(displacement.isDiagonal());
            Console.WriteLine(displacement.length());
            if (displacement.isDiagonal() && displacement.length() == 1)
            {
                Console.WriteLine("B");
                return displacement.getDiagonalPath();
            }

            throw new DisplacementException("Déplacement impossible pour le petit pion");
        }
    }
}