using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteEffectManager : MonoBehaviour {
    public List<GameObject> spriteEffectsAvailable = null;
    public List<GameObject> spriteEffectsInUse = null;

    //-------------
    //BEGINNING OF SINGLETON CODE CONFIGURATION5
    private static volatile SpriteEffectManager _instance;
    private static object _lock = new object();

    //Stops the lock being created ahead of time if it's not necessary
    static SpriteEffectManager() {
    }

    public static SpriteEffectManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if (_instance == null) {

                        GameObject spriteEffectManagerGameObject = new GameObject("SpriteEffectManagerGameObject");
                        _instance = (spriteEffectManagerGameObject.AddComponent<SpriteEffectManager>()).GetComponent<SpriteEffectManager>();
                    }
                }
            }
            return _instance;
        }
    }

    private SpriteEffectManager() {
    }

    // Use this for initialization
    void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
    }
    //END OF SINGLETON CODE CONFIGURATION

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Space)) {
            GenerateSpriteEffectType(SpriteEffectType.BrotocolAchieved, Vector3.zero);
        }
    }

    public GameObject GenerateSpriteEffectType(SpriteEffectType spriteEffectTypeToGenerate, Vector3 positionToGenerateAt) {
        GameObject newSpriteEffect = null;
        if(spriteEffectsAvailable.Count > 0) {
            newSpriteEffect = spriteEffectsAvailable[spriteEffectsAvailable.Count - 1];
            spriteEffectsAvailable.RemoveAt(spriteEffectsAvailable.Count - 1);
        }
        else {
            newSpriteEffect = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/SpriteEffects/SpriteEffect") as GameObject);
            newSpriteEffect.transform.parent = this.gameObject.transform;
        }

        if(!spriteEffectsInUse.Contains(newSpriteEffect)) {
            spriteEffectsInUse.Add(newSpriteEffect);
        }

        newSpriteEffect.SetActive(true);
        newSpriteEffect.transform.position = positionToGenerateAt;
        newSpriteEffect.GetComponent<SpriteEffect>().spriteEffectType = spriteEffectTypeToGenerate;

        return newSpriteEffect;
    }
}
