// Copyright 2021, Infima Games. All Rights Reserved.

using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

    /// <summary>
    /// Weapon. This class handles most of the things that weapons need.
    /// </summary>
    public class Weapon : WeaponBehaviour
    {

        [SerializeField, HideInInspector]
        private bool isMelee;

        [Header("Melee Weapon Stats")]

        [Tooltip("Weapon Damage.")]
        [SerializeField, HideInInspector]
        private float damage = 1f;

        [Tooltip("Attack Speed(How long the attack lasts.")]
        [SerializeField, HideInInspector]
        private float attackSpeed = 2f;

        [Tooltip("How far can the weapon reach?")]
        [SerializeField, HideInInspector]
        private float meleeReach = 2.0f;

        [Tooltip("Impact Blood Effect Prefabs.")]
        [SerializeField, HideInInspector]
        public Transform[] bloodImpactPrefabs;



    [Header("Firing")]

        [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
        [SerializeField, HideInInspector]
        private bool automatic;

        [Tooltip("How fast the projectiles are.")]
        [SerializeField, HideInInspector]
        private float projectileImpulse = 400.0f;

        [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
        [SerializeField, HideInInspector]
        private float roundsPerSecond = 4f;

        [Tooltip("Mask of things recognized when firing.")]
        [SerializeField, HideInInspector]
        private LayerMask mask;

        [Tooltip("Maximum distance at which this weapon can fire accurately. Shots beyond this distance will not use linetracing for accuracy.")]
        [SerializeField, HideInInspector]
        private float maximumDistance = 500.0f;

        [Tooltip("Total Ammunition.")]
        [SerializeField, HideInInspector]
        private int ammunitionTotal = 10;

        [Tooltip("Fire Spread.")]
        [SerializeField, HideInInspector]
        private Vector3 spread = new Vector3(0.01f,0.01f,0.01f);

        [Tooltip("Fire Spread Max Time.")]
        [SerializeField, HideInInspector]
        private float spreadTime = 5f;




    [Header("Animation")]

        [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
        [SerializeField, HideInInspector]
        private Transform socketEjection;

        [Header("Resources")]

        [Tooltip("Casing Prefab.")]
        [SerializeField, HideInInspector]
        private GameObject prefabCasing;

        [Tooltip("Projectile Prefab. This is the prefab spawned when the weapon shoots.")]
        [SerializeField, HideInInspector]
        private GameObject prefabProjectile;

        //[Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")]
        //[SerializeField]
        //public RuntimeAnimatorController controller;

        //[Tooltip("Weapon Body Texture.")]
        //[SerializeField]
        //private Sprite spriteBody;


        [Header("Audio Clips.")]

        [Tooltip("Holster Audio Clip.")]
        [SerializeField, HideInInspector]
        private AudioClip audioClipHolster;

        [Tooltip("Unholster Audio Clip.")]
        [SerializeField, HideInInspector]
        private AudioClip audioClipUnholster;

        [Header("Audio Clips Reloads")]

        [Tooltip("Reload Audio Clip.")]
        [SerializeField, HideInInspector]
        private AudioClip audioClipReload;

        [Tooltip("Reload Empty Audio Clip.")]
        [SerializeField, HideInInspector]
        private AudioClip audioClipReloadEmpty;

        [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
        [SerializeField, HideInInspector]
        private AudioClip audioClipFireEmpty;


        [Tooltip("Melee attack swing Audio Clip.")]
        [SerializeField, HideInInspector]
        private AudioClip meleeSwingSound;

        [Tooltip("Melee attack hit Audio Clip.")]
        [SerializeField, HideInInspector]
        private AudioClip meleeHitSound;

        [Tooltip("Melee Weapon Audio Source.")]
        [SerializeField, HideInInspector]
        private AudioSource audioSource;


    private bool meleeIsAttacking = false;
        private bool meleeReadyToAttack = true;
        private bool isFiring;
        private float lastFired;
        private PlayerLook playerLook;
        private float shootTime;
        private float shootStartTime;

        /// <summary>
        /// Weapon Animator.
        /// </summary>
        private Animator animator;


        /// <summary>
        /// Amount of ammunition left.
        /// </summary>
        private int ammunitionCurrent;


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



        protected override void Awake()
        {
            //Get Animator.
            animator = GetComponent<Animator>();
        playerLook = GetComponentInParent<PlayerLook>();
        shootStartTime = 0;
        //Get Attachment Manager.

        //Cache the game mode service. We only need this right here, but we'll cache it in case we ever need it again.
        // gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        //Cache the player character.
        //characterBehaviour = gameModeService.GetPlayerCharacter();
        //Cache the world camera. We use this in line traces.
        //playerCamera = characterBehaviour.GetCameraWorld().transform;
        playerCamera =  playerLook.cam.transform;
    }

        protected override void Start()
        {

        //Get Muzzle.
        // muzzleBehaviour = attachmentManager.GetEquippedMuzzle();

       // lastFired = Time.time;

        muzzleBehaviour = GetComponent<MuzzleBehaviour>();

            //Max Out Ammo.
            ammunitionCurrent = ammunitionTotal;
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

        public override int GetAmmunitionTotal() => ammunitionTotal;

        public override bool IsAutomatic() => automatic;
        public override float GetRateOfFire() => roundsPerSecond;

        public override bool IsFull() => ammunitionCurrent == ammunitionTotal;
        public override bool HasAmmunition() => ammunitionCurrent > 0;

        public override bool IsMelee() => isMelee;


    //public override RuntimeAnimatorController GetAnimatorController() => controller;



    protected override void Update()
    {
        if (automatic && isFiring)
        {
            if(shootStartTime == 0)
            {
                shootStartTime = Time.time;
            }
            if (Time.time - lastFired > 1 / roundsPerSecond)
            {
                lastFired = Time.time;
                Fire(3f);
            }
        }
    }
    public override void Shoot()
    {
        Debug.Log("Fire Test is melee: " + isMelee+" name: "+gameObject.name);
        if (isMelee)
        {
            Attack();
        }
        else
        {
            if (automatic)
            {
                isFiring = true;
            }
            else
            {
                Fire();
            }
        }

    }

    public override void CancelShoot()
    {
        if (isMelee) return;
        isFiring = false;
        shootTime = 0;
        shootStartTime = 0;
    }

    public override void Reload()
        {
            //Play Reload Animation.
            //animator.Play(HasAmmunition() ? "Reload" : "Reload Empty", 0, 0.0f);
        }
        public override void Fire(float spreadMultiplier = 1.0f)
        {
            //We need a muzzle in order to fire this weapon!
            if (muzzleBehaviour == null)
                return;

        //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
        if (playerCamera == null)
                return;




        //Get Muzzle Socket. This is the point we fire from.
        Transform muzzleSocket = muzzleBehaviour.GetSocket();

        shootTime = Time.time - shootStartTime;
        //Determine the rotation that we want to shoot our projectile in.
        Quaternion rotation = Quaternion.LookRotation(playerCamera.forward * 1000.0f - muzzleSocket.position);



        Vector3 shootDirection = playerCamera.forward + new Vector3(Random.Range(-spread.x,spread.x)*Mathf.Clamp(shootTime,1f,spreadTime), Random.Range(-spread.y, spread.y)* Mathf.Clamp(shootTime, 1f, spreadTime), Random.Range(-spread.z, spread.z)* Mathf.Clamp(shootTime, 1f, spreadTime))*spreadMultiplier;
        //Vector3 shootDirection = playerCamera.forward;
        shootDirection.Normalize();
        Ray ray;
        //If there's something blocking, then we can aim directly at that thing, which will result in more accurate shooting.
        if (Physics.Raycast(ray = new Ray(playerCamera.position, playerCamera.forward), out RaycastHit hit, maximumDistance, mask))
        {
            Debug.DrawRay(ray.origin, ray.direction * maximumDistance);
            rotation = Quaternion.LookRotation(hit.point - muzzleSocket.position);
        }
        Debug.DrawRay(ray.origin, ray.direction * maximumDistance);

        playerLook.cam.fieldOfView = 64;
        playerLook.ApplyRecoil(new Vector2(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y)) * (spreadMultiplier) * Mathf.Clamp01(shootTime/spreadTime));
        animator.SetTrigger("Shooting");
        //Try to play the fire particles from the muzzle!
        muzzleBehaviour.Effect();

        //Spawn projectile from the projectile spawn point.
        GameObject projectile = Instantiate(prefabProjectile, muzzleSocket.position, rotation);
        //Add velocity to the projectile.
        projectile.GetComponent<Rigidbody>().velocity =((projectile.transform.forward+shootDirection) * projectileImpulse);
        Debug.Log("Rotation 2: " + rotation);

        EjectCasing();
        Invoke("ResetFOV",0.05f);
    }


    public override void Attack()
    {
        //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
        Debug.Log("Test attk");
        if (playerCamera == null) return;
        if (!meleeReadyToAttack || meleeIsAttacking) return;
        Debug.Log("got through player camera check");



        Invoke(nameof(ResetAttack), attackSpeed);
        AttackRayCast();


    }

    public void ResetAttack()
    {
        meleeIsAttacking = false;
        meleeReadyToAttack = true;
    }
    public void AttackRayCast()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        Debug.DrawRay(ray.origin, ray.direction * meleeReach);
        RaycastHit hitInfo;
        animator.SetTrigger("Hit");
        meleeIsAttacking = true;
        meleeReadyToAttack = false;


        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(meleeSwingSound);
        if (Physics.Raycast(ray, out hitInfo, meleeReach, mask))
        {
            HitTarget(hitInfo);
        }
    }

    private void HitTarget(RaycastHit hitInfo)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(meleeHitSound);

        if (hitInfo.collider.CompareTag("Zombie"))
        {
            Debug.Log("HIT ZOMBIE");
            Instantiate(bloodImpactPrefabs[Random.Range(0, bloodImpactPrefabs.Length)], hitInfo.point,Quaternion.LookRotation(hitInfo.normal));
        }
    }



    public override void FillAmmunition(int amount)
        {
            //Update the value by a certain amount.
            ammunitionCurrent = amount != 0 ? Mathf.Clamp(ammunitionCurrent + amount,
                0, GetAmmunitionTotal()) : ammunitionTotal;
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
    }






}