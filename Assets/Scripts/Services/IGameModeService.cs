// Copyright 2021, Infima Games. All Rights Reserved.


using UnityEngine;
/// <summary>
/// Game Mode Service.
/// </summary>
public interface IGameModeService : IGameService
    {
        /// <summary>
        /// Returns the Player Character.
        /// </summary>
        InputManager GetPlayerCharacter();

        GameObject GetInteractButton();
    }