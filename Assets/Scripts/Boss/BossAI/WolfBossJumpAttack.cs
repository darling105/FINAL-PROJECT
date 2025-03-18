using UnityEngine;
using System.Collections;

public class WolfBossJumpAttack : MonoBehaviour
{
    public BossStateMachine bossStateMachine;
    public float jumpHeight = 3f; // Độ cao tối đa khi nhảy
    public float jumpDistance = 5f; // Khoảng cách nhảy về phía trước
    public float jumpDuration = 1f; // Thời gian thực hiện cú nhảy
    public Transform player; // Tham chiếu đến vị trí người chơi

    private bool isJumping = false;
    private Vector3 startJumpPos;
    private Vector3 targetJumpPos;
    private float jumpStartTime;

    //private NavMeshAgent agent;

    void Start()
    {
        //bossStateMachine = GetComponent<BossStateMachine>();
        player = bossStateMachine.PlayerPosition;
    }

    // Gọi từ Animation Event khi sói bắt đầu nhảy
    public void StartJump()
    {
        if (player == null || isJumping) return;

        isJumping = true;
        //bossStateMachine.TurnOffNavMesh();
        //agent.enabled = false; // Tắt NavMeshAgent để tự di chuyển

        // Xác định vị trí nhảy
        startJumpPos = transform.position;
        Vector3 direction = new Vector3(player.position.x - transform.position.x,0,player.position.z - transform.position.z).normalized;
        //Vector3 direction = (player.position - transform.position).normalized;
         targetJumpPos = transform.position + direction * jumpDistance;
        //targetJumpPos = player.position;

        // Ghi lại thời gian bắt đầu nhảy
        jumpStartTime = Time.time;

        // Bắt đầu coroutine di chuyển
        StartCoroutine(JumpMovement());
    }

    // Coroutine để di chuyển sói theo quỹ đạo nhảy
    IEnumerator JumpMovement()
    {
        while (Time.time - jumpStartTime < jumpDuration)
        {
            float t = (Time.time - jumpStartTime) / jumpDuration; // Tỉ lệ thời gian đã trôi qua
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight; // Mô phỏng đường cong nhảy

            // Tính toán vị trí mới theo đường cong parabol
            Vector3 newPosition = Vector3.Lerp(startJumpPos, targetJumpPos, t);
            newPosition.y += height; // Thêm độ cao vào vị trí

            transform.position = newPosition;
            yield return null;
        }

        // Kết thúc nhảy, đặt vị trí về đích
        transform.position = targetJumpPos;
        EndJump();
    }

    // Gọi từ Animation Event khi sói đáp đất
    public void EndJump()
    {
        isJumping = false;
        bossStateMachine.TurnOnNavMesh();
        //agent.enabled = true; // Bật lại NavMeshAgent để AI hoạt động bình thường
    }
}

