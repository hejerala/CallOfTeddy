using UnityEngine;
using System.Collections;
using UnityEditor;

public class WorldManager : MonoBehaviour {

	// Use this for initialization
	//IEnumerator Start () {
 //       string worldData = GetData();
 //       print(worldData);
 //       yield return new WaitForSeconds(5.0f);
 //       SetData(worldData);
	//}

    public string GetData() {
        LevelData data = new LevelData();
        //We search our scene for all objects with a LevelObject component
        LevelObject[] objectsInScene = FindObjectsOfType<LevelObject>();
        foreach (LevelObject obj in objectsInScene) {
            //We create a new ObjectData for every LevelObject and we store the necessary information in there
            ObjectData objData = new ObjectData();
            objData.name = obj.name;
            objData.position = obj.transform.position;
            objData.rotation = obj.transform.rotation;
            objData.scale = obj.transform.localScale;
            Spawner s = obj.GetComponent<Spawner>();
            if (s != null)
                objData.team = s.team;
            data.objectList.Add(objData);
        }
        return JsonUtility.ToJson(data);
    }

    public void SetData(string jsonString) {
        LevelData data = JsonUtility.FromJson<LevelData>(jsonString);

        //Before we instantiate the new level we should delete the old one
        foreach (LevelObject obj in FindObjectsOfType<LevelObject>())
            Undo.DestroyObjectImmediate(obj.gameObject);
            //DestroyImmediate(obj.gameObject);

        foreach (ObjectData objData in data.objectList) {
            //We try to load the object from our Resources folder based on the name stored in the Json file
            LevelObject objPrefab = Resources.Load<LevelObject>(objData.name);
            //Safety check in case the prefab doesnt exist in our Resources folder
            if (objPrefab == null) {
                Debug.LogError(objData.name + " doesnt exist!");
                continue;
            }
            LevelObject objClone = Instantiate(objPrefab);

            Undo.RegisterCreatedObjectUndo(objClone.gameObject, "Created level");

            //Unity automatically puts 'clone' behind any cloned object. We want to make sure that this is not the case
            objClone.name = objData.name;
            objClone.transform.position = objData.position;
            objClone.transform.rotation = objData.rotation;
            objClone.transform.localScale = objData.scale;
            Spawner s = objClone.GetComponent<Spawner>();
            if (s != null)
                s.team = objData.team;
        }
    }

}
