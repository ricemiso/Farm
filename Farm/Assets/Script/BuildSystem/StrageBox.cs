using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �v���C���[���߂Â��ƊJ������[�{�b�N�X��\���N���X�B
/// </summary>
public class StrageBox : MonoBehaviour
{
    /// <summary>
    /// �v���C���[���͈͓��ɂ��邩�ǂ����������t���O�B
    /// </summary>
    public bool playerInRange;

    /// <summary>
    /// �v���C���[�Ƃ̔��苗���B
    /// </summary>
    [SerializeField] float dis = 10f;

    /// <summary>
    /// ���[�{�b�N�X���̃A�C�e�����X�g�B
    /// </summary>
    [SerializeField] public List<string> items;

    /// <summary>
    /// �A�j���[�V�����R���|�[�l���g�B
    /// </summary>
    public Animation animation;

    /// <summary>
    /// �A�j���[�V�����̃J�E���g���Ǘ�����ϐ��B
    /// </summary>
    private int cnt = 0;

    /// <summary>
    /// �A�j���[�V�����R���|�[�l���g���擾���܂��B
    /// </summary>
    private void Start()
    {
        animation = GetComponent<Animation>();
    }

    /// <summary>
    /// �{�b�N�X�̃^�C�v��\���񋓌^�B
    /// </summary>
    public enum BoxType
    {
        smallBox,
        bigBox
    }

    /// <summary>
    /// ���̃{�b�N�X�̃^�C�v�B
    /// </summary>
    public BoxType thisboxType;

    /// <summary>
    /// �v���C���[�Ƃ̋����𑪒肵�A���[UI�̊J�ɉ����ă{�b�N�X���J���܂��B
    /// </summary>
    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < dis)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (StorageManager.Instance.storageUIOpen)
        {
            if (cnt == 0)
            {
                animation.Play("A_SeaChest_Open");
                cnt++;
            }
        }
        else if (!StorageManager.Instance.storageUIOpen)
        {
            if (cnt == 1)
            {
                animation.Play("A_SeaChest_Close");
                cnt = 0;
            }
        }
    }
}
