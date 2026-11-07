using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    private bool _isHit = false;
    public bool IsHit => _isHit;

    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & _playerLayer) != 0){
            _isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer) != 0)
        {
            _isHit = false;
        }
    }
}
