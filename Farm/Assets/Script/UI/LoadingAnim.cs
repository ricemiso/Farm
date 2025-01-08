using UnityEngine;
using TMPro; // TextMeshPro���g�p
using DG.Tweening; // DOTween���g�p

public class LoadingAnim : MonoBehaviour
{
    private const float DURATION = 1f; // �A�j���[�V�����̎���

    void Start()
    {
        // �q�I�u�W�F�N�g���� TMP_Text �R���|�[�l���g���擾
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();

        // TextMeshPro �R���|�[�l���g��������Ȃ��ꍇ�A�G���[���O��\�����ďI��
        if (texts.Length == 0)
        {
            Debug.LogError("No TMP_Text components found under LoadingAnim!");
            return;
        }

        Debug.Log($"Found {texts.Length} TMP_Text components.");

        // �e�����ɑ΂��ăA�j���[�V������ݒ�
        for (var i = 0; i < texts.Length; i++)
        {
            RectTransform rectTransform = texts[i].GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError($"RectTransform is null for text at index {i}");
                continue;
            }

            // �q�I�u�W�F�N�g�̈ʒu�ݒ�
            rectTransform.anchoredPosition = new Vector2((i - texts.Length / 2) * 50f, 0);


            // DOTween�A�j���[�V�����̃V�[�P���X���쐬
            Sequence sequence = DOTween.Sequence()
                .SetLoops(-1, LoopType.Restart) // �������[�v
                .SetDelay((DURATION / 2) * ((float)i / texts.Length)) // ���Ԃɒx��������
                .Append(rectTransform.DOAnchorPosY(30f, DURATION / 4)) // Y���W����Ɉړ�
                .Append(rectTransform.DOAnchorPosY(0f, DURATION / 4)); // ���̈ʒu�ɖ߂�

            // �A�j���[�V�������Đ�
            sequence.Play();
        }
    }
}
