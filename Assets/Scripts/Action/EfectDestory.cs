using UnityEngine;

public class EfectDestory : MonoBehaviour
{
    float _time = 0f;

    float _t = 2;

    void Start()
    {
        _time = _t;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_time >= 0)
        {
            _time -= Time.deltaTime;
            if(_time < 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
