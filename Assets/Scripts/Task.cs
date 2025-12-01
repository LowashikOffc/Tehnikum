using System.Collections;
using UnityEngine;

public class Task : MonoBehaviour
{
    [SerializeField] private GameObject _pressEText;
    [SerializeField] private Camera _cam;

    private Vector3 _startPosition;
    private bool _isPlayerInZone = false;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (_isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                EnterTest();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInZone = true;
            Debug.Log("Можно нажать E");

            if (_pressEText != null)
                _pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInZone = false;

            if (_pressEText != null)
                _pressEText.SetActive(false);
        }
    }

    private void EnterTest()
    {
    }
}
