using UnityEngine;
using System.Collections.Generic;
using System.IO;

// This is used to import a list of all the animations that exist
// and provide their path reference for easy animations...
public static class AnimationPrefabs {
    public static Dictionary<string, string> paths = null;
    
    public static string resourceBasePath = "Assets/Resources/";
    public static string animationsPath = "Prefabs/Animations/";
    
    public static string GetPath(string animationName) {
        string returnString = "";
        paths.TryGetValue(animationName, out returnString);
        return returnString;
    }
    
    public static string GetBasePath() {
        return resourceBasePath;
    }
    public static string GetAnimationPath() {
        return animationsPath;
    }
    
    public static void PopulatePaths() {
        paths = new Dictionary<string, string>();
        PopulateFromPath(GetAnimationPath());
    }
    
    public static void PopulateFromPath(string path) {
        // string currentPath = Getb
        string currentPath = GetBasePath() + path;
        // Debug.Log("------------------------------");
        DirectoryInfo directoryInfo = new DirectoryInfo(currentPath);
        DirectoryInfo[] subdirectoriesInfo = directoryInfo.GetDirectories();
        FileInfo[] filesInfo = directoryInfo.GetFiles("*.prefab");
        
        // Debug.Log("Path being searched: " + directoryInfo.FullName);
        // Debug.Log("Subdirectories found: " + subdirectoriesInfo.Length);
        // Debug.Log("Files found: " + filesInfo.Length);
        foreach(FileInfo fileInfo in filesInfo) {
            string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            // Debug.Log("Name: " + fileName + "\n");
            // Debug.Log("Path: " + path);
            paths[fileName] = path + fileName;
        }
        
        // Dives down
        foreach(DirectoryInfo subdirectoryInfo in subdirectoriesInfo) {
            string newPath = path + subdirectoryInfo.Name + "/";
            PopulateFromPath(newPath);
        }
    }
}