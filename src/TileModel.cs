using System.Linq;
using System;
using System.Collections.Generic;
using Godot;
using System.Diagnostics;

public partial class TileModel
{
    public Terrain terrain;
    public MapModel internalMap;
    public TileModel parent;
    public bool zoomable;
    public int scale;
    public string image;
    public TileResources localResources;

    public static HashSet<TileModel> activeTiles = new();
    public HashSet<Building> storageBuildings = new();

    // recursive
    public TileResources totalChildResources = new();
    public TileResources totalChildCapacity = new();
    public int highestTransportInside = int.MinValue;

    public TileModel(Terrain terrain, TileModel parent, int scale, bool zoomable = false)
    {
        this.terrain = terrain;
        this.parent = parent;
        this.scale = scale;
        this.zoomable = zoomable;
        image = terrain.filenameForTileType();
        localResources = GetResources(terrain, scale);
    }

    public void SetTerrainType(Terrain.TerrainType terrainType)
    {
        terrain.terrainType = terrainType;
        image = terrain.filenameForTileType();
    }

    public TileResources GetResources(Terrain terrain, int scale) { return new TileResources(); }

    public void BuildingMaintenanceTick()
    {
        internalMap.Buildings.ForEach((building) =>
        {
            building.active = building.TryMaintenance(this);
        });
    }

    public void BuildingResourcesTick()
    {
        var buildings = internalMap.Buildings;
        buildings.ForEach((building) =>
        {
            BuildingTemplate.Extraction extraction = building.template.extraction;
            if (extraction != null && building.active)
            {
                TemporarilyAddLocalResources(extraction.resource, extraction.rate);
            }
        });

        buildings.ForEach((building) =>
        {
            BuildingTemplate.Process process = building.template.process;
            if (process != null && totalChildResources.GetAmount(process.input) >= process.rate && building.active)
            {
                SubtractResource(process.input, process.rate);
                TemporarilyAddLocalResources(process.output, process.rate * process.amount);
            }
        });

        BuildingMaintenanceTick();
    }

    public int UpdateHighestTransportInside(bool onTurn = true)
    {
        int transportRange = int.MinValue;
        if (internalMap != null) // this weird check makes me think it binds to maps instead
        {
            transportRange = internalMap.Buildings
               .Where((building) => building.template.transport != null && building.active)
               .Select((building) => building.template.transport.range)
               .Append(int.MinValue)
               .Max();

            foreach (var tile in internalMap.Tiles)
            {
                transportRange = Math.Max(tile.UpdateHighestTransportInside(onTurn), transportRange);
            }
        }

        highestTransportInside = transportRange;
        return transportRange;
    }

    public int CalculateHighestTransportNeigbouring()
    {
        var tileConsidered = parent;
        var highestTransportNeighboring = highestTransportInside;
        while (tileConsidered != null)
        {
            if (tileConsidered.highestTransportInside >= tileConsidered.scale)
            {
                highestTransportNeighboring = Math.Max(highestTransportNeighboring, tileConsidered.highestTransportInside);
            }
            tileConsidered = tileConsidered.parent;
        }

        return highestTransportNeighboring;
    }

    public HashSet<Building> CalculateStorageBuildings(bool onTurn = true)
    {
        var buildings = new HashSet<Building>();
        if (internalMap != null) // this weird check makes me think it binds to maps instead
        {
            buildings = internalMap.Buildings
               .Where((building) => building.template.transport != null && building.active && building.template.transport.range >= scale).ToHashSet();

            foreach (var tile in internalMap.Tiles)
            {
                buildings.UnionWith(tile.storageBuildings.Where((building) => building.template.transport != null && building.active && building.template.transport.range >= scale).ToHashSet());
            }
        }
        if (parent != null)
        {
            buildings.UnionWith(parent.storageBuildings);
        }
        storageBuildings = buildings;
        return buildings;
    }

    public TileResources CalculateTotalChildResources()
    {
        var resources = new TileResources();
        if (internalMap != null)
        {
            foreach (var building in storageBuildings)
            {
                resources.AddTileResources(building.storage);
            }
        }

        totalChildResources = resources;
        return resources;
    }
    public TileResources CalculateTotalChildCapacity()
    {
        var resources = new TileResources();
        if (internalMap != null)
        {
            foreach (var building in storageBuildings)
            {
                resources.AddTileResources(TileResources.SubtractTileResources(building.capacity, building.storage));
            }
        }

        totalChildCapacity = resources;
        return resources;
    }

    public TileModel GetParentAtScale(int scale)
    {
        if (scale < this.scale) { return null; }
        if (scale == this.scale || parent == null) { return this; }
        return parent.GetParentAtScale(scale);
    }

    public TileResources GetAvailableResources()
    {
        return totalChildResources;
    }


    public void SubtractResource(string resource, decimal amount)
    {
        SubtractResourceFromThisOrChildren(resource, amount);
    }

    private void SubtractResourceFromThisOrChildren(string resource, decimal amount)
    {
        var childrenWithResource = GetChildrenWithResource(resource);
        var totalAmountInChildren = totalChildResources.GetAmount(resource);
        childrenWithResource.ForEach((it) =>
        {
            it.Item1.SubtractResourceFromStorage(resource, amount * (it.Item2 / totalAmountInChildren));
        });
        _ = CalculateTotalChildCapacity();
        _ = CalculateTotalChildResources();

    }

    private void TemporarilyAddLocalResources(string resource, decimal amount)
    {
        var childrenWithCapacity = GetChildrenWithCapacity(resource);
        var totalCapcityInChildren = totalChildCapacity.GetAmount(resource);
        if (totalCapcityInChildren < amount)
        {
            amount = totalCapcityInChildren;
        }
        childrenWithCapacity.ForEach((it) =>
        {
            it.Item1.AddResourceToStorage(resource, amount * (it.Item2 / totalCapcityInChildren));
        });
        _ = CalculateTotalChildCapacity();
        _ = CalculateTotalChildResources();
    }

    private void AddToAllParents(string resource, decimal amount)
    {
        var tile = this;
        while (tile != null)
        {
            tile.totalChildResources.AddAmount(resource, amount);
            tile = tile.parent;
        }
    }

    public List<(Building, decimal)> GetChildrenWithResource(string resource)
    {
        var childrenWithResource = new List<(Building, decimal)>();
        if (internalMap != null)
        {
            foreach (var building in storageBuildings)
            {
                var resourcesInTile = building.storage.GetAmount(resource);
                if (resourcesInTile > 0)
                {
                    childrenWithResource.Add((building, resourcesInTile));
                }
            }
        }
        return childrenWithResource;
    }
    public List<(Building, decimal)> GetChildrenWithCapacity(string resource)
    {
        var childrenWithResource = new List<(Building, decimal)>();
        if (internalMap != null)
        {
            foreach (var building in storageBuildings)
            {
                var resourcesInTile = building.capacity.GetAmount(resource) - building.storage.GetAmount(resource);
                if (resourcesInTile > 0)
                {
                    childrenWithResource.Add((building, resourcesInTile));
                }
            }
        }
        return childrenWithResource;
    }
}
