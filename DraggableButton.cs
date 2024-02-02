using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System; // 确保包含这个命名空间以使用 Action
public class DraggableButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Button mainButton;
    public Toggle dragToggle;
    public Animator animator;
    public string animationToPlay;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public event System.Action OnAnimationFinished;
    //public event Action<DraggableButton> OnDroppedOnMainButton;
    public bool isRequiredButton = false; // 新增字段，标记是否为requiredDraggableButton

    public void Initialize(Button mainButton, Toggle dragToggle, Animator animator, string animationToPlay)
    {
        this.mainButton = mainButton;
        this.dragToggle = dragToggle;
        this.animator = animator;
        this.animationToPlay = animationToPlay;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        originalPosition = rectTransform.anchoredPosition;
    }
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent, eventData.position, eventData.pressEventCamera, out position);
        rectTransform.anchoredPosition = position;
    }

   // 移除 OnDroppedOnMainButton 事件声明

public void OnEndDrag(PointerEventData eventData)
{
    canvasGroup.alpha = 1f;
    canvasGroup.blocksRaycasts = true;

    bool isOverMainButton = RectTransformUtility.RectangleContainsScreenPoint(mainButton.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera);

    if (dragToggle.isOn && isRequiredButton && isOverMainButton)
    {
        // 如果这个按钮是 requiredDraggableButton 且拖拽到了 mainButton 上
        animator.Play(animationToPlay); // 播放动画
        gameObject.SetActive(false); // 隐藏此按钮
        mainButton.gameObject.SetActive(false); // 隐藏 mainButton
    }
    else
    {
        // 不满足上述条件，按钮回到原位
        rectTransform.anchoredPosition = originalPosition;
    }
}


    public void AnimationFinished()
    {
        // 觸發外部訂閱的事件
        OnAnimationFinished?.Invoke();
    }
}
