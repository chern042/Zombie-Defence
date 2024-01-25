﻿//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Magazine.
    /// </summary>
    public class Magazine : MagazineBehaviour
    {
        #region FIELDS SERIALIZED

        [Title(label: "Settings")]
        
        [Tooltip("Total Ammunition. (Magazine/Clip Size)")]
        [SerializeField]
        private int ammunitionTotal = 10;

        [Title(label: "Interface")]

        [Tooltip("Interface Sprite.")]
        [SerializeField]
        private Sprite sprite;

        #endregion

        #region GETTERS

        /// <summary>
        /// Ammunition Total.
        /// </summary>
        public override int GetMagazineSize() => ammunitionTotal;
        /// <summary>
        /// Sprite.
        /// </summary>
        public override Sprite GetSprite() => sprite;

        #endregion

        #region SETTERS

        public override void SetMagazineSize(int newTotal) => ammunitionTotal = newTotal;


        #endregion
    }
}