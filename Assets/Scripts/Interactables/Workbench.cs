using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
namespace InfimaGames.LowPolyShooterPack
{
    public class Workbench : Interactable
    {

        //[SerializeField]
        //private GameObject playerHand;

        [SerializeField]
        private GameObject switchWeaponButton;

        [SerializeField]
        private GameObject upgradeBar;

        [SerializeField]
        private Image upgradeBarCurrent;

        private Weapon weapon;

        [SerializeField]
        private GameObject workbenchUpgradeSlot;

        private int? upgradeCost;

        private float upgradeTimer;

        private bool isUpgrading;

        private PlayerPoints playerPoints;

        private int itemUpgradingIndex;

        private bool isUpgraded;

        private bool finishedUpgrade;

        private int weaponUpgradeLevel;

        protected override void Awake()
        {
            //weapon = playerHand.GetComponentInChildren<WeaponBehaviour>();
            //upgradeCost = weapon.GetUpgradeCost();
            upgradeTimer = 0;
            isUpgrading = false;
            isUpgraded = false;
            finishedUpgrade = false;
            //playerPoints = playerHand.GetComponentInParent<PlayerPoints>();
            weaponUpgradeLevel = 0;
        }

        public override void OnLook(GameObject actor)
        {

            if (!isUpgrading)
            {
                //weapon = playerHand.GetComponentInChildren<WeaponBehaviour>();
                weapon = actor.GetComponentInParent<CharacterController>().gameObject.GetComponentInChildren<Weapon>();
                //playerPoints = playerHand.GetComponentInParent<PlayerPoints>();
                //Debug.Log("PLAYER****: " + actor.GetComponentInParent<CharacterController>().gameObject.name);
                //Debug.Log("WEAPON****: " + actor.GetComponentInParent<CharacterController>().gameObject.GetComponentInChildren<Weapon>().name);

                if (weapon != null && switchWeaponButton != null)
                {
                    //upgradeCost = weapon.GetUpgradeCost();
                    upgradeCost = (weapon.GetUpgradeLevel()+1) * 1000;
                    switchWeaponButton.SetActive(false);
                }
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                Debug.Log("REACHING INTERACT BUTTON SETTING");
                if ((interactButton != null) && weapon.GetUpgradeLevel() < 5)
                {
                    Debug.Log("REACHED INTERACT BUTTON SETTING");

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
                else if (isUpgrading && !finishedUpgrade)
                {
                    SetPromptText("Pick Up Weapon");
                    interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "GRAB";
                    interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                    interactButton.GetComponent<Image>().enabled = true;
                    interactButton.GetComponent<OnScreenButton>().enabled = true;
                }
            }

          //  base.OnLook();

        }

        public override void OnLookOff()
        {
            //base.OnLookOff();

            if (switchWeaponButton != null)
            {
                switchWeaponButton.SetActive(true);

            }
            if (!isUpgrading)
            {
                SetPromptText("Upgrade Weapon (" + upgradeCost + ")");
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



        public override void Interact(GameObject actor)
        {
            if (!isUpgrading)
            {
                //if ((playerPoints.points >= upgradeCost) && weapon.GetUpgradeLevel() <= 5)
                if ((10000 >= upgradeCost) && weapon.GetUpgradeLevel() <= 5)
                {
                    //playerPoints.RemovePoints((int)upgradeCost);
                    //foreach (GameObject wep in weaponPrefabs)
                    //{
                    //    if (wep.name.Substring(0, 3) == weapon.name.Substring(0, 3))
                    //    {
                    //        workbenchWeapon = wep;
                    //    }
                    //}
                    weapon.gameObject.transform.SetParent(workbenchUpgradeSlot.transform);
                    weapon.gameObject.transform.localPosition = Vector3.zero;
                    weapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                   // workbenchUpgradeSlot = weapon.gameObject;
                   // workbenchUpgradeSlot.SetActive(true);

                    isUpgrading = true;
                    SetPromptText("");
                    upgradeBar.SetActive(true);
                    upgradeBarCurrent.fillAmount = 0;
                    weaponUpgradeLevel = weapon.GetUpgradeLevel() + 1;

                    //itemUpgradingIndex = playerPoints.GetComponent<ItemChange>().DropItem();

                }
                else if (weapon.GetUpgradeLevel() == 5)
                {
                    SetPromptText("No More Upgrades");
                }
                else
                {
                    Debug.Log("NO POINTS");
                    SetPromptText("Insufficent Points");
                }
            }
            else
            {
                if (isUpgraded && !finishedUpgrade)
                {
                    isUpgraded = false;
                    finishedUpgrade = true;

                    weapon.gameObject.transform.SetParent(actor.GetComponentInParent<CharacterController>().gameObject.GetComponentInChildren<Inventory>().gameObject.transform);
                    weapon.gameObject.transform.localPosition = Vector3.zero;
                    weapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    //workbenchUpgradeSlot.SetActive(false);

                    // playerPoints.GetComponent<ItemChange>().ReturnItem(itemUpgradingIndex);

                    weapon.SetUpgradeLevel(weaponUpgradeLevel);
                    //weapon.SetWeaponIsUpgrading();


                }
            }
        }


        public override void CancelInteract()
        {
            if (finishedUpgrade)
            {
                isUpgrading = false;
                finishedUpgrade = false;
                upgradeCost = (weapon.GetUpgradeLevel()+1) * 1000;
                SetPromptText("Upgrade Weapon (" + upgradeCost + ")");
                weaponUpgradeLevel = 0;

            }
            //promptMessage = "Upgrade Weapon (" + upgradeCost + ")";

        }
        // Start is called before the first frame update
        protected override void Start()
        {
            if (upgradeCost != null)
            {
                if (weapon.GetUpgradeLevel() < 5)
                {
                    SetPromptText("Upgrade Weapon (" + upgradeCost + ")");
                }
                else
                {
                    SetPromptText("No More Upgrades");
                }
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (isUpgrading && !isUpgraded)
            {
                upgradeTimer += Time.deltaTime;
                //float percentComplete = upgradeTimer / weapon.GetUpgradeTime();
                float percentComplete = upgradeTimer / 20f;

                if (upgradeBar.activeInHierarchy)
                {
                    upgradeBarCurrent.fillAmount = percentComplete;
                }
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                interactButton.GetComponent<Image>().enabled = false;
                interactButton.GetComponent<OnScreenButton>().enabled = false;

                // if (upgradeTimer >= weapon.GetUpgradeTime())
                if (upgradeTimer >= 20f)
                {

                    isUpgraded = true;
                    upgradeTimer = 0;
                    upgradeBar.SetActive(false);
                    interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "GRAB";
                    interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                    interactButton.GetComponent<Image>().enabled = true;
                    interactButton.GetComponent<OnScreenButton>().enabled = true;

                }
            }
        }
    }
}
