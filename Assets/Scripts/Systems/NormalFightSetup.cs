using Controllers;
using UnityEngine;

public class NormalFightSetup : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _p1StartPos, _p2StartPos;
    [Space]
    [SerializeField] private GameObject _stagePrefab; //get these from app manager or level manager!
    [SerializeField] private Transform _stagePos;
    [Space]
    [SerializeField] private PlayerGuider _playerGuider;
    [SerializeField] private Cinemachine.CinemachineTargetGroup _targetGroup;
    [Space]
    [SerializeField] private GameObject _fightUIPrefab;
    [SerializeField] private GameObject _damageSystem;

    private GameObject _p1, _p2, _fightUI;

    void Awake() {
        Instantiate(_stagePrefab, _stagePos);

        _p1 = InitializePlayer(AppManager.Instance.GetInputUser(0));
        _p2 = InitializePlayer(AppManager.Instance.GetInputUser(1));

        _p1.transform.position = _p1StartPos.position;
        _p2.transform.position = _p2StartPos.position;

        _playerGuider.SetPlayers(_p1.GetComponent<PlayerInputHandler>(), _p2.GetComponent<PlayerInputHandler>());

        _targetGroup.AddMember(_p1.transform, 0.4f, 1.7f);
        _targetGroup.AddMember(_p2.transform, 0.5f, 1.7f);

        var alignScript = _targetGroup.GetComponent<Align3DCam>();
        alignScript.SetPlayers(_p1.GetComponent<PlayerInputHandler>(), _p2.GetComponent<PlayerInputHandler>());

        _fightUI = Instantiate(_fightUIPrefab);
        HealthBar hbP1 = _fightUI.GetComponent<FightUI>().GetPlayer1HPBar();
        HealthBar hbP2 = _fightUI.GetComponent<FightUI>().GetPlayer2HPBar();
        _p1.GetComponentInChildren<LifeController>().SetHPBar(hbP1);
        _p2.GetComponentInChildren<LifeController>().SetHPBar(hbP2);

    }

    public void ResetFight(){
        _p1.transform.position = _p1StartPos.position;
        _p2.transform.position = _p2StartPos.position;

        _p1.GetComponentInChildren<LifeController>().Reset();
        _p2.GetComponentInChildren<LifeController>().Reset();

        _fightUI.GetComponent<FightUI>().Reset();
    }

    private GameObject InitializePlayer(PlayableCharacter player){
        if(player == null){
            throw new System.Exception("how dare you");
        }

        GameObject go = Instantiate(_playerPrefab);

        PlayerInputHandler pc = go.GetComponent<PlayerInputHandler>();
        pc.SetInputUser(player.inputUser);
        pc.BindControls(player.controls);

        var model = Instantiate(player.characterData.Model, go.transform); //instance model with player gameObject as parent!
        //do the same when we eventually add the collider thingy!
        var damageSystem = Instantiate(_damageSystem, go.transform);
        damageSystem.GetComponent<DamageSystemHandler>().GetHurtbox.SetPlayerId(player.playerId);
        pc.SetDamageSystemHandler(damageSystem.GetComponent<DamageSystemHandler>());

        return go;
    }
}
