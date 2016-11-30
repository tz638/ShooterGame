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

    public int freeze=0, paused=0, controls=0;
    private int points, extra = 1, hits = 0, shots = 0, dHits = 0, hasrifle=0;
    public float speed, loader, timeChange, slowDown=1;
    private float distance, freezeTime=0;
    public Boundary boundary;
    public FireScript fire;
    public WallMovement wall;
    private ScreenShake shake;
    private float timeLeft;
    public Text score, time, highScore;
    public GameObject boom, background, rapid, rifle, dead, distanceShooter, remover;
    private GameObject target;
    private AudioClip[] sounds, sadsounds;
    private List<float> hitTimes = new List<float>();
    private string[] prefArray;
    public Canvas rulesCanvas, properCanvas, controlsCanvas;

    // Use this for initialization
    void Start () {
        
        points = 0;
        loader = 0;
        timeChange = 0;
        fire = (FireScript)FindObjectOfType(typeof(FireScript));
        shake = (ScreenShake)FindObjectOfType(typeof(ScreenShake));
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

        prefArray = new string[5]
        {
            "HighScore5", "HighScore4", "HighScore3", "HighScore2", "HighScore"
        };
    }

    // Update is called once per frame
    void Update()
    {

        move();
        implementTimer();
        time.text = "Time Left: " + (int)timeLeft;
        if (timeLeft <= 1.0f) endGame();

        fire = (FireScript)FindObjectOfType(typeof(FireScript));
        target = GameObject.FindGameObjectWithTag("Target");

        shakeEnd();

        if (freezeTime > 0 && Time.time - freezeTime > 5.0f)
        {
            freezeTime = 0;
            freeze = 0;
        }

        if (freeze == 0) wall.gameObject.SetActive(true);

        commandListener();
    }

    public void commandListener()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (controls == 1) return;

            if (rulesCanvas.gameObject.activeSelf)
            {
                rulesCanvas.gameObject.SetActive(false);
                Time.timeScale = 1;
                properCanvas.GetComponent<AudioSource>().UnPause();
                background.GetComponent<AudioSource>().UnPause();
                paused = 0;
            }
            else
            {
                rulesCanvas.gameObject.SetActive(true);
                Time.timeScale = 0;
                properCanvas.GetComponent<AudioSource>().Pause();
                background.GetComponent<AudioSource>().Pause();
                paused = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.N) && paused == 0 && controls == 0)
        {
            SceneManager.LoadScene("Level1");
        }

        if (Input.GetKeyDown(KeyCode.M) && paused == 0 && controls == 0)
        {
            if (AudioListener.volume != 0)
            {
                PlayerPrefs.SetFloat("Volume", AudioListener.volume);
                AudioListener.volume = 0;
            }

            else AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (paused == 1) return;

            if (controlsCanvas.gameObject.activeSelf)
            {
                controlsCanvas.gameObject.SetActive(false);
                Time.timeScale = 1;
                properCanvas.GetComponent<AudioSource>().UnPause();
                background.GetComponent<AudioSource>().UnPause();
                controls = 0;
            }
            else
            {
                controlsCanvas.gameObject.SetActive(true);
                Time.timeScale = 0;
                properCanvas.GetComponent<AudioSource>().Pause();
                background.GetComponent<AudioSource>().Pause();
                controls = 1;
            }
        }



    }

    public void incrementHits()
    {
        hits += 1;
        addHit();
    }

    public int getHits()
    {
        return hits;
    }

    public int hasRifle()
    {

        return hasrifle;
    }

    public void setRifle()
    {

        hasrifle = 1;
    }

    public void setHits(int num)
    {
        hits=num;
    }

    public void incrementShots()
    {

        shots += 1;
    }

    public int getShots()
    {

        return shots;
    }

    public void setShots(int num)
    {
        shots = num;
    }

    public float getTimeLeft()
    {

        return timeLeft;
    }

    public int getDistanceHits()
    {

        return dHits;
    }

    public void setDistanceHits(int dhits)
    {

        dHits=dhits;
    }

    public void incrementDistanceHits()
    {

        dHits++;
    }

    public void recordPlayerDistance()
    {

        distance=transform.position.x;
    }

    public float getPlayerDistance()
    {
        return distance;
    }

    public int getFreeze()
    {

        return freeze;
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

    public void removeGun()
    {

        foreach (Transform child in transform)
        {
            if (child.name == "ToyGun")
            {
                Destroy(child.gameObject);
                createRifle();                                
                return;
            }
        }
    }

    private void createRifle()
    {
        GameObject gun;
        gun = Instantiate(rifle, new Vector3(transform.position.x-2.0f, transform.position.y - 1.8f, transform.position.z), transform.rotation) as GameObject;
        gun.transform.parent = gameObject.transform;
    }

    public float getSlowDown()
    {

        return slowDown;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var collObject = collision.gameObject;

        if (collObject.name != "ToyGun" && collObject.name != "Rifle") return;

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
        if (extra == 2) return; /* Rapid shooter has already been activated! */

        extra = 2;
        GameObject message = Instantiate(rapid, fire.gameObject.transform.position, transform.rotation) as GameObject;

        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(134,0,204);

        Destroy(message, 1.2f);

    }

    public void distanceHitter()
    {
        if (freeze == 1) return;

        GameObject message = Instantiate(distanceShooter, transform.position, transform.rotation) as GameObject;

        Destroy(message, 1.2f);

    }

    public void deadShot()
    {
        GameObject message = Instantiate(dead, target.transform.position, target.transform.rotation) as GameObject;
        hasrifle = 1;        
        Destroy(message, 1.5f);
    }

    public void wallRemover()
    {
        GameObject message = Instantiate(remover, wall.gameObject.transform.position, transform.rotation) as GameObject;        
        Destroy(message, 1.5f);
    }

    public void tryFreeze()
    {
        if (hitTimes.Count < 5) return;

        var length = hitTimes.Count;

        for (int i = 0; i<length-4; i++)
        {
            if ((hitTimes[i + 4] - hitTimes[i]) > -8.0f)
            {
                hitTimes.Clear();
                freeze = 1;
                freezeTime = Time.time;
                wallRemover();
                return;
            }
            else hitTimes.Remove(hitTimes[i]);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)  
    {         
        var collObject = collider.gameObject;
        Destroy(collObject);

        shake.StartShake(0.5f, 0.2f);

        int points = calculatePoints(transform.position, true, fire.getScaleFactor());
        fire.initializeMessage(points, this);
        
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
            if (child.name == "ToyGun" || child.name == "Rifle") return true;            
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

    public void shakeEnd()
    {

        if (timeLeft<11.1f && (timeLeft-System.Math.Truncate(timeLeft))<0.03f) shake.StartShake(0.2f, 0.08f);
    }

    void endGame()
    {
        SceneManager.LoadScene("Main Menu");
        boom.GetComponent<AudioSource>().Play();
        var i = 0;

        PlayerPrefs.SetInt("lastScore", (int)getPoints());

        while (i < 5 && (int)getPoints()>PlayerPrefs.GetInt(prefArray[i], 0))
        {
            if (i != 0) PlayerPrefs.SetInt(prefArray[i - 1], PlayerPrefs.GetInt(prefArray[i]));

            PlayerPrefs.SetInt(prefArray[i], (int)getPoints());
            i++;
        }    
    }

    public void addHit()
    {
        if (freeze == 1) return;
        float hitTime = timeLeft;
        hitTimes.Add(hitTime);
    }
}
