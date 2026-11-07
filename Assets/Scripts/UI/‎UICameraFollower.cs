using System.Collections.Generic;
using UnityEngine;

public class UICameraFollower : MonoBehaviour
{
    public static readonly List<Transform> _uiList = new();

    void LateUpdate()
    {
        if (Camera.main == null) { return; }

        Quaternion camRot = Camera.main.transform.rotation;

        for (int i = 0; i < _uiList.Count; i++)
        {
            _uiList[i].rotation = camRot;
        }
    }
}
