using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public GameObject[] campItems;
    public GameObject pinkPerson;
    public GameObject greenPerson;
    public Camera mainCamera;
    public GameObject fairyLight;
    public Animator eatCanvas;
    public Animator controlsCanvas;
    public Animator sprintControlsCanvas;
    public SpawnMonster monsterSpawner;
    public Transform monsterSpawnPoint;
    public ScreenFlashTransition screenFlash;
    public CampfireManager campfireManager;

    [Header("Sequencing")]
    public float firstWaitDuration = 6f;
    public float firstPlayerWalkDuration = 6f;
    public float firstPinkWalkDuration = 6f;
    public float firstGreenWalkDuration = 6f;

    public float secondPinkWalkDuration = 6f;
    public float secondGreenWalkDuration = 6f;

    public float finalPinkWalkDuration = 6f;
    public float finalGreenWalkDuration = 6f;

    public Vector2[] monsterSpawnPositions;
    private int spawnIndex = 0;

    public Vector2 afterPlayerTripPosition;

    public Vector2 finalPinkPosition;
    public Vector2 finalGreenPosition;

    private NPCBehavior playerNPCBehavior;
    private SceneTransManager transitionManager;
    private LevelLoader levelLoader;

    void Awake()
    {
        transitionManager = GameObject.Find("SceneTransitionManager").GetComponent<SceneTransManager>();
        levelLoader = GameObject.Find("SceneTransitionManager").GetComponent<LevelLoader>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.gameObject.GetComponent<PlayerHealth>().OnDeath.AddListener(StartPlayerDeathSequence);
        playerNPCBehavior = PlayerController.Instance.gameObject.GetComponent<NPCBehavior>();
        StartCoroutine(IntroSequence());
    }

    public void SpawnMonster()
    {
        monsterSpawner.Spawn();
        screenFlash.StartScreenFlash();
        PlayerController.Instance.gameObject.GetComponent<PlayerHealth>().StartHearts();
    }

    public IEnumerator IntroSequence()
    {
        // Initial Disable
        PlayerController.Instance.gameObject.GetComponent<PlayerHealth>().heartParent.SetActive(false);
        PlayerController.Instance.DisableControls();
        mainCamera.gameObject.GetComponent<CameraController>().DisableControls();
        DisableCamp();
        campfireManager.DisableAllCampfires();
        fairyLight.SetActive(false);

        // Player and characters enter from left
        yield return new WaitForSeconds(1f);
        pinkPerson.GetComponent<NPCBehavior>().StartWalk(1, firstPinkWalkDuration);
        greenPerson.GetComponent<NPCBehavior>().StartWalk(1, firstGreenWalkDuration);
        playerNPCBehavior.StartWalk(1, firstPlayerWalkDuration);
        yield return new WaitForSeconds(firstPinkWalkDuration);
        pinkPerson.GetComponent<NPCBehavior>().StartWalk(-1, 0.4f);
        yield return new WaitForSeconds(firstGreenWalkDuration - firstPinkWalkDuration);
        greenPerson.GetComponent<NPCBehavior>().StartWalk(-1, 0.4f);

        // Set up camp
        yield return new WaitForSeconds(2f);
        transitionManager.StartTransition(1);
        yield return new WaitForSeconds(3f);
        EnableCamp();
        transitionManager.EndTransition(1);

        // Start glow from left, friends notice, friends start walking left (stop at mushrooms)
        fairyLight.SetActive(true);
        yield return new WaitForSeconds(4f);
        pinkPerson.GetComponent<NPCBehavior>().StartWalk(1, 0.4f);
        greenPerson.GetComponent<NPCBehavior>().StartWalk(1, 0.4f);
        yield return new WaitForSeconds(2f);
        pinkPerson.GetComponent<NPCBehavior>().StartWalk(1, secondPinkWalkDuration);
        greenPerson.GetComponent<NPCBehavior>().StartWalk(1, secondGreenWalkDuration);

        // Show walk controls pop-up
        controlsCanvas.SetTrigger("ShowButton");

        // Final enable
        PlayerController.Instance.EnableControls();
        mainCamera.gameObject.GetComponent<CameraController>().EnableControls();
        // Wait and hide controls
        yield return new WaitForSeconds(5f);
        controlsCanvas.SetTrigger("HideButton");
    }

    private void DisableCamp()
    {
        foreach (GameObject item in campItems)
        {
            item.SetActive(false);
        }
    }

    private void EnableCamp()
    {
        foreach (GameObject item in campItems)
        {
            item.SetActive(true);
        }
    }

    public void FairyLightColliderHit()
    {
        fairyLight.SetActive(false);
    }

    public void StartMushroomSequence()
    {
        StartCoroutine(MushroomSequence());
    }

    public IEnumerator MushroomSequence()
    {
        // Initial Disable
        PlayerController.Instance.DisableControls();
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetBool("IsSprinting", false);
        mainCamera.gameObject.GetComponent<CameraController>().DisableControls();

        // Wait and fade out
        yield return new WaitForSeconds(2f);
        transitionManager.StartTransition(1);
        yield return new WaitForSeconds(1f);
        
        // Change to mushroom sprites
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetTrigger("Hold");
        pinkPerson.GetComponent<Animator>().SetTrigger("Hold");
        greenPerson.GetComponent<Animator>().SetTrigger("Hold");
        
        // Fade in, show "consume" canvas
        transitionManager.EndTransition(1);
        yield return new WaitForSeconds(2f);
        eatCanvas.SetTrigger("ShowButton");
    }

    public void StartEatMushroomSequence()
    {
        StartCoroutine(EatMushroomSequence());
    }

    public IEnumerator EatMushroomSequence()
    {
        eatCanvas.SetTrigger("HideButton");

        // hold for effect
        yield return new WaitForSeconds(1f);

        // Change to eat sprites
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetTrigger("Eat");
        pinkPerson.GetComponent<Animator>().SetTrigger("Eat");
        greenPerson.GetComponent<Animator>().SetTrigger("Eat");
        yield return new WaitForSeconds(2f);

        // spawn baddie
        monsterSpawnPoint.position = monsterSpawnPositions[spawnIndex];
        spawnIndex++;
        monsterSpawner.Spawn();
        screenFlash.StartScreenFlash();

        // Deactivate monster
        monsterSpawner.GetSpawnedMonster().GetComponent<FairyController>().DisableControls();

        // Show shift to sprint pop-up
        sprintControlsCanvas.SetTrigger("ShowButton");
        yield return new WaitForSeconds(2f);

        // Show player heart
        PlayerController.Instance.gameObject.GetComponent<PlayerHealth>().StartHearts();
        
        // Final enable
        PlayerController.Instance.EnableControls();
        mainCamera.gameObject.GetComponent<CameraController>().EnableControls();
        monsterSpawner.GetSpawnedMonster().GetComponent<FairyController>().EnableControls();
        
        // Wait and hide controls
        yield return new WaitForSeconds(3f);
        sprintControlsCanvas.SetTrigger("HideButton");
    }

    public void StartTripSequence()
    {
        Debug.Log("Trip sequence");
        StartCoroutine(TripSequence());
    }

    public IEnumerator TripSequence()
    {
        // Initial Disable
        PlayerController.Instance.DisableControls();
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetBool("IsSprinting", false);
        // Despawn monster
        Destroy(monsterSpawner.GetSpawnedMonster());

        PlayerController.Instance.gameObject.GetComponent<Animator>().SetTrigger("Trip");
        // Wait and fade out
        yield return new WaitForSeconds(1f);
        transitionManager.StartTransition(1);
        yield return new WaitForSeconds(1f);

        // Move player
        PlayerController.Instance.transform.position = new Vector3(afterPlayerTripPosition.x, 0, afterPlayerTripPosition.y);
        screenFlash.SetLightScene();
        PlayerController.Instance.gameObject.GetComponent<PlayerHealth>().StopHearts();
        // Move monster spawn point
        monsterSpawnPoint.position = monsterSpawnPositions[spawnIndex];
        spawnIndex++;
        // Dialogue interaction

        // Fade in
        yield return new WaitForSeconds(2f);
        transitionManager.EndTransition(1);
        yield return new WaitForSeconds(1f);
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetTrigger("GetUp");

        // Final enable
        PlayerController.Instance.EnableControls();
        mainCamera.gameObject.GetComponent<CameraController>().EnableControls();
        campfireManager.EnableAllCampfires();
    }

    public void MoveMonsterSpawner()
    {
        monsterSpawnPoint.position = monsterSpawnPositions[spawnIndex];
        spawnIndex++;
    }

    public void StartPlayerDeathSequence()
    {
        // Get respawn position
        // Set
        StartCoroutine(PlayerDeathSequence());
    }

    public IEnumerator PlayerDeathSequence()
    {
        // Initial Disable
        PlayerController.Instance.DisableControls();
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetTrigger("Trip");
        // Wait and fade out
        yield return new WaitForSeconds(1f);
        transitionManager.StartTransition(1);
        yield return new WaitForSeconds(1f);

        // Move player
        Vector2 newPosition = GetComponentInChildren<PlayerRespawnManager>().playerRespawnPoint;
        PlayerController.Instance.transform.position = new Vector3(newPosition.x, 0, newPosition.y);

        // Reset player health
        PlayerController.Instance.gameObject.GetComponent<PlayerHealth>().Respawn();

        // Fade in
        yield return new WaitForSeconds(1f);
        transitionManager.EndTransition(1);
        yield return new WaitForSeconds(1f);
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetTrigger("GetUp");

        // Final enable
        PlayerController.Instance.EnableControls();
    }

    public void StartEndSequence()
    {
        StartCoroutine(EndSequence());
    }

    public IEnumerator EndSequence()
    {
        // Initial Disable
        PlayerController.Instance.DisableControls();
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);
        PlayerController.Instance.gameObject.GetComponent<Animator>().SetBool("IsSprinting", false);
        
        // Wait and fade out
        yield return new WaitForSeconds(1f);
        transitionManager.StartTransition(1);
        yield return new WaitForSeconds(1f);

        screenFlash.SetLightScene();
        PlayerController.Instance.gameObject.GetComponent<PlayerHealth>().StopHearts();

        // Move NPCs
        pinkPerson.transform.position = new Vector3(finalPinkPosition.x, finalPinkPosition.y, 0f);
        greenPerson.transform.position = new Vector3(finalGreenPosition.x, finalGreenPosition.y, 0f);
        pinkPerson.GetComponent<SpriteRenderer>().flipX = true;
        greenPerson.GetComponent<SpriteRenderer>().flipX = true;
        PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        campfireManager.DisableAllCampfires();

        // Fade in
        yield return new WaitForSeconds(2f);
        transitionManager.EndTransition(1);
        yield return new WaitForSeconds(3f);
        // PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        levelLoader.LoadScene(0);

    }
}
