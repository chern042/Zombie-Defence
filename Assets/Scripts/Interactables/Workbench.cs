using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class Workbench : Interactable
{
    [SerializeField]
    private GameObject[] weaponPrefabs;


    [SerializeField]
    private GameObject playerHand;

    [SerializeField]
    private GameObject switchWeaponButton;

    [SerializeField]
    private GameObject upgradeBar;

    [SerializeField]
    private Image upgradeBarCurrent;

    private WeaponBehaviour weapon;

    private GameObject workbenchWeapon;

    private int? upgradeCost;

    private float upgradeTime;

    private bool isUpgrading;

    private PlayerPoints playerPoints;

    private void Awake()
    {
        weapon = playerHand.GetComponentInChildren<WeaponBehaviour>();
        upgradeCost = weapon.GetUpgradeCost();
        upgradeTime = 0;
        isUpgrading = false;
        playerPoints = playerHand.GetComponentInParent<PlayerPoints>();
    }

    protected override void OnLook()
    {
        weapon = playerHand.GetComponentInChildren<WeaponBehaviour>();
        playerPoints = playerHand.GetComponentInParent<PlayerPoints>();

        if (weapon != null && switchWeaponButton != null)
        {
            upgradeCost = weapon.GetUpgradeCost();
            switchWeaponButton.SetActive(false);
        }
        if (!isUpgrading)
        {

            if ((interactButton != null && promptMessage.Substring(0, 7) == "Upgrade") && weapon.GetCurrentUpgradeLevel() < weapon.GetMaxUpgrade())
            {

                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "UPGRADE";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                interactButton.GetComponent<Image>().enabled = true;
                interactButton.GetComponent<OnScreenButton>().enabled = true;
            }
        }
        else
        {
            upgradeBar.SetActive(true);
        }

        base.OnLook();

    }

    public override void OnLookOff()
    {
        base.OnLookOff();
    
        if (switchWeaponButton != null)
        {
            switchWeaponButton.SetActive(true);

        }
        if (!isUpgrading)
        {
            promptMessage = "Upgrade Weapon (" + upgradeCost + ")";
            if (interactButton != null)
            {

                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                interactButton.GetComponent<Image>().enabled = false;
                interactButton.GetComponent<OnScreenButton>().enabled = false;
            }
        }
        else
        {
            upgradeBar.SetActive(false);
        }
    }



    protected override void Interact()
    {
        if((playerPoints.points >= upgradeCost && !isUpgrading) && weapon.GetCurrentUpgradeLevel() <= weapon.GetMaxUpgrade())
        {
            playerPoints.RemovePoints((int)upgradeCost);
            foreach(GameObject wep in weaponPrefabs)
            {
                if(wep.name.Substring(0,3) == weapon.name.Substring(0, 3))
                {
                    workbenchWeapon = wep;
                }
            }
            workbenchWeapon.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            isUpgrading = true;
            promptMessage = "";
            upgradeBar.SetActive(true);
            upgradeBarCurrent.fillAmount = 0;

        }else if(weapon.GetCurrentUpgradeLevel() == weapon.GetMaxUpgrade())
        {
            promptMessage = "No More Upgrades";
        }
        else
        {
            Debug.Log("NO POINTS");
            promptMessage = "Insufficent Points";
        }
    }


    public override void CancelInteract()
    {
        //promptMessage = "Upgrade Weapon (" + upgradeCost + ")";

    }
    // Start is called before the first frame update
    void Start()
    {
        if (upgradeCost != null)
        {
            if (weapon.GetCurrentUpgradeLevel() < weapon.GetMaxUpgrade())
            {
                promptMessage = "Upgrade Weapon (" + upgradeCost + ")";
            }
            else
            {
                promptMessage = "No More Upgrades";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpgrading)
        {
            upgradeTime += Time.deltaTime;
            float percentComplete = upgradeTime / weapon.GetUpgradeTime();
            if (upgradeBar.activeInHierarchy)
            {
                upgradeBarCurrent.fillAmount = percentComplete;
            }
            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            interactButton.GetComponent<Image>().enabled = false;
            interactButton.GetComponent<OnScreenButton>().enabled = false;

            if (upgradeTime >= weapon.GetUpgradeTime())
            {
                weapon.SetWeaponIsUpgrading();
                isUpgrading = false;
                upgradeTime = 0;
                upgradeBar.SetActive(false);
                workbenchWeapon.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                interactButton.GetComponent<Image>().enabled = true;
                interactButton.GetComponent<OnScreenButton>().enabled = true;

            }
        }
    }
}
