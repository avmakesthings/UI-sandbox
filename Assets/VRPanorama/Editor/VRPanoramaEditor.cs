
#if UNITY_EDITOR && !UNITY_WEBPLAYER

using UnityEngine;
using System.Collections;
using UnityEditor;
using VRPanorama;

#if UNITY_5_4_OR_NEWER


using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

#endif

using System.Diagnostics;


using System.IO;


namespace VRPanorama
{

    [CustomEditor(typeof(VRCapture))]
    [RequireComponent(typeof(AudioListener))]

    public class VRPanoramaEditor : Editor
    {
        private Texture banner;
        private bool changePrefix = false;


        void OnEnable()
        {
            banner = Resources.Load("VRHeader") as Texture;
        }






        public override void OnInspectorGUI()
        {



            VRCapture VRP = (VRCapture)target;
            //	VRP.fullPath = Path.GetFullPath(string.Format(@"{0}/", VRP.Folder));

            if (VRP.ImageFormatType == VRPanorama.VRCapture.VRFormatList.JPG)
            {
                VRP.formatString = ".jpg\"";

            }

            else if (VRP.ImageFormatType == VRPanorama.VRCapture.VRFormatList.PNG)
            {
                VRP.formatString = ".png\"";

            }

            else
            {
                VRP.formatString = ".exr\"";

            }


            GUILayout.Box(banner, GUILayout.ExpandWidth(true));



            GUILayout.BeginVertical("box");

            GUILayout.Label("VR Panorama");
            VRP.captureType = (VRPanorama.VRCapture.VRCaptureList)EditorGUILayout.EnumPopup("Capture Mode", VRP.captureType);
            VRP.panoramaType = (VRPanorama.VRCapture.VRModeList)EditorGUILayout.EnumPopup("Capture Type", VRP.panoramaType);
            if (VRP.panoramaType != VRPanorama.VRCapture.VRModeList.VideoCapture)
            VRP.captureAngle = (VRPanorama.VRCapture.VRCaptureAngle)EditorGUILayout.EnumPopup("Capture Angle", VRP.captureAngle);

            if (VRP.captureType == VRPanorama.VRCapture.VRCaptureList.StillImage) VRP.captureKey = (KeyCode)EditorGUILayout.EnumPopup("Still Image Capture Key", VRP.captureKey);

            if (VRP.captureType != VRPanorama.VRCapture.VRCaptureList.VRPanoramaRT)
            {
                VRP.ImageFormatType = (VRPanorama.VRCapture.VRFormatList)EditorGUILayout.EnumPopup("Sequence Format", VRP.ImageFormatType);
                if (VRP.ImageFormatType == VRPanorama.VRCapture.VRFormatList.JPG)
                {
                    VRP.jpgQuality = EditorGUILayout.IntSlider("JPG Quality", VRP.jpgQuality, 1, 100);
                }

                VRP.Folder = EditorGUILayout.TextField(new GUIContent("Save to Folder", "Store image sequence in this folder(has to be inside unity project folder)"), VRP.Folder);

                VRP.Folder = VRP.Folder.Replace(" ", "_");






                VRP.customPath = EditorGUILayout.Toggle(new GUIContent("Use Custom Path", "Use external folder Path"), VRP.customPath);
                if (VRP.customPath)
                    VRP.customPathFolder = EditorGUILayout.TextField(new GUIContent("Custom Path", "Custom path outside of unity project folder()"), VRP.customPathFolder);
                VRP.fullPath = VRP.customPathFolder + VRP.Folder + "/";
                if (!VRP.customPath)
                    VRP.fullPath = Path.GetFullPath(string.Format(@"{0}/", VRP.Folder));
                GUILayout.Label(VRP.fullPath);

                VRP.openDestinationFolder = EditorGUILayout.Toggle(new GUIContent("Open Destination Folder", ""), VRP.openDestinationFolder);
            }
            VRP.resolution = EditorGUILayout.IntField(new GUIContent("Resolution W", "Panorama width resolution"), VRP.resolution);




            VRP.resolutionH = EditorGUILayout.IntField(new GUIContent("Resolution H", "Panorama height resolution"), VRP.resolutionH);



            GUILayout.BeginVertical("box");
            GUILayout.Label("Resolution Presets");

            if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantMono || VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereo || VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereoSBS)
            {
                if (GUILayout.Button("HD 720p"))
                {
                    VRP.resolution = 1280;
                    VRP.resolutionH = 720;
                    VRP.Mp4Bitrate = 5000;
                }
                if (GUILayout.Button("HD 1080p"))
                {
                    VRP.resolution = 1920;
                    VRP.resolutionH = 1080;
                    VRP.Mp4Bitrate = 8000;
                }

                if (GUILayout.Button("QHD/ 2K"))
                {
                    VRP.resolution = 2560;
                    VRP.resolutionH = 1440;
                    VRP.Mp4Bitrate = 16000;
                }

                if (GUILayout.Button("UHD 4K"))
                {
                    VRP.resolution = 3840;
                    VRP.resolutionH = 2160;
                    VRP.Mp4Bitrate = 35000;
                }
                if (GUILayout.Button("Youtube 8K"))
                {
                    VRP.resolution = 7680;
                    VRP.resolutionH = 4320;
                    VRP.Mp4Bitrate = 60000;
                }
                if (GUILayout.Button("Facebook Image"))
                {
                    VRP.resolution = 6000;
                    VRP.resolutionH = 3000;
                    VRP.Mp4Bitrate = 55000;
                }
                if (GUILayout.Button("2048 X 2048"))
                {
                    VRP.resolution = 2048;
                    VRP.resolutionH = 2048;
                    VRP.Mp4Bitrate = 30000;
                }

                if (GUILayout.Button("4096 X 4096"))
                {
                    VRP.resolution = 4096;
                    VRP.resolutionH = 4096;
                    VRP.Mp4Bitrate = 40000;
                }

                if (GUILayout.Button("8192 X 8192"))
                {
                    VRP.resolution = 8192;
                    VRP.resolutionH = 8192;
                    VRP.Mp4Bitrate = 50000;
                }


            }

            if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.VideoCapture)
            {
                if (GUILayout.Button("HD 720p"))
                {
                    VRP.resolution = 1280;
                    VRP.resolutionH = 720;
                    VRP.Mp4Bitrate = 5000;
                }
                if (GUILayout.Button("HD 1080p"))
                {
                    VRP.resolution = 1920;
                    VRP.resolutionH = 1080;
                    VRP.Mp4Bitrate = 8000;
                }

                if (GUILayout.Button("QHD/ 2K"))
                {
                    VRP.resolution = 2560;
                    VRP.resolutionH = 1440;
                    VRP.Mp4Bitrate = 16000;
                }

                if (GUILayout.Button("UHD 4K"))
                {
                    VRP.resolution = 3840;
                    VRP.resolutionH = 2160;
                    VRP.Mp4Bitrate = 35000;
                }
                if (GUILayout.Button("Youtube 8K"))
                {
                    VRP.resolution = 7680;
                    VRP.resolutionH = 4320;
                    VRP.Mp4Bitrate = 60000;
                }



            }


            GUILayout.EndVertical();
            if (VRP.captureType == VRPanorama.VRCapture.VRCaptureList.AnimationCapture)
            {
                VRP.FPS = EditorGUILayout.IntField(new GUIContent("FPS", "Animation framerate"), VRP.FPS);

                GUILayout.BeginHorizontal("box");
                GUILayout.Label("FPS Presets");


                if (GUILayout.Button("24"))
                    VRP.FPS = 24;

                if (GUILayout.Button("25"))
                    VRP.FPS = 25;
                if (GUILayout.Button("30"))
                    VRP.FPS = 30;
                if (GUILayout.Button("50"))
                    VRP.FPS = 50;
                if (GUILayout.Button("60"))
                    VRP.FPS = 60;
                if (GUILayout.Button("75"))
                    VRP.FPS = 75;
                if (GUILayout.Button("90"))
                    VRP.FPS = 90;
                if (GUILayout.Button("100"))
                    VRP.FPS = 100;




                GUILayout.EndHorizontal();


                VRP.NumberOfFramesToRender = EditorGUILayout.IntField(new GUIContent("Number of frames to render", "Number of frames to render"), VRP.NumberOfFramesToRender);
                VRP.renderFromFrame = EditorGUILayout.IntField(new GUIContent("Resume fromframe", "Resume rendering from frame. This has to be used as resume option after crash or user cancel"), VRP.renderFromFrame);


                float sequenceTime = ((float)VRP.NumberOfFramesToRender - (float)VRP.renderFromFrame) / (float)VRP.FPS;
                int minutesSeq = (int)sequenceTime / 60;
                int secondsSeq = (int)sequenceTime % 60;

                int frames = (VRP.NumberOfFramesToRender - VRP.renderFromFrame) - (minutesSeq * VRP.FPS * 60 + secondsSeq * VRP.FPS);

                string sequenceLength = (minutesSeq + " min. " + secondsSeq + " sec. " + frames + " frames");


                GUILayout.Label("Sequence Length: " + sequenceLength);

            }

            GUILayout.EndVertical();
            if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.VideoCapture)
                VRP.alignPanoramaWithHorizont = false;
            if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereo || VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereoSBS || VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantMono)
            {

                GUILayout.BeginVertical("box");
                GUILayout.BeginVertical("box");
                GUILayout.Label("Panorama Settings");
                GUILayout.EndVertical();
                if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereo || VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereoSBS) VRP.IPDistance = EditorGUILayout.FloatField(new GUIContent("IP Distance", "Interpupilar distance"), VRP.IPDistance);
                if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereo || VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereoSBS) VRP.EnvironmentDistance = EditorGUILayout.FloatField(new GUIContent("Environment Distance", "Distance where stiching is perfect: adjust in base of your scene"), VRP.EnvironmentDistance);
                VRP.alignPanoramaWithHorizont = EditorGUILayout.Toggle(new GUIContent("Align with Horizont", "Forces camera to be aligned with horizont by forcing only rotations on Y axis, usefull fhen using existing animations that have camera X or Z rotations"), VRP.alignPanoramaWithHorizont);

                GUILayout.EndVertical();

            }



            if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantMono)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Optimizations");
                VRP.depth = EditorGUILayout.Toggle(new GUIContent("Use Depth buffer", ""), VRP.depth);
         
                VRP.useAlpha = EditorGUILayout.Toggle(new GUIContent("Use Alpha (PNG)", ""), VRP.useAlpha);
                VRP.disableTracking = EditorGUILayout.Toggle(new GUIContent("Disable VR Tracking", ""), VRP.disableTracking);
                VRP.renderQuality = EditorGUILayout.IntSlider("Speed vs.Quality", VRP.renderQuality, 1, 32);
                string ssaa = "  /SS Anti-Aliasing: " + VRP.renderQuality / 8f + "X";
                string q = "Lowest quality" + ssaa;
                if (VRP.renderQuality > 1)
                    q = "low quality preview" + ssaa;
                if (VRP.renderQuality > 14)
                    q = "optimal" + ssaa;
                if (VRP.renderQuality > 18)
                    q = "best and slow" + ssaa;

                GUILayout.Label("Quality: " + q);
                int size = VRP.resolution / 32 * VRP.renderQuality;
                if (size > 8192)
                    size = 8192;
                GUILayout.Label("One cube side is: " + size + "x" + size);


                int depthValue = 0;
                if (VRP.depth) depthValue = 1;
                int vramSize = SystemInfo.graphicsMemorySize;
                int vramUsage;
                if (!VRP.depth) vramUsage = (((size * size * (3 + depthValue)) / 1024 / 1024) * 6 + ((VRP.resolution * VRP.resolutionH * 3) / 1024 / 1024));
                else vramUsage = (((size * size * (3 + depthValue)) / 1024 / 1024) * 6 + ((VRP.resolution * VRP.resolutionH * 3) / 1024 / 1024) + ((size * size * (3)) / 1024 / 1024) * 6);
                GUILayout.Label("VRAM Usage: " + vramUsage + " MB");
                GUILayout.Label("VRAM available: " + (vramSize - vramUsage) + " from " + vramSize);

                if (SystemInfo.graphicsMemorySize - 256 < vramUsage)
                {

                    GUILayout.Label("WARNING! You are running Low on VRAM!");
                    GUILayout.Label("TIP: Turn off Depth buffer or lower Quality");

                }


                //				if (GUILayout.Button("Transmit Livestream")) {
                //					
                //	
                //					string ffmpegPath = Application.dataPath + "\\VRPanorama\\StreamingAssets\\";
                //
                //
                //					System.Diagnostics.Process.Start( ffmpegPath + "ffmpeg" , "rtbufsize 1500M -f gdigrab -r 25 -video_size 1080x720 -i desktop -f dshow -i audio=\"Microfono\" -vcodec h264 -b:v 1800k  -f flv \"rtmp://a.rtmp.youtube.com/live2/4bgd-6g9e-pw1m-9h81\"");
                //					
                //				}


                GUILayout.EndVertical();
            }



            if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereo || VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereoSBS)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Optimizations");

                VRP.depth = EditorGUILayout.Toggle(new GUIContent("Use Depth buffer", ""), VRP.depth);
                VRP.useAlpha = EditorGUILayout.Toggle(new GUIContent("Use Alpha (PNG)", ""), VRP.useAlpha);

                if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereo || VRP.panoramaType == VRPanorama.VRCapture.VRModeList.EquidistantStereoSBS)
                {
                    VRP.hQ = EditorGUILayout.Toggle(new GUIContent("HQ stereo Stitch (experimental-slow)", ""), VRP.hQ);
                    if (VRP.hQ)
                    {
                        VRP.steps = EditorGUILayout.IntSlider("Render slices", VRP.steps, 2, 18);
                        VRP.smoothing = EditorGUILayout.Slider("Smoothing", VRP.smoothing, 0, 5);
                    }
                }
                else VRP.hQ = false;
          //      VRP.camBL.targetDisplay = EditorGUILayout.PropertyField(Camera.);
                VRP.disableTracking = EditorGUILayout.Toggle(new GUIContent("Disable VR Tracking", ""), VRP.disableTracking);
                VRP.renderQuality = EditorGUILayout.IntSlider("Speed vs.Quality", VRP.renderQuality, 1, 32);
                string ssaa = "  /SS Anti-Aliasing: " + VRP.renderQuality / 8f + "X";
                string q = "Lowest quality" + ssaa;
                if (VRP.renderQuality > 1)
                    q = "low quality preview" + ssaa;
                if (VRP.renderQuality > 14)
                    q = "optimal" + ssaa;
                if (VRP.renderQuality > 18)
                    q = "best and slow" + ssaa;

                GUILayout.Label("Quality: " + q);
                int size = VRP.resolution / 32 * VRP.renderQuality;
                if (size > 8192)
                    size = 8192;

                GUILayout.Label("One cube side is: " + size + "x" + size);
                int depthValue = 0;
                if (VRP.depth) depthValue = 1;
                int vramSize = SystemInfo.graphicsMemorySize;
                int vramUsage;
                if (!VRP.depth) vramUsage = (((size * size * (3 + depthValue)) / 1024 / 1024) * 16 + ((VRP.resolution * VRP.resolutionH * 3) / 1024 / 1024));
                else vramUsage = (((size * size * (3 + depthValue)) / 1024 / 1024) * 16 + ((VRP.resolution * VRP.resolutionH * 3) / 1024 / 1024) + ((size * size * (3)) / 1024 / 1024) * 16);
                GUILayout.Label("VRAM Usage: " + vramUsage + " MB");
                GUILayout.Label("VRAM available: " + (vramSize - vramUsage) + " from " + vramSize);

                if (SystemInfo.graphicsMemorySize - 256 < vramUsage)
                {

                    GUILayout.Label("WARNING! You are running Low on VRAM!");
                    GUILayout.Label("TIP: Turn off Depth buffer or lower Quality");

                }



                GUILayout.EndVertical();
            }

            if (VRP.panoramaType == VRPanorama.VRCapture.VRModeList.VideoCapture)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Optimizations");
                VRP.depth = EditorGUILayout.Toggle(new GUIContent("Use Depth buffer", ""), VRP.depth);
                VRP.renderQuality = EditorGUILayout.IntSlider("Speed vs.Quality", VRP.renderQuality, 1, 32);
                string ssaa = "  /SS Anti-Aliasing: " + VRP.renderQuality / 8f + "X";
                string q = "Lowest quality" + ssaa;
                if (VRP.renderQuality > 1)
                    q = "low quality preview" + ssaa;
                if (VRP.renderQuality > 14)
                    q = "optimal" + ssaa;
                if (VRP.renderQuality > 18)
                    q = "best and slow" + ssaa;

                GUILayout.Label("Quality: " + q);
                int size = VRP.resolution / 32 * VRP.renderQuality;
                if (size > 8192)
                    size = 8192;


                GUILayout.EndVertical();
            }


            if (VRP.captureType == VRPanorama.VRCapture.VRCaptureList.AnimationCapture || VRP.captureType == VRPanorama.VRCapture.VRCaptureList.VRPanoramaRT)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Audio");

                VRP.volume = EditorGUILayout.Slider("Audio Volume", VRP.volume, 0, 1);
                if ((int)AudioSettings.speakerMode == 3)
                    VRP.ambisonics = EditorGUILayout.Toggle(new GUIContent("Export Ambisonics 4.0", ""), VRP.ambisonics);
                else
                {
                    GUILayout.BeginVertical("box");
                    GUILayout.Label("Ambisonics audio unavailable!");
                    GUILayout.Label("Please, set default speaker mode to QUAD.");

                    GUILayout.EndVertical();
                }
                VRP.mute = EditorGUILayout.Toggle(new GUIContent("Mute while Rendering", ""), VRP.mute);




                GUILayout.EndVertical();
            }

            if (VRP.captureType == VRPanorama.VRCapture.VRCaptureList.AnimationCapture)
            {
                GUILayout.BeginVertical("box");

                VRP.encodeToMp4 = EditorGUILayout.Toggle(new GUIContent("Encode H.264 video after rendering", "Encode H.246 video after rendering finishes"), VRP.encodeToMp4);
                GUILayout.Label("H.264 Movie Export Options");
                VRP.Mp4Bitrate = EditorGUILayout.IntField(new GUIContent("H.264 Bitrate", "MP4 Bitrate"), VRP.Mp4Bitrate);

                //	string fullPath = Path.GetFullPath(string.Format(@"{0}/", VRP.Folder));
                string ffmpegPath = Application.dataPath + "\\VRPanorama\\StreamingAssets\\";

                if (GUILayout.Button("Encode H.246 Video From Existing sequence "))
                {

                    if (System.IO.File.Exists(VRP.Folder + "/" + VRP.Folder + ".wav"))
                    {

                        System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + VRP.FPS + " -i \"" + VRP.fullPath + VRP._prefix + "%05d" + VRP.formatString + " -i \"" + VRP.fullPath + VRP.Folder + ".wav" + "\"" + " -r " + VRP.FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + VRP.Mp4Bitrate + "k" + " -c:a aac  -strict experimental  -b:a 192k -shortest " + " \"" + VRP.fullPath + VRP.Folder + ".mp4\"");
                    }
                    else System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + VRP.FPS + " -i \"" + VRP.fullPath + VRP._prefix + "%05d" + VRP.formatString + " -r " + VRP.FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + VRP.Mp4Bitrate + "k" + " \"" + VRP.fullPath + VRP.Folder + ".mp4\"");
                }

                if (GUILayout.Button("Create GIF animation "))
                {


                    System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + VRP.FPS + " -i \"" + VRP.fullPath + VRP._prefix + "%05d" + VRP.formatString + "" + " \"" + VRP.fullPath + VRP.Folder + ".gif\"");
                }



                if (GUILayout.Button("Encode best quality video for Gear VR from 4k sequence"))
                {

                    if (System.IO.File.Exists(VRP.Folder + "/" + VRP.Folder + ".wav"))
                    {


                        System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + VRP.FPS + " -i \"" + VRP.fullPath + VRP._prefix + "%05d" + VRP.formatString + " -i \"" + VRP.fullPath + VRP.Folder + ".wav" + "\"" + " -vf scale=3480:1920 " + " -r " + VRP.FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + VRP.Mp4Bitrate + "k" + " -c:a aac -strict experimental -b:a 192k -shortest " + " \"" + VRP.fullPath + VRP.Folder + "_360_TB.mp4\"");

                    }

                    else System.Diagnostics.Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + VRP.FPS + " -i \"" + VRP.fullPath + VRP._prefix + "%05d" + VRP.formatString + " -vf scale=3480:1920 " + " -r " + VRP.FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + VRP.Mp4Bitrate + "k \"" + VRP.fullPath + VRP.Folder + "_360_TB.mp4\"");
                }



                GUILayout.EndVertical();



#if !UNITY_EDITOR_OSX
                if (GUILayout.Button("Add 360 image metatags for Facebook"))
                {
                    Process compiler = new Process();
                    compiler.StartInfo.FileName = ffmpegPath + "exiftool";
                    compiler.StartInfo.Arguments = "-overwrite_original -ProjectionType=\"equirectangular\" " + VRP.fullPath;
                    compiler.StartInfo.UseShellExecute = false;
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    compiler.Start();

                    UnityEngine.Debug.Log(compiler.StandardOutput.ReadToEnd());

                    compiler.WaitForExit();
                }
#endif

                if (GUILayout.Button("Spatial Media Metadata Injector"))
                {



                    //		System.Diagnostics.Process.Start( ffmpegPath + "Spatial Media Metadata Injector");
                    System.Diagnostics.Process.Start(ffmpegPath + "Spatial Media Metadata Injector");



                }


                GUIStyle style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = Color.red;



                GUILayout.BeginVertical("box");

                VRP.mailme = EditorGUILayout.Toggle(new GUIContent("Notify by Email", "Notify by Email when rendering finishes"), VRP.mailme);
                if (VRP.mailme)
                {
                    VRP._mailto = EditorGUILayout.TextField(new GUIContent("Send confirmation Email TO:", "should be something like username@gmail.com"), VRP._mailto);
                    GUILayout.BeginVertical("box");

                    GUILayout.Label("GMAIL account settings");
                    VRP._mailfrom = EditorGUILayout.TextField(new GUIContent("FROM Gmail Adress:", "should be something like username@gmail.com"), VRP._mailfrom);
                    VRP._pass = EditorGUILayout.PasswordField(new GUIContent("Gmail Password:", "Don't worry, you are the only one to know it!"), VRP._pass);





                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("box");

                changePrefix = EditorGUILayout.Toggle(new GUIContent("Change image name prefix", "Change image name prefix"), changePrefix);
                if (changePrefix == true)
                {
                    GUILayout.BeginVertical("box");
                    VRP._prefix = EditorGUILayout.TextField(new GUIContent("Image name prefix", "Image name prefix followed by 5 numbers"), VRP._prefix);
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();

                if (GUILayout.Button("Open destination folder"))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = VRP.fullPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }


                if (GUILayout.Button("Delete All Images"))
                {
                    var list = System.IO.Directory.GetFiles(VRP.fullPath, "*.jpg");
                    foreach (var item in list)
                    {
                        System.IO.File.Delete(item);
                    }
                    var listPNG = System.IO.Directory.GetFiles(VRP.fullPath, "*.png");
                    foreach (var item in listPNG)
                    {
                        System.IO.File.Delete(item);
                    }
                    var listEXR = System.IO.Directory.GetFiles(VRP.fullPath, "*.exr");
                    foreach (var item in listEXR)
                    {
                        System.IO.File.Delete(item);
                    }
                }

                if (GUILayout.Button("Capture Audio", style))
                {
                    VRP.captureAudio = true;
                    UnityEditor.EditorApplication.isPlaying = true;
                }

                if (GUILayout.Button("Render Panorama", style))
                {
                    VRP.captureAudio = false;
                    UnityEditor.EditorApplication.isPlaying = true;
                }




            }
            if (VRP.captureType == VRPanorama.VRCapture.VRCaptureList.StillImage)
            {
                if (GUILayout.Button("Open destination folder"))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = VRP.fullPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(VRP);
#if UNITY_5_4_OR_NEWER
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }
        }




    }
}

#endif

#if UNITY_WEBPLAYER

using UnityEngine;
using System.Collections;
using UnityEditor;
using VRPanorama;

namespace VRPanorama {
	
	[CustomEditor(typeof(VRCapture))]
	
	public class VRPanoramaEditor : Editor 
	{
		private Texture banner = Resources.Load("VRHeader") as Texture;
		
		
		// Use this for initialization
		public override void OnInspectorGUI () {
			GUILayout.Box (banner, GUILayout.ExpandWidth(true));
			GUILayout.Label ("VR Panorama can't be initialised on Webplayer platform, please change your buiding mode to Standalone platform");
			Debug.LogError ("VR Panorama can't be initialised on Webplayer platform, please change your buiding mode to Standalone (under Build Settings/Platform/Standalone - Switch platform");
			
		}
	}
}

#endif
