using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public AIController aiPrefab;
    public float spawnInterval = 2.0f;
    public int team = 0;

	// Use this for initialization
	IEnumerator Start () {
        //Wait one frame for the GameScene to load
        yield return null;
        while (true) {
            AIController aiClone = (AIController)Instantiate(aiPrefab, transform.position, transform.rotation);
            aiClone.team = team;
            yield return new WaitForSeconds(spawnInterval);
        }
	}
	
}
