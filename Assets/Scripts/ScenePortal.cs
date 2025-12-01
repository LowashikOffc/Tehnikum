using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [Header("Настройки")]
    public string sceneName;
    public GameObject pressEText;

    private bool isPlayerInZone = false;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (GlobalData.isReturning)
        {
            transform.position = GlobalData.playerPosition;

            if (rb != null) rb.velocity = Vector3.zero;

            GlobalData.isReturning = false;
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                GlobalData.playerPosition = player.transform.position - player.transform.forward * 2f;
                GlobalData.isReturning = true;
            }

            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log("Можно нажать E");

            if (pressEText != null) 
                pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;

            if (pressEText != null)
                pressEText.SetActive(false);
        }
    }
}