using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    public float smooth = 3f;       // カメラモーションのスムーズ化用変数
    private Transform standardPos;          // the usual position for the camera, specified by a transform in the game
    private Transform frontPos;         // Front Camera locater
    private Transform jumpPos;          // Jump Camera locater

    [HideInInspector] public Transform eventPos;
    [HideInInspector] public Transform eventLookPos;
    [HideInInspector] public bool isEvent = false;
    [HideInInspector] public bool isClear = false;
    [HideInInspector] public bool isHit = false;
    [SerializeField] private LayerMask wallLayer;
    private Vector3 rayRoot;

    // スムーズに繋がない時（クイック切り替え）用のブーリアンフラグ
    private bool bQuickSwitch = false;  //Change Camera Position Quickly

    [Header("Input")]
    public string horizontalInput = "Player1_CameraHorizontal";
    public string verticalInput = "Player1_CameraVertical";
    public string frontCameraInput = "Player1_CameraFront";
    public string upCameraInput = "Player1_CameraUp";

    void Start()
    {
        // 各参照の初期化
        standardPos = GameObject.Find("CamPos").transform;

        if (GameObject.Find("FrontPos"))
            frontPos = GameObject.Find("FrontPos").transform;

        if (GameObject.Find("JumpPos"))
            jumpPos = GameObject.Find("JumpPos").transform;

        //カメラをスタートする
        transform.position = standardPos.position;
        transform.forward = standardPos.forward;
    }

    void FixedUpdate()  // このカメラ切り替えはFixedUpdate()内でないと正常に動かない
    {

        if (Input.GetButton(frontCameraInput))
        {   // q or joystick button 5
            // Change Front Camera
            setCameraPositionFrontView();
        }
        else if (Input.GetButton(upCameraInput))
        {   // e or joystick button 4
            // Change Jump Camera
            setCameraPositionJumpView();
        }
        else
        {
            // return the camera to standard position and direction
            setCameraPositionNormalView();
        }

        if (isEvent)
        {
            setCameraPositionEventView();
        }
        if (isClear)
        {
            setCameraPositionFrontView();
        }

        if (isEvent || isClear) return;

        if (isHit)
        {
            rayRoot = new Vector3(player.position.x, player.position.y + 0.08f, player.position.z);
            //　キャラクターとカメラの間に障害物があったら障害物の位置にカメラを移動させる
            if (Physics.Linecast(rayRoot, transform.position, out RaycastHit hitinfo, wallLayer))
            {
                transform.position = hitinfo.point;
            }
        }

    }

    void setCameraPositionNormalView()
    {
        if (bQuickSwitch == false)
        {
            // the camera to standard position and direction
            transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.fixedDeltaTime * smooth);
            transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
        }
        else
        {
            // the camera to standard position and direction / Quick Change
            transform.position = standardPos.position;
            transform.forward = standardPos.forward;
            bQuickSwitch = false;
        }
    }

    void setCameraPositionFrontView()
    {
        // Change Front Camera
        bQuickSwitch = true;
        transform.position = frontPos.position;
        transform.forward = frontPos.forward;
    }

    void setCameraPositionJumpView()
    {
        // Change Jump Camera
        bQuickSwitch = false;
        transform.position = Vector3.Lerp(transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);
        transform.forward = Vector3.Lerp(transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);
    }

    void setCameraPositionEventView()
    {
        transform.LookAt(eventLookPos);
        bQuickSwitch = false;
        transform.position = Vector3.Lerp(transform.position, eventPos.position, Time.fixedDeltaTime * smooth);
        transform.forward = Vector3.Lerp(transform.forward, eventPos.forward, Time.fixedDeltaTime * smooth);
    }
}
