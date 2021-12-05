using UnityEngine;
/// <summary>
/// Handles the network logic of player movement
/// </summary>
[RequireComponent(typeof(NetworkIdentity),typeof(NetworkPlayer))]
public class NetworkPlayerMovement : PlayerMovement
{    
    private NetworkPlayer player;
    private NetworkIdentity networkIdentity;
    private Vector3 previousPos, previousRot, previousCharRot;

    private void Awake()
    {
        player = GetComponent<NetworkPlayer>();
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start(); 
        if (!networkIdentity.isControlling())
        {
            enabled = false;
            return;
        }
        previousPos = transform.position;
        previousRot = transform.rotation.eulerAngles;
        previousCharRot = character.localEulerAngles;
    }

    private void Update()
    {
        if (previousPos != transform.position || previousRot != transform.eulerAngles)
        {
            networkIdentity.getSocket().Emit("updateTransform", player.getTransformToServer());
            previousPos = transform.position;
            previousRot = transform.eulerAngles;
        }
        if (previousCharRot != character.localEulerAngles)
        {
            networkIdentity.getSocket().Emit("characterRotate", player.getCharacterRotationToServer());
            previousCharRot = character.localEulerAngles;
        }
    }
}
