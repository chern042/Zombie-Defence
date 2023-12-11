// Copyright 2021, Infima Games. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class MeleeWeaponBehaviour : WeaponBehaviour
{
    public abstract AudioClip GetAudioClipHit();

    public abstract float GetWeaponHitDelay();

    public abstract void Attack();

    public abstract void ResetAttack();

    public abstract void AttackRayCast();

    protected abstract void HitTarget(RaycastHit hitInfo);

}