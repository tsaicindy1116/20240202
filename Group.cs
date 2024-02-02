using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Group : MonoBehaviour
{
    public Image image; // 群組中的圖片
    public Button mainButton; // 主要按鈕
    public List<DraggableButton> draggableButtons; // 可拖曳的按鈕列表
    public Animator animator; // 動畫控制器
    public Toggle dragToggle; // 勾選框
    public string animationToPlay; // 要播放的動畫名稱
    public GroupManager groupManager; // 對 GroupManager 的引用
    public DraggableButton requiredDraggableButton; // 需要拖拽的按钮
    //private bool animationFinished = true;

    void Start()
    {
        mainButton.onClick.AddListener(OnMainButtonClick);
        foreach (DraggableButton draggableButton in draggableButtons)
        {
            draggableButton.Initialize(mainButton, dragToggle, animator, animationToPlay);
            // 设置 isRequiredButton 标志
            draggableButton.isRequiredButton = (draggableButton == requiredDraggableButton);
        }
    }

    private void HandleDraggableButtonDropped(DraggableButton droppedButton)
    {
        // 如果 Toggle 没有勾选，所有按钮都应该弹回原位，这在 DraggableButton 脚本中已经处理
        // 当 Toggle 被勾选且是 requiredDraggableButton 被拖拽到 mainButton 上时
        if (droppedButton == requiredDraggableButton && dragToggle.isOn)
        {
            // 检查是否放置在 mainButton 上
            if (RectTransformUtility.RectangleContainsScreenPoint(mainButton.GetComponent<RectTransform>(), Input.mousePosition, null))
            {
                animator.Play(animationToPlay); // 播放动画
                droppedButton.gameObject.SetActive(false); // 隐藏 requiredDraggableButton
                mainButton.gameObject.SetActive(false); // 隐藏 mainButton
               // animationFinished = false; // 将动画完成标志设置为 false
            }
        }
        // 不需要额外的else语句，因为按钮回到原位的逻辑已在 DraggableButton 的 OnEndDrag 中处理
    }

    void OnMainButtonClick()
    {
        // 检查 Toggle 的 isOn 状态
        if (!dragToggle.isOn)
        {
            // 如果 Toggle 没有被勾选
            animator.Play(animationToPlay); // 播放动画
            mainButton.gameObject.SetActive(false); // 隐藏 mainButton
            //animationFinished = false; // 将动画完成标志设置为 false
        }
        // 如果 Toggle 被勾选，不执行任何操作
    }

    //    public void AnimationFinished()
    // {
    //     // 确保这是在调用 ShowNextGroup 之前输出
    //     Debug.Log("Received AnimationFinished message and called ShowNextGroup.");
    //     groupmanager.ShowNextGroup();
    // }
    
}
