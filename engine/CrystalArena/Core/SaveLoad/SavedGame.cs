using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CrystalArena
{
    public class SavedGame
    {
        public List<JObject> Decisions;
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
            json["decisions"] = new JArray(Decisions);
            return json;
        }

        public static SavedGame FromJson(JObject json)
        {
            var savedGame = new SavedGame();
            savedGame.RandomSeed = json["randomSeed"]!.Value<int>();
            savedGame.StateCount = json["stateCount"]!.Value<int>();
            savedGame.Player1 = json["player1"]!.ToObject<PlayerParameters>()!;
            savedGame.Player2 = json["player2"]!.ToObject<PlayerParameters>()!;

            var decisionsArray = (JArray)json["decisions"]!;
            savedGame.Decisions = decisionsArray.Select(a => (JObject)a).ToList();

            return savedGame;
        }
    }
}
