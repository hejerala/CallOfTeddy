using UnityEngine;
using System.Collections;

public class Outpost : MonoBehaviour {

    private SkinnedMeshRenderer flag;

    //Internal is like public but not accessible from the inspector
    internal float captureValue;
    internal int team;

    public float captureRate = 0.02f;

	// Use this for initialization
	void Start () {
        flag = GetComponentInChildren<SkinnedMeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Color teamColor = GameManager.instance.teamColors[team];
        flag.material.color = Color.Lerp(Color.white, teamColor, captureValue);
	}

    void OnTriggerStay(Collider otherCollider) {
        BaseUnit unit = otherCollider.GetComponent<BaseUnit>();
        if (unit != null) {
            if (unit.team == this.team) {
                captureValue += captureRate;
                if (captureValue >= 1)
                    captureValue = 1;
            } else {
                captureValue -= captureRate;
                if (captureValue <= 0) {
                    this.team = unit.team;
                    captureValue = 0;
                }    
            }
        }
    }

}
