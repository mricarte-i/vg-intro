using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonLayout : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;

    private static readonly Dictionary<string, string> _layoutDictionary = new Dictionary<string, string>
    {
        { "controller", "DPAD/LSTICK DOWN - Move Forward\nDPAD/LSTICK UP - Move Backwards\nDPAD/LSTICK LEFT - Move Left\nDPAD/LSTICK RIGHT - Move Right\nA - Jump\nY - Upper Attack\nX - Neutral Attack\nB - Lower Attack" },
        { "keyboard", "S - Move Forward\nW - Move Backwards\nA - Move Left\nD - Move Right\nSPACE- Jump\nP - Upper Attack\n I- Neutral Attack\nO - Lower Attack" }
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
