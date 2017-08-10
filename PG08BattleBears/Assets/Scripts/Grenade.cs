using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour {

    public GameObject explosionParticleSystem;
    public float radius = 10.0F;
    public float power = 2000.0F;
    public float explosionTimer = 3.0f;
    public int explosionDamage = 30;

    // Use this for initialization
    IEnumerator Start () {
        yield return new WaitForSeconds(explosionTimer);
        Instantiate(explosionParticleSystem, transform.position, transform.rotation);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            BaseUnit unit = hit.GetComponent<BaseUnit>();

            if (unit != null)
                unit.OnHit(explosionDamage);
            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);

        }
        Destroy(gameObject);
    }

}
