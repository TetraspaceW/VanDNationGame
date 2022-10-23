using Godot;
using System.Collections.Generic;
using System.Linq;

public class Sidebar : CanvasLayer
{
    RichTextLabel scaleLabel;
    RichTextLabel sidePanelLabel;
    RichTextLabel dateLabel;
    VBoxContainer availableBuildingsTable;

    public MapView mapView; // Does C# handle interfaces?

    public BuildingTemplate selectedBuilding;

    public override void _Ready()
    {
        scaleLabel = (RichTextLabel)FindNode("ScaleText");
        dateLabel = (RichTextLabel)FindNode("DateText");
        sidePanelLabel = (RichTextLabel)FindNode("SidePanelText");
        availableBuildingsTable = (VBoxContainer)FindNode("AvailableBuildingsTable");
    }

    public void _OnNextTurnButtonPress()
    {
        mapView.NextTurn();
    }

    public void SetScaleLabelText(string text) { SetLabelText(scaleLabel, text); }

    public void SetDateLabelText(string text) { SetLabelText(dateLabel, "[right]", text); }

    public void SetSidePanelLabelText(params object[] what) { SetLabelText(sidePanelLabel, what); }

    private void SetLabelText(RichTextLabel label, params object[] what)
    {
        var text = what.Select((it) => (it.ToString())).Aggregate((acc, it) => acc + it);
        label.Text = "";
        label.PushColor(new Color(1, 1, 1));
        label.AppendBbcode(text);
        label.Pop();
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
