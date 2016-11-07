using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class FireScript : MonoBehaviour {

    public GameObject BulletEmitter;
    public GameObject bullet;
    public GameObject target;
    public float bulletForwardForce;
    private DateTime time;
    private double scaleFactor;
    public GameObject text;
    public GameObject text2;
    private PlayerScript player;
    private Vector3 originalPosition;
    public GameObject bullsEye;
    

    // Use this for initialization
    void Start () {

        player = (PlayerScript)FindObjectOfType(typeof(PlayerScript));

        scaleFactor = 1;  /* Not only used for shrinking, but is also a global variable to boost points with time */

        time = DateTime.Now;

        initializeTarget();
    }
	
	// Update is called once per frame
	void Update () {

        scaleFactor = 1 - (60 - player.timeLeft) * 800 / 60000;     /* Math was done to determine the rate of shrinking of the target */
        shoot();
	}

    public DateTime getStartTime()
    {

        return time;
    }

    public double getScaleFactor()
    {

        return scaleFactor;
    }

    /* Calculate time that has passed between the game start and the point at which the target needs to be initialized 
     * Depending on time, the target wil shrink
     * But players also receive a boost, the smaller the target the more their hits are worth 
     */

    public void initializeTarget()
    {
        GameObject tempTarget, bullseye;
        System.Random random = new System.Random();
        
        float y1 = (2.12f * (float)random.NextDouble()) - 0.98f;
        float y2 = (-1.82f * (float)random.NextDouble()) - 2.08f;
        float[] array = { y1, y2 };
        float y = array[(int)Mathf.Ceil(player.timeLeft) % 2];

        tempTarget = Instantiate(target, new Vector3(-5.5f, y, 0.0f), target.transform.rotation) as GameObject;
        
        bullseye = Instantiate(bullsEye, new Vector3(tempTarget.transform.position.x, tempTarget.transform.position.y+1-(float)scaleFactor, 0), tempTarget.transform.rotation) as GameObject;
        bullseye.transform.localScale = new Vector3(bullsEye.transform.localScale.x, (float)scaleFactor * bullsEye.transform.localScale.y, 0);
    }

    /* https://www.youtube.com/watch?v=FD9HZB0Jn1w */
    public void shoot()
    {

        if (this.transform.parent == null) return;

        if (Input.GetButtonDown("Shoot"))
        {

            AudioSource sound = GetComponent<AudioSource>();
            sound.Play();
            GetComponent<Animator>().Play("Gun Animation");            

            GameObject temporaryBullet;
            temporaryBullet = Instantiate(bullet, BulletEmitter.transform.position, BulletEmitter.transform.rotation) as GameObject;

            Rigidbody2D temp = temporaryBullet.GetComponent<Rigidbody2D>();
            temp.AddForce(new Vector3(-bulletForwardForce, 0, 0));
            temp.gravityScale = 0;
            Destroy(temporaryBullet, 3.0f);

        }
    }

    public void initializeMessage(int points, PlayerScript player)
    {
        GameObject message;

        if (points>0)
        {
            message = Instantiate(text, transform.position, transform.rotation) as GameObject;
            message.GetComponent<Text>().text = "+" + points.ToString();
        }

        else
        {
            message = Instantiate(text2, transform.position, transform.rotation) as GameObject;
            message.GetComponent<Text>().text = points.ToString();
        }        

        Destroy(message, 1);
        player.increasePoints(points);
    }
}
