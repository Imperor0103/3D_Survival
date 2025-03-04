using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;   // Input Action���� �Է��� �� �޾ƿ���
    public LayerMask groundLayerMask;   // ground�� layer

    [Header("Look")]
    public Transform cameraContainer;   // ī�޶� ȸ��
    public float minXLook;  // ȸ������ �ּ�
    public float maxXLook;  // ȸ������ �ִ�
    private float camCurXRot;   // Input Action���� �޾ƿ��� ���콺�� delta��
    public float lookSensitivity;  // ȸ���� �ΰ���
    private Vector2 mouseDelta;     // ���콺�� delta��

    // UI ����
    public bool canLock = true;
    // ó������ �κ��丮 â�� ��Ȱ��ȭ�� ���·� �����Ѵ�
    // ��, CursorMode.Locked ����(Ŀ���� ȭ�� �߾ӿ� ����)���� ����
    // �̶� canLock�� true�� �Ѵ�

    public Action inventory;    // �κ��丮 ���� ������ Toggle �޼��带 ��Ƽ� ����

    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // ���� ���� �߿� ���콺 Ŀ�� �Ⱥ��̰�
    }
    // ���������� FixedUpdate���� ȣ��
    void FixedUpdate()
    {
        Move();
    }
    // ī�޶󿬻��� LateUpdate���� ȣ��
    private void LateUpdate()
    {
        if (canLock)
        {
            CameraLook();
        }
    }
    // ���� �̵�
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y; // ������ ���� ������ ���Ʒ��� �������� �ϹǷ� y���� �ӵ� �ʱ�ȭ

        _rigidbody.velocity = dir;
    }
    // ī�޶� ȸ��
    void CameraLook()
    {
        // ī�޶� ���� ȸ��
        camCurXRot += mouseDelta.y * lookSensitivity;   /// ����ȸ���� �ϱ� ���ؼ��� y���� x�� �ִ´�
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        // ī�޶� ȸ������ ������ǥ�� ����ϴ� ������ �÷��̾ ������ �Ǳ� ����
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);  /// -�� �ϴ� ����: ���콺�� ������ �Ʒ��� ȸ���ϰ� ����� ����

        // ī�޶� �¿� ȸ��
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); /// �¿�ȸ���� �ϱ� ���ؼ��� x���� y�� �ִ´�
    }
    // �Է� �̺�Ʈ ó��
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)  // Ű�� ��� ������ ����
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)    // Ű�� ���� ��
        {
            curMovementInput = Vector2.zero;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();  // ���콺 ���� �Է����� �ʾƵ� ��� �����ȴ�
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())  // Ű ������ �����Ҷ�
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);  // ������ ���������� �ö󰡾��ϹǷ� impurse 
        }
    }
    // �Ʒ��� ray�� ��� ���� ��Ҵ��� Ȯ��(�ߺ������� ����)
    bool IsGrounded()
    {
        // �÷��̾� ���� å��ٸ� 4�� �����
        Ray[] rays = new Ray[4]
        {
            // �÷��̾�� (transform.up * 0.01f) ������ ��� ����:
            // �÷��̾�� ���, �÷��̾ ���� �ε��� ��� ground���� �� ���� �־, ground�� ���� ���ϴ� ��찡 �߻�

            // z��(forward) ��,�� �ణ ������ ������ �Ʒ��������� �߻�
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(-transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // x��(right) ��,�� �ణ ������ ������ �Ʒ��������� �߻�
            new Ray(transform.right + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(-transform.right + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
        // ���� ��� ray�� ����
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                // 4���� ray�߿��� �ϳ��� ground�� layer ����Ǿ��ٸ�
                return true;
            }
        }
        return false;
    }
    // tabŰ ������ ������
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            /// UIInventory�� Toggle �޼��带 ����ϱ� ���� delegate�� ���
            inventory?.Invoke();    // delegate�� Toggle �޼��尡 ������ ȣ��
            ToggleCursor();
        }
    }
    /// <summary>
    /// ���������� Cursor�� Toggle���ִ� ���
    /// �κ��丮�� ���� ���� Ŀ���� ȭ�� �߾ӿ� �����Ǹ�, ������ �ʴ´�
    /// �κ��丮�� �������� ȭ���� �����ϰ�, �κ��丮�� Ŭ������ Ŀ���� ���ͼ� ȭ�� ��ü�� ������ �� �ִ�
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;    /// Locked: �κ��丮â�� ���� ������ ���� ����(Ŀ���� ȭ�� �߾ӿ� �����Ǿ��ִ�)
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        // toggle�� true: �κ��丮â�� ������ �ʾƼ� Ŀ���� ȭ�� �߾ӿ� ���� -> None���� ����, ȭ�鿡�� ������ �� �ְ� �����
        // toggle�� false: �κ��丮â�� �����ִٸ� Ŀ���� ȭ�鿡�� ������ �� �ִ� -> Locked�� ���� ȭ�� �߾ӿ� Ŀ���� �����Ѵ�

        canLock = !toggle;
        // toggle�� true: ������ Ŀ���� ȭ�鿡�� ������ �� �ְ� ��������Ƿ� canLock�� false
        // toggle�� false: ������ ȭ�� �߾ӿ� Ŀ���� ���������Ƿ� canLock�� true
    }
}
