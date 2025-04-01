using UnityEngine;

public class ShieldSpell : UtilitySpell
{
    public float duration = 5f;
    private bool isActive;
    private GameObject shieldVisual;
    [SerializeField] private AudioClip spellShieldSound;
    private AudioClip reversed;

    void Start()
    {
        spellShieldSound = Resources.Load<AudioClip>("Spells/SpellShield");
        reversed = AudioManager.Instance.CreateReversedClip(spellShieldSound);
    }

    public override void Activate()
    {
        if (isActive) return;

        Debug.Log("Shield activated!");
        isActive = true;
        AudioManager.Instance.PlaySFX(spellShieldSound);

        shieldVisual = GetComponentInParent<PlayerSpellManager>().transform.Find("ShieldVisual")?.gameObject;
        if (shieldVisual != null) shieldVisual.SetActive(true);

        GameManager gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();
        if (gameManager != null) gameManager.isShielded = true;

        Invoke(nameof(Deactivate), duration);
    }

    void Deactivate()
    {
        GameManager gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();
        if (gameManager != null) gameManager.isShielded = false;
        isActive = false;
        AudioManager.Instance.PlaySFX(reversed);

        GetComponentInParent<PlayerSpellManager>().ClearUtilitySpell();
        if (shieldVisual != null) shieldVisual.SetActive(false);

        Destroy(this.gameObject);
    }
}