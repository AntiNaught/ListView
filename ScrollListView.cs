using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 滑动列表
/// </summary>
public class ScrollListView : UIBehaviour
{
    public enum MovementType
    {
        /// <summary>
        /// Unrestricted movement. The content can move forever.
        /// </summary>
        Unrestricted,

        /// <summary>
        /// Elastic movement. The content is allowed to temporarily move beyond the container, but is pulled back elastically.
        /// </summary>
        Elastic,

        /// <summary>
        /// Clamped movement. The content can not be moved beyond its container.
        /// </summary>
        Clamped,
    }

    public enum HorizontalScrollDirection
    {
        Disable,
        LeftToRight,
        RightToLeft,
    }

    public enum VerticalScrollDirection
    {
        Disable,
        TopToBottom,
        BottomToTop,
    }

    /// <summary>
    /// Enum for which behavior to use for scrollbar visibility.
    /// </summary>
    public enum ScrollbarVisibility
    {
        /// <summary>
        /// Always show the scrollbar.
        /// </summary>
        Permanent,

        /// <summary>
        /// Automatically hide the scrollbar when no scrolling is needed on this axis. The viewport rect will not be changed.
        /// </summary>
        AutoHide,

        /// <summary>
        /// Automatically hide the scrollbar when no scrolling is needed on this axis, and expand the viewport rect accordingly.
        /// </summary>
        /// <remarks>
        /// When this setting is used, the scrollbar and the viewport rect become driven, meaning that values in the RectTransform are calculated automatically and can't be manually edited.
        /// </remarks>
        AutoHideAndExpandViewport,
    }

    [Serializable]
    public class ScrollRectEvent : UnityEvent<Vector2> { }

    [SerializeField] protected RectOffset m_Padding = new RectOffset();
    [SerializeField] protected Vector2 m_Spacing = Vector2.zero;

    /// <summary>
    /// 
    /// </summary>
    [DisplayOnly]
    [SerializeField]
    private Vector2 m_NormalizedPosition = new Vector2();

    private Vector2 m_2DPosition = new Vector2();

    private bool m_Horizontal = false;
    private bool m_Vertical = false;

    private Vector2 m_ViewPortSize = new Vector2();

    #region Vertical
    /// <summary>
    /// 垂直滑动方向设置
    /// </summary>
    [SerializeField]
    private VerticalScrollDirection m_VerticalDirection = VerticalScrollDirection.Disable;
    public VerticalScrollDirection vertical
    {
        get { return m_VerticalDirection; }
        set { m_VerticalDirection = value; }
    }

    private float CalculateTotalHeight()
    {
        float h = m_Padding.top + m_Padding.bottom;
        for (int i = 0; i < m_CellList.Count; i++)
        {
            h += m_CellList[i].height + m_Spacing.x;
        }
        return h;
    }

    private bool m_DirtyVertical = true;
    private float m_TotalLengthVertical = 0f;
    public float totalHeight
    {
        get
        {
            if (m_DirtyVertical)
            {
                m_TotalLengthVertical = CalculateTotalHeight();
                m_DirtyVertical = false;
            }
            return m_TotalLengthVertical;
        }
    }

    #endregion Vertical

    #region Horizontal
    /// <summary>
    /// 水平滑动方向设置
    /// </summary>
    [SerializeField]
    private HorizontalScrollDirection m_HorizontalDirection = HorizontalScrollDirection.Disable;
    public HorizontalScrollDirection horizontal
    {
        get { return m_HorizontalDirection; }
        set { m_HorizontalDirection = value; }
    }

    private float CalculateTotalWidth()
    {
        float w = m_Padding.left + m_Padding.right;
        for (int i = 0; i < m_CellList.Count; i++)
        {
            w += m_CellList[i].width + m_Spacing.x;
        }
        return w;
    }

    private float CalculatePositoinX(float normalizedPositoinX)
    {
        float pos = normalizedPositoinX * (totalWidth - m_ViewPortSize.x);
        return pos;
    }

    private float CalculatePositionY(float normalizedPositionY)
    {
        float pos = normalizedPositionY * (totalHeight - m_ViewPortSize.y);
        return pos;
    }

    private bool m_DirtyHorizontal = true;
    private float m_TotalLengthHorizontal = 0f;
    public float totalWidth
    {
        get
        {
            if (m_DirtyHorizontal)
            {
                m_TotalLengthHorizontal = CalculateTotalWidth();
                m_DirtyHorizontal = false;
            }
            return m_TotalLengthHorizontal;
        }
    }
    #endregion Horizontal

    private List<ScrollListCell> m_CellList = null;

    private void Initialize()
    {
        m_CellList = ListPool<ScrollListCell>.Get();
    }

    #region ---------------- Unity 消息 ----------------------

    protected override void Awake()
    {
    }

    private void Update()
    {
    }

    protected override void OnDisable()
    {
    }

    protected override void OnDestroy()
    {
        ListPool<ScrollListCell>.Release(m_CellList);
    }

    #endregion ---------------- Unity 消息 ----------------------

    #region ---------------- API ----------------------
    public void BindCellDisplayCallback()
    {
    }

    public void BindCellWidthCallback(Func<float> getwidth)
    {
    }

    public void BindCellHeightCallback(Func<float> getheight)
    {
    }

    /// <summary>
    /// >Set the horizontal or vertical scroll position as a value between 0 and 1, with 0 being at the left or at the bottom.
    /// </summary>
    /// <param name="value">The position to set, between 0 and 1.</param>
    /// <param name="axis">The axis to set: 0 for horizontal, 1 for vertical.</param>
    public void SetNormalizedPosition(float value, int axis)
    {
        if(axis == 0)
        {
            m_NormalizedPosition.x = value;
        }
        else if(axis == 1)
        {
            m_NormalizedPosition.y = value;
        }

    }
    #endregion ---------------- API ----------------------
}