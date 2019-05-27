using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDataBase : MonoBehaviour {

	public TextAsset txtData;
	public GameObject block1;
	public GameObject block2;
	public GameObject block3;
	public GameObject block4;

	public GameObject character;
	public GameObject character2;
	public GameObject badCharacter;
	string valueBlock="x";

	public int initialPosX1,initialPosY1;
	public int initialPosX2,initialPosY2;
	public int initialPosX3,initialPosY3;

	public GameObject goodP1;
	public GameObject goodP2;

	int goodCount=0;

	public struct blockStruct{
		public GameObject o;
		public Vector3 r;
	};

	public int badCount=1;

	public List<List<string>> list = new List<List<string>> ();

	// Use this for initialization
	void Start ()
	{
		list = getData (txtData);
		render (list);

		//string aux;
		//int i, j;
		//do{
		//	i=Random.Range(0,list.Count-2);
		//	j=Random.Range(0,list[0].Count-2);
		//	aux=list[i][j];
		//	if(aux!=valueBlock)
		goodP1 = putCharacter (7, 26, character);
		//}while(aux==valueBlock);
			
		//do{
		//	i=Random.Range(0,list.Count-2);
		//	j=Random.Range(0,list[0].Count-2);
		//	aux=list[i][j];
		//	if(aux!=valueBlock)
		goodP2 = putCharacter (7, 2, character2);
		//}while(aux==valueBlock);

		//do{
		//	i=Random.Range(0,list.Count-2);
		//	j=Random.Range(0,list[0].Count-2);
		//	aux=list[i][j];
		//	if(aux!=valueBlock)
		for (int i = 0; i < badCount; i++) {
			Invoke ("putBads", (float)i);
		}
		//}while(aux==valueBlock);
			


		//Invoke ("startSearch", 10f);
		GetComponent<OtherControlls> ().putGifts ();
	}

	void putBads(){
		putCharacter (8, 14, badCharacter);
	}


	public GameObject followGameObject(){
		List<GameObject> goods = new List<GameObject> ();
		goods.Add (goodP1);
		goods.Add (goodP2);

		GameObject aux = goods [goodCount];
		if (goodCount == 1)
			goodCount = 0;
		else
			goodCount = 1;

		return aux;
			
	}

	// Update is called once per frame
	GameObject putCharacter(int i, int j,GameObject charac){
		if (charac.name == "Character") {
			initialPosX1 = i;
			initialPosY1 = j;
		} else if (charac.name == "Character2") {
			initialPosX2 = i;
			initialPosY2 = j;
		} else {
			initialPosX3 = i;
			initialPosY3 = j;
		}
		//Debug.Log (i.ToString() + " " + j.ToString ());
		Vector2 pos = new Vector2 (j * 0.1f, i * -0.1f);
		return Instantiate (charac, pos, Quaternion.identity).gameObject;
	}
	List<List<string>> getData(TextAsset data){
		
		string[] preData = data.text.Split ('\n');

		List<List<string>> list = new List<List<string>> ();

		for (int i = 0; i < preData.Length; i++) {
			list.Add (new List<string> (preData [i].Split (',')));
		}

		return list;
	}

	void render(List<List<string>> list){
		float x = 0, y = 0;
		float cont = 0.1f;
		for (int i = 0; i < list.Count; i++) {
			for (int j = 0; j < list[i].Count; j++) {
				if (list [i] [j] == valueBlock) {
					Vector2 pos = new Vector2 (x, y);

					blockStruct auxBlockStruct=getObject (list [((i + 1) > (list.Count - 1)) ? list.Count - 1 : (i + 1)] [j] == valueBlock, list [((i - 1) < 0) ? list.Count - 1 : (i - 1)] [j] == valueBlock, list [i] [((j + 1) > (list [i].Count - 1)) ? list [i].Count - 1 : (j + 1)] == valueBlock, list [i] [((j - 1) < 0) ? list [i].Count - 1 : (j - 1)] == valueBlock);
					var o=Instantiate (auxBlockStruct.o, pos, Quaternion.identity);
					o.transform.Rotate(auxBlockStruct.r);
					//return;
				}
				x+=cont;
			}
			x = 0;
			y-=cont;
		}
	}

	blockStruct getObject(bool d,bool u,bool r, bool l){
		//Debug.Log (d);
		//Debug.Log (u);
		//Debug.Log (r);
		//Debug.Log (l);
		blockStruct aux=new blockStruct();
		aux.o = block1;
		aux.r = Vector3.zero;

		//Debug.Log (((d ^ u) ^ (r ^ l)).ToString()+" "+d.ToString()+" "+u.ToString()+" "+r.ToString()+" "+l.ToString());
		//if ((d ^ u) && (r ^ l))
		//if ((d && !u && !r && !l) || (!d && u && !r && !l) || (!d && !u && r && !l) || (!d && !u && !r && l))
			//return (block2,Vector2.zero);
		if        (d && !u && !r && !l) {
			aux.o = block2;
			aux.r = new Vector3 (0, 0, 90);
		} else if (!d && u && !r && !l) {
			aux.o = block2;
			aux.r = new Vector3 (0, 0, -90);
		} else if (!d && !u && r && !l) {
			aux.o = block2;
			aux.r = new Vector3 (0, 0, -180);
		}else if  (!d && !u && !r && l) {
			aux.o = block2;
		}

		if ((d && !u && r && !l)) {
			aux.o = block3;
		} else if ((d && !u && !r && l)) {
			aux.o = block3;
			aux.r = new Vector3 (0, -180, 0);
		}else if ((!d && u && r && !l)) {
			aux.o = block3;
			aux.r = new Vector3 (-180, 0, 0);
		} else if ((!d && u && !r && l)) {
			aux.o = block3;
			aux.r = new Vector3 (-180, -180, 0);
		}

		if (!d && !u && !r && !l)
			aux.o = block4;


		return aux;
	}
	void Update () {
		
	}

}
