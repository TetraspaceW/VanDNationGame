using System;
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
            buildingTemplates.ForEach((template) =>
            {
                Godot.GD.Print("Loading building ", template.name, ".");
            });
        }

    }

    public static BuildingTemplate Get(string name)
    {
        return buildingTemplates.First((template) => { return template.name == name; });
    }
}

public class BuildingTemplate
{
    [JsonConstructor]
    BuildingTemplate(string name, int size, List<string> terrainTypes, string technology, BuildingCost cost, Extraction extraction, Transport transport, Process process)
    {
        this.name = name;
        this.size = size;
        this.terrainTypes = terrainTypes.Select((it) => { return (Terrain.TerrainType)Enum.Parse(typeof(Terrain.TerrainType), it); }).ToHashSet();
        this.technology = technology;
        this.cost = cost;
        this.extraction = extraction;
        this.transport = transport;
        this.process = process;
        this.image = name.ToLower();
    }

    public string name;
    public int size;
    public HashSet<Terrain.TerrainType> terrainTypes;
    public string technology;
    public BuildingCost cost;
    Extraction extraction;
    Transport transport;
    Process process;

    public class BuildingCost
    {
        public string resource;
        public double amount;
    }
    class Extraction
    {
        string resource;
        double rate;
    }
    class Transport
    {
        int range;
    }
    class Process
    {
        string input;
        string output;
        double amount;
        double rate;
    }

    public string image;
}