namespace happygames.Data.MartianChess
{
    public class PawnData
    {
        public string pawn {get; init;}
        public int score {get; init;}

        public PawnData(string pawn, int score)
        {
            this.pawn = pawn;
            this.score = score;
        }
    }
}