namespace happygames.Data.MartianChess
{
    public class BoxData
    {
        public PlayerData? player { get; set; }
        public PawnData? pawn { get; set; }

        public BoxData(PlayerData? player, PawnData? pawn)
        {
            this.player = player;
            this.pawn = pawn;
        }
    }
}