using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
class Faction
{
    public bool player;
    public HashSet<string> techsKnown;
}

class FactionList
{
    public static List<Faction> factions = new FactionListLoader().factions;
    public static Faction GetPlayerFaction()
    {
        return factions.First((faction) => { return faction.player; });
    }

    class FactionListLoader
    {
        public List<Faction> factions;
        public FactionListLoader()
        {
            var factionFile = System.IO.File.ReadAllText("./src/faction/faction.json");
            factions = (JsonConvert.DeserializeObject<List<Faction>>(factionFile));
        }

    }
}