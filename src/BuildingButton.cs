using Godot;
using System;

public class BuildingButton : Button
{
    public Sidebar sidebar;
    public BuildingTemplate building;
    TextureRect texture;
    RichTextLabel label;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        texture = (TextureRect)FindNode("BuildingButtonTexture");
        label = (RichTextLabel)FindNode("BuildingButtonLabel");
    }

    public void _OnBuildingButtonPressed()
    {
        sidebar.SetSelectedBuilding(building);
    }

    public void SetBuilding(BuildingTemplate building)
    {
        this.building = building;
        texture.Texture = GD.Load<Texture>("res://assets/buildings/" + building.image + ".png");
        label.Text = "";
        label.PushColor(new Color(1, 1, 1));
        label.AppendBbcode(building.name + "\n" + building.cost.amount + " " + building.cost.resource);
        label.Pop();
    }
}
