using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{

    private PlayerScript character;
    private GameObject message;
    private Text score;

    // Use this for initialization
    void Start()
    {

        character = (PlayerScript)FindObjectOfType(typeof(PlayerScript));
        score = character.score;

        this.transform.SetParent(score.transform.parent, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position!=score.transform.position)
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, score.transform.position.x, 0.02f), Mathf.Lerp(transform.position.y, score.transform.position.y, 0.02f), 0);
    }
}
    

