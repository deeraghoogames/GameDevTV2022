using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    public float pickUpAmt;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.deathTimer += pickUpAmt;
            SoundManager.Instance.PlayTimePickUp();
            Destroy (gameObject);
        }
    }
}
