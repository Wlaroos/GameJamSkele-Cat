using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 5;
    [SerializeField] float _jumpForce = 5;
    [SerializeField] float _fallMultiplier = 2.5f;
    [SerializeField] float _lowJumpMultiplier = 2f;
    [SerializeField] float _checkGroundRadius = 0.2f;
    [SerializeField] float _rememberGroundedFor = 0.1f;
    [SerializeField] int _defaultAdditionalJumps = 0;

    private CameraShake shake;
    private Music _musicRef;

    bool _facingRight = true;
    int _additionalJumps = 0;
    float _lastTimeGrounded;
    public bool _isGrounded = false;
    public bool _isAlive = true;
    private bool _isNotTutorial = true;
    private int _space = 0;
    public bool _paused;
    private bool _tutStart = false;

    [SerializeField] Text _hudTimerText;

    [SerializeField] public int maxTimerCount = 5;
    public int timerCount = 5;
    public float timeRemaining = 1;

    [SerializeField] Transform _groundChecker;
    [SerializeField] GameObject _playerSpawn;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] ParticleSystem _bloodParticle;
    [SerializeField] ParticleSystem _boneParticle;
    [SerializeField] GameObject _tutorialCanvas;
    Rigidbody2D _rb;


    [SerializeField] int _bloodAmount = 250;
    [SerializeField] int _bloodDecrement = 100;
    public enum _stateEnum {Zero,One,Two,Three,Four}
    private _stateEnum _currentState;

    [SerializeField] AudioClip[] _explodeSFX;
    [SerializeField] AudioClip[] _jumpSFX;
    [SerializeField] AudioClip _tickSFX;
    [SerializeField] AudioClip _tickHighSFX;
    [SerializeField] AudioClip _deathSFX;
    [SerializeField] AudioClip _floatSFX;
    //[SerializeField] AudioClip _meowSFX;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _facingRight = true;
        _currentState = _stateEnum.Four;
        timerCount = maxTimerCount;

        _musicRef = FindObjectOfType<Music>();

        //Spawns respawn point at player's location when game is started
        GameObject _spawn = Instantiate(_playerSpawn, transform.position, Quaternion.identity);
        _playerSpawn = _spawn;

        //Reference for ShankeManager
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<CameraShake>();

        //Initial timer update and first tick SFX
        _hudTimerText.text = ("" + ((Mathf.Round(timeRemaining) + timerCount - 1)));
        _musicRef.PlaySound(_tickSFX);

        //Testing if this level is the tutorial
        _isNotTutorial = SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(1);

        //Full HP
        FindObjectOfType<HUDMenu>().hp = 4;
    }

    public void Update()
    {
        // No player interactivity in the tutorial
        if (_isAlive == true && _isNotTutorial == true && _paused == false)
        {
            Move();
            Jump();
            BetterJump();
            CheckIfGrounded();
        }

        if (_isNotTutorial == false)
        {
            if (_tutStart == true)
            {
                Timer();
            }
        }
        else if(_isNotTutorial == true)
        {
            Timer();
        }

        // Right click to blow up, can be done during the tutorial.
        if (Input.GetMouseButtonDown(1) && _isAlive == true && _paused == false)
        {
            _musicRef.PlaySound(_tickHighSFX);
            Explode();
        }
        
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * _speed;
        _rb.velocity = new Vector2(moveBy, _rb.velocity.y);
        if (_rb.velocity.x > 0 && !_facingRight)  // Moving right, facing left
        {
            Flip(); // Flip right
        }
        else if (_rb.velocity.x < 0 && _facingRight) // Moving left, facing right
        {
            Flip(); // Flip left
        }
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && (_isGrounded || Time.time - _lastTimeGrounded <= _rememberGroundedFor && _additionalJumps > 0))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _additionalJumps--;
            _musicRef.PlaySound(_jumpSFX[Random.Range(0, 1)]);
            _space = 1;
        }

        if ((Input.GetMouseButtonDown(0)) && (_isGrounded || Time.time - _lastTimeGrounded <= _rememberGroundedFor && _additionalJumps > 0))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _additionalJumps--;
            _musicRef.PlaySound(_jumpSFX[Random.Range(0, 1)]);
            _space = 2;
        }
    }

    void CheckIfGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(_groundChecker.position, _checkGroundRadius, _groundLayer);
        if (colliders != null)
        {
            _isGrounded = true;
            _additionalJumps = _defaultAdditionalJumps;
        }
        else
        {
            if (_isGrounded)
            {
                _lastTimeGrounded = Time.time;
            }
            _isGrounded = false;
        }
    }

    // Jump higher when holding space
    void BetterJump()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity * (_fallMultiplier - 1) * Time.deltaTime;
        }
       else if (_rb.velocity.y > 0 && !Input.GetMouseButton(0) && _space == 2)
       {
           _rb.velocity += Vector2.up * Physics2D.gravity * (_lowJumpMultiplier - 1) * Time.deltaTime;
       }
        else if (_rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && _space == 1)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Explode()
    {
        _isAlive = false;
        _musicRef.PlaySound(_floatSFX);
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CharacterAnimations>().setIsAlive(false);
        _rb.velocity = new Vector3(0, 0, 0);
        _rb.gravityScale = 0;

        // Makes the player float up for .75 seconds before exploding
        float timer = .75f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            _rb.AddForce(new Vector2(0, 50 * Time.deltaTime));
        }    
        DelayHelper.DelayAction(this, ExplodeDelay, .75f);
    }

    private void StateChange()
    {
        _currentState--;
        transform.position = _playerSpawn.transform.position;
        _rb.velocity = new Vector3(0,0,0);
        _rb.gravityScale = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        _bloodAmount -= _bloodDecrement;

        // TUTORIAL TEXT VISIBILITY BASED ON HOW MUCH HEALTH THE PLAYER HAS
        if (_currentState == _stateEnum.Three && _isNotTutorial == false)
        {
            _tutorialCanvas.gameObject.transform.Find("Tutorial01").GetComponent<Text>().enabled = (false);
            _tutorialCanvas.gameObject.transform.Find("Tutorial02").GetComponent<Text>().enabled = (true);
            _tutStart = true;
        }

        // TUTORIAL TEXT VISIBILITY BASED ON HOW MUCH HEALTH THE PLAYER HAS
        if (_currentState == _stateEnum.Two && _isNotTutorial == false)
        {
            _tutorialCanvas.gameObject.transform.Find("Tutorial02").GetComponent<Text>().enabled = (false);
            _tutorialCanvas.gameObject.transform.Find("Tutorial03").GetComponent<Text>().enabled = (true);
        }

        // Change to Skeleton animations then respawn
        if (_currentState == _stateEnum.One)
        {
            GetComponent<CharacterAnimations>().NewAnims();
            DelayHelper.DelayAction(this, Respawn, 2f);

            // TUTORIAL TEXT VISIBILITY BASED ON HOW MUCH HEALTH THE PLAYER HAS
            if (_isNotTutorial == false)
            {
                _tutorialCanvas.gameObject.transform.Find("Tutorial03").GetComponent<Text>().enabled = (false);
                _tutorialCanvas.gameObject.transform.Find("Tutorial04").GetComponent<Text>().enabled = (true);
                _tutorialCanvas.gameObject.transform.Find("Skull").GetComponent<Image>().enabled = (true);
            }
        }


        if (_currentState == _stateEnum.Zero)
        {
            // If killed in tutorial, go back to the main menu
            if (_isNotTutorial == false)
            {
                SceneManager.LoadScene(0);
            }
            // If not killed in the tutorial, continue with killing
            else if (_currentState == _stateEnum.Zero && _isNotTutorial == true)
            {
                FindObjectOfType<HUDMenu>().hp--;
                Kill();
            }
        }
        // Respawn function if the above if statements are not used
        else
        {
            FindObjectOfType<HUDMenu>().hp--;
            DelayHelper.DelayAction(this, Respawn, 2f);
        }
    }

    private void ExplodeDelay()
    {
        _musicRef.PlaySound(_explodeSFX[Random.Range(0, 2)]);
        //AudioHelper.PlayClip2D(_meowSFX, 1f);
        ParticleSystem bloodParticle = Instantiate(_bloodParticle, transform.position, Quaternion.identity);
        bloodParticle.GetComponent<BloodParticles>().SetParticleAmount(_bloodAmount);
        shake.CamShakeReverse();
        //New particles if in skeleton state
        if (_currentState == _stateEnum.One)
        {
            ParticleSystem boneParticle = Instantiate(_boneParticle, transform.position, Quaternion.identity);
        }
            StateChange();
    }

    private void Respawn()
    {
        _isAlive = true;
        GetComponent<CharacterAnimations>().setIsAlive(true);
        GetComponentInChildren<ParticleSystem>().Play();
        _rb.gravityScale = 1;
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().enabled = true;
        timerCount = maxTimerCount;
        _hudTimerText.text = ("" + ((Mathf.Round(timeRemaining) + timerCount - 1)));
    }

    private void Kill()
    {
        // Unhides the canvas UI
        GameObject.Find("CanvasMenu").GetComponent<CanvasMenu>().PlayerDeath();
        _musicRef.PlaySound(_deathSFX);
    }

    public void NextLevel()
    {
        _isAlive = false;
        _musicRef.PlaySound(_floatSFX);
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CharacterAnimations>().setIsAlive(false);
        _rb.velocity = new Vector3(0, 0, 0);
        _rb.gravityScale = 0;
        float timer = .75f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            _rb.AddForce(new Vector2(0, 50 * Time.deltaTime));
        }
    }

    private void Timer()
    {
        // Timer decrement
        if (timerCount >= 1 && _isAlive == true)
        {
            timeRemaining -= Time.deltaTime;
        }

        // Timer runs out, player explodes
        else if (timerCount < 1 && _isAlive == true)
        {
            _musicRef.PlaySound(_tickHighSFX);
            Explode();
        }
        // Reset the timer and update the timer text (timeRemaining counts for 1 second, timerCount is how many seconds)
        if (timeRemaining <= 0)
        {
            timeRemaining = 1;
            timerCount--;
            _hudTimerText.text = ("" + ((Mathf.Round(timeRemaining) + timerCount - 1)));
            // Tick sound effect and camera shake if timer isn't on the last second
            if (timerCount >= 1)
            {
                _musicRef.PlaySound(_tickSFX);
                shake.CamShake();
            }
        }
    }
}
