using System.Collections.Generic;
using System;
using Godot;

public partial class MapTileset
{
	public TileSet tileset;
	public TileSet buildingTileset;
	public MapTileset()
	{
		tileset = GetTilesetWithImages("/tiles");
		buildingTileset = GetTilesetWithImages("/buildings");
	}

	TileSet GetTilesetWithImages(string folder)
	{
		var images = GetFilesInFolder(folder);
		var newTileset = new TileSet();
		foreach (var image in images)
		{
			var s = new TileSetAtlasSource
			{
				Texture = GD.Load<Texture2D>("res://assets/" + image + ".png"),
				TextureRegionSize = new Vector2I(64, 64)
			};
			s.CreateTile(Vector2I.Zero, new Vector2I(1, 1));
			s.ResourceName = image.ToLower();
			_ = newTileset.AddSource(s, -1);
			//var id = newTileset.GetLastUnusedTileId();
			//newTileset.CreateTile(id);
			//newTileset.TileSetName(id, image.ToLower());
			//newTileset.TileSetTexture(id, GD.Load<Texture2D>("res://assets/" + image + ".png"));
			//newTileset.TileSetRegion(id, new Rect2(0, 0, 64, 64));
		}
		newTileset.TileSize = new Vector2I(64, 64);

		return newTileset;
	}

	List<String> GetFilesInFolder(string folder)
	{
		string extension = ".png";

		List<String> filesInFolder = new();
		var dir = DirAccess.Open("res://assets" + folder);
		_ = dir.ListDirBegin();
		while (true)
		{
			var open = dir.GetNext();


			if (open == "")
			{
				break;
			}
			else if (dir.CurrentIsDir())
			{
				var subdir = open;
				filesInFolder.AddRange(GetFilesInFolder(folder + '/' + subdir));
			}
			else if (open.EndsWith(extension))
			{
				var file = open.GetFile();
				filesInFolder.Add((folder + "/").TrimStart('/') + file.Left(file.Length - extension.Length));
			}
		}
		return filesInFolder;
	}

}
