//based on VRTK keyboard example

namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.UI;

    public class Input_VRKeyboard : MonoBehaviour
    {

        public InputField input;

        public void ClickKey(string character)
        {
            input.text += character;
        }

        public void Backspace()
        {
            if (input.text.Length > 0)
            {
                input.text = input.text.Substring(0, input.text.Length - 1);
            }
        }

        public void Enter()
        {
            VRTK_Logger.Info("You've typed [" + input.text + "]");
            input.text = "";
        }

        public void ActivateInputField(InputField newInput)
        {
            input = newInput;
        }

    }
}
