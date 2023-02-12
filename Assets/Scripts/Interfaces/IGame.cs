
    public interface IGame
    {
        int WaveCounts { get; }
        int CurrentWave { get; }

        public enum GameResult
        {
            NotStarted,
            InBuildProcess,
            InProcess,
            Victory,
            Lose
        }
        void StartGame();
        GameResult Status { get; }
    }
