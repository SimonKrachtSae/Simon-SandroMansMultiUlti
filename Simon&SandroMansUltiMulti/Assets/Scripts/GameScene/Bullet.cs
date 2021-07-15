using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun, IPunObservable
{
    private int entityID;
    private float damage;
	public Team Team { get => entityID < 2 ? Team.A : Team.B; }

	public void SetPlayer(int _entityID)
    {
        entityID = _entityID;
        //punRPC comunicate onwer ID
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out EntityBase _entity))
        {
            if (_entity.Team != Team)
            {
                _entity.DealDamage(20);
            }
        }

        if(photonView.IsMine)
			PhotonNetwork.Destroy(this.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

	[PunRPC]
	public void RPC_PlayShootSound()
	{
		GameAudioManager.Instance.PlayShootSound();
	}
}
