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

        settings.Coin = 4400;
		settings.Level = 1;
        GameSave.SetSettings(settings);
        GameSave.Save();
        
        Debug.Log($"Deleted level save level : {settings.Level}, Coin { settings.Coin}");
    }
}
