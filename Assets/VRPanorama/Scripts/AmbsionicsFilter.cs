using UnityEngine;
using System.Collections;

public class AmbsionicsFilter : MonoBehaviour {
	
	public bool filter;
	public int m_sampleRate;
	public int m_sampleDepth;
	public float gain;
	public int channels1;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnAudioFilterRead( float[] data, int channels )
	{
		for (int i = 0; i < data.Length; i +=4){
			float fl = data[i];
			float fr = data[i+1];
			float rl = data[i+2];
			float rr = data [i + 3];
			
//			float frontSum = fl + fr;
//			float backSum = rl + rr;
//			float frontDiff = fl - fr;
//			float backDiff = rl - rr;

//			float w = (frontSum / 1.414214f + backSum)/4.828427f;
//			float ch2 = (frontSum - backSum) / 2.414214f;
//			float ch3 = (frontDiff + backDiff) / 3.146264f;
//			float ch4 = 0;


			float w = 0.5f * (fl + rl + fr + rr);
			float x = 0.5f * ((fl - rl) + (fr - rr));
			float y = 0.5f * ((fl - rr) - (fr - rl));
			float z = 0.5f * ((fl - rl) + (rr - fr));




			data[i] = w;			
			data[i+1] = y;
			data[i+2] = z;
			data[i+3] = x;

		}
		
		}
}

