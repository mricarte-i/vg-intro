using UnityEngine;

/**
** based on
** https://web.archive.org/web/20201125013226/https://www.mattrifiedgames.com/tutorial-setting-up-a-3d-fighting-game-camera-using-cinemachine-in-unity3d/
**/
public class Align3DCam : MonoBehaviour
{

    [Tooltip("The transforms the camera aligns to.")]
    [SerializeField] private PlayerInputHandler _t1, _t2;

    private Cinemachine.CinemachineVirtualCamera _vCam;

    private Cinemachine.CinemachineTransposer _transposer;

    private float _distT1T2;
    private bool _hasVCam;
    private bool _hasTransposer;

    [SerializeField] private Vector3 _framingNormal;


    [Header("y = mx + b")]

    [Tooltip("Slope value (m) of the linear equation used to determine how far the camera should be based on the distance between the target transforms.")]
    [SerializeField] private float _transposerLinearSlope;

    [Tooltip("Offset value (b) of the linear equation used to determine how far the camera should be based on the distance between the target transforms.")]
    [SerializeField] private float _transposerLinearOffset;

    [Header("Framing helpers")]

    [SerializeField] private float _minDistance;

    void Awake()
    {
        _hasVCam = _vCam != null;
        if(_hasVCam){
            _transposer = _vCam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();

            if(_transposer == null){
                _hasVCam = false;
            }else{
                _framingNormal = _transposer.m_FollowOffset;
                _framingNormal.Normalize();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = _t1.transform.position - _t2.transform.position;
        _distT1T2 = diff.magnitude;

        diff.y = 0f;
        diff.Normalize();

        if(_hasVCam){
            _transposer.m_FollowOffset = _framingNormal * (Mathf.Max(_minDistance, _distT1T2) * _transposerLinearSlope + _transposerLinearOffset);

        }

        if(Mathf.Approximately(0f, diff.sqrMagnitude)){
            return;
        }

        Quaternion q  = Quaternion.LookRotation(diff, Vector3.up) * Quaternion.Euler(0, 90, 0);

        Quaternion qA = q * Quaternion.Euler(0, 180, 0);

        float angle = Quaternion.Angle(q, transform.rotation);
        float angleA = Quaternion.Angle(qA, transform.rotation);

        transform.rotation = angle < angleA ? q : qA;
    }
}
