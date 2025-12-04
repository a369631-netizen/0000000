using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int scoreOnHit = 10; // q’e‚É“–‚½‚Á‚½‚ç‰ÁZ‚·‚éƒXƒRƒA

    // ’e‘¤‚Í Collider2D ‚ğ "Is Trigger" ‚É‚·‚éê‡‚Í‚±‚¿‚ç
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            ScoreManager.Add(scoreOnHit);
            Destroy(other.gameObject); // ’e‚àÁ‚·ê‡
            Destroy(gameObject);       // “G‚ğÁ‚·
        }
    }

    // ’e‘¤‚ª Trigger ‚Å‚È‚¢ê‡‚Í‚±‚¿‚ç‚ğg‚¤
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            ScoreManager.Add(scoreOnHit);
            Destroy(collision.collider.gameObject); // ’e‚àÁ‚·ê‡
            Destroy(gameObject);                    // “G‚ğÁ‚·
        }
    }
}