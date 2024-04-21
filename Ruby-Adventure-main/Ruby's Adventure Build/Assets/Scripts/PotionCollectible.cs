//This script was created by Lilian Wagner. This script is attached to a game object that increases Ruby's speed and is destroyed when collected by Ruby.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCollectible : MonoBehaviour
{
    public AudioClip pickupSound;     // Teman Washington - This script addition allows a sound to play from Lilian's potion

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        // Teman Washington - This plays a sound for the potion at pickup
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        if (controller != null)
        {
            controller.speed = 6.0f;
            Destroy(gameObject);
        }
    }
}
