using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    [SerializeField]
    private float destroyAfterSeconds = 1.6f;
    // Start is called before the first frame update
    private void Awake()
    {
        Invoke("DestroyObject", destroyAfterSeconds);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }


}
