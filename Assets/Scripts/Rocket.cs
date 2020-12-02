using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    [SerializeField] float levelLoadDelay = 1f;

    bool isTransitioning = false;
    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        
        if(Debug.isDebugBuild)
        {
            RespondToDebugInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isTransitioning || collisionsDisabled) { return; } // ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1 >= maxScenes) ? 0 : currentSceneIndex + 1;
        
        SceneManager.LoadScene(nextSceneIndex); //todo: allow for more scenes
    }

    private void RespondToRotateInput()
    {
       

        float rotationSpeed = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rotationSpeed);
        }
    }

    private void RotateManually(float rotationSpeed)
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        transform.Rotate(Vector3.forward * rotationSpeed); // rotate left
        rigidBody.freezeRotation = false; // let physics take control of rotation
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        // Thrust
        float boostSpeed = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * boostSpeed);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        } 
    }
}
