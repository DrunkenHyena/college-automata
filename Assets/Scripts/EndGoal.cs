using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGoal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    /// <summary>
    /// Collision Detection function.
    /// When the player enters the end goal the game will end
    /// and load back the main menu.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter (Collider other)
    {
        Debug.Log("In Collider");
        SceneManager.LoadScene(0);
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(-1, 2, -3 * Time.deltaTime);	
	}
}
