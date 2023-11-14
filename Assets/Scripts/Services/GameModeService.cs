// Copyright 2021, Infima Games. All Rights Reserved.


    /// <summary>
    /// Game Mode Service.
    /// </summary>
    public class GameModeService : IGameModeService
    {
        #region FIELDS
        
        /// <summary>
        /// The Player Character.
        /// </summary>
        private InputManager playerCharacter;
        
        #endregion
        
        #region FUNCTIONS
        
        public InputManager GetPlayerCharacter()
        {
            //Make sure we have a player character that is good to go!
            if (playerCharacter == null)
                playerCharacter = UnityEngine.Object.FindObjectOfType<InputManager>();
            
            //Return.
            return playerCharacter;
        }
        
        #endregion
    }
