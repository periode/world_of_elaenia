using UnityEngine;
using System.Collections;

public class CubeBehavior : MonoBehaviour {

	GameObject holder;

	float noiseVal;
	public float noiseInc;

	public float threshMin;
	public float threshMax;

	public Material mWhite;
	public Material mBlack;
	public Material mLight;
	public Material mDark;
	public Material mBlue;
	public Material mGreen;
	public Material mRed;

	public Material mUnicolor;

	Material _mCurrent;
	Material _mOpposite;

	bool _canScale = false;
	public Vector3 maxScaleStart;
	Vector3 _maxScale;
	public float lerpIncScaleStart;
	public int scaleThreshold;
	float _lerpIncScale;
	float _lerpValScale = 0;
	Vector3 _startScale;
	bool _scaleThreshold = false;

	public bool unicolor = false;

	public int scaleBassTimer;
	public int scaleBass2Timer;
	public int scaleBass3Timer;

	bool _canRotate = false;
	bool _canRotate2 = false;

	int colorTimer;

	float time;

	// Use this for initialization
	void Start () {
		time = Time.time;
		holder = GameObject.Find ("Holder");
		noiseVal = Random.Range (0, 1000);

		threshMin = Random.Range (0.5f, 0.7f);
		threshMax = Random.Range (0.7f, 1f);

		int r = Random.Range (0, 10);
		switch (r) {
		case 0:
			_mCurrent = mBlack;
			_mOpposite = mWhite;
			colorTimer = 0;
			break;
		case 1:
			_mCurrent = mBlack;
			_mOpposite = mWhite;
			colorTimer = 20;
			break;
		case 2:
			_mCurrent = mBlack;
			_mOpposite = mWhite;
			colorTimer = 40;
			break;
		case 3:
			_mCurrent = mBlack;
			_mOpposite = mBlue;
			colorTimer = 60;
			break;
		case 4:
			_mCurrent = mBlack;
			_mOpposite = mBlue;
			colorTimer = 60;
			break;
		case 5:
			_mCurrent = mBlack;
			_mOpposite = mRed;
			colorTimer = 80;
			break;
		case 6:
			_mCurrent = mBlack;
			_mOpposite = mRed;
			colorTimer = 80;
			break;
		case 7:
			_mCurrent = mBlack;
			_mOpposite = mGreen;
			colorTimer = 100;
			break;
		case 8:
			_mCurrent = mBlack;
			_mOpposite = mGreen;
			colorTimer = 100;
			break;
		case 9:
			_mCurrent = mBlack;
			_mOpposite = mBlue;
			colorTimer = 110;
			break;
		default:
			_mCurrent = mBlack;
			_mOpposite = mGreen;
			colorTimer = 110;
			break;
		}

		_lerpIncScale = lerpIncScaleStart * (1 + (Random.Range (0, 40) / 100));
		_maxScale = maxScaleStart * (1 + (Random.Range (0, 40) / 100));

		_startScale = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > time+colorTimer  && Time.time < time+287-(colorTimer*0.5f))
			greyscaleMaterialSwitch ();

		if (noiseInc < 0.025f)
			noiseInc += 0.000001f;

		if (threshMin > 0.3f)
			threshMin -= 0.00001f;

		if(Time.time > time + scaleBassTimer && this.transform.position.z > -4.5f && this.transform.position.z < 4.5f  && Time.time < time+287)
			scaleBass ();

		if (Time.time > time + scaleBass3Timer && Time.time < time+287) {
			scaleThreshold = 9985;
			_canRotate2 = true;
		}

		if (Time.time > time + scaleBass2Timer && !_scaleThreshold  && Time.time < time+287) {
			scaleThreshold = 9990;
			_lerpIncScale = lerpIncScaleStart * (1 + (Random.Range (0, 50) / 100));
			_scaleThreshold = true;
			_canRotate = true;
		}

		if (unicolor)
			changeUnicolor ();

	}

	void changeUnicolor(){
		GetComponent<Renderer> ().material = mUnicolor;
	}

	public void greyscaleMaterialSwitch(){
		float n = Mathf.PerlinNoise(noiseVal, 1);
		if (n > threshMin && n < threshMax)
			this.GetComponent<Renderer>().material = _mOpposite;
		else
			this.GetComponent<Renderer>().material = _mCurrent;
		
		noiseVal += noiseInc;
	}

	public void scaleBass(){
		if (!_canScale && Random.Range (0, 10000) > scaleThreshold) {
			_canScale = true;
		}

		if (_canScale) {
			if (_lerpValScale < 1)
				_lerpValScale += _lerpIncScale;
			else
				_canScale = false;

			if (_canRotate) {
				transform.Rotate(Vector3.forward * Time.deltaTime * 175);
			}

			if (_canRotate2) {
				transform.Rotate(Vector3.up * Time.deltaTime * 175);
			}
		} else {
			if(_lerpValScale > 0){
				_lerpValScale -= _lerpIncScale;
			}
		}

		Vector3 transitScale = Vector3.Lerp (_startScale, _maxScale, _lerpValScale);
		this.transform.localScale = transitScale;
	}
}
