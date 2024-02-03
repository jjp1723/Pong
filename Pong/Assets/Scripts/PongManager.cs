using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongManager : MonoBehaviour
{
    public GameObject normBall;
    public GameObject fastBall;
    public GameObject sneakBall;
    public GameObject smallBall;
    public bool crazy;
    public bool timed;
    public float limit;

    protected List<GameObject> balls = new List<GameObject>();
    protected GameObject ball;
    protected GameObject temp;
    protected Ball ballScript;
    protected float score1 = 0;
    protected float score2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        temp = Instantiate(normBall);
        temp.SetActive(false);
        balls.Add(temp);
        if (crazy)
        {
            temp = Instantiate(fastBall);
            temp.SetActive(false);
            balls.Add(temp);

            temp = Instantiate(sneakBall);
            temp.SetActive(false);
            balls.Add(temp);

            temp = Instantiate(smallBall);
            temp.SetActive(false);
            balls.Add(temp);
        }

        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBall()
    {
        ball = balls[(int)Random.Range(1, balls.Count) - 1];
        ball.SetActive(true);
        ballScript = ball.GetComponent<Ball>();
        ballScript.Reset();
    }

    public void Player1Scored()
    {
        score1++;
        print(score1);
        DestroyBall();
    }

    public void Player2Scored()
    {
        score2++;
        print(score2);
        DestroyBall();
    }

    public void DestroyBall()
    {
        ball.SetActive(false);
        SpawnBall();
    }
}
