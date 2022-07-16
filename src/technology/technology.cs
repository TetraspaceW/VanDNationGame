using System.Collections.Generic;
using Newtonsoft.Json;

class Technology
{
    public string name;
    List<string> requirements;
    [JsonConstructor] Technology(string name, List<string> requirements) { this.name = name; this.requirements = requirements; }
}

class TechTree
{
    List<string> unlocked;

    public static List<Technology> techTree = new TechTreeLoader().techs;

    class TechTreeLoader
    {
        public List<Technology> techs;
        public TechTreeLoader()
        {
            var techFile = System.IO.File.ReadAllText("./src/technology/techs.json");
            this.techs = JsonConvert.DeserializeObject<List<Technology>>(techFile);
        }
    }
}