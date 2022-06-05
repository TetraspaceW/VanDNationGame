using Godot;

public class CameraBuddy : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Camera2D camera;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.camera = (Camera2D)GetNode("MapCamera");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GlobalPosition = GetGlobalMousePosition();
    }

    public void SetMapSize(int width, int height)
    {
        camera.LimitRight = width * 64 + 200;
        camera.LimitBottom = height * 64 + 16;
    }
}
