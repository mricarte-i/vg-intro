using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonLayout : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;

    private static readonly Dictionary<string, string> _layoutDictionary = new Dictionary<string, string>
    {
        //{ "controller", "DPAD/LSTICK DOWN - Move Forward\nDPAD/LSTICK UP - Move Backwards\nDPAD/LSTICK LEFT - Move Left\nDPAD/LSTICK RIGHT - Move Right\nA - Jump\nY - Upper Attack\nX - Neutral Attack\nB - Lower Attack" },
        //{ "controller", "<sprite name=DpadDown>/<sprite name=LstickDown> - Move Forward\n<sprite name=DpadUp>/<sprite name=LstickUp> - Move Backwards\n<sprite name=DpadLeft>/<sprite name=LstickLeft> - Move Left\n<sprite name=DpadRight>/<sprite name=LstickRight> - Move Right\n<sprite name=JoyA> - Jump\n<sprite name=JoyY> - Upper Attack\n<sprite name=JoyX> - Neutral Attack\n<sprite name=JoyB> - Lower Attack" },
        { "controller", "<sprite name=\"DpadDown\" color=#000000>/<sprite name=\"LstickDown\" color=#000000> - Move Forward\n<sprite name=\"DpadUp\" color=#000000>/<sprite name=\"LstickUp\" color=#000000> - Move Backwards\n<sprite name=\"DpadLeft\" color=#000000>/<sprite name=\"LstickLeft\" color=#000000> - Move Left\n<sprite name=\"DpadRight\" color=#000000>/<sprite name=\"LstickRight\" color=#000000> - Move Right\n<sprite name=\"JoyA\" color=#000000> - Jump\n<sprite name=\"JoyY\" color=#000000> - Upper Attack\n<sprite name=\"JoyX\" color=#000000> - Neutral Attack\n<sprite name=\"JoyB\" color=#000000> - Lower Attack" },
        //{ "keyboard", "S - Move Forward\nW - Move Backwards\nA - Move Left\nD - Move Right\nSPACE- Jump\nP - Upper Attack\n I- Neutral Attack\nO - Lower Attack" }
        //{ "keyboard", "<sprite name=S> - Move Forward\n<sprite name=W> - Move Backwards\n<sprite name=A> - Move Left\n<sprite name=D> - Move Right\n<sprite name=SPACE> - Jump\n<sprite name=P> - Upper Attack\n <sprite name=I> - Neutral Attack\n<sprite name=O> - Lower Attack" }
        { "keyboard", "<sprite name=\"S\" color=#000000> - Move Forward\n<sprite name=\"W\" color=#000000> - Move Backwards\n<sprite name=\"A\" color=#000000> - Move Left\n<sprite name=\"D\" color=#000000> - Move Right\n<sprite name=\"SPACE\" color=#000000> - Jump\n<sprite name=\"P\" color=#000000> - Upper Attack\n <sprite name=\"I\" color=#000000> - Neutral Attack\n<sprite name=\"O\" color=#000000> - Lower Attack" }
    };

    void Start() {
        if (string.IsNullOrEmpty(this._textField.text)) {
            this._textField.text = _layoutDictionary["controller"];
        }
    }

    public void changeLayout(string newLayout)
    {
        this._textField.text = _layoutDictionary[newLayout];
    }
}
