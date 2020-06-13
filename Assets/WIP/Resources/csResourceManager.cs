using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class csResourceManager : MonoBehaviour
{
    [FormerlySerializedAs("ResourceTrackerUIText")] public Text resourceTrackerUiText;
    [FormerlySerializedAs("ModeDebug")] public Text modeDebug;
    private string enabledModes = "";

    private readonly Dictionary<FruitResourceType, int> _fruitResources = new Dictionary<FruitResourceType, int>();
    
    // Start is called before the first frame update
    private void Start()
    {
        _fruitResources.Add(FruitResourceType.ForestApple, 0);
        _fruitResources.Add(FruitResourceType.BlueCoconut, 0);
        _fruitResources.Add(FruitResourceType.StoneMelon, 0);
        _fruitResources.Add(FruitResourceType.BrimstoneMelon, 0);
        _fruitResources.Add(FruitResourceType.BigIce, 0);
        _fruitResources.Add(FruitResourceType.VoidPineapple, 0);
        _fruitResources.Add(FruitResourceType.OrangeOrange, 0);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        resourceTrackerUiText.text = $@"Forest Apple: {_fruitResources[FruitResourceType.ForestApple]}
Blue Coconut:  {_fruitResources[FruitResourceType.BlueCoconut]}
Stone Melon: {_fruitResources[FruitResourceType.StoneMelon]}
Brimstone Melon: {_fruitResources[FruitResourceType.BrimstoneMelon]}
Big Ice: {_fruitResources[FruitResourceType.BigIce]}
Void Pineapple: {_fruitResources[FruitResourceType.VoidPineapple]}
Orange Orange: {_fruitResources[FruitResourceType.OrangeOrange]}
";

        enabledModes = "Enabled Modes= ";
        if (GameVariables.IsMenuMode)
        {
            enabledModes += "MenuMode, ";
        }

        if (GameVariables.IsActionModeEnabled)
        {
            enabledModes += "ActionModeEnabled, ";
        }
        
        if (GameVariables.IsBuildModeEnabled)
        {
            enabledModes += "BuildModeEnabled, ";
        }

        modeDebug.text = enabledModes;
    }

    public void IncreaseFruit(FruitResourceType fruitType, int fruitAmount)
    {
        _fruitResources[fruitType] += fruitAmount;
    }
}
