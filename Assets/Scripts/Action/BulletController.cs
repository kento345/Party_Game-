using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _exprosionPrefab;
    private float _height = 2f;
    private float _duration = 1f;
    private float _angle = 0;
    public GameObject _Owner { get; private set; }

    [SerializeField] private LayerMask _hitLayer;

    Rigidbody _rb;
    AtackController _attack;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y < _height)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30, _angle, 0), Time.deltaTime * 10f);
        }
    }

    public void Shot(Vector3 startPos, Vector3 endPos)
    {
        transform.position = startPos;

        transform.DOJump(endPos, _height, 1, _duration).SetEase(Ease.Linear);
    }

    /// <summary>
    /// AttackController取得,方向取得
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="a"></param>
    public void SetAttackController(AtackController controller ,float a)
    {
        _attack = controller;
        _angle = a;
    }

    /// <summary>
    /// 生成者を認識
    /// </summary>
    /// <param name="owner"></param>
    public void SetOwner(GameObject owner)
    {
        _Owner = owner;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _hitLayer) != 0)
        {
            _attack.BulletDestory();
            var obj = Instantiate(_exprosionPrefab, transform.position, Quaternion.identity);
            obj.Play();
            Destroy(this.gameObject);
        }
    }
}
