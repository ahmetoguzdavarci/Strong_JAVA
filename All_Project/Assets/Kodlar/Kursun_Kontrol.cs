using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kursun_Kontrol : MonoBehaviour     //kurşun nesnemizin içindeyiz.
{
    Dusman_Kontrol dusman;      //diğer scripte ulaşmak için oluşturduğumuz nesne.

    Rigidbody2D fizik;      //kurşunumuzun komponentine ulaşmak için oluşturduğumuz değişken.

    void Start()
    {
        dusman = GameObject.FindGameObjectWithTag("dusman_tag").GetComponent<Dusman_Kontrol>();     //diğer scriptimize ulaşıp nesnemize atadık.

        fizik = GetComponent<Rigidbody2D>();        //komponentimizi değişkenimize atıyoruz.

        fizik.AddForce(dusman.get_yon()*1000);       //diğer scriptten yönü add force a atıyoruz.
    }


    void Update()
    {
        
    }
}
