using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InterfaceManager : MonoBehaviour {

	bool stereo = false;
	bool mono = false;
	bool credits = false;
	bool main = false;

	public Image fade;

	GameObject[] texts;

	float alpha;

	// Use this for initialization
	void Start () {
		texts = GameObject.FindGameObjectsWithTag ("Text");
	}
	
	// Update is called once per frame
	void Update () {
		if (stereo) {
			alpha += 0.025f;
			if(alpha > 1)
				Application.LoadLevel ("Nespole");
		}

		if (mono) {
			alpha += 0.025f;
			if(alpha > 1)
				Application.LoadLevel ("Nespole");
		}

		if (credits) {
			alpha += 0.05f;
			if(alpha > 1)
				Application.LoadLevel ("Credits");
		}

		if (main) {
			alpha += 0.05f;
			if(alpha > 1)
				Application.LoadLevel ("Intro");
		}

		Color temp = new Color (0, 0, 0, alpha);
		fade.color = temp;

		Color tempText = new Color (1, 1, 1, 1-alpha);
		foreach (GameObject t in texts) {
			t.GetComponent<Text>().color = tempText;
		}
	}

	public void loadStereo(){
		stereo = true;
		PlayerPrefs.SetInt ("Stereo", 1);
	}

	public void loadMono(){
		mono = true;
		PlayerPrefs.SetInt ("Stereo", 0);
	}

	public void loadCredits(){
		Debug.Log ("click");
		credits = true;
	}

	public void loadMain(){
		main = true;
	}
}
