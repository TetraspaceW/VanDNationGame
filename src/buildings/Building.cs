public class Building
{
    public (int, int) coords;
    public BuildingTemplate template;
    public bool active;
    public bool removed;

    public Building((int, int) coords, BuildingTemplate template)
    {
        this.coords = coords;
        this.template = template;
        this.active = true;
    }

    public bool TryMaintenance(TileModel tile)
    {
        var maintenance = template.maintenance;

        // maintenance succeeds if there is none
        if (maintenance == null) { return true; }

        Godot.GD.Print("Available electronics ", tile.GetParentAtScale(tile.CalculateHighestTransportNeigbouring()).totalChildResources.GetAmount(maintenance.resource));

        if (tile.GetParentAtScale(tile.CalculateHighestTransportNeigbouring()).totalChildResources.GetAmount(maintenance.resource) >= maintenance.amount)
        {
            tile.SubtractResource(maintenance.resource, maintenance.amount);
            return true;
        }
        else { 
            switch(template.name)
            {
                case "Satellite":
                    removed = true;
                    break;
            }
            return false; }
    }
}