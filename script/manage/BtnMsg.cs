using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class BtnMsg : MonoBehaviour,IPointerDownHandler ,IPointerUpHandler 
{
    int bs;
    public GameObject gmj;
    public  void OnPointerUp(PointerEventData eventData)
    {
       ExecuteEvents.Execute<MYMSG>(gmj,null,(i,p)=>i.BtnUp(bs));
      
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ExecuteEvents.Execute<MYMSG>(gmj, null, (i, p) => i.BtnDW(bs));
    }
    // Use this for initialization
    void Start()
    {
        switch (this.transform.name)
        {
            case "上":
                bs = control.north;
                break;
            case "下":
                bs = control.south;
                break;
            case "左":
                bs = control.west;
                break;
            case "右":
                bs = control.east;
                break;
        }
    }

  
}
