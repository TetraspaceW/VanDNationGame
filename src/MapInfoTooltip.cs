using Godot;
using System;
using System.Linq;

public class MapInfoTooltip : CanvasLayer
{
    RichTextLabel scaleLabel;
    RichTextLabel sidePanelLabel;

    public override void _Ready()
    {
        scaleLabel = (RichTextLabel)FindNode("ScaleText");
        sidePanelLabel = (RichTextLabel)FindNode("SidePanelText");
    }

    public void setScaleLabelText(string text)
    {
        scaleLabel.Text = "";
        scaleLabel.PushColor(new Color(1, 1, 1));
        scaleLabel.AppendBbcode(text);
        scaleLabel.Pop();
    }

    public void setSidePanelLabelText(params object[] what)
    {
        var text = what.Select((it) => (it.ToString())).Aggregate((acc, it) => acc + it);
        sidePanelLabel.Text = "";
        sidePanelLabel.PushColor(new Color(1, 1, 1));
        sidePanelLabel.AppendBbcode(text);
        sidePanelLabel.Pop();
    }
}
