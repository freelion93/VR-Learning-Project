using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishInteraction : MonoBehaviour {

    public float GazeTime = 3f;
    private float Timer;
    public string level;

    private bool gazedAt;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(gazedAt == true)
        {
            Timer += Time.deltaTime;
        }
        if (Timer >= GazeTime)
        {
            SceneManager.LoadScene(1);
            GetComponent<Collider>().enabled = false;
        }
    }

    public void PointerEnter()
    {
        gazedAt = true;
        Debug.Log("Enter");
    }

    public void PointerExit()
    {
        gazedAt = false;
        Debug.Log("Quit");
    }

}
