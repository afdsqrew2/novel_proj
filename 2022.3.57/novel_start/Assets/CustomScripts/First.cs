using System;
using Naninovel;
using UnityEngine;

public class First : MonoBehaviour
{
    private void Awake()
    {

    }

    private void DoMyCustomWork()
    {
        Engine.OnInitializationFinished -=
            DoMyCustomWork;
        Debug.Log("My Custom Work");
        // Engine is initialized here, it's safe to use the APIs.
        var scriptPlayer =
            Engine.GetService<IScriptPlayer>();
        PlayFunc();
    }

    /*
    When exiting the novel mode and returning to the
        main game mode, you probably would like to unload
        all the resources currently used by Naninovel and
        stop all the engine services.F
        */

    async UniTask ResetService()
    {
        var stateManager =
            Engine.GetService<IStateManager>();
        await stateManager.ResetState();
    }

    async UniTask PlayFunc()
    {
        var scriptPlayer =
            Engine.GetService<IScriptPlayer>();
        await scriptPlayer.LoadAndPlay(
            "NovelScript/Script001");
    }
    /*
    Disabling Scene Independent option will make all
        the Naninovel-related objects part of the
        Unity scene where the engine was initialized;
    the engine will be destroyed when the scene is unloaded.

To reset the engine services (and dispose most of the occupied resources), use ResetState() method of IStateManager service;

To destroy all the engine services and completely remove Naninovel from memory, use Engine.Destroy() static method.
*/
    void OnGUI()
    {
        if (GUILayout.Button("init engine"))
        {
            CustomInit();
        }
        
        if (GUILayout.Button("ResetService"))
        {
            ResetService();
        }
        
        if (GUILayout.Button("CustomStart"))
        {
            CustomStart();
        }
    }
    
    private async UniTask CustomInit()
    {
        await RuntimeInitializer.Initialize();
        var manager = Engine.GetService<IUIManager>();
        if (manager.HasUI("TitleUI"))
        {
            Debug.Log("has title UI 2");
            manager.GetUI("TitleUI").ChangeVisibility(false);
        }

        // Engine may not be initialized here, so check first.
        if (Engine.Initialized) DoMyCustomWork();
        else
            Engine.OnInitializationFinished +=
                DoMyCustomWork;
    }
    
    private async UniTask CustomStart()
    {
        /*
        var manager = Engine.GetService<IUIManager>();
        if (manager.HasUI("TitleUI"))
        {
            Debug.Log("has title UI 1");
        }
        */
        
        // 1. Initialize Naninovel.
        //await RuntimeInitializer.InitializeAsync();
        await RuntimeInitializer.Initialize();
        var manager = Engine.GetService<IUIManager>();
        if (manager.HasUI("TitleUI"))
        {
            Debug.Log("has title UI 2");
            manager.GetUI("TitleUI").ChangeVisibility(false);
        }
        
        // 2. Enter adventure mode.
        var switchCommand = new SwitchToAdventureMode { ResetState = false };
        await switchCommand.ExecuteAsync();
    }
    
}