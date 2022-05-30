using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGoWithFX : MonoBehaviour
{
    [SerializeField]
    float destroyTime;

    public GameObject destroyFX;

    // Update is called once per frame
    void Update()
    {
        Destroy (gameObject, destroyTime);
        Instantiate(destroyFX, transform.position, transform.rotation);
    }
}
