﻿using System.Numerics;
using UnityEditor;


[CustomEditor(typeof(Weapon), true), CanEditMultipleObjects]
public class WeaponEditor : Editor
{

    SerializedProperty isMelee;
    SerializedProperty damage;
    SerializedProperty attackSpeed;
    SerializedProperty meleeReach;
    SerializedProperty bloodImpactPrefabs;

    SerializedProperty automatic;
    SerializedProperty projectileImpulse;
    SerializedProperty roundsPerSecond;
    SerializedProperty mask;
    SerializedProperty maximumDistance;
    SerializedProperty ammunitionTotal;
    SerializedProperty spread;
    SerializedProperty spreadTime;

    SerializedProperty socketEjection;
    SerializedProperty prefabCasing;
    SerializedProperty prefabProjectile;
    SerializedProperty audioClipHolster;
    SerializedProperty audioClipUnholster;
    SerializedProperty audioClipReload;
    SerializedProperty audioClipReloadEmpty;
    SerializedProperty audioClipFireEmpty;

    SerializedProperty meleeSwingSound;
    SerializedProperty meleeHitSound;
    SerializedProperty audioSource;




    private void OnEnable()
    {
        isMelee = serializedObject.FindProperty("isMelee");
        damage = serializedObject.FindProperty("damage");
        attackSpeed = serializedObject.FindProperty("attackSpeed");
        meleeReach = serializedObject.FindProperty("meleeReach");
        bloodImpactPrefabs = serializedObject.FindProperty("bloodImpactPrefabs");

        automatic = serializedObject.FindProperty("automatic");
        projectileImpulse = serializedObject.FindProperty("projectileImpulse");
        roundsPerSecond = serializedObject.FindProperty("roundsPerSecond");
        mask = serializedObject.FindProperty("mask");
        maximumDistance = serializedObject.FindProperty("maximumDistance");
        ammunitionTotal = serializedObject.FindProperty("ammunitionTotal");
        spread = serializedObject.FindProperty("spread");
        spreadTime = serializedObject.FindProperty("spreadTime");

        socketEjection = serializedObject.FindProperty("socketEjection");
        prefabCasing = serializedObject.FindProperty("prefabCasing");
        prefabProjectile = serializedObject.FindProperty("prefabProjectile");
        audioClipHolster = serializedObject.FindProperty("audioClipHolster");
        audioClipUnholster = serializedObject.FindProperty("audioClipUnholster");
        audioClipReload = serializedObject.FindProperty("audioClipReload");
        audioClipReloadEmpty = serializedObject.FindProperty("audioClipReloadEmpty");
        audioClipFireEmpty = serializedObject.FindProperty("audioClipFireEmpty");

        meleeSwingSound = serializedObject.FindProperty("meleeSwingSound");
        meleeHitSound = serializedObject.FindProperty("meleeHitSound");
        audioSource = serializedObject.FindProperty("audioSource");


    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(isMelee);

        if (isMelee.boolValue)
        {
            EditorGUILayout.PropertyField(damage);
            EditorGUILayout.PropertyField(attackSpeed);
            EditorGUILayout.PropertyField(meleeReach);
            EditorGUILayout.PropertyField(bloodImpactPrefabs);

            EditorGUILayout.PropertyField(mask);
            EditorGUILayout.PropertyField(audioClipHolster);
            EditorGUILayout.PropertyField(audioClipUnholster);

            EditorGUILayout.PropertyField(meleeSwingSound);
            EditorGUILayout.PropertyField(meleeHitSound);
            EditorGUILayout.PropertyField(audioSource);
        }
        else
        {
            EditorGUILayout.PropertyField(automatic);
            EditorGUILayout.PropertyField(projectileImpulse);
            EditorGUILayout.PropertyField(roundsPerSecond);
            EditorGUILayout.PropertyField(mask);
            EditorGUILayout.PropertyField(maximumDistance);
            EditorGUILayout.PropertyField(ammunitionTotal);
            EditorGUILayout.PropertyField(spread);
            EditorGUILayout.PropertyField(spreadTime);

            EditorGUILayout.PropertyField(socketEjection);
            EditorGUILayout.PropertyField(prefabCasing);
            EditorGUILayout.PropertyField(prefabProjectile);
            EditorGUILayout.PropertyField(audioClipHolster);
            EditorGUILayout.PropertyField(audioClipUnholster);
            EditorGUILayout.PropertyField(audioClipReload);
            EditorGUILayout.PropertyField(audioClipReloadEmpty);
            EditorGUILayout.PropertyField(audioClipFireEmpty);


        }
        serializedObject.ApplyModifiedProperties();


    }






}