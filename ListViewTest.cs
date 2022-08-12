using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListViewTest : MonoBehaviour
{
    public ScrollListView listview;
    // Start is called before the first frame update
    void Start()
    {
        listview.Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            listview.Refresh();
    }
}
