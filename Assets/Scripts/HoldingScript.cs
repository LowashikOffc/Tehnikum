using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldingScript : MonoBehaviour
{
    // --- Serialized References ---
    public LayerMask targetLayer;
    public LayerMask excludeLayer;
    public LayerMask wallLayer; // Добавлен слой для стен
    public GameObject pickuppedObj;
    public GameObject dragObject;
    public GameObject cursor;
    public TMP_Text text;
    public float maxDistance = 4f; // Максимальное расстояние удержания
    public float holdDistance = 2f; // Желаемое расстояние удержания
    public float smoothSpeed = 20f; // Плавность перемещения
    public float rotationSpeed = 5f; // Скорость вращения
    public float collisionOffset = 0.5f; // Отступ от стен

    // --- Private Variables ---
    private GameObject cam;
    private float scale = 1;
    private byte A = 1;
    private float currentHoldDistance; // Текущее расстояние удержания

    // Sound service
    public Sounds snd_;

    void Start()
    {
        cam = Camera.main.gameObject;
        currentHoldDistance = holdDistance;
    }

    void Update()
    {
        HandleCursorInteraction();
        HandlePickupAndDrop();
        UpdateDraggedObject();
    }

    private void HandleCursorInteraction()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 6, targetLayer);

        if (isHit && !pickuppedObj)
        {
            text.text = hit.collider.name;
            if (A == 0) snd_.playsound_(10);
            A = 1;
            scale = 0.8f;
        }
        else if (!pickuppedObj)
        {
            if (A == 1) snd_.playsound_(10);
            A = 0;
            scale = 1;
        }

        UpdateCursorVisuals();
        Debug.DrawLine(cam.transform.position, cam.transform.forward * 6, Color.green);
    }

    private void UpdateCursorVisuals()
    {
        cursor.GetComponent<RectTransform>().localScale = Vector3.Lerp(cursor.GetComponent<RectTransform>().localScale, new Vector3(scale, scale, scale), Time.deltaTime * 20);
        text.color = new Color(1, 1, 1, Mathf.Lerp(text.color.a, cursor.GetComponent<Image>().color.a * A, Time.deltaTime * 5));
    }

    private void HandlePickupAndDrop()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 6, targetLayer);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!pickuppedObj && isHit)
            {
                PickupObject(hit.collider.gameObject);
                currentHoldDistance = holdDistance; // Сброс расстояния при поднятии
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (pickuppedObj != null) DropObject();
        }
    }

    private void PickupObject(GameObject obj)
    {
        pickuppedObj = obj;

        Rigidbody rb = pickuppedObj.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 10;
        rb.freezeRotation = true;
        rb.excludeLayers = excludeLayer;

        dragObject.transform.position = cam.transform.position + cam.transform.forward * currentHoldDistance;
        dragObject.transform.localScale = pickuppedObj.transform.localScale;
        dragObject.GetComponent<Rigidbody>().mass = rb.mass;
    }

    private void DropObject()
    {

        Rigidbody rb = pickuppedObj.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.drag = 0;
        rb.freezeRotation = false;
        rb.excludeLayers = 4;

        pickuppedObj = null;
    }

    private void UpdateDraggedObject()
    {
        if (!pickuppedObj) return;

        // Проверка на столкновение с препятствиями
        RaycastHit hit;
        Vector3 desiredPosition = cam.transform.position + cam.transform.forward * currentHoldDistance;

        // Проверяем, есть ли препятствие между камерой и желаемой позицией
        if (Physics.Linecast(cam.transform.position, desiredPosition, out hit, wallLayer))
        {
            // Если есть препятствие, уменьшаем расстояние удержания
            currentHoldDistance = Mathf.Clamp(hit.distance - collisionOffset, 0.5f, maxDistance);
        }
        else
        {
            // Плавно возвращаемся к желаемому расстоянию, если нет препятствий
            currentHoldDistance = Mathf.Lerp(currentHoldDistance, holdDistance, Time.deltaTime * 2f);
        }

        // Обновляем целевую позицию с учетом возможного столкновения
        Vector3 targetPos = cam.transform.position + cam.transform.forward * currentHoldDistance;

        // Плавное перемещение dragObject
        dragObject.transform.position = Vector3.Lerp(
            dragObject.transform.position,
            targetPos,
            Time.deltaTime * smoothSpeed);

        // Плавное перемещение и вращение поднятого объекта
        pickuppedObj.transform.position = Vector3.Lerp(
            pickuppedObj.transform.position,
            dragObject.transform.position,
            Time.deltaTime * smoothSpeed);

        pickuppedObj.transform.rotation = Quaternion.Slerp(
            pickuppedObj.transform.rotation,
            cam.transform.rotation,
            Time.deltaTime * rotationSpeed);
    }
}