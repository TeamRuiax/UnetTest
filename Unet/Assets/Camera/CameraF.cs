using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraF : MonoBehaviour
{
    //싱글톤
    public static CameraF CameraInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CameraF>();
            }

            return instance;
        }
    }

    static CameraF instance;

    public float FollowSpeed = 5f;
    [SerializeField] public Vector3 yOffset = new Vector3(0.0f, 18.0f, 0.0f);
    [SerializeField] public Vector3 xOffset = new Vector3(0.0f, 0.0f, -20.0f);
    //private로 보호된 변수를 inspecter창에서만 수정이 가능
    
        Transform playerTransform;
    [SerializeField]
        private float CamRotateAngle = 30; // 나중엔 유저 변경도 가능케끔 할지 고민

    Transform cameraTransform;
    Vector3 followOffset;
	Vector3 current;

    Vector3 camOffset;
    public Quaternion cam;
    public Quaternion Realcam;

    

    void Start ()
    {
        init();

    }

    void init()
    {
        //cameraTransform = transform;
        //cameraTransform.position = playerTransform.position + Quaternion.identity* xOffset + Quaternion.identity * yOffset;
        //camOffset = cameraTransform.position - playerTransform.position;
    }
	
	void FixedUpdate ()
    {
        moveCamera();
	}
    
    void moveCamera()
    {


        //      if (Input.GetKey(KeyCode.E))// 오른쪽 카메라 회전
        //      {
        //          cam = Quaternion.AngleAxis(CamRotateAngle * Time.fixedDeltaTime, Vector3.up); // y축으로 회전각도
        //          camOffset = cam * camOffset; // 타겟 사이의 거리 벡터에 회전값을 넣어준다. 그러면 타겟주위를 빙글빙글 회전 
        //          cameraTransform.RotateAround(camOffset, Vector3.up, CamRotateAngle * Time.fixedDeltaTime);//카메라 자체 회전. 이게 없으면 카메라는 정면상태로 바라본다 
        //      }
        //      if (Input.GetKey(KeyCode.Q))// 왼쪽 카메라 회전
        //      {
        //          cam = Quaternion.AngleAxis(CamRotateAngle * Time.fixedDeltaTime, Vector3.down);
        //          camOffset = cam * camOffset;
        //          cameraTransform.RotateAround(camOffset, Vector3.down, CamRotateAngle * Time.fixedDeltaTime);
        //      }

        //      Vector3 newPos = playerTransform.position + camOffset;
        //      //cameraTransform.position = Vector3.Slerp(cameraTransform.position, newPos, Time.fixedDeltaTime * FollowSpeed);

        //      current = Vector3.Lerp (AimController.GetAimRealPosition(), newPos, 0.8f); // 캐릭터와 마우스좌표의 보간 
        //cameraTransform.position = Vector3.Slerp(current, newPos, Time.fixedDeltaTime * FollowSpeed); // 카메라좌표 부드럽게

        //      //Realcam = 
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + new Vector3(0, 10, -20);
        }
    }
    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
