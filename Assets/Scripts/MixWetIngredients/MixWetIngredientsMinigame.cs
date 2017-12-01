using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MixWetIngredientsMinigame : MonoBehaviour {

    [Header("Minigame parameters")]
    public float timeBeforePointingHand = 1f;
    public int cracksNeeded = 1;
    public float crackForceThreshold = 0f;
    public int eggsNeeded = 2;
    public float milkNeeded = 10f;
    public float milkError = 0.1f;

    public Sprite[] eggSprites;


    [Space(25)]
    [Header("External references")]
    public PointerControl pointer;
    public Transform eggsTarget;
    public Transform bowlBorderTarget;
    public Transform hoverMarkTarget;
    public GameObject hands;
    public Animator[] handsAnimators;
    public GameObject crackedEgg;
    public EggDrag[] eggs;
    public GameObject milk;
    public JugControl jug;
    public PromptControl promptControl;
    public GameObject[] promptContents;

    private MinigameManager manager;

    //[HideInInspector]
    public float milkPoured = 0f;
    public float particleRate = 0f;
    public int particlesPoured = 0;
    private Vector2 eggPosition;
    private bool draggingPhase = true;
    private int cracks = 0;
    private bool blockCalls = false;
    private bool calledOnce = false;
    private int eggsOpened = 0;
    public int lParticles;
    public int particles;


    private bool done = false;


    private void Start() {
        manager = FindObjectOfType<MinigameManager>();

        // Disable all egg colliders
        for (int i = 0; i < eggs.Length; i++) {
            eggs[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        // Set prompt content and show it
        AkSoundEngine.SetRTPCValue("MiniGame1Finish", 60f, GameObject.FindGameObjectWithTag("MainCamera"), 500);
        promptControl.content = promptContents[0];
        promptContents[0].SetActive(true);
        promptControl.ShowPromptAfter(1, 3, StartMinigame, true);
        AkSoundEngine.PostEvent("EggPrompt", gameObject);
    }

    // When the prompt finishes, enable the first egg collider
    public void StartMinigame() {
        FindObjectOfType<InputManager>().SetMultitouch(true);
        eggPosition = eggsTarget.position;
        SetPointer(eggPosition);
        eggs[0].gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }


    private void Update() {
        /* CHEATING */
        if(Input.GetKey(KeyCode.P)) {
            PourJugContent();
        }


        // Calculate milk poured and check if its more or less than needed
        particles = particlesPoured - lParticles;
        lParticles = particlesPoured;
        particleRate = particles / Time.deltaTime;
        if (particleRate > 0) {
            milkPoured = particlesPoured / particleRate;
        }
        float yscale = Mathf.Min(jug.milkMask.localScale.y + 0.0025f * particles, jug.finalScale);
        milkPoured = yscale;
        jug.milkMask.localScale = new Vector2(1f, yscale);
        //Debug.Log(milkPoured);
        //Alters pitch of water pouring sound with milkPoured Float - may need to use different tag
        AkSoundEngine.SetRTPCValue("Milk_Capacity", milkPoured, GameObject.FindGameObjectWithTag("MainCamera"), 0);
        if (!done && milkPoured > milkNeeded - milkError && milkPoured < milkNeeded + milkError) {
            done = true;
            Debug.Log("DONE!");
            milk.GetComponent<MilkControl>().blocked = true;

            promptControl.Hide(() => {
                promptControl.content = promptContents[2];
                promptContents[1].SetActive(false);
                promptContents[2].SetActive(true);

                // Open prompt again and play animation when visible
                promptControl.Show(promptControl.PlayAnimations);
            });
        }

    }

    public void StartDraggingEgg() {
        if (draggingPhase) {
            // Set pointing hand to the bowl border
            SetPointer(bowlBorderTarget.position);
        }
    }

    public void EndDraggingEgg() {
        if (draggingPhase) {
            // Set pointing hand to the position of the eggs
            SetPointer(eggPosition);
        }
    }

    public void CrackEgg(EggDrag egg) {
        // Increase the number of cracks performed
        cracks++;

        // Change the sprite to the next state and play animation
        egg.GetComponentInChildren<SpriteRenderer>().sprite = eggSprites[Math.Min(cracks, eggSprites.Length - 1)];
        egg.GetComponentInChildren<SpriteRenderer>().transform.localEulerAngles = new Vector3(0, 0, 90); // The cracked sprite is originally rotated, so we need to correct it
        egg.GetComponent<Animator>().Play("Animation");

        // If the player performed the cracks needed, end the dragging phase
        if (cracks == cracksNeeded) {
            draggingPhase = false;
            blockCalls = true;
            egg.CancelDrag();
            egg.GetComponent<Animator>().enabled = false;
			pointer.Hide();

            // Translate and rotate the egg to the hover position
            egg.MoveAndRotateTo(hoverMarkTarget.position, 90, true, EnableCrackedEgg);
            pointer.Hide();
            AkSoundEngine.PostEvent("Egg_Tap", gameObject);
            //StartEggCrackHandsAnimation();

            // Hide the prompt and set the content once it's not visible
            promptControl.Hide(() => {
                promptControl.content = promptContents[1];
                promptContents[0].SetActive(false);
                promptContents[1].SetActive(true);

                // Open prompt again and play animation when visible
                promptControl.Show(promptControl.PlayAnimations);
            });
        }
    }

    public void EnableCrackedEgg() {
        crackedEgg.SetActive(true);
    }

    // Locate the pointing hand and show it after 'timeBeforePointingHand' seconds
    private void SetPointer(Vector3 position) {
        pointer.Hide();
        pointer.PointTo(position);
        StartCoroutine(CallAfter(timeBeforePointingHand, pointer.Show));
    }

    // This is the event called when the cracked egg is clicked
    public void SeparatingEggCompleted() {

        // Instantiate the two parts and set the Rigidbody so it falls
        foreach (Rigidbody2D sec in crackedEgg.GetComponentsInChildren<Rigidbody2D>()) {
            GameObject go = Instantiate(sec.gameObject);
            go.transform.parent = crackedEgg.transform.parent;
            go.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            go.transform.position = sec.transform.position;
            go.transform.localScale = sec.transform.localScale;
        }

        // Disable the egg
        crackedEgg.SetActive(false);

        // Check if the player opened all the eggs needed
        if ((++eggsOpened) == eggsNeeded) {
            promptControl.Hide(null);

            // Set up scene for pouring the milk
            MilkStep();
            return;
        }

        // If there are more eggs to open, prepare scene for the next egg
        NextEgg();
    }

    private void NextEgg() {
        // Reset the cracks performed
        cracks = 0;
        // Return to dragging phase
        draggingPhase = true;

        if (eggsOpened < eggsNeeded) {
            eggs[eggsOpened].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            blockCalls = false;
            SetPointer(eggPosition);

            // Hide the prompt -> change the content -> show again
            promptControl.Hide(() => {
                promptControl.content = promptContents[0];
                promptContents[1].SetActive(false);
                promptContents[0].SetActive(true);
                promptControl.Show(promptControl.PlayAnimations);
            });
        }
    }

    // Function for setting up the next step
    private void MilkStep() {
        // Enable the milk so it is interactable
        milk.GetComponent<Collider2D>().enabled = true;
        blockCalls = false;
        AkSoundEngine.PostEvent("MilkPrompt", gameObject);

        // Show hand pointing to the milk
        SetPointer(milk.transform.position);
    }

    // This function is called when the milk is clicked (from MilkControl.cs)
    public void HoverMilk() {
        pointer.Hide();
        blockCalls = true;
        AkSoundEngine.PostEvent("Milk_Pickup", gameObject);

        // Set up elements: milk and jug
        milk.GetComponent<MilkControl>().Hover();
        jug.Show();
        jug.EnableDrag();
    }

    // Hide milk while dragging jug
    public void StartDraggingJug() {
        milk.GetComponent<MilkControl>().HideMilk();
    }

    // Show the milk once the player stops dragging the jug
    public void EndDraggingJug() {
        milk.GetComponent<MilkControl>().Hover();
    }

    // Function to call an Action after given seconds
    // NOTE: if blockCalls it's set to true, the Action won't be execute
    private IEnumerator CallAfter(float seconds, Action function) {
        float start = Time.time;
        while (Time.time - start < seconds) {
            yield return false;
        }
        if (!blockCalls) {
            function();
        }
    }


    public void PourJugContent(){
        manager.ScreenFadeOut("MixBatter");
        AkSoundEngine.SetRTPCValue("MiniGame1Finish", 0f, GameObject.FindGameObjectWithTag("MainCamera"), 25);
    }
}
