namespace happygames.Models.MartianChess
{
    public class Player
    {
        private string username;
        private List<Pawn> capturedPawns;

        public Player(string username)
        {
            this.username = username;
            this.capturedPawns = new List<Pawn>();
        }

        public List<Pawn> getCapturedPawns()
        {
            return capturedPawns;
        }

        public void addCapturedPawn(Pawn pawn)
        {
            capturedPawns.Add(pawn);
        }

        public int getNbCapturedPawns()
        {
            return capturedPawns.Count();
        }

        public string getUsername()
        {
            return username;
        }

        public int getScore()
        {
            int score = 0;
            foreach (var item in capturedPawns)
            {
                score += item.getScore();
            }
            return score;
        }
    }
}