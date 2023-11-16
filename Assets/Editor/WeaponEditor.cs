using System.Numerics;
using UnityEditor;


[CustomEditor(typeof(Weapon), true), CanEditMultipleObjects]
public class WeaponEditor : Editor
{

    SerializedProperty isMelee;
    SerializedProperty damage;
    SerializedProperty meleeAttackLength;
    SerializedProperty meleeReach;

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




    private void OnEnable()
    {
        isMelee = serializedObject.FindProperty("isMelee");
        damage = serializedObject.FindProperty("damage");
        meleeAttackLength = serializedObject.FindProperty("meleeAttackLength");
        meleeReach = serializedObject.FindProperty("meleeReach");

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


    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(isMelee);

        if (isMelee.boolValue)
        {
            EditorGUILayout.PropertyField(damage);
            EditorGUILayout.PropertyField(meleeAttackLength);
            EditorGUILayout.PropertyField(meleeReach);
            EditorGUILayout.PropertyField(mask);
            EditorGUILayout.PropertyField(audioClipHolster);
            EditorGUILayout.PropertyField(audioClipUnholster);

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
