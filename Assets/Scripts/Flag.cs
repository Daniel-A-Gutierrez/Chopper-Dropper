using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject winScreen;
    void Start()
    {
        winScreen.SetActive(false);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            winScreen.SetActive(true);
        }
        Time.timeScale = 0f;
    }
}
