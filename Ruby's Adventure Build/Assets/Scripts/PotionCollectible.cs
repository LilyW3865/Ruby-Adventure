//This script was created by Lilian Wagner. This script is attached to a game object that increases Ruby's speed and is destroyed when collected by Ruby.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.speed = 6.0f;
            Destroy(gameObject);
        }
    }
}
