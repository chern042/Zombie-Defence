// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// Weapon. This class handles most of the things that weapons need.
/// </summary>
public class Weapon : GunBehaviour
{

    [Tooltip("Weapon Damage.")]
    [SerializeField]
    private float damage = 1f;



    [Header("Firing")]

    [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
    [SerializeField]
    private bool automatic;

    [Tooltip("How fast the projectiles are.")]
    [SerializeField]
    private float projectileImpulse = 400.0f;

    [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
    [SerializeField]
    private float roundsPerMinute = 200f;

    [Tooltip("Mask of things recognized when firing.")]
    [SerializeField]
    private LayerMask mask;

    [Tooltip("Maximum distance at which this weapon can fire accurately. Shots beyond this distance will not use linetracing for accuracy.")]
    [SerializeField]
    private float maximumDistance = 500.0f;

    [Tooltip("Total(Maximum) Ammunition.")]
    [SerializeField]
    private int ammunitionTotal = 300;

    [Tooltip("Ammunition in clip.")]
    [SerializeField]
    private int ammunitionClip = 30;


    [Tooltip("Ammunition type (AR, Shotgun, Handgun, SMG, Any).")]
    [SerializeField]
    private string ammunitionType = "Any";

    [Tooltip("Fire Spread.")]
    [SerializeField]
    private Vector3 spread = new Vector3(0.1f, 0.1f, 0.1f);

    [Tooltip("Fire Spread Max Time.")]
    [SerializeField]
    private float spreadTime = 5f;




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

    //[Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")]
    //[SerializeField]
    //public RuntimeAnimatorController controller;

    //[Tooltip("Weapon Body Texture.")]
    //[SerializeField]
    //private Sprite spriteBody;


    [Header("Audio Clips.")]

    [Tooltip("Holster Audio Clip.")]
    [SerializeField]
    private AudioClip audioClipHolster;

    [Tooltip("Unholster Audio Clip.")]
    [SerializeField]
    private AudioClip audioClipUnholster;

    [Header("Audio Clips Reloads")]

    [Tooltip("Reload Audio Clip.")]
    [SerializeField]
    private AudioClip audioClipReload;

    [Tooltip("Reload Empty Audio Clip.")]
    [SerializeField]
    private AudioClip audioClipReloadEmpty;

    [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
    [SerializeField]
    private AudioClip audioClipFireEmpty;


    [Tooltip("Weapon Audio Source.")]
    [SerializeField]
    private AudioSource audioSource;


    [Tooltip("How much the weapon costs to upgrade (in points).")]
    [SerializeField]
    private int upgradeCost = 1000;


    [Tooltip("How many times can this weapon be upgraded.")]
    [SerializeField]
    private int maxUpgrade = 5;


    [Tooltip("How long it takes this weapon to upgrade to next level.")]
    [SerializeField]
    private float upgradeTime = 5;

    private bool isFiring;
    private float lastFired;
    private PlayerLook playerLook;
    private float shootTime;
    private float shootStartTime;
    private float timePassed;
    private bool isReloading = false;
    private int currentUpgradeLevel;
    private bool isUpgrading = false;
    private float semiAutoFireDelay = 0.33f;
    /// <summary>
    /// Weapon Animator.
    /// </summary>
    private Animator animator;


    /// <summary>
    /// Amount of ammunition left.
    /// </summary>
    private int ammunitionCurrent;

    private int shotsFired;


    /// <summary>
    /// Equipped Muzzle Reference.
    /// </summary>
    private MuzzleBehaviour muzzleBehaviour;


    /// <summary>
    /// The GameModeService used in this game!
    /// </summary>
    //private IGameModeService gameModeService;
    /// <summary>
    /// The main player character behaviour component.
    /// </summary>
    //private CharacterBehaviour characterBehaviour;

    /// <summary>
    /// The player character's camera.
    /// </summary>
    private Transform playerCamera;
    Transform muzzleSocket;

    private WeaponType weaponType;

    protected override void Awake()
    {
        //Get Animator.
        animator = GetComponent<Animator>();
        playerLook = GetComponentInParent<PlayerLook>();
        shootStartTime = 0;

        //Get Attachment Manager.
        weaponType = WeaponType.Gun;
        //Cache the camera
        playerCamera = playerLook.cam.transform;
        currentUpgradeLevel = 0;
    }

    protected override void Start()
    {

        //Get Muzzle.
        // muzzleBehaviour = attachmentManager.GetEquippedMuzzle();

        // lastFired = Time.time;
        // if (!isMelee)
        // {
        muzzleBehaviour = GetComponent<MuzzleBehaviour>();

        //muzzleSocket = muzzleBehaviour.GetSocket();

        //Max Out Ammo.
        ammunitionCurrent = ammunitionTotal;
        // }


    }



    public override Animator GetAnimator() => animator;

    //public override Sprite GetSpriteBody() => spriteBody;

    public override AudioClip GetAudioClipHolster() => audioClipHolster;
    public override AudioClip GetAudioClipUnholster() => audioClipUnholster;

    public override AudioClip GetAudioClipReload() => audioClipReload;
    public override AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;

    public override AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;

    public override AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();

    public override int GetAmmunitionCurrent() => ammunitionCurrent;

    public override int GetAmmunitionClip() => ammunitionClip - shotsFired;


    public override int GetAmmunitionTotal() => ammunitionTotal;

    public override bool IsAutomatic() => automatic;
    public override float GetRateOfFire() => roundsPerMinute;

    public override bool IsFull() => ammunitionCurrent == ammunitionTotal;
    public override bool HasAmmunition() => ammunitionCurrent > 0;

    public override bool IsReloading() => isReloading;

    public override string GetAmmunitionType() => ammunitionType;

    public override float GetDamage() => damage;


    public override int GetUpgradeCost() => upgradeCost;
    public override int GetMaxUpgrade() => maxUpgrade;
    public override float GetUpgradeTime() => upgradeTime;
    public override int GetCurrentUpgradeLevel() => currentUpgradeLevel;
    public override void SetWeaponIsUpgrading() => isUpgrading = true;
    public override WeaponType GetWeaponType() => weaponType;

    public override void SetUpgradeLevel(int level) => currentUpgradeLevel = level;


    //public override RuntimeAnimatorController GetAnimatorController() => controller;



    protected override void Update()
    {
        if (isUpgrading)
        {

            currentUpgradeLevel++;
            isUpgrading = false;
            damage += (((currentUpgradeLevel + 1) * (currentUpgradeLevel + 1)) * 0.5f);
            ammunitionClip += 3;
            ammunitionTotal += 20;
            spread.x *= 0.9f;
            spread.y *= 0.9f;
            spread.z *= 0.9f;
            upgradeCost = upgradeCost * ( (currentUpgradeLevel + 1) * (currentUpgradeLevel + 1));
            upgradeTime = upgradeTime * ((currentUpgradeLevel + 1) * (currentUpgradeLevel + 1));
            if (automatic)
            {
                roundsPerMinute = roundsPerMinute + ((currentUpgradeLevel + 1) * (currentUpgradeLevel + 1));
            }
            else
            {
                semiAutoFireDelay *= 0.8f;
            }


        }
        if (automatic && isFiring)
        {
            if (shootStartTime == 0)
            {
                shootStartTime = Time.time;
            }
            timePassed += Time.deltaTime;
            //if (Time.time - lastFired >= 1 / roundsPerSecond)
            if (timePassed >= 1 / (roundsPerMinute / 60))
            {
                //lastFired = Time.time;
                Fire();
                timePassed = 0;
            }
        }
        else
        {
            timePassed += Time.deltaTime;
        }
        if(automatic && !isFiring)
        {
            if(playerLook.cam.fieldOfView != 65f)
            {
                ResetFOV();
            }
        }
    }
    public override void Shoot()
    {

            if (automatic)
            {
                isFiring = true;
            }
            else
            {
                if((timePassed) > semiAutoFireDelay)
                {
                    Fire();
                    timePassed = 0;
                }

            }


    }

    public override void CancelShoot()
    {
        isFiring = false;
        shootTime = 0;
        shootStartTime = 0;
        Invoke("ResetFOV", 0.05f);
    }

    public void ReloadEvent()
    {

        if (ammunitionCurrent >= ammunitionClip)
        {
            if(shotsFired < ammunitionClip)
            {
                ammunitionCurrent -= shotsFired;
                shotsFired = 0;
            }
            else
            {
                shotsFired = 0;
                ammunitionCurrent -= ammunitionClip;
            }

        }
        else
        {
            shotsFired = ammunitionClip - ammunitionCurrent;
            ammunitionCurrent = 0;
        }
        isReloading = false;
    }

    public override void Reload()
    {
        Debug.Log("Reloading");
        if (!isReloading)
        {
            isReloading = true;
            audioSource.PlayOneShot(audioClipReload);
            animator.SetTrigger("Reload");
        }
    }
    public override void Fire(float spreadMultiplier = 1.0f)
    {
        //We need a muzzle in order to fire this weapon!
        if (muzzleBehaviour == null)
            return;

        //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
        if (playerCamera == null)
            return;


        if (shotsFired != ammunitionClip && ammunitionCurrent != 0)
        {
            //Get Muzzle Socket. This is the point we fire from.
            Transform muzzleSocket = muzzleBehaviour.GetSocket();

            shootTime = Time.time - shootStartTime;
            //Determine the rotation that we want to shoot our projectile in.
            Quaternion rotation = Quaternion.LookRotation((playerCamera.forward * 1000.0f));//- muzzleSocket.position);

            playerLook.ApplyRecoil(new Vector2(Random.Range(-spread.x, spread.x) * (spreadMultiplier) * Mathf.Clamp01(shootTime / spreadTime), Random.Range(-spread.y, spread.y)) * (spreadMultiplier) * Mathf.Clamp01(shootTime / spreadTime));

            Vector3 shootDirection;// = (playerCamera.forward * 1000f);// - muzzleSocket.position);
            Ray ray;
            //If there's something blocking, then we can aim directly at that thing, which will result in more accurate shooting.
            //RaycastHit[] hits = Physics.RaycastAll(ray = new Ray(playerCamera.position, playerCamera.forward), maximumDistance, mask);
            //bool shot = false;


            //foreach (RaycastHit hit in hits)
            if (Physics.Raycast(ray = new Ray(playerCamera.position, playerCamera.forward), out RaycastHit hit, maximumDistance, mask))
            {
                Debug.DrawRay(ray.origin, ray.direction * maximumDistance);
                Debug.Log("*****HIT TAGS*******: "+hit.collider.name);



                //if (!hit.collider.CompareTag("PlayerLimit"))
               // {
                    rotation = Quaternion.LookRotation(hit.point - muzzleSocket.position);
                    shootDirection = (hit.point - muzzleSocket.position);
                    shootDirection.x += Random.Range(-spread.x, spread.x) * Mathf.Clamp01(shootTime / spreadTime);
                    shootDirection.y += Random.Range(-spread.y, spread.y) * Mathf.Clamp01(shootTime / spreadTime);
                    shootDirection.z += Random.Range(-spread.z, spread.z) * Mathf.Clamp01(shootTime / spreadTime);

                    shootDirection.Normalize();
                    Debug.DrawRay(ray.origin, ray.direction * maximumDistance);

                playerLook.cam.fieldOfView = 65 - Mathf.Clamp(shootTime / (spreadTime/4), 0f, 4f);


                //Try to play the fire particles from the muzzle!
                muzzleBehaviour.Effect();

                    //Spawn projectile from the projectile spawn point.
                    GameObject projectile = Instantiate(prefabProjectile, muzzleSocket.position, rotation);


                    ////Add velocity to the projectile.
                    projectile.GetComponent<Rigidbody>().velocity = ((shootDirection) * projectileImpulse);
               // }

            }


            GetComponentInParent<PlayerPoints>().AddPoints(100);


            EjectCasing();

            if (automatic)
            {
                float roundsPerSec = roundsPerMinute / 60f;
                animator.speed = roundsPerSec;
            }
            animator.SetTrigger("Shooting");
            shotsFired++;

            Debug.Log("Ammunition: " + (ammunitionClip - shotsFired) + "/" + ammunitionCurrent);

        }
        else if(shotsFired == ammunitionClip && ammunitionCurrent != 0)
        {
            Debug.Log("Ammunition: " + (ammunitionClip - shotsFired) + "/" + ammunitionCurrent);

            audioSource.PlayOneShot(audioClipFireEmpty);



            Debug.Log("Ammunition: " + (ammunitionClip - shotsFired) + "/" + ammunitionCurrent);


        }
        else if(ammunitionCurrent == 0)
        {
            Debug.Log("Ammunition: " + (ammunitionClip - shotsFired) + "/" + ammunitionCurrent);
            Debug.Log("Empty");
            audioSource.PlayOneShot(audioClipReloadEmpty);

            animator.SetTrigger("ReloadEmpty");

            //reload empty
        }

    }



    public override void FillAmmunition(int amount)
    {
        //Update the value by a certain amount.
        //ammunitionCurrent = amount != 0 ? Mathf.Clamp(ammunitionCurrent + amount,
        //    0, GetAmmunitionTotal()) : ammunitionTotal;
        if(ammunitionCurrent + amount <= ammunitionTotal)
        {
            ammunitionCurrent += amount;
        }
        else
        {
            ammunitionCurrent = ammunitionTotal;
        }
    }

    public override void EjectCasing()
    {
        //Spawn casing prefab at spawn point.
        if (prefabCasing != null && socketEjection != null)
            Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
    }

    private void ResetFOV()
    {
        playerLook.cam.fieldOfView = 65;
        animator.speed = 1;
    }






}