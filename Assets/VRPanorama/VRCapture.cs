
#if !UNITY_WEBPLAYER

using UnityEngine;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using VRPanorama;

#if !UNITY_5_0

using UnityEngine.VR;

#endif


using System;
using System.IO;
using System.Net;
using System.Net.Mail;



using System.Net.Security;
using System.Security.Cryptography.X509Certificates;



namespace VRPanorama


{
    [RequireComponent(typeof(AudioListener))]
    [RequireComponent(typeof(Camera))]
    public class VRCapture : MonoBehaviour
    {
        //Render Cubemap Init
        public int cubemapSize = 128;
        public bool oneFacePerFrame = false;
        public Camera cubeCam;
        private RenderTexture rtex;
        private RenderTexture rtexr;
        //

        public bool captureAudio = false;
        public float volume = 1;
        public bool mute = true;
        private float remainingTime;
        private int minutesRemain;
        private int secondsRemain;

        public bool alignPanoramaWithHorizont = true;
        private Material VRAA;
        private RenderTexture unfilteredRt;


        //		private GameObject renderHead;
        private GameObject rig;
        private GameObject cam;
        private GameObject panoramaCam;
        private string timestamp = "";


        private RenderTexture flTex;
        private RenderTexture frTex;
        private RenderTexture llTex;
        private RenderTexture lrTex;
        private RenderTexture rlTex;
        private RenderTexture rrTex;
        private RenderTexture dlTex;
        private RenderTexture drTex;
        private RenderTexture blTex;
        private RenderTexture brTex;
        private RenderTexture tlTex;
        private RenderTexture trTex;

        private RenderTexture tlxTex;
        private RenderTexture trxTex;
        private RenderTexture dlxTex;
        private RenderTexture drxTex;

        private Material FL;
        private Material FR;
        private Material LL;
        private Material LR;
        private Material RL;
        private Material RR;
        private Material DL;
        private Material DR;
        private Material BL;
        private Material BR;
        private Material TL;
        private Material TR;

        private Material DLX;
        private Material DRX;
        private Material TLX;
        private Material TRX;
        private Material HQMat;

        private RenderTexture rt;
        private Texture2D screenShot;
        private Texture2D screenShotHQ;

        public Camera camLL;
        public Camera camRL;
        public Camera camTL;
        public Camera camBL;
        public Camera camFL;
        public Camera camDL;

        public Camera camLR;
        public Camera camRR;
        public Camera camTR;
        public Camera camBR;
        public Camera camFR;
        public Camera camDR;

        public Camera camDRX;
        public Camera camDLX;
        public Camera camTRX;
        public Camera camTLX;

        private GameObject cloneCamLL = null;
        private GameObject cloneCamRL = null;
        private GameObject cloneCamTL = null;
        private GameObject cloneCamBL = null;
        private GameObject cloneCamFL = null;
        private GameObject cloneCamDL = null;
        private GameObject cloneCamLR = null;
        private GameObject cloneCamRR = null;
        private GameObject cloneCamTR = null;
        private GameObject cloneCamBR = null;
        private GameObject cloneCamFR = null;
        private GameObject cloneCamDR = null;
        private GameObject cloneCamDRX = null;
        private GameObject cloneCamDLX = null;
        private GameObject cloneCamTRX = null;
        private GameObject cloneCamTLX = null;


        GameObject camll;
        GameObject camrl;
        GameObject camfl;
        GameObject camtl;
        GameObject cambl;
        GameObject camdl;

        private bool waitRenderFinish = false;




        private GameObject renderPanorama;
        //	private bool panoramaInitialized = false;

        public int StartFrame = 0;



        public enum VRModeList { EquidistantStereo, EquidistantStereoSBS, EquidistantMono, VideoCapture };
        public enum VRCaptureList { AnimationCapture, StillImage, VRPanoramaRT };
        public enum VRCaptureAngle { _360, _180 };

        public VRModeList panoramaType = VRModeList.EquidistantMono;
        public VRCaptureList captureType = VRCaptureList.AnimationCapture;
        public VRCaptureAngle captureAngle = VRCaptureAngle._360;

        public enum VRFormatList { JPG, PNG, EXR_HDRI };
        public VRFormatList ImageFormatType;


        public string Folder = "VR_Sequence";

        public KeyCode captureKey = KeyCode.Space;

        public int FPS = 25;

        private Color[] EXRArray;

        public int resolution = 2048;
        public int resolutionH = 2048;

        public int step = 0;

        public int NumberOfFramesToRender = 100;
        public int renderFromFrame = 1;

        [VRPanorama]
        public string sequenceLength;


        public float IPDistance = 0.066f;
        public float EnvironmentDistance = 2;
        public float restoreFOV;


        public bool openDestinationFolder = true;
        public bool ambisonicsSupportTest = false;

        public string fullPath;
        public bool customPath = false;
        public string customPathFolder = "C:/";





        public bool encodeToMp4 = true;
        public int Mp4Bitrate = 20000;
        public int jpgQuality = 100;

        [Header("RenderTime/Quality Optimization")]
        [Range(1, 16)]
        public int renderQuality = 16;

        public string formatString;
        private int qualityTemp;
        public bool mailme = false;
        public string _mailto = "name@domain.com";
        public string _pass;
        public string _mailfrom = "name@gmail.com";
        public string _prefix = "img";
        TextureFormat texFormat;
        RenderTextureFormat renderTexFormat;


        [VRPanorama]
        public string RenderInfo = " ";


        public int bufferSize;
        public int numBuffers;
        private int outputRate = 48000;
        private int headerSize = 44; //default for uncompressed wav

        public bool recOutput;
        public bool depth = true;
        public int depthBufferSize;
        public bool ambisonics = false;
        public bool disableTracking = true;

        private FileStream fileStream;
        public UInt16 achannels;
        public bool hQ = false;
        public int steps = 10;
        public bool useAlpha = false;
        public float smoothing = 1f;


        //		void Awake(){
        //
        //
        //			
        //		}



        void Start()
        {
            if (panoramaType == VRModeList.EquidistantMono) hQ = false;
            if (customPath) fullPath = customPathFolder + Folder + "/";
            if (!customPath) fullPath = Path.GetFullPath(string.Format(@"{0}/", Folder));


            Application.runInBackground = true;
            StartFrame = Time.frameCount;
            //		gameObject.GetComponent<Camera>().fieldOfView = 100;


            if (!disableTracking)
                UnityEngine.XR.XRSettings.enabled = false;

            if (mute) AudioListener.volume = 0;
            if (depth == true)
                depthBufferSize = 24;
            else
                depthBufferSize = 0;

            if (captureType == VRCaptureList.AnimationCapture || captureType == VRCaptureList.VRPanoramaRT)
            {

                if (captureAudio)
                {

                    if ((int)AudioSettings.speakerMode == 3)
                        ambisonicsSupportTest = true;
                    if ((int)AudioSettings.speakerMode == 1) achannels = 1;
                    else if ((int)AudioSettings.speakerMode == 2) achannels = 2;
                    else if ((int)AudioSettings.speakerMode == 3) achannels = 4;
                    else if ((int)AudioSettings.speakerMode == 4) achannels = 5;
                    else if ((int)AudioSettings.speakerMode == 5) achannels = 6;
                    else if ((int)AudioSettings.speakerMode == 6) achannels = 8;
                    UnityEngine.Debug.Log("channels" + (int)AudioSettings.speakerMode + achannels);
                    outputRate = AudioSettings.outputSampleRate;
                    AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);
                    AudioListener.volume = volume;

                    System.IO.Directory.CreateDirectory(fullPath);
                    StartWriting(fullPath + "/" + Folder + ".wav");
                    recOutput = true;
                    print("rec start");

                }
                else
                {
                    if (panoramaType == VRModeList.VideoCapture) VideoRenderPrepare();

                    else
                    {
                        PreparePano();
                        RenderPano();
                    }
                }
            }
        }





        public void RenderPano()
        {


            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {
                screenShot = new Texture2D(resolution, resolutionH, texFormat, false);
                if  (hQ)
                screenShotHQ = new Texture2D(resolution, resolutionH, texFormat, false);
            }
            else
            {
                screenShot = new Texture2D(resolution, resolutionH, texFormat, false);
            }

            float aAfactor = resolutionH;
            float aAfactorH = resolution;


            flTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
            llTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
            rlTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
            dlTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
            blTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
            tlTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);



            FL = Resources.Load("RTs/Materials/FL") as Material;
            FR = Resources.Load("RTs/Materials/FR") as Material;
            LL = Resources.Load("RTs/Materials/LL") as Material;
            LR = Resources.Load("RTs/Materials/LR") as Material;
            RL = Resources.Load("RTs/Materials/RL") as Material;
            RR = Resources.Load("RTs/Materials/RR") as Material;
            DL = Resources.Load("RTs/Materials/DL") as Material;
            DR = Resources.Load("RTs/Materials/DR") as Material;
            BL = Resources.Load("RTs/Materials/BL") as Material;
            BR = Resources.Load("RTs/Materials/BR") as Material;
            TL = Resources.Load("RTs/Materials/TL") as Material;
            TR = Resources.Load("RTs/Materials/TR") as Material;
            HQMat = Resources.Load("RTs/Materials/hQScanline") as Material;


            camLL.targetTexture = llTex;
            camRL.targetTexture = rlTex;
            camTL.targetTexture = tlTex;
            if (captureAngle == VRCaptureAngle._360)
                camBL.targetTexture = blTex;
            camFL.targetTexture = flTex;
            camDL.targetTexture = dlTex;


            FL.SetFloat("_U", aAfactor);
            FR.SetFloat("_U", aAfactor);
            LL.SetFloat("_U", aAfactor);
            LR.SetFloat("_U", aAfactor);
            RL.SetFloat("_U", aAfactor);
            RR.SetFloat("_U", aAfactor);
            DL.SetFloat("_U", aAfactor);
            DR.SetFloat("_U", aAfactor);
            BL.SetFloat("_U", aAfactor);
            BR.SetFloat("_U", aAfactor);
            TL.SetFloat("_U", aAfactor);
            TR.SetFloat("_U", aAfactor);

            FL.SetFloat("_V", aAfactorH);
            FR.SetFloat("_V", aAfactorH);
            LL.SetFloat("_V", aAfactorH);
            LR.SetFloat("_V", aAfactorH);
            RL.SetFloat("_V", aAfactorH);
            RR.SetFloat("_V", aAfactorH);
            DL.SetFloat("_V", aAfactorH);
            DR.SetFloat("_V", aAfactorH);
            BL.SetFloat("_V", aAfactorH);
            BR.SetFloat("_V", aAfactorH);
            TL.SetFloat("_V", aAfactorH);
            TR.SetFloat("_V", aAfactorH);


            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {
                dlxTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                drxTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                tlxTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                trxTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);


                frTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                lrTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                rrTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                drTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                if (captureAngle == VRCaptureAngle._360)
                {
                    brTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                    trTex = RenderTexture.GetTemporary(qualityTemp, qualityTemp, depthBufferSize, renderTexFormat);
                }


                FL.SetTexture("_Main", flTex);
                FR.SetTexture("_Main", frTex);
                LL.SetTexture("_Main", llTex);
                LR.SetTexture("_Main", lrTex);
                RL.SetTexture("_Main", rlTex);
                RR.SetTexture("_Main", rrTex);
                DL.SetTexture("_Main", dlTex);
                DR.SetTexture("_Main", drTex);


                if (captureAngle == VRCaptureAngle._360)
                {
                    BL.SetTexture("_Main", blTex);
                    BR.SetTexture("_Main", brTex);
                }

                TL.SetTexture("_Main", tlTex);
                TR.SetTexture("_Main", trTex);


                TL.SetTexture("_MainR", trTex);
                TR.SetTexture("_MainR", tlTex);
                DL.SetTexture("_MainR", drTex);
                DR.SetTexture("_MainR", dlTex);

                TL.SetTexture("_MainX", trxTex);
                TR.SetTexture("_MainX", tlxTex);
                TL.SetTexture("_MainRX", tlxTex);
                TR.SetTexture("_MainRX", trxTex);
                DL.SetTexture("_MainX", dlxTex);
                DR.SetTexture("_MainX", drxTex);
                DL.SetTexture("_MainRX", drxTex);
                DR.SetTexture("_MainRX", dlxTex);



                camLR.targetTexture = lrTex;
                camRR.targetTexture = rrTex;
                camTR.targetTexture = trTex;
                if (captureAngle == VRCaptureAngle._360)
                    camBR.targetTexture = brTex;
                camFR.targetTexture = frTex;
                camDR.targetTexture = drTex;


                camDRX.targetTexture = drxTex;
                camDLX.targetTexture = dlxTex;
                camTRX.targetTexture = trxTex;
                camTLX.targetTexture = tlxTex;

            }

            else
            {


                FL.SetTexture("_Main", flTex);
                FR.SetTexture("_Main", flTex);
                LL.SetTexture("_Main", llTex);
                LR.SetTexture("_Main", llTex);
                RL.SetTexture("_Main", rlTex);
                RR.SetTexture("_Main", rlTex);
                DL.SetTexture("_Main", dlTex);
                DR.SetTexture("_Main", dlTex);
                if (captureAngle == VRCaptureAngle._360)
                {
                    BL.SetTexture("_Main", blTex);
                    BR.SetTexture("_Main", blTex);
                }
                TL.SetTexture("_Main", tlTex);
                TR.SetTexture("_Main", tlTex);




                TL.SetTexture("_MainR", tlTex);

                DL.SetTexture("_MainR", dlTex);


                TL.SetTexture("_MainX", tlTex);

                TL.SetTexture("_MainRX", tlTex);

                DL.SetTexture("_MainX", dlTex);

                DL.SetTexture("_MainRX", dlTex);



                TR.SetTexture("_MainR", tlTex);

                DR.SetTexture("_MainR", dlTex);


                TR.SetTexture("_MainX", tlTex);

                TR.SetTexture("_MainRX", tlTex);

                DR.SetTexture("_MainX", dlTex);

                DR.SetTexture("_MainRX", dlTex);




            }


            if (Application.isPlaying && captureType != VRCaptureList.VRPanoramaRT)
            {
                Time.captureFramerate = FPS;
                System.IO.Directory.CreateDirectory(fullPath);
            }

        }



        void Update()
        {
            if (captureType == VRCaptureList.AnimationCapture || captureType == VRCaptureList.VRPanoramaRT)
            {

                if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
                {
                    cloneCamFL.transform.LookAt(camfl.transform.position + camfl.transform.forward * EnvironmentDistance, camfl.transform.up);
                    cloneCamFR.transform.LookAt(camfl.transform.position + camfl.transform.forward * EnvironmentDistance, camfl.transform.up);

                    cloneCamLL.transform.LookAt(camll.transform.position + camll.transform.forward * EnvironmentDistance, camll.transform.up);
                    cloneCamLR.transform.LookAt(camll.transform.position + camll.transform.forward * EnvironmentDistance, camll.transform.up);

                    cloneCamRL.transform.LookAt(camrl.transform.position + camrl.transform.forward * EnvironmentDistance, camrl.transform.up);
                    cloneCamRR.transform.LookAt(camrl.transform.position + camrl.transform.forward * EnvironmentDistance, camrl.transform.up);
                    if (captureAngle == VRCaptureAngle._360)
                    {
                        cloneCamBL.transform.LookAt(cambl.transform.position + cambl.transform.forward * EnvironmentDistance, cambl.transform.up);
                        cloneCamBR.transform.LookAt(cambl.transform.position + cambl.transform.forward * EnvironmentDistance, cambl.transform.up);
                    }

                    cloneCamTL.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);
                    cloneCamTR.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);

                    cloneCamDL.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);
                    cloneCamDR.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);


                    cloneCamTLX.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);
                    cloneCamTRX.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);

                    cloneCamDLX.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);
                    cloneCamDRX.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);

                }




                if (captureAudio)
                {
                }

                else
                {

                    if ((Time.frameCount - StartFrame) == NumberOfFramesToRender - 2 && mailme == true)
                    {


                    }

                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    print("Render Aborted");
                    gameObject.GetComponent<VRCapture>().enabled = false;
                    Destroy(renderPanorama);
                    Destroy(panoramaCam);
                    Destroy(rig);
                    Time.captureFramerate = 0;

                }
            }
        }

        void LateUpdate()
        {

            if (captureType == VRCaptureList.AnimationCapture || captureType == VRCaptureList.VRPanoramaRT)
            {
                if (captureAudio)
                {
                    if (Time.timeSinceLevelLoad > NumberOfFramesToRender / FPS)
                    {
                        recOutput = false;
                        WriteHeader();
                        QuitEditor();
                    }
                }
                else
                {
                    if (panoramaType == VRModeList.VideoCapture)
                    {
                        RenderVideo();
                    }

                    else
                    {
                        if (alignPanoramaWithHorizont)
                        {
                            Vector3 angleCorection = gameObject.transform.rotation.eulerAngles;
                            angleCorection = new Vector3(0, angleCorection.y, 0);
                            gameObject.transform.rotation = Quaternion.Euler(angleCorection);
                        }
                        if (captureType != VRCaptureList.VRPanoramaRT)
                        {

                            RenderVRPanorama();
                            CounterPost();
                        }

                    }

                }
            }

            else
            {

                if (Input.GetKeyDown(captureKey) && waitRenderFinish == false) step = 1;
                NumberOfFramesToRender = 1000000000;

                if (step == 3)
                {
                    Destroy(renderPanorama);
                    Destroy(panoramaCam);
                    Destroy(rig);
                    RenderTexture.Destroy(flTex);
                    RenderTexture.Destroy(llTex);
                    RenderTexture.Destroy(rlTex);
                    RenderTexture.Destroy(dlTex);
                    RenderTexture.Destroy(blTex);
                    RenderTexture.Destroy(tlTex);
                    RenderTexture.Destroy(rt);

                    Texture2D.Destroy(screenShot);
                    Texture2D.Destroy(screenShotHQ);

                    if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
                    {



                        RenderTexture.Destroy(tlxTex);
                        RenderTexture.Destroy(trxTex);
                        RenderTexture.Destroy(dlxTex);
                        RenderTexture.Destroy(drxTex);


                        RenderTexture.Destroy(frTex);
                        RenderTexture.Destroy(lrTex);
                        RenderTexture.Destroy(rrTex);
                        RenderTexture.Destroy(drTex);
                        if (captureAngle == VRCaptureAngle._360)
                        {
                            RenderTexture.Destroy(brTex);
                            RenderTexture.Destroy(trTex);
                        }
                    }

                        cam.GetComponent<Camera>().fieldOfView = restoreFOV;
                    string ffmpegPath = Application.dataPath + "\\VRPanorama\\StreamingAssets\\";
                    //	string fullPath = Path.GetFullPath(string.Format(@"{0}/",Folder));

#if !UNITY_EDITOR_OSX
#if !UNITY_STANDALONE_OSX

                    Process compiler = new Process();
                    compiler.StartInfo.FileName = ffmpegPath + "exiftool";
                    compiler.StartInfo.Arguments = ("-overwrite_original -ProjectionType=\"equirectangular\" \"" + fullPath + string.Format(_prefix + "{1:D05}" + timestamp + ".jpg", Folder, (Time.frameCount - StartFrame - 2)) + "\"");
                    compiler.StartInfo.UseShellExecute = false;
                    //	UnityEngine.Debug.Log ("-overwrite_original -ProjectionType=\"equirectangular\" \"" + fullPath + string.Format ( _prefix + "{1:D05}.jpg", Folder, (Time.frameCount - StartFrame - 2 )) + "\"");
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    compiler.Start();

                    UnityEngine.Debug.Log(compiler.StandardOutput.ReadToEnd());

                    compiler.WaitForExit();
#endif
#endif

                    step = 0;
                    waitRenderFinish = false;
                }


                if (step == 2)
                {
                    if (alignPanoramaWithHorizont)
                    {
                        Vector3 angleCorection = gameObject.transform.rotation.eulerAngles;
                        angleCorection = new Vector3(0, angleCorection.y, 0);
                        gameObject.transform.rotation = Quaternion.Euler(angleCorection);
                    }
                    RenderVRPanorama();
                    print("Panorama Captured");


                    Time.captureFramerate = 0;
                    step = 3;
                }
                if (step == 1)
                {
                    waitRenderFinish = true;
                    PreparePano();
                    RenderPano();



                    step = step + 1;
                }

            }
        }





        public void RenderVRPanorama()
        {

            if (captureType != VRCaptureList.VRPanoramaRT)
            {

                if (Time.frameCount - StartFrame < NumberOfFramesToRender)

                {
                    if ((Time.frameCount - StartFrame) > 0 && (Time.frameCount - StartFrame) > renderFromFrame)
                    {
                        SaveScreenshot();
                    }
                }

            }
        }





        public void RenderVideo()
        {

            float sequenceTime = (float)NumberOfFramesToRender / (float)FPS;
            int minutesSeq = (int)sequenceTime / 60;
            int secondsSeq = (int)sequenceTime % 60;


            sequenceLength = (minutesSeq + " min. " + secondsSeq + " sec. ");




            if ((Time.frameCount - StartFrame) < NumberOfFramesToRender)

            {



                RenderInfo = "Rendering";
                if ((Time.frameCount - StartFrame) > 0)
                {
                    SaveScreenshotVideo();
                }

                remainingTime = Time.realtimeSinceStartup / (Time.frameCount - StartFrame) * (NumberOfFramesToRender - (Time.frameCount - StartFrame));
                minutesRemain = (int)remainingTime / 60;
                secondsRemain = (int)remainingTime % 60;
#if UNITY_EDITOR
                if (UnityEditor.EditorUtility.DisplayCancelableProgressBar("Rendering", " Remaining time: " + minutesRemain + " min. " + secondsRemain + " sec.", (float)(Time.frameCount - StartFrame) / (float)NumberOfFramesToRender))
                {
                    ClearBar();
                    UnityEngine.Debug.Log(Time.realtimeSinceStartup);
                    QuitEditor();

                }
#endif


            }

            else
            {

                ClearBar();




                if (openDestinationFolder)
                {




                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = fullPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }

                if (encodeToMp4)
                {
                    //string fullPath = Path.GetFullPath(string.Format(@"{0}/", Folder));
                    string ffmpegPath = Application.dataPath + "\\VRPanorama\\StreamingAssets\\";
                    if (System.IO.File.Exists(fullPath + Folder + ".wav"))
                    {


                        System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _prefix + "%05d" + formatString + " -i \"" + fullPath + Folder + ".wav" + "\"" + " -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + Mp4Bitrate + "k" + " -c:a aac -strict experimental -b:a 192k -shortest " + " \"" + fullPath + Folder + ".mp4\"");



                    }
                    else System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _prefix + "%05d" + formatString + " -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + Mp4Bitrate + "k \"" + fullPath + Folder + ".mp4\"");
                }

                QuitEditor();



            }

        }

        ////HDRI textures needs flipping
        Texture2D FlipTexture(Texture2D original)
        {
            Texture2D flipped = new Texture2D(original.width, original.height, TextureFormat.RGBAHalf, false);

            int xN = original.width;
            int yN = original.height;


            for (int i = 0; i < xN; i++)
            {
                for (int j = 0; j < yN; j++)
                {
                    flipped.SetPixel(i, yN - j - 1, original.GetPixel(i, j));
                }
            }
            flipped.Apply();

            return flipped;
        }






        public void SaveScreenshot()
        {

            if (ImageFormatType == VRFormatList.JPG)
            {

                if (captureType == VRCaptureList.StillImage) timestamp = System.DateTime.Now.Ticks + "Screenshot";
                string filePathf = string.Format(fullPath + _prefix + "{1:D05}" + timestamp + ".jpg", Folder, (Time.frameCount - StartFrame - 1));
                formatString = ".jpg\"";


             //   Texture2D screenShot;

                if (hQ) screenShot = GetScreenshotHQ(true);
                else screenShot = GetScreenshot(true);




                StartCoroutine(SaveFileJPG(filePathf, screenShot, jpgQuality));
                
            }

            else if (ImageFormatType == VRFormatList.PNG)
            {
                if (hQ) screenShot = GetScreenshotHQ(true);
                else screenShot = GetScreenshot(true);

                if (captureType == VRCaptureList.StillImage) timestamp = System.DateTime.Now.Ticks + "Screenshot";
                string filePathf = string.Format(fullPath + _prefix + "{1:D05}" + timestamp + ".png", Folder, (Time.frameCount - StartFrame - 1));
                formatString = ".png\"";
                StartCoroutine(SaveFilePNG(filePathf, screenShot));
                
            }
            else
            {
                Texture2D screenShot = GetScreenshot(true);
                Texture2D screenShot2 = FlipTexture(screenShot);

                EXRArray = screenShot2.GetPixels(0, 0, resolution, resolutionH);
                string filePathf = string.Format(fullPath + _prefix + "{1:D05}.exr", Folder, (Time.frameCount - StartFrame - 1));
                MiniEXR.MiniEXR.MiniEXRWrite(filePathf, Convert.ToUInt32(resolution), Convert.ToUInt32(resolutionH), EXRArray);
            }
        }


        private static IEnumerator SaveFileJPG(string filePathd, Texture2D SShot, int jpg)
        {
            yield return null;
            byte[] bytes = SShot.EncodeToJPG(jpg);
            File.WriteAllBytes(filePathd, bytes);
           // DestroyImmediate(SShot);
        }

        private static IEnumerator SaveFilePNG(string filePathd, Texture2D SShot)
        {
            yield return null;
            byte[] bytes = SShot.EncodeToPNG();
            File.WriteAllBytes(filePathd, bytes);
        }

        private static IEnumerator SaveFileEXR(string filePathd, Texture2D SShot)
        {
            yield return null;
            byte[] bytes = SShot.EncodeToPNG();
            File.WriteAllBytes(filePathd, bytes);
        }


        public Texture2D GetScreenshot(bool eye)
        {

            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {

                if (panoramaType == VRModeList.EquidistantStereo) rt = RenderTexture.GetTemporary(resolution, resolutionH / 2, 0, renderTexFormat);
                if (panoramaType == VRModeList.EquidistantStereoSBS) rt = RenderTexture.GetTemporary(resolution / 2, resolutionH, 0, renderTexFormat);

            }

            else rt = RenderTexture.GetTemporary(resolution, resolutionH, 0, renderTexFormat);

            if (eye)
            {

                if (panoramaType == VRModeList.EquidistantStereo)
                {
                    GameObject.Find("QuadL").transform.localPosition = new Vector3(0f, 0f, 1.5f);
                    GameObject.Find("QuadR").transform.localPosition = new Vector3(0f, 0f, 6.5f);
                }
                if (panoramaType == VRModeList.EquidistantStereoSBS)
                {
                    GameObject.Find("QuadL").transform.localPosition = new Vector3(0f, 0f, 6.5f);
                    GameObject.Find("QuadR").transform.localPosition = new Vector3(0f, 0f, 1.5f);
                }
            }

            Camera VRCam = panoramaCam.GetComponent<Camera>();
            VRCam.targetTexture = rt;
            VRCam.Render();
            RenderTexture.active = rt;

            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {

                if (panoramaType == VRModeList.EquidistantStereo) screenShot.ReadPixels(new Rect(0, 0, resolution, resolutionH / 2), 0, 0);
                if (panoramaType == VRModeList.EquidistantStereoSBS) screenShot.ReadPixels(new Rect(0, 0, resolution / 2, resolutionH), 0, 0);

                if (panoramaType == VRModeList.EquidistantStereo)
                {
                    GameObject.Find("QuadR").transform.localPosition = new Vector3(0f, 0f, 1.5f);
                    GameObject.Find("QuadL").transform.localPosition = new Vector3(0f, 0f, 6.5f);
                }
                if (panoramaType == VRModeList.EquidistantStereoSBS)
                {
                    GameObject.Find("QuadR").transform.localPosition = new Vector3(0f, 0f, 6.5f);
                    GameObject.Find("QuadL").transform.localPosition = new Vector3(0f, 0f, 1.5f);
                }

                VRCam.targetTexture = rt;
                VRCam.Render();
                RenderTexture.active = rt;

                if (panoramaType == VRModeList.EquidistantStereo)
                {
                    screenShot.ReadPixels(new Rect(0, 0, resolution, resolutionH / 2), 0, resolutionH / 2);
                }

                if (panoramaType == VRModeList.EquidistantStereoSBS)
                {
                    screenShot.ReadPixels(new Rect(0, 0, resolution, resolutionH), resolution / 2, 0);
                }
                RenderTexture.active = null;
                VRCam.targetTexture = null;
                RenderTexture.ReleaseTemporary(rt);
                
                RenderTexture.DestroyImmediate(rt);
            }
            else
            {
                screenShot.ReadPixels(new Rect(0, 0, resolution, resolutionH), 0, 0);
                RenderTexture.active = null;
                VRCam.targetTexture = null;
                RenderTexture.ReleaseTemporary(rt);
                
                RenderTexture.DestroyImmediate(rt);
            }
            return screenShot;

        }

        public Texture2D GetScreenshotHQ(bool eye)
        {


            float multiplySBS = 1;

            Shader.SetGlobalFloat("hQSteps", steps);
            RenderTexture hqrt = RenderTexture.GetTemporary(resolution, resolutionH, 0, renderTexFormat);
            Vector3 pos = gameObject.transform.position;
            Quaternion rot = gameObject.transform.rotation;
            if (hQ)
            {
                Shader.SetGlobalFloat("scanlineSoft", smoothing);

                if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
                {

                    if (panoramaType == VRModeList.EquidistantStereo)
                    {
                        rt = RenderTexture.GetTemporary(resolution, resolutionH / 2, 0, renderTexFormat);
                        Shader.SetGlobalFloat("sbs", 1);
                        multiplySBS = 1;
                    }

                    if (panoramaType == VRModeList.EquidistantStereoSBS)
                    {
                        rt = RenderTexture.GetTemporary(resolution / 2, resolutionH, 0, renderTexFormat);
                        Shader.SetGlobalFloat("sbs", 2);
                        multiplySBS = 2;
                    }

                }

                else rt = RenderTexture.GetTemporary(resolution, resolutionH, 0, renderTexFormat);


                Camera VRCam = panoramaCam.GetComponent<Camera>();
                GameObject panoramaHQCam = GameObject.Find("CameraHQ");
                Camera HQCam = panoramaHQCam.GetComponent<Camera>();



                for (int i = 0; i < steps; i++)
                {
                    gameObject.transform.Rotate(0, (360f / steps / 4), 0);
                    camLL.Render();
                    camRL.Render();
                    camTL.Render();
                    camBL.Render();
                    camFL.Render();
                    camDL.Render();

                    camLR.Render();
                    camRR.Render();
                    camTR.Render();
                    camBR.Render();
                    camFR.Render();
                    camDR.Render();

                    camDRX.Render();
                    camDLX.Render();
                    camTRX.Render();
                    camTLX.Render();


                    if (panoramaType == VRModeList.EquidistantStereo)
                    {
                        GameObject.Find("QuadR").transform.localPosition = new Vector3(0f, 0f, 1.5f);
                        GameObject.Find("QuadL").transform.localPosition = new Vector3(0f, 0f, 6.5f);
                    }
                    if (panoramaType == VRModeList.EquidistantStereoSBS)
                    {
                        GameObject.Find("QuadR").transform.localPosition = new Vector3(0f, 0f, 6.5f);
                        GameObject.Find("QuadL").transform.localPosition = new Vector3(0f, 0f, 1.5f);
                    }






                    if (eye)
                    {

                        if (panoramaType == VRModeList.EquidistantStereo)
                        {
                            GameObject.Find("QuadR").transform.localPosition = new Vector3((1f / 4 / steps) * i , 0f, 1.5f);
                            GameObject.Find("QuadL").transform.localPosition = new Vector3((1f / 4 / steps) * i , 0f, 6.5f);
                        }
                        if (panoramaType == VRModeList.EquidistantStereoSBS)
                        {
                            GameObject.Find("QuadL").transform.localPosition = new Vector3((1f / 4 / steps) * i , 0f, 6.5f);
                            GameObject.Find("QuadR").transform.localPosition = new Vector3((1f / 4 / steps) * i , 0f, 1.5f);
                        }


                    }


                    VRCam.targetTexture = rt;
                    VRCam.Render();
                    RenderTexture.active = rt;

                    if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
                    {

                        if (panoramaType == VRModeList.EquidistantStereo) screenShot.ReadPixels(new Rect(0, 0, resolution, resolutionH / 2), 0, 0);
                        if (panoramaType == VRModeList.EquidistantStereoSBS) screenShot.ReadPixels(new Rect(0, 0, resolution / 2, resolutionH), 0, 0);

                        GameObject.Find("QuadL").transform.localPosition = new Vector3((1f / 4 / steps) * i, 0f, 1.5f);
                        GameObject.Find("QuadR").transform.localPosition = new Vector3((1f / 4 / steps) * i, 0f, 6.5f);

                        VRCam.targetTexture = rt;
                        VRCam.Render();
                        RenderTexture.active = rt;





                        if (panoramaType == VRModeList.EquidistantStereo)
                        {
                            screenShot.ReadPixels(new Rect(0, 0, resolution, resolutionH), 0, resolutionH / 2);
                        }

                        if (panoramaType == VRModeList.EquidistantStereoSBS)
                        {
                            screenShot.ReadPixels(new Rect(0, 0, resolution, resolutionH), resolution / 2, 0);
                        }
                        screenShot.Apply();


                       Shader.SetGlobalFloat("hQStep", steps - i);

                        HQMat.SetTexture("_Main", screenShot);

                        HQCam.targetTexture = hqrt;


                        RenderTexture.active = hqrt;

                        HQCam.Render();
                       VRCam.targetTexture = null;
                     HQCam.targetTexture = null;
                        

                    }

                }
            }

            gameObject.transform.position = pos;
           gameObject.transform.rotation = rot;
            screenShotHQ.ReadPixels(new Rect(0, 0, resolution, resolutionH), 0, 0);
            RenderTexture.active = null;

            RenderTexture.ReleaseTemporary(rt);
           RenderTexture.ReleaseTemporary(hqrt);
            RenderTexture.DestroyImmediate(rt);
            RenderTexture.DestroyImmediate(hqrt);
            return screenShotHQ;

        }

        public void SendEmail()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_mailfrom);
            mail.To.Add(_mailto);
            mail.Subject = "VR Panorama Rendered";
            mail.Body = "Congratulations, VR panorama has finished rendering panorama named" + Folder;

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential(_mailfrom, _pass) as ICredentialsByHost;
            smtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
            smtpServer.Send(mail);
            UnityEngine.Debug.Log("Mail sent");

        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {

            UnityEditor.Handles.RadiusHandle(Quaternion.identity,
                                  transform.position,
                                  EnvironmentDistance);

        }

#endif
        ////////////	VIDEO SCREENSHOT	


        public Texture2D GetVideoScreenshot()
        {



            Camera VRCam = Camera.main;
            if (VRCam == null)
            {
                UnityEngine.Debug.LogError("There are no cameras with MAIN CAMERA tag in scene. Please assign MAIN CAMERA tag to your camera");
            }

            VRCam.targetTexture = unfilteredRt;
            VRCam.Render();

            RenderTexture.active = unfilteredRt;
            VRAA.mainTexture = unfilteredRt;
            VRAA.SetInt("_U", resolution * 2);
            VRAA.SetInt("_V", resolutionH * 2);

            Graphics.Blit(unfilteredRt, rt, VRAA, -1);

            screenShot.ReadPixels(new Rect(0, 0, resolution, resolutionH), 0, 0);

            return screenShot;

        }




        public void VideoRenderPrepare()
        {

            VRAA = Resources.Load("Materials/VRAA") as Material;
            unfilteredRt = new RenderTexture(resolution * 4 / 32 * renderQuality, resolutionH * 4 / 32 * renderQuality, 24, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Default);

            if (ImageFormatType == VRFormatList.EXR_HDRI)
            {
                rt = new RenderTexture(resolution, resolutionH, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);
                screenShot = new Texture2D(resolution, resolutionH, TextureFormat.RGBAHalf, false);
            }
            else
            {
                rt = new RenderTexture(resolution, resolutionH, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
                screenShot = new Texture2D(resolution, resolutionH, TextureFormat.ARGB32, false);
            }



            if (Application.isPlaying && captureType != VRCaptureList.VRPanoramaRT)
            {
                Time.captureFramerate = FPS;
                System.IO.Directory.CreateDirectory(fullPath);
            }
            else
            {
                ClearBar();

            }

        }




        public void SaveScreenshotVideo()
        {

            Texture2D screenShot = GetVideoScreenshot();

            if (ImageFormatType == VRFormatList.JPG)
            {
                byte[] bytes = screenShot.EncodeToJPG(jpgQuality);
                string filePathh = string.Format(fullPath + _prefix + "{1:D05}.jpg", Folder, (Time.frameCount - StartFrame - 1));
                formatString = ".jpg\"";
                print(filePathh);
                File.WriteAllBytes(filePathh, bytes);
            }

            else if (ImageFormatType == VRFormatList.PNG)
            {
                byte[] bytes = screenShot.EncodeToPNG();
                string filePathh = string.Format(fullPath + _prefix + "{1:D05}.png", Folder, (Time.frameCount - StartFrame - 1));
                formatString = ".png\"";
                File.WriteAllBytes(filePathh, bytes);
            }

            else
            {

                Texture2D screenShot2 = FlipTexture(screenShot);
                EXRArray = screenShot2.GetPixels(0, 0, resolution, resolutionH);

                string filePathh = string.Format(fullPath + _prefix + "{1:D05}.exr", Folder, (Time.frameCount - StartFrame - 1));
                MiniEXR.MiniEXR.MiniEXRWrite(filePathh, Convert.ToUInt32(resolution), Convert.ToUInt32(resolutionH), EXRArray);
            }

        }





        public void RenderStaticVRPanorama()
        {
            SaveScreenshot();
        }



        public void CounterPost()
        {
            float sequenceTime = (float)NumberOfFramesToRender / (float)FPS;
            int minutesSeq = (int)sequenceTime / 60;
            int secondsSeq = (int)sequenceTime % 60;


            sequenceLength = (minutesSeq + " min. " + secondsSeq + " sec. ");




            if ((Time.frameCount - StartFrame) < NumberOfFramesToRender)
            {
                remainingTime = Time.realtimeSinceStartup / (Time.frameCount - StartFrame) * (NumberOfFramesToRender - (Time.frameCount - StartFrame));
                minutesRemain = (int)remainingTime / 60;
                secondsRemain = (int)remainingTime % 60;
                VRPanoInterface loader = renderPanorama.GetComponent<VRPanoInterface>();
                loader.progressInd = (float)(Time.frameCount - StartFrame) / (float)NumberOfFramesToRender;
                loader.timeCounterText = " Remaining time: " + minutesRemain + " min. " + secondsRemain + " sec.";
                loader.height = resolutionH;
                loader.width = resolution;
                if (panoramaType == VRModeList.EquidistantStereo)
                {

                    loader.height = resolutionH / 2;
                    loader.width = resolution;

                }

                if (panoramaType == VRModeList.EquidistantStereoSBS)
                {

                    loader.height = resolutionH;
                    loader.width = resolution / 2;

                }


#if UNITY_EDITOR && !UNITY_WEBPLAYER
                if (UnityEditor.EditorUtility.DisplayCancelableProgressBar("Rendering", " Remaining time: " + minutesRemain + " min. " + secondsRemain + " sec.", (float)(Time.frameCount - StartFrame) / (float)NumberOfFramesToRender))
                {
                    ClearBar();
                    UnityEngine.Debug.Log("Rendering Time: " + Time.realtimeSinceStartup + " Seconds");
                    QuitEditor();
                }

            }

            else
            {
                ClearBar();


#endif
                if (openDestinationFolder)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = fullPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }



                if (encodeToMp4)
                {
                    //	string fullPath = Path.GetFullPath(string.Format(@"{0}/", Folder));
                    string ffmpegPath = Application.dataPath + "\\VRPanorama\\StreamingAssets\\";
                    UnityEngine.Debug.Log(ffmpegPath);
                    string extension = "_360";

                    if (panoramaType == VRModeList.EquidistantStereo)
                        extension = "_360_TB";
                    if (panoramaType == VRModeList.EquidistantStereoSBS)
                        extension = "_360_SBS";




                    if (System.IO.File.Exists(fullPath + Folder + ".wav"))
                    {
                        System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _prefix + "%05d" + formatString + " -i \"" + fullPath + Folder + ".wav" + "\"" + " -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + Mp4Bitrate + "k" + " -c:a aac -strict experimental -b:a 192k -shortest " + " \"" + fullPath + Folder + extension + ".mp4\"");

                    }

                    else System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _prefix + "%05d" + formatString + " -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + Mp4Bitrate + "k \"" + fullPath + Folder + extension + ".mp4\"");
                }

                if (mailme) SendEmail();

                UnityEngine.Debug.Log(Time.realtimeSinceStartup);
                QuitEditor();
            }

        }




        public void QuitEditor()
        {

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
#endif

        }

        public void ClearBar()
        {

#if UNITY_EDITOR
            UnityEditor.EditorUtility.ClearProgressBar();
#endif
        }


        void StartWriting(string name)
        {
            fileStream = new FileStream(name, FileMode.Create);
            byte emptyByte = new byte();

            for (int i = 0; i < headerSize; i++) //preparing the header
            {
                fileStream.WriteByte(emptyByte);
            }
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            if (recOutput)
            {
                if (ambisonics && ambisonicsSupportTest)
                {

                    for (int i = 0; i < data.Length; i += 4)
                    {
                        float fl = data[i];
                        float fr = data[i + 1];
                        float rl = data[i + 2];
                        float rr = data[i + 3];
                        //	Alternative mormalization mode
                        //						float frontSum = fl + fr;
                        //						float backSum = rl + rr;
                        //						float frontDiff = fl - fr;
                        //						float backDiff = rl - rr;

                        //			float w = (frontSum / 1.414214f + backSum)/4.828427f;
                        //			float ch2 = (frontSum - backSum) / 2.414214f;
                        //			float ch3 = (frontDiff + backDiff) / 3.146264f;
                        //			float ch4 = 0;


                        float w = 0.5f * (fl + rl + fr + rr);
                        float x = 0.5f * ((fl - rl) + (fr - rr));
                        float y = 0.5f * ((fl - rr) - (fr - rl));
                        float z = 0.5f * ((fl - rl) + (rr - fr));




                        data[i] = w;
                        data[i + 1] = y;
                        data[i + 2] = z;
                        data[i + 3] = x;
                    }
                }



                ConvertAndWrite(data);
            }
        }

        void ConvertAndWrite(float[] dataSource)
        {

            Int16[] intData = new Int16[dataSource.Length];


            Byte[] bytesData = new Byte[dataSource.Length * 2];


            int rescaleFactor = 32767;

            for (int i = 0; i < dataSource.Length; i++)
            {
                intData[i] = (short)(dataSource[i] * rescaleFactor);
                Byte[] byteArr = new Byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            fileStream.Write(bytesData, 0, bytesData.Length);
        }

        void WriteHeader()
        {

            fileStream.Seek(0, SeekOrigin.Begin);

            Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            fileStream.Write(riff, 0, 4);

            Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
            fileStream.Write(chunkSize, 0, 4);

            Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            fileStream.Write(wave, 0, 4);

            Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            fileStream.Write(fmt, 0, 4);

            Byte[] subChunk1 = BitConverter.GetBytes(16);
            fileStream.Write(subChunk1, 0, 4);

            //		UInt16 two = 2;
            UInt16 one = 1;
            UInt16 six = achannels;

            Byte[] audioFormat = BitConverter.GetBytes(one);
            fileStream.Write(audioFormat, 0, 2);

            Byte[] numChannels = BitConverter.GetBytes(six);
            fileStream.Write(numChannels, 0, 2);

            Byte[] sampleRate = BitConverter.GetBytes(outputRate);
            fileStream.Write(sampleRate, 0, 4);

            Byte[] byteRate = BitConverter.GetBytes(outputRate * 2 * achannels);


            fileStream.Write(byteRate, 0, 4);

            UInt16 four = (UInt16)(achannels + achannels);
            Byte[] blockAlign = BitConverter.GetBytes(four);
            fileStream.Write(blockAlign, 0, 2);

            UInt16 sixteen = 16;
            Byte[] bitsPerSample = BitConverter.GetBytes(sixteen);
            fileStream.Write(bitsPerSample, 0, 2);

            Byte[] dataString = System.Text.Encoding.UTF8.GetBytes("data");
            fileStream.Write(dataString, 0, 4);

            Byte[] subChunk2 = BitConverter.GetBytes(fileStream.Length - headerSize);
            fileStream.Write(subChunk2, 0, 4);

            fileStream.Close();
        }
        void OnGUI()
        {
            if (rt != null)
                GUI.DrawTexture(new Rect(0.0f, 0.0f, 300.0f, 300.0f), rt, ScaleMode.ScaleToFit, false, 0);
        }

        public void PreparePano()
        {

            if (ImageFormatType == VRFormatList.EXR_HDRI)
            {
                texFormat = TextureFormat.RGBAHalf;
                renderTexFormat = RenderTextureFormat.DefaultHDR;

            }
            else
            {
                if (!useAlpha)
                    texFormat = TextureFormat.RGB24;
                else texFormat = TextureFormat.ARGB32;

            }





            qualityTemp = resolution / 32 * renderQuality;
            rig = (GameObject)Instantiate(Resources.Load("Rig"));
            rig.hideFlags = HideFlags.HideInHierarchy;
            rig.name = "Rig";
            rig.transform.SetParent(transform, false);
            cam = gameObject;
            restoreFOV = cam.GetComponent<Camera>().fieldOfView;
            cam.GetComponent<Camera>().fieldOfView = 100;
            float IPDistanceV = -IPDistance;


            camll = GameObject.Find("Rig/Left");
            camrl = GameObject.Find("Rig/Right");
            camfl = GameObject.Find("Rig/Front");
            camtl = GameObject.Find("Rig/Top");
            cambl = GameObject.Find("Rig/Back");
            camdl = GameObject.Find("Rig/Down");





            cloneCamLL = Instantiate(cam);

            Destroy(cloneCamLL.GetComponent(typeof(Animator)));
            Destroy(cloneCamLL.GetComponent(typeof(Animation)));
            Destroy(cloneCamLL.GetComponent(typeof(VRCapture)));
            Destroy(cloneCamLL.GetComponent(typeof(AudioListener)));

            cloneCamRL = Instantiate(cam);
            Destroy(cloneCamRL.GetComponent(typeof(Animator)));
            Destroy(cloneCamRL.GetComponent(typeof(Animation)));
            Destroy(cloneCamRL.GetComponent(typeof(VRCapture)));
            Destroy(cloneCamRL.GetComponent(typeof(AudioListener)));

            cloneCamTL = Instantiate(cam);
            Destroy(cloneCamTL.GetComponent(typeof(Animator)));
            Destroy(cloneCamTL.GetComponent(typeof(Animation)));
            Destroy(cloneCamTL.GetComponent(typeof(VRCapture)));
            Destroy(cloneCamTL.GetComponent(typeof(AudioListener)));
            ///
            if (captureAngle == VRCaptureAngle._360)
            {
                cloneCamBL = Instantiate(cam);
                Destroy(cloneCamBL.GetComponent(typeof(Animator)));
                Destroy(cloneCamBL.GetComponent(typeof(Animation)));
                Destroy(cloneCamBL.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamBL.GetComponent(typeof(AudioListener)));
            }

            cloneCamFL = Instantiate(cam);
            Destroy(cloneCamFL.GetComponent(typeof(Animator)));
            Destroy(cloneCamFL.GetComponent(typeof(Animation)));
            Destroy(cloneCamFL.GetComponent(typeof(VRCapture)));
            Destroy(cloneCamFL.GetComponent(typeof(AudioListener)));

            cloneCamDL = Instantiate(cam);
            Destroy(cloneCamDL.GetComponent(typeof(Animator)));
            Destroy(cloneCamDL.GetComponent(typeof(Animation)));
            Destroy(cloneCamDL.GetComponent(typeof(VRCapture)));
            Destroy(cloneCamDL.GetComponent(typeof(AudioListener)));

            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {
                cloneCamLR = Instantiate(cam);
                Destroy(cloneCamLR.GetComponent(typeof(Animator)));
                Destroy(cloneCamLR.GetComponent(typeof(Animation)));
                Destroy(cloneCamLR.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamLR.GetComponent(typeof(AudioListener)));

                cloneCamRR = Instantiate(cam);
                Destroy(cloneCamRR.GetComponent(typeof(Animator)));
                Destroy(cloneCamRR.GetComponent(typeof(Animation)));
                Destroy(cloneCamRR.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamRR.GetComponent(typeof(AudioListener)));

                cloneCamTR = Instantiate(cam);
                Destroy(cloneCamTR.GetComponent(typeof(Animator)));
                Destroy(cloneCamTR.GetComponent(typeof(Animation)));
                Destroy(cloneCamTR.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamTR.GetComponent(typeof(AudioListener)));

                if (captureAngle == VRCaptureAngle._360)
                {
                    cloneCamBR = Instantiate(cam);
                    Destroy(cloneCamBR.GetComponent(typeof(Animator)));
                    Destroy(cloneCamBR.GetComponent(typeof(Animation)));
                    Destroy(cloneCamBR.GetComponent(typeof(VRCapture)));
                    Destroy(cloneCamBR.GetComponent(typeof(AudioListener)));
                }

                cloneCamFR = Instantiate(cam);
                Destroy(cloneCamFR.GetComponent(typeof(Animator)));
                Destroy(cloneCamFR.GetComponent(typeof(Animation)));
                Destroy(cloneCamFR.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamFR.GetComponent(typeof(AudioListener)));

                cloneCamDR = Instantiate(cam);
                Destroy(cloneCamDR.GetComponent(typeof(Animator)));
                Destroy(cloneCamDR.GetComponent(typeof(Animation)));
                Destroy(cloneCamDR.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamDR.GetComponent(typeof(AudioListener)));

                cloneCamDRX = Instantiate(cam);
                Destroy(cloneCamDRX.GetComponent(typeof(Animator)));
                Destroy(cloneCamDRX.GetComponent(typeof(Animation)));
                Destroy(cloneCamDRX.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamDRX.GetComponent(typeof(AudioListener)));

                cloneCamDLX = Instantiate(cam);
                Destroy(cloneCamDLX.GetComponent(typeof(Animator)));
                Destroy(cloneCamDLX.GetComponent(typeof(Animation)));
                Destroy(cloneCamDLX.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamDLX.GetComponent(typeof(AudioListener)));

                cloneCamTRX = Instantiate(cam);
                Destroy(cloneCamTRX.GetComponent(typeof(Animator)));
                Destroy(cloneCamTRX.GetComponent(typeof(Animation)));
                Destroy(cloneCamTRX.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamTRX.GetComponent(typeof(AudioListener)));

                cloneCamTLX = Instantiate(cam);
                Destroy(cloneCamTLX.GetComponent(typeof(Animator)));
                Destroy(cloneCamTLX.GetComponent(typeof(Animation)));
                Destroy(cloneCamTLX.GetComponent(typeof(VRCapture)));
                Destroy(cloneCamTLX.GetComponent(typeof(AudioListener)));
            }

            camLL = cloneCamLL.GetComponent<Camera>();
            camRL = cloneCamRL.GetComponent<Camera>();
            camTL = cloneCamTL.GetComponent<Camera>();
            if (captureAngle == VRCaptureAngle._360)
                camBL = cloneCamBL.GetComponent<Camera>();
            camFL = cloneCamFL.GetComponent<Camera>();
            camDL = cloneCamDL.GetComponent<Camera>();

            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {
                camLR = cloneCamLR.GetComponent<Camera>();
                camRR = cloneCamRR.GetComponent<Camera>();
                camTR = cloneCamTR.GetComponent<Camera>();
                if (captureAngle == VRCaptureAngle._360)
                    camBR = cloneCamBR.GetComponent<Camera>();
                camFR = cloneCamFR.GetComponent<Camera>();
                camDR = cloneCamDR.GetComponent<Camera>();

                camDRX = cloneCamDRX.GetComponent<Camera>();
                camDLX = cloneCamDLX.GetComponent<Camera>();
                camTRX = cloneCamTRX.GetComponent<Camera>();
                camTLX = cloneCamTLX.GetComponent<Camera>();
            }

            cloneCamLL.transform.SetParent(camll.transform, false);
            cloneCamRL.transform.SetParent(camrl.transform, false);
            cloneCamTL.transform.SetParent(camtl.transform, false);
            if (captureAngle == VRCaptureAngle._360)
                cloneCamBL.transform.SetParent(cambl.transform, false);
            cloneCamFL.transform.SetParent(camfl.transform, false);
            cloneCamDL.transform.SetParent(camdl.transform, false);

            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {
                cloneCamTLX.transform.SetParent(camtl.transform, false);
                cloneCamDLX.transform.SetParent(camdl.transform, false);
            }

            if (panoramaType == VRModeList.EquidistantMono) IPDistanceV = 0;

            Vector3 IPD = new Vector3(IPDistanceV, 0, 0);
            Vector3 IPDX = new Vector3(0, IPDistanceV, 0);


            cloneCamLL.transform.localPosition = -IPD / 2;
            cloneCamRL.transform.localPosition = -IPD / 2;
            cloneCamTL.transform.localPosition = -IPD / 2 * (-1f);
            if (captureAngle == VRCaptureAngle._360)
                cloneCamBL.transform.localPosition = -IPD / 2;
            cloneCamFL.transform.localPosition = -IPD / 2;
            cloneCamDL.transform.localPosition = -IPD / 2;

            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {
                cloneCamLR.transform.SetParent(camll.transform, false);
                cloneCamLR.transform.localPosition = IPD / 2;
                cloneCamRR.transform.SetParent(camrl.transform, false);
                cloneCamRR.transform.localPosition = IPD / 2;
                cloneCamTR.transform.SetParent(camtl.transform, false);
                cloneCamTR.transform.localPosition = IPD / 2 * (-1f);
                if (captureAngle == VRCaptureAngle._360)
                {
                    cloneCamBR.transform.SetParent(cambl.transform, false);
                    cloneCamBR.transform.localPosition = IPD / 2;
                }
                cloneCamFR.transform.SetParent(camfl.transform, false);
                cloneCamFR.transform.localPosition = IPD / 2;
                cloneCamDR.transform.SetParent(camdl.transform, false);
                cloneCamDR.transform.localPosition = IPD / 2;

                cloneCamDLX.transform.localPosition = -IPDX / 2;
                cloneCamTLX.transform.localPosition = -IPDX / 2;

                cloneCamTRX.transform.SetParent(camtl.transform, false);
                cloneCamTRX.transform.localPosition = IPDX / 2;
                cloneCamDRX.transform.SetParent(camdl.transform, false);
                cloneCamDRX.transform.localPosition = IPDX / 2;
            }






            //	renderHead = (GameObject)Instantiate(Resources.Load("360RenderHead"));
            if (captureType == VRCaptureList.VRPanoramaRT)
            {
                if (captureAngle == VRCaptureAngle._360)
                    renderPanorama = (GameObject)Instantiate(Resources.Load("360UnwrappedRT2"));
                else renderPanorama = (GameObject)Instantiate(Resources.Load("180UnwrappedRT2"));
            }
            else
            {
                if (captureAngle == VRCaptureAngle._360)
                    renderPanorama = (GameObject)Instantiate(Resources.Load("360Unwrapped"));
                else renderPanorama = (GameObject)Instantiate(Resources.Load("180Unwrapped"));
            }

            panoramaCam = GameObject.Find("PanoramaCamera");

            //      renderPanorama.hideFlags = HideFlags.HideInHierarchy;

            VRPanoInterface loader = renderPanorama.GetComponent<VRPanoInterface>();
            loader.height = resolutionH;
            loader.width = resolution;
            if (captureType == VRCaptureList.VRPanoramaRT)
            {
                if (panoramaType == VRModeList.EquidistantMono) loader.mono = true;
                if (panoramaType == VRModeList.EquidistantStereoSBS) loader.sbs = true;
            }

            if (panoramaType == VRModeList.EquidistantStereo)
            {
                loader.height = resolutionH / 2;
                loader.width = resolution;
            }

            if (panoramaType == VRModeList.EquidistantStereoSBS)
            {
                loader.height = resolutionH;
                loader.width = resolution / 2;
            }
            if (panoramaType == VRModeList.EquidistantStereo || panoramaType == VRModeList.EquidistantStereoSBS)
            {
                cloneCamFL.transform.LookAt(camfl.transform.position + camfl.transform.forward * EnvironmentDistance, camfl.transform.up);
                cloneCamFR.transform.LookAt(camfl.transform.position + camfl.transform.forward * EnvironmentDistance, camfl.transform.up);

                cloneCamLL.transform.LookAt(camll.transform.position + camll.transform.forward * EnvironmentDistance, camll.transform.up);
                cloneCamLR.transform.LookAt(camll.transform.position + camll.transform.forward * EnvironmentDistance, camll.transform.up);

                cloneCamRL.transform.LookAt(camrl.transform.position + camrl.transform.forward * EnvironmentDistance, camrl.transform.up);
                cloneCamRR.transform.LookAt(camrl.transform.position + camrl.transform.forward * EnvironmentDistance, camrl.transform.up);
                if (captureAngle == VRCaptureAngle._360)
                {
                    cloneCamBL.transform.LookAt(cambl.transform.position + cambl.transform.forward * EnvironmentDistance, cambl.transform.up);
                    cloneCamBR.transform.LookAt(cambl.transform.position + cambl.transform.forward * EnvironmentDistance, cambl.transform.up);
                }

                cloneCamTL.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);
                cloneCamTR.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);

                cloneCamDL.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);
                cloneCamDR.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);

                cloneCamTLX.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);
                cloneCamTRX.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);

                cloneCamDLX.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);
                cloneCamDRX.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);
            }

            if (panoramaType == VRModeList.EquidistantMono)
            {
                EnvironmentDistance = 10000;
                cloneCamFL.transform.LookAt(camfl.transform.position + camfl.transform.forward * EnvironmentDistance, camfl.transform.up);
                cloneCamLL.transform.LookAt(camll.transform.position + camll.transform.forward * EnvironmentDistance, camll.transform.up);
                cloneCamRL.transform.LookAt(camrl.transform.position + camrl.transform.forward * EnvironmentDistance, camrl.transform.up);
                if (captureAngle == VRCaptureAngle._360)
                    cloneCamBL.transform.LookAt(cambl.transform.position + cambl.transform.forward * EnvironmentDistance, cambl.transform.up);
                cloneCamTL.transform.LookAt(camtl.transform.position + camtl.transform.forward * EnvironmentDistance, camtl.transform.up);
                cloneCamDL.transform.LookAt(camdl.transform.position + camdl.transform.forward * EnvironmentDistance, camdl.transform.up);
            }
        }
    }
}



#endif

#if UNITY_WEBPLAYER

using UnityEngine;
using System.Collections;
using VRPanorama;

namespace VRPanorama {
	public class VRCapture : MonoBehaviour {
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}

#endif