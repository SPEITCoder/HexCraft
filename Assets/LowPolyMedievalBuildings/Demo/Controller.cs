using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public GameObject spring;
	public GameObject winter;
	private bool seasonState;
	private bool springState;
	private bool winterState;
	// Use this for initialization
	void Start () {
		seasonState = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("e") && seasonState == false) 
		{
			seasonState = true;
		}

		if (Input.GetKeyUp ("q") && seasonState == true) 
		{
			seasonState = false;
		}

		if (seasonState == false) {
			spring.SetActive(true);
			winter.SetActive(false);
		} 
		if (seasonState == true)
		{
			spring.SetActive(false);
			winter.SetActive(true);
		}
	}
}
