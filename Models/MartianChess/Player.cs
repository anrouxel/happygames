using happygames.Data.MartianChess;

namespace happygames.Models.MartianChess
{
    public class Player
    {
        private string username;
        private string connectionId;
        private List<Pawn> capturedPawns;

        public Player(string username, string connectionId)
        {
            this.username = username;
            this.connectionId = connectionId;
            this.capturedPawns = new List<Pawn>();
        }

        public PlayerData Clone()
        {
            List<PawnData> capturedPawnsData = new List<PawnData>();
            foreach (var item in capturedPawns)
            {
                capturedPawnsData.Add(item.Clone());
            }
            return new PlayerData(username, connectionId, capturedPawnsData);
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

        public string getConnectionId()
        {
            return connectionId;
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