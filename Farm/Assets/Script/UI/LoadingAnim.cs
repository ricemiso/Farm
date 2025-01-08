using UnityEngine;
using TMPro; // TextMeshProを使用
using DG.Tweening; // DOTweenを使用

public class LoadingAnim : MonoBehaviour
{
    private const float DURATION = 1f; // アニメーションの周期

    void Start()
    {
        // 子オブジェクトから TMP_Text コンポーネントを取得
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();

        // TextMeshPro コンポーネントが見つからない場合、エラーログを表示して終了
        if (texts.Length == 0)
        {
            Debug.LogError("No TMP_Text components found under LoadingAnim!");
            return;
        }

        Debug.Log($"Found {texts.Length} TMP_Text components.");

        // 各文字に対してアニメーションを設定
        for (var i = 0; i < texts.Length; i++)
        {
            RectTransform rectTransform = texts[i].GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError($"RectTransform is null for text at index {i}");
                continue;
            }

            // 子オブジェクトの位置設定
            rectTransform.anchoredPosition = new Vector2((i - texts.Length / 2) * 50f, 0);


            // DOTweenアニメーションのシーケンスを作成
            Sequence sequence = DOTween.Sequence()
                .SetLoops(-1, LoopType.Restart) // 無限ループ
                .SetDelay((DURATION / 2) * ((float)i / texts.Length)) // 順番に遅延させる
                .Append(rectTransform.DOAnchorPosY(30f, DURATION / 4)) // Y座標を上に移動
                .Append(rectTransform.DOAnchorPosY(0f, DURATION / 4)); // 元の位置に戻す

            // アニメーションを再生
            sequence.Play();
        }
    }
}
