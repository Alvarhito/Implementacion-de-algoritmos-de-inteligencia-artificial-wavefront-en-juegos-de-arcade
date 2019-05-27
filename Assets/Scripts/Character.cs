using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	public int velocity =1;
	public float speed=0.1f;
	string valueBlock="x";

	public float stateX=0,stateY=0;

	public int posX,posY;

	bool canDelete=false;

	float myTime=0;
	public int morePoints=0;

	bool move=false;

	public bool stop=false;

	public string numberCharacter = "";

	GameObject controller;
	// Use this for initialization
	void Start () {
		
		controller = GameObject.Find ("Controller");
		if (numberCharacter == "") {
			posX = controller.GetComponent<GetDataBase> ().initialPosX1;
			posY = controller.GetComponent<GetDataBase> ().initialPosY1;
		} else {
			posX = controller.GetComponent<GetDataBase> ().initialPosX2;
			posY = controller.GetComponent<GetDataBase> ().initialPosY2;
		}
		canDelete = true;

		lessPoints ();

	}
	
	// Update is called once per frame
	void lessPoints (){
		morePoints = morePoints > 0 ? morePoints - (controller.GetComponent<OtherControlls> ().gifNowNumber+1): 0;
		Invoke ("lessPoints", 3f);
	}
	void Update () {
		if (!stop) {
			float x = Input.GetAxis ("Horizontal" + numberCharacter);
			x = x > 0 ? 1 : (x < 0 ? -1 : 0);
			float y = Input.GetAxis ("Vertical" + numberCharacter);
			y = y > 0 ? 1 : (y < 0 ? -1 : 0);

			stateX = x != 0 ? x : y != 0 ? 0 : stateX;
			stateY = y != 0 ? y : x != 0 ? 0 : stateY;

			if (stateX != 0 && !move) {
				try {
					//Debug.Log((absolute((int)(transform.position.y / 0.1f))).ToString()+" "+(absolute((int)(transform.position.x / 0.1f)) + (int)stateX ).ToString()+" "+stateX.ToString()+" "+(controller.GetComponent<GetDataBase> ().list [absolute((int)(transform.position.y / 0.1f))] [absolute((int)(transform.position.x / 0.1f)) + (int)stateX]).ToString());
					if (controller.GetComponent<GetDataBase> ().list [posX] [posY + (int)stateX] != valueBlock) {
						float horizontal = stateX * speed;
						posY += (int)stateX;
						//transform.transform.position += new Vector3 (horizontal, 0, 0);
						move = true;
						StartCoroutine (moveX (horizontal));
					}
				} catch {
					Debug.Log ("Error...");
				}

			} else if (stateY != 0 && !move) {
				try {
					if (controller.GetComponent<GetDataBase> ().list [posX - (int)stateY] [posY] != valueBlock) {
						float vertical = stateY * speed;
						posX -= (int)stateY;
						move = true;
						//transform.transform.position += new Vector3 (0, vertical,0);
						StartCoroutine (moveY (vertical));
					}
				} catch {
					Debug.Log ("Error...");
				}
			}
		}
	}

	int absolute(int value){
		return (value < 0 ? value * -1 : value);
	}
	float absoluteFloat(float value){
		return (value < 0 ? value * -1 : value);
	}

	IEnumerator moveX(float horizontal){
		float aux = velocity;

		float increase = 0.1f / aux;

		for (int i = 0; i < aux; i++) {
			transform.position += new Vector3 (horizontal*10*increase, 0, 0);
			yield return null;
		}
		move = false;
	}
	IEnumerator moveY(float vertical){
		float aux = velocity;
	
		float increase = 0.1f / aux;

		for (int i = 0; i < aux; i++) {
			transform.position += new Vector3 (0, vertical*10*increase, 0);
			yield return null;
		}
		move = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Gift" && canDelete){
			if ((Time.time - myTime) <= 2f) {
				morePoints += controller.GetComponent<OtherControlls> ().gifNowNumber+1;
			}
			myTime = Time.time;
				
			controller.GetComponent<OtherControlls> ().gifts.Remove (other.gameObject);
			Destroy (other.gameObject);
			controller.GetComponent<OtherControlls> ().increasePoints ((5*(controller.GetComponent<OtherControlls> ().gifNowNumber+1))+morePoints,gameObject.name);

		}
	}
}
