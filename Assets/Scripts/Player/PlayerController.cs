using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;   // Input Action에서 입력한 값 받아오기
    public LayerMask groundLayerMask;   // ground의 layer

    [Header("Look")]
    public Transform cameraContainer;   // 카메라 회전
    public float minXLook;  // 회전범위 최소
    public float maxXLook;  // 회전범위 최대
    private float camCurXRot;   // Input Action에서 받아오는 마우스의 delta값
    public float lookSensitivity;  // 회전의 민감도
    private Vector2 mouseDelta;     // 마우스의 delta값


    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 게임 시작 중에 마우스 커서 안보이게
    }
    // 물리연산은 FixedUpdate에서 호출
    void FixedUpdate()
    {
        Move();
    }
    // 카메라연산은 LateUpdate에서 호출
    private void LateUpdate()
    {
        CameraLook();
    }
    // 실제 이동
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y; // 점프를 했을 때에만 위아래로 움직여야 하므로 y방향 속도 초기화

        _rigidbody.velocity = dir;
    }
    // 카메라 회전
    void CameraLook()
    {
        // 카메라 상하 회전
        camCurXRot += mouseDelta.y * lookSensitivity;   /// 상하회전을 하기 위해서는 y값을 x에 넣는다
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        // 카메라 회전각에 로컬좌표를 줘야하는 이유는 플레이어가 기준이 되기 때문
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);  /// -를 하는 이유: 마우스를 내리면 아래로 회전하게 만들기 위해

        // 카메라 좌우 회전
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); /// 좌우회전을 하기 위해서는 x값을 y에 넣는다
    }
    // 입력 이벤트 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)  // 키가 계속 눌리는 동안
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)    // 키를 뗐을 때
        {
            curMovementInput = Vector2.zero;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();  // 마우스 값은 입력하지 않아도 계속 유지된다
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())  // 키 누르기 시작할때
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);  // 점프는 순간적으로 올라가야하므로 impurse 
        }
    }
    // 아래로 ray를 쏘아 땅에 닿았는지 확인(중복점프를 방지)
    bool IsGrounded()
    {
        // 플레이어 기준 책상다리 4개 만든다
        Ray[] rays = new Ray[4]
        {
            // 플레이어보다 (transform.up * 0.01f) 위에서 쏘는 이유:
            // 플레이어에서 쏘면, 플레이어가 땅에 부딪힌 경우 ground에서 쏠 수가 있어서, ground를 인지 못하는 경우가 발생

            // z축(forward) 앞,뒤 약간 떨어진 곳에서 아래방향으로 발사
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(-transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // x축(right) 앞,뒤 약간 떨어진 곳에서 아래방향으로 발사
            new Ray(transform.right + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(-transform.right + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
        // 위의 모든 ray를 검출
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                // 4개의 ray중에서 하나라도 ground의 layer 검출되었다면
                return true;
            }
        }
        return false;
    }
}
