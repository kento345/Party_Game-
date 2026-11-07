using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class HitController : MonoBehaviour
{
    [SerializeField] private LayerMask _bulletLayer = default;
    private bool _isAlive = false;
    /// <summary>
    /// 生存処理(生:true,死:false)
    /// </summary>
    public bool IsAlive => _isAlive;


    private void Awake()
    {
        _isAlive = true;
    }

    /// <summary>
    /// 攻撃を受けたら子供オブジェクト消去
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _bulletLayer) != 0)
        {
            var b = other.gameObject.GetComponent<BulletController>();
            // BulletControllerがないなら無視
            if (b == null)
                return;
            // 自分が撃ったBulletなら無視
            if (b._Owner == gameObject)
                return;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(false);
            }
            _isAlive = false;
        }
    }
}
