using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowEnemy : MonoBehaviour
{
    public Transform objectToFollow;

    [SerializeField]
    float offsetY;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (objectToFollow != null)
        {
            // rectTransform.anchoredPosition = objectToFollow.localPosition ;
            rectTransform.anchoredPosition =
                new Vector3(objectToFollow.transform.localPosition.x,
                    objectToFollow.transform.localPosition.y + offsetY,
                    objectToFollow.transform.localPosition.z);
        }
    }
}
