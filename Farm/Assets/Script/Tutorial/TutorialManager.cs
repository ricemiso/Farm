using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム上のチュートリアルを管理するマネージャクラス
/// </summary>
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; set; }

    // チュートリアル用UI
    protected RectTransform tutorialTextArea;
    protected Text TutorialTitle;
    protected Text TutorialText;

    public GameObject tree;
    public GameObject stone;
    public GameObject enemy;

    // チュートリアルタスク
    protected ITutorialTask currentTask;
    protected List<ITutorialTask> tutorialTask;

    // チュートリアル表示フラグ
    private bool isEnabled;

    // チュートリアルタスクの条件を満たした際の遷移用フラグ
    private bool task_executed = false;

    // チュートリアル表示時のUI移動距離
    private float fade_pos_x = 900;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // チュートリアル表示用UIのインスタンス取得
        tutorialTextArea = GameObject.Find("TutorialArea").GetComponent<RectTransform>();
        TutorialTitle = tutorialTextArea.Find("Title").GetComponent<Text>();
        TutorialText = tutorialTextArea.Find("Text").GetComponentInChildren<Text>();

        tree.SetActive(false);
        stone.SetActive(false);
        enemy.SetActive(false);

        // チュートリアルの一覧
        tutorialTask = new List<ITutorialTask>()
        {
            new MovementTask(),
            new EquipTask(),
            new AttackTask(),
            new AttackTask2(),
            new LootTask(),
            new OpenInventroyTask(),
            new HealTask(),
            new StackInventroyTask(),
            new ChoppableTreeTask(),
            new ChoppableStoneTask(),
            new OpenCraftingScreenOpenTask(),
            new CraftAllMinion(),
            new FarmTask1(),
            new FarmTask2(),
            new FarmTask3(),
            new ConstructionTask(),
            new LevelUpTask(),
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            SwitchEnabled();
        }
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