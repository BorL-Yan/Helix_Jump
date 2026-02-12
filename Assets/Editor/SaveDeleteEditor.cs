using UnityEditor;
using UnityEngine;

public class SaveDeleteEditor
{
    [MenuItem("Tools/SaveDelete")]
    public static void DeleteLevelSave()
    {
        GameSave.Init();
        var settings = GameSave.GetSettings();
        settings.Level = 1;
        GameSave.SetSettings(settings);
        GameSave.Save();
        
        Debug.Log($"Deleted level save {settings.Level}");
    }
}
