using UnityEngine;

public class API03GameObject : MonoBehaviour
{
    private void Start()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Debug.Log(go.activeInHierarchy);
        //设置为未激活状态。
        go.SetActive(false);
        Debug.Log(go.activeInHierarchy);
        Debug.Log(go.tag);
    }
}