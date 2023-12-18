using System.Collections;
using System.Collections.Generic;
using EvolveGames;
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

    private float weaponUpgradeTime;

    private bool isUpgrading;

    private PlayerPoints playerPoints;

    private int itemUpgradingIndex;

    private bool isUpgraded;

    private bool finishedUpgrade;

    private int weaponUpgradeLevel;

    private void Awake()
    {
        weapon = playerHand.GetComponentInChildren<WeaponBehaviour>();
        upgradeCost = weapon.GetUpgradeCost();
        upgradeTime = 0;
        isUpgrading = false;
        isUpgraded = false;
        finishedUpgrade = false;
        playerPoints = playerHand.GetComponentInParent<PlayerPoints>();
        weaponUpgradeLevel = 0;
    }

    protected override void OnLook()
    {

        if (!isUpgrading)
        {
            weapon = playerHand.GetComponentInChildren<WeaponBehaviour>();
            playerPoints = playerHand.GetComponentInParent<PlayerPoints>();

            if (weapon != null && switchWeaponButton != null)
            {
                upgradeCost = weapon.GetUpgradeCost();
                switchWeaponButton.SetActive(false);
            }

            if ((interactButton != null) && weapon.GetCurrentUpgradeLevel() < weapon.GetMaxUpgrade())
            {

                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "UPGRADE";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                interactButton.GetComponent<Image>().enabled = true;
                interactButton.GetComponent<OnScreenButton>().enabled = true;
            }
        }
        else
        {
            if (!isUpgraded)
            {
                upgradeBar.SetActive(true);
            }
            else if(isUpgrading && !finishedUpgrade)
            {
                promptMessage = "Pick Up Weapon";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "GRAB";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                interactButton.GetComponent<Image>().enabled = true;
                interactButton.GetComponent<OnScreenButton>().enabled = true;
            }
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
            if (!isUpgraded)
            {
                upgradeBar.SetActive(false);
            }
            else
            {
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                interactButton.GetComponent<Image>().enabled = false;
                interactButton.GetComponent<OnScreenButton>().enabled = false;
            }
        }
    }



    protected override void Interact()
    {
        if (!isUpgrading)
        {
            if ((playerPoints.points >= upgradeCost) && weapon.GetCurrentUpgradeLevel() <= weapon.GetMaxUpgrade())
            {
                playerPoints.RemovePoints((int)upgradeCost);
                foreach (GameObject wep in weaponPrefabs)
                {
                    if (wep.name.Substring(0, 3) == weapon.name.Substring(0, 3))
                    {
                        workbenchWeapon = wep;
                    }
                }
                workbenchWeapon.SetActive(true);
                isUpgrading = true;
                promptMessage = "";
                upgradeBar.SetActive(true);
                upgradeBarCurrent.fillAmount = 0;
                weaponUpgradeLevel = weapon.GetCurrentUpgradeLevel() + 1;

                itemUpgradingIndex = playerPoints.GetComponent<ItemChange>().DropItem();

            }
            else if (weapon.GetCurrentUpgradeLevel() == weapon.GetMaxUpgrade())
            {
                promptMessage = "No More Upgrades";
            }
            else
            {
                Debug.Log("NO POINTS");
                promptMessage = "Insufficent Points";
            }
        }
        else
        {
            if (isUpgraded && !finishedUpgrade) {
                isUpgraded = false;
                finishedUpgrade = true;
                workbenchWeapon.SetActive(false);
                playerPoints.GetComponent<ItemChange>().ReturnItem(itemUpgradingIndex);

                weapon.SetUpgradeLevel(weaponUpgradeLevel);
                weapon.SetWeaponIsUpgrading();


            }
        }
    }


    public override void CancelInteract()
    {
        if (finishedUpgrade)
        {
            isUpgrading = false;
            finishedUpgrade = false;
            upgradeCost = weapon.GetUpgradeCost();
            promptMessage = "Upgrade Weapon (" + upgradeCost + ")";
            weaponUpgradeLevel = 0;

        }
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
        if (isUpgrading && !isUpgraded)
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

                isUpgraded = true;
                upgradeTime = 0;
                upgradeBar.SetActive(false);
                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "GRAB";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                interactButton.GetComponent<Image>().enabled = true;
                interactButton.GetComponent<OnScreenButton>().enabled = true;

            }
        }
    }
}
