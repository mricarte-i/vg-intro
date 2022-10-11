using System.Collections.Generic;
using UnityEngine;

public class WSStageSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject _stageBoxPrefab;
    [SerializeField] private List<Transform> _stageBoxPositions = new List<Transform>();
    [SerializeField] private List<StageData> _stages = new List<StageData>();
    [Space]
    [SerializeField] private SelectMenuConductor _menu;
    // Start is called before the first frame update
    void Start()
    {
        if(_stageBoxPositions.Count != _stages.Count){
            throw new System.Exception("stage select menu needs same number of stages & positions");
        }
        InitializeStageGrid();
    }

    private void InitializeStageGrid(){
        for(int i = 0; i < _stages.Count; i++){
            SpawnStageBox(_stages[i], _stageBoxPositions[i]);
        }
    }

    private void SpawnStageBox(StageData stage, Transform t){
        GameObject box = Instantiate(_stageBoxPrefab);
        box.transform.position = t.position;

        StageBox sbox = box.GetComponent<StageBox>();
        sbox.SetData(stage);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
