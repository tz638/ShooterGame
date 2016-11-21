using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BullsEyeScript : MonoBehaviour {
    
    private PlayerScript character;
    private FireScript fire;
    private WallScript[] walls;

    private int rifle;
    

    // Use this for initialization
    void Start () {
        
        character = (PlayerScript) FindObjectOfType(typeof(PlayerScript));
        fire = (FireScript)FindObjectOfType(typeof(FireScript));
        walls = (WallScript[])FindObjectsOfType(typeof(WallScript));
        
        if (fire.name == "Rifle(Clone)") rifle = 1;
        else rifle = 0;        
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {        
        var coll = collision.gameObject;
        if (coll.name != "ToyBullet(Clone)") return;
        
        character.incrementHits();
        if (character.getPlayerDistance() > 5.0) character.incrementDistanceHits();

        character.playHappySound();

        if (character.getHits() > 15 && character.getHits() == character.getShots() && rifle == 0)
        {
            character.removeGun();
            character.deadShot();
            rifle = 1;
        }

        if (character.getDistanceHits() > 10 && character.shrink == 0)
        {

            for (int i=0; i<walls.Length; i++)
            {
                WallScript wall = walls[i];
                wall.shrink();
            }
            
            character.shrink = 1;
            character.distanceHitter();
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        GameObject[] bullseyes = GameObject.FindGameObjectsWithTag("BE");

        int points = character.calculatePoints(character.transform.position, false, fire.getScaleFactor());
        fire.initializeMessage(points, character);
        Destroy(coll);
        for (int i=0; i<targets.Length; i++)
        {
            GameObject target = targets[i];
            Destroy(target);
        }
        for (int i = 0; i < bullseyes.Length; i++)
        {
            GameObject bullseye = bullseyes[i];
            Destroy(bullseye);
        }

        fire.initializeTarget();
        
    } 
}
