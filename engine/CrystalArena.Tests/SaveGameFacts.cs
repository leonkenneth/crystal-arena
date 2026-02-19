namespace CrystalArena.Tests
{
    using AI;
    using Infrastructure;
    using Newtonsoft.Json;
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

            var hash1 = game.CalculateHash();
            var hash2 = game1.CalculateHash();

            if (hash1 != hash2)
            {
                var savedGame1 = game.Save().ToJson();
                var savedGame2 = game1.Save().ToJson();
                var debug1 = game.DebugCalculateHash();
                var debug2 = game1.DebugCalculateHash();
                Assert.Fail(
                    $"Hash mismatch.\nGame 1:\n{debug1.ToString(Formatting.Indented)}\n"
                        + $"Game 2:\n{debug2.ToString(Formatting.Indented)}\n\n"
                        + "---\n\n"
                        + $"Saved Game 1:\n{savedGame1.ToString(Formatting.Indented)}\n"
                        + $"Saved Game 2:\n{savedGame2.ToString(Formatting.Indented)}\n"
                );
            }
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
