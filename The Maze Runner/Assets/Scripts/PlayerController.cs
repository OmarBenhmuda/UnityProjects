using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{


    // UI ------------------

    [SerializeField]
    public GameObject _menu;



    public GameObject namechooser;
    public GameObject inputField;
    public GameObject nameError;


    public GameObject mazeCompleteText;


    public Text redKeyCollected;
    public Text blueKeyCollected;
    public Text greenKeyCollected;

    // ---------------------------


    private GameObject _redDoor;
    private GameObject _blueDoor;
    private GameObject _greenDoor;



    private Vector3 _velocity;

    [SerializeField] private float _rotateSpeed = 1;

    [SerializeField] private float _speed = 3;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundMask;


    private float _gravity = -9.81f;

    private bool _isPaused = false;


    private CharacterController controller;

    // Start is called before the first frame update
    Animator animator;

    private string _username;


    // Start is called before the first frame update
    private void Start()
    {
        Pause();
        _menu.SetActive(false);

        _redDoor = GameObject.FindGameObjectWithTag("redDoor");
        _blueDoor = GameObject.FindGameObjectWithTag("blueDoor");
        _greenDoor = GameObject.FindGameObjectWithTag("greenDoor");


        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }

        Move();

        Animate();


    }
    private void Move()
    {
        // Player Movement ------------------

        _isGrounded = Physics.CheckSphere(transform.position, _groundCheckDistance, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");

        float rotateY = Input.GetAxis("Horizontal");

        transform.Rotate(0, rotateY * _rotateSpeed, 0);

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        controller.SimpleMove(moveZ * _speed * forward);


        _velocity.y += _gravity * Time.deltaTime;

        // ----------------------------------

    }

    private void Animate()
    {
        // Animation ----------------
        bool movingForward = Input.GetAxis("Vertical") > 0;
        bool movingBackward = Input.GetAxis("Vertical") < 0;

        if (movingForward)
        {
            animator.SetBool("isRunningBackwards", false);
            animator.SetBool("isRunning", true);
        }
        else if (movingBackward)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningBackwards", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningBackwards", false);
        }
        // ------------------------------
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("startLine"))
        {   
            TimeController.instance.BeginTimer();
        }
        if (other.gameObject.CompareTag("redKey"))
        {
            redKeyCollected.text = "Red Key: Collected";
            Destroy(_redDoor);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("blueKey"))
        {
            blueKeyCollected.text = "Blue Key: Collected";
            Destroy(_blueDoor);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("greenKey"))
        {
            greenKeyCollected.text = "Green Key: Collected";
            Destroy(_greenDoor);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("finishLine"))
        {
            mazeCompleteText.SetActive(true);
            StartCoroutine(FinishedLevel());
            string timeScore = TimeController.instance.EndTimer();
            SaveScore(_username, timeScore);
            Destroy(other.gameObject);

        }


    }

    private void SaveScore(string name, string timeScore)
    {
        int size;
        if (!PlayerPrefs.HasKey("size"))
        {
            PlayerPrefs.SetInt("size", 0);
        }

        size = PlayerPrefs.GetInt("size");

        PlayerPrefs.SetString(size.ToString(), name + "," + timeScore);
        size += 1;
        PlayerPrefs.SetInt("size", size);


    }

    private void Resume()
    {
        Time.timeScale = 1;
        _menu.SetActive(false);
        _isPaused = false;
    }

    private void Pause()
    {
        Time.timeScale = 0;
        _menu.SetActive(true);
        _isPaused = true;
    }


    public void RestartMaze()
    {
        Resume();
        SceneManager.LoadScene("Maze");
    }

    public void ReturnToTitle()
    {
        Resume();
        SceneManager.LoadScene("Title");
    }


    public void EnterName()
    {
        _username = inputField.GetComponent<Text>().text;

        if (_username.Length < 3)
        {
            nameError.SetActive(true);
        }
        else
        {
            Resume();
            namechooser.SetActive(false);
        }

    }

    private IEnumerator FinishedLevel()
    {
        yield return new WaitForSeconds(2f);
        RestartMaze();
    }
}
