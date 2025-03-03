using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// ī�޶󿡼� ray�� ���
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // ray�� ��� �ð�����
    private float lastCheckTime;    // ������ ray�� �� �ð�
    public float maxCheckDistance;  // �󸶳� �ָ����� üũ�Ұ��ΰ�
    public LayerMask layerMask;     // � layer�� �޷��ִ� ���ӿ�����Ʈ�� �����Ұ��ΰ�

    /// <summary>
    /// ray�� ����� ���ӿ�����Ʈ�� ������ ��´�
    /// </summary>
    public GameObject curInteractGameObject;    // ���� �����ߴٸ�, interaction�ϴ� ���ӿ�����Ʈ ������ ����
    private IInteractable curInteractable;      /// �ڰ���� ������ �������̽��� ĳ���Ѵ� 

    // ������ ������ ������ promptText�� ����
    public TextMeshProUGUI promptText;  /// �ϴ� �и������� ������, ���ΰ����Ҷ��� UI�� �и��ؼ� drag and drop ���ϰ� ����ϴ� ����� ã�Ƽ� �����丵 �غ���
    private Camera camera;  // ī�޶�

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // checkRate �������� ray�� �������Ѵ�
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // ī�޶󿡼� ȭ�� �߾ӿ� ray �߻�
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));    // ī�޶� ��� �ִ� ������ �ֱ� ������, �������� �����ָ� �ȴ�
            RaycastHit hit; // ray�� �ε��� ���ӿ�����Ʈ�� ������ ����

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // ray�� ����� ������Ʈ�� �ִ�
            {
                if (hit.collider.gameObject != curInteractGameObject)   // ray�� �浹�� ���ӿ�����Ʈ�� ���� ��ȣ�ۿ��ϴ� ���ӿ�����Ʈ�� �ƴ϶��
                {
                    curInteractGameObject = hit.collider.gameObject;    // ���ο� ������ �ٲ�
                    curInteractable = hit.collider.GetComponent<IInteractable>();       /// �ڰ���� ������ �������̽��� ĳ��
                    SetPromptText();    // promptText�� ����ض�
                }
            }
            else // ������� ray�� �� ���
            {
                // ��� ������ ���ֶ�
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }
    // promptText�� ������ ����
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);  // text�� Ȱ��ȭ
        promptText.text = curInteractable.GetInteractPrompt();  /// ���������̽����� ������ ������ ������ ���
    }
    // EŰ ������ �� ��ȣ�ۿ�
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        /// E�� ������ ��, aim�� �������� �ٶ󺸰� ���� ��(�������̽��� ĳ���ϰ� �ִ� ������ ���� ��)
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();   // ��ȣ�ۿ� ������ �κ��丮�� �̵��� �������� Destroy���� ���ش�
            // ��ȣ�ۿ��� �������� ��� null, ��Ȱ��ȭ
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
