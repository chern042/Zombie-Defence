using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLife : MonoBehaviour
{

    [SerializeField]
    private float playerMaxHealth = 100f;

    [SerializeField]
    private float chipSpeed = 2f;


    [SerializeField]
    private Image currentHealthBar;

    [SerializeField]
    private Image dynamicDamageHealthBar;

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private Image overlay;

    [SerializeField]
    private float duration = 2f;

    [SerializeField]
    private float fadeSpeed = 1.5f;


    private float durationTimer;
    private float playerCurrentHealth;
    private float lerpTimer;


    private bool playerAlive;
    public bool PlayerAlive { get => playerAlive; }

    // Start is called before the first frame update
    void Start()
    {
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        playerCurrentHealth = playerMaxHealth;
        playerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, playerMaxHealth);
        UpdateHealthUI();
        if(overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI()
    {
        float fillCHB = currentHealthBar.fillAmount;
        float fillDDHB = dynamicDamageHealthBar.fillAmount;
        float hFraction = playerCurrentHealth / playerMaxHealth;
        if(fillDDHB > hFraction)
        {
            currentHealthBar.fillAmount = hFraction;
            dynamicDamageHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            dynamicDamageHealthBar.fillAmount = Mathf.Lerp(fillDDHB, hFraction, percentComplete);
        }
        if(fillCHB < hFraction)
        {
            dynamicDamageHealthBar.color = Color.green;
            dynamicDamageHealthBar.fillAmount = hFraction;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            currentHealthBar.fillAmount = Mathf.Lerp(fillCHB, dynamicDamageHealthBar.fillAmount, percentComplete);
        }
    }

    public void RestorePlayerHealth(float healAmount)
    {
        playerCurrentHealth += healAmount;
        healthText.text = Mathf.RoundToInt(playerCurrentHealth).ToString() + "/100";
        lerpTimer = 0f;
    }

    public void DamagePlayer(float damage)
    {
        playerCurrentHealth -= damage;
        healthText.text = Mathf.RoundToInt(playerCurrentHealth).ToString() + "/100";
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        lerpTimer = 0f;
        durationTimer = 0f;
        if(playerCurrentHealth <= 0)
        {
            healthText.transform.position = Vector3.zero;
            healthText.text = "YOU ARE DEAD";
            Debug.Log("PLAYER DEAD");
            playerAlive = false;
        }
    }
 
}
