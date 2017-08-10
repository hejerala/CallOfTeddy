using UnityEngine;
using System.Collections;

public class PlayerControls : BaseUnit {

    public float speed = 5.0f;
    public float jumpHeight = 10.0f;
    private float raycastDistance = 0.1f;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        //Cursor.lockState = CursorLockMode.Locked;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(horizontalInput, 0.0f, verticalInput);
        anim.SetFloat("HorizontalSpeed", horizontalInput);
        anim.SetFloat("VerticalSpeed", verticalInput);
        //WE have to make sure we rotate the input vector by the rotation of the player
        Vector3 vel = transform.TransformVector(input) * speed;
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            vel.y = jumpHeight;
            anim.SetTrigger("Jump");
        } else {
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
        Camera.main.transform.parent.Rotate(mouseYInput, 0.0f, 0.0f);
    }

    private bool IsGrounded() {
        //Retuns true if it hits anything and false if it didnt
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance);
    }

}
