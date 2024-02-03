using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public PongManager pongManager;

    protected Rigidbody ballBody;

    private float x;
    private float z;

    private void Awake()
    {
        ballBody = GetComponent<Rigidbody>();
        pongManager = GameObject.Find("PongManager").GetComponent<PongManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        ballBody.position = new Vector3(0, 0.25f, 0);
        x = Random.value < 0.5f ? -1.0f : 1.0f;
        z = Random.value < 0.5f ? -1.0f : 1.0f;
        ballBody.velocity = new Vector3(x, 0, z) * 15.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ScoreZone1")
        {
            pongManager.Player2Scored();
        }
        if (collision.gameObject.tag == "ScoreZone2")
        {
            pongManager.Player1Scored();
        }
    }
}
