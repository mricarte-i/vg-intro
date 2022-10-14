using UnityEngine;
using UnityEngine.UI;

public enum AudioType {
    MASTER,
    EFFECTS,
    MUSIC,
}
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioType _audioType;

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener(val => {
            switch(_audioType){
                case AudioType.MASTER:
                    AudioManager.Instance.ChangeMasterVolume(val);
                    break;
                case AudioType.EFFECTS:
                    AudioManager.Instance.ChangeEffectsVolume(val);
                    break;
                case AudioType.MUSIC:
                    AudioManager.Instance.ChangeMusicVolume(val);
                    break;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
