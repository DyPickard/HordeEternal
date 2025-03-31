using UnityEngine;
using System.Collections;
using HordeEternal.Movement;

namespace HordeEternal.Movement
{
    public class TeleportAbility : MovementAbility
    {
        [SerializeField] private float teleportDistance = 5f;

        protected override void Start()
        {
            base.Start();
            cooldown = 8f; // Longest cooldown
        }

        public override void Activate()
        {
            if (!isOnCooldown)
            {
                StartCoroutine(TeleportSequence());
            }
        }

        private IEnumerator TeleportSequence()
        {
            isOnCooldown = true;

            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (direction == Vector2.zero)
            {
                direction = transform.right; // Default to facing right if no direction input
            }

            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = currentPosition + (direction * teleportDistance);

            transform.position = targetPosition;

            yield return new WaitForSeconds(cooldown);
            isOnCooldown = false;
        }
    }
}