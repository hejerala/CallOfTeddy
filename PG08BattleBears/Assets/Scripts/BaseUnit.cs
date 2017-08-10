using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour {

    public int team = 0;
    public float throwForce = 15.0f;
    public Laser laserPrefab;
    public Grenade grenadePrefab;
    public int health = 100;
    public int attackPower = 10;
    public float viewAngle = 75.0f;
    protected Rigidbody rb;
    protected Animator anim;
    protected int maxHealth;
    private Eye[] eyes;

	// Use this for initialization
	protected virtual void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        Color teamColor = GameManager.instance.teamColors[team];
        transform.Find("Teddy/Teddy_Body").GetComponent<Renderer>().material.color = teamColor;

        eyes = GetComponentsInChildren<Eye>();

        maxHealth = health;
    }

    protected bool CanSee(Transform hitObject, Vector3 hitPos) {
        foreach (Eye eyeObj in eyes) {
            //The direction from point A to B is calculated by using Vec3 dir = B - A
            Vector3 direction = hitPos - eyeObj.transform.position;
            //The angle between forward and direction are calculated by using two direction vectors
            if (Vector3.Angle(transform.forward, direction) > viewAngle)
                return false;
            Ray eyeRay = new Ray(eyeObj.transform.position, direction);
            RaycastHit hitInfo;
            //We raycast towards the position that the camera hit
            //If the object that we hit, is the same as the object that the camera hit
            //Thiss teddy can si the hitObject
            if (Physics.Raycast(eyeRay, out hitInfo))
                if (hitInfo.transform == hitObject)
                    return true;
        }
        return false;
    }

    protected void ShootAt(Transform hitObject, Vector3 hitPos) {
        BaseUnit unit = hitObject.GetComponent<BaseUnit>();
        if (unit != null)
            unit.OnHit(attackPower);
        foreach (Eye eyeObj in eyes) {
            Laser laserClone = Instantiate(laserPrefab);
            laserClone.Init(eyeObj.transform.position, hitPos);
        }
    }

    protected void TossGrenade() {
        Grenade tmp = (Grenade)Instantiate(grenadePrefab, transform.position + (transform.forward * 2), transform.rotation);
        tmp.GetComponent<Rigidbody>().AddForce((transform.forward+ transform.up)* throwForce, ForceMode.Impulse);
    }

    public void OnHit(int damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            Die();
        }
    }

    public void HealUnit(int heal) {
        health += heal;
        if (health >= maxHealth) {
            health = maxHealth;
        }
    }

    protected virtual void Die() {
        anim.SetBool("Death", true);
        //Destroy(gameObject);
    }

}
