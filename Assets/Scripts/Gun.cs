using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("SetInEditor")]
    [SerializeField] GameObject projectile;
    [SerializeField] ParticleSystem bubbleParticles;

    [Header("Gun control is meaningless, we only offer hopes and prayers in this household")]
    [SerializeField] float shootCoolDown;
    [SerializeField] int damage;
    bool isReadyToShoot;

    [Header("Keybinds")]
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isReadyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        collectInput();
    }

    public int GetDamage() { return damage; }

    void collectInput()
    {
        if(Input.GetKey(shootKey))
        {
            shootBubbles();

            Invoke(nameof(resetShooting), shootCoolDown);
        }
    }

    void shootBubbles()
    {
        if (!isReadyToShoot)
            return;

        isReadyToShoot = false;
        Debug.Log("pew pew");
        //need to instantiate some kind of invisible projectile that causes damage as a particle system can't do that
        //also need to instantiate the particle system once that's done
    }

    void resetShooting()
    {
        isReadyToShoot = true;
    }
}
