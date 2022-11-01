class StructureRule
{
    public double weight;
    public Structure structure;
    public StructureRule(Structure structure, double weight = 1)
    {
        this.structure = structure;
        this.weight = weight;
    }
}