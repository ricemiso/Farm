//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Tutorial_Spawn : MonoBehaviour
//{
//	private GameObject itemToAdd;
//	// Start is called before the first frame update
//	void Start()
//	{
//		// Treeのリソースをロード
//		SetName("Tree");
//		if (itemToAdd == null)
//		{
//			Debug.LogError("Treeのリソースが見つかりません！");
//			return;
//		}

//		// Treeを生成
//		for (int i = 0; i < 2; i++)
//		{
//			Vector3 spawnPosition = new Vector3(15.0f, 0.0f, 46.0f + i * 10.0f);
//			GameObject newTree = Instantiate(itemToAdd, spawnPosition, Quaternion.identity);
//			newTree.name = "Tree_" + i;  // 名前に連番を追加
//		}

//	}
//	// Update is called once per frame
//	void Update()
//	{}

//	private GameObject SetName(string gameObject)
//	{
//		return itemToAdd = Resources.Load<GameObject>(gameObject);
//	}
//}

using System.Collections;
using UnityEngine;

public class Tutorial_Spawn : MonoBehaviour
{
	private GameObject itemToAdd; // スポーンさせるGameObject（Enemy用）
	public float spawnInterval = 5.0f; // Enemyのスポーン間隔
	private int enemyCount = 0; // Enemyの生成数カウント
	public float spawnRange = 10.0f; // プレイヤー周辺のスポーン範囲
	public Transform playerTransform; // プレイヤーのTransform

	public bool hasSpawnStarted = false; // コルーチンが開始されたかのフラグ
	public bool isManaHaving;
	public bool isEnemyPresent;
	public ItemCounter manaCount;

	void Start()
	{
		isManaHaving = true;
		isEnemyPresent = false;

		// プレイヤーのTransformを自動的に取得
		if (playerTransform == null)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player"); // "Player" タグを使ってプレイヤーを検索
			if (player != null)
			{
				playerTransform = player.transform; // プレイヤーのTransformを設定
			}
			else
			{
				Debug.LogError("プレイヤーが見つかりません。");
			}
		}

		// 初期状態ではスポーンを開始しない
		hasSpawnStarted = false;
	}

	void Update()
	{
		// マナの存在を確認
		if (manaCount.counter > 0)
		{
			isManaHaving = true;
		}
		else
		{
			isManaHaving = false;
		}

		// シーン上のEnemyオブジェクト数を確認
		isEnemyPresent = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;

		// 条件: マナが0で敵が存在しない場合のみスポーンを開始
		if (!isManaHaving && !isEnemyPresent && !hasSpawnStarted)
		{
			Debug.Log("マナが存在しないため、Enemyのスポーンを開始します。");

			SetName("goblin"); // goblinのリソースをロード
			if (itemToAdd == null)
			{
				Debug.LogError("goblinのリソースが見つかりません！ Resourcesフォルダに存在するか確認してください。");
				return;
			}

			// 初めてスポーン処理を開始する
			hasSpawnStarted = true;
			StartCoroutine(SpawnEnemyRoutine());
		}
		// 条件: マナが存在するか、敵がシーン上にいる場合はスポーン停止
		else if ((isManaHaving || isEnemyPresent) && hasSpawnStarted)
		{
			Debug.Log("条件が満たされなくなったため、Enemyのスポーンを停止します。");
			StopAllCoroutines();
			hasSpawnStarted = false; // スポーン停止フラグをリセット
		}
	}

	// 敵を一定間隔でスポーンするコルーチン
	IEnumerator SpawnEnemyRoutine()
	{
		// スポーン処理開始後に永遠にループする
		while (true)
		{
			// プレイヤーの周辺でランダムにスポーン位置を決定
			Vector3 spawnPosition = new Vector3(
				playerTransform.position.x + Random.Range(-spawnRange, spawnRange),
				0.0f,
				playerTransform.position.z + Random.Range(-spawnRange, spawnRange)
			);

			// Enemyを生成
			GameObject newEnemy = Instantiate(itemToAdd, spawnPosition, Quaternion.identity);
			newEnemy.name = "Enemy_" + enemyCount; // 名前に連番を追加
			newEnemy.tag = "Enemy"; // タグを明示的に設定
			enemyCount++;

			yield return new WaitForSeconds(spawnInterval); // 一定間隔を待機
		}
	}

	// Resourcesから指定した名前のGameObjectをロード
	private void SetName(string gameObjectName)
	{
		itemToAdd = Resources.Load<GameObject>(gameObjectName);
	}
}
