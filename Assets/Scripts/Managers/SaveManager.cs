using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveManager
{
    private static string saveFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "Save.json";

    // default values
    public CharNames playerChar = CharNames.Korrah;
    public RoomNames currentRoom = RoomNames.CentralPlace; 
    public Vector3 loadAt; // default is 0, 0, 0

    // Character specific
    public bool zeoliaDefeated = false;

    public void Save()
    {
        File.WriteAllText(saveFile, JsonUtility.ToJson(this));
    }

    public static SaveManager Load() // add save file ID as an argument once I have a save menu
    {
        if (File.Exists(saveFile))
        {
            return JsonUtility.FromJson<SaveManager>(File.ReadAllText(saveFile));
        }
        else
        {
            return new SaveManager();
        }
    }
}
