using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponListDisplay : Display<WeaponListItem>, IDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler,IPointerClickHandler,IPointerDownHandler
{
    public event Action<WeaponListDisplay> OnBeganDragging = delegate { };
    public event Action<WeaponListDisplay, int> OnStoppedDragging = delegate { };
    public event Action<WeaponStats> OnBeganHovering = delegate { };
    public event Action OnStoppedHovering = delegate { };
    public event Action<WeaponListDisplay, PointerEventData.InputButton,int> OnClicks = delegate { };
    public event Action<WeaponListDisplay, bool> OnMouseButtonUp = delegate { };
    public event Action<WeaponListDisplay> OnBeganHolding = delegate { };
    public event Action<WeaponListDisplay, Transform, int, PointerEventData> OnStoppedHolding = delegate { };

    [SerializeField] private Image weaponImage;
    [SerializeField] private Text weaponNameText;
    private Animator animator;
    private WeaponStats weaponStats => info.weaponStats;

    private ChosenGunMasterWeaponDisplay chosenGunMasterWeaponDisplay;
    private Transform parentTransform;
    private int indexInTableView;
    private bool beganHolding = false;
    private bool isHeld = false;
    private float heldTime = 0;

    [HideInInspector] public bool chosenState = false;
    [HideInInspector] public RectTransform rectTransform;

    protected override void setView(WeaponListItem info)
    {
        weaponNameText.text = info.name;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isHeld)
        {
            heldTime += Time.deltaTime;
            if (heldTime > 0.2 && !beganHolding)
            {
                parentTransform = transform.parent;
                OnBeganHolding(this);
                beganHolding = true;
            }
        }
    }

    public void setChosenGunMasterWeaponDisplay(ChosenGunMasterWeaponDisplay chosenGunMasterWeaponDisplay)
    {
        this.chosenGunMasterWeaponDisplay = chosenGunMasterWeaponDisplay;
        this.chosenState = true;
    }

    public ChosenGunMasterWeaponDisplay getChosenGunMasterWeaponDisplay()
    {
        return chosenGunMasterWeaponDisplay;
    }

    public void clearChosenGunMasterWeaponDisplay()
    {
        if (chosenGunMasterWeaponDisplay != null)
        {
            chosenGunMasterWeaponDisplay.clear();
            chosenGunMasterWeaponDisplay = null;
            chosenState = false;
        }
    }

    public void shrinkAnimation()
    {
        animator.SetBool("Shrink", true);
    }

    public void fastShrink()
    {
        //TODO: Find a way to fast shrink
        animator.SetBool("Shrink", true);
    }

    public void growAnimation()
    {
        animator.SetBool("Shrink", false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging && !isHeld)
        {
            indexInTableView = transform.GetSiblingIndex();
            OnBeganHovering(weaponStats);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!eventData.dragging && !isHeld)
        {
            OnStoppedHovering();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseButtonUp(this,beganHolding);
        if (beganHolding)
        {
            OnStoppedHolding(this, parentTransform, indexInTableView, eventData);
        }
        isHeld = false;
        heldTime = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging && !beganHolding)
        {
            OnClicks(this, eventData.button, eventData.clickCount);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        beganHolding = false;
        isHeld = true;
    }
}
