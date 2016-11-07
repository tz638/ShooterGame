using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerScript : MonoBehaviour {

    private int points, extra=1;
    public float speed, loader, timeChange;
    public Boundary boundary;
    public FireScript fire;
    private float timeLeft;
    public Text score, time, highScore;
    public GameObject background, rapid;
    private AudioClip[] sounds, sadsounds;

    // Use this for initialization
    void Start () {
        
        points = 0;
        loader = 0;
        timeChange = 0;
        fire = (FireScript)FindObjectOfType(typeof(FireScript));
        timeLeft = 60;
        highScore.text = "High Score: "+PlayerPrefs.GetInt("HighScore").ToString();

        sounds = new AudioClip[6]
        {
            (AudioClip)Resources.Load("Sounds/ohyeah"),
            (AudioClip)Resources.Load("Sounds/comeon"),
            (AudioClip)Resources.Load("Sounds/thatsright"),
            (AudioClip)Resources.Load("Sounds/keepgoing"),
            (AudioClip)Resources.Load("Sounds/niceshot"),
            (AudioClip)Resources.Load("Sounds/welldone")
        };

        sadsounds = new AudioClip[3]
        {
            (AudioClip)Resources.Load("Sounds/ugh"),
            (AudioClip)Resources.Load("Sounds/auh"),
            (AudioClip)Resources.Load("Sounds/thathurts")
        };
    }

    public float getTimeLeft()
    {

        return timeLeft;
    }

    public void playHappySound()
    {
        if (GetComponents<AudioSource>()[1].isPlaying) return; // don't play a new sound unless the last hasn't finished
        GetComponents<AudioSource>()[1].clip = sounds[Random.Range(0, sounds.Length)];
        GetComponents<AudioSource>()[1].Play();

    }

    private void playHurtSound()
    {
        if (GetComponents<AudioSource>()[0].isPlaying) return; // don't play a new sound unless the last hasn't finished
        GetComponents<AudioSource>()[0].clip = sadsounds[Random.Range(0, sadsounds.Length)];
        GetComponents<AudioSource>()[0].Play();
    }

    public void increasePoints(int delta)
    {
        if (delta > 0) GetComponent<Animator>().Play("HappyAnimation");
        else GetComponent<Animator>().Play("Sad Animation");
        points += delta;
        score.text = "Score: "+points.ToString();
    }

    public float getPoints()
    {

        return points;
    }    
	
	// Update is called once per frame
	void Update () {

        move();
        implementTimer();
        time.text = "Time Left: " + (int)timeLeft;
        if (timeLeft <= 1.0f) endGame();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var collObject = collision.gameObject;

        if (collObject.name != "ToyGun") return;

        this.GetComponent<CircleCollider2D>().isTrigger = true;
        collObject.GetComponent<BoxCollider2D>().isTrigger = true;

        collObject.transform.position = this.transform.position-new Vector3(2.35f, 0.9f, 0);
        collObject.transform.parent = this.gameObject.transform;         
    }

    public int calculatePoints(Vector3 position, bool bounce, double boost)
    {
        int trial;

        if (bounce) trial = points+(int)Mathf.Floor(-(50.0f - (3.7f - position.x) * 15) / (float)boost);    /* See whether the rapid shooter message should be displayed */
        else trial= points+(int)Mathf.Floor((30.0f - (3.7f - position.x) * 15) / (float)boost);
        
        if (trial > 1800 && timeLeft > 10) rapidShooter();

        if (bounce) return (int)Mathf.Floor(-(50.0f - (3.7f - position.x) * 15) / (float)boost);

        else
        {
            if (points<1800) return (int)Mathf.Floor((30.0f - (3.7f - position.x) * 15) / (float)boost);

            else return extra * (int)Mathf.Floor((30.0f - (3.7f - position.x) * 15) / (float)boost);
        }
        
    }

    void rapidShooter()
    {
        if (extra == 2) return; //This has already happened!

        extra = 2;
        GameObject message = Instantiate(rapid, transform.position, transform.rotation) as GameObject;

        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(134,0,204);

        Destroy(message, 1);

    }

    void OnTriggerEnter2D(Collider2D collider)  
    {                                           
        var collObject = collider.gameObject;
        Destroy(collObject);

        int points = calculatePoints(transform.position, true, fire.getScaleFactor());
        fire.initializeMessage(points, this);

        //this.GetComponents<AudioSource>()[0].Play();
        playHurtSound();
    }

    public void move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Rigidbody2D body = GetComponent<Rigidbody2D>();

        if (moveHorizontal!=0 || moveVertical!=0)
        {
            timeChange = Time.deltaTime;
            speed += timeChange ;
        }

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        body.velocity = movement * speed;

        body.position = new Vector3
       (
           Mathf.Clamp(body.position.x, boundary.xMin, boundary.xMax),
           Mathf.Clamp(body.position.y, boundary.yMin, boundary.yMax),
           0.0f
       );

        if (moveHorizontal==0 && moveVertical==0)
        {
            speed = 3;
        }
    }

    public bool hasGun()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "ToyGun") return true;            
        }

        return false;
    }

    public void implementTimer()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 11.1f && timeLeft > 10.9f)
        {
            background.GetComponent<AudioSource>().Play();
        }

    }

    void endGame()
    {
        SceneManager.LoadScene("Main Menu");
        var high = PlayerPrefs.GetInt("HighScore", 0);

        if (high < (int)getPoints())
            PlayerPrefs.SetInt("HighScore", (int)getPoints());
    }
}
