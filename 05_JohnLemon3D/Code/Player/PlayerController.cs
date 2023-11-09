using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerState
{
	void EnterState(PlayerController player);
	void FixedUpdate(PlayerController player);
	void OnCollisionEnter(PlayerController player, Collision collision);
	void ExitState(PlayerController player);
}

public class PlayerController : MonoBehaviour
{
	//public Transform cam;
	public Transform orientation;

	public IPlayerState currentState { get; private set; }
	public IPlayerState previousState { get; private set; }
	public Animator animator { get; private set; }
	public Rigidbody rb { get; private set; }
	public RunState runState = new RunState();
	public IdleState idleState = new IdleState();
	public AudioSource audioSource;
	public AudioClip footstepSound;

	public float speed = 7f;
	public int hasKey = 0;


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		animator = GetComponent<Animator>();
		//cam = Camera.main.transform;

		currentState = idleState;
	}

	void Start()
	{
		currentState.EnterState(this);
	}

	private void FixedUpdate()
	{
		currentState?.FixedUpdate(this);
	}

	public void TransitionToState(IPlayerState newState)
	{
		currentState?.ExitState(this);
		previousState = currentState;
		currentState = newState;
		currentState.EnterState(this);
	}
}
