//担当者　越浦晃生

Shader "Custom/SkyboxTransition"
{
    // シェーダーのプロパティ（外部から設定可能な値）
    Properties
    {
        // "_TransitionFactor" は 0～1の範囲で遷移の度合いを設定するプロパティ
        _TransitionFactor("Transition Factor", Range(0, 1)) = 0.0

        // "_AtmosphereTex" は、大気圏内の空の表現を行うためのキューブマップ
        _AtmosphereTex("Atmosphere CubeMap", Cube) = "" {}

    // "_SpaceTex" は、宇宙の空の表現を行うためのキューブマップ
    _SpaceTex("Space CubeMap", Cube) = "" {}
    }

        SubShader
    {
        // このシェーダーは「Background」キューに設定され、通常のオブジェクトの描画よりも後、背景として描画される
        Tags { "Queue" = "Background" }

        Pass
        {
            // HLSLプログラムの開始
            CGPROGRAM

            // 頂点シェーダーを定義
            #pragma vertex vert

            // フラグメントシェーダーを定義
            #pragma fragment frag

            // Unityの共通ライブラリをインクルード
            #include "UnityCG.cginc"

            // 頂点データの構造体定義
            struct appdata_t
            {
                float4 vertex : POSITION; // 頂点の座標
            };

    // 頂点シェーダーからフラグメントシェーダーへ渡すデータ
    struct v2f
    {
        float3 pos : TEXCOORD0;   // 頂点の3D位置（キューブマップのサンプリングに使用）
        float4 vertex : SV_POSITION; // 頂点のスクリーン座標
    };

    // プロパティとして定義された値を使用するために、変数として宣言
    float _TransitionFactor; // 遷移係数（0 だと大気、1 だと宇宙）
    samplerCUBE _AtmosphereTex; // 大気のキューブマップ
    samplerCUBE _SpaceTex; // 宇宙のキューブマップ

    // 頂点シェーダー
    v2f vert(appdata_t v)
    {
        v2f o;

        // 頂点の3D座標をそのままフラグメントシェーダーに渡す
        o.pos = v.vertex.xyz;

        // 頂点の座標をクリップ空間に変換（画面上の位置を計算）
        o.vertex = UnityObjectToClipPos(v.vertex);

        return o;
    }

   
    half4 frag(v2f i) : SV_Target
    {
        // 変更前のキューブマップから色を取得
        half4 atmosphereColor = texCUBE(_AtmosphereTex, i.pos);

        // 変更後のキューブマップから色を取得
        half4 spaceColor = texCUBE(_SpaceTex, i.pos);

        // _TransitionFactor に基づいて、2つのキューブマップの色を線形補間
        // _TransitionFactor が0のときは変更前の色、1のときは変更後の色
        half4 finalColor = lerp(atmosphereColor, spaceColor, _TransitionFactor);

        // 最終的な色を返す
        return finalColor;
    }
    ENDCG
}
    }

        // 他のシェーダーが使用できなかった場合に、"Skybox/Cubemap" シェーダーを使う
        FallBack "Skybox/Cubemap"
}
