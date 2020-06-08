using UnityEngine;

public class SizeController : MonoBehaviour
{
    public void SetSize(Vector3 newSize)
    {
        transform.localScale = newSize;
    }

    public void SetSize(float? x, float? y, float? z)
    {
        Vector3 newSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (x != null)
        {
            newSize.x = (float) x;
        }

        if (y != null)
        {
            newSize.y = (float)y;
        }

        if (z != null)
        {
            newSize.z = (float)z;
        }

        SetSize(newSize);
    }
}
