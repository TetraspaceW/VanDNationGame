public class Building
{
    public (int, int) coords;
    public BuildingTemplate template;

    public Building((int, int) coords, BuildingTemplate template)
    {
        this.coords = coords;
        this.template = template;
    }
}