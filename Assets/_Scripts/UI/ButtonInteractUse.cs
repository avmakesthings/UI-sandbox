using UnityEngine;
using UnityEngine.Events;
using VRTK;



//Custom button class for non-UnityUI objects to handle VRTK pointer interaction
public class ButtonInteractUse : VRTK_InteractableObject
{

    //public color to indicate use
    public Color buttonActiveColor;
    public UnityEvent onButtonActivated;

    private static Color buttonInactiveColor;

    void Awake()
    {
        if (onButtonActivated == null)
            onButtonActivated = new UnityEvent();
    }

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = buttonActiveColor;
        onButtonActivated.Invoke();
    }

    public override void StopUsing(VRTK_InteractUse usingObject)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = buttonInactiveColor;
        onButtonActivated.Invoke();
    }

    protected void Start()
    {
        buttonInactiveColor = gameObject.GetComponent<MeshRenderer>().material.color;
    }

}