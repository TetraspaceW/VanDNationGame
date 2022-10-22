using Godot;
using System;

public class BuildingButton : Button
{
    TextureRect texture;
    RichTextLabel label;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        texture = (TextureRect)FindNode("BuildingButtonTexture");
        label = (RichTextLabel)FindNode("BuildingButtonLabel");
    }

    public void SetBuilding(BuildingTemplate building)
    {
        texture.Texture = GD.Load<Texture>("res://assets/buildings/" + building.image + ".png");
        label.Text = "";
        label.PushColor(new Color(1, 1, 1));
        label.AppendBbcode(building.name + "\n" + building.cost.amount + " " + building.cost.resource);
        label.Pop();
    }
}
