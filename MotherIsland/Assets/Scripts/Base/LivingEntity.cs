using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
	public float startingHealth;

	protected float health;
	protected RaycastHit lastHit;

	public bool IsActive { get; protected set; }
	public bool IsDead { get; protected set; }

	public event Action OnDeath;

	protected virtual void Start()
	{
		health = startingHealth;
	}

	public virtual void Initialize()
	{
		Debug.Log ("Initialized " + gameObject.name );
		health = startingHealth;
		IsActive = true;
		IsDead = false;
	}

	public virtual void TakeHit(float damage, RaycastHit hit)
	{
		lastHit = hit;
		TakeDamage (damage);
	}

	public virtual void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0 && !IsDead)
		{
			Die ();
		}
	}

	protected virtual void Die()
	{
		IsActive = false;
		IsDead = true;
		if (OnDeath != null)
			OnDeath ();

		// NOTE: Every type of living entity should decide what happens to itself when they die.
	}
}
