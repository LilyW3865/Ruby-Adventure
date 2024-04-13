using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public GameObject projectilePrefab;
    public ParticleSystem damageparticles;
    public ParticleSystem healthparticles;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public int health { get { return currentHealth; }}
    int currentHealth;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    Rigidbody2D rigidbody2d;
    float horizontal; 
    float vertical;
    public int score = 0;
    public GameObject ScoreText;
    public GameObject GameText;
    TextMeshProUGUI ScoreText_text;
    TextMeshProUGUI GameText_text;
    bool Gameover;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        ScoreText_text = ScoreText.GetComponent<TextMeshProUGUI>();
        GameText_text = GameText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

       ScoreText_text.text = "Fixed Robots: " + score.ToString();

       if(currentHealth < 1)
       {
            Gameover = true;
            GameText.SetActive(true);
            speed = 0;
            GameText_text.text = "You lost! Press R to Restart!";
            
            if(Gameover == true)
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
       }

       if(score == 2)
       {
            Gameover = true;
            GameText.SetActive(true);
            speed = 0;
            GameText_text.text = "You Win! Game Created by Group 37!";
       }
        
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            ParticleSystem DamageEffect = Instantiate(damageparticles, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            PlaySound(hitSound);
        }

        if(amount > 0)
        {
            ParticleSystem HealthEffect = Instantiate(healthparticles, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);   
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

    }

    public void ChangeScore(int ScoreAmount)
    {
        score += ScoreAmount;
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        
        PlaySound(throwSound);
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }


}
