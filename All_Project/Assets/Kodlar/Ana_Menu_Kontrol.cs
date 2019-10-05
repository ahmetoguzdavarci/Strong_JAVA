using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ana_Menu_Kontrol : MonoBehaviour       //ana menüdeki kameramızın içindeyiz.
{
    GameObject leveller;        //levellere ulaşmak için oluşturduğumuz değişken.

    GameObject kilitler;        //kilit resimlerine ulaşmak için oluşturduğumuz değişken.

    void Start()
    {
        //PlayerPrefs.DeleteAll();      //oyun test edilirkenki kayıtları silecek olan kodumuz.

        leveller = GameObject.Find("Leveller");     //leveller nesnemizi atıyoruz.

        kilitler = GameObject.Find("Kilitler");     //kilitler nesnemizi atıyoruz.

        for (int i = 0; i < leveller.transform.childCount; i++)     //level sayımız kadar dönecek olan döngümüz.
        {
            leveller.transform.GetChild(i).gameObject.SetActive(false);     //levellerimizi false yapacak.
        }

        for (int i = 0; i < kilitler.transform.childCount; i++)
        {
            kilitler.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("kacinci_level"); i++)       //kayıtlardan kaçıncı levelde kaldıysak oraya kadar çalışacak döngümüz.
        {
            leveller.transform.GetChild(i).GetComponent<Button>().interactable = true;      //kaçıncı levelde kaldıysak oraya kadar olanları aktif edicek kod.
        }
    }

    public void Buton_Sec(int gelen_buton)
    {
        if (gelen_buton == 1)   //basılan buton 1 ise koşulu.
        {
            SceneManager.LoadScene(1);      //start düğmesine tıklanınca ilk bölümden başlıyor oyun.
        }

        else if (gelen_buton == 2)  //basılan buton 2 ise koşulu.
        {
            for (int i = 0; i < kilitler.transform.childCount; i++)     //döngümüz kilit sayısı kadar dönücek.
            {
                kilitler.transform.GetChild(i).gameObject.SetActive(true);      //tüm kilitleri görünür yapacak.
            }

            for (int i = 0; i < leveller.transform.childCount; i++)     //level sayımız kadar dönecek olan döngümüz.
            {
                leveller.transform.GetChild(i).gameObject.SetActive(true);     //levellerimizi görünür yapacak.
            }

            for (int i = 0; i < PlayerPrefs.GetInt("kacinci_level"); i++)       //kayıtlardan kaçıncı levelde kaldıysak oraya kadar çalışacak döngümüz.
            {
                kilitler.transform.GetChild(i).gameObject.SetActive(false);     //kilit resimlerimiz kayıtlı olan levele kadar kaldırılacak.
            }
        }

        else if (gelen_buton == 3)  //basılan buton 3 ise koşulu.
        {
            Application.Quit();     //oyundan çıkış yapacak.
        }

    }

    public void Leveller_Buton(int gelen_level)
    {
        SceneManager.LoadScene(gelen_level);
    }
}
