

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public Terrain terrain;
    private TerrainData terrainData;
    private Vector3 terrainPos;
    private AudioSource currentAudioSource; // åªç›ÇÃë´âπópÇÃAudioSource

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;
    private bool isGroundedD;
    private bool isGroundedR;
    private bool isGroundedL;

    private Vector3 lastPosition;
    public bool isMoving;

    private void Start()
    {
        lastPosition = new Vector3(0f, 0f, 0f);
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // RaycastHit hitInfoD;

        // isGrounded = Physics.SphereCast(groundCheck.position, 0.1f, Vector3.down, out hitInfoD, groundDistance, groundMask);
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && move.magnitude > 0.1f)
        {
            isMoving = true;
            UpdateFootstepSound();
        }
        else
        {
            isMoving = false;
            if (currentAudioSource != null && currentAudioSource.isPlaying)
            {
                currentAudioSource.Stop();
            }
        }

        lastPosition = gameObject.transform.position;
        PlayerState.Instance.setPlayerPosition(lastPosition);
    }

    private void UpdateFootstepSound()
    {
        Vector3 playerPosition = transform.position;
        int layerIndex = GetCurrentTerrainLayer(playerPosition);
        AudioSource newAudioSource = GetFootstepSoundForLayer(layerIndex);

        if (newAudioSource != currentAudioSource)
        {
            if (currentAudioSource != null && currentAudioSource.isPlaying)
            {
                currentAudioSource.Stop();
            }

            currentAudioSource = newAudioSource;
            currentAudioSource.loop = true;
            currentAudioSource.Play();
        }
        else if (!currentAudioSource.isPlaying)
        {
            currentAudioSource.Play();
        }
    }

    private AudioSource GetFootstepSoundForLayer(int layerIndex)
    {
        AudioSource audioSource;

        switch (layerIndex)
        {
            case 0: // ëê
                audioSource = SoundManager.Instance.grassWalkSound;
                break;
            case 1: // çªóò
                audioSource = SoundManager.Instance.gravelWalkSound;
                break;
            case 2: // åÕÇÍëê
                audioSource = SoundManager.Instance.grassWalkSound;
                break;

            default:
                audioSource = SoundManager.Instance.grassWalkSound;
                break;
        }

        return audioSource;
    }

    private int GetCurrentTerrainLayer(Vector3 position)
    {
        TerrainData terrainData = terrain.terrainData;
        float[,,] splatmapData = terrainData.GetAlphamaps(
            (int)((position.x / terrainData.size.x) * terrainData.alphamapWidth),
            (int)((position.z / terrainData.size.z) * terrainData.alphamapHeight),
            1, 1);

        int maxTextureIndex = 0;
        float maxAlpha = 0f;

        for (int i = 0; i < splatmapData.GetLength(2); i++)
        {
            float alpha = splatmapData[0, 0, i];

            if (alpha > maxAlpha)
            {
                maxAlpha = alpha;
                maxTextureIndex = i;
            }
        }

        return maxTextureIndex;
    }
}