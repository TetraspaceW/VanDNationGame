using Godot;

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
}
