using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
	[Header("State")]
	public NPCState currentState = NPCState.Idle;

	[Header("Movement")]
	public Transform[] waypoints;
	public float idleTime = 2f;
	public float workTime = 3f;
	public float restTime = 2f;

	private NavMeshAgent agent;
	private int currentWaypointIndex = 0;
	private float stateTimer;

	[Header("Visuals")]
	public Renderer capsuleRenderer;
	private Color originalColor;
	public Color workColor = Color.green;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();

		if (capsuleRenderer == null)
			capsuleRenderer = GetComponent<Renderer>();

		if (capsuleRenderer != null)
			originalColor = capsuleRenderer.material.color;
	}

	void Start()
	{
		Debug.Log("Agent on NavMesh? " + agent.isOnNavMesh);

		if (agent.isOnNavMesh)
		{
			SwitchState(NPCState.Idle);
		}
	}

	void Update()
	{
		UpdateState();
	}

	// fsm - finite state machine
	void UpdateState()
	{
		switch (currentState)
		{
			case NPCState.Idle:
				UpdateIdle();
				break;

			case NPCState.Move:
				UpdateMove();
				break;

			case NPCState.Work:
				UpdateWork();
				break;

			case NPCState.Rest:
				UpdateRest();
				break;
		}
	}

	void SwitchState(NPCState newState)
	{
		currentState = newState;
		stateTimer = 0f;

		Debug.Log($"NPC -> {newState}");

		// do only when nav mesh agent on nav mesh
		if (agent != null && agent.isOnNavMesh)
		{
			if (newState == NPCState.Move)
			{
				MoveToNextWaypoint();
			}

			if (newState == NPCState.Idle || newState == NPCState.Work || newState == NPCState.Rest)
			{
				agent.ResetPath();
			}
		}

		// color
		if (capsuleRenderer != null)
		{
			if (newState == NPCState.Work)
				capsuleRenderer.material.color = workColor;
			else
				capsuleRenderer.material.color = originalColor;
		}
	}

	// states
	void UpdateIdle()
	{
		stateTimer += Time.deltaTime;

		if (stateTimer >= idleTime)
		{
			SwitchState(NPCState.Move);
		}
	}

	void UpdateMove()
	{
		// nav mesh is not active
		if (!agent.isOnNavMesh)
			return;

		// wait path rendering
		if (agent.pathPending)
			return;

		// if complete path swap to another
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			SwitchState(NPCState.Work);
		}
	}

	void UpdateWork()
	{
		stateTimer += Time.deltaTime;

		if (stateTimer >= workTime)
		{
			SwitchState(NPCState.Rest);
		}
	}

	void UpdateRest()
	{
		stateTimer += Time.deltaTime;

		if (stateTimer >= restTime)
		{
			SwitchState(NPCState.Idle);
		}
	}

	// helpers
	void MoveToNextWaypoint()
	{
		if (waypoints == null || waypoints.Length == 0)
		{
			Debug.LogWarning("Waypoints not set!");
			return;
		}

		agent.SetDestination(waypoints[currentWaypointIndex].position);
		currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
	}
}
