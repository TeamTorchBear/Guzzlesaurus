using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCoop : MonoBehaviour {

    [SerializeField]
    private GameObject[] chickenPrefabs;

    private ChickenControl[] chickens;

    [SerializeField]
    private GameObject exitButton;

    [SerializeField]
    private GameObject door;

    private void Start() {

        // Calculate number of eggs that we need at start
        Data data = SaveNLoadTxt.Load();
        int eggsNeeded = Pancake.eggs - data.eggQuantity;

        // Get all the available chickens
        chickens = FindObjectsOfType<ChickenControl>();

        // Generate types of chicken randomly
        foreach (ChickenControl chicken in chickens) {
            int c = Random.Range(0, chickenPrefabs.Length);
            GameObject chickenInstance = Instantiate(chickenPrefabs[c], chicken.transform);
            chicken.chickenObject = chickenInstance;
            chicken.chickenObject.name = chickenPrefabs[c].name;
        }

        AkSoundEngine.PostEvent("CoopDoorOpen", gameObject);
        AkSoundEngine.SetRTPCValue("Time", 100f, null, 1350);

        // Place eggs randomly
        int eggsPlaced = 0;
        while (eggsPlaced < eggsNeeded && eggsPlaced < chickens.Length) {
            int rand = Random.Range(0, chickens.Length);
            
            if (chickens[rand].hasEgg) {
                continue;
            }

            // If the chicken has no egg, place one and increment eggsPlaced
            chickens[rand].hasEgg = true;
            ++eggsPlaced;
            
        }
        


        if (data.eggQuantity == Pancake.eggs) {
            exitButton.SetActive(true);
        }

        GetComponent<InputManager>().enabled = false;
        StartCoroutine(Coroutines.AnimatePosition(door, new Vector3(20f, door.transform.position.y, door.transform.position.z), 10, () => {
            GetComponent<InputManager>().enabled = true;
            AkSoundEngine.PostEvent("TapChicken", gameObject);
            
        }));
    }

    /*
     * This function will be called from ChickenControl.cs each time a chicken
     * is tapped (and it's a valid tap, that is, it is not during an animation)
     * and returns true if everything went as expected.
     */
    public bool OnChickenTap(ChickenControl caller) {
        


        return true;
    }

    public bool OnEggTap(EggCoop egg) {
        Data data = SaveNLoadTxt.Load();
        

        if (++data.eggQuantity == Pancake.eggs) {
            exitButton.SetActive(true);
        }
        
        SaveNLoadTxt.Save(data);

        return true;
    }


}
