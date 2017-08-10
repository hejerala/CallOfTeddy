using UnityEngine;
using System.Collections;

public class PlayerController : BaseUnit {

    public float respawnTime = 5.0f;
    public float speed = 5.0f;
    public float jumpHeight = 10.0f;
    private float raycastDistance = 0.1f;
    //private int startHealth;
    private float sidewaysClamp = 0.8f;
    private float backwardClamp = 0.4f;
    private float cameraClamp = 40.0f;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        //startHealth = health;
    }
	
	// Update is called once per frame
	void Update () {
        //Cursor.lockState = CursorLockMode.Locked;

        if (Time.timeScale == 0)
            return;

        if (health <= 0)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //Clamps speed to 4.0 sideways and 2.0 backwards (taking 5 as the maximum (normal) speed)
        Vector3 input = new Vector3(Mathf.Clamp(horizontalInput, -sidewaysClamp, sidewaysClamp), 0.0f, Mathf.Clamp(verticalInput, -backwardClamp, 1.0f));
        anim.SetFloat("HorizontalSpeed", horizontalInput);
        anim.SetFloat("VerticalSpeed", verticalInput);
        //WE have to make sure we rotate the input vector by the rotation of the player
        Vector3 vel = transform.TransformVector(input) * speed;
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            vel.y = jumpHeight;
            anim.SetTrigger("Jump");
        }
        else
        {
            //If we are not jumping we set the velocity to the original value
            //If we dont do this the velocity will reset to 0 every frame (making the object float)
            vel.y = rb.velocity.y;
        }
        //rb.velocity = vel * speed;
        rb.velocity = vel;

        float mouseXInput = Input.GetAxis("Mouse X");
        float mouseYInput = Input.GetAxis("Mouse Y");
        transform.Rotate(0.0f, mouseXInput, 0.0f);
        //We rotate de cam pivot based on the mouseYInput
        //Camera.main.transform.parent.Rotate(mouseYInput, 0.0f, 0.0f);
        Vector3 rot = Camera.main.transform.parent.eulerAngles;
        //Limits the rotation of the camera in the x axis to stay between -40 and 40
        rot.x = ClampAngle(rot.x + mouseYInput);
        Camera.main.transform.parent.eulerAngles = rot;

        if (Input.GetMouseButtonDown(0)) {  
            Ray camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(camRay, out hitInfo)) {
                //hitInfo.point provieds us with the position in world space where the ray hit the collider
                if (CanSee(hitInfo.transform, hitInfo.point))
                    ShootAt(hitInfo.transform, hitInfo.point);
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            TossGrenade();
        }
    }

    public float ClampAngle(float currentValue, float clampAroundAngle = 0) {
        float angle = currentValue - (clampAroundAngle + 180);
        while (angle < 0) {
            angle += 360;
        }
        angle = Mathf.Repeat(angle, 360);
        return Mathf.Clamp(angle - 180, -cameraClamp, cameraClamp) + 360 + clampAroundAngle;
    }

    private bool IsGrounded() {
        //Retuns true if it hits anything and false if it didnt
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance);
    }

    protected override void Die() {
        base.Die();
        Invoke("Respawn", respawnTime);
    }

    void Respawn() {
        anim.SetBool("Death", false);
        health = maxHealth;
    }

}
