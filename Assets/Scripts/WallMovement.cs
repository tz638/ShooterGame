using UnityEngine;
using System.Collections;
using System;

public class WallMovement : MonoBehaviour {
    
    public float down;

    // Use this for initialization
    void Start () {

        down = 0.05f;
    }
	
	// Update is called once per frame
	void Update () {
        
        System.Random random = new System.Random();
        double coeff = (0.4 * random.NextDouble() + 0.9);
                           
        // Y axis
        if (transform.position.y <= -2.6 || transform.position.y >= -0.36)
        {
            if (transform.position.y <= -2.6) transform.Translate(0,0.1f,0);

            else transform.Translate(0, -0.1f, 0);

            down *= -(float)coeff;

            if (down > 0.08 || down < -0.08)    /* Don't let the speed get out of hand */
            {
                bool positive = down > 0;
                if (positive) down = 0.05f;
                else down = -0.05f;
            }

            else if (down > 0 && down < 0.035) down = 0.05f;    /* Don't let the wall move too slowly */
            else if (down < 0 && down > -0.035) down = -0.05f;
        }       

        transform.Translate(down * Vector3.down);
    }

}
