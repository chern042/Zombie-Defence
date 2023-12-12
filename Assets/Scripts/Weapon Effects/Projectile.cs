using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem.HID;

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
    public Transform[] woodImpactPrefabs;
    public Transform[] metalImpactPrefabs;
    public Transform[] dirtImpactPrefabs;
    public Transform[] concreteImpactPrefabs;
    public Transform[] sandImpactPrefabs;




    private BarrierController barrier;


     private Collider player;
    private WeaponBehaviour weapon;

    private void Start()
    {
        //Grab the game mode service, we need it to access the player character!
        //var gameModeService = PlayerMotor.Get<CharacterController>();
        var gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        //Debug.Log("TEST: " + gameModeService.GetPlayerCharacter().GetComponent<Collider>());
        // Debug.Log("TEST: " + gameModeService.GetPlayerCharacter());

        //Ignore the main player character's collision. A little hacky, but it should work.
        player = gameModeService.GetPlayerCharacter().GetComponent<Collider>();
        weapon = gameModeService.GetPlayerCharacter().GetComponent<WeaponBehaviour>();
        Physics.IgnoreCollision(player, GetComponent<Collider>());


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



        //If bullet collides with "Metal" tag
        if (collision.transform.tag == "Terrain")
        {
            //Instantiate random impact prefab from array
            Transform impact = Instantiate(dirtImpactPrefabs[Random.Range
                (0, dirtImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Sand")
        {
            //Instantiate random impact prefab from array
            Transform impact = Instantiate(sandImpactPrefabs[Random.Range
                (0, sandImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            Destroy(gameObject);
        }

        //If bullet collides with "Zombie" tag
        if (collision.transform.tag == "Zombie")
        {
            //Instantiate random impact prefab from array
            collision.collider.gameObject.GetComponentInParent<Enemy>().DamageEnemy(player.gameObject.GetComponentInChildren<WeaponBehaviour>().GetDamage());

            Transform impact = Instantiate(bloodImpactPrefabs[Random.Range
                (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Zombie Head")
        {
            //Instantiate random impact prefab from array
            collision.collider.gameObject.GetComponentInParent<Enemy>().DamageEnemy(weapon.GetDamage() * 2f);

            Transform impact = Instantiate(bloodImpactPrefabs[Random.Range
                (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            Destroy(gameObject);
        }


        //If bullet collides with "Concrete" tag
        if (collision.transform.tag == "Concrete")
        {
            //Instantiate random impact prefab from array
            Transform impact = Instantiate(concreteImpactPrefabs[Random.Range
                (0, concreteImpactPrefabs.Length)], collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Wood")
        {
            //Instantiate random impact prefab from array
            Transform impact = Instantiate(woodImpactPrefabs[Random.Range
                (0, woodImpactPrefabs.Length)], collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Metal")
        {
            //Instantiate random impact prefab from array
            Transform impact = Instantiate(metalImpactPrefabs[Random.Range
                (0, metalImpactPrefabs.Length)], collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            impact.SetParent(collision.transform);

            //Destroy bullet object
            Destroy(gameObject);
        }


        if (collision.transform.tag == "Barrier")
        {
            //Instantiate random impact prefab from array
            Transform impact = Instantiate(woodImpactPrefabs[Random.Range
                (0, woodImpactPrefabs.Length)], transform.position,
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
        if (collision.transform.tag == "ExplosiveBarrel")
        {
            //Toggle "explode" on explosive barrel object
            collision.transform.gameObject.GetComponent
                <ExplosiveBarrelScript>().explode = true;
            //Destroy bullet object
            Destroy(gameObject);
        }

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