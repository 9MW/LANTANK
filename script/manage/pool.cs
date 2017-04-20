using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Collections;

public class pool : MonoBehaviour
    {
        static MYDaleyEvent mevt=new MYDaleyEvent();
        static Dictionary<string, List<GameObject>> pol = new Dictionary<string, List<GameObject>>();
        public static bool AIPD = false;
        static float delaytim;
         static GameObject tmpGameobject;
   
    private void Awake()
    {
        mevt.AddListener(xc);

    }

      public static GameObject getGMOBJ(GameObject got, Vector3 wz, Quaternion xz)
        {
            string objnm = got.name;
            // Debug.Log("取的是"+objnm);
            if (AIPD)
            {
                objnm = "AIPD";
                AIPD = false;
            }
            if (pol.ContainsKey(objnm))
            {
                if (pol[objnm].Count != 0)
                {
                    GameObject spawn = pol[objnm][0];
                    spawn.transform.position = wz;
                    spawn.transform.rotation = xz;
                    spawn.SetActive(true);
                    pol[objnm].RemoveAt(0);
                    return spawn;
                 }
                else
                {
                    GameObject instanti = Instantiate(got, wz, xz) as GameObject;
                    instanti.name = objnm;
                    return instanti;
                 }
            }
            else
            {
                var ist = Instantiate(got, wz, xz) as GameObject;
                ist.name = objnm;
                pol.Add(objnm, new List<GameObject>());
                return ist;
             }
            
        }
    public static void put(GameObject pt, float tim = 0)
        {
        //   yield return new WaitForSeconds(tim);
        if (tim != 0)
        {
            Debug.Log("delaytime = "+tim);
            mevt.Invoke(pt, tim);
            return;
        }
            
        string nm = pt.name;
            pt.transform.parent = ScreenManager.boundary.transform;
          
                pt.SetActive(false);
                pol[nm].Add(pt);
            
            //   Debug.Log(nm+"已放入");
        }
    void xc(GameObject trg, float tm)
    {
        Debug.Log("Starting " + Time.time);
        StartCoroutine(e(trg, tm));
        Debug.Log("Done " + Time.time);
    }
    IEnumerator e(GameObject pt, float tim)
    {
        yield return new WaitForSeconds(tim);
        Debug.Log("WaitAndDebug.Log " + Time.time);
        pt.SetActive(false);
        put(pt);
    }
    private void Update()
    {
        if (delaytim != 0) {
            Invoke("dely",delaytim );
            delaytim = 0;
        }
    }
}
 class MYDaleyEvent: UnityEvent<GameObject,float>
{

}
