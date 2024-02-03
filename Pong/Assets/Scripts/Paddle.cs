using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    protected Rigidbody paddleBody;
    protected float speed = 15.0f;

    public KeyCode keyUpwards;
    public KeyCode keyDownwards;

    private void Awake()
    {
        paddleBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(keyUpwards) && transform.position.z <= 8)
        {
            if (transform.position.z + speed * Time.deltaTime >= 8)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 8);
            }
            else
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            }
        }

        if (Input.GetKey(keyDownwards))
        {
            if (transform.position.z - speed * Time.deltaTime <= -8)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -8);
            }
            else
            {
                transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            }
        }
    }
}
