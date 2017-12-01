using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct State {
    public int cycles;
    public Sprite sprite;
}

[Serializable]
public struct Trafficlight {
    public SpriteRenderer red;
    public SpriteRenderer amber;
    public SpriteRenderer green;




    public void SetRed() {
        red.color = new Color(0, 0, 0, 0);
        amber.color = new Color(0, 0, 0, 0.8f);
        green.color = new Color(0, 0, 0, 0.8f);
    }
    public void SetAmber() {
        amber.color = new Color(0, 0, 0, 0);
        red.color = new Color(0, 0, 0, 0.8f);
        green.color = new Color(0, 0, 0, 0.8f);
    }
    public void SetGreen() {
        green.color = new Color(0, 0, 0, 0);
        amber.color = new Color(0, 0, 0, 0.8f);
        red.color = new Color(0, 0, 0, 0.8f);
    }
}
public class MixingControl : Interactable {

    private enum SPEED {
        NORMAL,
        FAST,
        SLOW,
        START
    }

    private SPEED speedState = SPEED.START;

    [Header("Balancing Parameters")]
    public float minCycleTime = 0.6f;
    public float maxCycleTime = 2f;
    public int cyclesToComplete = 35;

    public int pointsCorrectCycle = 2;
    public int pointsSlowCycle = 1;
    public int pointsFastCycle = 0;
    public int SCORE = 0;

    public State[] statesList;

    [Header("Dev Parameters")]
    public Text debugScoreText;
    public Transform debugMark;
    public Transform spoonTransform;
    public float multiplier = 0.02f;
    public PromptControl prompt;

    public SpriteRenderer spritesheetRenderer;
    public Sprite[] spritesheet;


    private int currentSprite;

    public Trafficlight trafficlight;


    [SerializeField]
    private Collider2D outCollider;
    [SerializeField]
    private Collider2D inCollider;

    private MinigameManager manager;


    public float angleError = 10f;
    public SpriteRenderer spriteRenderer;

    public Animator guzzlesarus;
    public Animator feedbackGuzz;

    private int cyclesCompleted = 0;
    private int stateCyclesCompleted = 0;

    private Transform spriteTransform;
    private float lx;
    private float ly;

    private bool mixing = false;
    private float speed = 0f;

    private Vector2 v0 = new Vector2();
    private Vector2 v1 = new Vector2();
    private float v_angle;
    private float v_angle_delta;
    private float s_angle = 0f;
    private bool cycleDone = false;
    private float cycleStartTime;
    private int currentState;
    private bool wasOut;

    private bool isSpoonMoving = false;

    public override void OnStart() {
        base.OnStart();
        manager = FindObjectOfType<MinigameManager>();
        currentState = 0;
        if (debugScoreText != null)
            debugScoreText.text = "SCORE: " + SCORE;
        spriteRenderer.sprite = statesList[currentState].sprite;
        spriteTransform = spoonTransform.GetComponentInChildren<SpriteRenderer>().transform;
        AkSoundEngine.PostEvent("MixTheBatter", gameObject);
        AkSoundEngine.SetRTPCValue("MiniGame2Finish", 60f, GameObject.FindGameObjectWithTag("MainCamera"), 500);
    }

    public override void OnInteractionStart(Vector3 position) {
        Vector2 touchPos = ScreenToWorldTouch(position);
        if (outCollider.OverlapPoint(touchPos) && !inCollider.OverlapPoint(touchPos)) {
            // The touch was made inside the desired area
            mixing = true;
            AkSoundEngine.PostEvent("Play_Stir", gameObject);
            lx = touchPos.x;
            ly = touchPos.y;

            v0 = new Vector2(lx, ly).normalized;

            cycleStartTime = Time.time;


        }
    }


    public override void OnInteractionHold(Vector3 position) {
        if (!mixing) {
            //AkSoundEngine.PostEvent("Pause_Stir", gameObject);
            return;
        }
        Vector2 touchPos = ScreenToWorldTouch(position);
        if (!(outCollider.OverlapPoint(touchPos) && !inCollider.OverlapPoint(touchPos))) {
            //mixing = false;
            //Debug.Log("PointOutOfBounds");

            wasOut = true;
            return;
        }

        speed = (Vector2.Distance(touchPos, new Vector2(lx, ly))) / Time.deltaTime;
        isSpoonMoving = speed > 0;
        if ((touchPos.y > 0.0 && touchPos.x < (lx - (multiplier * speed))) ||
           (touchPos.y < -0.0 && touchPos.x > (lx + (multiplier * speed))) ||
           (touchPos.x > 0.0 && touchPos.y > (ly + (multiplier * speed))) ||
           (touchPos.x < -0.0 && touchPos.y < (ly - (multiplier * speed)))) {

            //Debug.Log("Not that way! \nLast position (" + lx + ", " + ly + ")\nCurrent Position (" + touchPos.x + ", " + touchPos.y + ")\nSpeed: " + speed);
            //mixing = false;
            wasOut = true;
            return;

        }

        lx = touchPos.x;
        ly = touchPos.y;

        if (wasOut) {
            v0 = new Vector2(lx, ly).normalized;
            cycleDone = false;
            cycleStartTime = Time.time;
            wasOut = false;
        }
        debugMark.position = touchPos;

        v1 = new Vector2(lx, ly).normalized;
        float tmp_angle = Vector2.SignedAngle(v1, v0);
        if (tmp_angle < 0) {
            tmp_angle = 360 + tmp_angle;
        }
        v_angle_delta += v_angle - tmp_angle;

        v_angle = tmp_angle;


        if (Mathf.Abs(v_angle_delta) > 90) {
            v_angle_delta = 0;

            spritesheetRenderer.sprite = spritesheet[currentSprite];
            currentSprite = (currentSprite + 1) % 4 + (4 * (currentState));
        }

        float delta = -(s_angle + v_angle);
        float x = Mathf.Cos(delta * Mathf.Deg2Rad) * 3f;
        float y = Mathf.Sin(delta * Mathf.Deg2Rad) * 0.8f;

        spriteTransform.localPosition = new Vector3(x, y, 0);

        if (v_angle > (360 - angleError)) {
            if (!cycleDone) {
                cycleDone = true;
                float cycleTime = Time.time - cycleStartTime;
                cycleStartTime = Time.time;
                CompleteCycle(cycleTime);
            }
        } else {
            cycleDone = false;
        }

    }

    public override void OnInteractionEnd(Vector3 position) {
        speedState = SPEED.START;
        mixing = false;
        cycleDone = false;
        isSpoonMoving = false;
        AkSoundEngine.PostEvent("Pause_Stir", gameObject);
        s_angle += v_angle;
        if (s_angle < 0) {
            s_angle = 360 + s_angle;
        }
    }

    private void CompleteCycle(float time) {
        Debug.Log("Cycle done in " + time + "s");

        if (time > maxCycleTime) {
            Debug.Log("Too slow!");
            SCORE += pointsSlowCycle;
            if (speedState != SPEED.SLOW) {
                speedState = SPEED.SLOW;
                guzzlesarus.Play("gza_sadFeedback");
                AkSoundEngine.PostEvent("Too_Slow", gameObject);
                trafficlight.SetAmber();
            }

        } else if (time < minCycleTime) {
            Debug.Log("Too fast!");
            SCORE += pointsFastCycle;
            if (speedState != SPEED.FAST) {
                speedState = SPEED.FAST;
                guzzlesarus.Play("gza_sadFeedback");
                AkSoundEngine.PostEvent("Too_Fast", gameObject);
                trafficlight.SetRed();
            }
        } else {
            if (speedState != SPEED.NORMAL) {

                speedState = SPEED.NORMAL;
                guzzlesarus.Play("gza_happyFeedback");
            }
            SCORE += pointsCorrectCycle;
            trafficlight.SetGreen();
        }
        if (debugScoreText != null)
            debugScoreText.text = "SCORE: " + SCORE;
        cyclesCompleted++;
        stateCyclesCompleted++;
        if (currentState + 1 < statesList.Length && stateCyclesCompleted >= statesList[currentState].cycles) {
            stateCyclesCompleted = 0;

            // The program will not enter here, we are keeping this only while testing the scoring
            if (currentState + 1 == statesList.Length) {
                Debug.Log("COMPLETE!");
                mixing = false;
                AkSoundEngine.PostEvent("Stop_Stir", gameObject);
                prompt.ShowPromptAfter(0.4f, 5, () => {
                    manager.ScreenFadeOut("FryPancake");
                }, true);

            } else {
                currentState++;
                spriteRenderer.sprite = statesList[currentState].sprite;
            }
        } else if (cyclesCompleted == cyclesToComplete) {
            Debug.Log("COMPLETE!");
            mixing = false;
            AkSoundEngine.PostEvent("Stop_Stir", gameObject);

            feedbackGuzz.Play("GuzzMoveOut");

            prompt.ShowPromptAfter(0.8f, 5, () => {
                manager.ScreenFadeOut("FryPancake");
            }, true);
        }
    }
}

