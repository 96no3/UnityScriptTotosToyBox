using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SoundManager sound;
    [SerializeField] private GameObject meshObject;

    [HideInInspector] public TotoFace face;
    [HideInInspector] public UIMgr ui;
    private ThirdPersonCamera cameraObject;         // メインカメラへの参照
    private float keyTimer = 0;
    private bool iskeyUi = false;
    [HideInInspector] public bool onUFO = false;
    [HideInInspector] public bool onUp = false;
    private Vector3 goalPlayerPos;

    public enum PlayerState
    {
        Stay,
        Run,
        Clear,
        Goal
    }
    [HideInInspector] public PlayerState playerState = PlayerState.Stay;

    private bool isRun = false;
    private bool isAnim = false;
    private bool isGoal = false;
    private float effectTime = 0;
    private float confettiTime = 0;
    private float smokeTime = 0;

    [Header("parameter")]
    public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
    [SerializeField] private float upForceMover = 2.0f;
    [SerializeField] private float upForceEnemy = 2.4f;
    [SerializeField] private float enemyForce = 2.0f;
    [SerializeField] private float knockbackForce = 0.2f;

    private float useCurvesHeight = 0.1f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）
    private bool isJump = false;                 // 追加：ジャンプして最大高さに至るまでがtrue
    private bool isHighJump = false;

    // 以下キャラクターコントローラ用パラメタ
    // 前進速度
    public float forwardSpeed = 0.7f;
    // 後退速度
    public float backwardSpeed = 0.2f;
    // 旋回速度
    public float rotateSpeed = 2.0f;
    //ジャンプの高さ
    public float normalJumpHeight = 3.0f;
    public float highJumpHeight = 6.0f;
    //最大高さ到達までの時間
    public float jumpTime = 0.3f;

    private float jumpHeight;
    private float elapsed;     //滞空時間
    private float radian;      //時間当たりの角度

    [HideInInspector] public Rigidbody rb;
    // キャラクターの移動量
    private Vector3 velocity;    

    private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
    private AnimatorStateInfo currentBaseState;     // base layerで使われる、アニメーターの現在の状態の参照

    // アニメーター各ステートへの参照
    private int idleState = Animator.StringToHash("Base Layer.idle");
    private int runState = Animator.StringToHash("Base Layer.run");
    private int jumpState = Animator.StringToHash("Base Layer.jump");
    private int damageState = Animator.StringToHash("Base Layer.damage");
    private int runBackState = Animator.StringToHash("Base Layer.runBack");
    private int clearState = Animator.StringToHash("Base Layer.clear");
    private int shakeState = Animator.StringToHash("Base Layer.shake");

    [Header("Effect")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject smokeFX;
    [SerializeField] private GameObject confettiFX;

    private AudioSource aud;
    [Header("SoundSE")]
    public AudioClip seClear;
    public AudioClip seWalk;
    public AudioClip seHit;
    public AudioClip sejump;
    public AudioClip seSyabonHit;

    [Header("Input")]
    public string horizontalInput = "Player1_Horizontal";
    public string verticalInput = "Player1_Vertical";
    public string jumpInput = "Player1_Jump";
    public string cameraHorizontalInput = "Player1_CameraHorizontal";

    public void ResetPlayer()
    {
        playerState = PlayerState.Stay;
        onUFO = false;
        onUp = false;
        isRun = false;
        isJump = false;
        isHighJump = false;
        isAnim = false;
        isGoal = false;
        effectTime = 0;
        confettiTime = 0;
        smokeTime = 0;
        anim.SetBool("jump", false);
        anim.SetTrigger("Reset");
        anim.SetFloat("Speed", 0);
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        meshObject.transform.rotation = Quaternion.identity;
    }

    // 初期化
    public void Start()
    {
        face = GetComponent<TotoFace>();
        aud = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();        
        hitFX.SetActive(false);
        smokeFX.SetActive(false);
        confettiFX.SetActive(false);
        //メインカメラを取得する
        cameraObject = GameObject.FindWithTag("MainCamera").GetComponent<ThirdPersonCamera>();
        ui = GameObject.FindWithTag("UIMgr").GetComponent<UIMgr>();
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        ResetPlayer();
    }

    private void Update()
    {
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        if (hitFX.activeInHierarchy)
        {
            effectTime += Time.deltaTime;
            if (effectTime > 1.0f)
            {
                hitFX.SetActive(false);
                effectTime = 0;
            }
        }

        if (smokeFX.activeInHierarchy)
        {
            smokeTime += Time.deltaTime;
            if (smokeTime > 1.5f)
            {
                smokeFX.SetActive(false);
                smokeTime = 0;
            }
        }

        if (confettiFX.activeInHierarchy)
        {
            confettiTime += Time.deltaTime;
            if (confettiTime > 1.0f)
            {
                confettiFX.SetActive(false);
                confettiTime = 0;
                cameraObject.isClear = false;
                playerState = PlayerState.Run;
            }
        }

        switch (playerState)
        {
            case PlayerState.Stay:
                UpdateStay();
                break;
            case PlayerState.Run:
                UpdateRun();
                break;
            case PlayerState.Clear:
                UpdateClear();
                break;
            case PlayerState.Goal:
                UpdateGoal();
                break;
            default:
                break;
        }
    }

    // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
    void FixedUpdate()
    {
        if (!isRun || playerState == PlayerState.Clear || isGoal) return;

        if (rb.velocity.y < -0.1f)
        {
            anim.SetBool("jump", true);
        }

        float h = Input.GetAxis(horizontalInput);              // 入力デバイスの水平軸をhで定義
        float v = Input.GetAxis(verticalInput);                // 入力デバイスの垂直軸をvで定義
        float h2 = Input.GetAxis(cameraHorizontalInput);

        currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        //rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする

        if(!isJump || currentBaseState.fullPathHash != damageState)
        {
            anim.SetFloat("Speed", v);

            //以下、キャラクターの移動処理
            velocity = new Vector3(0, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得
                                                    // キャラクターのローカル空間での方向に変換
            velocity = transform.TransformDirection(velocity);
            //以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
            if (v > 0.1)
            {
                velocity *= forwardSpeed;       // 移動速度を掛ける
                KeyUiReset();
            }
            else if (v < -0.1)
            {
                velocity *= backwardSpeed;  // 移動速度を掛ける
                KeyUiReset();
            }
        }        

        if (Input.GetButtonDown(jumpInput) && !onUFO && !onUp && !isGoal)
        {   // スペースキーを入力したら            

            //アニメーションのステートがrunまたはidleのときジャンプできる
            if (currentBaseState.fullPathHash != jumpState && currentBaseState.fullPathHash != damageState &&
                currentBaseState.fullPathHash != clearState && currentBaseState.fullPathHash != shakeState)
            {
                //ステート遷移中でなかったらジャンプできる
                if (!anim.IsInTransition(0))
                {
                    KeyUiReset();
                    aud.PlayOneShot(sejump);
                    //velocity.y = jumpPower;
                    anim.SetBool("jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                    isJump = true;
                    rb.useGravity = false;
                    elapsed = 0.0f;
                    radian = Mathf.PI / jumpTime;

                    if (isHighJump)
                    {
                        jumpHeight = highJumpHeight;
                    }
                    else
                    {
                        jumpHeight = normalJumpHeight;
                    }
                }
            }
        }

        //ジャンプ処理(Sin関数利用)
        if (isJump)
        {
            anim.SetBool("jump", true);     // Animatorにジャンプに切り替えるフラグを送る
            elapsed += Time.fixedDeltaTime;

            velocity.y = (jumpHeight / 2) * Mathf.Sin(elapsed * radian);

            if (elapsed >= jumpTime)
            {
                isJump = false;
                rb.useGravity = true;
                elapsed = 0.0f;
            }
        }

        // 上下のキー入力でキャラクターを移動させる
        rb.position += velocity * Time.fixedDeltaTime;

        // 左右のキー入力でキャラクタをY軸で旋回させる
        transform.Rotate(0, h * rotateSpeed, 0);
        transform.Rotate(0, h2 * rotateSpeed, 0);

        //// Rigidbodyコンポーネントの関数を使って物理的に場所を移動
        //rb.MovePosition(transform.position + velocity);

        //// JUMP中の処理
        //// 現在のベースレイヤーがidleState以外の時
        if (currentBaseState.fullPathHash != idleState && currentBaseState.fullPathHash != clearState && currentBaseState.fullPathHash != shakeState)
        {
            if (!anim.IsInTransition(0))
            {
                // レイキャストをキャラクターのセンターから落とす
                Ray ray = new Ray(transform.position + Vector3.up * 0.07f, Vector3.down);
                RaycastHit hitInfo = new RaycastHit();
                // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                if (Physics.Raycast(ray, out hitInfo/*, groundLayer*/))
                {
                    //Debug.Log(hitInfo.distance);
                    if (hitInfo.distance <= useCurvesHeight)
                    {
                        // Jump bool値をリセットする		
                        anim.SetBool("jump", false);
                        isHighJump = false;
                    }
                }
            }
        }

    }

    private void UpdateStay()
    {
        if (aud.isPlaying)
        {
            aud.Stop();
        }
        if(currentBaseState.fullPathHash != idleState && !anim.IsInTransition(0))
        {            
            anim.SetBool("jump", false);
            anim.SetFloat("Speed", 0);
            if (!isAnim)
            {
                anim.SetTrigger("Reset");
                isAnim = true;
            }            
        }        
        rb.velocity = Vector3.zero;
        
        if (isRun)
        {
            isRun = false;
        }
    }

    private void UpdateRun()
    {
        keyTimer += Time.deltaTime;
        if (!iskeyUi && keyTimer > 2.0f)
        {
            ui.KeyImageIn();
            iskeyUi = true;
        }

        if (!isRun)
        {
            isRun = true;
        }
    }
    
    private void UpdateClear()
    {
        confettiFX.SetActive(true);
        cameraObject.isClear = true;
        face.ChangeFace(1);
        playerState = PlayerState.Stay;
    }

    private void UpdateGoal()
    {
        rb.position = goalPlayerPos;

        if (isRun)
        {
            isRun = false;
            meshObject.transform.rotation = Quaternion.AngleAxis(180.0f, Vector3.up);
            anim.SetTrigger("Clear");
            anim.SetFloat("Speed", 0);
            anim.SetBool("jump", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mover" || other.tag == "Enemy")
        {
            if (other.tag == "Mover")
            {
                rb.AddForce(Vector3.up * upForceMover, ForceMode.Impulse);
            }
            StartCoroutine("Hit");
        }

        if (other.tag == "syabon")
        {
            aud.PlayOneShot(seSyabonHit);
            rb.AddForce(other.transform.forward * knockbackForce, ForceMode.VelocityChange);
            anim.SetTrigger("damage");
        }

        if (other.tag == "EnemyHand")
        {
            Vector3 vec = new Vector3(other.transform.forward.x, upForceEnemy, other.transform.forward.z);
            rb.AddForce(vec * enemyForce, ForceMode.Impulse);
            StartCoroutine("Hit");
        }

        if (other.tag == "Goal")
        {
            goalPlayerPos = other.gameObject.GetComponent<Goal>().playerGoalPos.transform.position;
            //aud.PlayOneShot(seClear);
            sound.PlaySE(SoundManager.Sound.CLEAR);
            face.ChangeFace(1);
            confettiFX.SetActive(true);
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            isGoal = true;
            rb.rotation = Quaternion.identity;
            meshObject.transform.rotation = Quaternion.identity;
            playerState = PlayerState.Goal;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "HighJump")
        {            
            isHighJump = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HighJump")
        {
            isHighJump = false;
        }
    }

    private void KeyUiReset()
    {
        if (iskeyUi)
        {
            ui.KeyImageOut();
            iskeyUi = false;
        }
        keyTimer = 0;
    }

    public void PlayWalkSe()
    {
        aud.PlayOneShot(seWalk, 0.1f);
    }

    public void SetJump(bool flag)
    {
        isJump = flag;
    }

    public void ChangeShakeAnim()
    {
        anim.SetTrigger("Shake");
    }

        
    IEnumerator Hit()
    {
        yield return null;
        aud.PlayOneShot(seHit);
        effectTime = 0;
        hitFX.SetActive(true);        
        anim.SetTrigger("damage");
        yield return null;
        smokeTime = 0;
        smokeFX.SetActive(true);
        anim.SetBool("jump", true);
        yield return null;
    }
}
