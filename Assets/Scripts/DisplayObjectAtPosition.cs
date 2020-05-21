using UnityEngine;

public class DisplayObjectAtPosition : MonoBehaviour
{
    public bool Enabled;
    public GameObject Object;
    public Material DisplayMaterial;
    public Vector3 Position;
    public Vector3 PositionOffset = new Vector3(0, 1, 0);

    private GameObject instantiatedObject;

    private void Awake()
    {
        InstantiateAndSetMaterial();
    }

    public void EnableDisplay(bool enabled)
    {
        instantiatedObject.SetActive(enabled);
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
        instantiatedObject.transform.position = position + PositionOffset;
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

    private void InstantiateAndSetMaterial()
    {
        instantiatedObject = Instantiate(Object, Position + PositionOffset, Quaternion.identity);
        if (DisplayMaterial)
        {
            instantiatedObject.GetComponent<Renderer>().material = DisplayMaterial;
        }
    }
}
