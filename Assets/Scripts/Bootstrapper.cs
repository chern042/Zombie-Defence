// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;


    /// <summary>
    /// Bootstraper.
    /// </summary>
    public static class Bootstraper
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            //Initialize default service locator.
            ServiceLocator.Initialize();

            //Game Mode Service.
            ServiceLocator.Current.Register<IGameModeService>(new GameModeService());


        Debug.Log("Initialized.");

        }
    }
