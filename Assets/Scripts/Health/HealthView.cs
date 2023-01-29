using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HealthView : MonoBehaviour
{
    [SerializeField] Gradient healtColors;
    private SpriteRenderer _spriteRenderer;

    private const float coef = 2.553336f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeHealthView(int maxHealth, int health)
    {
        float healthProcent = ((float)health) / maxHealth;
        gameObject.transform.localScale = new Vector3(healthProcent, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        gameObject.transform.localPosition = new Vector3((1 - healthProcent) / 2 * coef, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        _spriteRenderer.color = healtColors.Evaluate(healthProcent);
    }
}
