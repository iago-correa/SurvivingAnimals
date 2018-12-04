using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class SlowFlash : MonoBehaviour{
 
    public Text text;
 
     // Use this for initialization
    void Start (){
        
    }
 
     // Update is called once per frame
    void Update(){
        text.color = new Color(text.color.r,text.color.g,text.color.b, Mathf.Sin(Time.time * 7));
    }
 }