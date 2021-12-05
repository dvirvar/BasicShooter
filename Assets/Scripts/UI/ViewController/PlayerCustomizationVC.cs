using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerCustomizationVC : ViewController
{
    [SerializeField] private Text subTitleText;
    #region For coloring the characterModel
    [SerializeField] private ColorPicker colorPicker;
    [SerializeField] private PlayerPreview playerPreview;

    private Color originalJelly;
    private Color originalDonut;
    private DonutColorable currentObjectToColor; //0 -> Donut, 1 -> Jelly
    #endregion
    #region For rotating the characterModel
    private Quaternion originalRotation;
    private Vector3 previousRotation;
    private Vector3 previousDelta;
    #endregion
    #region For not rotating the characterModel when we choose a color
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private EventSystem eventSystem;
    #endregion
    #region For camera animations
    private Animator mainMenuCameraAnimator;
    #endregion
    #region For server requests
    [SerializeField] private Popup popupPanel;
    private CharacterSocketService characterSocketService;
    private SocketHandler socketHandler;
    #endregion

    private void Awake()
    {
        mainMenuCameraAnimator = mainCamera.GetComponent<Animator>();
        socketHandler = FindObjectOfType<SocketHandler>();
        characterSocketService = new CharacterSocketService(socketHandler);
    }

    void Start()
    {
        #region For coloring
        colorPicker.onValueChanged.AddListener(color =>
        {
            playerPreview.setColorTo(currentObjectToColor, color);
        });
        #endregion
        #region For rotating
        originalRotation = playerPreview.characterModel.transform.parent.localRotation;
        previousRotation = originalRotation.eulerAngles;
        previousDelta = Vector3.zero;
        #endregion
    }

    private void OnEnable()
    {
        mainMenuCameraAnimator.SetTrigger("Move");
        subTitleText.text = "Select the part of the donut you want to color";
        colorPicker.gameObject.SetActive(false);
        originalJelly = playerPreview.getColor(DonutColorable.jelly);
        originalDonut = playerPreview.getColor(DonutColorable.donut);
    }

    private void OnDisable()
    {
        mainMenuCameraAnimator.SetTrigger("GoIdle");//For now we do it on OnDisable
        playerPreview.transform.localRotation = originalRotation;
        popupPanel.isActive = false;
        popupPanel.setText("Saving, Please wait");
    }

    private void Update()
    {
        if(Input.GetButton(StaticStrings.Weapon.fire))
        {
            PointerEventData pointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);
            RaycastResult ray = results.Find(result => result.gameObject.transform == colorPicker.transform);
            if (ray.gameObject != colorPicker.gameObject) {
                previousDelta = Input.mousePosition - previousRotation;
                playerPreview.transform.Rotate(playerPreview.transform.up, -Vector3.Dot(previousDelta, mainCamera.transform.right), Space.World);
            }
        }
        previousRotation = Input.mousePosition;
    }

    public void jellyColorPressed()
    {
        currentObjectToColor = DonutColorable.jelly;
        colorPicker.AssignColor(playerPreview.getColor(currentObjectToColor));
        colorPicker.gameObject.SetActive(true);
        subTitleText.text = "Select a color for the jelly";
    }

    public void donutColorPressed()
    {
        currentObjectToColor = DonutColorable.donut;
        colorPicker.AssignColor(playerPreview.getColor(currentObjectToColor));
        colorPicker.gameObject.SetActive(true);
        subTitleText.text = "Select a color for the donut";
    }

    public void savePressed()
    {
        Color donutColor = playerPreview.getColor(DonutColorable.donut);
        Color jellyColor = playerPreview.getColor(DonutColorable.jelly);
        popupPanel.isActive = true;
        characterSocketService.customizeDonut(jellyColor, donutColor, delegate (BasicSocketResponse response) {
            customizePlayerCallback(response);
        });
    }

    private void customizePlayerCallback(BasicSocketResponse response)
    {
        popupPanel.showCloseBtn();
        if (response.parsedResponse.permission)
        {
            this.navigationController.pop();
        }
        else
        {
            popupPanel.setText(response.parsedResponse.reason);
        }
    }

    public override void backButtonPressed()
    {
        playerPreview.setColorTo(DonutColorable.donut, originalDonut);
        playerPreview.setColorTo(DonutColorable.jelly, originalJelly);
        base.backButtonPressed();
    }

    private void OnDestroy()
    {
        colorPicker.onValueChanged.RemoveAllListeners();
    }
}
