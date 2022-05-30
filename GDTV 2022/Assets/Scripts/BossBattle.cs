using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattle : MonoBehaviour
{
    public FloatSlider bossHealthBar;

    public float bossMaxHealth = 30;

    public float bossHealth = 30;

    public Color flashColor;

    public Color normalColor;

    public float flashDuration = 0.2f;

    public int numberOfFlashes;

    public SpriteRenderer bossSprite;

    public static BossBattle Instance;

    void Awake()
    {
        if (Instance == null)
        {
            // Save a reference to 'this'
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = bossMaxHealth;
        bossHealthBar.SetMaxFloatValue (bossHealth);
        bossHealthBar.SetFloatValue (bossHealth);
    }

    // Update is called once per frame
    void Update()
    {
        bossHealthBar.SetFloatValue (bossHealth);
    }

    public void DamageBoss()
    {
        bossHealth--;
        StartCoroutine(FlashDamageCo());
        bossHealthBar.SetFloatValue (bossHealth);

        if (bossHealth <= 0)
        {
            bossHealth = 0;
        }
    }

    IEnumerator FlashDamageCo()
    {
        int flashcount = 0;

        // playerTrigger.SetActive(false);
        while (flashcount < numberOfFlashes)
        {
            bossSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            bossSprite.color = normalColor;
            yield return new WaitForSeconds(flashDuration);
            flashcount++;
        }

        bossSprite.color = normalColor;
        // playerTrigger.SetActive(true);
    }
}
