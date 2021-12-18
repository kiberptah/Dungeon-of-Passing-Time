using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;

using System.IO;


public class CustomEditorUtilities
{
    static string scenesFolderPath = "Assets/Scenes/";


    [MenuItem("UTILITIES/Recompile")]
    static void RecompileAllScripts()
    {
        UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
    }



    //kAutoRefresh has two posible values
    //0 = Auto Refresh Disabled
    //1 = Auto Refresh Enabled
    [MenuItem("UTILITIES/Auto Refresh")]
    static void AutoRefreshToggle()
    {
        var status = EditorPrefs.GetInt("kAutoRefresh");
        if (status == 1)
            EditorPrefs.SetInt("kAutoRefresh", 0);
        else
            EditorPrefs.SetInt("kAutoRefresh", 1);
    }
    [MenuItem("UTILITIES/Auto Refresh", true)]
    static bool AutoRefreshToggleValidation()
    {
        var status = EditorPrefs.GetInt("kAutoRefresh");
        if (status == 1)
            Menu.SetChecked("UTILITIES/Auto Refresh", true);
        else
            Menu.SetChecked("UTILITIES/Auto Refresh", false);
        return true;
    }



    [MenuItem("UTILITIES/Editor/Reload Scene")]
    public static void ReloadScene()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        EditorSceneManager.OpenScene(scenesFolderPath + EditorSceneManager.GetActiveScene().name + ".unity");

    }

    
    

    
}
