using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BullsEyeScript : MonoBehaviour {
    
    private PlayerScript character;
    private FireScript fire;
    private WallScript[] walls;    

    // Use this for initialization
    void Start () {
        
        character = (PlayerScript) FindObjectOfType(typeof(PlayerScript));
        fire = (FireScript)FindObjectOfType(typeof(FireScript));
        walls = (WallScript[])FindObjectsOfType(typeof(WallScript));
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

        /* Check if necessary to activate freeze */
        character.tryFreeze();

        /* Check if necessary to activate Dead Shot */
        if (character.getHits() ==7 && character.getHits() == character.getShots() && character.hasRifle()==0)
        {
            character.removeGun();
            character.deadShot();
        }

        /* Check if necessary to activate Distance Hitter */
        if (character.getDistanceHits() == 10)
        {
            for (int i=0; i<walls.Length; i++)
            {
                WallScript wall = walls[i];
                wall.shrink();
            }
            
            character.distanceHitter();
            character.setDistanceHits(0);
        }        

        int points = character.calculatePoints(character.transform.position, false, fire.getScaleFactor());
        fire.initializeMessage(points, character);
        Destroy(coll);

        fire.resetTime();        
    } 
}
