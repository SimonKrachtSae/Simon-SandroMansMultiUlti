using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class NPC : EntityBase
{
	private NPCstate npcState;
	private ViewCone viewCone;
	private EntityBase targetPlayer;
	private GameManager gameManager;
	private NavMeshAgent agent;
	private Vector3 targetPos;
	[SerializeField] private Vector3 bulletDir;
	public bool destroy;

	private void Start()
	{
		viewCone = GetComponent<ViewCone>();
		agent = GetComponent<NavMeshAgent>();
		npcState = NPCstate.Wander;
		gameManager = GameUI_Manager.Instance.GameManager;
		SetNewTargetPosition();
	}
	
	private void Update()
	{
		if (destroy)
		{
			GameUI_Manager.Instance.GameManager.EntityDead(ID);
			PhotonNetwork.Destroy(this.gameObject);
		}

		bulletDir = transform.forward;
		CheckForPlayersInRange();

		switch (npcState)
		{
			case NPCstate.Wander:
				Wander();
				break;

			case NPCstate.Shoot:
				Debug.Log("Shooting...");
				Shoot();
				break;

			case NPCstate.Chase:
				Debug.Log("Chasing...");
				Chase();
				break;
		}
	}

	private void Wander()
	{
		agent.stoppingDistance = 3;
		float distance = (targetPos - transform.position).magnitude;
		if ((distance - 0.15f) <= agent.stoppingDistance + agent.radius || targetPos == null)
		{
			SetNewTargetPosition();
		}

		agent.SetDestination(targetPos);
	}

	private void Chase()
	{
		agent.stoppingDistance = 20;
		agent.SetDestination(targetPlayer.transform.position);
	}

	private void Shoot()
	{
		GameObject _bullet = PhotonNetwork.Instantiate("Bullet", gunPoint.transform.position, Quaternion.identity);

		_bullet.GetComponent<Rigidbody>().velocity = bulletDir.normalized * ShootSpeed;
		_bullet.GetComponent<Bullet>().SetPlayer(ID);
	}

	private void SetNewTargetPosition()
	{
		MyRoom room = MyRoom.Instance;
		float rndX = Random.Range(room.transform.position.x - room.HalfXScale, room.transform.position.x + room.HalfXScale);
		float rndZ = Random.Range(room.transform.position.z - room.HalfZScale, room.transform.position.z + room.HalfZScale);

		targetPos = new Vector3(rndX, 0, rndZ);
	}

	private void CheckForPlayersInRange()
	{
		for (int i = 0; i < gameManager.activePlayers.Count; i++)
		{
			if ((gameManager.activePlayers[i].transform.position - transform.position).magnitude < 30)
			{
				if (gameManager.activePlayers[i].Team != Team)
				{
					targetPlayer = gameManager.activePlayers[i];
					viewCone.TargetObject = targetPlayer.gameObject;
					return;
				}
			}
		}
		viewCone.TargetObject = null;
		targetPlayer = null;
	}

	public void SetNPCState(NPCstate state)
	{
		if (state != npcState)
		{
			npcState = state;
		}

	}

	public NPCstate GetCurrentState()
	{
		return npcState;
	}

	void RotateTowardsPosition(Vector3 target)
	{
		if (target == null)
			return;

		Vector3 targ = target;
		targ.z = 0f;
		Vector3 objectPos = transform.position;
		targ.x = targ.x - objectPos.x;
		targ.y = targ.y - objectPos.y;
		float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	public void RotateToward(Vector3 target, float speed)
	{
		Vector3 from = transform.up;
		Vector3 to = target - transform.position;

		float angle = Vector3.SignedAngle(from, to, transform.forward) * Time.fixedDeltaTime * speed;
		transform.Rotate(0.0f, 0.0f, angle);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(targetPos, 0.4f);
	}
}