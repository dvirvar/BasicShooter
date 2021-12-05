
[System.Serializable]
public struct BulletInfo
{
    public WeaponID weaponID;
    public string ownerID;
    public int damage;

    public BulletInfo(WeaponID weaponID, string ownerID, int damage)
    {
        this.weaponID = weaponID;
        this.ownerID = ownerID;
        this.damage = damage;
    }
}
