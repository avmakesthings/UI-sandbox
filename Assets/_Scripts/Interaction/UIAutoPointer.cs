using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class UIAutoPointer : VRTK_Pointer {

    UIAutoPointer autoPointer;
    VRTK_StraightPointerRenderer laserPointerRenderer;
    VRTK_BezierPointerRenderer teleportPointerRenderer;

    void Start()
    {
        //Get the pointer and renderers from the controller gameobject
        autoPointer = GetComponent<UIAutoPointer>();
        laserPointerRenderer = GetComponent<VRTK_StraightPointerRenderer>();
        teleportPointerRenderer = GetComponent<VRTK_BezierPointerRenderer>();

        if (autoPointer == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerPointerEvents_ListenerExample", "VRTK_DestinationMarker", "the Controller Alias"));
            return;
        }

        //Setup controller event listeners
        autoPointer.DestinationMarkerEnter += new DestinationMarkerEventHandler(DoPointerIn);
        autoPointer.DestinationMarkerExit += new DestinationMarkerEventHandler(DoPointerOut);

    }
    private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        if (e.target.GetComponent<VRTK_InteractableObject>())
        {
            autoPointer.pointerRenderer = laserPointerRenderer;

        }
    }

    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        if (autoPointer.enabled)
        {
            autoPointer.pointerRenderer = null;
        }

    }
}
