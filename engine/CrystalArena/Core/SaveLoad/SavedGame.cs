using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CrystalArena
{
    public class SavedGame
    {
        public DecisionLog Decisions;
        public PlayerParameters Player1;
        public PlayerParameters Player2;
        public int RandomSeed;
        public int StateCount;

        public JObject ToJson()
        {
            var json = new JObject();
            json["randomSeed"] = RandomSeed;
            json["stateCount"] = StateCount;
            json["player1"] = JObject.FromObject(Player1);
            json["player2"] = JObject.FromObject(Player2);
            json["decisions"] = new JArray(Decisions.SavedDecisions.Select(d => JObject.Parse(d)));
            return json;
        }

        public static SavedGame FromJson(JObject json, Game game)
        {
            var savedGame = new SavedGame();
            savedGame.RandomSeed = json["randomSeed"]!.Value<int>();
            savedGame.StateCount = json["stateCount"]!.Value<int>();
            savedGame.Player1 = json["player1"]!.ToObject<PlayerParameters>()!;
            savedGame.Player2 = json["player2"]!.ToObject<PlayerParameters>()!;

            var decisionsArray = (JArray)json["decisions"]!;
            var decisionStrings = decisionsArray.Select(d => d.ToString(Formatting.None)).ToList();
            savedGame.Decisions = new DecisionLog(game, decisionStrings);

            return savedGame;
        }
    }
}
