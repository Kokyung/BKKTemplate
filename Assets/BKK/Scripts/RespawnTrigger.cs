using UnityEngine;

namespace BKK
{
    public class RespawnTrigger : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
    
        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player") || other.collider.GetComponent<MyPlayer>())
            {
                MyPlayer.Instance.Teleport(spawnPoint);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.GetComponent<MyPlayer>())
            {
                MyPlayer.Instance.Teleport(spawnPoint);
            }
        }
    }
}
