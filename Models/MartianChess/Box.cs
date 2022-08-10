using happygames.Data.MartianChess;

namespace happygames.Models.MartianChess
{
    public class Box
    {
        private Player? player;
        private Pawn? pawn;

        public BoxData Clone()
        {
            PlayerData? playerData = null;
            PawnData? pawnData = null;
            if (player != null)
            {
                playerData = player.Clone();
            }
            if (pawn != null)
            {
                pawnData = pawn.Clone();
            }
            return new BoxData(playerData, pawnData);
        }

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