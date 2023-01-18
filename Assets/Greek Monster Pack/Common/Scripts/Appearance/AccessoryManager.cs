using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AccessoryManager : MonoBehaviour
{
    [Header("Is Enabled:"), Tooltip("(True) Will check for changes and update accordingly.")]
    public bool isEnabled = true;
    [Tooltip("(True) Will check for changes made in the editor inspector window during runtime.")]
    public bool runtimeValidation = false;

    [System.Serializable]
    public class BodyMaterialSettings
    {
        [Header("Required Settings:"), Tooltip("Contains general information about the character type.")]
        public CreatureProfile creatureProfile;
        [Header("Profile Information: (Read-Only)")]
        public List<string> bodyMaterials;
        [Header("Material Settings:"), Tooltip("Color of the beast portion of the character. Refer to the body materials list above to get the index of specific materials.")]
        public int bodyMaterialIndex = 0;
        [Range(0, 2), Tooltip("Change the character's skin tone from lighter to darker.")]
        public int skinTone = 0;
        [HideInInspector] //Character main mesh renderer
        public SkinnedMeshRenderer meshRenderer;
        [HideInInspector] //Used to test for changes
        public int lastMaterialIndex = -1;
    }

    [SerializeField, Header("Settings:")]
    public BodyMaterialSettings bodyMaterialSettings = new BodyMaterialSettings();

    [System.Serializable]
    public class HairSettings
    {
        [Header("Profiles:"), Tooltip("(True) Will check for changes made to hair settings.")]
        public bool enableHair = true;
        [Tooltip("Contains information that serves to locate hair objects within the hair set.")]
        public HairProfile hairProfile;
        [Tooltip("Contains information that serves to locate beard objects within the hair set.")]
        public BeardProfile beardProfile;
        [Tooltip("Contains information that serves to locate eyebrow objects within the hair set.")]
        public EyebrowProfile eyebrowProfile;
        [Tooltip("Contains hair colors that may be used. To limit possible hair selections use a different profile.")]
        public HairMaterialProfile hairMaterialProfile;
        [HideInInspector] //Used to call functions that cannot be called in the validate function
        public EditorParentageManager parentageManager;
        [HideInInspector] //Parent of all possible hair style gameobjects
        public Transform hairGroup;
        [HideInInspector] //Gameobject containing the active hair's mesh renderer
        public GameObject activeHairObject;
        [HideInInspector] //Gameobject containing the active eyebrow's mesh renderer
        public GameObject activeEyebrowObject;
        [HideInInspector] //Gameobject containing the active beard's mesh renderer
        public GameObject activeBeardObject;
        [HideInInspector] //Used to call functions that cannot be called in the validate function
        public bool updateParentage = false;
        [HideInInspector] //Used to call functions that cannot be called in the validate function
        public List<ParentageSet> parentageSets = new List<ParentageSet>();
        [HideInInspector] //Used to test for changes
        public int lastUsedHairIndex = -1;

        [Header("Hair Selections:"), Tooltip("Active Hair Style")]
        public HairType hairType = HairType.Bald;
        [Tooltip("Active Eyebrow Style")]
        public EyebrowType eyebrowType = EyebrowType.None;
        [Tooltip("Active Beard Style (Male Only)")]
        public BeardType beardType = BeardType.None;
        [SerializeField, HideInInspector] //Used to test for changes
        public HairType lastHairType = HairType.Bald;
        [SerializeField, HideInInspector] //Used to test for changes
        public EyebrowType lastEyebrowType = EyebrowType.None;
        [SerializeField, HideInInspector] //Used to test for changes
        public BeardType lastBeardType = BeardType.None;

        [Header("Profile Information (Read-Only):")]
        public List<string> hairUsableMaterials = new List<string>();
        [Header("Hair Material:"), Tooltip("Active hair material. To find a specific index refer to the hair usable material list above.")]
        public int hairMaterialIndex;
    }
    [SerializeField]
    public HairSettings hairSettings = new HairSettings();

    public EyeSettings eyeSettings = new EyeSettings();

    [System.Serializable]
    public class EyeSettings
    {
        [Header("Profile:"), Tooltip("Contains Eye colors that may be used. To limit possible eye selections use a different profile.")]
        public EyeMaterialProfile eyeMaterialProfile;
        [Header("Profile Information (Read-Only):")]
        public List<string> eyeUsableMaterials = new List<string>();
        [Header("Eye Material:"), Tooltip("Active eye material. To find a specific index refer to the eye usable material list above.")]
        public int eyeIndex = 0;
        [Tooltip("This value is used only for Cyclops")]
        public SkinnedMeshRenderer altEyeRenderer = null;
        [HideInInspector]
        public int lastUsedEyeIndex = -1;
    }

    public RandomSettings randomSettings = new RandomSettings();

    [System.Serializable]
    public class RandomSettings 
    {
        [Tooltip("Randomize the color of the beast portion of the character now or when the gameobject is activated at runtime.")]
        public MonsterActivationType randomizeBeastColor = MonsterActivationType.Disabled;
        [Tooltip("Randomize the skin tone of the character now or when the gameobject is activated at runtime.")]
        public MonsterActivationType randomizeSkinTone = MonsterActivationType.Disabled;
        [Tooltip("Randomize the character's hair style now or when the gameobject is activated at runtime. (Bald and male-only hair styles are exluded from randomiztion for female characters by default)")]
        public MonsterActivationType randomizeHairType = MonsterActivationType.Disabled;
        [Tooltip("(False) Prevents female characters from being bald when randomizing hair styles.")]
        public bool allowRandomFemaleBaldness = false;
        [Tooltip("Randomize the character's beard now or when the gameobject is activated at runtime. (Male Only)")]
        public MonsterActivationType randomizeBeardType = MonsterActivationType.Disabled;
        [Tooltip("Randomize the character's eyerbrows now or when the gameobject is activated at runtime.")]
        public MonsterActivationType randomizeEyebrowType = MonsterActivationType.Disabled;
        [Tooltip("Randomize the hair color of the character now or when the gameobject is activated at runtime. To limit possible selections use a different hair material profile.")]
        public MonsterActivationType randomizeHairColor = MonsterActivationType.Disabled;
        [Tooltip("Randomize the eye color of the character now or when the gameobject is activated at runtime. To limit possible selections use a different eye profile.")]
        public MonsterActivationType randomizeEyeColor = MonsterActivationType.Disabled;
    }
    //Prevents changes will finalizing the character's look
    private bool inDestructionMode = false;
    public FinalizeSettings finalizeSettings = new FinalizeSettings();

    [System.Serializable]
    public class FinalizeSettings
    {
        [SerializeField, Tooltip("Setting this value will remove this script and corresponding components from the character and delete unused hair objects at a specific point in time (selection). \n \n Further changes cannot be easily made if finalized now. Finalizing On Awake will allow further changes to be made in editor and is recomended for character's who appearance will not need to be changed after the Awake Call.  \n \n This value cannot be set to Now if the gameobject is part of a prefab. You must unpack it first.")]
        public MonsterActivationType completeChanges = MonsterActivationType.Disabled;
    }

    //Calls to the Extremity manager primarily for other material changes
    [HideInInspector]
    public UnityEvent<float> OnApperanceUpdate = new UnityEvent<float>();

    private void Awake()
    {
        //Updates the character on awake with awake functionality
        UpdateCharacterApp(true);
    }

    //Updates the character without awake functionality when called via custom implementation
    public void CallUpdateCharacter()
    {
        if (isEnabled)
        UpdateCharacterApp(false);
    }
    //Changes and updates the character to specific or random value
    public void ChangeBeastMaterial(int index, bool setToRandom = false)
    {
        if (setToRandom)
        {
            randomSettings.randomizeBeastColor = MonsterActivationType.Now;
            CallUpdateCharacter();
            return;
        }
        bodyMaterialSettings.bodyMaterialIndex = index;
        CallUpdateCharacter();
    }
    //Changes and updates the character to specific or random value
    public void ChangeSkinTone(int index, bool setToRandom = false)
    {
        if (setToRandom)
        {
            randomSettings.randomizeSkinTone = MonsterActivationType.Now;
            CallUpdateCharacter();
            return;
        }
        bodyMaterialSettings.skinTone = index;
        CallUpdateCharacter();
    }
    //Changes and updates the character to specific or random value
    public void ChangeEyeColor(int index, bool setToRandom = false)
    {
        if (setToRandom)
        {
            randomSettings.randomizeEyeColor = MonsterActivationType.Now;
            CallUpdateCharacter();
            return;
        }
        eyeSettings.eyeIndex = index;
        CallUpdateCharacter();
    }
    //Changes and updates the character to specific or random value
    public void ChangeHairColor(int index, bool setToRandom = false)
    {
        if (setToRandom)
        {
            randomSettings.randomizeHairColor = MonsterActivationType.Now;
            CallUpdateCharacter();
            return;
        }
        hairSettings.hairMaterialIndex = index;
        CallUpdateCharacter();
    }
    //Changes and updates the character to specific or random value
    public void ChangeHairStyle(HairType hairType, bool setToRandom = false)
    {
        if (setToRandom)
        {
            randomSettings.randomizeHairType = MonsterActivationType.Now;
            CallUpdateCharacter();
            return;
        }
        hairSettings.hairType = hairType;
        CallUpdateCharacter();
    }
    //Changes and updates the character to specific or random value
    public void ChangeBeardStyle(BeardType beardType, bool setToRandom = false)
    {
        if (setToRandom)
        {
            randomSettings.randomizeBeardType = MonsterActivationType.Now;
            CallUpdateCharacter();
            return;
        }
        hairSettings.beardType = beardType;
        CallUpdateCharacter();
    }
    //Changes and updates the character to specific or random value
    public void ChangeEyebrowStyle(EyebrowType eyebrowType, bool setToRandom = false)
    {
        if (setToRandom)
        {
            randomSettings.randomizeEyebrowType = MonsterActivationType.Now;
            CallUpdateCharacter();
            return;
        }
        hairSettings.eyebrowType = eyebrowType;
        CallUpdateCharacter();
    }

    //Randomizes the entire character
    public void RandomizeCharacter()
    {
        randomSettings.randomizeSkinTone = MonsterActivationType.Now;
        randomSettings.randomizeEyebrowType = MonsterActivationType.Now;
        randomSettings.randomizeBeardType = MonsterActivationType.Now;
        randomSettings.randomizeHairType = MonsterActivationType.Now;
        randomSettings.randomizeBeastColor = MonsterActivationType.Now;
        randomSettings.randomizeEyeColor = MonsterActivationType.Now;
        randomSettings.randomizeHairColor = MonsterActivationType.Now;
        CallUpdateCharacter();
    }

    //Finalizes the character 
    public void FinalizeCharacterNow()
    {
        finalizeSettings.completeChanges = MonsterActivationType.Now;
        CallUpdateCharacter();
    }

    //Main function used to update character
    void UpdateCharacterApp(bool inAwake = false)
    {
        //Checks internal values and populates them if required. Returns false if it cannot find required information.
        if (!CheckPreReqs()) return;

        UpdateCreatureProfile();
        if (inAwake)
        {
            CheckRandoms(MonsterActivationType.OnAwake);
        }
        else
        {
            CheckRandoms();
        }
        int bodyMaterialIndex = UpdateCharacter();
        UpdateHair_v2();
        UpdateBeard_v2();
        UpdateEyebrow_V2();
        UpdateParentage();
        OnApperanceUpdate.Invoke(bodyMaterialIndex);
        UpdateHairMaterial();
        UpdateEyeMaterial();

        if (inAwake)
        {
            CheckCompletion(true);
        }
        else
        {
            CheckCompletion(false);
        }
    }

    //Main script enabler check
    bool CheckEnablers()
    {
#if UNITY_EDITOR
        if (!runtimeValidation)
        {
            if (EditorApplication.isPlaying)
            {
                return false;
            }
        }
#endif
        if (!isEnabled) return false;
        return true;
    }

    //Checks internal values and populates them if required. Returns false if it cannot find required information.
    bool CheckPreReqs()
    {
        //Stops the update process if scipt is finalizing changes
        if (inDestructionMode)
        {
            return false;
        }
        //Checks for profiles
        if (bodyMaterialSettings.creatureProfile == null)
        {
            return false;
        }
        if (hairSettings.enableHair && hairSettings.hairProfile == null)
        {
            return false;
        }
        if (hairSettings.enableHair && hairSettings.beardProfile == null && bodyMaterialSettings.creatureProfile.creatureGender == CreatureSex.Male)
        {
            return false;
        }
        if (hairSettings.enableHair && hairSettings.eyebrowProfile == null)
        {
            return false;
        }
        if (hairSettings.enableHair && hairSettings.hairMaterialProfile == null)
        {
            return false;
        }
        if (eyeSettings.eyeMaterialProfile == null)
        {
            return false;
        }
        //Gets main mesh renderer
        if (bodyMaterialSettings.meshRenderer == null)
        {
            SkinnedMeshRenderer[] newMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer newMeshRenderer in newMeshRenderers)
            {
                if (newMeshRenderer.gameObject.name == bodyMaterialSettings.creatureProfile.creatureGender.ToString())
                {
                    bodyMaterialSettings.meshRenderer = newMeshRenderer;
                    break;
                }
            }
            if (bodyMaterialSettings.meshRenderer == null)
            {
                Debug.LogError("Creature Mesh Renderer could not be found.");
                return false;
            }
        }
        //Gets main hair group
        if (hairSettings.enableHair && hairSettings.hairGroup == null)
        {
            if (bodyMaterialSettings.creatureProfile.creatureGender == CreatureSex.Male)
            {
                hairSettings.hairGroup = transform.Find("HairSet_Male");
            }
            else
            {
                hairSettings.hairGroup = transform.Find("HairSet_Female");
            }
            if (hairSettings.hairGroup == null)
            {
                Debug.LogWarning("Hair set could not be found. Make sure it has not been renamed.");
                return false;
            }
        }
        //Adds script that will control functions that cannot be called in the validate function
        if (hairSettings.enableHair && hairSettings.parentageManager == null)
        {
            hairSettings.parentageManager = GetComponent<EditorParentageManager>();
            if (hairSettings.parentageManager == null)
            {
                gameObject.AddComponent(typeof(EditorParentageManager));
                hairSettings.parentageManager = GetComponent<EditorParentageManager>();
            }
        }

        return true;
    }

    //Creates a reference list for monster materials based on its profile
    void UpdateCreatureProfile()
    {
        if (bodyMaterialSettings.creatureProfile == null) return;
        bodyMaterialSettings.bodyMaterials.Clear();
        if (bodyMaterialSettings.creatureProfile.usableMaterials.Count > 0)
        {
            string lastName = "";
            for (int i = 0; i < bodyMaterialSettings.creatureProfile.usableMaterials.Count; i++)
            {
                string newName = bodyMaterialSettings.creatureProfile.usableMaterials[i].name;
                if (lastName == newName) continue;
                lastName = newName;
                bodyMaterialSettings.bodyMaterials.Add(newName);
            }
        }
    }

    //Checks for random values
    void CheckRandoms(MonsterActivationType comparison = MonsterActivationType.Now)
    {
        if (randomSettings.randomizeBeastColor == comparison)
        {
            RandomizeBeastColor();
        }
        if (randomSettings.randomizeSkinTone == comparison)
        {
            RandomizeSkinTone();
        }
        if (randomSettings.randomizeHairType == comparison)
        {
            RandomizeHairType();
        }
        if (randomSettings.randomizeBeardType == comparison)
        {
            RandomizeBeardType();
        }
        if (randomSettings.randomizeEyebrowType == comparison)
        {
            RandomizeEyebrowType();
        }
        if (randomSettings.randomizeHairColor == comparison)
        {
            RandomizeHairColor();
        }
        if (randomSettings.randomizeEyeColor == comparison)
        {
            RandomizeEyeColor();
        }
    }
    //Randomizes the beast color
    void RandomizeBeastColor()
    {
        if (!bodyMaterialSettings.creatureProfile.handleSkinTonesSeperately)
        {
            bodyMaterialSettings.bodyMaterialIndex = Random.Range(0, bodyMaterialSettings.creatureProfile.usableMaterials.Count / bodyMaterialSettings.creatureProfile.skinTones);
        }
        else
        {
            bodyMaterialSettings.bodyMaterialIndex = Random.Range(0, bodyMaterialSettings.creatureProfile.usableMaterials.Count);
        }
        randomSettings.randomizeBeastColor = MonsterActivationType.Disabled;
    }
    //Randomizes skin tone
    void RandomizeSkinTone()
    {
        bodyMaterialSettings.skinTone = Random.Range(0, bodyMaterialSettings.creatureProfile.skinTones);
        randomSettings.randomizeSkinTone = MonsterActivationType.Disabled;
    }
    //Randomizes hair type
    void RandomizeHairType()
    {
        if (bodyMaterialSettings.creatureProfile.creatureGender == CreatureSex.Male)
        {
            hairSettings.hairType = (HairType)Random.Range(0, 7);
        }
        else
        {
            int startingInt = 1;
            if (randomSettings.allowRandomFemaleBaldness) startingInt = 0;
            hairSettings.hairType = (HairType)Random.Range(startingInt, 5);
        }
        randomSettings.randomizeHairType = MonsterActivationType.Disabled;
    }
    //Randomizes beard type
    void RandomizeBeardType()
    {
        if (bodyMaterialSettings.creatureProfile.creatureGender == CreatureSex.Male)
        {
            hairSettings.beardType = (BeardType)Random.Range(0, 6);
        }
        randomSettings.randomizeBeardType = MonsterActivationType.Disabled;
    }
    //Randomizes Eyebrow tone
    void RandomizeEyebrowType()
    {
        hairSettings.eyebrowType = (EyebrowType)Random.Range(1, 8);
        randomSettings.randomizeEyebrowType = MonsterActivationType.Disabled;
    }
    //Randomizes hair material
    void RandomizeHairColor()
    {
        hairSettings.hairMaterialIndex = Random.Range(0, hairSettings.hairUsableMaterials.Count);
        randomSettings.randomizeHairColor = MonsterActivationType.Disabled;
    }
    //Randomizes eye material
    void RandomizeEyeColor()
    {
        eyeSettings.eyeIndex = Random.Range(0, eyeSettings.eyeUsableMaterials.Count);
        randomSettings.randomizeEyeColor = MonsterActivationType.Disabled;
    }
    //Actually changes skin tone and beast color based on an index compared against the profile list. Returns -1 if the material cannot be found
    int UpdateCharacter()
    {
        int newBodyMaterialIndex;
        int returnValue;
        //Checks for single skin tone in monster profile
        if (bodyMaterialSettings.creatureProfile.skinTones == 1)
        {
            bodyMaterialSettings.skinTone = 0;
        }
        //Calculates combined beast color and skin tone index
        if (bodyMaterialSettings.bodyMaterialIndex < 0) bodyMaterialSettings.bodyMaterialIndex = 0;
        if (!bodyMaterialSettings.creatureProfile.handleSkinTonesSeperately)
        {
            if (bodyMaterialSettings.bodyMaterialIndex >= bodyMaterialSettings.creatureProfile.usableMaterials.Count / bodyMaterialSettings.creatureProfile.skinTones)
            {
                bodyMaterialSettings.bodyMaterialIndex = bodyMaterialSettings.creatureProfile.usableMaterials.Count / bodyMaterialSettings.creatureProfile.skinTones - 1;
            }
            newBodyMaterialIndex = bodyMaterialSettings.bodyMaterialIndex * bodyMaterialSettings.creatureProfile.skinTones + bodyMaterialSettings.skinTone;
            returnValue = newBodyMaterialIndex;
        }
        else
        {
            if (bodyMaterialSettings.bodyMaterialIndex >= bodyMaterialSettings.creatureProfile.usableMaterials.Count - 1)
            {
                bodyMaterialSettings.bodyMaterialIndex = bodyMaterialSettings.creatureProfile.usableMaterials.Count - 1;
            }
            newBodyMaterialIndex = bodyMaterialSettings.bodyMaterialIndex;
            returnValue = bodyMaterialSettings.bodyMaterialIndex * bodyMaterialSettings.creatureProfile.skinTones + bodyMaterialSettings.skinTone;
        }

        //Checks calculated index
        if (newBodyMaterialIndex >= bodyMaterialSettings.creatureProfile.usableMaterials.Count)
        {
            Debug.LogError("Creature Material could not be found.");
            return -1;
        }

        if (newBodyMaterialIndex == bodyMaterialSettings.lastMaterialIndex) return returnValue;

        //Sets the new material
        Material[] sharedMaterials = bodyMaterialSettings.meshRenderer.sharedMaterials;
        sharedMaterials[bodyMaterialSettings.creatureProfile.meshMaterialIndex] = bodyMaterialSettings.creatureProfile.usableMaterials[newBodyMaterialIndex].material;
        bodyMaterialSettings.meshRenderer.sharedMaterials = sharedMaterials;

        return returnValue;
    }

    //Disables hair objects not in use and activates hair objects in use
    void UpdateHair_v2()
    {
        if (!hairSettings.enableHair) return;
        //Checks for selection changes
        if (hairSettings.lastHairType == hairSettings.hairType) return;
        hairSettings.lastHairType = hairSettings.hairType;

        //Marks old hair object for disabling by the parentage manager
        if (hairSettings.activeHairObject != null)
        {
            AddParentageSet(hairSettings.activeHairObject, false);
            hairSettings.activeHairObject = null;
        }
        //Finds new hair object
        hairSettings.activeHairObject = GetHairObject(hairSettings.hairProfile.GetId(hairSettings.hairType));

        //Marks new hair object for enabling by the parentage manager
        if (hairSettings.activeHairObject != null && hairSettings.hairType != HairType.Bald)
        {
            AddParentageSet(hairSettings.activeHairObject, true);
            hairSettings.lastUsedHairIndex = -1;
        }

        //Will turn on the parentage manager near cycle end
        hairSettings.updateParentage = true;
    }

    //Disables beard objects not in use and activates beard objects in use
    void UpdateBeard_v2()
    {
        if (!hairSettings.enableHair) return;

        //Checks for selection changes and gender
        if (hairSettings.lastBeardType == hairSettings.beardType) return;
        if (bodyMaterialSettings.creatureProfile.creatureGender == CreatureSex.Female)
        {
            hairSettings.beardType = BeardType.None;
            hairSettings.lastBeardType = hairSettings.beardType;
            return;
        }
        hairSettings.lastBeardType = hairSettings.beardType;

        //Marks old beard object for disabling by the parentage manager
        if (hairSettings.activeBeardObject != null)
        {
            AddParentageSet(hairSettings.activeBeardObject, false);
            hairSettings.activeBeardObject = null;
        }

        //Finds new beard object
        hairSettings.activeBeardObject = GetHairObject(hairSettings.beardProfile.GetId(hairSettings.beardType));

        //Marks new beard object for enabling by the parentage manager
        if (hairSettings.activeBeardObject != null && hairSettings.beardType != BeardType.None)
        {
            AddParentageSet(hairSettings.activeBeardObject, true);
            hairSettings.lastUsedHairIndex = -1;
        }

        //Will turn on the parentage manager near cycle end
        hairSettings.updateParentage = true;
    }

    //Disables eyebrow objects not in use and activates eyebrow objects in use
    void UpdateEyebrow_V2()
    {
        if (!hairSettings.enableHair) return;
        //Checks for selection changes
        if (hairSettings.lastEyebrowType == hairSettings.eyebrowType) return;
        hairSettings.lastEyebrowType = hairSettings.eyebrowType;

        //Marks old eyebrow object for disabling by the parentage manager
        if (hairSettings.activeEyebrowObject != null)
        {
            AddParentageSet(hairSettings.activeEyebrowObject, false);
            hairSettings.activeEyebrowObject = null;
        }

        //Finds new eyebrow object
        hairSettings.activeEyebrowObject = GetHairObject(hairSettings.eyebrowProfile.GetId(hairSettings.eyebrowType));

        //Marks new eyebrow object for enabling by the parentage manager
        if (hairSettings.activeEyebrowObject != null && hairSettings.eyebrowType != EyebrowType.None)
        {
            AddParentageSet(hairSettings.activeEyebrowObject, true);
            hairSettings.lastUsedHairIndex = -1;
        }

        //Will turn on the parentage manager near cycle end
        hairSettings.updateParentage = true;
    }

    //Marks object for enabling or disabling via the parentage manager
    void AddParentageSet(GameObject hairObject, bool active)
    {
        ParentageSet newParentageSet = new ParentageSet();
        newParentageSet.gameObject = hairObject;
        newParentageSet.setActive = active;
        hairSettings.parentageSets.Add(newParentageSet);
    }

    //Returns hair object child the main hair group object using name in hair profile
    GameObject GetHairObject(string id)
    {
        if (id == "") return null;
        GameObject hairObject = hairSettings.hairGroup.Find(id).gameObject;
        if (hairObject == null)
        {
            Debug.LogWarning("Hair object not found. Make sure it has not been renamed.");
            return null;
        }
        return hairObject;
    }

    //Calls for parentage manager character update in its next update cycle
    void UpdateParentage()
    {
        if (hairSettings.updateParentage == false) return;
        hairSettings.parentageManager.enabled = true;
        hairSettings.parentageManager.SetParentageSet(hairSettings.parentageSets);
        hairSettings.parentageSets.Clear();
        hairSettings.updateParentage = false;
    }

    //Changes hair, beard, eyebrow object material
    void UpdateHairMaterial()
    {
        //Check for enabling and value ranges
        if (!hairSettings.enableHair) return;
        if (hairSettings.hairMaterialIndex == 0)
        {
            hairSettings.hairMaterialIndex = 1;
        }
        //Populates hair material reference list
        hairSettings.hairUsableMaterials.Clear();
        foreach(SimpleMaterialInformation materialInformation in hairSettings.hairMaterialProfile.usableMaterials)
        {
            hairSettings.hairUsableMaterials.Add(materialInformation.name);
        }

        if (hairSettings.lastUsedHairIndex == hairSettings.hairMaterialIndex) return;

        //Checks hair index ranges
        if (hairSettings.hairMaterialIndex < 0) hairSettings.hairMaterialIndex = 0;
        if (hairSettings.hairMaterialIndex > hairSettings.hairUsableMaterials.Count-1) hairSettings.hairMaterialIndex = hairSettings.hairUsableMaterials.Count-1;

        hairSettings.lastUsedHairIndex = hairSettings.hairMaterialIndex;

        if (hairSettings.hairMaterialProfile.usableMaterials[hairSettings.hairMaterialIndex] == null) return;
        if (hairSettings.hairMaterialProfile.usableMaterials[hairSettings.hairMaterialIndex].material == null) return;

        //Changes hair material
        if (hairSettings.hairType == HairType.ShavedMaleOnly)
        {
            ChangeGameobjectMaterial(hairSettings.activeHairObject.transform, hairSettings.hairMaterialProfile.usableMaterials[0].material);
        }
        else if (hairSettings.activeHairObject != null)
        {
            if (hairSettings.hairType == HairType.Poneytail)
            {
                ChangeGameobjectMaterial(hairSettings.activeHairObject.transform, hairSettings.hairMaterialProfile.usableMaterials[hairSettings.hairMaterialIndex].material,1);
            }
            else
            {
                ChangeGameobjectMaterial(hairSettings.activeHairObject.transform, hairSettings.hairMaterialProfile.usableMaterials[hairSettings.hairMaterialIndex].material);
            }
            
        }
        //Changes beard material
        if(hairSettings.activeBeardObject)
        ChangeGameobjectMaterial(hairSettings.activeBeardObject.transform, hairSettings.hairMaterialProfile.usableMaterials[hairSettings.hairMaterialIndex].material);
        //Changes Eyerbow material
        if (hairSettings.activeEyebrowObject)
        ChangeGameobjectMaterial(hairSettings.activeEyebrowObject.transform, hairSettings.hairMaterialProfile.usableMaterials[hairSettings.hairMaterialIndex].material);
    }

    //Changes gameobject material to a specific material
    void ChangeGameobjectMaterial(Transform materialObject, Material material, int materialIndex = 0)
    {
        if (materialObject == null) return;
        SkinnedMeshRenderer meshRenderer = materialObject.GetComponentInChildren<SkinnedMeshRenderer>();
        if (meshRenderer == null) return;

        Material[] sharedMaterials = meshRenderer.sharedMaterials;
        sharedMaterials[materialIndex] = material;
        meshRenderer.sharedMaterials = sharedMaterials;
    }

    //Changes eye material
    public void UpdateEyeMaterial(bool forceUpdate = false)
    {
        //Populates reference list
        eyeSettings.eyeUsableMaterials.Clear();
        foreach(SimpleMaterialInformation materialInformation in eyeSettings.eyeMaterialProfile.usableMaterials)
        {
            eyeSettings.eyeUsableMaterials.Add(materialInformation.name);
        }
        //Check index value ranges
        if (!forceUpdate && eyeSettings.lastUsedEyeIndex == eyeSettings.eyeIndex) return;
        eyeSettings.lastUsedEyeIndex = eyeSettings.eyeIndex;

        if (eyeSettings.eyeIndex < 0) eyeSettings.eyeIndex = 0;
        if (eyeSettings.eyeIndex > eyeSettings.eyeUsableMaterials.Count - 1) eyeSettings.eyeIndex = eyeSettings.eyeUsableMaterials.Count - 1;

        if (eyeSettings.eyeMaterialProfile.usableMaterials[eyeSettings.eyeIndex] == null) return;
        if (eyeSettings.eyeMaterialProfile.usableMaterials[eyeSettings.eyeIndex].material == null) return;
        //Sets new material
        Material[] sharedMaterials;
        if (eyeSettings.altEyeRenderer != null)
        {
            sharedMaterials = eyeSettings.altEyeRenderer.sharedMaterials;
            sharedMaterials[bodyMaterialSettings.creatureProfile.eyeMaterialIndex] = eyeSettings.eyeMaterialProfile.usableMaterials[eyeSettings.eyeIndex].material;
            eyeSettings.altEyeRenderer.sharedMaterials = sharedMaterials;
            return;
        }
        sharedMaterials = bodyMaterialSettings.meshRenderer.sharedMaterials;
        sharedMaterials[bodyMaterialSettings.creatureProfile.eyeMaterialIndex] = eyeSettings.eyeMaterialProfile.usableMaterials[eyeSettings.eyeIndex].material;
        bodyMaterialSettings.meshRenderer.sharedMaterials = sharedMaterials;
    }
    //Checks for completion values
    void CheckCompletion(bool isAwake = false)
    {
        //Check used for debugging
        if (finalizeSettings.completeChanges == MonsterActivationType.Disabled)
        {
            inDestructionMode = false;
            if (hairSettings.parentageManager)
            hairSettings.parentageManager.inDestructionMode = false;
        } 
        //Checks if it should finalize now
        if (finalizeSettings.completeChanges == MonsterActivationType.Now)
        {
#if UNITY_EDITOR
            //Shows dialog used to prevent accidental usage and editor specific expections from occuring
             if (!EditorApplication.isPlaying)
             {
                if (CheckForRandomSelection(MonsterActivationType.OnAwake))
                {
                    finalizeSettings.completeChanges = MonsterActivationType.Disabled;
                    EditorUtility.DisplayDialog("Error (Randomize Settings Set to On Awake):", "You cannot finalize changes now when the monster has randomize settings set to On Awake. If you wish to keep randomize settings set to On Awake, set the Complete Changes value to On Awake.", "Ok");
                    return;
                }
                if (EditorUtility.DisplayDialog("Confirm Changes:", "This action cannot be undone! If there is any possibilty you will want to make future changes to this characters apperance, set this value to On Awake or Disabled.", "Finalize Changes Now", "Cancel"))
                {
                    if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                    {
                        finalizeSettings.completeChanges = MonsterActivationType.Disabled;
                        EditorUtility.DisplayDialog("Error (Character is part of a Prefab):","Characters that are part of a prefab cannot be finalized now. \n Solution 1: Unpack the prefab and re-finalize \n Solution 2: Set the Complete Changes setting to On Awake.", "Ok");
                        Debug.LogWarning("Characters that are part of a prefab cannot be finalized now. Solution 1: Unpack the prefab and re-finalize Solution 2: Set the complete changes to On Awake.");
                        return;
                    }
                    CompleteChanges();
                    return;
                }
                else
                {
                    finalizeSettings.completeChanges = MonsterActivationType.Disabled;
                    return;
                }
             }
#endif
            //if in runtime finalize now
            CompleteChanges();
        }
        if (isAwake && finalizeSettings.completeChanges == MonsterActivationType.OnAwake)
        {
            //if in awake call finalize now
            StartCoroutine(DelayCompleteChanges());
        }
    }
    IEnumerator DelayCompleteChanges()
    {
        yield return 0;
        CompleteChanges();
    }

    //Checks to see if a random value has been set
    bool CheckForRandomSelection(MonsterActivationType activationType)
    {
        if(randomSettings.randomizeBeardType == activationType) return true;
        if (randomSettings.randomizeBeastColor == activationType) return true;
        if (randomSettings.randomizeEyebrowType == activationType) return true;
        if (randomSettings.randomizeEyeColor == activationType) return true;
        if (randomSettings.randomizeHairColor == activationType) return true;
        if (randomSettings.randomizeHairType == activationType) return true;
        if (randomSettings.randomizeSkinTone == activationType) return true;
        return false;
    } 
    //Calls editor parentage manager to finalize monster in its next update cycle
    void CompleteChanges()
    {
        inDestructionMode = true;
        hairSettings.parentageManager.enabled = true;
        hairSettings.parentageManager.DestoryInactiveArmatureExtensions(hairSettings.hairGroup, this);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        //Checks for editor specific requirements
        if (!CheckEnablers()) return;
        //Updates the character
        UpdateCharacterApp();
    }

#endif
}
//Value containing gameobject and desired active state
[System.Serializable]
public class ParentageSet
{
    public GameObject gameObject;
    public bool setActive = false;
}
//Value containing hair types
[System.Serializable]
public enum HairType
{
    Bald,
    Long,
    Poneytail,
    Short,
    Shaggy,
    ThinMaleOnly,
    ShavedMaleOnly
}
//Value containing eyebrow types
[System.Serializable]
public enum EyebrowType
{
    None,
    Eyebrow01,
    Eyebrow02,
    Eyebrow03,
    Eyebrow04,
    Eyebrow05,
    Eyebrow06,
    Eyebrow07
}
//Value containing beard types
[System.Serializable]
public enum BeardType
{
    None,
    Full,
    Goatee01,
    Goatee02,
    Goatee03,
    Mustash
}
//Value containing information used for finalizing functionality
public enum MonsterActivationType
{
    Disabled,
    Now,
    OnAwake
}



