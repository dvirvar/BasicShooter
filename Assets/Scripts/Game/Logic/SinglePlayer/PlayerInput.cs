using UnityEngine;

/// <summary>
/// Handles player input
/// </summary>
[RequireComponent(typeof(PlayerMovement), typeof(PlayerMouseLook))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] protected LoadoutHolder loadoutHolder;
    private PlayerMouseLook playerMouseLook;
    private PlayerMovement playerMovement;
    private float horizontalLook, verticalLook;
    protected float rawXDirection, rawZDirection;
    protected bool isXDirection, isZDirection;
    private Weapon weapon;

    protected virtual void Awake()
    {
        playerMouseLook = GetComponent<PlayerMouseLook>();
        playerMovement = GetComponent<PlayerMovement>();
        loadoutHolder.onWeaponChanged += LoadoutHolder_onWeaponChanged;
    }

    private void LoadoutHolder_onWeaponChanged(Weapon obj)
    {
        this.weapon = obj;
    }

    // Update is called once per frame
    private void Update()
    {
        #region look
        horizontalLook = Input.GetAxis(StaticStrings.Input.horizontalLook);
        verticalLook = Input.GetAxis(StaticStrings.Input.verticalLook);
        
        #endregion
        #region movement
        isXDirection = Input.GetButton(StaticStrings.Input.horizontalMovement);
        isZDirection = Input.GetButton(StaticStrings.Input.verticalMovement);
        rawXDirection = Input.GetAxis(StaticStrings.Input.horizontalMovement);
        rawZDirection = Input.GetAxis(StaticStrings.Input.verticalMovement);
        
        if (!isZDirection && isXDirection)
        {
            playerMovement.rotatePlayer(rawXDirection);
        }
        
        #endregion
        #region weapon
        if (Input.GetButton(StaticStrings.Weapon.fire))
        {
            weapon?.pushTrigger();
        }
        if (Input.GetButtonDown(StaticStrings.Weapon.reload))
        {
            weapon?.reload();
        }
        if (Input.GetButtonDown(StaticStrings.Weapon.changeMode))
        {
            weapon?.switchMode();
        }
        if (Input.GetButtonDown(StaticStrings.Weapon.switchWeapon))
        {
            switchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switchWeapon(1);
        }
        #endregion
    }

    protected virtual void switchWeapon()
    {
        loadoutHolder.switchWeapon();
    }

    protected virtual void switchWeapon(int index)
    {
        loadoutHolder.switchWeapon(index);
    }

    private void LateUpdate()
    {
        if (horizontalLook != 0 || verticalLook != 0)
        {
            playerMouseLook.moveLook(horizontalLook, verticalLook);
        }
    }

    private void FixedUpdate()
    {
        if (isZDirection || isXDirection)
        {
            playerMovement.movePlayer(rawXDirection, rawZDirection);
        }
    }

    private void OnDestroy()
    {
        loadoutHolder.onWeaponChanged -= LoadoutHolder_onWeaponChanged;
    }
}
