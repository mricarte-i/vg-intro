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

    void Awake() {
        Instantiate(_stagePrefab, _stagePos);

        var player1 = InitializePlayer(AppManager.Instance.GetInputUser(0));
        var player2 = InitializePlayer(AppManager.Instance.GetInputUser(1));

        player1.transform.position = _p1StartPos.position;
        player2.transform.position = _p2StartPos.position;

        _playerGuider.SetPlayers(player1, player2);

        _targetGroup.AddMember(player1.transform, 0.4f, 1.7f);
        _targetGroup.AddMember(player2.transform, 0.5f, 1.7f);

        var alignScript = _targetGroup.GetComponent<Align3DCam>();
        alignScript.SetPlayers(player1, player2);

        GameObject fightUI = Instantiate(_fightUIPrefab);
        HealthBar hbP1 = fightUI.GetComponent<FightUI>().GetPlayer1HPBar();
        HealthBar hbP2 = fightUI.GetComponent<FightUI>().GetPlayer2HPBar();
        player1.GetComponentInChildren<LifeController>().SetHPBar(hbP1);
        player2.GetComponentInChildren<LifeController>().SetHPBar(hbP2);

    }

    private PlayerInputHandler InitializePlayer(PlayableCharacter player){
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
        pc.SetDamageSystemHandler(damageSystem.GetComponent<DamageSystemHandler>());

        return pc;
    }
}
