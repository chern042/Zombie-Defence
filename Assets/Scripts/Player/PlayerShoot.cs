using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShoot : MonoBehaviour
{ 

    public Camera cam;

    [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
    [SerializeField]
    private Transform socketEjection;

    [Header("Resources")]

    [Tooltip("Casing Prefab.")]
    [SerializeField]
    private GameObject prefabCasing;


    [SerializeField]
    private GameObject muzzlePrefab;


    [SerializeField]
    private GameObject bulletHolePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        InvokeRepeating("Shooting", 0, 0.1f);
    }

    public void CancelShoot()
    {
        CancelInvoke("Shooting");
    }

    public void Shooting()
    { 
        RaycastHit hit;

        muzzlePrefab.GetComponent<ParticleSystem>().Play();

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100))
        {
            GameObject impact = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
            EjectCasing();
        }
        Invoke("StopMuzzleFlash", 0.1f);
    }

    private void StopMuzzleFlash()
    {
        muzzlePrefab.GetComponent<ParticleSystem>().Stop();

    }

    public void EjectCasing()
    {
        //Spawn casing prefab at spawn point.
        if (prefabCasing != null && socketEjection != null)
        {
            GameObject casing = Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
            Destroy(casing, 2f);

        }
    }
}
