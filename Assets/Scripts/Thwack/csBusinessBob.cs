using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csBusinessBob : MonoBehaviour
{
    public string Player1Tag = "Player";
    public GameObject ResourceManager;
    public AudioClip[] Howdies;
    public AudioClip[] GoodByedies;
    private AudioSource _audioSource;
    private System.Random r = new System.Random();
    private int _soundTracker = 1;
    private int _soundTrackerG = 1;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Random r = new Random();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag(Player1Tag))
        {
            if (_soundTracker % 15 == 0)
            {
                _audioSource.PlayOneShot(Howdies[4]);
            }
            else
            {
                _audioSource.PlayOneShot(Howdies[r.Next(0, Howdies.Length-1)]);
            }
            ResourceManager.GetComponent<csResourceManager>().SetSellStatus(true);
            _soundTracker++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.CompareTag(Player1Tag))
        {
            if (_soundTrackerG % 25 == 0)
            {
                _audioSource.PlayOneShot(GoodByedies[7]);
            }
            else
            {
                _audioSource.PlayOneShot(GoodByedies[r.Next(0, GoodByedies.Length-1)]);
            }
            ResourceManager.GetComponent<csResourceManager>().SetSellStatus(false);
            _soundTrackerG++;
        }
            
    }
}
