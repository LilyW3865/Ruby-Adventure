/* Teman Washington */
// Magic Potion Script


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPotion : MonoBehaviour

{
    public AudioClip pickupSound; //Teman Washington - Creates a public Audio clip called pickupSound
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        // Teman Washington - This plays a sound for the potion at pickup
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        if (controller != null)
        {
            // This should make the player walk backwards until picking up another powerup.

            controller.speed -= 6.0f;
            Destroy(gameObject);
        }
    }
}
