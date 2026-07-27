using TMPro;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager Instance { get; private set; }

    [SerializeField] private TMP_Text treasureText;

    private int collectedTreasure;
    private int totalTreasure;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TreasureItem[] treasures =
            FindObjectsByType<TreasureItem>(
                FindObjectsSortMode.None
            );

        totalTreasure = treasures.Length;

        UpdateTreasureUI();
    }

    public void CollectTreasure()
    {
        collectedTreasure++;
        UpdateTreasureUI();
    }

    public int GetCollectedTreasure()
    {
        return collectedTreasure;
    }

    public int GetTotalTreasure()
    {
        return totalTreasure;
    }

    public bool HasCollectedAllTreasure()
    {
        return collectedTreasure >= totalTreasure;
    }

    private void UpdateTreasureUI()
    {
        if (treasureText == null)
        {
            return;
        }

        treasureText.text =
            "TREASURE\n" +
            collectedTreasure +
            " / " +
            totalTreasure;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}