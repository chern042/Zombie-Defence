using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestZombies : MonoBehaviour
{

    [SerializeField]
    public GameObject zombiePrefab;

    private int pos;

    void Start()
    {
        InvokeRepeating("MakeZombie",0.1f,0.1f);
        Invoke("StopInvoke", 30f);
    }


    private void MakeZombie()
    {
        Vector3 pos = new Vector3(transform.position.x + (Random.insideUnitSphere.x * 15f), 1.25f, transform.position.z + (Random.insideUnitSphere.z * 15f));
        Instantiate(zombiePrefab, pos, transform.rotation);
    }

    private void StopInvoke()
    {
        CancelInvoke();
    }

    void Update()
    {
        
    }
}
