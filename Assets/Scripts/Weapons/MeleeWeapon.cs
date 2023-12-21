// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEditor.PackageManager;
using UnityEngine;


/// <summary>
/// Weapon. This class handles most of the things that weapons need.
/// </summary>
public class MeleeWeapon : MeleeWeaponBehaviour
{


    [Header("Melee Weapon Stats")]

    [Tooltip("Weapon Damage.")]
    [SerializeField]
    private float damage = 1f;

    [Tooltip("Attack Speed(How long the attack lasts.")]
    [SerializeField]
    private float attackSpeed = 2f;

    [Tooltip("How far can the weapon reach?")]
    [SerializeField]
    private float meleeReach = 2.0f;

    [Tooltip("Impact Blood Effect Prefabs.")]
    [SerializeField]
    public Transform[] bloodImpactPrefabs;




    [Header("Audio Clips.")]

    [Tooltip("Holster Audio Clip.")]
    [SerializeField]
    private AudioClip audioClipHolster;

    [Tooltip("Unholster Audio Clip.")]
    [SerializeField]
    private AudioClip audioClipUnholster;


    [Tooltip("Melee attack swing Audio Clip.")]
    [SerializeField]
    private AudioClip meleeSwingSound;

    [Tooltip("Melee attack hit Audio Clip.")]
    [SerializeField]
    private AudioClip meleeHitSound;

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

    [Tooltip("Mask of things recognized when hitting.")]
    [SerializeField]
    private LayerMask mask;

    private bool meleeIsAttacking = false;
    private bool meleeReadyToAttack = true;
    private PlayerLook playerLook;
    private int currentUpgradeLevel;
    private bool isUpgrading = false;
    /// <summary>
    /// Weapon Animator.
    /// </summary>
    private Animator animator;







    /// <summary>
    /// The player character's camera.
    /// </summary>
    private Transform playerCamera;

    [SerializeField]
    private WeaponType weaponType;

    protected override void Awake()
    {
        //Get Animator.
        animator = GetComponent<Animator>();
        playerLook = GetComponentInParent<PlayerLook>();

        //Get Attachment Manager.
        //Cache the camera
        playerCamera = playerLook.cam.transform;
        currentUpgradeLevel = 0;
    }

    protected override void Start()
    {


    }



    public override Animator GetAnimator() => animator;


    public override AudioClip GetAudioClipHolster() => audioClipHolster;
    public override AudioClip GetAudioClipUnholster() => audioClipUnholster;



    public override float GetWeaponHitDelay() => attackSpeed;


    public override float GetDamage() => damage;
    public override WeaponType GetWeaponType() => weaponType;


    public override int GetUpgradeCost() => upgradeCost;
    public override int GetMaxUpgrade() => maxUpgrade;
    public override float GetUpgradeTime() => upgradeTime;
    public override int GetCurrentUpgradeLevel() => currentUpgradeLevel;
    public override void SetWeaponIsUpgrading() => isUpgrading = true;
    public override AudioClip GetAudioClipHit() => meleeHitSound;

    public override void SetUpgradeLevel(int level) => currentUpgradeLevel = level;


    //public override RuntimeAnimatorController GetAnimatorController() => controller;



    protected override void Update()
    {
        if (isUpgrading)
        {

            currentUpgradeLevel++;
            isUpgrading = false;
            damage += (((currentUpgradeLevel + 1) * (currentUpgradeLevel + 1)) * 0.5f);
            upgradeCost = upgradeCost * ( (currentUpgradeLevel + 1) * (currentUpgradeLevel + 1));
            upgradeTime = upgradeTime * ((currentUpgradeLevel + 1) * (currentUpgradeLevel + 1));
        }
    }
    public override void Shoot()
    {

            Attack();
       

    }

    public override void CancelShoot()
    {
        return;
    }




    public override void Attack()
    {
        //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
        Debug.Log("Test attk");
        if (playerCamera == null)
        {
            return;
        }
        if (!meleeReadyToAttack || meleeIsAttacking)
        {
            return;
        }
        Debug.Log("got through player camera check");
        meleeIsAttacking = true;
        meleeReadyToAttack = false;


        Invoke(nameof(ResetAttack), attackSpeed);
        AttackRayCast();


    }

    public override void ResetAttack()
    {
        meleeIsAttacking = false;
        meleeReadyToAttack = true;
    }
    public override void AttackRayCast()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        Debug.DrawRay(ray.origin, ray.direction * meleeReach);
        RaycastHit hitInfo;
        animator.SetTrigger("Hit");
        // meleeIsAttacking = true;
        // meleeReadyToAttack = false;


        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(meleeSwingSound);
        if (Physics.Raycast(ray, out hitInfo, meleeReach, mask))
        {
            HitTarget(hitInfo);
        }
    }

    protected override void HitTarget(RaycastHit hitInfo)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(meleeHitSound);

        Enemy enemy = hitInfo.collider.gameObject.GetComponent<Enemy>();
        enemy.DamageEnemy(damage);


        //if (hitInfo.collider.CompareTag("Zombie"))
        //{
        //    Debug.Log("HIT ZOMBIE");
        //    Instantiate(bloodImpactPrefabs[Random.Range(0, bloodImpactPrefabs.Length)], hitInfo.point,Quaternion.LookRotation(hitInfo.normal));
        //}
    }









}