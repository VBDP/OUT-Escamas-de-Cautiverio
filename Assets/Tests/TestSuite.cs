using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class TestSuite
{
private GameObject player;
    private LifeSystem lifeSystem;
    private Image healthBar;
    private TextMeshProUGUI interactionText;

    [SetUp]
    public void Setup()
    {
        // Crear jugador temporal
        player = new GameObject("TestPlayer");
        player.AddComponent<Rigidbody>();
        player.AddComponent<AudioSource>();

        // Crear canvas temporal para Image y TextMeshProUGUI
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        GameObject healthGO = new GameObject("Health");
        healthGO.transform.parent = canvasGO.transform;
        healthBar = healthGO.AddComponent<Image>();

        GameObject textGO = new GameObject("Text");
        textGO.transform.parent = canvasGO.transform;
        interactionText = textGO.AddComponent<TextMeshProUGUI>();

        // Añadir LifeSystem
        lifeSystem = player.AddComponent<LifeSystem>();

        // Inicializar referencias privadas
        typeof(LifeSystem).GetField("healthImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(lifeSystem, healthBar);

        typeof(LifeSystem).GetField("interactionText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(lifeSystem, interactionText);

        // Llamar Start manualmente para inicializar
        lifeSystem.Start();
    }

    [UnityTest]
    public IEnumerator DamagePlayerReducesHealth()
    {
        float initialHealth = lifeSystem.currentHealth;
        lifeSystem.DamagePlayer(30f);

        yield return null; // Esperar un frame

        Assert.Less(lifeSystem.currentHealth, initialHealth);
        Assert.AreEqual(0.7f, lifeSystem.healthImage.fillAmount, 0.01f);
    }

    [UnityTest]
    public IEnumerator HealPlayerIncreasesHealth()
    {
        lifeSystem.DamagePlayer(50f); // Llevar vida a 50
        yield return null;

        lifeSystem.HealPlayer(30f); // Sanar 30
        yield return null;

        Assert.AreEqual(80f, lifeSystem.currentHealth, 0.01f);
        Assert.AreEqual(0.8f, lifeSystem.healthImage.fillAmount, 0.01f);
    }

    [UnityTest]
    public IEnumerator KillPlayerResetsPositionAndHealth()
    {
        Vector3 initialPos = player.transform.position;
        player.transform.position = new Vector3(10, 0, 10);

        lifeSystem.KillPlayer();
        yield return null;

        Assert.AreEqual(initialPos, player.transform.position);
        Assert.AreEqual(100f, lifeSystem.currentHealth);
        Assert.AreEqual(1f, lifeSystem.healthImage.fillAmount, 0.01f);
    }
    
}


