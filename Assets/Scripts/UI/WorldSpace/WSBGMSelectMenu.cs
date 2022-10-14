using System.Collections.Generic;
using UnityEngine;

public class WSBGMSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject _bgmBoxPrefab;
    [SerializeField] private List<Transform> _bgmBoxPositions = new List<Transform>();
    [SerializeField] private List<BgmData> _bgms = new List<BgmData>();
    [Space]
    [SerializeField] private SelectMenuConductor _menu;
    // Start is called before the first frame update
    void Start()
    {
        if(_bgmBoxPositions.Count != _bgms.Count){
            throw new System.Exception("stage select menu needs same number of stages & positions");
        }
        InitializeStageGrid();
    }

    private void InitializeStageGrid(){
        for(int i = 0; i < _bgms.Count; i++){
            SpawnBGMBox(_bgms[i], _bgmBoxPositions[i]);
        }
    }

    private void SpawnBGMBox(BgmData bgm, Transform t){
        GameObject box = Instantiate(_bgmBoxPrefab);
        box.transform.position = t.position;

        BgmBox sbox = box.GetComponent<BgmBox>();
        sbox.SetData(bgm);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
