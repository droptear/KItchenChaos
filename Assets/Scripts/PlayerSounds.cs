using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = .1f;
    private float volume = 1.0f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;

       if (footstepTimer <= 0)
       {
            footstepTimer = footstepTimerMax;

            if (player.IsWalking())
            {
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume);
            }
       }
    }
}
