using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject Building;
    public Vector3 BuildingPosition;
    public Vector3 BuildingOffset = new Vector3(0, 1, 0);
    public Quaternion BuildingRotation = Quaternion.identity;
    public bool IsBuildingPositionValid { get; set; }

    private Vector3 buildingExtents = new Vector3(0.5f, 0.5f, 0.5f);

    private void Awake()
    {
        SetBuilding(Building);
    }

    public void SetBuildingPosition(Vector3 buildingPosition)
    {
        IsBuildingPositionValid = !AreOtherThingsInTheWay(buildingPosition + BuildingOffset);
        BuildingPosition = buildingPosition;
    }

    public void SetBuildingOffset(Vector3 buildingOffset)
    {
        IsBuildingPositionValid = !AreOtherThingsInTheWay(BuildingPosition + buildingOffset);
        BuildingOffset = buildingOffset;
    }

    public void SetBuilding(GameObject building)
    {
        Building = building;

        if (Building == null)
        {
            return;
        }

        // Get the building extents. This is used to check for collisions to determine if this is a valid spot to build
        Collider buildingCollider = building.GetComponent<Collider>();
        if (buildingCollider != null)
        {
            buildingExtents = buildingCollider.bounds.extents;
        }
        else
        {
            buildingExtents = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public void SetBuildingRotation(Quaternion buildingRotation)
    {
        BuildingRotation = buildingRotation;
    }

    public void Build()
    {
        if (IsBuildingPositionValid && Building != null)
        {
            Instantiate(Building, BuildingPosition + BuildingOffset, BuildingRotation);
        }
        else
        {
            Debug.Log("This is an invalid building position or there is nothing to build");
        }
    }

    private bool AreOtherThingsInTheWay(Vector3 position)
    {
        // FIXME for some reason BoxCast() returns false even if there are things in the way but BoxCastAll returns correctly
        return Physics.BoxCastAll(position, buildingExtents, Vector3.up).Length > 0;
    }
}
