using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PointerRendererSwitch : VRTK_Pointer {

    PointerRendererSwitch myPointer;
    VRTK_StraightPointerRenderer laserPointerRenderer;
    VRTK_BezierPointerRenderer teleportPointerRenderer;

	// Use this for initialization
	void Start () {
        //Get the pointer and renderers from the controller gameobject
        myPointer = GetComponent<PointerRendererSwitch>();
        laserPointerRenderer = GetComponent<VRTK_StraightPointerRenderer>();
        teleportPointerRenderer = GetComponent<VRTK_BezierPointerRenderer>();

        if (myPointer == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerPointerEvents_ListenerExample", "VRTK_DestinationMarker", "the Controller Alias"));
            return;
        }

        //Setup controller event listeners
        myPointer.DestinationMarkerEnter += new DestinationMarkerEventHandler(DoPointerIn);
        myPointer.DestinationMarkerExit += new DestinationMarkerEventHandler(DoPointerOut);
        
    }
	

    private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        if (e.target.GetComponent<VRTK_InteractableObject>())
        {
            laserPointerRenderer.enabled = true;
            myPointer.interactWithObjects = true;
            myPointer.enableTeleport = false;
            myPointer.pointerRenderer = laserPointerRenderer;

        }
    }

    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        if (myPointer.pointerRenderer == laserPointerRenderer)
        {
            laserPointerRenderer.enabled = false;
            myPointer.interactWithObjects = false;
            myPointer.enableTeleport = true;
            myPointer.pointerRenderer = teleportPointerRenderer;
        }

    }











}
