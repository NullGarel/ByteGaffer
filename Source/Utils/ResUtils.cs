using System.Collections.Generic;
using Godot;

public static class ResUtils
{
    // Added generic type constraint ensuring T inherits from Resource
    public static List<T> LoadResourcesFromFolder<T>(string path) where T : Resource
    {
        var resources = new List<T>();
        using var dir = DirAccess.Open(path);

        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();

            while (fileName != "")
            {
                if (!dir.CurrentIsDir())
                {
                    if (fileName.EndsWith(".remap"))
                        fileName = fileName.Replace(".remap", "");
                    else if (fileName.EndsWith(".import"))
                        fileName = fileName.Replace(".import", "");

                    // Checks for standard resource extensions
                    if (fileName.EndsWith(".tres") || fileName.EndsWith(".res"))
                    {
                        string fullPath = path.PathJoin(fileName);
                        
                        // Load and safely cast using 'as' pattern matching
                        if (ResourceLoader.Load(fullPath) is T res)
                        {
                            if (!resources.Contains(res))
                            {
                                resources.Add(res);
                            }
                        }
                    }
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }
        else
        {
            GD.Print($"Failed to open directory: {path}");
        }

        return resources;
    }
}
