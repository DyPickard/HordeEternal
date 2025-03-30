using UnityEngine;

public class ShieldSpell : UtilitySpell
{
    public float duration = 5f;
    private bool isActive;
    private GameObject shieldVisual;

    public override void Activate()
    {
        if (isActive) return;

        Debug.Log("Shield activated!");
        isActive = true;

        shieldVisual = GetComponentInParent<PlayerSpellManager>().transform.Find("ShieldVisual")?.gameObject;
        if (shieldVisual != null) shieldVisual.SetActive(true);

        Invoke(nameof(Deactivate), duration);
    }

    void Deactivate()
    {
        isActive = false;
        GetComponentInParent<PlayerSpellManager>().ClearUtilitySpell();
        if (shieldVisual != null) shieldVisual.SetActive(false);
        Destroy(this.gameObject);
    }
}