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

    [SerializeField]
    private GameObject prefab;

    /// <summary>
    /// 
    /// </summary>
    [DisplayOnly]
    [SerializeField]
    private Vector2 m_NormalizedPosition = new Vector2();

    private Vector2 m_2DPosition = new Vector2();

    [SerializeField]
    private bool m_Horizontal = false;
    [SerializeField]
    private bool m_Vertical = false;

    [SerializeField]
    private RectTransform m_ViewPort;

    private int m_BordIdx_Top = 1;
    private int m_BordIdx_Bottom = 1;
    private int m_BordIdx_Left = 1;
    private int m_BordIdx_Right = 1;

    #region Vertical

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
        float pos = normalizedPositoinX * (totalWidth - m_ViewPort.rect.width);
        return pos;
    }

    private float CalculatePositionY(float normalizedPositionY)
    {
        float pos = normalizedPositionY * (totalHeight - m_ViewPort.rect.height);
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

    private bool m_initialized = false;
    private void Initialize()
    {
        m_CellList = ListPool<ScrollListCell>.Get();
        // 初始化每个 cell 的 location
        m_initialized = true;

        // 设置宽高都为 dirty 以使其初始化
        m_DirtyHorizontal = true;
        m_DirtyVertical = true;
    }

    private void FindBordCellIndex(int axis)
    {
        if (axis == 0)
        {
        }
        else if (axis == 1)
        {
        }
    }

    #region ---------------- Unity 消息 ----------------------

    protected override void Awake()
    {
        Initialize();
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
    public void Refresh()
    {
        ClearCellList();
        for (int i = 0; i < 15; i++)
        {
            CreateCellData(i, 1);
        }

        float offset_x = m_Padding.top;
        float offset_y = m_Padding.left;

        for (int i = 0; i < m_CellList.Count; i++)
        {
            ScrollListCell cell = m_CellList[i];
            if (m_Horizontal) offset_x += cell.width;
            if (m_Vertical) offset_y += cell.height;
            cell.location = new Vector2(offset_x, offset_y);

            cell.UpdateLocation();

            offset_x += m_Spacing.x;
            offset_y += m_Spacing.y;
        }

        SetNormalizedPosition(0, 0);
        SetNormalizedPosition(0, 1);
    }

    public void ClearCellList()
    {
        m_CellList.Clear();
    }

    public void CreateCellData(int index, int type)
    {
        print("创建了一个");
        Transform celltrans = Instantiate<GameObject>(prefab, transform, false).transform;
        ScrollListCell cell = celltrans.GetComponent<ScrollListCell>();
        cell.Init(index, type);
        m_CellList.Add(cell);
    }

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
        if (axis == 0)
        {
            m_NormalizedPosition.x = value;
            // 根据 normalized position 计算出 2d position
            m_2DPosition.x = CalculatePositoinX(value);
        }
        else if (axis == 1)
        {
            m_NormalizedPosition.y = value;
            m_2DPosition.y = CalculatePositionY(value);
        }
    }
    #endregion ---------------- API ----------------------
}