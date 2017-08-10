using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    //This is called the Singleton pattern. A singleton is a script with a static reference to a single instance of it
    private static GameManager _instance;
    public static GameManager instance {
        get {
            if (_instance == null) {
                //This is only true once at the start of the scene
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public Outpost[] outposts;
    public Hospital[] hospitals;
    public Color[] teamColors;

    //Awake is called before start but also only once
	void Awake () {
        outposts = FindObjectsOfType<Outpost>();
        hospitals = FindObjectsOfType<Hospital>();
	}

    void Start () {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
