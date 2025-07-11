using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//担当者　越浦晃生

/// <summary>
/// ゲーム上のチュートリアルを管理するマネージャクラス
/// </summary>
public class TutorialManager : MonoBehaviour
{
	
	// チュートリアル用UI
	protected RectTransform tutorialTextArea;
	protected Text TutorialTitle;
	protected Text TutorialText;

	

	// チュートリアルタスク
	protected ITutorialTask currentTask;
	protected List<ITutorialTask> tutorialTask;

	// チュートリアル表示フラグ
	private bool isEnabled;

	// チュートリアルタスクの条件を満たした際の遷移用フラグ
	private bool task_executed = false;

	// チュートリアル表示時のUI移動距離
	private float fade_pos_x = 900;

	//チュートリアル用エネミー
	public GameObject enemy;
	public GameObject enemy2;
	//public bool isSpawn = false;

	void Start()
	{
		// チュートリアル表示用UIのインスタンス取得
		tutorialTextArea = GameObject.Find("TutorialArea").GetComponent<RectTransform>();
		TutorialTitle = tutorialTextArea.Find("Title").GetComponent<Text>();
		TutorialText = tutorialTextArea.Find("Text").GetComponentInChildren<Text>();

		// チュートリアルの一覧
		tutorialTask = new List<ITutorialTask>()
		{
			new MovementTask(),
			new EquipTask(),
			new infoTask(),
			//new AttackTask(),
			new OpenInventroyTask(),
			//new CraftAllMinion(),
			new FarmTask1(),
			new FarmTask2(),
			new FarmTask3(),
			new ConstructionTask(),
			new DefeatEnemy(),
			new LevelUpTask(),
			new HealTask(),
			//new StackInventroyTask(),
			new ChoppableTreeTask(),
			new ChoppableStoneTask(),
			new ManaCraftTask(),
			new ChargeTask(),
		};

		// 最初のチュートリアルを設定
		StartCoroutine(SetCurrentTask(tutorialTask.First()));

		isEnabled = true;

	}

	void Update()
	
	{
		// チュートリアルが存在し実行されていない場合に処理
		if (currentTask != null && !task_executed)
		{
			// 現在のチュートリアルが実行されたか判定
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

		if (Input.GetKeyDown(KeyCode.Y))
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
	/// 新しいチュートリアルタスクを設定する
	/// </summary>
	/// <param name="task"></param>
	/// <param name="time"></param>
	/// <returns></returns>
	protected IEnumerator SetCurrentTask(ITutorialTask task, float time = 0)
	{
		// timeが指定されている場合は待機
		yield return new WaitForSeconds(time);

		currentTask = task;
		task_executed = false;

		// UIにタイトルと説明文を設定
		TutorialTitle.text = task.GetTitle();
		TutorialText.text = task.GetText();


		task.OnTaskSetting();

		iTween.MoveTo(tutorialTextArea.gameObject, iTween.Hash(
			"position", tutorialTextArea.transform.position + new Vector3(fade_pos_x, 0, 0),
			"time", 1f
		));

		if (task.GetTitle() == "基本操作 攻撃(2/2)")
		{
			enemy.SetActive(true);
			//isSpawn = true;
		}
		if(task.GetTitle() == "敵を倒せ")
		{
			enemy2.SetActive(true);
		}

	}

	/// <summary>
	/// チュートリアルの有効・無効の切り替え
	/// </summary>
	protected void SwitchEnabled()
	{
		isEnabled = !isEnabled;

		// UIの表示切り替え
		float alpha = isEnabled ? 1f : 0;
		tutorialTextArea.GetComponent<CanvasGroup>().alpha = alpha;
	}
}