public class Building
{
    public (int, int) coords;
    public BuildingTemplate template;
    public bool active;

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

        if (tile.totalChildResources.GetAmount(maintenance.resource) >= maintenance.amount)
        {
            tile.SubtractResource(maintenance.resource, maintenance.amount);
            return true;
        }
        else { return false; }
    }
}