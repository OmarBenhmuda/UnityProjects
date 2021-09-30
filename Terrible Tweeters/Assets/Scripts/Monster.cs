using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]

public class Monster : MonoBehaviour
{

    [SerializeField] Sprite _deadSprite;
    [SerializeField] ParticleSystem _particle;
    private bool _hasDied;


    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (ShouldDieFromCollision(collision))
        {
            StartCoroutine(Die());
        }


    }

    private bool ShouldDieFromCollision(Collision2D collision)
    {

        if (_hasDied)
            return false;

        Bird bird = collision.gameObject.GetComponent<Bird>();

        if (bird != null)
            return true;

        if (collision.contacts[0].normal.y < -0.5)
            return true;

        
        if (collision.gameObject.tag == "Ground")
            return true;

        return false;
    }

    private IEnumerator Die()
    {

        _hasDied = true;
        GetComponent<SpriteRenderer>().sprite = _deadSprite;
        _particle.Play();

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
