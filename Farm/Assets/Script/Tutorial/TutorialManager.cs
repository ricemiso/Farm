using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �Q�[����̃`���[�g���A�����Ǘ�����}�l�[�W���N���X
/// </summary>
public class TutorialManager : MonoBehaviour
{
	
	// �`���[�g���A���pUI
	protected RectTransform tutorialTextArea;
	protected Text TutorialTitle;
	protected Text TutorialText;

	

	// �`���[�g���A���^�X�N
	protected ITutorialTask currentTask;
	protected List<ITutorialTask> tutorialTask;

	// �`���[�g���A���\���t���O
	private bool isEnabled;

	// �`���[�g���A���^�X�N�̏����𖞂������ۂ̑J�ڗp�t���O
	private bool task_executed = false;

	// �`���[�g���A���\������UI�ړ�����
	private float fade_pos_x = 900;

	//�`���[�g���A���p�G�l�~�[
	public GameObject enemy;
	public GameObject enemy2;
	//public bool isSpawn = false;

	void Start()
	{
		// �`���[�g���A���\���pUI�̃C���X�^���X�擾
		tutorialTextArea = GameObject.Find("TutorialArea").GetComponent<RectTransform>();
		TutorialTitle = tutorialTextArea.Find("Title").GetComponent<Text>();
		TutorialText = tutorialTextArea.Find("Text").GetComponentInChildren<Text>();

		// �`���[�g���A���̈ꗗ
		tutorialTask = new List<ITutorialTask>()
		{
			new MovementTask(),
			new EquipTask(),
			new AttackTask(),
			new OpenInventroyTask(),
			//new CraftAllMinion(),
			new FarmTask1(),
			new FarmTask2(),
			new FarmTask3(),
			new ConstructionTask(),
			new DefeatEnemy(),
			new LevelUpTask(),
			new HealTask(),
			new StackInventroyTask(),
			new ChoppableTreeTask(),
			new ChoppableStoneTask(),
			new ManaCraftTask(),
			new ChargeTask(),
		};

		// �ŏ��̃`���[�g���A����ݒ�
		StartCoroutine(SetCurrentTask(tutorialTask.First()));

		isEnabled = true;

	}

	void Update()
	
	{
		// �`���[�g���A�������݂����s����Ă��Ȃ��ꍇ�ɏ���
		if (currentTask != null && !task_executed)
		{
			// ���݂̃`���[�g���A�������s���ꂽ������
			if (currentTask.CheckTask())
			{
				task_executed = true;

				DOVirtual.DelayedCall(currentTask.GetTransitionTime(), () => {
					iTween.MoveTo(tutorialTextArea.gameObject, iTween.Hash(
						"position", tutorialTextArea.transform.position - new Vector3(fade_pos_x, 0, 0),
						"time", 1f
					));

					tutorialTask.RemoveAt(0);

					var nextTask = tutorialTask.FirstOrDefault();
					if (nextTask != null)
					{
						StartCoroutine(SetCurrentTask(nextTask, 1f));
					}
				});
			}
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			SwitchEnabled();
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			MainMenuSaveManager.Instance.StartLoadedGame("MainScene");
			GrobalState.Instance.isSkip = true;
		}

		//if (isSpawn)
		//{
		//	enemy.SetActive(true);
		//	isSpawn = false;
		//}
	}

	/// <summary>
	/// �V�����`���[�g���A���^�X�N��ݒ肷��
	/// </summary>
	/// <param name="task"></param>
	/// <param name="time"></param>
	/// <returns></returns>
	protected IEnumerator SetCurrentTask(ITutorialTask task, float time = 0)
	{
		// time���w�肳��Ă���ꍇ�͑ҋ@
		yield return new WaitForSeconds(time);

		currentTask = task;
		task_executed = false;

		// UI�Ƀ^�C�g���Ɛ�������ݒ�
		TutorialTitle.text = task.GetTitle();
		TutorialText.text = task.GetText();


		task.OnTaskSetting();

		iTween.MoveTo(tutorialTextArea.gameObject, iTween.Hash(
			"position", tutorialTextArea.transform.position + new Vector3(fade_pos_x, 0, 0),
			"time", 1f
		));

		if (task.GetTitle() == "��{���� �U��(2/2)")
		{
			enemy.SetActive(true);
			//isSpawn = true;
		}
		if(task.GetTitle() == "�G��|��")
		{
			enemy2.SetActive(true);
		}

	}

	/// <summary>
	/// �`���[�g���A���̗L���E�����̐؂�ւ�
	/// </summary>
	protected void SwitchEnabled()
	{
		isEnabled = !isEnabled;

		// UI�̕\���؂�ւ�
		float alpha = isEnabled ? 1f : 0;
		tutorialTextArea.GetComponent<CanvasGroup>().alpha = alpha;
	}
}