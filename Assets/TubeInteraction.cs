using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeInteraction : MonoBehaviour {

    public GameObject toInteractWith;
    public static Vector3 temp = new Vector3(0, 1.5f, 0);
    

    public float GazeTime = 3f;
    private float Timer;

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
        if(gazedAt == false)
        {
            Timer = 0;
        }
        if (Timer >= GazeTime)
        {
            toInteractWith.transform.position += temp;
            Timer = 0f;
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
