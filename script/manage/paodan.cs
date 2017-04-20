using UnityEngine;
using System.Collections;

    public class paodan : MonoBehaviour
    {
        int i = 0;
        public GameObject explod1;
        public GameObject enemyexplod;
        AudioSource bullet_source;
        public AudioClip zhuan_clip;
        public AudioClip plate_clip;
        public AudioClip enemy_killclip;
        public AudioClip enemy_clip;
        AI smz;
        // Use this for initialization
        GameObject prefab;
        GuiAndManage GM;
        private void Start()
        {
        GM = GameObject.FindObjectOfType<GuiAndManage>();
        bullet_source = gameObject.GetComponent<AudioSource>();
        pool.put(gameObject, 6);
        }

        void OnTriggerEnter2D(Collider2D e)
        {
            // Collider[] cols = Physics.OverlapSphere(transform.position, 0.2f);
            // for(int i=0;i<cols.Length;i++){
            //istEnemy.Add（e.gameObject）;
            if (e.gameObject.name == "砖块")
            {
                i++;
                Destroy(e.gameObject);
                if (i >= 20)
                {
                    prefab = pool.getGMOBJ(explod1, transform.position, transform.rotation);
                pool.put(prefab, 0.2f);
                    pool.put(gameObject);


                    //Debug.Log("砖块" + i);
                }
                // bullet_source.clip = zhuan_clip;
                //bullet_source.Play();
            }
            else if (e.gameObject.name == "界")
            {
                pool.put(gameObject);
            }
            if (e.gameObject.CompareTag("enemy") && name != "AIPD")//没有AIPD的判断会炸死AI自身
            {
                int z = Random.Range(1, 2);
                smz = e.gameObject.GetComponent<AI>();

                //Debug.Log(z);
                if (z == 1)
                {
                    smz.hp--;
                    if (smz.hp == 0)
                    {
                        GameObject pre = pool.getGMOBJ(explod1, transform.position, transform.rotation);
                        pool.put(e.gameObject);
                    pool.put(pre, 0.2f);
                       GM.onenemykill( e.gameObject);//将炮弹作为摄像机子物体
                        bullet_source.clip = enemy_killclip;
                        bullet_source.Play();
                    }
                }
                else
                {
                    pool.put(this.gameObject);
                    bullet_source.clip = enemy_clip;
                    bullet_source.Play();
                }
                pool.put(gameObject);
            }
        }
    }
    // }

    //pool.put(gameObject);



