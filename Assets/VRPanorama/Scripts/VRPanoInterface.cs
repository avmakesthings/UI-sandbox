using UnityEngine;
using System.Collections;

namespace VRPanorama
{
public class VRPanoInterface : MonoBehaviour {
	public float progressInd = 0; 
	public string timeCounterText;
	public UnityEngine.UI.Image texLoader ; 
	public UnityEngine.UI.Text timeCounter;
	public int width;
	public int height;
        public GameObject PanoramaCam;
        public GameObject hQCam;
        public bool rt = false;
	public bool mono = false;
	public bool sbs = false;
	public GameObject quadR;
        public GameObject quadL;
        public GameObject hQPlane;
        public bool hQ = false;
        // Use this for initialization
        public int step = 0;


        void Start () {
			
	
	}
	
	// Update is called once per frame
	void Update () {
			if (!rt){
				texLoader.fillAmount = progressInd;
				timeCounter.text = timeCounterText;
				PanoramaCam.transform.localScale = new Vector3 ((1.0f / height * width)  , 1, 2);
			}

			if (sbs && !mono && rt){
				PanoramaCam.transform.localScale = new Vector3 ((1f / height * width)  , 1, 1);
				quadR.transform.localPosition = new Vector3(-0.5f, 0, 1.5f);
				quadL.transform.localPosition = new Vector3(0.5f, 0, 1.5f);
				}
			if (!sbs && !mono && rt){
				PanoramaCam.transform.localScale = new Vector3 ((0.5f / height * width)  , 0.5f, 1);
				quadR.transform.localPosition = new Vector3(0, 0.5f, 1.5f);
				quadL.transform.localPosition = new Vector3(0, -0.5f, 1.5f);
			}

            if (mono && rt) PanoramaCam.transform.localScale = new Vector3(1.0f / height * width, 1, 1);

            if (hQ) hQPlane.SetActive(true);
            else hQPlane.SetActive(false);



    }
}
}