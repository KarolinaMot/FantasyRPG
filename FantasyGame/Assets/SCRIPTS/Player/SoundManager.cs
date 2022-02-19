using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
	[Header("Locomotion Sounds")]
	public AudioSource running;
	public AudioSource walking;
	public AudioSource sprinting;

	private ThirdPersonController thirdPersonController;

	private void Awake()
    {
		thirdPersonController = GetComponent<ThirdPersonController>();	
    }
    private void Update()
    {
        PlayMovingSounds();
    }
    private void PlayMovingSounds()
    {
        if (!thirdPersonController.isJumping && thirdPersonController.targetSpeed > 0)
        {
			if (thirdPersonController.targetSpeed == thirdPersonController.moveSpeed && !running.isPlaying)
			{
				running.Play();
				walking.Stop();
				sprinting.Stop();
				Debug.Log("Running is playing");
			}
			else if (thirdPersonController.targetSpeed == thirdPersonController.sprintSpeed && !sprinting.isPlaying)
			{
				running.Stop();
				walking.Stop();
				sprinting.Play();
				Debug.Log("Sprinting is playing");
			}
			else if (thirdPersonController.targetSpeed == thirdPersonController.moveSpeed / 2 && !walking.isPlaying)
			{
				running.Stop();
				walking.Play();
				sprinting.Stop();
				Debug.Log("Walking is playing");
			}
		}
        else
        {
			Debug.Log("Nothing is playing");
			StopAllMovingSounds();
        }
		
    }

	private void StopAllMovingSounds()
    {
		running.Stop();
		walking.Stop();
		sprinting.Stop();
	}
}

