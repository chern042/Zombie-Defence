using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EvolveGames
{
    public class ItemChange : MonoBehaviour
    {
        [Header("Item Change")]
        [SerializeField] public Animator ani;
        [SerializeField] Image ItemCanvasLogo;
        [SerializeField, Tooltip("You can add your new item here.")] GameObject[] Items;
        [SerializeField, Tooltip("These logos must have the same order as the items.")] Sprite[] ItemLogos;
        [SerializeField] public int ItemIdInt;
        [SerializeField] private AudioSource holsteringSounds;
        
        private WeaponBehaviour weapon;
        int MaxItems;
        int ChangeItemInt;
        [HideInInspector] public bool DefiniteHide;
        bool ItemChangeLogo;

        private void Start()
        {
            if (ani == null && GetComponent<Animator>()) ani = GetComponent<Animator>();
            Color OpacityColor = ItemCanvasLogo.color;
            OpacityColor.a = 0;
            ItemCanvasLogo.color = OpacityColor;
            ItemChangeLogo = false;
            DefiniteHide = false;
            ChangeItemInt = ItemIdInt;
            ItemCanvasLogo.sprite = ItemLogos[ItemIdInt];
            MaxItems = Items.Length - 1;
            weapon = GetComponentInChildren<WeaponBehaviour>();
            Debug.Log("Weapon behaviour test: " + weapon.name + " " + weapon.isActiveAndEnabled);
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
            ItemIdInt++;

        }
        private void HolsteringSounds(bool holster)
        {
            AudioClip holsterClip;
            AudioClip unholsterClip;
            if(weapon!= null && weapon.name == Items[ItemIdInt].name)
            {
                holsterClip = weapon.GetAudioClipHolster();
                unholsterClip = weapon.GetAudioClipUnholster();
                Debug.Log("Test: " + holsterClip.name + " " + unholsterClip.name);
                holsteringSounds.clip = holster ? holsterClip : unholsterClip;
                holsteringSounds.Play();

            }
        }


        IEnumerator ItemChangeObject()
        {
            if(!DefiniteHide) ani.SetBool("Hide", true);
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < (MaxItems + 1); i++)
            {
                Items[i].SetActive(false);
                HolsteringSounds(false);
            }
            Items[ItemIdInt].SetActive(true);
            HolsteringSounds(true);
            if (!ItemChangeLogo) StartCoroutine(ItemLogoChange());

            if (!DefiniteHide) ani.SetBool("Hide", false);
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

    }

}
