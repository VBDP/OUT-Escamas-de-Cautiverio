using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class LifeSystemTests
{
    private GameObject playerGO;
    private LifeSystem lifeSystem;
    private GameObject gmGO;
    private GeneralManager gm;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // ----- Crear GeneralManager falso -----
        gmGO = new GameObject("GeneralManager");
        gm = gmGO.AddComponent<GeneralManager>();
        gm.healthBar = new GameObject("HealthBar").AddComponent<Image>();
        gm.interactionText = new GameObject("Text").AddComponent<TextMeshProUGUI>();
        gm.sfxSource = gmGO.AddComponent<AudioSource>();

        // ----- Crear Player -----
        playerGO = new GameObject("Player");
        playerGO.AddComponent<Rigidbody>();

        lifeSystem = playerGO.AddComponent<LifeSystem>();

        // Forzar la inyección de dependencias
        yield return null;
    }

    [UnityTest]
    public IEnumerator DamagePlayer_ReducesHealth()
    {
        float initialHealth = lifeSystem.currentHealth;
        lifeSystem.DamagePlayer(20f);
        yield return null;
        Assert.AreEqual(initialHealth - 20f, lifeSystem.currentHealth);
    }

    [UnityTest]
    public IEnumerator HealPlayer_IncreasesHealth()
    {
        lifeSystem.currentHealth = 50f;
        lifeSystem.HealPlayer(20f);
        yield return null;
        Assert.AreEqual(70f, lifeSystem.currentHealth);
    }

    [UnityTest]
    public IEnumerator HealPlayer_DoesNotExceedMaxHealth()
    {
        lifeSystem.currentHealth = 90f;
        lifeSystem.HealPlayer(20f);
        yield return null;
        Assert.AreEqual(lifeSystem.maxHealth, lifeSystem.currentHealth);
    }

    [UnityTest]
    public IEnumerator KillPlayer_RespawnsAndRestoresHealth()
    {
        Vector3 spawnPos = playerGO.transform.position;
        lifeSystem.currentHealth = 0f;
        lifeSystem.KillPlayer();
        yield return null;
        Assert.AreEqual(100f, lifeSystem.currentHealth);
        Assert.AreEqual(spawnPos, playerGO.transform.position);
    }

    [UnityTest]
    public IEnumerator LifeImageFillAmount_UpdatesUI()
    {
        lifeSystem.currentHealth = 50f;
        lifeSystem.LifeImageFillAmount();
        yield return null;
        Assert.AreEqual(0.5f, lifeSystem.healthImage.fillAmount);
    }

    [TearDown]
    public void Cleanup()
    {
        Object.Destroy(playerGO);
        Object.Destroy(gmGO);
    }
}
