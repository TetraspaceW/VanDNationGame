using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

class BuildingTemplateList
{
    public static List<BuildingTemplate> buildingTemplates = new BuildingTemplateListLoader().buildingTemplates;

    class BuildingTemplateListLoader
    {
        public List<BuildingTemplate> buildingTemplates;
        public BuildingTemplateListLoader()
        {
            var buildingFile = System.IO.File.ReadAllText("./src/buildings/buildings.json");
            buildingTemplates = (JsonConvert.DeserializeObject<List<BuildingTemplate>>(buildingFile));
        }

    }

    public static BuildingTemplate Get(string name)
    {
        return buildingTemplates.First((template) => { return template.name == name; });
    }
}