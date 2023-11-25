﻿using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{

    [Range(5, 100)]
    [Tooltip("After how long time should the bullet prefab be destroyed?")]
    public float destroyAfter;
    [Tooltip("If enabled the bullet destroys on impact")]
    public bool destroyOnImpact = false;
    [Tooltip("Minimum time after impact that the bullet is destroyed")]
    public float minDestroyTime;
    [Tooltip("Maximum time after impact that the bullet is destroyed")]
    public float maxDestroyTime;

    [Header("Impact Effect Prefabs")]
    public Transform[] bloodImpactPrefabs;
    public Transform[] metalImpactPrefabs;
    public Transform[] dirtImpactPrefabs;
    public Transform[] concreteImpactPrefabs;


    public GameObject[] invisibleWalls;

    private BarrierController barrier;


    //[SerializeField] private Collider player;

    private void Start()
    {
        //Grab the game mode service, we need it to access the player character!
        //var gameModeService = PlayerMotor.Get<CharacterController>();
        var gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        //Debug.Log("TEST: " + gameModeService.GetPlayerCharacter().GetComponent<Collider>());
        // Debug.Log("TEST: " + gameModeService.GetPlayerCharacter());

        //Ignore the main player character's collision. A little hacky, but it should work.
        Physics.IgnoreCollision(gameModeService.GetPlayerCharacter().GetComponent<Collider>(), GetComponent<Collider>());

        foreach(GameObject invisibleWall in invisibleWalls)
        {
            Physics.IgnoreCollision(invisibleWall.GetComponent<Collider>(), GetComponent<Collider>());

        }


        //Start destroy timer
        StartCoroutine(DestroyAfter());
    }

    //If the bullet collides with anything
    private void OnCollisionEnter(Collision collision)
    {
        //Ignore collisions with other projectiles.
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            return;

        }
        //var gameModeService = ServiceLocator.Current.Get<IGameModeService>();

        //Physics.IgnoreCollision(bulletHolePrefab.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        //Physics.IgnoreCollision(gameModeService.GetPlayerCharacter().GetComponent<Collider>(), GetComponent<Collider>());
        if (collision.collider.CompareTag("PlayerLimit"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            return;
        }


        //If destroy on impact is false, start 
        //coroutine with random destroy timer
        if (!destroyOnImpact)
        {
            StartCoroutine(DestroyTimer());
        }
        //Otherwise, destroy bullet on impact
        else
        {
            Destroy(gameObject);
        }

        //If bullet collides with "Blood" tag
        if (collision.transform.tag == "Blood")
        {
            //Instantiate random impact prefab from array
            Instantiate(bloodImpactPrefabs[Random.Range
                (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            //Destroy bullet object
            Destroy(gameObject);
        }

        //If bullet collides with "Metal" tag
        if (collision.transform.tag == "Metal")
        {
            //Instantiate random impact prefab from array
            Instantiate(metalImpactPrefabs[Random.Range
                (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            //Destroy bullet object
            Destroy(gameObject);
        }

        //If bullet collides with "Zombie" tag
        if (collision.transform.tag == "Zombie")
        {
            //Instantiate random impact prefab from array
            Instantiate(bloodImpactPrefabs[Random.Range
                (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            //Destroy bullet object
            Destroy(gameObject);
        }

        //If bullet collides with "Concrete" tag
        if (collision.transform.tag == "Concrete")
        {
            Debug.Log("DDDERRR");
            //Instantiate random impact prefab from array
            Transform impact = Instantiate(concreteImpactPrefabs[Random.Range
                (0, concreteImpactPrefabs.Length)], collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Barrier")
        {
            Debug.Log("DDDERRR");
            //Instantiate random impact prefab from array
            Transform impact = Instantiate(concreteImpactPrefabs[Random.Range
                (0, concreteImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            barrier = collision.gameObject.GetComponentInParent<BarrierController>();

            if (barrier != null)
            {
                barrier.TakeDamage(1f);
            }
            else
            {
                Debug.Log("Barrier is null.");
            }
            Destroy(gameObject);

        }

        ////If bullet collides with "Target" tag
        //if (collision.transform.tag == "Target")
        //{
        //    //Toggle "isHit" on target object
        //    collision.transform.gameObject.GetComponent
        //        <TargetScript>().isHit = true;
        //    //Destroy bullet object
        //    Destroy(gameObject);
        //}

        ////If bullet collides with "ExplosiveBarrel" tag
        //if (collision.transform.tag == "ExplosiveBarrel")
        //{
        //    //Toggle "explode" on explosive barrel object
        //    collision.transform.gameObject.GetComponent
        //        <ExplosiveBarrelScript>().explode = true;
        //    //Destroy bullet object
        //    Destroy(gameObject);
        //}

        ////If bullet collides with "GasTank" tag
        //if (collision.transform.tag == "GasTank")
        //{
        //    //Toggle "isHit" on gas tank object
        //    collision.transform.gameObject.GetComponent
        //        <GasTankScript>().isHit = true;
        //    //Destroy bullet object
        //    Destroy(gameObject);
        //}
    }

    private IEnumerator DestroyTimer()
    {
        //Wait random time based on min and max values
        yield return new WaitForSeconds
            (Random.Range(minDestroyTime, maxDestroyTime));
        //Destroy bullet object
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfter()
    {
        //Wait for set amount of time
        yield return new WaitForSeconds(destroyAfter);
        //Destroy bullet object
        Destroy(gameObject);
    }
}