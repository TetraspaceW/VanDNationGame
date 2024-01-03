using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Sidebar : CanvasLayer
{
	RichTextLabel scaleLabel;
	RichTextLabel sidePanelLabel;
	RichTextLabel dateLabel;
	VBoxContainer availableBuildingsTable;

	public MapView mapView; // Does C# handle interfaces?

	public BuildingTemplate selectedBuilding;

	public override void _Ready()
	{
		scaleLabel = (RichTextLabel)FindChild("ScaleText");
		dateLabel = (RichTextLabel)FindChild("DateText");
		sidePanelLabel = (RichTextLabel)FindChild("SidePanelText");
		availableBuildingsTable = (VBoxContainer)FindChild("AvailableBuildingsTable");
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
		label.ParseBbcode(text);
	}

	public void SetAvailableBuildingsList(List<(BuildingTemplate, bool)> buildings)
	{
		foreach (BuildingButton button in availableBuildingsTable.GetChildren())
		{
			availableBuildingsTable.RemoveChild(button);
		}

		buildings.ForEach((building) =>
		{
			var buildingButton = GD.Load<PackedScene>("res://src/BuildingButton.tscn").Instantiate() as BuildingButton;
			buildingButton.sidebar = this;
			availableBuildingsTable.AddChild(buildingButton);

			buildingButton.SetBuilding(building.Item1);
			buildingButton.Disabled = !building.Item2;
		});
	}

	public void SetSelectedBuilding(BuildingTemplate building)
	{
		selectedBuilding = building;
		foreach (BuildingButton button in availableBuildingsTable.GetChildren())
		{
			button.ButtonPressed = (building != null && button.building.name == building.name);
		}
	}
}
