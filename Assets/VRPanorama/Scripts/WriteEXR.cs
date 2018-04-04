using UnityEngine;
using System.Collections;
using System;

public class WriteEXR : MonoBehaviour {
	public RenderTexture rt;
	public RenderTexture unfilteredRt;
	public int HRes;
	public int WRes;
	public Texture2D screenShot;
	private Color[] EXRArray;
	public bool render;
	private Material VRAA;

	// Use this for initialization
	void Start () {
		rt = new RenderTexture(512, 512, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);
		Material VRAA = Resources.Load("Materials/VRAA") as Material;
		screenShot = new Texture2D (512, 512, TextureFormat.RGBAHalf, false);

		//screenShot = new Texture2D(512, 512, TextureFormat.RGBAHalf, false, true);

	//	testColors = new Color [HRes*WRes];
//
//		for (int x = 0; x < HRes; x++) {
//			for (int y = 0; y < WRes; y++) {
//				testColors[y * 512 + x].r = (float)x / 512f;
//				testColors[y * 512 + x].g = (float)y / 512f;
//				testColors[y * 512 + x].b = ( (x&y) == 1) ? 1.0f : 0.0f; // blue Sierpinski triangle
//			}
//		}

	
	}
	
	// Update is called once per frame
	void Update () {
		if (render){

			//	Graphics.Blit (unfilteredRt, rt, VRAA, -1);
		VideoRenderPrepare ();
		screenShot = GetVideoScreenshot();
		EXRArray = screenShot.GetPixels (0, 0, 512, 512);
		string ffmpegPath = Application.dataPath + "\\VRPanorama\\StreamingAssets\\";
		MiniEXR.MiniEXR.MiniEXRWrite(ffmpegPath +"/Test.exr", Convert.ToUInt16 (HRes), Convert.ToUInt16 (WRes), EXRArray);
		render = false;
		}
	}

	public void VideoRenderPrepare () {

		VRAA = Resources.Load("Materials/VRAA") as Material;
		unfilteredRt = new RenderTexture(512, 512, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Default);
		rt = new RenderTexture(512, 512, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
		screenShot = new Texture2D (512, 512, TextureFormat.ARGB32, false);
	}


	public Texture2D GetVideoScreenshot()
	{
		

		Camera VRCam = Camera.main ;
		if (VRCam == null){
			UnityEngine.Debug.LogError ("There are no cameras with MAIN CAMERA tag in scene. Please assign MAIN CAMERA tag to your camera");
		}

		VRCam.targetTexture = unfilteredRt;
		VRCam.Render();

		RenderTexture.active = unfilteredRt;            
		VRAA.mainTexture = unfilteredRt;
		VRAA.SetInt("_U", 512*2);
		VRAA.SetInt("_V", 512*2);

		Graphics.Blit (unfilteredRt, rt, VRAA, -1);

		screenShot.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);

		return screenShot;

	}



}
