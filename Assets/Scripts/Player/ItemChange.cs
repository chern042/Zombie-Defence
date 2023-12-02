using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EvolveGames
{
    public class ItemChange : MonoBehaviour
    {
        [Header("Item Change")]
        [SerializeField] Image ItemCanvasLogo;
        [SerializeField, Tooltip("You can add your new item here.")] GameObject[] Items;
        [SerializeField, Tooltip("These logos must have the same order as the items.")] Sprite[] ItemLogos;
        [SerializeField] public int ItemIdInt;
        [SerializeField] private AudioSource holsteringSounds;

        private Animator ani;
        private WeaponBehaviour weapon;
        int MaxItems;
        int ChangeItemInt;
        [HideInInspector] public bool DefiniteHide;
        bool ItemChangeLogo;
        private bool canSwitch;

        private void Start()
        {
            canSwitch = true;
            Color OpacityColor = ItemCanvasLogo.color;
            OpacityColor.a = 0;
            ItemCanvasLogo.color = OpacityColor;
            ItemChangeLogo = false;
            DefiniteHide = false;
            ChangeItemInt = ItemIdInt;
            ItemCanvasLogo.sprite = ItemLogos[ItemIdInt];
            MaxItems = Items.Length - 1;
            weapon = GetComponentInChildren<WeaponBehaviour>();
            if (ani == null && weapon.GetComponentInChildren<Animator>()) ani = weapon.GetComponentInChildren<Animator>();
            StartCoroutine(ItemChangeObject());
        }
        private void Update()
        {

            if (ItemIdInt < 0) ItemIdInt = MaxItems;
            if (ItemIdInt > MaxItems) ItemIdInt = 0;


            if (ItemIdInt != ChangeItemInt)
            {
                ChangeItemInt = ItemIdInt;

                StartCoroutine(ItemChangeObject());
            }
        }

        public void ChangeItem()
        {
            if (canSwitch)
            {
                ItemIdInt++;
                canSwitch = false;
            }
        }
        private void HolsteringSounds(bool holster)
        {
            AudioClip holsterClip;
            AudioClip unholsterClip;
            if(weapon!= null && weapon.name == Items[ItemIdInt].name)
            {
                holsterClip = weapon.GetAudioClipHolster();
                unholsterClip = weapon.GetAudioClipUnholster();
                holsteringSounds.clip = holster ? holsterClip : unholsterClip;
                holsteringSounds.Play();

            }
        }


        IEnumerator ItemChangeObject()
        {
            if (!DefiniteHide && ani != null) ani.SetTrigger("Hide");
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < (MaxItems + 1); i++)
            {
                Items[i].SetActive(false);
                HolsteringSounds(false);
            }
            Items[ItemIdInt].SetActive(true);
            weapon = Items[ItemIdInt].GetComponent<WeaponBehaviour>();
            ani = weapon.GetComponent<Animator>();


            HolsteringSounds(true);
            canSwitch = true;
            if (!ItemChangeLogo) StartCoroutine(ItemLogoChange());
        }

        IEnumerator ItemLogoChange()
        {
            ItemChangeLogo = true;
            yield return new WaitForSeconds(0.5f);
            ItemCanvasLogo.sprite = ItemLogos[ItemIdInt];
            yield return new WaitForSeconds(0.1f);
            ItemChangeLogo = false;
        }

        private void FixedUpdate()
        {
            
            if (ItemChangeLogo)
            {
                Color OpacityColor = ItemCanvasLogo.color;
                OpacityColor.a = Mathf.Lerp(OpacityColor.a, 0, 20 * Time.deltaTime);
                ItemCanvasLogo.color = OpacityColor;
            }
            else
            {
                Color OpacityColor = ItemCanvasLogo.color;
                OpacityColor.a = Mathf.Lerp(OpacityColor.a, 1, 6 * Time.deltaTime);
                ItemCanvasLogo.color = OpacityColor;
            }
        }

        public  WeaponBehaviour GetActiveWeapon() => weapon;


    }

}
