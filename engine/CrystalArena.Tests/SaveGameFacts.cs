namespace CrystalArena.Tests
{
    using AI;
    using Infrastructure;
    using Newtonsoft.Json.Linq;
    using Xunit;

    public class SaveGameFacts : Scenario
    {
        [Fact]
        public void Save()
        {
            var game = SimulateGame();
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

        [Fact]
        public void ToJson()
        {
            var game = SimulateGame();
            var savedGame = game.Save();
            var json = savedGame.ToJson();

            Assert.True(json.ContainsKey("randomSeed"));
            Assert.Equal(savedGame.RandomSeed, json["randomSeed"]!.Value<int>());

            Assert.True(json.ContainsKey("stateCount"));
            Assert.Equal(savedGame.StateCount, json["stateCount"]!.Value<int>());

            Assert.True(json.ContainsKey("player1"));
            var p1 = (JObject)json["player1"]!;
            Assert.Equal("Player1", p1["Name"]!.Value<string>());

            Assert.True(json.ContainsKey("player2"));
            var p2 = (JObject)json["player2"]!;
            Assert.Equal("Player2", p2["Name"]!.Value<string>());

            Assert.True(json.ContainsKey("decisions"));
            var decisions = (JArray)json["decisions"]!;
            Assert.All(decisions, d => Assert.IsType<JObject>(d));
        }

        private Game SimulateGame()
        {
            var p = GameParameters.Simulation(
                DeckLibrary.CreateTestFire(),
                DeckLibrary.CreateTestIce(),
                new SearchParameters(15, 2, SearchPartitioningStrategies.SingleThreaded)
            );

            var game = new Game(p);
            game.Start(numOfTurns: 5);
            return game;
        }
    }
}
