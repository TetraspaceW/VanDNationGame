using System.Collections.Generic;
using System;
using Godot;

public class MapTileset
{
    public List<String> images;
    public MapTileset()
    {
        this.images = GetFilesInFolder("", ".png");
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