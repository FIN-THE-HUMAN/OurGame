using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


//IMPORTANT:
//THIS SCRIPT IS FOR DEMO PURPOSES ONLY!

//THIS CHARACTER CREATOR WAS BUILT ONLY FOR DEMO PURPOSES AND IS NOT INTENDED TO BE REPURPOSED
public class SimpleCharacterCreator : MonoBehaviour
{
    [Header("Character Settings:"), Tooltip("Female, Male Character References")]
    public List<CreatureGenderPair> characters = new List<CreatureGenderPair>();

    [System.Serializable]
    public class CreatureGenderPair
    {
        [Tooltip("Female, Male Character References")]
        public List<AccessoryManager> genders = new List<AccessoryManager>();
        [Tooltip("Alt Body color panel")]
        public GameObject altColorPanel = null;
        [Tooltip("Alt corresponding button colors")]
        public List<Color> altButtonColors = null;
        [Tooltip("Split body color button colors")]
        public List<Color> secondaryButtonColors = null;
        [Tooltip("Alt skin tone panel")]
        public GameObject altSecondaryColorPanel = null;
        [Tooltip("Alt corresponding skin tone button colors")]
        public List<Color> altSecondaryButtonColors = null;
        [Tooltip("Used only for cyclops")]
        public bool reverseGenderOrder = false;
        [HideInInspector]
        public bool[] ragdollTriggered = new bool[] { false, false };
    }
    [Tooltip("Gender button reference")]
    public List<Button> genderButtons = new List<Button>();

    [Header("Camera Settings:"), Tooltip("Camera locations sets for each character")]
    public List<SimpleCameraLocationSet> cameraLocationSets = new List<SimpleCameraLocationSet>();
    private int activeCharacterIndex = 0;
    private int activeCreatureSex = 0;
    [Tooltip("Main Camera")]
    public Transform myCamera;

    [System.Serializable]
    public class SimpleCameraLocationSet
    {
        public Transform wideCamPos;
        public Transform closeCamPos;
    }
    [Tooltip("Camera Movement Speed")]
    public float cameraSpeed = 10;
    [Tooltip("Camera Rotation Speed")]
    public float cameraRotSpeed = 5;
    [Range(0, 1), Tooltip("Zoom Value")]
    public float activeCameraPos = 0;
    [Range(-180, 180), Tooltip("Orbit Value")]
    public float rotationGoal;
    //Used for mouse orbit
    private float mouseRotGoalStart;
    //Used for orbit
    private float currentRotation;
    [Tooltip("Orbit Speed")]
    public float rotationSpeed = 5;
    [Tooltip("Zoom speed.")]
    public float zoomSpeed = 10;
    //Used for mouse orbit
    private float mouseStartPos = -100;
    [Tooltip("Mouse orbit speed")]
    public float mouseDragSpeedMultiplier = 1;
    [Tooltip("Main orbit slider")]
    public Slider rotationSlider;
    [Tooltip("Character should look at camera")]
    public bool lookAtCamera = true;
    public Toggle lookAtToggle;
    //UI main page
    private int onScreen = 0;
    [Header("UI Settings:"), Tooltip("UI main panels")]
    public List<GameObject> mainPanels = new List<GameObject>();
    [Tooltip("UI page transition buttons")]
    public List<Button> pageButtons = new List<Button>();
    [Tooltip("UI app panel descriptions")]
    public List<Text> appDescriptions = new List<Text>();
    [Tooltip("UI default color pickers")]
    public List<GameObject> colorPickers = new List<GameObject>();
    [Tooltip("UI color picker buttons")]
    public List<Image> colorPickerOpeners = new List<Image>();
    [Tooltip("UI Secondary beast color image")]
    public Image secondaryBeastColorRep;
    [Tooltip("UI default beast colors")]
    public List<Color> beastColors = new List<Color>();
    [Tooltip("UI default skin tones")]
    public List<Color> skinTones = new List<Color>();
    [Tooltip("UI hair colors")]
    public List<Color> hairColors = new List<Color>();
    [Tooltip("UI eye colors")]
    public List<Color> eyeColors = new List<Color>();
    [Tooltip("UI enable paint body color")]
    public Toggle paintToggle;
    //Used for body material calculations
    private bool paintEnabled;
    private int bodyColorIndex;

    //Ragdoll Activation Script
    private AnimationBoneWanderCorrection wanderCorrection;
    [Tooltip("Ragdoll warning page")]
    public GameObject ragdollWarningUI;

    [Header("Gorgon Functionality:"), Tooltip("Switch Gorgon Eye Profile")]
    public Toggle gorgonEyeSwitch;
    [Tooltip("Default Eye Material Profile")]
    public EyeMaterialProfile normalEyeProfile;
    [Tooltip("Gorgon Eye Material Profile")]
    public EyeMaterialProfile gorgonEyeProfile;

    [Tooltip("Demo weapon gameobjects")]
    public WeaponSets weapons;
    [System.Serializable]
    public class WeaponSets
    {
        public GameObject spear;
        public GameObject shield;
        public GameObject staff;
        public GameObject sword01;
        public GameObject sword02;
    }
    [Tooltip("Mecanim animation buttons")]
    public List<GameObject> mecanimDemoButtons = new List<GameObject>();

    //Sets UI Values
    private void Awake()
    {
        PopulateNames();
        CloseColorPickers();
        UpdateColorButtonColorFromMeshData();
        UpdateGenderButtons();
        pageButtons[0].interactable = false;
    }
    //Updates Camera
    private void Update()
    {
        if (wanderCorrection == null) wanderCorrection = characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<AnimationBoneWanderCorrection>();
        UpdateCameraPosition();
        UpdateLookAtCam();
        UpdateMouseDrag();
    }

    public Animator GetActiveAnimator()
    {
        return characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>();
    }

    //Updates Camera
    void UpdateCameraPosition()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        activeCameraPos = Mathf.Clamp01(activeCameraPos+Input.GetAxis("Mouse ScrollWheel")* zoomSpeed * Time.deltaTime);

        Vector3 wideGoalPos = Vector3.Lerp(myCamera.position, cameraLocationSets[activeCharacterIndex].wideCamPos.position, cameraSpeed * Time.deltaTime);
        Quaternion wideGoalRot = Quaternion.Slerp(myCamera.rotation, cameraLocationSets[activeCharacterIndex].wideCamPos.rotation, cameraRotSpeed * Time.deltaTime);
        Vector3 closeGoalPos = Vector3.Lerp(myCamera.position, cameraLocationSets[activeCharacterIndex].closeCamPos.position, cameraSpeed * Time.deltaTime);
        Quaternion closeGoalRot = Quaternion.Slerp(myCamera.rotation, cameraLocationSets[activeCharacterIndex].closeCamPos.rotation, cameraRotSpeed * Time.deltaTime);
        myCamera.position = Vector3.Lerp(wideGoalPos, closeGoalPos, activeCameraPos);
        myCamera.rotation = Quaternion.Slerp(wideGoalRot, closeGoalRot, activeCameraPos);

        currentRotation = Mathf.Lerp(currentRotation, rotationGoal, rotationSpeed * Time.deltaTime);
        Quaternion newRot = Quaternion.identity;
        newRot.eulerAngles = new Vector3(0, currentRotation, 0);

        cameraLocationSets[activeCharacterIndex].wideCamPos.parent.rotation = newRot;
        cameraLocationSets[activeCharacterIndex].closeCamPos.parent.rotation = newRot;
    }
    //Sets character head track target
    void UpdateLookAtCam()
    {
        characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<SimpleHeadTrack>().target = myCamera;

        if (!lookAtCamera)
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<SimpleHeadTrack>().isEnabled = false;
        }
        else
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<SimpleHeadTrack>().isEnabled = true;
        }
    }
    //Updates Camera rotation
    void UpdateMouseDrag()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            mouseStartPos = Input.mousePosition.x/Screen.width;
            mouseRotGoalStart = rotationGoal;
            CloseColorPickers();
        }
        if (mouseStartPos != -100 && Input.GetMouseButtonUp(0))
        {
            mouseStartPos = -100;
        }
        if (mouseStartPos == -100) return;
        rotationGoal = Mathf.Clamp(mouseRotGoalStart + ((mouseStartPos - (Input.mousePosition.x / Screen.width)) * mouseDragSpeedMultiplier), Mathf.Clamp(rotationGoal-4,-180,180), Mathf.Clamp(rotationGoal + 4, -180, 180));
        rotationSlider.SetValueWithoutNotify(-rotationGoal/180);
    }
    //Changes UI main Page
    public void ChangeMainPage(int direction)
    {
        if (onScreen + direction == 2 && characters[activeCharacterIndex].ragdollTriggered[activeCreatureSex]) return; 

        if (onScreen+direction>=0 && onScreen+direction<=2)
        {
            onScreen += direction;
        }
        foreach(GameObject gameObject in mainPanels)
        {
            gameObject.SetActive(false);
        }
        UpdateGenderButtons();
        pageButtons[0].interactable = true;
        pageButtons[1].interactable = true;
        if (onScreen == 0) pageButtons[0].interactable = false;
        if (onScreen == 2) pageButtons[1].interactable = false;
        mainPanels[onScreen].SetActive(true);
        ragdollWarningUI.SetActive(false);
        PlayMecanimAnimation(0);
    }
    //Changes UI main Page
    public void ChangeToPage(int index)
    {
        if (index == 2 && characters[activeCharacterIndex].ragdollTriggered[activeCreatureSex]) return;
        onScreen = index;
        foreach (GameObject gameObject in mainPanels)
        {
            gameObject.SetActive(false);
        }
        UpdateGenderButtons();
        pageButtons[0].interactable = true;
        pageButtons[1].interactable = true;
        if (onScreen == 0) pageButtons[0].interactable = false;
        if (onScreen == 2) pageButtons[1].interactable = false;
        mainPanels[onScreen].SetActive(true);
        ragdollWarningUI.SetActive(false);
        PlayMecanimAnimation(0);
    }
    //Disables button based on active creature sex
    void UpdateGenderButtons()
    {
        if (characters[activeCharacterIndex].genders[1] == null)
        {
            genderButtons[0].gameObject.SetActive(false);
            genderButtons[1].gameObject.SetActive(false);
            return;
        }
        else
        {
            genderButtons[0].gameObject.SetActive(true);
            genderButtons[1].gameObject.SetActive(true);
        }

        if (activeCreatureSex == 0)
        {
            genderButtons[0].interactable = false;
            genderButtons[1].interactable = true;
            return;
        }
        genderButtons[0].interactable = true;
        genderButtons[1].interactable = false;
        return;
    }
    //Changes creature
    public void ChangeCreature(int index)
    {
        if (index == activeCharacterIndex)
        {
            ChangeMainPage(1);
            return;
        }
        DisplayWeaponSet(0);
        characters[activeCharacterIndex].genders[0].gameObject.SetActive(false);
        if (characters[activeCharacterIndex].genders[1] != null)
            characters[activeCharacterIndex].genders[1].gameObject.SetActive(false);
        activeCharacterIndex = index;
        ChangeGender(true, true);
    }
    //Changes Gender
    public void ChangeGender(bool isFemale)
    {
        if (isFemale && activeCreatureSex == 0) return;
        if (!isFemale && activeCreatureSex == 1) return;
        DisplayWeaponSet(0);

        wanderCorrection = null;
        if (isFemale)
        {
            characters[activeCharacterIndex].genders[0].gameObject.SetActive(true);
            characters[activeCharacterIndex].genders[1].gameObject.SetActive(false);
            activeCreatureSex = 0;
            ChangeToPage(1);
            PopulateNames();
            UpdateColorButtonColorFromMeshData(true);
            UpdateGenderButtons();
            return;
        }
        characters[activeCharacterIndex].genders[0].gameObject.SetActive(false);
        characters[activeCharacterIndex].genders[1].gameObject.SetActive(true);
        activeCreatureSex = 1;
        ChangeToPage(1);
        PopulateNames();
        UpdateColorButtonColorFromMeshData(true);
        UpdateGenderButtons();
        return;
    }
    //Changes Gender
    public void ChangeGender(bool isFemale, bool overrideChecks)
    {
        if (!overrideChecks) return;
        wanderCorrection = null;
        DisplayWeaponSet(0);

        if (isFemale || characters[activeCharacterIndex].genders[1] == null)
        {
            characters[activeCharacterIndex].genders[0].gameObject.SetActive(true);
            if (characters[activeCharacterIndex].genders[1] != null)
            characters[activeCharacterIndex].genders[1].gameObject.SetActive(false);
            activeCreatureSex = 0;
            ChangeToPage(1);
            PopulateNames();
            UpdateColorButtonColorFromMeshData(true);
            UpdateGenderButtons();
            return;
        }
        characters[activeCharacterIndex].genders[0].gameObject.SetActive(false);
        characters[activeCharacterIndex].genders[1].gameObject.SetActive(true);
        activeCreatureSex = 1;
        ChangeToPage(1);
        PopulateNames();
        UpdateColorButtonColorFromMeshData(true);
        UpdateGenderButtons();
        return;
    }
    //Randomizes character look
    public void RandomizeCharacter()
    {
        characters[activeCharacterIndex].genders[activeCreatureSex].RandomizeCharacter();
        PopulateNames();
        UpdateColorButtonColorFromMeshData(true);
    }
    //Enables and Disables character head track
    public void ChangeHeadTrack(bool b)
    {
        lookAtCamera = b;
    }
    //Changes Target Cam Rot
    public void ChangeTargetCamRot(float f)
    {
        rotationGoal = f * -180;
    }
    //Changes Target Cam Zoom
    public void ChangeCameraZoom(float f)
    {
        activeCameraPos = f;
    }
    //Updates app panel names
    void PopulateNames()
    {
        int enumIndex = (int)characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType;
        if (activeCreatureSex == 0)
            appDescriptions[0].text = characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType.ToString() + " (" + (enumIndex + 1) + "/5)";
        if (activeCreatureSex == 1)
            appDescriptions[0].text = characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType.ToString() + " (" + (enumIndex + 1) + "/7)";
        enumIndex = (int)characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.eyebrowType;
        appDescriptions[1].text = "Eyebrow (" + (enumIndex + 1) + "/8)";
        if (activeCreatureSex == 0 && characters[activeCharacterIndex].reverseGenderOrder == false)
        {
            appDescriptions[2].text = "None";
        }
        else
        {
            enumIndex = (int)characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.beardType;
            appDescriptions[2].text = characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.beardType.ToString() + " (" + (enumIndex + 1) + "/5)";
        }
    }
    //Changes character hair style
    void ChangeHairStyle(int index, int direction)
    {
        int enumIndex = (int)characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType;

        if (activeCreatureSex == 0 && characters[activeCharacterIndex].reverseGenderOrder == false)
        {
            if (direction>0 && enumIndex < 4)
            {
                enumIndex++;
                characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType = (HairType)(enumIndex);
            }
            if (direction < 0 && enumIndex > 0)
            {
                enumIndex--;
                characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType = (HairType)(enumIndex);
            }
            appDescriptions[index].text = characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType.ToString() + " (" + (enumIndex + 1) + "/5)";
        }
        else
        {
            if (direction > 0 && enumIndex < 6)
            {
                enumIndex++;
                characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType = (HairType)(enumIndex);
            }
            if (direction < 0 && enumIndex > 0)
            {
                enumIndex--;
                characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType = (HairType)(enumIndex);
            }
            appDescriptions[index].text = characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairType.ToString() + " (" + (enumIndex + 1) + "/7)";
        }

        characters[activeCharacterIndex].genders[activeCreatureSex].CallUpdateCharacter();
        
    }
    //Changes character Eyebrow style
    void ChangeEyebrowStyle(int index, int direction)
    {
        int enumIndex = (int)characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.eyebrowType;
            if (direction > 0 && enumIndex < 7)
            {
                enumIndex++;
                characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.eyebrowType = (EyebrowType)(enumIndex);
            }
            if (direction < 0 && enumIndex > 0)
            {
                enumIndex--;
                characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.eyebrowType = (EyebrowType)(enumIndex);
            }
            appDescriptions[index].text = "Eyebrow (" + (enumIndex + 1) + "/8)";

        characters[activeCharacterIndex].genders[activeCreatureSex].CallUpdateCharacter();

    }
    //Changes character Beard style
    void ChangeBeardStyle(int index, int direction)
    {
        if (activeCreatureSex == 0 && characters[activeCharacterIndex].reverseGenderOrder == false)
        {
            return;
        }
        int enumIndex = (int)characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.beardType;
        if (direction > 0 && enumIndex < 5)
        {
            enumIndex++;
            characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.beardType = (BeardType)(enumIndex);
        }
        if (direction < 0 && enumIndex > 0)
        {
            enumIndex--;
            characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.beardType = (BeardType)(enumIndex);
        }
        characters[activeCharacterIndex].genders[activeCreatureSex].CallUpdateCharacter();
        appDescriptions[index].text = characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.beardType.ToString() + " (" + (enumIndex + 1) + "/5)";
    }
    //Changes character hair style
    public void ButtonForwardCall(int index)
    {
        CloseColorPickers();
        if (index == 0)
        {
            ChangeHairStyle(index, 1);
        }
        if (index == 1)
        {
            ChangeEyebrowStyle(index, 1);
        }
        if (index == 2)
        {
            ChangeBeardStyle(index, 1);
        }
    }
    //Changes character hair style
    public void ButtonBackwardCall(int index)
    {
        CloseColorPickers();
        if (index == 0)
        {
            ChangeHairStyle(index, -1);
        }
        if (index == 1)
        {
            ChangeEyebrowStyle(index, -1);
        }
        if (index == 2)
        {
            ChangeBeardStyle(index, -1);
        }
    }
    //Closes color picker panels
    void CloseColorPickers(GameObject doNotClose = null)
    {
        foreach(GameObject gameObject in colorPickers)
        {
            if (gameObject != doNotClose)
            gameObject.SetActive(false);
        }
        foreach (CreatureGenderPair creatureGenderPair in characters)
        {
            if (creatureGenderPair.altColorPanel != null && creatureGenderPair.altColorPanel != doNotClose)
            {
                creatureGenderPair.altColorPanel.SetActive(false);
            }
            if (creatureGenderPair.altSecondaryColorPanel != null && creatureGenderPair.altSecondaryColorPanel != doNotClose)
            {
                creatureGenderPair.altSecondaryColorPanel.SetActive(false);
            }
        }
    }
    //Toggles color picker panel
    public void ToggleColorPicker(int index)
    {
        if (index == 0 && characters[activeCharacterIndex].altColorPanel != null)
        {
            CloseColorPickers(characters[activeCharacterIndex].altColorPanel);
            characters[activeCharacterIndex].altColorPanel.SetActive(!characters[activeCharacterIndex].altColorPanel.activeInHierarchy);
            return;
        }
        if (index == 1 && characters[activeCharacterIndex].altSecondaryColorPanel != null)
        {
            CloseColorPickers(characters[activeCharacterIndex].altSecondaryColorPanel);
            characters[activeCharacterIndex].altSecondaryColorPanel.SetActive(!characters[activeCharacterIndex].altSecondaryColorPanel.activeInHierarchy);
            return;
        }
        CloseColorPickers(colorPickers[index]);
        colorPickers[index].SetActive(!colorPickers[index].activeInHierarchy);
    }
    //Toggles paint value
    public void TogglePaint()
    {
        CloseColorPickers();
        paintEnabled = !paintEnabled;
        CalculateBodyColorIndex();
    }
    //Sets character body color index with regard to skin tone
    public void SetBodyColorIndex(int index)
    {
        CloseColorPickers();
        bodyColorIndex = index;
        paintToggle.interactable = true;
        UpdateColorPickerButtonColor(0, index);
        CalculateBodyColorIndex();
    }
    //Sets character body color index
    public void SetBodyColorIndexAbsolute(int index)
    {
        CloseColorPickers();
        bodyColorIndex = index;
        paintToggle.interactable = false;
        paintToggle.isOn = false;
        CalculateBodyColorIndex(true, true, true);
    }
    //Sets character skin tone index
    public void SetSkinToneIndex(int index)
    {
        CloseColorPickers();
        UpdateColorPickerButtonColor(1, index);
        characters[activeCharacterIndex].genders[activeCreatureSex].bodyMaterialSettings.skinTone = index;
        characters[activeCharacterIndex].genders[activeCreatureSex].CallUpdateCharacter();
    }
    //Sets clothing color
    public void SetClothingColor(int index)
    {
        CloseColorPickers();
        UpdateColorPickerButtonColor(1, index);
        characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<SimpleClothingMaterialSelector>().UpdateApp(index);
        TurnOnClothing();
    }
    //Turns off clothing
    public void TurnOffClothing(int index)
    {
        CloseColorPickers();
        UpdateColorPickerButtonColor(1, index);
        characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<SimpleClothingMaterialSelector>().myRenderer.gameObject.SetActive(false);
    }
    //Turns on clothing
    public void TurnOnClothing()
    {
        characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<SimpleClothingMaterialSelector>().myRenderer.gameObject.SetActive(true);
    }
    //Calculates body color index with reagrds to skin tone
    void CalculateBodyColorIndex(bool isAbsolute = false, bool reloadButtonDataFromMesh = false, bool forceUpdate = false)
    {
        int calcIndex = bodyColorIndex;
        if (!isAbsolute && !paintEnabled)
        {
            calcIndex += 5;
        }
        characters[activeCharacterIndex].genders[activeCreatureSex].bodyMaterialSettings.bodyMaterialIndex = calcIndex;

        if (reloadButtonDataFromMesh)
        {
            UpdateColorButtonColorFromMeshData();
            if (forceUpdate)
            {
                characters[activeCharacterIndex].genders[activeCreatureSex].CallUpdateCharacter();
            }
        }
        else
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].CallUpdateCharacter();
        }
    }
    //Sets hair color index
    public void SetHairColorIndex(int index)
    {
        CloseColorPickers();
        if (characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairMaterialProfile == null) return;
        index++;
        UpdateColorPickerButtonColor(2, index);
        characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairMaterialIndex = index;
        characters[activeCharacterIndex].genders[activeCreatureSex].CallUpdateCharacter();
    }
    //Sets eye color index
    public void SetEyeColorIndex(int index)
    {
        CloseColorPickers();
        UpdateColorPickerButtonColor(3, index);
        characters[activeCharacterIndex].genders[activeCreatureSex].eyeSettings.eyeIndex = index;
        characters[activeCharacterIndex].genders[activeCreatureSex].CallUpdateCharacter();
    }
    //Changes Eye profile
    public void ChangeEyeProfile()
    {
        if (characters[activeCharacterIndex].genders[activeCreatureSex].eyeSettings.eyeMaterialProfile == gorgonEyeProfile)
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].eyeSettings.eyeMaterialProfile = normalEyeProfile;
        }
        else
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].eyeSettings.eyeMaterialProfile = gorgonEyeProfile;
        }

        characters[activeCharacterIndex].genders[activeCreatureSex].UpdateEyeMaterial(true);
    }
    //Updates app panel based on character data
    void UpdateColorButtonColorFromMeshData(bool changeActualIndex = false)
    {
        int index = characters[activeCharacterIndex].genders[activeCreatureSex].bodyMaterialSettings.bodyMaterialIndex;

        if (activeCharacterIndex == 5)
        {
            gorgonEyeSwitch.gameObject.SetActive(true);
        }
        else
        {
            gorgonEyeSwitch.gameObject.SetActive(false);
        }

        if (characters[activeCharacterIndex].altColorPanel != null)
        {
            paintToggle.gameObject.SetActive(false);
            if (characters[activeCharacterIndex].secondaryButtonColors.Count > 0)
            {
                secondaryBeastColorRep.gameObject.SetActive(true);
            }
            else
            {
                secondaryBeastColorRep.gameObject.SetActive(false);
            }
        }
        else
        {
            bool paintToggleValue = true;
            secondaryBeastColorRep.gameObject.SetActive(false);
            paintToggle.gameObject.SetActive(true);
            if (index > 4)
            {
                index -= 5;
                paintToggleValue = false;
                if (index > 4)
                {
                    paintToggle.interactable = false;
                }
                else
                {
                    paintToggle.interactable = true;
                }
            }
            else
            {
                paintToggle.interactable = true;
                paintToggleValue = true;
            }
            paintToggle.isOn = paintToggleValue;
        }
        if (changeActualIndex)
        {
            bodyColorIndex = index;
        }
        UpdateColorPickerButtonColor(0, index);

        if (characters[activeCharacterIndex].altSecondaryColorPanel == null)
            UpdateColorPickerButtonColor(1,characters[activeCharacterIndex].genders[activeCreatureSex].bodyMaterialSettings.skinTone);
        if (characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairMaterialProfile != null)
        UpdateColorPickerButtonColor(2, characters[activeCharacterIndex].genders[activeCreatureSex].hairSettings.hairMaterialIndex);
        UpdateColorPickerButtonColor(3, characters[activeCharacterIndex].genders[activeCreatureSex].eyeSettings.eyeIndex);
    }
    //Updates app panel based on character data
    void UpdateColorPickerButtonColor(int buttonIndex, int colorIndex)
    {
        if (buttonIndex == 0)
        {
            if (characters[activeCharacterIndex].altColorPanel != null)
            {
                colorPickerOpeners[buttonIndex].color = characters[activeCharacterIndex].altButtonColors[colorIndex];
                if (characters[activeCharacterIndex].secondaryButtonColors.Count != 0)
                {
                    secondaryBeastColorRep.color = characters[activeCharacterIndex].secondaryButtonColors[colorIndex];
                }
                return;
            }
            colorPickerOpeners[buttonIndex].color = beastColors[colorIndex];
            return;
        }

        if (buttonIndex == 1)
        {
            if (characters[activeCharacterIndex].altSecondaryColorPanel != null)
            {
                colorPickerOpeners[buttonIndex].color = characters[activeCharacterIndex].altSecondaryButtonColors[colorIndex];
                return;
            }
            colorPickerOpeners[buttonIndex].color = skinTones[colorIndex];
        }

        if (buttonIndex == 2)
            colorPickerOpeners[buttonIndex].color = hairColors[colorIndex-1];

        if (buttonIndex == 3)
            colorPickerOpeners[buttonIndex].color = eyeColors[colorIndex];
    }
    //Sets animator trigger
    public void PlayCharacterAnimation(int index)
    {
        ragdollWarningUI.SetActive(false);
        if (wanderCorrection.GetRagdollStatus())
        {
            wanderCorrection.DisableRagdoll();
        }

        if (index == 0)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Idle");
        if (index == 1)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Walk");
        if (index == 2)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Run");
        if (index == 3)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Fall");
        if (index == 4)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("CombatIdle");
        if (index == 5)
        {
            if (activeCharacterIndex == 7)
            {
                ChangeHeadTrack(false);
                lookAtToggle.SetIsOnWithoutNotify(false);
            }
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Attack_01");
        }
        if (index == 6)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Attack_02");
        if (index == 7)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Hit");
        if (index == 8)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Special_01");
    }
    //Sets animator trigger
    public void PlayMecanimAnimation(int index)
    {
        ragdollWarningUI.SetActive(false);
        if (index == 0)
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("NoWeapon");
            DisplayWeaponSet(0);
            DisplayMecanimButtons(0);
            return;
        }
        characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().ResetTrigger("NoWeapon");
        ChangeHeadTrack(false);
        lookAtToggle.SetIsOnWithoutNotify(false);
        if (index == 1)
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Sword");
            DisplayWeaponSet(1);
            DisplayMecanimButtons(1);
            return;
        }
        if (index == 2)
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Staff");
            DisplayWeaponSet(2);
            DisplayMecanimButtons(2);
            return;
        }
        if (index == 3)
        {
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Spear");
            DisplayWeaponSet(3);
            DisplayMecanimButtons(3);
            return;
        }
        if (index == 4)
            characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>().SetTrigger("Attack");
    }
    //Changes mecanim demo page
    void DisplayMecanimButtons(int index)
    {
        if (index == 0)
        {
            mecanimDemoButtons[0].SetActive(true);
            mecanimDemoButtons[1].SetActive(true);
            mecanimDemoButtons[2].SetActive(true);
            mecanimDemoButtons[3].SetActive(false);
            mecanimDemoButtons[4].SetActive(false);
            return;
        }
        if (index > 0)
        {
            mecanimDemoButtons[0].SetActive(false);
            mecanimDemoButtons[1].SetActive(false);
            mecanimDemoButtons[2].SetActive(false);
            mecanimDemoButtons[3].SetActive(true);
            mecanimDemoButtons[4].SetActive(true);
            return;
        }
    }
    //Shows demo weapons
    void DisplayWeaponSet(int index)
    {
        if (index == 0)
        {
            weapons.sword01.GetComponent<SimpleSetParent>().animator = null;
            weapons.sword02.GetComponent<SimpleSetParent>().animator = null;
            weapons.staff.GetComponent<SimpleSetParent>().animator = null;
            weapons.spear.GetComponent<SimpleSetParent>().animator = null;
            weapons.shield.GetComponent<SimpleSetParent>().animator = null;

            weapons.sword01.transform.SetParent(null);
            weapons.sword02.transform.SetParent(null);
            weapons.staff.transform.SetParent(null);
            weapons.spear.transform.SetParent(null);
            weapons.shield.transform.SetParent(null);

            weapons.sword01.SetActive(false);
            weapons.sword02.SetActive(false);
            weapons.staff.SetActive(false);
            weapons.spear.SetActive(false);
            weapons.shield.SetActive(false);
            return;
        }

        Animator ani = characters[activeCharacterIndex].genders[activeCreatureSex].GetComponent<Animator>();
        weapons.sword01.GetComponent<SimpleSetParent>().animator = ani;
        weapons.sword02.GetComponent<SimpleSetParent>().animator = ani;
        weapons.staff.GetComponent<SimpleSetParent>().animator = ani;
        weapons.spear.GetComponent<SimpleSetParent>().animator = ani;
        weapons.shield.GetComponent<SimpleSetParent>().animator = ani;

        if (index == 1)
        {
            weapons.sword01.SetActive(true);
            weapons.sword02.SetActive(true);
            weapons.staff.SetActive(false);
            weapons.spear.SetActive(false);
            weapons.shield.SetActive(false);
            return;
        }
        if (index == 2)
        {
            weapons.sword01.SetActive(false);
            weapons.sword02.SetActive(false);
            weapons.staff.SetActive(true);
            weapons.spear.SetActive(false);
            weapons.shield.SetActive(false);
            return;
        }
        if (index == 3)
        {
            weapons.sword01.SetActive(false);
            weapons.sword02.SetActive(false);
            weapons.staff.SetActive(false);
            weapons.spear.SetActive(true);
            weapons.shield.SetActive(true);
            return;
        }
    }
    //Displays ragdoll warning page
    public void WarnRagdoll()
    {
        ragdollWarningUI.SetActive(true);
    }
    //Activates character ragdoll
    public void EnableRagdoll()
    {
        ragdollWarningUI.SetActive(false);
        characters[activeCharacterIndex].ragdollTriggered[activeCreatureSex] = true;
        ChangeToPage(1);
        if (!wanderCorrection.GetRagdollStatus())
        {
            wanderCorrection.EnableRagdoll();
        }
    }
}
