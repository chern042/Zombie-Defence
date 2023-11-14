using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Muzzle : MuzzleBehaviour
{

    [Tooltip("Socket at the tip of the Muzzle. Commonly used as a firing point.")]
    [SerializeField]
    private Transform socket;

    [Tooltip("Audio clip played when firing through this muzzle.")]
    [SerializeField]
    private AudioClip audioClipFire;

    [Header("Particles")]

    [Tooltip("Muzzle Flash Firing Particles.")]
    [SerializeField]
    private GameObject prefabFlashParticles;

    [Tooltip("Number of particles to emit when firing.")]
    [SerializeField]
    private int flashParticlesCount = 5;

    [Header("Flash Light")]

    [Tooltip("Muzzle Flash light Prefab. A small light we use when firing.")]
    [SerializeField]
    private GameObject prefabFlashLight;

    [Tooltip("Time that the light flashed stays active. After this time, it is disabled.")]
    [SerializeField]
    private float flashLightDuration;




    private ParticleSystem particles;
    private Light flashLight;
    public AudioSource audioSource;


    public override Transform GetSocket() => socket;

    //public override Sprite GetSprite() => sprite;
    public override AudioClip GetAudioClipFire() => audioClipFire;

    public override ParticleSystem GetParticlesFire() => particles;
    public override int GetParticlesFireCount() => flashParticlesCount;

    public override Light GetFlashLight() => flashLight;
    public override float GetFlashLightDuration() => flashLightDuration;

    private void Awake()
    {
        //Null Check.
        if (prefabFlashParticles != null)
        {
            //Instantiate Particles.
            GameObject spawnedParticlesPrefab = Instantiate(prefabFlashParticles, socket);
            //Reset the position.
            spawnedParticlesPrefab.transform.localPosition = default;
            //Reset the rotation.
            spawnedParticlesPrefab.transform.localEulerAngles = new Vector3(0,90f,0);

            //Get Reference.
            particles = spawnedParticlesPrefab.GetComponent<ParticleSystem>();
        }

        //Null Check.
        if (prefabFlashLight)
        {
            //Instantiate.
            GameObject spawnedFlashLightPrefab = Instantiate(prefabFlashLight, socket);
            //Reset the position.
            //spawnedFlashLightPrefab.transform.localPosition = flashLightOffset;
            //Reset the rotation.
            spawnedFlashLightPrefab.transform.localEulerAngles = default;

            //Get reference.
            flashLight = spawnedFlashLightPrefab.GetComponent<Light>();
            //Disable.
            flashLight.enabled = false;
        }
    }


    public override void Effect()
    {
        //Try to play the fire particles from the muzzle!
        if (particles != null)
            particles.Emit(flashParticlesCount);

        //Make sure that we have a light to flash!
        if (flashLight != null)
        {
            //Enable the light.
            flashLight.enabled = true;
            //Disable the light after a few seconds.
            StartCoroutine(nameof(DisableLight));
        }

        if (audioClipFire != null)
        {
            audioSource.clip = audioClipFire;
            audioSource.Play();
        }
    }

    private IEnumerator DisableLight()
    {
        //Wait.
        yield return new WaitForSeconds(flashLightDuration);
        //Disable.
        flashLight.enabled = false;
    }

}
