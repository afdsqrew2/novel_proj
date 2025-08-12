using Naninovel;
using Naninovel.Commands;
using UnityCommon;
using UnityEngine;

[CommandAlias("adventure")]
public class SwitchToAdventureMode : Command
{
    [ParameterAlias("reset")]
    public BooleanParameter ResetState = true;

    public async UniTask ExecuteAsync (AsyncToken asyncToken = default)
    {

    }

    public override async UniTask Execute(AsyncToken asyncToken = default)
    {
        // 1. Disable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = false;

        // 2. Stop script player.
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        scriptPlayer.Stop();

        // 3. Hide text printer.
        var hidePrinter = new HidePrinter();
        hidePrinter.Execute(asyncToken).Forget();
        //hidePrinter.ExecuteAsync(asyncToken).Forget();

        // 4. Reset state (if required).
        if (ResetState)
        {
            var stateManager = Engine.GetService<IStateManager>();
            await stateManager.ResetState();
            //await stateManager.ResetStateAsync();
        }

        // 5. Switch cameras.
        GameObject go =
            GameObject.Find("AdventureModeCamera");
        if (go != null)
        {
            var advCamera = GameObject.Find("AdventureModeCamera").GetComponent<Camera>();
            advCamera.enabled = true;
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = false;
        }


        // 6. Enable character control.
        var controller = Object.FindObjectOfType<CharacterController3D>();
        if (controller != null)
        {
            controller.IsInputBlocked = false;
        }
        
        IUIManager manager = Engine.GetService<IUIManager>();
        manager.GetUI("TitleUI").Hide();
    }
}
