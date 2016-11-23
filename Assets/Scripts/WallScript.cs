using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

    private FireScript fire;

	// Use this for initialization
	void Start () {

        fire = (FireScript)FindObjectOfType(typeof(FireScript));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        var coll = collision.gameObject;
        if (coll.name != "ToyBullet(Clone)") return;

        coll.transform.localRotation = new Quaternion(0, 180.0f, 0, 0);
        
        coll.GetComponent<Rigidbody2D>().AddForce(new Vector3(fire.bulletForwardForce/2.5f, 0, 0));

        Destroy(coll, 2);
    }

    public void shrink()
    {
            GetComponent<Animator>().Play("Wall Animation");
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.8f, transform.localScale.z);   
    }
       
}
