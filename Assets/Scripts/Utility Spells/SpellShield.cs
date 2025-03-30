using UnityEngine;

public class ShieldSpell : UtilitySpell
{
    public float duration = 5f;
    private bool isActive;

    public override void Activate()
    {
        if (isActive) return;

        Debug.Log("Shield activated!");
        isActive = true;

        // Example: turn player blue
        GetComponentInParent<SpriteRenderer>().color = Color.cyan;

        Invoke(nameof(Deactivate), duration);
    }

    void Deactivate()
    {
        isActive = false;
        GetComponentInParent<SpriteRenderer>().color = Color.white;
    }
}