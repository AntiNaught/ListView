using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollListCell : MonoBehaviour
{
    private float m_Width = 0;
    private float m_Height = 0;
    private Vector2 m_Location = new Vector2();
    private int index = 0;
    private int type = 0;

    public ScrollListCell(int index, int type, Transform trans)
    {
    }

    public void Init(int index, int type)
    {
        this.index = index;
        this.type = type;
        m_HeightDirty = true;
        m_WidthDirty = true;
    }

    private float CalculateWidth()
    {
        //TODO
        return 100f;
    }

    private float CalculateHeight()
    {
        //TODO
        return 30f;
    }

    private bool m_WidthDirty = true;
    private bool m_HeightDirty = true;

    public float width
    {
        get
        {
            if (m_WidthDirty)
            {
                m_Width = CalculateWidth();
                m_WidthDirty = false;
            }
            return m_Width;
        }
    }

    public float height
    {
        get
        {
            if (m_HeightDirty)
            {
                m_Height = CalculateHeight();
                m_HeightDirty = false;
            }
            return m_Height;
        }
    }

    public Vector2 location
    {
        get
        {
            return m_Location;
        }
        set
        {
            m_Location = value;
        }
    }

    public void UpdateLocation()
    {
        transform.localPosition = location;
    }
}