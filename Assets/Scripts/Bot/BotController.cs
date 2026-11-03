using Mono.Cecil.Cil;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [Header("移動,回転設定")]
    Vector2 inputVer;           //入力方向
    float curentNearDistance;   //現在の近い距離
    
    GameObject nearPlayer;      //近くのPlayer  
    GameObject previousPlayer;  //前回のPlayer

    public Vector2 InputVer => inputVer;  //入力値を参照

    [Header("攻撃設定")]
    private Vector3 moveTarget;
    private bool hasMove = false;

    [Header("判定")]
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private LayerMask playerLayer;
    private float direction = 1.5f; //Rayの距離
    Ray ray; //Ray
    RaycastHit hit; //RayHit情報

    //-----Component-----


    //-----Script-----
    MoveControlleer move;            //移動Script
    private AtackController atack;   //攻撃Script
    private StateManager state;      //ステートScript
    private SensorController sensor; //センサーScript


    void Start()
    {
        move  = GetComponent<MoveControlleer>();
        atack = GetComponent<AtackController>();
        state = GetComponent<StateManager>();
        sensor = GetComponentInChildren<SensorController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         *-----BOTの行動パターン-----
         * 行動攻撃処理に移行できるのか
         * 　|-攻撃中return
         * 一番近くのPlayerを探査,登録
         *   |
         * ターゲット追跡
         *   |-ターゲットとの距離が指定以上中は追跡
         * チャージに移行
         *   |-距離が指定以上はチャージ
         * 移動停止,ターゲット方向に回転
         *   |-センサーに触れるまで回転
         * センサーがターゲットに触れたら攻撃
         *   |-
         *   
         */
        if (state.attackState == AttackState.Atatck) { return; }
        //近くのPlayerオブジェクトがない場合探査
        if(nearPlayer == null)
        {
            NearPlayer();
            return;
        }
        Chase(nearPlayer.transform.position);



       /* if (wasAttack)
        {
            if (!hasMove)
            {
                moveTarget = new Vector3(Random.Range(-8, 8), 0.5f, Random.Range(-8, 8));
                hasMove = true;
            }
            Chase(moveTarget);
            var dist = Vector3.Distance(transform.position, moveTarget);
            if (dist < 2f)
            {
                hasMove = false;
                nearPlayer = null;
                return;
            }
        }
        if (CheckObj() && !isRota)
        {
            Debug.Log("近いPlayer");
            isRota = true;
            //0->-1,1->1になる
            rota = Random.Range(0, 2) == 0 ? -1 : 1;
            angle = 0f;
        }
        if (isRota)
        {
            OnMove(new Vector2(rota, 0));
            angle += move.GetRotaSpeed * Time.deltaTime;

            if(angle >= 90)
            {
                isRota = false;
                rota = 0f;
                OnMove(Vector2.zero);
            }
            return;
        }
        //敵を探す
        if (nearPlayer == null)
        {
            NearPlayer();
            return;
        }
        //ターゲットがいる
        else
        {
            Chase(nearPlayer.transform.position);

            var dist = Vector3.Distance(transform.position, nearPlayer.transform.position);
            if (dist < 7f)
            {
                atack.Attack(AttackState.Charge);
                var dir = nearPlayer.transform.position - transform.position;
                dir.y = 0;
                var localDir = transform.InverseTransformDirection(dir.normalized);
                if (sensor.IsHit)
                {
                    atack.Attack(AttackState.Atatck);
                    previousPlayer = nearPlayer;
                    nearPlayer = null;
                    //wasAttck = true;
                    return;
                }
                if (dist < 4f)
                {
                    // 相手の方向を向く
                    OnMove(new Vector2(localDir.x, 0));

                    // 相手の方向誤差0.1以内なら攻撃
                    if (Mathf.Abs(localDir.x) < 0.1f)
                    {
                        atack.Attack(AttackState.Atatck);
                        
                        previousPlayer = nearPlayer;
                        nearPlayer = null;
                        //wasAttack = true;
                    }
                }
            }
        }*/

       
    }

    /// <summary>
    /// Rayを作りObjの判定
    /// </summary>
    /// <returns></returns>
    bool CheckObj()
    {
        var origin = transform.position + new Vector3(0, 0.5f, 0);
        var dir = transform.forward;
        ray = new Ray(origin, dir);
        Debug.DrawRay(origin, dir * direction, Color.red);

        return Physics.Raycast(ray, out hit, direction, objectLayer);
    }

    /// <summary>
    /// 最も近いプレイヤーを検索し、nearPlayer と curentNearDistance を更新する。
    /// </summary>
    /// <remarks>自身および previousPlayer を除外し、GameManager.Instance.playerList の各プレイヤーとの距離を Vector3.Distance
    /// で比較する。最短距離が見つかれば nearPlayer に割り当て、curentNearDistance を更新する。処理開始時に curentNearDistance は Mathf.Infinity
    /// で初期化される。</remarks>
    void NearPlayer()
    {
        //初期化
        curentNearDistance = Mathf.Infinity;
        nearPlayer = null;
        var myPos = transform.position;

        //PlayerListの中で一番近いPlayerを検索,取得
        //foreach (var p in JoinDataHolder.instance.GetPlayerData.Keys)
        foreach (var p in Manager.Instance.TargetList)
        {
            //除外処理(自身,null,前回のターゲット,死亡者)
            if (p == gameObject || p == null || p == previousPlayer) continue;
            var life = p.GetComponent<HitController>();
            if(life != null)
            {
                // 死亡しているなら除外
                if (!life.IsAlive) continue;
            }

            //2点間の距離計算
            var dist = p.transform.position - myPos;
            var sqrDist = dist.sqrMagnitude;

            //curentNearDistanceより小さい場合登録
            if (sqrDist < curentNearDistance)
            {
                curentNearDistance = sqrDist;
                nearPlayer = p;
            }
        }
    }

    /// <summary>
    /// 追跡処理
    /// </summary>
    void Chase(Vector3 targetPosition)
    {
        var dir = targetPosition - transform.position;
        dir.y = 0f;

        //相手までの角度計算
        float angle = Vector3.SignedAngle(transform.forward,
                                          dir.normalized, 
                                          Vector3.up );

        if(Mathf.Abs(angle) > 5f){
            OnMove(new Vector2(Mathf.Sign(angle), 0f));
        }
        else{
            OnMove(new Vector2(0, 1));
        }
    }

    /// <summary>
    /// 入力値などの変更
    /// </summary>
    /// <param name="context"></param>
    void OnMove(Vector2 context)
    {
        move.SetMoveInput(context);
        inputVer = context;
    }

    /// <summary>
    /// 移動,攻撃処理を停止
    /// </summary>
    /// <param name="x"></param>
    public void OnMoveStop(bool x)
    {
        if (move == null || atack == null) { return; }
        move.enabled = x;
        atack.enabled = x;
    }
}
