//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Interactable.
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        //TODO
        [SerializeField]
        protected string promptText;

        [SerializeField]
        protected GameObject interactButton;

        [SerializeField]
        protected Sprite promptIcon;

        #endregion

        private bool showIcon;

        #region UNITY

        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake(){}

        /// <summary>
        /// Start.
        /// </summary>
        protected virtual void Start(){}

        /// <summary>
        /// Update.
        /// </summary>
        protected virtual void Update(){}

        /// <summary>
        /// Fixed Update.
        /// </summary>
        protected virtual void FixedUpdate(){}

        /// <summary>
        /// Late Update.
        /// </summary>
        protected virtual void LateUpdate(){}

        #endregion
        
        #region METHODS
        
        /// <summary>
        /// Called to interact with this object.
        /// </summary>
        /// <param name="actor">The actor starting the interaction.</param>
        public abstract void Interact(GameObject actor = null);

        public virtual void InteractHold(GameObject actor = null)
        {
        }

        public virtual void CancelInteract()
        {
        }

        public virtual void OnLook(GameObject actor = null)
        {
        }

        public virtual void OnLookOff()
        {
        }
        
        #endregion

        #region GETTERS

        //TODO
        public virtual string GetPromptText() => promptText;
        public virtual Sprite GetPromptIcon() => promptIcon;
        public virtual bool GetShowPromptIcon() => showIcon;


        #endregion

        #region SETTERS

        public virtual string SetPromptText(string text) => promptText = text;

        public virtual bool SetShowPromptIcon(bool show) => showIcon = show;

        #endregion
    }
}