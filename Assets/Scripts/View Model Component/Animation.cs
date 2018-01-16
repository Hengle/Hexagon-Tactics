using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour {

    bool isWalking, isAttacking;

    public bool Walking {
        get { return isWalking; }
        set {
            if (isWalking != value) {
                anim.SetBool("walking", value);
                isWalking = value;
            }
        }
    }

    public bool Attacking {
        get { return isAttacking; }
        set {
            if (isAttacking != value) {
                anim.SetBool("attacking", value);
                isAttacking = value;
            }
        }
    }

    private Animator anim;

	// Use this for initialization
	void Awake () {
        anim = GetComponentInChildren<Animator>();
        isWalking = false;
        anim.SetBool("walking", false);
        isAttacking = false;
    }
	
	public void Attack() {
        anim.SetTrigger("Attack");
    }
}
