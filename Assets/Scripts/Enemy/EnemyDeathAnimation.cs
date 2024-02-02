using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathAnimation : MonoBehaviour
{

    public Transform[] bodyPartPrefabs;
    private Transform[] instantiatedParts;

    //private Transform zombie;


    public float explosionRange = 2f;
    public float destroyAfterTime = 5f;

    void Start()
    {
        //zombie = GetComponent<Transform>();
        instantiatedParts = new Transform[bodyPartPrefabs.Length];
    }

    public void KillZombie()
    {
        Vector3 zombiePosition = transform.position;//zombie.position;

        GetComponent<NavMeshAgent>().enabled = false;
        transform.position = transform.position + (Vector3.down * 10000f);
        for(int i=0; i < bodyPartPrefabs.Length;i++)
        {
            Transform splatter = Instantiate(bodyPartPrefabs[i], zombiePosition+(Random.insideUnitSphere * 0.1f)+(Vector3.up*0.5f), Random.rotation);

            splatter.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-explosionRange,explosionRange), Random.Range(1f, explosionRange), Random.Range(-explosionRange, explosionRange));
            instantiatedParts.SetValue(splatter, i);
        }
        Invoke("DestroyObjectsAfterTime", destroyAfterTime);

    }

    private void DestroyObjectsAfterTime()
    {
        foreach (Transform splatter in instantiatedParts)
        {
            Destroy(splatter.gameObject);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
