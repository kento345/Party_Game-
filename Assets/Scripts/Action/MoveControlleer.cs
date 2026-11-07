using UnityEngine;

public class MoveControlleer : MonoBehaviour
{
    [Header("移動,回転設定")]
    [SerializeField] private float _speed = 5f;//移動速度
    private float _speed2 = 0f;//チャージ中の移動
    [SerializeField] private float _moveRate = 0.3f; //移動速度低下率
    float _curentSpeed = 0f; //現在の速度

    [SerializeField] private float _rotaSpeed = 10.0f;//回転速度
    private float _rotaSpeed2 = 0f;//チャージ中の回転速度
    [SerializeField] private float _rotaRate = 0.7f;//回転速度低下率
    private float _curentRotaSpeed = 0f;//現在の回転速度
    public float GetRotaSpeed => _curentRotaSpeed;

    Vector2 _inputVer;  //移動入力
    Rigidbody _rb;

    private StateManager _stateManager;

    private void Awake()
    {
        //初期化
        _speed2 = _speed * _moveRate;
        _rotaSpeed2 = _rotaSpeed * _rotaRate;

        //取得
        _stateManager = GetComponent<StateManager>();
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 入力受け取り
    /// </summary>
    /// <param name="input"></param>
    public void SetMoveInput(Vector2 input)
    {
        _inputVer = input;
    }

    private void FixedUpdate()
    {
        _curentSpeed = _speed;
        _curentRotaSpeed = _rotaSpeed;

        if (_stateManager.attackState == AttackState.Charge)
        {
            _curentSpeed = _speed2;
            _curentRotaSpeed = _rotaSpeed2;
        }

        //攻撃以外の時の処理
        if (_stateManager.attackState != AttackState.Atatck)
        {
            //移動処理
            Vector3 move = transform.forward * _inputVer.y * _curentSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + move);
            //回転処理
            var rota = _inputVer.x * _curentRotaSpeed * Time.fixedDeltaTime;
            var Rot = Quaternion.Euler(0,rota,0);
            _rb.MoveRotation(_rb.rotation * Rot);
            _stateManager.UpdateMoveState(_inputVer);
        }
    }
}
