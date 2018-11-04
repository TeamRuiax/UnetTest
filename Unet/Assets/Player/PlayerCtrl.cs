using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class PlayerCtrl : NetworkBehaviour
{
    private Animator playerAC;//플레이어 에니메이션(임시)

    public static Transform PlayerTransform; // 플레이어 좌표 
   
    public float MovementSpeed = 10f; // 무브 속도
    public bool RotationSmooth = true; // 회전 감도
    public Transform WeaponPivot; // 무기 좌표
    public Transform PlayerModel; // 플레이어 모델링
    public Transform Cam;
    public CameraF CamQ;
	//태그
    public string NoAimOffsetTag = "NoAimOffset";
    public string WallTag = "Wall";
    public string EnemyTag = "Enemy";
    public string PlayerTag = "Player";
    Rigidbody playerBody;
    Transform playerTransform;
    Vector3 CamPivot = new Vector3(0, 0, 0);
    Quaternion RealCam;
    void Start ()
	{
		init ();
	}

    void init()
    {
        //(임시)
        //CamQ = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraF>();
        playerAC = GetComponent<Animator>();
        //
        playerBody = GetComponent<Rigidbody>();
        playerTransform = transform;

        PlayerTransform = transform;
    }
	
    void dieEventHandler()
    {
        enabled = false;
    }
    
	void FixedUpdate ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        movePlayer();
        rotatePlayer();
        //rotateWeaponPivot();
	}
    
    void movePlayer()
    {
        CamPivot = new Vector3(0, 0, 0);

        
        //(임시) wasd를 입력받고 있을 시 애니메이션 true false
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        //    playerAC.SetBool("PlayerMove", true);
        //else
        //    playerAC.SetBool("PlayerMove", false);
        //

        
        
        
        //playerBody.MovePosition(playerBody.position + CamPivot.normalized * MovementSpeed  * Time.fixedDeltaTime);
        Vector3 movementAmount = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movementAmount = Vector3.ClampMagnitude(movementAmount, 1f); // 속도제한 (스피드값 따로 곱할것)
       // RealCam=Quaternion.LookRotation(Cam.forward);
        //Vector3 RealMove = RealCam * movementAmount;
        //Debug.Log(RealMove);
        float speedMultiplier = 1f;

        float angle = Vector3.Angle(movementAmount, AimController.GetAimDirection());

        //각도별 속도
        speedMultiplier = (1f - angle / 360f);
        //Debug.Log (speedMultiplier);
        playerBody.MovePosition(playerBody.position + movementAmount * MovementSpeed * speedMultiplier * Time.fixedDeltaTime);
    }

    void rotatePlayer()
    {
        //플레이어 회전 (리지드바디 이용)
        Quaternion rotation = Quaternion.FromToRotation(playerTransform.forward, (AimController.GetAimRealPosition() - playerTransform.position).normalized) * playerBody.rotation;


        // 회전 감도
        if (RotationSmooth)
        {
            playerBody.MoveRotation(Quaternion.Slerp(playerBody.rotation, Quaternion.Euler(rotation.eulerAngles.y * Vector3.up), Time.fixedDeltaTime * 20f));
        }
        else
        {
            playerBody.MoveRotation(Quaternion.Euler(rotation.eulerAngles.y * Vector3.up));
        }
    }

    void rotateWeaponPivot()
    {
        float aimY = AimController.GetAimRealPosition().y; // 에임 y 좌표
        float playerY = playerTransform.position.y; // 플레이어 y 좌표
        float targetY = AimController.GetTargetPosition().y; // 타겟의 y좌표 

        float yDist = Mathf.Abs(playerY - targetY);//타겟과의 높이차이
        Vector3 targetAimPoint = AimController.GetAimRealPosition();//실제 에임 좌표


        // 허공 보정 (물론 허공에 플레인 만들어야됨)
        if (AimController.TargetTag == NoAimOffsetTag)
        {
            targetAimPoint.y = playerY;
        }

        // 벽 조준 보정
        if (AimController.TargetTag == WallTag)
        {
            yDist = Mathf.Abs(playerY - aimY);
            //Debug.Log(yDist);
            if (yDist > 0.3f)
            {
                //targetAimPoint.y = aimY;
                targetAimPoint.y = playerY;
            }
            else
            {
                //Debug.Log("응안된다");
                targetAimPoint.y = playerY;
            }
        }
        //플레이어 보정
        //if (AimController.TargetTag == PlayerTag)
        //{

        //}
        // 적 조준 보정
        if (AimController.TargetTag == EnemyTag)
        {
            //if (yDist > 0.3f)
            //{
            Debug.Log("적 조준중");
            targetAimPoint.y = aimY;
            //        }
            //        else
            //        {

            //targetAimPoint.y = targetY;
            //        }
        }
        // 단위 사원수만이 회전가능
        Quaternion rotation = Quaternion.FromToRotation(WeaponPivot.forward, (targetAimPoint - WeaponPivot.position).normalized) * WeaponPivot.rotation; // 무기방향벡터 에임방향벡터의 각도 * 무기회전 값만큼 증가
        WeaponPivot.rotation = Quaternion.Euler((Vector2)rotation.eulerAngles); // 오일러각 회전 제한 (z축)

    }

    //host만 발동함
    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraF>().setTarget(gameObject.transform);
    }
}
