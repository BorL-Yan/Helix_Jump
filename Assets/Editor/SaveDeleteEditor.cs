using UnityEditor;
using UnityEngine;

public class SaveDeleteEditor
{
    [MenuItem("Tools/SaveDelete")]
    public static void DeleteLevelSave()
    {
        GameSave.Init();
        var settings = GameSave.GetSettings();
        settings = new GameSettings();
        GameSave.SetSettings(settings);
        GameSave.Save();
        
        Debug.Log($"Deleted level save {settings.Level}");
    }
}
