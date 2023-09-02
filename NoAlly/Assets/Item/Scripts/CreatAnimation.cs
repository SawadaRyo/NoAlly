//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsOpen",true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsOpen", false);
        }
    }
}
