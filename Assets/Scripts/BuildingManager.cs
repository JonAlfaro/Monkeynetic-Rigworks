using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject Building;
    public Vector3 BuildingPosition;
    public Vector3 BuildingOffset = new Vector3(0, 1, 0);
    public Quaternion BuildingRotation = Quaternion.identity;

    public void SetBuildingPosition(Vector3 buildingPosition)
    {
        BuildingPosition = buildingPosition;
    }

    public void SetBuildingOffset(Vector3 buildingOffset)
    {
        BuildingOffset = buildingOffset;
    }

    public void SetBuilding(GameObject building)
    {
        Building = building;
    }

    public void SetBuildingRotation(Quaternion buildingRotation)
    {
        BuildingRotation = buildingRotation;
    }

    public void Build()
    {
        Instantiate(Building, BuildingPosition + BuildingOffset, BuildingRotation);
    }
}
