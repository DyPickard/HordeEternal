using UnityEngine;
using System.Collections;
using HordeEternal.Movement;

namespace HordeEternal.Movement
{
    public class ChargeAbility : MovementAbility
    {
        [SerializeField] private float chargeSpeed = 20f;
        [SerializeField] private float chargeDuration = 0.3f;
        private Rigidbody2D rb;

        protected override void Start()
        {
            base.Start();
            cooldown = 5f; // Medium cooldown
            rb = GetComponent<Rigidbody2D>();
        }

        public override void Activate()
        {
            if (!isOnCooldown && rb != null)
            {
                StartCoroutine(ChargeSequence());
            }
        }

        private IEnumerator ChargeSequence()
        {
            isOnCooldown = true;

            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (direction == Vector2.zero)
            {
                direction = transform.right; // Default to facing right if no direction input
            }

            Vector2 originalVelocity = rb.linearVelocity;

            rb.linearVelocity = direction * chargeSpeed;

            yield return new WaitForSeconds(chargeDuration);

            rb.linearVelocity = originalVelocity;

            yield return new WaitForSeconds(cooldown);
            isOnCooldown = false;
        }
    }
}