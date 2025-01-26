using UnityEngine;

public class VictoryChevk : MonoBehaviour
{
    [SerializeField] BoxCollider myCollider;
    GameManager managerInstance;

    private void Start()
    {
        if (managerInstance == null)
            managerInstance = GameManager.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            managerInstance.Victory();
        }
    }
}
