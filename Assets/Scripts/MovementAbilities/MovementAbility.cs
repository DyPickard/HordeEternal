using UnityEngine;

namespace HordeEternal.Movement
{
    public abstract class MovementAbility : MonoBehaviour
    {
        protected float cooldown;
        protected bool isOnCooldown;

        protected virtual void Start()
        {
            isOnCooldown = false;
        }

        public abstract void Activate();
    }
}