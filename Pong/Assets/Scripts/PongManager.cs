using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PongManager : MonoBehaviour
{
    // ----- Fields -----

    // normBall, fastBall, sneakBall, smallBall, cpuPaddle, & player2Paddle fields - Used to reference the different ball and paddle objects
    public GameObject normBall;
    public GameObject fastBall;
    public GameObject sneakBall;
    public GameObject smallBall;
    public GameObject cpuPaddle;
    public GameObject player2Paddle;

    // ballBody field - Used to keep track of the ball's current location
    protected Rigidbody ballBody;

    // balls field - A list of GameObjects which will be used to store the balls usable in the current round
    protected List<GameObject> balls = new List<GameObject>();

    // ballIndex field - Used to keep track of what ball is in play
    private int ballIndex;

    // temp & ball fields - Used to reference ball objects currently loaded in the game
    protected GameObject temp;
    protected GameObject ball;

    // ballScript field - Used to call Ball methods
    protected Ball ballScript;

    // timed, crazy & cpu fields - Used to determine if the current round will be timed, have non-normal balls enabled, or have a cpu instead of player 2
    public bool timed;
    public bool crazy;
    public bool cpu;

    // limit field - Used to store the time limit or par score for the round
    public float limit;

    // player1Score, player2Score, timer, par, winner, pauseMenu, and overMenu fields - Used to alter the displayed UI
    public TMP_Text player1Score;
    public TMP_Text player2Score;
    public TMP_Text timer;
    public TMP_Text par;
    public TMP_Text winner;
    public GameObject pauseMenu;
    public GameObject overMenu;

    // paused field - Used to track whether the game is paused or not
    protected bool paused = false;

    // score1 & score2 fields - Used to track the scores of player 1 and player 2 respectively
    protected float score1 = 0;
    protected float score2 = 0;

    // soundList field - Used to store sound effects to be played
    public List<AudioClip> soundList = new List<AudioClip>();

    // gameEnded field - Used to prevent to keep track of the game's state
    private bool gameEnded = false;

    private float ballTime = 0.0f;



    // ----- Methods -----

    // Awake method - Loads static field values into the scene and determines whether to activate the CPU
    private void Awake()
    {
        limit = MenuManager.limit;
        timed = MenuManager.timed;
        crazy = MenuManager.crazy;
        cpu = MenuManager.cpu;

        gameEnded = false;
        cpuPaddle.SetActive(cpu);
        player2Paddle.SetActive(!cpu);
    }

    // Start method - Sets up the game scene
    void Start()
    {
        // Determining whether this round is timed; if it is, the timer UI is displayed, if not, the par UI is displayed
        if (timed)
        {
            timer.gameObject.SetActive(true);
            limit *= 60;
            timer.text = "Time: " + limit.ToString();
        }
        else
        {
            par.gameObject.SetActive(true);
            par.text = "Par: " + limit.ToString();
        }

        // Instantiating normalBall and adding it to the balls list
        temp = Instantiate(normBall);
        temp.SetActive(false);
        balls.Add(temp);

        // If extra balls are enabled, they are also instantiated and added to the balls list
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

        // Calling PreGamePause method
        StartCoroutine(PreGamePause());
    }

    // Update method - Updates the timer UI (if enabled) and detects key presses
    void Update()
    {
        // If the current round is timed, the timer is updated
        if (timed)
        {
            // Updating the limit field
            if (limit > 0)
            {
                limit -= Time.deltaTime;
            }

            // If the time remaining is less than or equal to zero, the game ends and the GameOver method is called
            else
            {
                limit = 0;
                timer.text = "Time: 0:00";
                GameOver();
            }
            if (limit < 0)
            {
                limit = 0;
                timer.text = "Time: 0:00";
                GameOver();
            }

            // How the timer is displayed changes based on how many seconds are left to keep said display consistant
            if(limit % 60 >= 10)
            {
                timer.text = "Time: " + (Mathf.FloorToInt(limit / 60)).ToString() + ":" + ((int)(limit % 60)).ToString();
            }
            else
            {
                timer.text = "Time: " + (Mathf.FloorToInt(limit / 60)).ToString() + ":0" + ((int)(limit % 60)).ToString();
            }
        }

        // Calls CPU method is the cpu is enabled
        if (cpu)
        {
            CPU();
        }

        // If the ESC key is pressed, the TogglePause method is called
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // If the flashing ball is in play, ever other second it turns either invisible or visible
        if (ballIndex == 2)
        {
            ballTime += Time.deltaTime;
            if ((int)(ballTime % 2) == 0)
            {
                ballScript.ballRenderer.enabled = true;
            }
            else
            {
                ballScript.ballRenderer.enabled = false;
            }
        }

        // If the R key is pressed, the ball is reset (for use when the ball starts moving horizontally)
        if (Input.GetKeyDown(KeyCode.R))
        {
            ballScript.Reset();
        }
    }

    // PreGamePause method - Waits for half a second before calling the SpawnBall method; gives players more time to react to the first ball
    IEnumerator PreGamePause()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnBall();
    }

    // SpawnBall method - Spawns a random ball from the balls list by setting it as active and calling its Reset method from ballScript
    public void SpawnBall()
    {
        ballIndex = (int)Random.Range(0, balls.Count);
        ball = balls[ballIndex];
        ballScript = ball.GetComponent<Ball>();
        ballBody = ballScript.ballBody;

        // If the fast ball was selected, its innitial speed is set to be slower than a normal ball
        if(ballIndex == 1)
        {
            ballScript.speed = 5.0f;
        }

        // If the small ball was selected, its speed is set to be slightly faster than a normal ball
        else if (ballIndex == 3)
        {
            ballScript.speed = 16.0f;
        }

        // The ball is made active and its Reset method is called
        ballTime = 0;
        ball.SetActive(true);
        ballScript.Reset();
    }

    // TogglePause method - Toggles whether the game is paused and the pauseMenu is visible
    public void TogglePause()
    {
        Time.timeScale = (Time.timeScale + 1) % 2;
        paused = !paused;
        pauseMenu.SetActive(paused);
    }

    // GameOver method - Pauses runtime and displays the overMenu with the relevant winner displayed
    public void GameOver()
    {
        Time.timeScale = 0;
        overMenu.SetActive(true);
        if (score1 > score2)
        {
            winner.text = "Player 1 Wins!";
        }
        else if (score1 < score2)
        {
            winner.text = "Player 2 Wins!";
        }
        else
        {
            winner.text = "Draw!";
        }

        // Playing the game over sound effect
        if (!gameEnded)
        {
            PlaySound(3);
        }

        // Updating gameEnded
        gameEnded = true;
    }

    // Player1Scored method - Handles when player 1 scores
    public void Player1Scored()
    {
        // score1 is incremented and the UI is updated
        score1++;
        player1Score.text = score1.ToString();

        // Playing the score sound effect
        PlaySound(2);

        // If player 1 has matched or exceeded par, the game ends
        if (!timed && score1 >= limit)
        {
            GameOver();
        }

        // Calls the DestroyBall method
        DestroyBall();
    }

    // Player2Scored method - Handles when player 2 scores
    public void Player2Scored()
    {
        // score2 is incremented and the UI is updated
        score2++;
        player2Score.text = score2.ToString();

        // Playing the score sound effect
        PlaySound(2);

        // If player 2 has matched or exceeded par, the game ends
        if (!timed && score2 >= limit)
        {
            GameOver();
        }

        // Calls the DestroyBall method
        DestroyBall();
    }

    // DestroyBall method - Despawns the ball by setting it to be inactive; calls the SpawnBall method to spawn a new ball
    public void DestroyBall()
    {
        ball.SetActive(false);
        SpawnBall();
    }

    // PlaySound method - Plays a sound at a designated index in soundList
    public void PlaySound(int index)
    {
        transform.GetComponent<AudioSource>().PlayOneShot(soundList[index]);
    }

    // CPU method - Handles CPU behavior
    public void CPU()
    {
        // If there is a ball, the CPU moves up/down to align with it
        if (ballBody != null)
        {
            if (ballBody.transform.position.z > cpuPaddle.transform.position.z)
            {
                cpuPaddle.transform.position = new Vector3(15, 0, cpuPaddle.transform.position.z + 12.4f * Time.deltaTime);
            }
            else if (ballBody.transform.position.z < cpuPaddle.transform.position.z)
            {
                cpuPaddle.transform.position = new Vector3(15, 0, cpuPaddle.transform.position.z - 12.4f * Time.deltaTime);
            }
        }

        // Prevents the CPU from moving out of bounds
        if (cpuPaddle.transform.position.z > 9)
        {
            cpuPaddle.transform.position = new Vector3(15, 0, 9);
        }
        else if (cpuPaddle.transform.position.z < -9)
        {
            cpuPaddle.transform.position = new Vector3(15, 0, -9);
        }
    }
}
