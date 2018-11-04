using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class AimController : MonoBehaviour
{
    public static string TargetTag = "";
    private float groundY;
    static Vector3 aimProjection; //보정 
    static Vector3 aimRealPosition; // 실제
    static Vector3 targetPosition;// 타겟

    public float MouseSensivity = 100f; // 감도 
    public LayerMask RaycastMask;

    RectTransform aimRectTransform; 
    RectTransform parent;

    Camera playerCamera;

    Vector3 oldMousePosition;
    Vector3 RealAimPos;
    PlayerCtrl player;
    Plane groundPlane;

    void Start ()
    {
        init();
	}

    void init()
    {

        player = GetComponent<PlayerCtrl>();
        aimRectTransform = GetComponent<RectTransform>(); 
        parent = aimRectTransform.parent.parent.GetComponent<RectTransform>();
        RaycastMask = ~RaycastMask;    // Invert LayerMask
        playerCamera = Camera.main;
        Cursor.visible = false; // 마우스 커서 숨기기
    }
	
	void FixedUpdate ()
    {
        // 에임움직이기 , 에임 고정, 실제좌표값 계산
        moveAim();
        clampAim();
        calcRealPosition();
	}

    void moveAim()
    {
        aimRectTransform.position = playerCamera.WorldToScreenPoint(aimRealPosition); // 실제 좌표를 화면상 포지션에 적용
    }

    void clampAim()
    {
        float x = aimRectTransform.anchoredPosition.x;
        float y = aimRectTransform.anchoredPosition.y;
        x = Mathf.Clamp(x, -parent.sizeDelta.x / 2f, parent.sizeDelta.x / 2f);
        y = Mathf.Clamp(y, -parent.sizeDelta.y / 2f, parent.sizeDelta.y / 2f);
        aimRectTransform.anchoredPosition = new Vector2(x, y);
    }

    void calcRealPosition()
    {
 
        aimProjection = new Vector3(aimRectTransform.anchoredPosition.x, 0, aimRectTransform.anchoredPosition.y);
        aimProjection = aimProjection.normalized;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        //groundPlane = new Plane(Vector3.up, Vector3.up*PlayerCtrl.PlayerTransform.position.y);
        float rayDistance;
        if (TargetTag == "Ground")
        {
            groundPlane = new Plane(Vector3.up, Vector3.up * (groundY+ 1.0f));
            Debug.Log("땅");
            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, point, Color.red);
                aimRealPosition = point;
            }
        }
        else if(TargetTag == "None")
        {
            groundPlane = new Plane(Vector3.up, Vector3.up * PlayerCtrl.PlayerTransform.position.y);
            Debug.Log("배경");
            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, point, Color.red);
                aimRealPosition = point;
            }
        }
        else
        {
            aimRealPosition = RealAimPos;
        }
        

        

        if (Physics.Raycast(ray, out hitInfo, 100f, RaycastMask))
        {
            /*
            오토에임
            if (hitInfo.collider.tag == "Enemy")
            {
                aimRealPosition = hitInfo.collider.transform.position;
            }
            else
            {
                aimRealPosition = hitInfo.point;
            }
            */

            TargetTag = hitInfo.collider.tag; // 마우스가 가리키고 있는 타겟 태그
            targetPosition = hitInfo.collider.transform.position; // 타겟 좌표
            if(TargetTag=="Ground")
            {
                groundY = targetPosition.y;
                Debug.Log("땅");
            }
            RealAimPos = hitInfo.point; //에임 좌표
            Debug.DrawLine(playerCamera.transform.position, aimRealPosition, Color.red);
        }
    }


    //겟 
    public static Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    public static Vector3 GetAimDirection()
    {
        return aimProjection;
    }

    public static Vector3 GetAimRealPosition()
    {
        return aimRealPosition;
    }
}
