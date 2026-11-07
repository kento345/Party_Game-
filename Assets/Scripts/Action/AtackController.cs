using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AtackController : MonoBehaviour
{
    [Header("攻撃設定")]
    [SerializeField] private GameObject _bullet;//弾Prefab
    private Vector3 _curentAttackPos;            //現在の攻撃位置

    private float _cooldown = 1.0f;          //攻撃クールダウン
    //-----チャージ-------
    private const float _chargeMax = 1.0f;   //Maxチャージ量
    private float _curentCharge = 0f;        //現在のチャージ量
  
    private GameObject _curentBullet;

    /*    //-----硬直---------
        private float StrongRecoveryTime = 1.0f;//硬直時間
        private float curentRecoveryTime;       //現在の硬直時間*/

    [Header("ノックバック,無敵設定")]
    private float _weakPower = 10.0f;    //弱ノックバック力
    private float _strongPower = 20.0f;  //強ノッコバック力
    private float _curentPower = 0.0f;   //現在のコックバック力


    Rigidbody _rb;
    StateManager _stateManager;

    private void Awake()
    {
        //初期化

        //取得
        _rb = GetComponent<Rigidbody>();
        _stateManager = GetComponent<StateManager>();
    }

    /// <summary>
    /// チャージゲージの同期
    /// </summary>
    /// <param name="value"></param>
    public void SetCharge(float value, Vector3 attackPos = default(Vector3))
    {
        _curentCharge = Mathf.Clamp01(value);
        _curentAttackPos = attackPos;
    }

    private void Update()
    {
        //maxなら強攻撃
        if (_curentCharge >= _chargeMax)
        {
            _stateManager.SetAttackPower(AtackPower.Strong);
        }

    }

    /// <summary>
    /// チャージ,攻撃処理
    /// </summary>
    /// <param name="x"></param>
    public void Attack(AttackState state)
    {
        if (!enabled) {return; }
        //チャージ開始(ステートをチャージ中に)
        if (state == AttackState.Charge)
        {
            if (_stateManager.attackState == AttackState.Cooldown || _stateManager.attackState == AttackState.Charge) { return; }
            _stateManager.SetAttackState(state);
        }
        //攻撃開始
        if (state == AttackState.Atatck && _curentBullet == null)
        {
            if (_stateManager.attackState == AttackState.Cooldown) { return; }

            if (_stateManager.attackState == AttackState.Charge)
            {
                _stateManager.SetAttackState(state);
                //attackPowerステートがStrongならstrongPower,それ以外ならweakPower
                _curentPower = _stateManager.attackPower == AtackPower.Strong ? _strongPower : _weakPower;
                if(_stateManager.attackPower == AtackPower.Strong)
                {
                    _curentPower = _strongPower;
                }
                var y = transform.eulerAngles.y;
                _curentBullet = Instantiate(_bullet,new Vector3(transform.position.x,1f,transform.position.z + 1f),Quaternion.Euler(-20,y,0));
                StartCoroutine(CooldownCount());
                var p = _curentBullet.GetComponent<BulletController>();
                if(p != null)
                {
                    p.SetAttackController(this,y);
                    p.Shot(_curentBullet.transform.position, _curentAttackPos);
                    p.SetOwner(gameObject);
                }

            }
        }
    }

    public void BulletDestory()
    {
        _curentBullet = null;
        //StartCoroutine(CooldownCount());
    }

    /// <summary>
    /// クールダウン処理
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownCount()
    {
        _stateManager.SetAttackState(AttackState.Cooldown);
        yield return new WaitForSeconds(_cooldown);
        _stateManager.SetAttackState(AttackState.None);
    }
}


