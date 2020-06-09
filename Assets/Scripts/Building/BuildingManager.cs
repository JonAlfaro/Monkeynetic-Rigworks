using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BuildingManager : MonoBehaviour
{
    public BuildingPrefab[] Buildings;
    public Vector3? BuildingPosition;
    public Vector3 BuildingOffset = new Vector3(0, 1, 0);
    public Quaternion BuildingRotation = Quaternion.identity;
    public bool IsBuildingPositionValid { get; set; }
    public UnityEvent<bool> BuildingValidChanged;
    public UnityEvent<BuildingPrefab> BuildingChanged;

    private GameObject selectedBuilding;
    private Vector3 buildingExtents = new Vector3(0.5f, 0.5f, 0.5f);
    private List<UIBuildingOption> uiBuildingOptions;

    private void Start()
    {
        uiBuildingOptions = new List<UIBuildingOption>(FindObjectsOfType<UIBuildingOption>(true)).OrderBy(x => x.Order).ToList();
        SetBuilding(0);
    }

    public void SetBuildingPosition(Vector3? buildingPosition)
    {
        BuildingPosition = buildingPosition;
        SetIsBuildingPositionValid();
    }

    public void SetBuildingOffset(Vector3 buildingOffset)
    {
        BuildingOffset = buildingOffset;
        SetIsBuildingPositionValid();
    }

    public void SetBuildingRotation(Quaternion buildingRotation)
    {
        BuildingRotation = buildingRotation;
    }

    public void Build()
    {
        if (!GameVariables.IsBuildModeEnabled)
        {
            return;
        }

        if (IsBuildingPositionValid && selectedBuilding != null)
        {
            Instantiate(selectedBuilding, (Vector3)BuildingPosition + BuildingOffset, BuildingRotation);
        }
        else
        {
            Debug.Log("This is an invalid building position or there is nothing to build");
        }
    }

    public void ToggleBuildMode()
    {
        SetIsBuildingPositionValid();
    }

    public void SelectBuilding(int buildingIndex)
    {
        if (!GameVariables.IsBuildModeEnabled)
        {
            return;
        }
        if (buildingIndex < 1 || buildingIndex > Buildings.Length)
        {
            Debug.Log($"Can't set building {buildingIndex}, assign it in the inspector first.");
            return;
        }

        SetBuilding(buildingIndex - 1);
    }

    private void SetBuilding(int buildingIndex)
    {
        BuildingPrefab buildingPrefab = Buildings[buildingIndex];
        selectedBuilding = buildingPrefab.Building;
        foreach (UIBuildingOption buildingOption in uiBuildingOptions)
        {
            buildingOption.SelectedOverlay.SetActive(false);
        }

        if (selectedBuilding == null)
        {
            return;
        }

        // Get the building extents. This is used to check for collisions to determine if this is a valid spot to build
        Collider buildingCollider = selectedBuilding.GetComponent<Collider>();
        if (buildingCollider != null)
        {
            buildingExtents = buildingCollider.bounds.extents;
        }
        else
        {
            buildingExtents = new Vector3(0.5f, 0.5f, 0.5f);
        }
        uiBuildingOptions[buildingIndex].SelectedOverlay.SetActive(true);
        BuildingChanged.Invoke(buildingPrefab);
        SetIsBuildingPositionValid();
    }

    private void SetIsBuildingPositionValid()
    {
        IsBuildingPositionValid = GameVariables.IsBuildModeEnabled && BuildingPosition != null && !AreOtherThingsInTheWay((Vector3)BuildingPosition + BuildingOffset);
        BuildingValidChanged.Invoke(IsBuildingPositionValid);
    }

    private bool AreOtherThingsInTheWay(Vector3 position)
    {
        // FIXME for some reason BoxCast() returns false even if there are things in the way but BoxCastAll returns correctly
        return Physics.BoxCastAll(position, buildingExtents, Vector3.up).Length > 0;
    }
}
