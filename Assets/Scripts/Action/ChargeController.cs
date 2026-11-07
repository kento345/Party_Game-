using UnityEngine;
using UnityEngine.UI;

public class ChargeController : MonoBehaviour
{
    [SerializeField] private float _maxChargeTime = 1.5f;
    [SerializeField] private float _maxPos        = 500f;
    [SerializeField] private float _maxSize       = 300f;
    [SerializeField] private Image _pointImage;
    [SerializeField] private Image _arrowImage;
    //position
    RectTransform _pos;
    Vector2 _initPos = new Vector2(0, 250);
    //height
    RectTransform _imageSize;
    Vector2 _initSize = new Vector2(100, 100);


    private AtackController _ac;
    private StateManager _stateManager;

    private void Awake()
    {
        _ac = GetComponent<AtackController>();
        _stateManager = GetComponent<StateManager>();

        _pos = _pointImage.rectTransform;
        _imageSize = _arrowImage.rectTransform;
        _pos.anchoredPosition = _initPos;
        _imageSize.sizeDelta = _initSize;
    }

    private void Update()
    {
        if (_stateManager.attackState == AttackState.Charge)
        {
            _pointImage.enabled = true;
            float speed = (_maxPos - _initPos.y) / _maxChargeTime;
            float speedSize = (_maxSize - _initSize.y) / _maxChargeTime;
            float height = _pos.anchoredPosition.y;
            float heightSize = _imageSize.sizeDelta.y;
            height += speed * Time.deltaTime;
            heightSize += speedSize * Time.deltaTime;

            height = Mathf.Clamp(height, _initPos.y, _maxPos);
            heightSize = Mathf.Clamp(heightSize, _initSize.y, _maxSize);

            _pos.anchoredPosition = new Vector2(_initPos.x,height);
            _imageSize.sizeDelta = new Vector2(_imageSize.sizeDelta.x, heightSize);
            float chargeRate = (height - _initPos.y) / (_maxPos - _initPos.y);
            _ac.SetCharge(chargeRate, _pointImage.transform.position);
        }
        else
        {
            _pos.anchoredPosition = _initPos;
            _imageSize.sizeDelta = _initSize;
            _ac.SetCharge(0);
            _pointImage.enabled = false;
        } 
    }
}
