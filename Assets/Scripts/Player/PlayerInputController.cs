using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    //入力値
    Vector2 _inputVer;
    //入力値を参照
    public Vector2 InputVer => _inputVer;

    //-----Script-----
    private StateManager _state;
    private MoveControlleer _move;
    private AtackController _atack;

    private void Awake()
    {
        _state = GetComponent<StateManager>();
        _move = GetComponent<MoveControlleer>();
        _atack = GetComponent<AtackController>();
    }


    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputVer = context.ReadValue<Vector2>();
        //入力の更新
        //state.UpdateMoveState(inputVer);
        _move.SetMoveInput(_inputVer);
    }


    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <param name="context"></param>
    public void OnAtack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _atack.Attack(AttackState.Charge);
        }
        if (context.canceled)
        {
            _atack.Attack(AttackState.Atatck);
        }
    }

    /// <summary>
    /// 移動,攻撃処理を停止
    /// </summary>
    /// <param name="x"></param>
    public void OnMoveStop(bool x)
    {
        if (_move == null || _atack == null) { return; }
        _move.enabled = x;
        _atack.enabled = x;
    }
}
