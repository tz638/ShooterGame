using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour {

    public Text text;

    public void startAgain()
    {

        SceneManager.LoadScene("Level1");
    }

    public void resetHighScore()
    {

        PlayerPrefs.SetInt("HighScore", 0);
        text.gameObject.SetActive(true);
    }

    public void loadRules()
    {

        SceneManager.LoadScene("Rules");
    }

    public void goBack()
    {

        SceneManager.LoadScene("Game Start");
    }
}
