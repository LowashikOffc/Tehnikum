using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ToggleScript : MonoBehaviour
{
    // --- Serialized References ---
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private GameObject _cursor;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _maxDistance = 4f;
    [SerializeField] private float _holdDistance = 2f;

    // --- Private Variables ---
    private GameObject _cam;
    private float scale = 1;
    private byte A = 1;

    // Sound service
    [SerializeField] private Sounds snd_;

    void Start()
    {
        _cam = Camera.main.gameObject;
    }

    void Update()
    {
        HandleCursorInteraction();
    }

    private void HandleCursorInteraction()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, 6, _targetLayer);

        if (isHit)
        {
            _text.text = hit.collider.name;
            if (A == 0) snd_.playsound_(10);
            A = 1;
            scale = 0.8f;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (hit.transform.name == "Door")
                {
                    hit.transform.GetComponent<DoorScript>().ChangeState();
                }
            }
        }
        else
        {
            if (A == 1) snd_.playsound_(10);
            A = 0;
            scale = 1;
        }

        UpdateCursorVisuals();
    }

    private void UpdateCursorVisuals()
    {
        _cursor.GetComponent<RectTransform>().localScale = Vector3.Lerp(_cursor.GetComponent<RectTransform>().localScale, new Vector3(scale, scale, scale), Time.deltaTime * 20);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, Mathf.Lerp(_text.color.a, A, Time.deltaTime * 5));

    }
}
