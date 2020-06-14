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

    private readonly Dictionary<FruitResourceType, ResourceInfo> _fruitResources = new Dictionary<FruitResourceType, ResourceInfo>();
    [FormerlySerializedAs("GoldTracker")] public int goldTracker = 0;
    public Image uiImageForage;
    public Image uiImageMoney;
    private ActionType _selectedAction = ActionType.Forage;
    private bool inRangeOfBusinessBob = false;
    public Text uiGold;
    public Text uiGoldOutOfRangWarning;
    private float warningStepTracker = -1;
    private int warningCharacterIndex = 0;
    private string warningMessage = "You Are Not In Range OF Business BOB";
    public GameObject playerWithUnit;

    
    public class ResourceInfo
    {
        public int Count = 0;
        public int Value = 0;

        public ResourceInfo(int value)
        {
            this.Value = value;
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        uiGold.text = $"Gold ${goldTracker}";
        _fruitResources.Add(FruitResourceType.ForestApple, new ResourceInfo(1));
        _fruitResources.Add(FruitResourceType.BlueCoconut, new ResourceInfo(2));
        _fruitResources.Add(FruitResourceType.StoneMelon, new ResourceInfo(6));
        _fruitResources.Add(FruitResourceType.BrimstoneMelon, new ResourceInfo(8));
        _fruitResources.Add(FruitResourceType.BigIce, new ResourceInfo(15));
        _fruitResources.Add(FruitResourceType.VoidPineapple, new ResourceInfo(20));
        _fruitResources.Add(FruitResourceType.OrangeOrange, new ResourceInfo(40));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        resourceTrackerUiText.text = $@"Forest Apple: {_fruitResources[FruitResourceType.ForestApple].Count}
Blue Coconut:  {_fruitResources[FruitResourceType.BlueCoconut].Count}
Stone Melon: {_fruitResources[FruitResourceType.StoneMelon].Count}
Brimstone Melon: {_fruitResources[FruitResourceType.BrimstoneMelon].Count}
Big Ice: {_fruitResources[FruitResourceType.BigIce].Count}
Void Pineapple: {_fruitResources[FruitResourceType.VoidPineapple].Count}
Orange Orange: {_fruitResources[FruitResourceType.OrangeOrange].Count}
";

        enabledModes = "Enabled Modes= ";
        if (GameVariables.IsMenuMode)
        {
            enabledModes += "MenuMode, ";
        }

        if (GameVariables.IsActionModeEnabled)
        {
            uiImageForage.enabled = (_selectedAction == ActionType.Forage) ? true : false;
            uiImageMoney.enabled = (_selectedAction == ActionType.Sell) ? true : false;
            enabledModes += "ActionModeEnabled, ";
        }
        
        if (GameVariables.IsBuildModeEnabled)
        {
            enabledModes += "BuildModeEnabled, ";
        }

        modeDebug.text = enabledModes;
        
        // Start Warning
        if (warningStepTracker > 5)
        {
            if (warningCharacterIndex > 0)
            {
                warningCharacterIndex--;
                uiGoldOutOfRangWarning.text = warningMessage.Substring(0, warningCharacterIndex);
            }
            else
            {
                warningStepTracker = -1;
            }
        }
        else if (warningStepTracker != -1)
        {
            warningStepTracker += Time.fixedDeltaTime;
            if (warningCharacterIndex <= warningMessage.Length)
            {
                uiGoldOutOfRangWarning.text = warningMessage.Substring(0, warningCharacterIndex);
                warningCharacterIndex++;
            }
        }
    }
    
    public void SelectActionInt(int actionIndex)
    {
        SelectAction((ActionType)actionIndex-1);
    }

    public void SelectAction(ActionType actionType)
    {
        _selectedAction = actionType;
    }

    public void IncreaseFruit(FruitResourceType fruitType, int fruitAmount)
    {
        _fruitResources[fruitType].Count += fruitAmount;
    }
    
    public void EatApples()
    {
        var missingHealth =  playerWithUnit.GetComponent<Unit>().UnitStats.MaxHealth -  playerWithUnit.GetComponent<Unit>().UnitStats.CurrentHealth;
        if (missingHealth == 0) return;
        playerWithUnit.GetComponent<Unit>().UnitStats.CurrentHealth += Mathf.Clamp(_fruitResources[FruitResourceType.ForestApple].Count, 0,missingHealth);
        _fruitResources[FruitResourceType.ForestApple].Count -=  (int)Mathf.Clamp(_fruitResources[FruitResourceType.ForestApple].Count, 0,missingHealth);
        playerWithUnit.GetComponent<Unit>().UnitStats.UpdateHealth();

    }

    public void SetSellStatus(bool sellState)
    {
        inRangeOfBusinessBob = sellState;
    }

    public bool UseMoney(int amount)
    {
        if (goldTracker >= amount)
        {
            goldTracker -= amount;
            uiGold.text = $"Gold ${goldTracker}";
            return true;
        }

        return false;
    }

    public void SellAll()
    {
        if (inRangeOfBusinessBob)
        {
            foreach(var item in _fruitResources)
            {
                goldTracker += item.Value.Count * item.Value.Value;
                item.Value.Count = 0;
            }
        }
        else
        {
            if (warningStepTracker == -1)
            {
                warningStepTracker = 0;
            } 
        }

        uiGold.text = $"Gold ${goldTracker}";
    }
}
