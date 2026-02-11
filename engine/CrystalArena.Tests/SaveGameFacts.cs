namespace CrystalArena.Tests
{
    using AI;
    using Infrastructure;
    using Xunit;

    public class SaveGameFacts : Scenario
    {
        [Fact]
        public void Save()
        {
            var originalP = GameParameters.Simulation(
                DeckLibrary.CreateTestFire(),
                DeckLibrary.CreateTestIce(),
                new SearchParameters(15, 2, SearchPartitioningStrategies.SingleThreaded)
            );

            var game = new Game(originalP);
            game.Start(numOfTurns: 5);
            var savedGame = game.Save();

            var p = GameParameters.Load(
                player1Controller: PlayerType.Machine,
                player2Controller: PlayerType.Machine,
                savedGame: savedGame
            );

            var game1 = new Game(p);

            // 2 games will be equal only if game is in exact same state
            // load game only loads the game up to the last decision recorded
            // resume the game until state count is equal
            game1.Simulate(() => game1.Turn.StateCount < game.Turn.StateCount);

            // hash depends on card visibility, visibility depends
            // on who the searching player is.
            game.Players.Searching = game.Players.Player1;
            game1.Players.Searching = game1.Players.Player1;

            Assert.Equal(game.CalculateHash(), game1.CalculateHash());
        }

        private Game SimulateGame()
        {
            var p = GameParameters.Simulation(
                DeckLibrary.CreateTestFire(),
                DeckLibrary.CreateTestIce(),
                new SearchParameters(15, 2, SearchPartitioningStrategies.SingleThreaded)
            );

            var game = new Game(p);
            game.Start(numOfTurns: 0);
            return game;
        }
    }
}
