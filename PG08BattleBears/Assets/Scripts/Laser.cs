using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
    private float lifeTime = 0.5f;
    public void Init(Vector3 startPos, Vector3 endPos) {
        LineRenderer line = GetComponent<LineRenderer>();
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
        Destroy(gameObject, lifeTime);
    }
}
