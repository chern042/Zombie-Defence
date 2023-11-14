using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class PlayerShoot : MonoBehaviour
{ 

    //public Camera cam;

    //[SerializeField]
    //private GameObject muzzlePrefab;


    //[SerializeField]
    //private GameObject bulletHolePrefab;


    //[Tooltip("Number of particles to emit when firing.")]
    //[SerializeField]
    //private int flashParticlesCount = 5;



    [Header("Firing")]

    [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
    [SerializeField]
    private bool automatic;

    [Tooltip("How fast the projectiles are.")]
    [SerializeField]
    private float projectileImpulse = 400.0f;

    [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
    [SerializeField]
    private int roundsPerMinutes = 200;

    [Tooltip("Mask of things recognized when firing.")]
    [SerializeField]
    private LayerMask mask;

    [Tooltip("Maximum distance at which this weapon can fire accurately. Shots beyond this distance will not use linetracing for accuracy.")]
    [SerializeField]
    private float maximumDistance = 500.0f;


    [Header("Animation")]

    [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
    [SerializeField]
    private Transform socketEjection;

    [Header("Resources")]

    [Tooltip("Casing Prefab.")]
    [SerializeField]
    private GameObject prefabCasing;

    [Tooltip("Projectile Prefab. This is the prefab spawned when the weapon shoots.")]
    [SerializeField]
    private GameObject prefabProjectile;


    private Animator animator;
    private int ammunitionCurrent;
    private Transform playerCamera;
    private Muzzle muzzle;




    public void Shoot()
    {
        InvokeRepeating("Shooting", 0, 0.1f);

    }

    public void CancelShoot()
    {
        CancelInvoke("Shooting");
    }

    //public void Shooting()
    //{ 
    //    RaycastHit hit;

    //    ParticleSystem particles = muzzlePrefab.GetComponent<ParticleSystem>();


    //    //Try to play the fire particles from the muzzle!
    //    if (particles != null)
    //    {
    //        particles.Emit(flashParticlesCount);
    //    }

    //    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100))
    //    {
    //        GameObject impact = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
    //        //Destroy(impact, 2f);
    //        EjectCasing();
    //    }
    //    Invoke("StopMuzzleFlash", 0.1f);
    //}

    //private void StopMuzzleFlash()
    //{
    //    muzzlePrefab.GetComponent<ParticleSystem>().Stop();

    //}

    public void EjectCasing()
    {
        //Spawn casing prefab at spawn point.
        if (prefabCasing != null && socketEjection != null)
        {
            GameObject casing = Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
            Destroy(casing, 2f);

        }
    }








    //public override void Reload()
    //{
    //    //Play Reload Animation.
    //    animator.Play(HasAmmunition() ? "Reload" : "Reload Empty", 0, 0.0f);
    //}

    

}
