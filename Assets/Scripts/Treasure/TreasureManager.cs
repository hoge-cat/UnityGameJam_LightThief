using System.Collections;
using TMPro;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager Instance { get; private set; }

    [Header("宝UI")]
    [SerializeField] private TMP_Text treasureText;

    [Header("目的UI")]
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text goalMessageText;
    [SerializeField] private float goalMessageDuration = 4.0f;

    [Header("ゴール設定")]
    [SerializeField] private GoalDoor goalDoor;

    private int collectedTreasure;
    private int totalTreasure;
    private bool hasActivatedGoal;

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

        if (objectiveText != null)
        {
            objectiveText.text =
                "すべての宝を回収しろ";
        }

        if (goalMessageText != null)
        {
            goalMessageText.gameObject.SetActive(false);
        }

        UpdateTreasureUI();
    }

    public void CollectTreasure()
    {
        collectedTreasure++;
        UpdateTreasureUI();

        if (HasCollectedAllTreasure() &&
            !hasActivatedGoal)
        {
            ActivateGoal();
        }
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
        return totalTreasure > 0 &&
               collectedTreasure >= totalTreasure;
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

    private void ActivateGoal()
    {
        hasActivatedGoal = true;

        if (goalDoor != null)
        {
            goalDoor.UnlockGoal();
        }
        else
        {
            Debug.LogWarning(
                "TreasureManager：GoalDoorが設定されていません。");
        }

        if (objectiveText != null)
        {
            objectiveText.text = "脱出しろ";
        }

        if (goalMessageText != null)
        {
            StartCoroutine(
                ShowGoalMessage()
            );
        }

        Debug.Log(
            "すべての宝を集めました。出口が解放されました。"
        );
    }

    private IEnumerator ShowGoalMessage()
    {
        goalMessageText.text =
            "宝を集めきった!\n" +
            "脱出地点へ向かえ";

        goalMessageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(
            goalMessageDuration
        );

        goalMessageText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}