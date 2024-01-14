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

        //private PlayerPoints playerPoints;

        //private ItemChange playerItems;

        //[SerializeField]
        //private GameObject playerHand;

        [SerializeField]
        private float weaponGenerationTime;

        [SerializeField]
        private int weaponCost;

        [SerializeField]
        private GameObject[] weaponPrefabs;

        private GameObject weaponGeneratedPrefab;

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
           // playerPoints = playerHand.GetComponentInParent<PlayerPoints>();
            //playerItems = playerHand.GetComponentInParent<ItemChange>();
            timeGenerating = 0;
            //weaponList = playerItems.GetWeaponList();
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

        //private void FindGeneratedWeapon()
        //{
        //    foreach (GameObject wep in weaponPrefabs)
        //    {
        //        if (wep.name.Substring(0, 3) == weaponList[generatedItemIndex].name.Substring(0, 3))
        //        {
        //            weaponGeneratedPrefab = wep;
        //        }
        //    }

        //}

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
                    //FindGeneratedWeapon();


                    TurnOnInteractButton("GRAB");
                    SetPromptText("Pick up Weapon");
                    animator.SetBool("ChestOpen", true);
                    //Invoke("SetWeaponPrefabActive", boxDelay);

                }
                else if (insufficientPoints)
                {
                    SetPromptText("Insufficient points");
                }
            }
            else
            {
                SetPromptText("Weapon Grabbed");
            }

        }

        public override void Interact(GameObject actor)
        {

            //if (playerPoints.points < weaponCost && !isGeneratingWeapon && !weaponGenerationComplete)
            //{
            //    Debug.Log("INTERACT PLAYERPOINTS < COST == TRUE");
            //    insufficientPoints = true;
            //}

            if (!insufficientPoints)
            {

                if (!weaponGenerationComplete && !isGeneratingWeapon)
                {
                    //playerPoints.RemovePoints(weaponCost);
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
                        Instantiate(weaponGeneratedPrefab, inventory.gameObject.transform, false );
                        //weaponGeneratedPrefab.gameObject.transform.SetParent(inventory.gameObject.transform);
                        //weaponGeneratedPrefab.SetActive(false);
                        int newIndex = inventory.GetComponentsInChildren<WeaponBehaviour>(true).Length;
                        Debug.Log(newIndex+" ...SIBLING INDEX BEFORE: " + weaponGeneratedPrefab.gameObject.transform.GetSiblingIndex());
                        weaponGeneratedPrefab.transform.SetSiblingIndex(newIndex);
                        //weaponGeneratedPrefab.gameObject.transform.localPosition = Vector3.zero;
                        //weaponGeneratedPrefab.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        weaponGeneratedPrefab.gameObject.layer = actor.GetComponentInParent<CharacterController>().gameObject.layer;
                        SceneHelper.SetLayerAllChildren(weaponGeneratedPrefab.gameObject.transform, actor.GetComponentInParent<CharacterController>().gameObject.layer);
                        Debug.Log("SIBLING INDEX AFTER: " + weaponGeneratedPrefab.gameObject.transform.GetSiblingIndex());


                         actor.GetComponentInParent<Character>().SwitchWeaponManual(inventory.GetLastIndex(), true);

                        //playerItems.ReturnItem(generatedItemIndex);
                        TurnOffInteractButton();
                        weaponGrabbed = true;
                        isGeneratingWeapon = false;
                        weaponGenerationComplete = false;
                        animator.SetBool("ChestOpen", false);
                        //Invoke("SetWeaponPrefabInactive", 1f);
                      //  weaponGeneratedPrefab.GetComponent<Animator>().SetTrigger("WeaponHide");



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
                    animator.SetBool("ChestOpen", false);
                    //Invoke("SetWeaponPrefabInactive", 1f);
                    weaponGeneratedPrefab.GetComponent<Animator>().SetTrigger("WeaponHide");


                }
            }

            if (weaponGrabbed)
            {
                weaponGrabbed = false;
            }
        }
    }
}
