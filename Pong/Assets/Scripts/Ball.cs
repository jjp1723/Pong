using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // ----- Fields -----

    // ballBody & ballRenderer fields - Used to reference the ball object
    public Rigidbody ballBody;
    public Renderer ballRenderer;

    // pongManager field - Used to call PongManager methods
    public PongManager pongManager;

    // speed, x, & z fields - Used to set the velocity of the ball
    public float speed = 15.0f;
    private float x;
    private float z;



    // ----- Methods -----

    // Awake method - Gets a reference to the ball's RigidBody component and the PongManager's script
    protected void Awake()
    {
        ballBody = GetComponent<Rigidbody>();
        ballRenderer = GetComponent<Renderer>();
        pongManager = GameObject.Find("PongManager").GetComponent<PongManager>();
    }

    // Start method - Calls the Reset method
    void Start()
    {
        Reset();
    }

    // Reset method - Resets the position and velocity of the ball
    public void Reset()
    {
        // Setting the ball's position to the center of the scene
        ballBody.position = new Vector3(0, 0.25f, 0);

        // Setting the ball's velocity to be at a random angle
        x = Random.value < 0.5f ? -1.0f : 1.0f;
        z = Random.value < 0.5f ? -1.0f : 1.0f;
        ballBody.velocity = new Vector3(x, 0, z) * speed;
    }

    // Oncollision method - Handles collisions between the ball and the score zones
    private void OnCollisionEnter(Collision collision)
    {
        // If the ball collides with ScoreZone1, the Player2Scored method in the PongManager script is called
        if (collision.gameObject.tag == "ScoreZone1")
        {
            pongManager.Player2Scored();
        }
        // If the ball collides with ScoreZone12, the Player1Scored method in the PongManager script is called
        if (collision.gameObject.tag == "ScoreZone2")
        {
            pongManager.Player1Scored();
        }

        // If the ball is a fast ball, its speed increases each time it collides with a wall
        if((collision.gameObject.tag == "WallTop" || collision.gameObject.tag == "WallBottom") && !(speed == 15 || speed == 16))
        {
            ballBody.velocity *= 1.2f;
        }
    }
}
