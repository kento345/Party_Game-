using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private ParticleSystem exprosionPrefab;
    private float height = 2f;
    private float duration = 1f;
    private float angle = 0;
    public GameObject Owner { get; private set; }

    [SerializeField] private LayerMask hitLayer;

    Rigidbody rb;
    AtackController attack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y < height)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30, angle, 0), Time.deltaTime * 10f);
        }
    }

    public void Shot(Vector3 startPos, Vector3 endPos)
    {
        transform.position = startPos;

        transform.DOJump(endPos, height, 1, duration).SetEase(Ease.Linear);
    }

    /// <summary>
    /// AttackController取得,方向取得
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="a"></param>
    public void SetAttackController(AtackController controller ,float a)
    {
        attack = controller;
        angle = a;
    }

    /// <summary>
    /// 生成者を認識
    /// </summary>
    /// <param name="owner"></param>
    public void SetOwner(GameObject owner)
    {
        Owner = owner;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayer) != 0)
        {
            attack.BulletDestory();
            var obj = Instantiate(exprosionPrefab, transform.position, Quaternion.identity);
            obj.Play();
            Destroy(this.gameObject);
        }
    }
}
