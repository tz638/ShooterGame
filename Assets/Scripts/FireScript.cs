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
    private float targetTime;
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
        targetTime = 0;
        initializeTarget();

    }
	
	// Update is called once per frame
	void Update () {
        
        scaleFactor = 1 - (60 - player.getTimeLeft()) * 800 / 60000;     /* Math was done to determine the rate of shrinking of the target */
        shoot();

        targetTime += Time.deltaTime;

        if (Math.Floor(targetTime) % 4 == 0 && targetTime - Math.Truncate(targetTime) < 0.03f && player.getTimeLeft()<60.0f)
        {
            initializeTarget();
        }                   
    }

    public DateTime getStartTime()
    {

        return time;
    }

    public double getScaleFactor()
    {

        return scaleFactor;
    }

    public void resetTime()
    {
        targetTime = 0;
    }

    /* Calculate time that has passed between the game start and the point at which the target needs to be initialized 
     * Depending on time, the target wil shrink
     * But players also receive a boost, the smaller the target the more their hits are worth 
     */

    public void initializeTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        GameObject[] bullseyes = GameObject.FindGameObjectsWithTag("BE");

        for (int i = 0; i < targets.Length; i++)
        {
            GameObject target1 = targets[i];
            Destroy(target1);
        }
        for (int i = 0; i < bullseyes.Length; i++)
        {
            GameObject bullseye1 = bullseyes[i];
            Destroy(bullseye1);
        }

        GameObject tempTarget, bullseye;
        System.Random random = new System.Random();
        
        float[] array = { 1.28f, 1, 0.7f, -1.02f, -1.32f, -1.71f, -4.53f, -4.23f, -4.09f };
        float y = array[random.Next(0,array.Length)];

        tempTarget = Instantiate(target, new Vector3(-5.4f, y, 0.0f), target.transform.rotation) as GameObject;
        
        bullseye = Instantiate(bullsEye, new Vector3(tempTarget.transform.position.x, tempTarget.transform.position.y+1-(float)scaleFactor, 0), tempTarget.transform.rotation) as GameObject;
        bullseye.transform.localScale = new Vector3(bullsEye.transform.localScale.x, (float)scaleFactor * bullsEye.transform.localScale.y, 0);
    }

    /* https://www.youtube.com/watch?v=FD9HZB0Jn1w */
    public void shoot()
    {

        if (this.transform.parent == null) return;

        if (Input.GetButtonDown("Shoot"))
        {
            if (player.getHits() != player.getShots())
            {

                player.setHits(0);
                player.setShots(0);
            }

            player.incrementShots();
            player.recordPlayerDistance();

            AudioSource sound = GetComponent<AudioSource>();
            sound.Play();
            if (name=="ToyGun") GetComponent<Animator>().Play("Gun Animation");    
            else GetComponent<Animator>().Play("Rifle Animation");

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
            message = Instantiate(text, transform.parent.position, transform.parent.rotation) as GameObject;
            message.GetComponent<Text>().text = "+" + points.ToString();
        }

        else
        {
            message = Instantiate(text2, transform.parent.position, transform.parent.rotation) as GameObject;
            message.GetComponent<Text>().text = points.ToString();
        }        

        Destroy(message, 1);
        player.increasePoints(points);
    }
}
