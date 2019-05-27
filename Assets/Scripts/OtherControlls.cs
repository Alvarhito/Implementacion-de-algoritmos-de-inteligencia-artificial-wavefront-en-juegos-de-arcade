using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OtherControlls : MonoBehaviour {

	public List<GameObject> giftsPref;
	public List<GameObject> particles;
	GameObject giftNow;
	public int gifNowNumber=0;
	string valueBlock="x";

	public int numberGifts=5;
	public float updateGiftsTime=5f;

	public Text pointText;
	public Text pointText2;

	public List<GameObject> gifts;

	public int points=0;
	public int points2=0;
	public GameObject[] o;

	public GameObject lostImage;

	public GameObject restartButton;

	public GameObject congratsImage;

	public GameObject canvas;

	// Use this for initialization
	void Awake(){
		giftNow = giftsPref[gifNowNumber];
		restartButton.GetComponent<Button> ().onClick.AddListener (rest);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Gameover(){
		Invoke ("verify", 0.5f);
	}
	void rest(){
		SceneManager.LoadScene ("Main");
	}
	void congrats (){

		GameObject[] o = GameObject.FindGameObjectsWithTag ("Bad");

		for (int i = 0; i < o.Length; i++) {
			o [i].GetComponent<BadControllerWavefront> ().delete (o [i]);
		}
		o = GameObject.FindGameObjectsWithTag ("Player");

		for (int i = 0; i < o.Length; i++) {
			o [i].GetComponent<Character> ().stop = true;
		}
			
		congratsImage.SetActive (true);
		restartButton.GetComponent<Text>().text="New game";
		restartButton.SetActive (true);
		putParticles ();
	}

	void putParticles (){
		var o = Instantiate (particles [Random.Range (0, particles.Count - 1)], new Vector3 (Random.Range (0.1f, 2.8f), Random.Range (0.1f, -1.7f), 90), Quaternion.identity);

		//o.transform.SetParent (canvas.transform);
		//o.transform.localScale= new Vector2 (0.04977158f, 0.04977158f);
		Destroy (o, 2f);
		Invoke ("putParticles", 0.5f);
	}

	void verify(){
		o = GameObject.FindGameObjectsWithTag ("Player");

		//Debug.Log ("Test Game Over: " + o.Length.ToString ());
		if (o.Length == 0) {
			Debug.Log ("Game over");
			lostImage.SetActive (true);
			restartButton.SetActive (true);
			//
		}
	}

	void moreSpeedGood(){
		GameObject[] o = GameObject.FindGameObjectsWithTag ("Player");

		for (int i = 0; i < o.Length; i++) {
			o [i].GetComponent<Character> ().velocity = 8;
		}
	}
	void lessSpeed(){
		GameObject[] o = GameObject.FindGameObjectsWithTag ("Bad");

		for (int i = 0; i < o.Length; i++) {
			o [i].GetComponent<BadControllerWavefront> ().velocity -= 1;
			if (o [i].GetComponent<BadControllerWavefront> ().velocity <= 9f)
				o [i].GetComponent<BadControllerWavefront> ().velocity = 10f;
		}
	}

	public void increasePoints(int m, string name){
		if (name == "Character(Clone)") {
			points += points > 9998 ? 0 : m;
			pointText.text = "Puntos: " + points.ToString ();
		} else {
			points2 += points2 > 9998 ? 0 : m;
			pointText2.text = "Puntos: " + points2.ToString ();
		}
		numberGifts -= 1;
		if (numberGifts == 0){
			numberGifts = 15;
			updateGiftsTime -= (updateGiftsTime * 0.2f);
			lessSpeed ();
			gifNowNumber = (giftsPref.Count-1) > gifNowNumber ? (gifNowNumber + 1) : gifNowNumber;

			if (gifNowNumber==(giftsPref.Count-1)) {
				numberGifts = 0;
				congrats ();
			}

			giftNow = giftsPref[gifNowNumber];
			if(gifNowNumber==2)
				moreSpeedGood();
		}

	}
	public void putGifts(){
		List<List<string>> list = GetComponent<GetDataBase> ().list;

	
		for(int x=0;x<numberGifts;x++){
			string aux;
			int i, j;
			do{
				i=Random.Range(0,list.Count-2);
				j=Random.Range(0,list[0].Count-2);
				aux=list[i][j];
				if(aux!=valueBlock){
					var o = Instantiate (giftNow,new Vector2 (j * 0.1f, i * -0.1f),Quaternion.identity);
					gifts.Add(o);
				}
			}while(aux==valueBlock);
				
		}

		Invoke ("updateGifts", updateGiftsTime);
	}

	void updateGifts(){
		while(gifts.Count>0) {
			GameObject aux = gifts [gifts.Count-1];
			aux.GetComponent<Animator> ().SetTrigger ("Disappear");
			gifts.RemoveAt (gifts.Count - 1);
			Destroy (aux,2f);			
		}
		//gifts.Clear ();
		Invoke ("putGifts", 1.6f);
	}
}
