#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityToolbarExtender; 
using UnityEditor.SceneManagement;



[InitializeOnLoad]
public class ToolbarSceneLoader
{
    static ToolbarSceneLoader()
    {
        // Добавляем нашу кнопку в левую часть тулбара (после кнопок Play/Pause)
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.Space(10);

        GUIContent buttonContent = EditorGUIUtility.IconContent("SceneAsset Icon");
        buttonContent.tooltip = "Quick Scene Switcher";
        buttonContent.text = " Scenes";

        if (GUILayout.Button(buttonContent, "Command", GUILayout.Width(80)))
        {
            ShowSceneMenu();
        }
    }

    static void ShowSceneMenu()
    {
        GenericMenu menu = new GenericMenu();
        
        string[] sceneGuids = AssetDatabase.FindAssets("t:SceneAsset");
        foreach (var guid in sceneGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            
            menu.AddItem(new GUIContent(name), false, () => OpenScene(path));
        }
        
        menu.ShowAsContext();
    }

    static void OpenScene(string path)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(path);
        }
    }
}
#endif