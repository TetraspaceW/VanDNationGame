using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;
public class BuildingTemplate
{
    [JsonConstructor]
    BuildingTemplate(string name, string image, int size, List<string> terrainTypes, string technology, BuildingCost cost, Extraction extraction, Transport transport, Process process, Maintenance maintenance)
    {
        this.name = name;
        this.size = size;
        this.terrainTypes = terrainTypes.Select((it) => (Terrain.TerrainType)Enum.Parse(typeof(Terrain.TerrainType), it)).ToHashSet();
        this.technology = technology;
        this.cost = cost;
        this.extraction = extraction;
        this.transport = transport;
        this.process = process;
        if (image != null) { this.image = image; } else { this.image = name.ToLower(); }
        this.maintenance = maintenance;
    }

    public string name;
    public int size;
    public HashSet<Terrain.TerrainType> terrainTypes;
    public string technology;
    public BuildingCost cost;
    public Extraction extraction;
    public Transport transport;
    public Process process;
    public Maintenance maintenance;

    public class BuildingCost
    {
        public string resource;
        public decimal amount;
    }
    public class Extraction
    {
        public string resource;
        public decimal rate;
    }
    public class Transport
    {
        public int range;
    }
    public class Process
    {
        public string input;
        public string output;
        public decimal amount;
        public decimal rate;
    }
    public class Maintenance
    {
        public string resource;
        public decimal amount;
    }

    public string image;
}