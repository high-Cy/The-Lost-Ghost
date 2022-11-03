using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickAnimations : MonoBehaviour
{
    private Vector3 center;
    private Animator animator;
    private float eatEndTime;
    private float eatStartTime;
    private float jumpEndTime;
    private Vector3 walkDir = Vector3.zero;
    private float turnSpeed = 0.1f;
    private float turnTime;
    private bool isChick;

    // Start is called before the first frame update
    void Start()
    {
        isChick = gameObject.tag == "Toon Chick";
        center = FindObjectOfType<ChickensGenerator>().GetCenter();
        animator = GetComponent<Animator>();
        eatStartTime = Random.Range(1f, 2f);
        eatEndTime = Random.Range(3f, 5f);
        jumpEndTime = Random.Range(10f, 20f);
        RepeatAnimations();

    }

    void FixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, center);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Walk In Place") &&
            dist < 10)
        {
            float angle = Mathf.Atan2(walkDir.x, walkDir.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSpeed, turnTime);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            transform.Translate(walkDir * Time.deltaTime, Space.World);
        } else if (dist >= 10) {
            walkDir = RandomVec();
        }

    }
    void RepeatAnimations()
    {
        StartCoroutine(PlayAnimations());
    }

    private IEnumerator PlayAnimations()
    {
        var rand = Random.Range(0f, 1f);
        if (rand > 0.6)
        {
            animator.SetBool("Eat", true);
            yield return new WaitForSeconds(eatStartTime);
            animator.SetBool("Eat", false);
            yield return new WaitForSeconds(eatEndTime);
        }
        else if (rand < 0.1)
        {
            animator.Play("Base Layer.Idle");
            yield return new WaitForSeconds(eatStartTime);

        }
        else if (rand < 0.4 && rand >= 0.1)
        {
            animator.SetBool("Walk", true);
            walkDir = RandomVec();
            transform.Translate(walkDir * Time.deltaTime, Space.World);
            yield return new WaitForSeconds(1);
            animator.SetBool("Walk", false);
        }
        else
        {
            if (isChick)
            {
                animator.Play("Base Layer.Jump W Root");
                yield return new WaitForSeconds(jumpEndTime);
            }
            else
            {
                animator.SetBool("Turn Head", true);
                yield return new WaitForSeconds(jumpEndTime);
                animator.SetBool("Turn Head", false);
            }
        }

        RepeatAnimations();
        yield return null;
    }

    private Vector3 RandomVec()
    {
        var rand = Random.Range(0, 4);
        if (rand == 0) return Vector3.back;
        else if (rand == 1) return Vector3.forward;
        else if (rand == 2) return Vector3.left;
        else return Vector3.right;
    }
}
