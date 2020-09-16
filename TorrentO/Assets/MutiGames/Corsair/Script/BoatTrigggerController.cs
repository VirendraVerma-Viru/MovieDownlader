
using UnityEngine;

public class BoatTrigggerController : MonoBehaviour
{
    public GameObject PlayerDestroyer;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Money")
        {

            Destroy(col.gameObject);
            FindObjectOfType<AudioManager>().Play("Coin");
            FindObjectOfType<GameController>().AddScore();
            FindObjectOfType<GameController>().UpdateUI();

        }
        else if (col.gameObject.tag == "Bomb")
        {
            FindObjectOfType<AudioManager>().Play("GameOver");
            FindObjectOfType<GameController>().GameOver();
            GameObject go = Instantiate(PlayerDestroyer, gameObject.transform.position, gameObject.transform.rotation);
            foreach (Transform child in go.transform)
            {
                child.transform.parent = null;
                child.GetComponent<Rigidbody>().AddExplosionForce(200, new Vector3(0, 0, 0), 100);
            }
            go.GetComponent<Rigidbody>().AddExplosionForce(100, Vector3.up, 5);
            Destroy(col.gameObject);
            Destroy(go);
            Destroy(gameObject);
        }
    }
}
