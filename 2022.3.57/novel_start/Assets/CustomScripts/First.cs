using System;
using Naninovel;
using UnityEngine;

public class First : MonoBehaviour
{
    private void Awake()
    {
        // Engine may not be initialized here, so check first.
        if (Engine.Initialized) DoMyCustomWork();
        else
            Engine.OnInitializationFinished +=
                DoMyCustomWork;
    }

    private void DoMyCustomWork()
    {
        Debug.Log(
            "Engine is initialized here, it's safe to use the APIs");
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
            RuntimeInitializer.Initialize();
        }
        
        if (GUILayout.Button("ResetService"))
        {
            ResetService();
        }
    }
}