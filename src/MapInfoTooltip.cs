using Godot;
using System;

public class MapInfoTooltip : RichTextLabel
{

    public override void _Ready()
    {

    }

    public void setText(string text)
    {
        Text = "";
        this.PushColor(new Color(0, 0, 0));
        AppendBbcode(text);
        this.Pop();
    }
}
