using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBeatBar : MonoBehaviour
{
    [SerializeField] private PlayerId _playerId;
    [SerializeField] private GameObject _beatSpawnPos, _beatEndPos;
    [SerializeField] private Image _latencySafeZone;
    [SerializeField] private GameObject _renderedBeatPrefab; //has RenderedBeat component!
    [SerializeField] private Image _startMarker;
    [SerializeField] private Image _endMarker;

    private Dictionary<int, GameObject> _renderedBeats = new Dictionary<int, GameObject>();

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

            var beatStartNewPos = _beatSpawnPos.transform.position;
            beatStartNewPos.x = latencySafeZonePosition.x - latencySafeZoneWidth * RhythmController.Instance.LatencyThreshold;
            _beatSpawnPos.transform.position = beatStartNewPos;

            var beatEndNewPos = _beatEndPos.transform.position;
            beatEndNewPos.x = latencySafeZonePosition.x + latencySafeZoneWidth * RhythmController.Instance.LatencyThreshold;
            _beatEndPos.transform.position = beatEndNewPos;

            _startMarker.transform.position = beatStartNewPos;
            _endMarker.transform.position = beatEndNewPos;
        }else{
            _latencySafeZone.fillOrigin = (int) Image.OriginHorizontal.Left;
            _latencySafeZone.fillAmount = RhythmController.Instance.LatencyThreshold * 2;

            var beatStartNewPos = _beatSpawnPos.transform.position;
            beatStartNewPos.x = latencySafeZonePosition.x + latencySafeZoneWidth * RhythmController.Instance.LatencyThreshold;
            _beatSpawnPos.transform.position = beatStartNewPos;

            var beatEndNewPos = _beatEndPos.transform.position;
            beatEndNewPos.x = latencySafeZonePosition.x - + latencySafeZoneWidth * RhythmController.Instance.LatencyThreshold;
            _beatEndPos.transform.position = beatEndNewPos;

            _startMarker.transform.transform.Rotate(0, 180, 0);
            _startMarker.transform.position = beatStartNewPos;
            _endMarker.transform.transform.Rotate(0, 180, 0);
            _endMarker.transform.position = beatEndNewPos;
        }
    }

    //TODO: update renderedBeats
    void Update()
    {
        var possibleBeats = RhythmController.Instance.GetPossibleBeats(_playerId);
        Debug.Log($"there are {possibleBeats.Count} possible beats");

        //check renderedBeats against possibleBeats, remove those that are no longer in possibleBeats (checking BeatNumber)
        List<int> toRemoveBeats = new List<int>(_renderedBeats.Keys);
        foreach (BeatInfo beatInfo in possibleBeats) {
            toRemoveBeats.Remove(beatInfo.BeatNumber);

            if (!_renderedBeats.ContainsKey(beatInfo.BeatNumber)) {
                // instantiate prefab & add to dict
                var newBeat = Instantiate(
                    _renderedBeatPrefab,
                    _startMarker.transform.position,
                    _startMarker.transform.rotation
                );
                newBeat.transform.SetParent(this.transform);
                newBeat.name = $"Beat-{beatInfo.BeatNumber}-{_playerId}";
                Debug.Log($"Beat{beatInfo.BeatNumber} at {newBeat.transform.position}");

                _renderedBeats.Add(beatInfo.BeatNumber, newBeat);
            }

            GameObject currentBeat = _renderedBeats[beatInfo.BeatNumber];
            var markerDistance = _endMarker.transform.position - _startMarker.transform.position;
            currentBeat.transform.position = _startMarker.transform.position + beatInfo.Progress() * markerDistance;
        }

        // remove unused
        foreach (int unusedIndex in toRemoveBeats) {
            Destroy(_renderedBeats[unusedIndex]);
            _renderedBeats.Remove(unusedIndex);
        }

        //check if there's new beats to be added to renderedBeats

        //update positions based on possibleBeats .Position
        //with Position = 0 being at _beatEndPos, =1 at _beatStartPos, if Position < 0 keep moving forward
        //change positions by interpolating between the two points and then some more to allow for the latency threshold
    }
}
