namespace happygames.Models.MartianChess
{
    public class Box
    {
        private Player? player;
        private Pawn? pawn;

        public bool isEmpty()
        {
            return pawn == null;
        }

        public void setPlayer(Player player)
        {
            this.player = player;
        }

        public void setPawn(Pawn? pawn)
        {
            this.pawn = pawn;
        }

        public Player? getPlayer()
        {
            return player;
        }

        public Pawn? getPawn()
        {
            return pawn;
        }
    }
}