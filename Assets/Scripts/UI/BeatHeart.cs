using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatHeart : MonoBehaviour
{
    [SerializeField] private Image _heart;
    [SerializeField] private List<Sprite> _heartSprites;

    private const int HEART_BAD_ID = 0;
    private const int HEART_GOOD_ID = 1;
    private const int HEART_GREAT_ID = 2;

    // Start is called before the first frame update
    void Start()
    {
        _heart.sprite = _heartSprites[HEART_BAD_ID];
    }
    
    // Update is called once per frame
    void Update()
    {
        switch(RhythmController.Instance.GetBeat())
        {
            case RhythmState.Bad:
                _heart.sprite = _heartSprites[HEART_BAD_ID];
                break;
            case RhythmState.Good:
                _heart.sprite = _heartSprites[HEART_GOOD_ID];
                break;
            case RhythmState.Great:
                _heart.sprite = _heartSprites[HEART_GREAT_ID];
                break;
        }
    }
}