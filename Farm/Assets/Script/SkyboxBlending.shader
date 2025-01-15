//�S���ҁ@�z�Y�W��

Shader "Custom/SkyboxTransition"
{
    // �V�F�[�_�[�̃v���p�e�B�i�O������ݒ�\�Ȓl�j
    Properties
    {
        // "_TransitionFactor" �� 0�`1�͈̔͂őJ�ڂ̓x������ݒ肷��v���p�e�B
        _TransitionFactor("Transition Factor", Range(0, 1)) = 0.0

        // "_AtmosphereTex" �́A��C�����̋�̕\�����s�����߂̃L���[�u�}�b�v
        _AtmosphereTex("Atmosphere CubeMap", Cube) = "" {}

    // "_SpaceTex" �́A�F���̋�̕\�����s�����߂̃L���[�u�}�b�v
    _SpaceTex("Space CubeMap", Cube) = "" {}
    }

        SubShader
    {
        // ���̃V�F�[�_�[�́uBackground�v�L���[�ɐݒ肳��A�ʏ�̃I�u�W�F�N�g�̕`�������A�w�i�Ƃ��ĕ`�悳���
        Tags { "Queue" = "Background" }

        Pass
        {
            // HLSL�v���O�����̊J�n
            CGPROGRAM

            // ���_�V�F�[�_�[���`
            #pragma vertex vert

            // �t���O�����g�V�F�[�_�[���`
            #pragma fragment frag

            // Unity�̋��ʃ��C�u�������C���N���[�h
            #include "UnityCG.cginc"

            // ���_�f�[�^�̍\���̒�`
            struct appdata_t
            {
                float4 vertex : POSITION; // ���_�̍��W
            };

    // ���_�V�F�[�_�[����t���O�����g�V�F�[�_�[�֓n���f�[�^
    struct v2f
    {
        float3 pos : TEXCOORD0;   // ���_��3D�ʒu�i�L���[�u�}�b�v�̃T���v�����O�Ɏg�p�j
        float4 vertex : SV_POSITION; // ���_�̃X�N���[�����W
    };

    // �v���p�e�B�Ƃ��Ē�`���ꂽ�l���g�p���邽�߂ɁA�ϐ��Ƃ��Đ錾
    float _TransitionFactor; // �J�ڌW���i0 ���Ƒ�C�A1 ���ƉF���j
    samplerCUBE _AtmosphereTex; // ��C�̃L���[�u�}�b�v
    samplerCUBE _SpaceTex; // �F���̃L���[�u�}�b�v

    // ���_�V�F�[�_�[
    v2f vert(appdata_t v)
    {
        v2f o;

        // ���_��3D���W�����̂܂܃t���O�����g�V�F�[�_�[�ɓn��
        o.pos = v.vertex.xyz;

        // ���_�̍��W���N���b�v��Ԃɕϊ��i��ʏ�̈ʒu���v�Z�j
        o.vertex = UnityObjectToClipPos(v.vertex);

        return o;
    }

   
    half4 frag(v2f i) : SV_Target
    {
        // �ύX�O�̃L���[�u�}�b�v����F���擾
        half4 atmosphereColor = texCUBE(_AtmosphereTex, i.pos);

        // �ύX��̃L���[�u�}�b�v����F���擾
        half4 spaceColor = texCUBE(_SpaceTex, i.pos);

        // _TransitionFactor �Ɋ�Â��āA2�̃L���[�u�}�b�v�̐F����`���
        // _TransitionFactor ��0�̂Ƃ��͕ύX�O�̐F�A1�̂Ƃ��͕ύX��̐F
        half4 finalColor = lerp(atmosphereColor, spaceColor, _TransitionFactor);

        // �ŏI�I�ȐF��Ԃ�
        return finalColor;
    }
    ENDCG
}
    }

        // ���̃V�F�[�_�[���g�p�ł��Ȃ������ꍇ�ɁA"Skybox/Cubemap" �V�F�[�_�[���g��
        FallBack "Skybox/Cubemap"
}
