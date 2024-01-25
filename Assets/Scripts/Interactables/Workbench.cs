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

        private int weaponIndex;

        private bool isUpgraded;

        private bool finishedUpgrade;

        private int weaponUpgradeLevel;

        private bool insufficientPoints;

        protected override void Awake()
        {
            upgradeTimer = 0;
            isUpgrading = false;
            isUpgraded = false;
            finishedUpgrade = false;
            insufficientPoints = false;
            weaponUpgradeLevel = 0;
            SetShowPromptIcon(true);
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            if (upgradeCost != null)
            {
                SetPromptText("Upgrade Weapon (" + upgradeCost + ")");
            }

        }

        public override void OnLook(GameObject actor)
        {
            if (!actor.GetComponentInParent<Character>().IsHolstering())
            {
                if (!isUpgrading)
                {
                    SetShowPromptIcon(true);
                    weapon = actor.GetComponentInParent<CharacterController>().gameObject.GetComponentInChildren<Weapon>();


                    if (weapon != null && switchWeaponButton != null && !insufficientPoints)
                    {
                        upgradeCost = (weapon.GetUpgradeLevel() + 1) * 1000;
                        switchWeaponButton.SetActive(false);

                        if ((interactButton != null) && weapon.GetUpgradeLevel() < 5)
                        {
                            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "UPGRADE";
                            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                            interactButton.GetComponent<Image>().enabled = true;
                            interactButton.GetComponent<OnScreenButton>().enabled = true;
                            SetPromptText("Upgrade Weapon (" + upgradeCost + ")");
                        }
                        else if(weapon.GetUpgradeLevel() == 5)
                        {
                            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
                            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                            interactButton.GetComponent<Image>().enabled = false;
                            interactButton.GetComponent<OnScreenButton>().enabled = false;
                            SetPromptText("Max Upgrade Reached");

                        }
                    }else if (insufficientPoints)
                    {
                        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
                        interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                        interactButton.GetComponent<Image>().enabled = false;
                        interactButton.GetComponent<OnScreenButton>().enabled = false;
                        SetPromptText("Insufficent Points");

                    }
                }
                else if (isUpgrading && !finishedUpgrade)
                {
                    SetShowPromptIcon(false);
                    if (isUpgraded)
                    {
                        SetShowPromptIcon(true);
                        SetPromptText("Pick Up Weapon");
                        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "GRAB";
                        interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                        interactButton.GetComponent<Image>().enabled = true;
                        interactButton.GetComponent<OnScreenButton>().enabled = true;
                    }
                    else
                    {
                        upgradeBar.SetActive(true);
                    }
                }

                //  base.OnLook();
            }
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
                insufficientPoints = false;
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

                Inventory inventory = actor.GetComponentInParent<CharacterController>().gameObject.GetComponentInChildren<Inventory>();
                playerPoints = actor.GetComponentInParent<PlayerPoints>();

            if (!isUpgrading)
                {
                    if ((playerPoints.points >= upgradeCost) && weapon.GetUpgradeLevel() <= 5 && inventory.GetNextIndex() != inventory.GetLastIndex())
                    {
                        playerPoints.RemovePoints((int)upgradeCost);


                        weapon.gameObject.transform.SetParent(workbenchUpgradeSlot.transform);
                        weapon.gameObject.transform.localPosition = Vector3.zero;
                        weapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        weapon.gameObject.layer = workbenchUpgradeSlot.gameObject.layer;
                        SceneHelper.SetLayerAllChildren(weapon.gameObject.transform, workbenchUpgradeSlot.gameObject.layer);
                        weaponIndex = inventory.GetEquippedIndex();
                        actor.GetComponentInParent<Character>().SwitchWeaponManual(inventory.GetNextIndex());

                        isUpgrading = true;
                        SetPromptText("");
                    SetShowPromptIcon(false);

                    upgradeBarCurrent.fillAmount = 0;
                        weaponUpgradeLevel = weapon.GetUpgradeLevel() + 1;

                    }
                    else if (inventory.GetNextIndex() == inventory.GetLastIndex()) {
                        SetPromptText("Secondary Weapon Required");
                    }
                    else
                    {
                        Debug.Log("NO POINTS");
                        insufficientPoints = true;
                    }
                }
                else
                {
                    if (isUpgraded && !finishedUpgrade)
                    {
                        isUpgraded = false;
                        finishedUpgrade = true;

                        weapon.gameObject.SetActive(false);
                        weapon.gameObject.transform.SetParent(inventory.gameObject.transform);
                        weapon.gameObject.transform.localPosition = Vector3.zero;
                        weapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        weapon.gameObject.layer = actor.GetComponentInParent<CharacterController>().gameObject.layer;
                        SceneHelper.SetLayerAllChildren(weapon.gameObject.transform, weapon.gameObject.layer);



                        MagazineBehaviour magazine = weapon.GetAttachmentManager().GetEquippedMagazine();

                        if (weapon.name.Contains("AR") || weapon.name.Contains("SMG"))
                        {
                            weapon.SetRateOfFire(Mathf.RoundToInt(weapon.GetRateOfFire() * 1.25f));
                            magazine.SetAmmunitionTotal(magazine.GetAmmunitionTotal() + 5);
                        }
                        else if (weapon.name.Contains("RL") || weapon.name.Contains("GL"))
                        {
                            //magazine.SetAmmunitionTotal(magazine.GetAmmunitionTotal() + 5);
                            //make ammunition TOTAL go up, clip must remain 1
                        }
                        else if (weapon.name.Contains("Sniper"))
                        {
                            magazine.SetAmmunitionTotal(magazine.GetAmmunitionTotal() + 1);
                        }
                        else if (weapon.name.Contains("Shotgun"))
                        {
                            magazine.SetAmmunitionTotal(magazine.GetAmmunitionTotal() + 3);
                            weapon.SetShotCount(weapon.GetShotCount() + 1);
                        }
                        else
                        {
                            magazine.SetAmmunitionTotal(magazine.GetAmmunitionTotal() + 5);
                        }


                        weapon.SetMultiplierMovementSpeed(weapon.GetMultiplierMovementSpeed() * 1.1f);
                        weapon.SetDamage(Mathf.RoundToInt(weapon.GetDamage() + ((weaponUpgradeLevel + 1) * (weaponUpgradeLevel + 1) * 0.5f)));
                        weapon.SetSpread(weapon.GetSpread() * 0.85f);
                        weapon.SetUpgradeLevel(weaponUpgradeLevel);
                        weapon.transform.SetSiblingIndex(weaponIndex);
                        actor.GetComponentInParent<Character>().SwitchWeaponManual(inventory.GetLastIndex(), false);

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
                //SetPromptText("Upgrade Weapon (" + upgradeCost + ")");
                weaponUpgradeLevel = 0;

            }

        }




        // Update is called once per frame
        protected override void Update()
        {
            if (isUpgrading && !isUpgraded)
            {
                upgradeTimer += Time.deltaTime;
                //float percentComplete = upgradeTimer / weapon.GetUpgradeTime();
                float percentComplete = upgradeTimer / 5f;

                if (upgradeBar.activeInHierarchy)
                {
                    upgradeBarCurrent.fillAmount = percentComplete;
                }
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                interactButton.GetComponent<Image>().enabled = false;
                interactButton.GetComponent<OnScreenButton>().enabled = false;

                // if (upgradeTimer >= weapon.GetUpgradeTime())
                if (upgradeTimer >= 5f)
                {

                    isUpgraded = true;
                    upgradeTimer = 0;
                    upgradeBar.SetActive(false);

                }
            }
        }



    }
}
