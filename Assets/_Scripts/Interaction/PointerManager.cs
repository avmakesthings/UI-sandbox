using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


//class that manages pointer renderer implementation based on events

public class PointerManager : MonoBehaviour {

    VRTK_Pointer laserPointer;
    VRTK_Pointer teleportPointer;
    VRTK_UIPointer uiPointer;
    VRTK_StraightPointerRenderer laserPointerRenderer;
    VRTK_BezierPointerRenderer teleportPointerRenderer;




    // Use this for initialization
    void Start () {

        laserPointerRenderer = GetComponent<VRTK_StraightPointerRenderer>();
        teleportPointerRenderer = GetComponent<VRTK_BezierPointerRenderer>();

        VRTK_Pointer[] pointers = GetComponents<VRTK_Pointer>();

        uiPointer = GetComponent<VRTK_UIPointer>();

        foreach (VRTK_Pointer p in pointers) {

            if (p.pointerRenderer == laserPointerRenderer)
            {
                laserPointer = p;

            }else if (p.pointerRenderer == teleportPointerRenderer)
            {
                teleportPointer = p;
            }
        }

        if (laserPointer == null || teleportPointer == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerPointerEvents_ListenerExample", "VRTK_DestinationMarker", "the Controller Alias"));
            return;
        }

        

        //Setup controller event listeners
        laserPointer.DestinationMarkerEnter += new DestinationMarkerEventHandler(DoLaserPointerIn);
        laserPointer.DestinationMarkerExit += new DestinationMarkerEventHandler(DoLaserPointerOut);
        teleportPointer.DestinationMarkerEnter += new DestinationMarkerEventHandler(DoTeleportPointerIn);
        teleportPointer.DestinationMarkerExit += new DestinationMarkerEventHandler(DoTeleportPointerOut);
        //uiPointer.UIPointerElementEnter += new UIPointerElementEnter(DoTeleportPointerIn);
        //uiPointer.DestinationMarkerExit += new DestinationMarkerEventHandler(DoTeleportPointerOut);

    }

    //TO-FIX - filed bug with thestonefox.  pointer doesn't draw onEnable, only when trigger is pressed.
    private void DoLaserPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        //print("DoLaserPointerIn");
        laserPointerRenderer.invalidCollisionColor = new Color(255, 255, 255, 1f);
    }

    private void DoLaserPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        //print("DoLaserPointerOut");
        laserPointerRenderer.invalidCollisionColor = new Color(255, 255, 255, 0f);

    }

    private void DoTeleportPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        //print("DoTeleportPointerIn");
    }

    private void DoTeleportPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        //print("DoTeleportPointerOut");
    }



}
