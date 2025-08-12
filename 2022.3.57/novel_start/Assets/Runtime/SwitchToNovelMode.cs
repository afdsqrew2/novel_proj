using Naninovel;
using UnityCommon;
using UnityEngine;

[CommandAlias("novel")]
public class SwitchToNovelMode : Command
{
    public StringParameter ScriptName;
    public StringParameter Label;

    public async UniTask ExecuteAsync (AsyncToken asyncToken = default)
    {

    }
    
    public override async UniTask Execute(AsyncToken token = default)
    {
        Debug.Log("execute 1");
        // 1. Disable character control.
        var controller = Object.FindObjectOfType<CharacterController3D>();
        if (controller)
        {
            controller.IsInputBlocked = true;
        }
        Debug.Log("execute 2");
        // 2. Switch cameras.
        var go = GameObject.Find("AdventureModeCamera");
        if (go != null)
        {
            var advCamera = go.GetComponent<Camera>();
            advCamera.enabled = false;
        }
        Debug.Log("execute 3");
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;
        Debug.Log("execute 4");
        // 3. Load and play specified script (is required).
        if (Assigned(ScriptName))
        {
            Debug.Log("execute 41");
            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            Debug.Log("execute 42");
            if (scriptPlayer == null)
            {
                Debug.Log("execute 421");
            }

            await scriptPlayer.LoadAndPlayAtLabel(ScriptName, label: Label);
            Debug.Log("execute 43");
            //await scriptPlayer.PreloadAndPlayAsync(ScriptName, label: Label);
        }
        Debug.Log("execute 5");
        // 4. Enable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        if (inputManager != null)
            inputManager.ProcessInput = true;
    }
}
