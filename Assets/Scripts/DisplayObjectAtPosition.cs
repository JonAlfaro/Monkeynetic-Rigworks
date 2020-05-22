using UnityEngine;

public class DisplayObjectAtPosition : MonoBehaviour
{
    public GameObject Object;
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

    public void SetObject(GameObject gameObject)
    {
        Destroy(instantiatedObject);
        Object = gameObject;
        InstantiateAndSetMaterial();
    }

    public void SetObject(BuildingPrefab buildingPrefab)
    {
        Object = buildingPrefab.BuildingPreview;
        InstantiateAndSetMaterial();
    }

    private void InstantiateAndSetMaterial()
    {
        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
        }
        instantiatedObject = Instantiate(Object, (Vector3)Position + PositionOffset, Quaternion.identity);
        instantiatedObject.transform.SetParent(transform);
        if (DisplayMaterial)
        {
            instantiatedObject.GetComponent<Renderer>().material = DisplayMaterial;
        }
    }
}
