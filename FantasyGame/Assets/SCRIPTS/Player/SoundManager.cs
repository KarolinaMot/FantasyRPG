using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
	[Header("Locomotion Sounds")]
	public AudioSource running;
	public AudioSource walking;
	public AudioSource sprinting;
	public AudioSource dash;

	[Header("Weapon Woosh")]
	public AudioSource[] wooshs;
	public AudioSource hit;
	private ThirdPersonController thirdPersonController;
	private Animator anim;
	private CombatManager combatManager;

	private void Awake()
    {
		thirdPersonController = GetComponent<ThirdPersonController>();	
		combatManager = GetComponent<CombatManager>();
		anim = GetComponent<Animator>();
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
				dash.Stop();
				Debug.Log("Running is playing");
			}
			else if (thirdPersonController.targetSpeed == thirdPersonController.sprintSpeed && !sprinting.isPlaying)
			{
				running.Stop();
				walking.Stop();
				sprinting.Play();
				dash.Stop();
				Debug.Log("Sprinting is playing");
			}
			else if (thirdPersonController.targetSpeed == thirdPersonController.moveSpeed / 2 && !walking.isPlaying)
			{
				running.Stop();
				walking.Play();
				sprinting.Stop();
				dash.Stop();

				Debug.Log("Walking is playing");
			}
			else if(thirdPersonController.targetSpeed == thirdPersonController.dashSpeed && !dash.isPlaying){
				running.Stop();
				walking.Stop();
				sprinting.Stop();
				Debug.Log("Walking is playing");
				dash.Play();
			}
		}
        else
        {
			Debug.Log("Nothing is playing");
			StopAllMovingSounds();
        }
		
    }

	public void PlayWoosh(int index)
    {
		wooshs[index].Play();
    }

	private void StopAllMovingSounds()
    {
		running.Stop();
		walking.Stop();
		sprinting.Stop();
	}
}

