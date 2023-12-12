using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{

    [SerializeField]
    public GameObject hand;

    [SerializeField]
    public GameObject reloadButton;

    [SerializeField]
    public TextMeshProUGUI ammoText;

    private int currentClipAmmo;
    private int currentTotalAmmo;
    private GunBehaviour weapon;
    private GunBehaviour gun;

    private string lastWeapon;
    // Start is called before the first frame update
    void Start()
    {
        weapon = hand.GetComponentInChildren<GunBehaviour>();
        //Debug.Log("WEAPON: " + weapon.name);
        //lastWeapon = weapon.name;
        //if (weapon.GetWeaponType() != WeaponBehaviour.WeaponType.Melee)
        //{
        //    gun = hand.GetComponent<GunBehaviour>();
        //    //currentClipAmmo = gun.GetAmmunitionClip();
        //}
        //else
        //{
        //    gun = null;
        //}
    }

    // Update is called once per frame
    void Update()
    {

        if (weapon != null)
        {
            if (weapon.name != hand.GetComponentInChildren<GunBehaviour>().name)
            {
                Debug.Log("WEAPON: " + weapon.name);

                weapon = hand.GetComponentInChildren<GunBehaviour>();
                //if (weapon.GetWeaponType() != WeaponBehaviour.WeaponType.Melee)
                //{
                //    gun = hand.GetComponent<GunBehaviour>();
                //}
                //else
                //{
                //    gun = null;
                //}
            }
            //if (gun != null)
            //{
            Debug.Log("WEAPON: " + weapon.name);


            ammoText.text = "AMMO: " + GetWeaponAmmo();


                Debug.Log("Current Ammo: " + weapon.GetAmmunitionClip());
                if (weapon.GetAmmunitionClip() == 0)
                {
                    if (weapon.HasAmmunition() && !weapon.IsReloading())
                    {
                        reloadButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                        reloadButton.GetComponent<Image>().enabled = true;
                        reloadButton.GetComponent<OnScreenButton>().enabled = true;
                    }
                    else if (weapon.IsReloading())
                    {
                        reloadButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                        reloadButton.GetComponent<Image>().enabled = false;
                        reloadButton.GetComponent<OnScreenButton>().enabled = false;
                    }
                    if (!weapon.HasAmmunition())
                    {
                        ammoText.color = Color.red;
                    }
                    else
                    {
                        ammoText.color = Color.black;
                    }
                }
            }
            else
            {
            weapon = hand.GetComponentInChildren<GunBehaviour>();

            ammoText.text = "";
            }
        //}
        //else
        //{
        //    ammoText.text = "";
        //}

    }


    private string GetWeaponAmmo()
    {
        return weapon.GetAmmunitionClip() + "/" + weapon.GetAmmunitionCurrent();
    }
}
