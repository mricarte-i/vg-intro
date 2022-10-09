using UnityEngine;

public class CursorMaterialHandler : MonoBehaviour
{
    [SerializeField] private GameObject _outline;
    [SerializeField] private Material _blackOutline;
    [SerializeField] private Material _redOutline;
    [SerializeField] private Material _blueOutline;

    public void SetOutlineBlack(){
        _outline.GetComponent<MeshRenderer>().material = _blackOutline;
    }
    public void SetOutlineBlue(){
        _outline.GetComponent<MeshRenderer>().material = _blueOutline;
    }
    public void SetOutlineRed(){
        Debug.Log("lmao");
        _outline.GetComponent<MeshRenderer>().material = _redOutline;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
