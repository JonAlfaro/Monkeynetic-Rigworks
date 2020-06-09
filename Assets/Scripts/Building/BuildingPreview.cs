using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public BuildingPrefab BuildingPrefab;
    public Material DisplayMaterial;
    public Vector3? Position = new Vector3(0, 0, 0);
    public Vector3 PositionOffset = new Vector3(0, 1, 0);

    private GameObject instantiatedObject;

    private void Awake()
    {
        InstantiateAndSetMaterial();
    }

    public void SetPosition(Vector3? position)
    {
        Position = position;
        if (position != null && instantiatedObject != null)
        {
            instantiatedObject.transform.position = (Vector3)Position + PositionOffset;
        }
    }

    public void SetMaterial(Material material)
    {
        DisplayMaterial = material;
        instantiatedObject.GetComponent<Renderer>().material = material;
    }

    public void SetObject(BuildingPrefab buildingPrefab)
    {
        BuildingPrefab = buildingPrefab;
        InstantiateAndSetMaterial();
    }

    private void InstantiateAndSetMaterial()
    {
        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
        }
        if (Position == null)
        {
            Position = new Vector3(0, 0, 0);
        }
        instantiatedObject = Instantiate(BuildingPrefab.BuildingPreview, (Vector3)Position + PositionOffset, Quaternion.identity);
        instantiatedObject.transform.SetParent(transform);
        if (DisplayMaterial)
        {
            instantiatedObject.GetComponent<Renderer>().material = DisplayMaterial;
        }

        // The SizeController component controls the size of the range indicator in the building preview
        SizeController sizeController = instantiatedObject.GetComponentInChildren<SizeController>();
        float range = BuildingPrefab.Building.GetComponent<Unit>().AttackRange;
        if (sizeController)
        {
            sizeController.SetSize(range * 2, null, range * 2);
        }
    }
}
