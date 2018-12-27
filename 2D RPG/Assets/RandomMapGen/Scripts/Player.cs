using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private MapMovementController moveController;

	// Use this for initialization
	void Start () {
        moveController = GetComponent<MapMovementController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Debug.Log("Up key");
        }
		
	}
}
