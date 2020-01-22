using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableOnPlay : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
