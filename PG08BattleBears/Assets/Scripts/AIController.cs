using UnityEngine;
using System.Collections;

public class AIController : BaseUnit {

    public float attackRange = 10.0f;
    public float attackInterval = 1.0f;
    public float healThreshold = 40.0f;
    private NavMeshAgent agent;

    private enum State {
        Idle,
        MovingToOutpost,
        ChasingEnemy,
        MovingToHospital
    }

    private State currentState;
    private Outpost currentOutpost;
    private BaseUnit currentEnemy;
    private Hospital currentHospital;
    private Vector3 shootOffset = new Vector3(0.0f, 1.5f, 0.0f);

    // Use this for initialization
    protected override void Start () {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        ChangeState(State.Idle);
    }

    void ChangeState(State newState) {
        //We want to Ensure we are only in one state at a time
        StopAllCoroutines();
        currentState = newState;
        switch (currentState) {
            case State.Idle:
                StartCoroutine(OnIdle());
                break;
            case State.MovingToOutpost:
                StartCoroutine(OnMovingToOutpost());
                break;
            case State.ChasingEnemy:
                StartCoroutine(OnChasingEnemy());
                break;
            case State.MovingToHospital:
                StartCoroutine(OnMovingToHospital());
                break;
            default:
                break;
        }
    }

    IEnumerator OnIdle() {
        //print(Time.time);
        //yield return new WaitForSeconds(1.5f);
        //print(Time.time);

        //print(Time.time);
        while (currentOutpost == null) {
            LookForOutpost();
            //yield return new WaitForSeconds(1.5f);
            //This(null) will pause the execution for one frame
            yield return null;
            //print(Time.time);
        }
        ChangeState(State.MovingToOutpost);
    }

    IEnumerator OnMovingToOutpost() {
        //This runs only once when we enter the OnMovingToOutpost State
        agent.SetDestination(currentOutpost.transform.position);
        //As long as the outpost in not fully captured by my team, stay on this state
        while (!(currentOutpost.captureValue == 1 && currentOutpost.team == this.team)) {
            LookForEnemies();
            yield return null;
        }
        //When we have fully captured the outpost we forget that target
        currentOutpost = null;
        ChangeState(State.Idle);
    }

    IEnumerator OnMovingToHospital() {
        //This runs only once when we have low health
        //Debug.Log("Hospital");
        agent.SetDestination(currentHospital.transform.position);
        //As long as the unit isnt fully healed
        while (health < maxHealth)
        {
            yield return null;
        }
        //When we have fully healed we forget that target
        currentHospital = null;
        ChangeState(State.Idle);
    }

    IEnumerator OnChasingEnemy() {
        float attackTimer = 0.0f;
        while (currentEnemy != null && currentEnemy.health > 0) {
            if (health < healThreshold) {
                //Debug.Log("need heals");
                if(currentHospital == null)
                    LookForHostipal();
                ChangeState(State.MovingToHospital);
            }
            //If the enemy is out of range or I cant see him, chase him
            if (Vector3.Distance(transform.position, currentEnemy.transform.position) > attackRange || !CanSee(currentEnemy.transform, currentEnemy.transform.position + shootOffset)) {
                agent.SetDestination(currentEnemy.transform.position);
            } else {//If the enemy is in range and in view stop and attack
                agent.ResetPath();
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackInterval)
                {
                    //Generates a random attack with a 33% chance to throw a grenade
                    int rand = Random.Range(0,3);
                    //Debug.Log(rand);
                    if (rand <= 1) {
                        //Shoot at the enemy with offset to shoot at the chest;
                        ShootAt(currentEnemy.transform, currentEnemy.transform.position + shootOffset);
                    }
                    else {
                        //Launches a grenade infront of him
                        TossGrenade();
                    }
                    attackTimer = 0.0f;
                }

            }
            yield return null;
        }
        currentEnemy = null;
        ChangeState(State.Idle);
    }

    /// <summary>
    /// Assings currentOutpost to a random one
    /// </summary>
    void LookForOutpost() {
        int rand = Random.Range(0, GameManager.instance.outposts.Length);
        currentOutpost = GameManager.instance.outposts[rand];
    }

    void LookForHostipal() {
        int rand = Random.Range(0, GameManager.instance.hospitals.Length);
        currentHospital = GameManager.instance.hospitals[rand];
    }

    void LookForEnemies() {
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider c in surroundingColliders) {
            BaseUnit otherUnit = c.GetComponent<BaseUnit>();
            //If the collider is another unit, if its from another team, if its alive
            if (otherUnit != null && otherUnit.team != this.team && otherUnit.health > 0 && CanSee(otherUnit.transform, otherUnit.transform.position + shootOffset)) {
                currentEnemy = otherUnit;
                ChangeState(State.ChasingEnemy);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //PlayerController player = FindObjectOfType<PlayerController>();
        //agent.SetDestination(player.transform.position);
        anim.SetFloat("VerticalSpeed", agent.velocity.magnitude);
	}

    protected override void Die() {
        base.Die();
        //We stop all coroutines to ensure we are in no state
        StopAllCoroutines();
        agent.Stop();
        Destroy(GetComponent<Collider>());
    }
}
