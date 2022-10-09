using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState {
    PAIR_CHAR_SELECT,
    STAGE_MODE_SELECT,
    MUSIC_BPM_SELECT,
};

public class SelectMenuConductor : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _vCam;
    [SerializeField] private MenuState _state = MenuState.PAIR_CHAR_SELECT;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
