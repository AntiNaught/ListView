using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollListCell : MonoBehaviour
{
    private float m_Width = 0;
    private float m_Height = 0;

    private float CalculateWidth()
    {
        //TODO
        return 0f;
    }

    private float CalculateHeight()
    {
        //TODO
        return 0f;
    }

    private bool m_WidthDirty = true;
    private bool m_HeightDirty = true;

    public float width
    {
        get 
        {
            if(m_WidthDirty)
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
            if(m_HeightDirty)
            {
                m_Height = CalculateHeight();
                m_HeightDirty = false;
            }
            return m_Height; 
        }
    }
}
