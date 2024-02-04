using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // ----- Fields -----

    // speed field - Stores the speed of the paddle; used to move the paddle
    protected float speed = 15.0f;

    // keyUpwards & keyDownwards fields - Used to set which keys are used to move the paddle
    public KeyCode keyUpwards;
    public KeyCode keyDownwards;



    // ----- Methods -----

    // Update method - Detects key presses to move the paddle
    void Update()
    {
        // If the key assigned to keyUpwards is pressed, the paddle moves upwards; however, the paddle is limited in how far it can move upwards
        if (Input.GetKey(keyUpwards) && transform.position.z <= 9)
        {
            if (transform.position.z + speed * Time.deltaTime >= 9)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 9);
            }
            else
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            }
        }

        // If the key assigned to keyDownwards is pressed, the paddle moves downwards; however, the paddle is limited in how far it can move downwards
        if (Input.GetKey(keyDownwards))
        {
            if (transform.position.z - speed * Time.deltaTime <= -9)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -9);
            }
            else
            {
                transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            }
        }
    }
}
