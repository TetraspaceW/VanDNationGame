using Godot;

public partial class CameraBuddy : Node2D
{
	Camera2D camera;

	public override void _Ready()
	{
		this.camera = (Camera2D)GetNode("MapCamera");
	}

	public override void _Process(double delta)
	{
		GlobalPosition = GetGlobalMousePosition();
	}

	public void SetMapSize(int width, int height)
	{
		camera.LimitRight = width * 64 + 300;
		camera.LimitBottom = height * 64 + 16;
	}
}
