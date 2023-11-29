using System.Numerics;
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
    SerializedProperty roundsPerMinute;
    SerializedProperty mask;
    SerializedProperty maximumDistance;
    SerializedProperty ammunitionTotal;
    SerializedProperty ammunitionClip;
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
        roundsPerMinute = serializedObject.FindProperty("roundsPerMinute");
        mask = serializedObject.FindProperty("mask");
        maximumDistance = serializedObject.FindProperty("maximumDistance");
        ammunitionTotal = serializedObject.FindProperty("ammunitionTotal");
        ammunitionClip = serializedObject.FindProperty("ammunitionClip");
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
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(mask);
        EditorGUILayout.PropertyField(bloodImpactPrefabs);


        if (isMelee.boolValue)
        {
            EditorGUILayout.PropertyField(attackSpeed);
            EditorGUILayout.PropertyField(meleeReach);



            EditorGUILayout.PropertyField(meleeSwingSound);
            EditorGUILayout.PropertyField(meleeHitSound);
            EditorGUILayout.PropertyField(audioSource);
        }
        else
        {
            EditorGUILayout.PropertyField(automatic);
            EditorGUILayout.PropertyField(projectileImpulse);
            EditorGUILayout.PropertyField(roundsPerMinute);

            EditorGUILayout.PropertyField(maximumDistance);
            EditorGUILayout.PropertyField(ammunitionTotal);
            EditorGUILayout.PropertyField(ammunitionClip);
            EditorGUILayout.PropertyField(spread);
            EditorGUILayout.PropertyField(spreadTime);

            EditorGUILayout.PropertyField(socketEjection);
            EditorGUILayout.PropertyField(prefabCasing);
            EditorGUILayout.PropertyField(prefabProjectile);

            EditorGUILayout.PropertyField(audioClipReload);
            EditorGUILayout.PropertyField(audioClipReloadEmpty);
            EditorGUILayout.PropertyField(audioClipFireEmpty);


        }
        EditorGUILayout.PropertyField(audioClipHolster);
        EditorGUILayout.PropertyField(audioClipUnholster);
        serializedObject.ApplyModifiedProperties();


    }






}
