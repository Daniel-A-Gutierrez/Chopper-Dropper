using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance ;
    [SerializeField]
    private string playerTag;
    private GameObject player;
    public GameObject Player{get=>player;}
    
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Debug.LogWarning("Second Game Manager Awakens");
        player = GameObject.FindGameObjectWithTag(playerTag);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
