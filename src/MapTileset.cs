using System.Collections.Generic;
using System;
using Godot;

public class MapTileset
{
    public TileSet tileset;
    public MapTileset()
    {
        var images = GetFilesInFolder("", ".png");

        tileset = new TileSet();
        foreach (var image in images)
        {
            var id = tileset.GetLastUnusedTileId();
            tileset.CreateTile(id);
            tileset.TileSetName(id, image);
            tileset.TileSetTexture(id, GD.Load<Texture>("res://assets/tiles/" + image.ToLower() + ".png"));
            tileset.TileSetRegion(id, new Rect2(0, 0, 64, 64));
        }
    }

    List<String> GetFilesInFolder(string folder, string extension)
    {
        List<String> filesInFolder = new List<string>();
        var dir = new Directory();
        dir.Open("res://assets/tiles" + folder);
        dir.ListDirBegin(true, true);
        while (true)
        {
            var open = dir.GetNext();
            if (dir.CurrentIsDir())
            {
                var subdir = open;
                filesInFolder.AddRange(GetFilesInFolder(folder + '/' + subdir, extension));
            }
            else if (open.EndsWith(extension))
            {
                var file = open.GetFile();
                filesInFolder.Add((folder + "/").TrimStart('/') + file.Left(file.Length() - extension.Length()));
            }
            else if (open == "")
            {
                break;
            }
        }
        return filesInFolder;
    }

}