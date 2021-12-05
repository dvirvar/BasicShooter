using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the logic of weapon
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    protected AudioSource audioSource;
    protected Animator animator;
    [SerializeField]
    protected WeaponStats weaponStats;
    protected WeaponInfo weaponInfo;
    protected WeaponPartsBuilder weaponPartsBuilder;

    public int bulletsInMagazine
    {
        get => _bulletsInMagazine;
        private set
        {
            _bulletsInMagazine = value;
            bulletsInMagazineChanged(_bulletsInMagazine);
        }
    }//The bullets inside the magazine
    private int _bulletsInMagazine;
    public int bullets
    {
        get => _bullets;
        private set
        {
            _bullets = value;
            bulletsChanged(_bullets);
        }

    }//The bullets asside
    private int _bullets;
    private int currentWeaponMode = 0;
    protected float currentRecoil = 0f;
    private Quaternion recoilQuaternion => Quaternion.Euler(0f, 0f, -currentRecoil);
    private bool isFiring = false;
    private bool isReloading = false;

    public Transform fireTransform;
    public string ownerID;

    public event System.Action<int> bulletsInMagazineChanged = delegate { };
    public event System.Action<int> bulletsChanged = delegate { };
    public event System.Action<WeaponModes> weaponModeChanged = delegate { };

    protected void Awake() {
        weaponPartsBuilder = GetComponent<WeaponPartsBuilder>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audioSource.clip = weaponStats.fireSound;
        bulletsInMagazine = weaponStats.magazineCapacity;
        bullets = weaponStats.bullets;
    }

    public void init(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
        weaponPartsBuilder.setWeaponInfo(weaponInfo);
        weaponPartsBuilder.hidePartsLocations();
    }

    protected void Update()
    {
        recoil();
    }
    /// <summary>
    /// Returns to idle state
    /// </summary>
    public void resetState()
    {
        isReloading = false;
        isFiring = false;
    }

    public void pushTrigger()
    {
        if (needReload())
        {
            reload();
        } else
        {
            fire();
        } 
    }

    private bool needReload()
    {
        if (bulletsInMagazine <= 0)
        {
            return true;
        }
        return false;
    }

    private void fire()
    {
        if (isFiring || isReloading)
        {
            return;
        }
        //TODO: remove all Input references
        switch (getCurrentWeaponMode())
        {
            case WeaponModes.auto:
                StartCoroutine("autoFire");
                break;
            case WeaponModes.burst:
                if (Input.GetButtonDown(StaticStrings.Weapon.fire))
                {
                    StartCoroutine("burstFire");
                }
                break;
            case WeaponModes.single:
                if (Input.GetButtonDown(StaticStrings.Weapon.fire))
                {
                    StartCoroutine("singleFire");
                }
                break;
        }
        
    }

    public void reload()
    {
        if (isReloading)
        {
            return;
        }
        if (bulletsInMagazine != weaponStats.magazineCapacity && bullets > 0)
        {
            StartCoroutine("startReloading");
        }

    }
    
    private IEnumerator startReloading()
    {
        isReloading = true;
        int missingBullets = weaponStats.magazineCapacity - bulletsInMagazine;
        int bulletsToReload = missingBullets < bullets ? missingBullets : bullets;
        yield return new WaitForSeconds(weaponStats.reloadSpeed);
        bullets -= bulletsToReload;
        bulletsInMagazine += bulletsToReload;
        isReloading = false;
    }

    public void switchMode()
    {
        
        int weaponModesSize = weaponStats.weaponModes.Length;
        if (weaponModesSize == 1)
        {
            return;
        }

        currentWeaponMode++;
        if (currentWeaponMode >= weaponModesSize)
        {
            currentWeaponMode = 0;
        }
        weaponModeChanged(getCurrentWeaponMode());
    }

    public WeaponModes getCurrentWeaponMode()
    {
        return weaponStats.weaponModes[currentWeaponMode];
    }

    private IEnumerator singleFire()
    {
        isFiring = true;
        instantiateAndFireBullet();
        yield return new WaitForSeconds(weaponStats.fireRate * 3);
        isFiring = false;
    }

    private IEnumerator burstFire()
    {
        isFiring = true;
        instantiateAndFireBullet();
        yield return new WaitForSeconds(weaponStats.fireRate * 1.25f);
        if (bulletsInMagazine > 0)
        {
            instantiateAndFireBullet();
            yield return new WaitForSeconds(weaponStats.fireRate * 1.25f);
        }
        if (bulletsInMagazine > 0) {
            instantiateAndFireBullet();
            yield return new WaitForSeconds(weaponStats.fireRate);
        }
            
        isFiring = false;
    }

    private IEnumerator autoFire()
    {
        isFiring = true;
        instantiateAndFireBullet();
        yield return new WaitForSeconds(weaponStats.fireRate);
        isFiring = false;
    }

    private Bullet instantiateBulletWithInfo() {
        bulletsInMagazine--;
        var bullet = GameObjectsPool.instance.get(weaponStats.bulletID) as Bullet;
        bullet.transform.SetPositionAndRotation(fireTransform.position, fireTransform.rotation);
        bullet.info = new BulletInfo(weaponStats.id,ownerID, weaponStats.damage);
        return bullet;
    }

    protected virtual void fireBullet(Bullet bullet)
    {
        audioSource.Play();
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
        currentRecoil += weaponStats.recoil;
        //add firepower
        Vector3 force = fireTransform.forward * weaponStats.firePower;
        bullet.rb.AddForce(force);
    }

    private void instantiateAndFireBullet()
    {
        fireBullet(instantiateBulletWithInfo());
    }

    private void recoil()
    {
        currentRecoil = Mathf.Clamp(currentRecoil, 0f, 25f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, recoilQuaternion, 10 * Time.deltaTime);
        currentRecoil -= Random.Range(0.3f, 0.6f);
    }

    public WeaponInfo getWeaponInfo()
    {
        return weaponInfo;
    }
}
