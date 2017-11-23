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
    public float milkError = 0.5f;

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
        eggs[0].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        for (int i = 1; i < eggs.Length; i++) {
            eggs[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        promptControl.content = promptContents[0];
        promptContents[0].SetActive(true);
        promptControl.ShowPromptAfter(1, 3, StartMinigame, true);
    }

    public void StartMinigame() {
        FindObjectOfType<InputManager>().SetMultitouch(true);
        eggPosition = eggsTarget.position;
        SetPointer(eggPosition);
        eggs[0].gameObject.GetComponent<BoxCollider2D>().enabled = true;

    }


    private void Update() {

        //Debug.Log(particlesPoured + " - " + particles);
        particles = particlesPoured - lParticles;
        lParticles = particlesPoured;
        particleRate = particles / Time.deltaTime;
        if (particleRate > 0) {
            milkPoured = particlesPoured / particleRate;
        }
        float yscale = Mathf.Min((milkPoured / milkNeeded) * jug.finalScale, jug.finalScale);
        yscale = Mathf.Max(yscale, jug.milkMask.localScale.y);
        //Debug.Log(yscale);
        yscale = Mathf.Min(jug.milkMask.localScale.y + 0.002f * particles, jug.finalScale);
        jug.milkMask.localScale = new Vector2(1f, yscale);
<<<<<<< HEAD
        //Debug.Log(milkPoured);
        //Alters pitch of water pouring sound with milkPoured Float - may need to use different tag
        AkSoundEngine.SetRTPCValue("Milk_Capacity", milkPoured, GameObject.FindGameObjectWithTag("MainCamera"), 0);
        if (!done && milkPoured > milkNeeded - milkError && milkPoured < milkNeeded + milkError){
=======
        if (!done && milkPoured > milkNeeded - milkError && milkPoured < milkNeeded + milkError) {
>>>>>>> fc94ca05b2313aca431621ea628d36c4f16146b9
            done = true;
            Debug.Log("DONE!");
        }

    }

    public void StartDraggingEgg() {
        if (draggingPhase) {
            SetPointer(bowlBorderTarget.position);
        }
    }

    public void EndDraggingEgg() {
        if (draggingPhase) {
            SetPointer(eggPosition);
        }
    }

    public void CrackEgg(EggDrag egg) {
        //Debug.Log("Detected that! " + egg.velocity);
        cracks++;
        egg.GetComponentInChildren<SpriteRenderer>().sprite = eggSprites[Math.Min(cracks, eggSprites.Length - 1)];
        egg.GetComponentInChildren<SpriteRenderer>().transform.localEulerAngles = new Vector3(0, 0, 90);

        egg.GetComponent<Animator>().Play("Animation");
        if (cracks == cracksNeeded) {
            draggingPhase = false;
            blockCalls = true;
            egg.CancelDrag();
            egg.GetComponent<Animator>().enabled = false;
            egg.MoveAndRotateTo(hoverMarkTarget.position, 90, true, EnableCrackedEgg);
            pointer.Hide();
            AkSoundEngine.PostEvent("Egg_Tap", gameObject);
            //StartEggCrackHandsAnimation();

            promptControl.Hide(() => {
                promptControl.content = promptContents[1];
                promptContents[0].SetActive(false);
                promptContents[1].SetActive(true);
                promptControl.Show(promptControl.PlayAnimations);
            });

        } else {

        }
    }

    public void EnableCrackedEgg() {
        crackedEgg.SetActive(true);
    }

    private void StartEggCrackHandsAnimation() {
        //hands.SetActive(true);
        foreach (Animator animator in handsAnimators) {
            animator.Play("Animation");
        }
    }

    private void SetPointer(Vector3 position) {
        pointer.Hide();
        pointer.PointTo(position);
        StartCoroutine(CallAfter(timeBeforePointingHand, pointer.Show));
    }

    public void SeparatingEgg(bool separating) {
        //hands.SetActive(!separating);
        if (!separating) StartEggCrackHandsAnimation();
    }

    public void SeparatingEggCompleted() {
        //hands.SetActive(false);

        foreach (Rigidbody2D sec in crackedEgg.GetComponentsInChildren<Rigidbody2D>()) {
            GameObject go = Instantiate(sec.gameObject);
            go.transform.parent = crackedEgg.transform.parent;
            go.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            go.transform.position = sec.transform.position;
            go.transform.localScale = sec.transform.localScale;
        }

        crackedEgg.SetActive(false);
        if ((++eggsOpened) == eggsNeeded) {
            promptControl.Hide(null);
            MilkStep();
            return;
        }

        NextEgg();
    }

    private void NextEgg() {
        cracks = 0;

        draggingPhase = true;
        if (eggsOpened < eggsNeeded) {
            eggs[eggsOpened].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            blockCalls = false;
            SetPointer(eggPosition);
            promptControl.Hide(() => {
                promptControl.content = promptContents[0];
                promptContents[1].SetActive(false);
                promptContents[0].SetActive(true);
                promptControl.Show(promptControl.PlayAnimations);
            });
        }
    }

    private void MilkStep() {
        milk.GetComponent<Collider2D>().enabled = true;
        blockCalls = false;
        SetPointer(milk.transform.position);
    }

    public void HoverMilk() {
        pointer.Hide();
        blockCalls = true;
        milk.GetComponent<MilkControl>().Hover();
        jug.Show();
        jug.EnableDrag();
    }

    public void StartDraggingJug() {
        milk.GetComponent<MilkControl>().HideMilk();
    }

    public void EndDraggingJug() {
        milk.GetComponent<MilkControl>().Hover();
    }

    private IEnumerator CallAfter(float seconds, Action function) {
        float start = Time.time;
        while (Time.time - start < seconds) {
            yield return false;
        }
        if (!blockCalls) {
            function();
        }
    }



}
