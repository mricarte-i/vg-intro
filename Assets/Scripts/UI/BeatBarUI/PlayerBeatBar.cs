using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBeatBar : MonoBehaviour
{
    [SerializeField] private PlayerId _playerId;
    [SerializeField] private GameObject _beatSpawnPos, _beatEndPos;
    [SerializeField] private Image _latencySafeZone;
    [SerializeField] private GameObject _renderedBeatPrefab; //has RenderedBeat component!

    private List<GameObject> _renderedBeats = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateBasePositions();
    }

    private void UpdateBasePositions(){
        var latencySafeZoneWidth = (_latencySafeZone.rectTransform.anchorMax.x - _latencySafeZone.rectTransform.anchorMin.x)*Screen.width;
        var latencySafeZonePosition = new Vector2(_latencySafeZone.rectTransform.anchorMin.x * Screen.width, _latencySafeZone.rectTransform.anchorMin.y * Screen.height);

        if(_playerId == PlayerId.P1) {
            _latencySafeZone.fillOrigin = (int) Image.OriginHorizontal.Right;
            _latencySafeZone.fillAmount = RhythmController.Instance.LatencyThreshold * 2;

            var beatEndNewPos = _beatEndPos.transform.position;
            beatEndNewPos.x = latencySafeZonePosition.x +  latencySafeZoneWidth * (1 - RhythmController.Instance.LatencyThreshold);
            _beatEndPos.transform.position = beatEndNewPos;

            var beatStartNewPos = _beatSpawnPos.transform.position;
            beatStartNewPos.x = latencySafeZonePosition.x;
            _beatSpawnPos.transform.position = beatStartNewPos;
        }else{
            _latencySafeZone.fillOrigin = (int) Image.OriginHorizontal.Left;
            _latencySafeZone.fillAmount = RhythmController.Instance.LatencyThreshold * 2;

            var beatEndNewPos = _beatEndPos.transform.position;
            beatEndNewPos.x = latencySafeZonePosition.x +  latencySafeZoneWidth * (RhythmController.Instance.LatencyThreshold);
            _beatEndPos.transform.position = beatEndNewPos;

            var beatStartNewPos = _beatSpawnPos.transform.position;
            beatStartNewPos.x = latencySafeZonePosition.x + latencySafeZoneWidth;
            _beatSpawnPos.transform.position = beatStartNewPos;
        }
    }

    //TODO: update renderedBeats
    void Update()
    {
        UpdateBasePositions();
        var possibleBeats = RhythmController.Instance.GetPossibleBeats(_playerId);


        //check renderedBeats against possibleBeats, remove those that are no longer in possibleBeats (checking BeatNumber)

        //check if there's new beats to be added to renderedBeats

        //update positions based on possibleBeats .Position
        //with Position = 0 being at _beatEndPos, =1 at _beatStartPos, if Position < 0 keep moving forward
        //change positions by interpolating between the two points and then some more to allow for the latency threshold
    }
}
