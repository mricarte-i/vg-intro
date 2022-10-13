using UnityEngine;

public class FightUI : MonoBehaviour
{
    [SerializeField] private HealthBar _HPBarP1;
    [SerializeField] private HealthBar _HPBarP2;

    public HealthBar GetPlayer1HPBar() => _HPBarP1;
    public HealthBar GetPlayer2HPBar() => _HPBarP2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
