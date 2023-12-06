using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    [SerializeField]
    public float destroyAfterSeconds = 1.6f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", destroyAfterSeconds);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }


}
