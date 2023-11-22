using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerNetworkBehave : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    private Movement playerMovement;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            PlayerInit();
        }
    }

    public void PlayerInit()
    {
        playerMovement = GetComponent<Movement>();
        //playerMovement.networkID = Random.Range(0,9999);
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = transform.position;
    }

    
    void Update()
    {
        //transform.position = Position.Value;
    }
}
