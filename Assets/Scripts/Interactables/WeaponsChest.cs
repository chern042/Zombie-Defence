using EvolveGames;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
namespace InfimaGames.LowPolyShooterPack
{
    public class WeaponsChest : Interactable
    {
        private Animator animator;

        private PlayerPoints playerPoints;

        [SerializeField]
        private float weaponGenerationTime;

        [SerializeField]
        private int weaponCost;

        [SerializeField]
        private GameObject[] weaponPrefabs;

        [SerializeField]
        private Transform weaponChestSlot;

        private GameObject weaponGeneratedPrefab;

        private GameObject instantiatedWeapon;

        private bool isGeneratingWeapon;

        private bool weaponGenerationComplete;

        private bool insufficientPoints;

        private bool weaponGrabbed;

        private float timeGenerating;

        private int generatedItemIndex;

        //private GameObject[] weaponList;

        private float boxDelay;


        // Start is called before the first frame update
        protected override void Start()
        {
            animator = GetComponent<Animator>();
            //playerItems = playerHand.GetComponentInParent<ItemChange>();
            timeGenerating = 0;
            isGeneratingWeapon = false;
            weaponGenerationComplete = false;
            insufficientPoints = false;
            weaponGrabbed = false;
            SetShowPromptIcon(true);
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (isGeneratingWeapon && !weaponGenerationComplete)
            {
                timeGenerating += Time.deltaTime;
                SetPromptText("Generating weapon: (" + Mathf.RoundToInt((timeGenerating / weaponGenerationTime) * 100f) + "/100)");
                if (timeGenerating >= weaponGenerationTime)
                {
                    //generatedItemIndex = Random.Range(0, weaponList.Length);
                    generatedItemIndex = Random.Range(0, weaponPrefabs.Length);
                    timeGenerating = 0;
                    weaponGenerationComplete = true;
                    animator.SetBool("WeaponIsGenerating", false);
                    weaponGeneratedPrefab = weaponPrefabs[generatedItemIndex];
                    instantiatedWeapon = Instantiate(weaponGeneratedPrefab, weaponChestSlot, false);
                    instantiatedWeapon.gameObject.layer = weaponChestSlot.gameObject.layer;
                    SceneHelper.SetLayerAllChildren(instantiatedWeapon.gameObject.transform, weaponChestSlot.gameObject.layer);
                    instantiatedWeapon.SetActive(true);


                }
            }
        }

        private void TurnOffInteractButton()
        {
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            interactButton.GetComponent<Image>().enabled = false;
            interactButton.GetComponent<OnScreenButton>().enabled = false;
        }

        private void TurnOnInteractButton(string text)
        {
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            interactButton.GetComponent<Image>().enabled = true;
            interactButton.GetComponent<OnScreenButton>().enabled = true;
        }


        public override void OnLook(GameObject actor)
        {

            if (!weaponGrabbed)
            {
                if (!isGeneratingWeapon && !weaponGenerationComplete && !insufficientPoints)
                {
                    TurnOnInteractButton("BUY");
                    SetPromptText("Purchase a weapon (" + weaponCost + ")");
                    animator.SetBool("ChestOpen", true);
                }

                if (isGeneratingWeapon && !weaponGenerationComplete)
                {
                    TurnOffInteractButton();
                }
                else if (isGeneratingWeapon && weaponGenerationComplete)
                {


                    TurnOnInteractButton("GRAB");
                    SetPromptText("Pick up Weapon");
                    animator.SetBool("ChestOpen", true);
                    animator.SetBool("WeaponGenerated", true);

                }
                else if (insufficientPoints)
                {
                    SetPromptText("Insufficient Points");
                }
            }
            else
            {
                SetPromptText("Weapon Grabbed");
            }

        }

        public override void Interact(GameObject actor)
        {
            playerPoints = actor.GetComponentInParent<PlayerPoints>();

            if (playerPoints.points < weaponCost && !isGeneratingWeapon && !weaponGenerationComplete)
            {
                Debug.Log("INTERACT PLAYERPOINTS < COST == TRUE");
                insufficientPoints = true;
            }

            if (!insufficientPoints)
            {

                if (!weaponGenerationComplete && !isGeneratingWeapon)
                {
                    playerPoints.RemovePoints(weaponCost);
                    Debug.Log("INTERACT WEAPON GENERATING");
                    isGeneratingWeapon = true;
                    animator.SetBool("WeaponIsGenerating", true);
                    boxDelay = 2.5f;
                }
                else if (isGeneratingWeapon)
                {
                    if (weaponGenerationComplete)
                    {
                        Inventory inventory = actor.GetComponentInParent<CharacterController>().gameObject.GetComponentInChildren<Inventory>();

                        instantiatedWeapon.transform.SetParent(inventory.gameObject.transform);
                        instantiatedWeapon.gameObject.transform.localPosition = Vector3.zero;
                        instantiatedWeapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        instantiatedWeapon.gameObject.transform.localScale = Vector3.one;

                        instantiatedWeapon.gameObject.layer = actor.GetComponentInParent<CharacterController>().gameObject.layer;
                        SceneHelper.SetLayerAllChildren(instantiatedWeapon.gameObject.transform, actor.GetComponentInParent<CharacterController>().gameObject.layer);


                        instantiatedWeapon.transform.SetSiblingIndex(inventory.GetEquippedIndex()+1);
                        instantiatedWeapon.SetActive(false);
                        inventory.GetInventoryIsFull();
                        actor.GetComponentInParent<Character>().SwitchWeaponManual(inventory.GetNextIndex(), false);





                        TurnOffInteractButton();
                        weaponGrabbed = true;
                        isGeneratingWeapon = false;
                        weaponGenerationComplete = false;
                        animator.SetBool("WeaponGenerated", false);
                        animator.SetBool("ChestOpen", false);



                    }
                }
            }

        }


        public override void CancelInteract()
        {
            Debug.Log("CANCEL INTERACT");

        }

        public override void OnLookOff()
        {

            if (insufficientPoints)
            {
                insufficientPoints = false;
                SetPromptText("Purchase a weapon (" + weaponCost + ")");
                animator.SetBool("ChestOpen", false);
                TurnOffInteractButton();
            }
            else if (!isGeneratingWeapon)
            {
                insufficientPoints = false;
                SetPromptText("Purchase a weapon (" + weaponCost + ")");
                animator.SetBool("ChestOpen", false);
                TurnOffInteractButton();
            }
            else
            {
                if (weaponGenerationComplete && !weaponGrabbed)
                {
                    SetPromptText("Weapon Generated");
                    TurnOffInteractButton();
                    boxDelay = 0.5f;
                    animator.SetBool("WeaponGenerated", false);
                    animator.SetBool("ChestOpen", false);

                }
            }

            if (weaponGrabbed)
            {
                weaponGrabbed = false;
            }
        }
    }
}
