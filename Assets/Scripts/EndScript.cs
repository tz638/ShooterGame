using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour {

    public UnityEngine.UI.Text highScore;
    //private PlayerScript player;

	// Use this for initialization
	void Start () {

        //player= (PlayerScript)FindObjectOfType(typeof(PlayerScript));
        highScore.text = "Your highscore is " + PlayerPrefs.GetInt("HighScore")+".";

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
