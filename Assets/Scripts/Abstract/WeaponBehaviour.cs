﻿// Copyright 2021, Infima Games. All Rights Reserved.

using System;
using UnityEngine;

    public abstract class WeaponBehaviour : MonoBehaviour
    {


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

        /// <summary>
        /// Returns the holster audio clip.
        /// </summary>
        public abstract AudioClip GetAudioClipHolster();
        /// <summary>
        /// Returns the unholster audio clip.
        /// </summary>
        public abstract AudioClip GetAudioClipUnholster();

        /// <summary>
        /// Returns the reload audio clip.
        /// </summary>
        public abstract AudioClip GetAudioClipReload();
        /// <summary>
        /// Returns the reload empty audio clip.
        /// </summary>
        public abstract AudioClip GetAudioClipReloadEmpty();

        /// <summary>
        /// Returns the fire empty audio clip.
        /// </summary>
        public abstract AudioClip GetAudioClipFireEmpty();

        /// <summary>
        /// Returns the fire audio clip.
        /// </summary>
        public abstract AudioClip GetAudioClipFire();

        /// <summary>
        /// Returns Current Ammunition. 
        /// </summary>
        public abstract int GetAmmunitionCurrent();
        /// <summary>
        /// Returns Total Ammunition.
        /// </summary>
        public abstract int GetAmmunitionTotal();

    public abstract string GetAmmunitionType();


    /// <summary>
    /// Returns the Weapon's Animator component.
    /// </summary>
    public abstract Animator GetAnimator();

        /// <summary>
        /// Returns true if this weapon shoots in automatic.
        /// </summary>
        public abstract bool IsAutomatic();
        /// <summary>
        /// Returns true if the weapon has any ammunition left.
        /// </summary>
        public abstract bool HasAmmunition();



    public abstract int GetAmmunitionClip();
    /// <summary>
    /// Returns true if the weapon is full of ammunition.
    /// </summary>
    public abstract bool IsFull();
        /// <summary>
        /// Returns the weapon's rate of fire.
        /// </summary>
        public abstract float GetRateOfFire();

    public abstract bool IsMelee();

    public abstract bool IsReloading();


        /// <summary>
        /// Returns the RuntimeAnimationController the Character needs to use when this Weapon is equipped!
        /// </summary>
    //public abstract RuntimeAnimatorController GetAnimatorController();


    /// <summary>
    /// Fires the weapon.
    /// </summary>
    /// <param name="spreadMultiplier">Value to multiply the weapon's spread by. Very helpful to account for aimed spread multipliers.</param>
        public abstract void Fire(float spreadMultiplier = 1.0f);


    public abstract void Shoot();
    public abstract void CancelShoot();
        /// <summary>
        /// Reloads the weapon.
        /// </summary>
        public abstract void Reload();

        /// <summary>
        /// Fills the character's equipped weapon's ammunition by a certain amount, or fully if set to -1.
        /// </summary>
        public abstract void FillAmmunition(int amount);

        /// <summary>
        /// Ejects a casing from the weapon. This is commonly called from animation events, but can be called from anywhere.
        /// </summary>
        public abstract void EjectCasing();

        public abstract void Attack();

}