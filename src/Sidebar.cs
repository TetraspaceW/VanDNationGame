using Godot;
using System.Collections.Generic;
using System.Linq;

public class Sidebar : CanvasLayer
{
    RichTextLabel scaleLabel;
    RichTextLabel sidePanelLabel;
    VBoxContainer availableBuildingsTable;

    public BuildingTemplate selectedBuilding;

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
        foreach (BuildingButton button in availableBuildingsTable.GetChildren())
        {
            availableBuildingsTable.RemoveChild(button);
        }

        buildings.ForEach((building) =>
        {
            var buildingButton = GD.Load<PackedScene>("res://src/BuildingButton.tscn").Instance() as BuildingButton;
            buildingButton.sidebar = this;
            availableBuildingsTable.AddChild(buildingButton);

            buildingButton.SetBuilding(building);
        });
    }

    public void SetSelectedBuilding(BuildingTemplate building)
    {
        selectedBuilding = building;
        foreach (BuildingButton button in availableBuildingsTable.GetChildren())
        {
            button.Pressed = (building != null && button.building.name == building.name);
        }
    }
}
