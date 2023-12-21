// Copyright 2021, Infima Games. All Rights Reserved.

using System;
using UnityEngine;

    public abstract class WeaponBehaviour : MonoBehaviour
    {

        public enum WeaponType { Melee, Handgun, Shotgun, Rifle}
        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake() { }

        /// <summary>
        /// Start.
        /// </summary>
        protected virtual void Start() { }

        /// <summary>
        /// Update.
        /// </summary>
        protected virtual void Update() { }

        /// <summary>
        /// Late Update.
        /// </summary>
        protected virtual void LateUpdate() { }



    /// <summary>
    /// Returns the sprite to use when displaying the weapon's body.
    /// </summary>
    /// <returns></returns>
    //public abstract Sprite GetSpriteBody();
    public abstract WeaponType GetWeaponType();
        /// <summary>
        /// Returns the holster audio clip.
        /// </summary>
        public abstract AudioClip GetAudioClipHolster();
        /// <summary>
        /// Returns the unholster audio clip.
        /// </summary>
        public abstract AudioClip GetAudioClipUnholster();


    public abstract float GetDamage();

    /// <summary>
    /// Returns the Weapon's Animator component.
    /// </summary>
    public abstract Animator GetAnimator();
    
    public abstract int GetUpgradeCost();
    public abstract int GetMaxUpgrade();
    public abstract float GetUpgradeTime();
    public abstract int GetCurrentUpgradeLevel();
    public abstract void SetWeaponIsUpgrading();
    public abstract void SetUpgradeLevel(int level);




    public abstract void Shoot();
    public abstract void CancelShoot();
      


}