using UnityEngine;
using System.Collections;

public class Hospital : MonoBehaviour {

    public int healingFactor = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider otherCollider) {
        BaseUnit unit = otherCollider.GetComponent<BaseUnit>();
        if (unit != null) {
            unit.HealUnit(healingFactor);
        }
    }
}
