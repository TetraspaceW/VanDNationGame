using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;
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
    public Extraction extraction;
    public Transport transport;
    public Process process;

    public class BuildingCost
    {
        public string resource;
        public double amount;
    }
    public class Extraction
    {
        public string resource;
        public double rate;
    }
    public class Transport
    {
        public int range;
    }
    public class Process
    {
        public string input;
        public string output;
        public double amount;
        public double rate;
    }

    public string image;
}