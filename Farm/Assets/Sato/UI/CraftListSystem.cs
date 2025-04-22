using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　佐藤貴哉

public class CraftListSystem : MonoBehaviour
{

	public GameObject craftSlot; //クラフトスロットのプレハブ
	public RectTransform contentsRect; //配置領域のRect
	public GameObject canvas; //配置領域の親オブジェクト
	public float contentsHeight; //領域の高さ
	public float changeRange = 150; //todo:定数消す

	private GameObject newSlot; //新しく生成するスロット
	private Vector3 slotPos; //スロットの位置

	// Start is called before the first frame update
	void Start()
	{
		contentsRect = gameObject.GetComponent<RectTransform>();

		contentsHeight = contentsRect.rect.bottom; //配置領域の高さを取得

		newSlot = null;
		slotPos = new Vector3(400, -100, 0); //スロットの位置を初期化
	}

	// Update is called once per frame
	void Update()
	{
		//テスト用にEnterキーを押したらクラフトスロットを生成
		if (Input.GetKeyDown(KeyCode.I))
		for(int i = 0; i < 10; i++)
		{
			//newSlot = Instantiate(craftSlot, slotPos, Quaternion.identity); //スロットを生成

			GameObject prefab = (GameObject)Instantiate(craftSlot);
			prefab.transform.SetParent(canvas.transform, false);

			prefab.transform.localPosition = slotPos; //スロットの位置を設定

			//配置領域を長くする
			contentsHeight -= changeRange;
			contentsRect.offsetMin = new Vector2(0, contentsHeight);
			contentsRect.offsetMax = new Vector2(0, 0);

			slotPos.y -= changeRange; //スロットの位置を下にずらす
		}
	}
}
