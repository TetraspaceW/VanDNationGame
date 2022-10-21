public class Building
{
    (int, int) coords;
    BuildingTemplate template;

    public Building((int, int) coords, BuildingTemplate template)
    {
        this.coords = coords;
        this.template = template;
    }
}