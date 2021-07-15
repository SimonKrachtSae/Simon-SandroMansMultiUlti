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
	private EntityBase targetEntity;
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
				RotateToward(targetEntity.transform.position);
				Shoot();
				break;

			case NPCstate.Chase:
				Debug.Log("Chasing...");
				RotateToward(targetEntity.transform.position);
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
		agent.SetDestination(targetEntity.transform.position);
	}

	private void Shoot()
	{
		StartCoroutine(NPCshoot(1));
	}

	private IEnumerator NPCshoot(float _time)
	{
		yield return new WaitForSeconds(_time);

		GameObject _bullet = PhotonNetwork.Instantiate("Bullet", gunPoint.transform.position, Quaternion.identity);

		_bullet.GetComponent<Rigidbody>().velocity = transform.forward * ShootSpeed;
		_bullet.GetComponent<Bullet>().SetPlayer(ID);
		StopAllCoroutines();
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
					targetEntity = gameManager.activePlayers[i];
					viewCone.TargetObject = targetEntity.gameObject;
					return;
				}
			}
		}
		viewCone.TargetObject = null;
		targetEntity = null;
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

	public void RotateToward(Vector3 targ)
	{
		if (targetEntity == null)
			return;

		targ.y = 0f;
		Vector3 objectPos = transform.position;
		targ.x = targ.x - objectPos.x;
		targ.z = targ.z - objectPos.z;
		float angle = Mathf.Atan2(targ.z, targ.x) * Mathf.Rad2Deg - 90;
		transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(targetPos, 0.4f);
	}
}