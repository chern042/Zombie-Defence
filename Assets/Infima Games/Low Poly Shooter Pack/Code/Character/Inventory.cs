﻿//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class Inventory : InventoryBehaviour
    {
        #region FIELDS

        [SerializeField]
        private int carryingCapacity = 2;

        /// <summary>
        /// Array of all weapons. These are gotten in the order that they are parented to this object.
        /// </summary>
        private WeaponBehaviour[] weapons;
        
        /// <summary>
        /// Currently equipped WeaponBehaviour.
        /// </summary>
        private WeaponBehaviour equipped;
        /// <summary>
        /// Currently equipped index.
        /// </summary>
        private int equippedIndex = -1;

        private bool isEquippingFull;

        private GameObject weaponToDestroy;

        #endregion
        
        #region METHODS
        
        public override void Init(int equippedAtStart = 0)
        {
            //Cache all weapons. Beware that weapons need to be parented to the object this component is on!
            weapons = GetComponentsInChildren<WeaponBehaviour>(true);
            isEquippingFull = false;
            
            //Disable all weapons. This makes it easier for us to only activate the one we need.
            foreach (WeaponBehaviour weapon in weapons)
                weapon.gameObject.SetActive(false);

            //Equip.
            Equip(equippedAtStart);
        }

        public override WeaponBehaviour Equip(int index, bool manual=false)
        {
            //If we have no weapons, we can't really equip anything.
            if (weapons == null)
                return equipped;
            
            //The index needs to be within the array's bounds.
            if (index > weapons.Length - 1)
                return equipped;

            //No point in allowing equipping the already-equipped weapon.
            if (equippedIndex == index)
                return equipped;
            
            //Disable the currently equipped weapon, if we have one.
            if (equipped != null && !manual)
                equipped.gameObject.SetActive(false);

            if (GetInventoryIsFull())
            {
                isEquippingFull = true;
                weaponToDestroy = equipped.gameObject;
            }

            //Update index.
            equippedIndex = index;
            //Update equipped.
            equipped = weapons[equippedIndex];
            //Activate the newly-equipped weapon.
            equipped.gameObject.SetActive(true);

            //Return.
            return equipped;
        }
        
        #endregion

        #region Getters

        public override int GetLastIndex()
        {
            //Get last index with wrap around.
            int newIndex = equippedIndex - 1;
            if (newIndex < 0)
                newIndex = weapons.Length - 1;

            //Return.
            return newIndex;
        }

        public override int GetNextIndex()
        {
            //Get next index with wrap around.
            int newIndex = equippedIndex + 1;
            if (newIndex > weapons.Length - 1)
                newIndex = 0;

            //Return.
            return newIndex;
        }

        public int GetInventorySize()
        {
            weapons = GetComponentsInChildren<WeaponBehaviour>(true);
            return weapons.Length;
        }

        public override WeaponBehaviour GetEquipped() => equipped;
        public override int GetEquippedIndex() => equippedIndex;
        public bool GetInventoryIsFull() => GetInventorySize() > carryingCapacity;

        #endregion

        private void Update()
        {
            if (isEquippingFull)
            {
                if (!equipped.GetComponentInParent<Character>().IsHolstering())
                {
                    Debug.Log("DesTROYING WEAPON");
                    isEquippingFull = false;
                    Destroy(weaponToDestroy);
                }
            }
        }
    }
}