using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{

    private HealthSystem healthSystem;
    private Transform healthBar;
    private Animator animator;

    private void Awake()
    {
        healthSystem = new HealthSystem(100);
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        healthBar = transform.Find("HealthBar").Find("Bar");
        animator = transform.Find("ShootingTarget").GetComponent<Animator>();
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        healthBar.transform.localScale = new Vector3(healthSystem.GetHealthNormalized(), 1f, 1f);
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    public void Damage()
    {
        healthSystem.Damage(35);
        string trigger = Random.Range(0, 100) < 50 ? "Damage" : "Damage_2";
        animator.SetTrigger(trigger);
    }

}