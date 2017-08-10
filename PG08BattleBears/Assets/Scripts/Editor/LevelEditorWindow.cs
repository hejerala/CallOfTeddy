using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class LevelEditorWindow : EditorWindow {

    private string levelName;
    private WorldManager world;

    private const string PATH = "Assets/Levels/";

    [MenuItem("Tools/PG08 Level Editor")]
    public static void ShowWindow() {
        //This will show a new panel of the LevelEditorWindow
        EditorWindow.GetWindow<LevelEditorWindow>();
    }

    void OnGUI() {
        if (world == null) {
            world = FindObjectOfType<WorldManager>();
            if (world == null) {
                GUILayout.Label("No world found in the scene.");
                return;
            }
        }
        levelName = EditorGUILayout.TextField("Name of level", levelName);

        if (GUILayout.Button("Save")) {
            string filePath = PATH + levelName + ".txt";
            //We create a file
            StreamWriter sw = File.CreateText(filePath);
            //We place the Json data in the file
            sw.Write(world.GetData());
            //It is very important that you always clase the streamwriter after using it
            sw.Close();
            //This refreshes the project view so we can see the saved file
            AssetDatabase.Refresh();
        }

        GUILayout.Label("Saved Levels:");
        DirectoryInfo dir = new DirectoryInfo(PATH);
        //This gets an array of all .txt the files in this directory (my saved levels)
        FileInfo[] files = dir.GetFiles("*.txt");
        foreach (FileInfo file in files) {
            if (GUILayout.Button(file.Name)) {
                StreamReader sr = file.OpenText();
                //Returns all text in the file as a string and set the world with it
                world.SetData(sr.ReadToEnd());
                //Always coles de streamreader
                sr.Close();

                //We should rebake the navmesh when the world is rebuilt <- commented because it takes too long
                //NavMeshBuilder.BuildNavMesh();
            }
        }

    }

}
