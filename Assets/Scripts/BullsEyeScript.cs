using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BullsEyeScript : MonoBehaviour {
    
    private PlayerScript character;
    private FireScript fire;
    

    // Use this for initialization
    void Start () {

        character = (PlayerScript) FindObjectOfType(typeof(PlayerScript));
        fire = (FireScript)FindObjectOfType(typeof(FireScript));
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        
        var coll = collision.gameObject;
        if (coll.name != "ToyBullet(Clone)") return;
        
        character.playHappySound();

        GameObject target = GameObject.FindGameObjectWithTag("Target");
        GameObject bullseye = GameObject.FindGameObjectWithTag("BE");
        
        int points = character.calculatePoints(character.transform.position, false, fire.getScaleFactor());
        Debug.Log(points);
        fire.initializeMessage(points, character);
        Destroy(coll);
        Destroy(target);
        Destroy(bullseye);
        fire.initializeTarget();        
    } 
}
