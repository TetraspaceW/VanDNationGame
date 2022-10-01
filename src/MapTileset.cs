using System.Collections.Generic;
using System;
using Godot;

public class MapTileset
{
    public TileSet tileset;
    public TileSet buildingTileset;
    public MapTileset()
    {
        this.tileset = GetTilesetWithImages("/tiles");
        // this.buildingTileset = GetTilesetWithImages("/buildings");
    }

    TileSet GetTilesetWithImages(string folder)
    {
        var images = GetFilesInFolder(folder);
        var newTileset = new TileSet();
        foreach (var image in images)
        {
            var id = newTileset.GetLastUnusedTileId();
            newTileset.CreateTile(id);
            newTileset.TileSetName(id, image.ToLower());
            newTileset.TileSetTexture(id, GD.Load<Texture>("res://assets/" + image + ".png"));
            newTileset.TileSetRegion(id, new Rect2(0, 0, 64, 64));
        }

        return newTileset;
    }

    List<String> GetFilesInFolder(string folder)
    {
        string extension = ".png";

        List<String> filesInFolder = new List<string>();
        var dir = new Directory();
        dir.Open("res://assets" + folder);
        dir.ListDirBegin(true, true);
        while (true)
        {
            var open = dir.GetNext();
            if (dir.CurrentIsDir())
            {
                var subdir = open;
                filesInFolder.AddRange(GetFilesInFolder(folder + '/' + subdir));
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