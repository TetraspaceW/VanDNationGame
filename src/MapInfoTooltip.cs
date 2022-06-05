using Godot;
using System;

public class MapInfoTooltip : CanvasLayer
{
    RichTextLabel label;

    public override void _Ready()
    {
        label = (RichTextLabel)((Panel)GetChild(0)).GetChild(0);
    }

    public void setText(string text)
    {
        label.Text = "";
        label.PushColor(new Color(1, 1, 1));
        label.AppendBbcode(text);
        label.Pop();
    }
}
