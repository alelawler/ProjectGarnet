using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats myInstance;

    public int maxLife = 200;
    public int currentLife = 150;

    public int maxArmor = 200;
    public int currentArmor = 100;

    public WeaponSlot weaponSlot;

    //Armas
    //weapon1 = rifle
    //public bool weapon1On = false;
    //public bool weapon1Equipped = false;

    //Habilidades
    public bool hasDoubleJump = false;

    //Con esto lo podemos tirar en la scena y configurar los valores por ahi. 
    // Para llamarlo en cualquier lado solo tenemos que poner PlayerStats.myInstance.NOMBREPROPIEDAD y ahi hacemos lo que queremos

    // Start is called before the first frame update
    private void Awake()
    {
        if (myInstance == null)
        {
            myInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        weaponSlot = GetComponent<WeaponSlot>();
    }

    public void Damage(int damageAmount, Transform enemyPosition = null)
    {
        if (currentArmor > 0)
        {
            currentArmor -= damageAmount * 2;
            if (currentArmor <= 0)
            {
                currentArmor = 0;
            }
        }
        else
        {
            currentArmor = 0;

            if (currentLife > 0)
            {
                currentLife -= damageAmount;
                if (currentLife <= 0)
                {
                    currentLife = 0;
                    GetComponent<PlayerCharacterController>().state = PlayerCharacterController.State.Dead;
                }
            }
        }

        RegisterDamageIndicator(enemyPosition);
    }

    public void HealHp(int healAmount)
    {
        if (currentLife < maxLife)
        {
            currentLife += healAmount;
        }

        if (currentLife > maxLife)
        {
            currentLife = maxLife;
        }
    }
    public void HealArmor(int healAmount)
    {
        if (currentArmor < maxArmor)
        {
            currentArmor += healAmount;
        }

        if (currentArmor > maxArmor)
        {
            currentArmor = maxArmor;
        }
    }
    public void AddAmmoAmount(int amount)
    {
        if (weaponSlot.currentWeapon.totalCurrentAmmo < weaponSlot.currentWeapon.totalCurrentAmmoMax)
        {
            weaponSlot.currentWeapon.totalCurrentAmmo += amount;
        }

        if (weaponSlot.currentWeapon.totalCurrentAmmo > weaponSlot.currentWeapon.totalCurrentAmmoMax)
        {
            weaponSlot.currentWeapon.totalCurrentAmmo = weaponSlot.currentWeapon.totalCurrentAmmoMax;
        }
    }

    public void Reload()
    {
        int bulletDifferential = weaponSlot.currentWeapon.maxClipAmmo - weaponSlot.currentWeapon.currentAmmo;

        if(weaponSlot.currentWeapon.totalCurrentAmmo > 0)
        {
            weaponSlot.currentWeapon.totalCurrentAmmo -= bulletDifferential;
            weaponSlot.currentWeapon.currentAmmo = weaponSlot.currentWeapon.maxClipAmmo;
        }
        
        if(weaponSlot.currentWeapon.totalCurrentAmmo <= 0)
        {
            weaponSlot.currentWeapon.totalCurrentAmmo = 0;
            weaponSlot.currentWeapon.currentAmmo = 0;
        }
    }

    public void GetDoubleJump()
    {
        hasDoubleJump = true;
    }

    private void RegisterDamageIndicator(Transform enemyPosition)
    {
        if (enemyPosition != null)
        {
            if (!DI_System.CheckIfObjectInSight(enemyPosition))
            {
                DI_System.CreateIndicator(enemyPosition);
            }
        }
    }
}
