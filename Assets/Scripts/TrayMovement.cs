using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TrayInventory : MonoBehaviour
{
    [Header("Referencias")]
    public Transform trayTransform;       // normalmente la propia bandeja (puede ser transform)
    public Rigidbody playerRb;            // Rigidbody del jugador (se asigna en inspector)
    public PlayerMovement playerMovement; // referencia al script PlayerMovement

    [Header("UI / Eventos")]
    public TextMeshProUGUI cupsText;
    public UnityEvent OnAllCupsLost;      // asigna listeners en inspector (ej: detener score)

    private List<Cup> cups = new List<Cup>();

    private void Awake()
    {
        if (trayTransform == null) trayTransform = transform;
    }

    public void AddCup(Cup c)
    {
        if (!cups.Contains(c)) cups.Add(c);
        UpdateUI();
    }

    public void RemoveCup(Cup c)
    {
        if (cups.Contains(c)) cups.Remove(c);
        UpdateUI();

        if (cups.Count == 0)
        {
            OnAllCupsLost?.Invoke();
        }
    }

    public int CurrentCupCount => cups.Count;

    private void UpdateUI()
    {
        if (cupsText != null) cupsText.text = cups.Count.ToString();
    }
}
