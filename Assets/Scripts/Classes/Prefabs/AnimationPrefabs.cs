using UnityEngine;
using System.Collections.Generic;
// using System.IO;

// This is used to import a list of all the animations that exist
// and provide their path reference for easy animations...
// YOU MUST configure the subdirectory paths to be the path that would be loaded
// you were calling GameObject.Instantiate. For example if your prefab is under
// Resources/Prefabs/Example/ExampleObject you would use the subdirectory path
// "Prefabs/Example". This is because it determines the loading string associated
// with the asset based on that path
public static class AnimationPrefabs {
    public static Dictionary<string, string> paths = null;
    
    public static string resourceBasePath = "Assets/Resources/";
    public static string[] subDirectoryPaths = new string[] { "Prefabs/Animations/" };
    
    public static string GetPath(string animationName) {
        string returnString = "";
        paths.TryGetValue(animationName, out returnString);
        return returnString;
    }
    
    public static string GetBasePath() {
        return resourceBasePath;
    }
    
    public static string[] GetSubdirectoryPaths() {
        return subDirectoryPaths;
    }
    
    public static void PopulatePathsManually() {
        paths = new Dictionary<string, string>();
        paths["LightningCloud"] = "Prefabs/Animations/Clouds/LightningCloud";
        paths["EntranceSmoke"] = "Prefabs/Animations/Smoke/EntranceSmoke";
    }
    
    // public static void PopulatePathsViaDirectoryIteration() {
    //     paths = new Dictionary<string, string>();
    //     foreach(string subDirectoryPath in subDirectoryPaths) {
    //         // PopulateFromPath(GetSubdirectoryPaths());
    //         PopulateFromPath(subDirectoryPath);
    //     }
    // }
    
    // public static void PopulateFromPath(string path) {
    //     string currentPath = GetBasePath() + path;
    //     // Debug.Log("------------------------------");
    //     DirectoryInfo directoryInfo = new DirectoryInfo(currentPath);
    //     DirectoryInfo[] subdirectoriesInfo = directoryInfo.GetDirectories();
    //     FileInfo[] filesInfo = directoryInfo.GetFiles("*.prefab");
    
    //     // Debug.Log("Path being searched: " + directoryInfo.FullName);
    //     // Debug.Log("Subdirectories found: " + subdirectoriesInfo.Length);
    //     // Debug.Log("Files found: " + filesInfo.Length);
    //     foreach(FileInfo fileInfo in filesInfo) {
    //         string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
    //         // Debug.Log("Name: " + fileName + "\n");
    //         // Debug.Log("Path: " + path);
    //         paths[fileName] = path + fileName;
    //     }
    
    //     // Dives down
    //     foreach(DirectoryInfo subdirectoryInfo in subdirectoriesInfo) {
    //         string newPath = path + subdirectoryInfo.Name + "/";
    //         PopulateFromPath(newPath);
    //     }
    // }
}