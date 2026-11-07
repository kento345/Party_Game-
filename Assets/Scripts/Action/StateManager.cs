using System;
using UnityEngine;

/// <summary>
/// 移動ステート
/// </summary>
public enum MoveState
{
    Idel, //停止状態
    Walk, //移動状態
}
/// <summary>
/// 攻撃ステート
/// 攻撃力と統一可
/// </summary>
public enum AttackState
{
    None,    //なし
    Charge,  //チャージ中
    Atatck,  //攻撃中
    Cooldown //クールダウン中
}
/// <summary>
/// 攻撃力
/// </summary>
public enum AtackPower
{
    None,   //なし
    Weak,   //弱攻撃
    Strong, //強攻撃
}
/// <summary>
/// 状態ステート
/// </summary>
public enum State
{
    None,      //なし
    KnockBack, //ノックバック中
    Hit,       //攻撃ヒット中
    Rigid      //硬直中
}

public class StateManager : MonoBehaviour
{

    public MoveState _moveState {  get; private set; } = MoveState.Idel;
    public AttackState _attackState { get; private set; } = AttackState.None;
    public AtackPower _attackPower { get;private set; }   = AtackPower.None;
    public State _state { get; private set; } = State.None;

    /// <summary>
    /// ステート更新
    /// </summary>
    public void UpdateMoveState(Vector2 inputVer)
    {
        _moveState = inputVer.sqrMagnitude > 0.01 ? MoveState.Walk:MoveState.Idel;
    }

    public void SetAttackState(AttackState state)
    {
        _attackState = state;
    }
    public void SetAttackPower(AtackPower power)
    {
        _attackPower = power;
    }
    public void SetState(State State)
    {
        _state = State;
    }
}
