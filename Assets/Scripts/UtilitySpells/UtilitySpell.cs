using UnityEngine;

public abstract class UtilitySpell : MonoBehaviour
{
    public abstract void Activate();

    protected T FindWeaponSpellComponent<T>() where T : Component
    {
        if (GameManager.Instance == null || GameManager.Instance.Player == null)
        {
            Debug.LogError($"{GetType().Name}: GameManager or Player is null!");
            return null;
        }

        PlayerSpellManager spellManager = GameManager.Instance.Player.GetComponent<PlayerSpellManager>();
        if (spellManager != null && spellManager.spellSlot != null)
        {
            T spellComponent = spellManager.spellSlot.GetComponentInChildren<T>();
            if (spellComponent != null)
            {
                Debug.Log($"{GetType().Name} found {typeof(T).Name} component successfully");
                return spellComponent;
            }
            else
            {
                Debug.LogError($"{GetType().Name} couldn't find {typeof(T).Name} in spell slot!");
                return null;
            }
        }
        else
        {
            Debug.LogError($"{GetType().Name} couldn't find PlayerSpellManager or spell slot!");
            return null;
        }
    }
}