using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterAnimations : MonoBehaviour
{

    private Animator anim;
    public AnimatorOverrideController skeleIdle;
    public AnimatorOverrideController skeleWalk;
    public AnimatorOverrideController skeleFall;
    [SerializeField] GameObject _playerRef;

    private void Start()
    {
        _playerRef = gameObject;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0) && _playerRef.GetComponent<Rigidbody2D>().velocity.x != 0 && _playerRef.GetComponent<PlayerController>()._isGrounded == true)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if(_playerRef.GetComponent<Rigidbody2D>().velocity.y != 0 && _playerRef.GetComponent<PlayerController>()._isGrounded == false)
        {
            anim.SetBool("JumpFall", true);
        }
        else
        {
            anim.SetBool("JumpFall", false);
        }
    }

    public void NewAnims()
    {
        GetComponent<Animator>().runtimeAnimatorController = skeleIdle as RuntimeAnimatorController;
        GetComponent<Animator>().runtimeAnimatorController = skeleFall as RuntimeAnimatorController;
        GetComponent<Animator>().runtimeAnimatorController = skeleWalk as RuntimeAnimatorController;
    }

    public void setIsAlive(bool _is)
    {
            anim.SetBool("isAlive", _is);
    }
}
