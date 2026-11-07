using UnityEngine;

public class CanvasController : MonoBehaviour
{
    void OnEnable() => UICameraFollower._uiList.Add(transform);
    void OnDisable() => UICameraFollower._uiList.Remove(transform);
}
