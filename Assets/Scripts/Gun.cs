using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [Header("SetInEditor")]
    [SerializeField] ParticleSystem bubbleParticles;
    [SerializeField] AudioClip bubbleSound;
    [SerializeField] AudioSource gunSound;

    [Header("Gun control is meaningless, we only offer hopes and prayers in this household")]
    [SerializeField] float shootCoolDown;
    [SerializeField] int damage;
    bool isReadyToShoot;

    [Header("Keybinds")]
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;

    GameManager managerInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isReadyToShoot = true;
        if (managerInstance == null)
            managerInstance = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        collectInput();
    }

    public int GetDamage() { return damage; }

    void collectInput()
    {
        if (managerInstance.GetPlayerHealth().GetIsDead())
            return;
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

        Instantiate(bubbleParticles, transform.position, transform.rotation); //the rotation probably isn't correct quite yet <-- Yes it is, past Norppa is a liar

        if(!gunSound.isPlaying)
            gunSound.PlayOneShot(bubbleSound);

        Vector3 _screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);

        var _ray = Camera.main.ScreenPointToRay(_screenCenter);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit))
        {
            Debug.Log("hit  " + _hit.collider.gameObject.name);
            if(_hit.collider.tag == "Enemy")
            {
                managerInstance.GetEnemyHealth(_hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject).TakeDamage(damage); //I hate this so fucking much
                Debug.Log("Hit enemy");
            }
        }
    }

    void resetShooting()
    {
        isReadyToShoot = true;
    }
}
