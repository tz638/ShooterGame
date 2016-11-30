using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour {

    public UnityEngine.UI.Text highScore;

	// Use this for initialization
	void Start () {
        
        highScore.text = "Last score: " + PlayerPrefs.GetInt("lastScore") +". Highscore: "+ PlayerPrefs.GetInt("HighScore")+".";

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
