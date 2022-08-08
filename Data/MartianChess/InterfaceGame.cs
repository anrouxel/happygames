namespace happygames.Data.MartianChess
{
    public interface InterfaceGame
    {
        public void initializeGame(Player player1, Player player2, int mnswg);
        public bool possibleDisplacement(int coordOriginX, int coordOriginY);
        public bool possibleDisplacement(int coordOriginX, int coordOriginY, int coordDestinationX, int coordDestinationY, Player? player);
        public void displace(int coordOriginX, int coordOriginY, int coordDestinationX, int coordDestinationY, Player? player);
        public Player? winnerPlayer();
    }
}