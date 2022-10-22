using Godot;
using System.Collections.Generic;
using System.Linq;

public class Sidebar : CanvasLayer
{
    RichTextLabel scaleLabel;
    RichTextLabel sidePanelLabel;
    VBoxContainer availableBuildingsTable;

    public override void _Ready()
    {
        scaleLabel = (RichTextLabel)FindNode("ScaleText");
        sidePanelLabel = (RichTextLabel)FindNode("SidePanelText");
        availableBuildingsTable = (VBoxContainer)FindNode("AvailableBuildingsTable");
    }

    public void SetScaleLabelText(string text)
    {
        scaleLabel.Text = "";
        scaleLabel.PushColor(new Color(1, 1, 1));
        scaleLabel.AppendBbcode(text);
        scaleLabel.Pop();
    }

    public void SetSidePanelLabelText(params object[] what)
    {
        var text = what.Select((it) => (it.ToString())).Aggregate((acc, it) => acc + it);
        sidePanelLabel.Text = "";
        sidePanelLabel.PushColor(new Color(1, 1, 1));
        sidePanelLabel.AppendBbcode(text);
        sidePanelLabel.Pop();
    }

    public void SetAvailableBuildingsList(List<BuildingTemplate> buildings)
    {
        var children = availableBuildingsTable.GetChildren();
        foreach (Node child in children)
        {
            availableBuildingsTable.RemoveChild(child);
        }

        buildings.ForEach((building) =>
        {
            var buildingButton = GD.Load<PackedScene>("res://src/BuildingButton.tscn").Instance() as BuildingButton;
            availableBuildingsTable.AddChild(buildingButton);
            buildingButton.SetBuilding(building);
        });
    }
}
