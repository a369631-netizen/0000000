using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Score { get; private set; }

    // どこからでも簡単に加点できるように静的メソッドにしています
    public static void Add(int amount)
    {
        Score += amount;
        Debug.Log($"Score: {Score}");
    }
}