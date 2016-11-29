using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HSScript : MonoBehaviour {

    public Text hs1;
    public Text hs2;
    public Text hs3;
    public Text hs4;
    public Text hs5;

    // Use this for initialization
    void Start () {

        hs1.text = "1. " + PlayerPrefs.GetInt("HighScore");
        hs2.text = "2. " + PlayerPrefs.GetInt("HighScore2");
        hs3.text = "3. " + PlayerPrefs.GetInt("HighScore3");
        hs4.text = "4. " + PlayerPrefs.GetInt("HighScore4");
        hs5.text = "5. " + PlayerPrefs.GetInt("HighScore5");

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
