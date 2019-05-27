using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BadControllerWavefront : MonoBehaviour {

	public GameObject controller;

	public string toInit="i",toFinal = "1";

	public List<List<string>> list = new List<List<string>> ();

	// Use this for initialization
	public float velocity=9f;

	public List<Vector2> routes;

	public float myPosX,myPosY;
	public float otherPosX,otherPosY;

	public GameObject otherGame;

	public bool newSearch=false;

	public GameObject redParticle;
	public GameObject blueParticle;
	public GameObject purpleParticle;

	void Start(){
		controller = GameObject.Find ("Controller");

		otherGame = controller.GetComponent<GetDataBase> ().followGameObject();

		//Vector2 me = new Vector2 (controller.GetComponent<GetDataBase> ().initialPosX3, controller.GetComponent<GetDataBase> ().initialPosY3);
		//Vector2 other = new Vector2 (controller.GetComponent<GetDataBase> ().initialPosX1, controller.GetComponent<GetDataBase> ().initialPosY1);

		myPosX = controller.GetComponent<GetDataBase> ().initialPosX3;
		myPosY = controller.GetComponent<GetDataBase> ().initialPosY3;



		//Debug.Log ("IniciaLLLLLLLLLLLLL: "+me.ToString () + " " + other.ToString ());

		//search (me, other);
		try{
			search (new Vector2 (myPosX, myPosY), new Vector2(otherGame.GetComponent<Character> ().posX,otherGame.GetComponent<Character> ().posY));
		}catch{
			delete (gameObject);
		}

	}
	public void delete(GameObject toDelet){
		GameObject aux;
		if (toDelet.name.Contains ("Character2")) {
			aux = purpleParticle;
		} else {
			if (toDelet.name.Contains ("Character")) {
				aux = blueParticle;
			} else {
				aux = redParticle;
			}
		}
		var o = Instantiate (aux, toDelet.transform.position, Quaternion.identity);
		Destroy (toDelet.gameObject);
		Destroy (o, 2f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player"){
			//Debug.Log ("Explota");
			//other.GetComponent<ParticleSystem> ().Emit (100);

			//other.GetComponent<SpriteRenderer>().enabled=false;
			//GetComponent<SpriteRenderer>().enabled=false;

			//GetComponent<ParticleSystem> ().Emit (100);

			delete (other.gameObject);
			controller.GetComponent<OtherControlls> ().Gameover ();
			delete (gameObject);
		}
	}

	IEnumerator moveX(float horizontal, float auxV){
		
		float increase = 0.1f / auxV;

		for (int i = 0; i < auxV; i++) {
			transform.position += new Vector3 (horizontal*increase, 0, 0);
			yield return null;
		}
	}
	IEnumerator moveY(float vertical, float auxV){  

		float increase = 0.1f / auxV;

		for (int i = 0; i < auxV; i++) {
			transform.position += new Vector3 (0, vertical*increase, 0);
			yield return null;
		}
	}

	void nSxT(){
		try{
			search (new Vector2 (myPosY, myPosX), new Vector2 (otherGame.GetComponent<Character> ().posX, otherGame.GetComponent<Character> ().posY));
		}catch{
			delete (gameObject);
		}
	}

	IEnumerator goToGood(){
		float auxV = velocity;
		int x = (int)routes [0].x, y = (int)routes [0].y;
		for (int i = 1; i < routes.Count; i++) {
			try{
				if ((otherPosY != otherGame.GetComponent<Character> ().posY) || (otherPosX != otherGame.GetComponent<Character> ().posX)) {
					nSxT();
					break;
					}
			}catch{
				delete (gameObject);
			}
			int auxx = (x - (int)routes [i].x);
			int auxy = (y - (int)routes [i].y);

			//Debug.Log ("Moviendo: " + i.ToString ());

			if (auxx != 0) {
				yield return StartCoroutine (moveY (auxx, auxV));
			} else {
				yield return StartCoroutine (moveX (-auxy, auxV));
			}

			myPosX = routes [i].y;
			myPosY = routes [i].x;

			x = (int)routes [i].x;
			y = (int)routes [i].y;

			if (i == (routes.Count - 1)) {
				try{
					delete (otherGame);
					controller.GetComponent<OtherControlls> ().Gameover ();
					delete (gameObject);
				}catch{
					delete (gameObject);
				}
			}
		}
		//nSxT ();
	}

	List<List<string>> copyOfList(List<List<string>> l){
		List<List<string>> newlist = new List<List<string>> ();

		for (int i = 0; i < l.Count; i++) {
			List<string> aux = new List<string> ();
			for (int j = 0; j < l [i].Count; j++) {
				aux.Add (l [i] [j]);
			}
			newlist.Add (aux);
		}

		return newlist;
	}

	public void search(Vector2 init,Vector2 final){
		
		controller = GameObject.Find ("Controller");

		list.Clear ();
		list = copyOfList (controller.GetComponent<GetDataBase> ().list);
		//controller.GetComponent<GetDataBase>().list.ForEach(c => list.Add(c));

		//showList ();

		otherPosX = final.x;
		otherPosY = final.y;


		if (doing (init, final)) {
			routes = organizeRoutes ((int)init.x, (int)init.y);
			StartCoroutine (goToGood ());
		} else {
			Debug.Log ("Falló la busqueda");
			try{
				delete (otherGame);
				controller.GetComponent<OtherControlls> ().Gameover ();
				delete (gameObject);
			}catch{
				delete (gameObject);
			}
		}
	}

	int stringToInt(string text){
		int x = 0;
		Int32.TryParse(text, out x);
		return x;
	}

	List<Vector2> organizeRoutes(int x,int y){
		List<Vector2> routes = new List<Vector2> ();

		Vector2 r = new Vector2 (x, y);
		int cont = stringToInt (list [x] [y]);

		while (cont > 0) {
			x = (int)r.x;
			y = (int)r.y;

			r=new Vector2 (x, y);

			routes.Add (r);

			try{
				if(list[x][y-1]==(cont-1).ToString()){
					r=new Vector2 (x, y-1);
				}else{
					//Inducir un error
					list[list.Count+10][y]="sgsd";
				}
				
			}catch{
				try{
					if(list[x][y+1]==(cont-1).ToString()){
						r=new Vector2 (x, y+1);
					}else{
						//Inducir un error
						list[list.Count+10][y]="sgsd";
					}

				}catch{
					try{
						if(list[x-1][y]==(cont-1).ToString()){
							r=new Vector2 (x-1, y);
						}else{
							//Inducir un error
							list[list.Count+10][y]="sgsd";
						}

					}catch{
						try{
							if(list[x+1][y]==(cont-1).ToString()){
								r=new Vector2 (x+1, y);
							}else{
								//Inducir un error
								list[list.Count+10][y]="sgsd";
							}

						}catch{
						}
					}

				}
				
			}
			cont-=1;
		}
		return routes;
	}

	int newman (int x, int y){
		bool enable = false;
		////////////////////////////////////////////////////////////
		try{
			if(list[x][y-1]==toInit){
				list[x][y-1]=(stringToInt(list[x][y])+1).ToString();
				return 1;
			}else if(list[x][y-1]==" "){
				list[x][y-1]=(stringToInt(list[x][y])+1).ToString();
				//Debug.Log("Algo pas2 "+list[x][y-1].ToString());
				enable=true;
			}
		}catch{}
		////////////////////////////////////////////////////////////
		try{
			if(list[x][y+1]==toInit){
				list[x][y+1]=(stringToInt(list[x][y])+1).ToString();
				return 1;
			}else if(list[x][y+1]==" "){
				list[x][y+1]=(stringToInt(list[x][y])+1).ToString();
				//Debug.Log("Algo pas2 "+list[x][y+1].ToString());
				enable=true;
			}
		}catch{}
		////////////////////////////////////////////////////////////
		try{
			if(list[x-1][y]==toInit){
				list[x-1][y]=(stringToInt(list[x][y])+1).ToString();
				return 1;
			}else if(list[x-1][y]==" "){
				list[x-1][y]=(stringToInt(list[x][y])+1).ToString();
				//Debug.Log("Algo pas2 "+list[x-1][y].ToString());
				enable=true;
			}
		}catch{}
		////////////////////////////////////////////////////////////
		try{
			if(list[x+1][y]==toInit){
				list[x+1][y]=(stringToInt(list[x][y])+1).ToString();
				return 1;
			}else if(list[x+1][y]==" "){
				list[x+1][y]=(stringToInt(list[x][y])+1).ToString();
				//Debug.Log("Algo pas2 "+list[x+1][y].ToString());
				enable=true;
			}
		}catch{}

		if (!enable)
			return 2;
		return 0;
	}

	void showList(){
		Debug.Log ("Imprimiendo lista");
		for (int i = 0; i < list.Count; i++) {
			string var1 = list [i] [0].ToString () + ", " + list [i] [1].ToString () + ", " + list [i] [2].ToString () + ", " + list [i] [3].ToString () + ", " + list [i] [4].ToString () + ", " + list [i] [5].ToString () + ", " + list [i] [6].ToString () + ", " + list [i] [7].ToString () + ", " + list [i] [8].ToString () + ", " + list [i] [9].ToString () + ", " + list [i] [10].ToString () + ", " + list [i] [11].ToString () + ", " + list [i] [12].ToString () + ", " + list [i] [13].ToString ();
			string var2 = list [i] [14].ToString () + ", " + list [i] [15].ToString () + ", " + list [i] [16].ToString () + ", " + list [i] [17].ToString () + ", " + list [i] [18].ToString () + ", " + list [i] [19].ToString () + ", " + list [i] [20].ToString () + ", " + list [i] [21].ToString () + ", " + list [i] [22].ToString () + ", " + list [i] [23].ToString () + ", " + list [i] [24].ToString () + ", " + list [i] [25].ToString () + ", " + list [i] [26].ToString () + ", " + list [i] [27].ToString ();
			string var3 = list [i] [28].ToString ()+", "+list [i] [29].ToString ();

			Debug.Log (var1 + ", " + var2 + ", " + var3);
		}
	}

	bool doing(Vector2 init,Vector2 final){
		list [ (int)init.x] [ (int)init.y] = toInit;
		list [(int)final.x] [(int)final.y] = toFinal;

		//showList ();


		int counter = 1;
		int auxG = newman ((int)final.x, (int)final.y);
		counter += 1;

		while (true && auxG != 2) {
			bool enable = false, arrived = false;

			for (int i = 0; i < list.Count; i++) {
				for (int j = 0; j < list [0].Count; j++) {
					if (list [i] [j] == counter.ToString ()) {
						int aux = newman (i, j);
						if (aux != 2)
							enable = true;
						if (aux == 1)
							arrived = true;
					}
				}
			}

			if (!enable) {
				Debug.Log ("No hay ruta");
				return false;
			}
			if (arrived) {
				//showList ();
				//Debug.Log("Llega");
				return true;
			}

			counter += 1;
			//Debug.Log ("Counter: " + counter.ToString ());
				
		}
		Debug.Log ("Error indefinido");
		return false;

	}
}
