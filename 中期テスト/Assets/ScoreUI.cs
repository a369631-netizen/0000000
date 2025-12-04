using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private Text scoreText;

    private void Awake()
    {
        // Canvas を強制生成（存在しない場合）
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // スコア Text を強制生成（存在しない場合）
        scoreText = FindObjectOfType<Text>();
        if (scoreText == null)
        {
            GameObject textGO = new GameObject("ScoreText");
            textGO.transform.SetParent(canvas.transform);
            scoreText = textGO.AddComponent<Text>();
            scoreText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            scoreText.fontSize = 24;
            scoreText.color = Color.white;
            scoreText.alignment = TextAnchor.UpperLeft;

            RectTransform rt = scoreText.rectTransform;
            rt.anchorMin = new Vector2(0f, 1f);   // 左上アンカー
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(10f, -10f); // 画面左上からの余白
            rt.sizeDelta = new Vector2(300f, 50f);
        }
    }

    private void Update()
    {
        // スコア表示を更新
        scoreText.text = $"Score: {ScoreManager.Score}";
    }
}