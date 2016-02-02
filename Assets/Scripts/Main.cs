using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public GameObject prefab;
	GameObject sphere;

	float theta;
	float phi;
	public float rad;
	float radZ;
	float radX;
	public float radZInc;
	public float radZDec;

	public int num;

	List<GameObject> cubes;

	[Header("Unicolor")]
	public int unicolorTimer;
	public int unicolorMin;
	public int unicolorMax;
	public int unicolorInc;

	public int unicolor2Timer;
	public float unicolor2Min;
	public float unicolor2Max;
	public float unicolor2Inc;

	public int unicolor3Timer;
	public float unicolor3Min;
	public float unicolor3Max;
	public float unicolor3Inc;

	[Header("Rotate")]
	public int wave1TimerStart;
	public int wave1TimerStop;

	public int wave2TimerStart;
	public int wave2TimerStop;

	public int wave3TimerStart;
	public int wave3TimerStop;

	public GameObject holder;
	public float rotation;
	bool secondRotation = true;
	bool thirdRotation = true;

	[Header("Break")]
	public int breakTimer;
	public int breakEndTimer;

	public float timerBreathe;
	public float expandVal;

	float time;

	// Use this for initialization
	void Start () {
		time = Time.time;
		int mode = PlayerPrefs.GetInt ("Stereo");

		if (mode == 0)
			Cardboard.SDK.VRModeEnabled = false;
		else
			Cardboard.SDK.VRModeEnabled = true;

		sphere = GameObject.Find ("Sphere");
		cubes = new List<GameObject> ();

		for (int u = 0; u < num; u++) {
			for(int v = 0; v < num; v++){

				theta = 2*Mathf.PI * u /num;
				phi = 2*Mathf.PI * v /num;

				float x = (1 + rad * Mathf.Cos(phi))*Mathf.Cos(theta);
				float y = (1 + rad * Mathf.Cos(phi))*Mathf.Sin(theta);
				float z = rad*Mathf.Sin(phi);

				Vector3 pos = new Vector3(x, y, z);

				GameObject g = (GameObject)(Instantiate(prefab, pos, Quaternion.identity));
				g.transform.parent = sphere.transform;
				cubes.Add(g);
			}
		}

		radZ = rad;
		radX = rad;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time < 290 + time) {
			if (holder.GetComponent<Light> ().range < 25)
				holder.GetComponent<Light> ().range += 0.0125f;
			
			if (holder.GetComponent<Light> ().intensity < 4)
				holder.GetComponent<Light> ().intensity += 0.0125f;
			
		} else {
			holder.GetComponent<Light> ().range -= 0.045f;
			if(holder.GetComponent<Light> ().range < 0.5)
				Time.timeScale = 0.5f;
		}

		if ((Time.time > time + wave1TimerStart && Time.time < time + wave1TimerStop  && Time.time < time+285))
			rotateHolder ();

		if (Time.time > time + wave2TimerStart && Time.time < time + wave2TimerStop)
			secondRotation = true;

		if (Time.time > time + wave3TimerStart && Time.time < time + wave3TimerStop)
			thirdRotation = true;

		if (Time.time > time + wave2TimerStart)
			rotation = 0.05f;

		if (Time.time > time + unicolorTimer)
			changeUnicolor ();

		if(Time.time > time + unicolor2Timer)
			changeUnicolor3 ();

		if(Time.time > time + unicolor3Timer)
			changeUnicolor2 ();


		if (Time.time > time + timerBreathe) {
			expand ();
			expandVal += Time.deltaTime;
		}


	}

	void expand(){
		Vector3 tempScale = sphere.transform.localScale;
		tempScale.x = 1 + Mathf.Sin (expandVal*2.1f) * 0.05f;
		tempScale.y = 1 + Mathf.Sin (expandVal*2.1f) * 0.05f;
		if (Time.time > breakTimer && Time.time < breakEndTimer) {
			tempScale.z += radZInc;
		}
		if (Time.time > breakEndTimer && tempScale.z > 1) {
			tempScale.z -= radZDec;
		}
		sphere.transform.localScale = tempScale;
	}

	void rotateHolder(){
		holder.transform.Rotate (Vector3.forward * rotation, Space.World);
		if(secondRotation)
			holder.transform.Rotate (Vector3.up * rotation * 1.1f, Space.World);
		if(thirdRotation)
			holder.transform.Rotate (Vector3.left * rotation * 1.3f, Space.World);
	}

	void changeUnicolor(){
		for(int c = 0; c < cubes.Count; c++){
			if(c < Mathf.Min(unicolorMax, cubes.Count-1) && c > unicolorMin)
				cubes[c].GetComponent<CubeBehavior>().unicolor = true;
			else
				cubes[c].GetComponent<CubeBehavior>().unicolor = false;
		}
		
		unicolorMax += unicolorInc;
		unicolorMin += unicolorInc;
		if (unicolorMax > cubes.Count*1f || unicolorMax < 1) {
			unicolorInc *= -1;
			foreach(GameObject g in cubes)
				g.GetComponent<CubeBehavior>().unicolor = false;
		}
	}

	void changeUnicolor2(){
		for(int c = 0; c < cubes.Count; c++){
			if(c % 50 > unicolor2Min)
				cubes[c].GetComponent<CubeBehavior>().unicolor = true;
			else
				cubes[c].GetComponent<CubeBehavior>().unicolor = false;
		}
		
		unicolor2Max += unicolor2Inc;
		unicolor2Min += unicolor2Inc;
		if (unicolor2Min > 50 || unicolor2Min < 0) {
			unicolor2Inc *= -1;
			foreach(GameObject g in cubes)
				g.GetComponent<CubeBehavior>().unicolor = false;
		}
	}

	void changeUnicolor3(){
		for(int c = 0; c < cubes.Count; c++){
			if(cubes[c].transform.position.z > unicolor3Min && cubes[c].transform.position.z < unicolor3Max)
				cubes[c].GetComponent<CubeBehavior>().unicolor = true;
			else
				cubes[c].GetComponent<CubeBehavior>().unicolor = false;
		}
		
		unicolor3Max += unicolor3Inc;
		unicolor3Min += unicolor3Inc;
		if (unicolor3Min > 6 || unicolor3Min < -6) {
			unicolor3Inc *= -1;
			foreach(GameObject g in cubes)
				g.GetComponent<CubeBehavior>().unicolor = false;
		}
	}
}
