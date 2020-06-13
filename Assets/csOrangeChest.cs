using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csOrangeChest : MonoBehaviour
{
    public float amount = 1000f;
    public float spawnPerTick = 5f;

    public GameObject orangeObject;

    public float sCooldown = 20f;
    private float _CooldownStep = -1;
    public GameObject resourceManager;
    public float shootUpSpeed = 3f;
    public MeshRenderer selfMesh;
    public Collider selfCollider;

    private float spawnCount = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnCount >= 0 && spawnCount <= amount)
        {
            for (var i = 0; i < spawnPerTick; i++)
            {
                var gb = Instantiate(orangeObject, this.transform.position, Quaternion.identity);
                gb.GetComponent<Rigidbody>().velocity = (Random.onUnitSphere * 3 + (gb.transform.up*shootUpSpeed));
                gb.GetComponent<csResourceDetails>().increaseResource.AddListener(resourceManager.GetComponent<csResourceManager>().IncreaseFruit);
                spawnCount++;
                if (spawnCount > amount)
                {
                    ToggleActive();
                    _CooldownStep = 0f;
                    spawnCount = -1;
                    break;
                }
            }

        }

        if (_CooldownStep >= sCooldown)
        {
            ToggleActive();
            _CooldownStep = -1;
        }
        else if (_CooldownStep != -1)
        {
            _CooldownStep += Time.fixedDeltaTime;
        }
    }

    public void OpenChest()
    {
        if (spawnCount == -1)
        {
            spawnCount = 0;
        }
    }

    private void ToggleActive()
    {
        selfCollider.enabled = !selfCollider.enabled;
        selfMesh.enabled = !selfMesh.enabled;
    }
}
