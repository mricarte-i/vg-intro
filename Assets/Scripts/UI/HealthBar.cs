using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;

    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private Image _healthBar;
    [SerializeField] private Image.OriginHorizontal _onScreenPosition = Image.OriginHorizontal.Left;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateCurrentHealth(float hp){
        _currentHealth = hp;
    }

    public void UpdateMaxHealth(float hp){
        _maxHealth = hp;
    }

    // Update is called once per frame
    void Update()
    {
        _text.alignment = ( _onScreenPosition == Image.OriginHorizontal.Left ?  TMPro.TextAlignmentOptions.TopLeft : TMPro.TextAlignmentOptions.TopRight);
        _healthBar.fillOrigin = (int) (_onScreenPosition == Image.OriginHorizontal.Left ? Image.OriginHorizontal.Right : Image.OriginHorizontal.Left );
        _healthBar.fillAmount = _currentHealth / _maxHealth;
    }
}
