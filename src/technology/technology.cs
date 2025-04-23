using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

class Technology
{
    public string name;
    public List<string> requirements;
    [JsonConstructor] Technology(string name, List<string> requirements) { this.name = name; this.requirements = requirements; }
}

class TechTree
{
    public static List<TreeTechnology> techTree = new TechTreeLoader().techs;

    public partial class TreeTechnology
    {
        public Technology techDef;
        public int? x;
        public int? y;
        public TreeTechnology(Technology tech) { techDef = tech; }
    }

    class TechTreeLoader
    {
        public List<TreeTechnology> techs;
        public TechTreeLoader()
        {
            var techFile = System.IO.File.ReadAllText("./src/technology/techs.json");
            var importedTechs = JsonConvert.DeserializeObject<List<Technology>>(techFile).Select((tech) => new TreeTechnology(tech)).ToList();

            for (int i = 0; i < importedTechs.Count(); i++)
            {
                if (importedTechs[i].x == null)
                {
                    SetTechCoords(importedTechs[i], importedTechs);
                }
            }

            techs = importedTechs;
        }

        private static void SetTechCoords(TreeTechnology tech, List<TreeTechnology> techs)
        {
            var newX = tech.techDef.requirements.Select((req) =>
            {
                var indexToCheck = techs.FindIndex((it) => it.techDef.name == req);
                if (techs[indexToCheck].x == null) { SetTechCoords(techs[indexToCheck], techs); }
                return techs[indexToCheck].x;
            }).Append(0).Max() + 1;
            var newY = techs.FindAll((it) => it.y != null && it.x == newX).Select((it) => it.y).Append(0).Max() + 1;

            tech.x = newX;
            tech.y = newY;
        }
    }
}