using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class csResourceManager : MonoBehaviour
{
    [FormerlySerializedAs("ResourceAppleForestText")] public Text resourceAppleForestText;

    [FormerlySerializedAs("ResourceBlueCoconutText")] public Text resourceBlueCoconutText;
    [FormerlySerializedAs("ResourceWhiteBerry")] public Text resourceWhiteBerry;
    [FormerlySerializedAs("ModeDebug")] public Text modeDebug;
    private string enabledModes = "";

    private readonly Dictionary<FruitResourceType, int> _fruitResources = new Dictionary<FruitResourceType, int>();
    
    // Start is called before the first frame update
    private void Start()
    {
        _fruitResources.Add(FruitResourceType.ForestApple, 0);
        _fruitResources.Add(FruitResourceType.BlueCoconut, 0);
        _fruitResources.Add(FruitResourceType.WhiteBerry, 0);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        resourceAppleForestText.text = $"Forest Apple: {_fruitResources[FruitResourceType.ForestApple]}";
        resourceBlueCoconutText.text = $"Blue Coconut: {_fruitResources[FruitResourceType.BlueCoconut]}";
        resourceWhiteBerry.text = $"White Berry: {_fruitResources[FruitResourceType.WhiteBerry]}";

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
