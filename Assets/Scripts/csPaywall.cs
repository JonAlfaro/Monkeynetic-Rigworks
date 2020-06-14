using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Resources;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class csPaywall : MonoBehaviour
{
    [FormerlySerializedAs("Player1Tag")] public string player1Tag = "";
    public int price = 0;
    public Text uiText;
    public bool inSaleRange = false;
    private float lerpTracker = 0f;
    private int _directionMod = 1;
    public GameObject ghostBridge;
    public csResourceManager gbManager;
    public bool sold = false;
    public string payWallName;

    // Start is called before the first frame update
    void Start()
    {
        ghostBridge.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inSaleRange && Input.GetKey(KeyCode.E))
        {
            Purchase();
        }
        
        uiText.color = Color.HSVToRGB(Mathf.Lerp(0, 1f, lerpTracker), 1f, 1f);
        lerpTracker += (Time.deltaTime / 3) * _directionMod;
        if (lerpTracker > 1 )
        {
            lerpTracker = 0;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        
        if (!sold && other.gameObject.CompareTag(player1Tag))
        {
            uiText.text = $"Press E Purchase {payWallName} for {price} Gold ?";
            inSaleRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (!sold && other.gameObject.CompareTag(player1Tag))
        {
            uiText.text = $"";
            inSaleRange = false;
        }
    }

    public void Purchase()
    {
        if (gbManager.UseMoney(price))
        {
            Debug.Log("Bought " + payWallName);
            ghostBridge.SetActive(false);
            GetComponent<MeshCollider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            uiText.text = $"";
            sold = true;
            inSaleRange = false;
            this.enabled = false;
        }
    }
}
