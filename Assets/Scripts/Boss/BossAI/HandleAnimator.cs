using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimator : MonoBehaviour
{
    private Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void PlayAnimation(string anim,float ratio = 0.2f){
        _animator.CrossFade(anim,ratio);
    }
    public bool AnimationIsPlaying(float ratio = 0.9f)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= ratio;
    }

}
