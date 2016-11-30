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
        PlayerPrefs.SetInt("HighScore2", 0);
        PlayerPrefs.SetInt("HighScore3", 0);
        PlayerPrefs.SetInt("HighScore4", 0);
        PlayerPrefs.SetInt("HighScore5", 0);
        text.gameObject.SetActive(true);
    }

    public void loadRulesFromStart()
    {
        SceneManager.LoadScene("Rules");
    }

    public void loadRules()
    {
        SceneManager.LoadScene("Rules");
    }

    public void loadSpecial()
    {
        SceneManager.LoadScene("Special");
    }

    public void loadControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void loadHSTable()
    {

        SceneManager.LoadScene("HSTable");
    }

    public void goBack()
    {
        SceneManager.LoadScene("Game Start");
    }

    public void mainMenuFromHS()
    {
        SceneManager.LoadScene("Main Menu");
        PlayerPrefs.SetInt("FromHS", 1);
    }
}
