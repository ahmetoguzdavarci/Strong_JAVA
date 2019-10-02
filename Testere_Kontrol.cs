using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR        //bu kodumuz sadece editörde derlencek.
using UnityEditor;      //editör sınıfını oluşturmak için tanımladığımız kütüphane.
#endif      //editör kodunun sonu.

public class Testere_Kontrol : MonoBehaviour        //testere nesnemizin içindeyiz.
{
    GameObject[] gidilecek_noktalar;        //çizdirdiğimiz nesneleri dizide tutmak için dizi tanımlıyoruz.

    bool aradaki_mesafeyi_bir_kere_al = true;       //kontrol amaçlı oluşturulan değişken.

    Vector3 aradaki_mesafe;     //nesnelerimizin konumları arasındaki mesafeyi tutmak için oluşturduğumuz değişken.

    int aradaki_mesafe_sayaci = 0;      //testere ilk çizilen nesneye gittikten sonra sırasıyla diğerlerine geçmesi için oluşturulan değişken.

    bool ilerimi_gerimi = true;     //testerenin hareket yönünü kontrol edecek olan kontrol.

    void Start()
    {
        gidilecek_noktalar = new GameObject[transform.childCount];      //dizimizin sayısını belirliyoruz.

        for (int i = 0; i < gidilecek_noktalar.Length; i++)     //dizi üzerinde işlem yapmak için kullandığımız döngü.
        {
            gidilecek_noktalar[i] = transform.GetChild(0).gameObject;       //diziden çıkarılacak nesneyi hep sıfırıncı indise atıyoruz ve aşağıda çıkartıyoruz.

            gidilecek_noktalar[i].transform.SetParent(transform.parent);        //çizilen ve dizide tutulan nesnemizi child olmaktan çıkarıyoruz.

        }
        
    }


    void FixedUpdate()
    {
        transform.Rotate(0, 0, 5);

        Noktalara_Git();
    }

    void Noktalara_Git()
    {
        if (aradaki_mesafeyi_bir_kere_al)
        {
            aradaki_mesafe = (gidilecek_noktalar[aradaki_mesafe_sayaci].transform.position - transform.position).normalized;    //çizilen nesneyle testere arasındaki mesafeyi bir kere alıyoruz.
                                                                                                                                //normalize ettiğimizde bize hep sıfır ve bir arasında değer üretiyor.
            aradaki_mesafeyi_bir_kere_al = false;
        }
        float mesafe = Vector3.Distance(transform.position, gidilecek_noktalar[aradaki_mesafe_sayaci].transform.position);      //testeremiz sonsuza gitmesin diye aradaki mesafeyi float
                                                                                                                                //türünden bulduk ve kullanıcaz.

        transform.position += aradaki_mesafe * Time.deltaTime * 10;     //testeremiz burada hareketine başlıyor.

        if (mesafe < 0.5f)      //testerenin çizime yaklaşma koşulu.
        {
            aradaki_mesafeyi_bir_kere_al = true;        //tekrar koşulu sağlatmak için kontrol değişkeni true yapılıyor.

            if (aradaki_mesafe_sayaci == gidilecek_noktalar.Length - 1)     //sıfırdan başlayıp dizinin tüm indizlerini gezicek ve eşit olduğunda
            {
                ilerimi_gerimi = false;                                 //kontrol değişkenimiz false olucak.
            }

            else if (aradaki_mesafe_sayaci == 0)                        //geriye doğru gelicek dizi üzerinde ve sıfır olduğunda
            {
                ilerimi_gerimi = true;                                  //kontrol değişkenimiz true olucak.
            }

            if (ilerimi_gerimi)
            {
                aradaki_mesafe_sayaci++;        //koşul sağlandığında sayaç artıcak ve dizi indisi gibi kullanılacak.
            }

            else
            {
                aradaki_mesafe_sayaci--;        //kontrol değişkenimiz false olduğunda geriye doğru gidicek dizi üzerinde.
            }           
        }
    }


#if UNITY_EDITOR    //bu koşul altındaki kodlar sadece editörde derlenicek. Yani unity panelinde.

    void OnDrawGizmos()     //sahnemize çizim yapmak için çalışan fonksiyon.
    {
        for (int i = 0; i < transform.childCount; i++)      //ürettiğimiz nesne kadar çizim yaptırmak için kullandığımız döngü.
        {
            Gizmos.color = Color.red;       //çizilen nesneye kırmızı rengini atadık.
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);     //ne çizileceğini yazdık.İlk parametre konum, ikinci parametre yarıçap değeri.
        }

        for (int i = 0; i < transform.childCount - 1; i++)      //çizdirdiğimiz nesneler arası çizgi çekmek için kullandığımız döngü.
        {
            Gizmos.color = Color.blue;      //çizilen çizgiye mavi rengi atadık.
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position);  //çizgi çizme kodumuz. İlk parametre nerden başlicağı, ikincisi nerede biteceği.
        }
    }

#endif      //editör koşul sonu.

}

#if UNITY_EDITOR        //build aşamasında derlenmemesi için koşulumuz.
[CustomEditor(typeof(Testere_Kontrol))]      //yukarıdaki sınıfla iletişim halinde olmak için yazıdığımız kod.
[System.Serializable]
class Testere_Editor : Editor
{
    public override void OnInspectorGUI()   //içerisine yazdığımız kodların sadece editörde çalışmasını sağlayan kod.
    {
        Testere_Kontrol script = (Testere_Kontrol)target;   //Testere_Kontrol sınıfımızı nesnemize atadık.

        if (GUILayout.Button("ÜRET", GUILayout.MinWidth(100), GUILayout.Width(100)))        //editörümüzde (panelde) üret isminde butonumuzu oluşturduk.
        {
            GameObject yeni_nesne = new GameObject();       //üret tuşuna bastığımzıda nesne üretiyoruz.

            yeni_nesne.transform.parent = script.transform;     //bu nesnemiz testeremizin alt nesnesi oluyor.
            yeni_nesne.transform.position = script.transform.position;      //üretilen alt nesneyi testeremizle aynı konuma atadık.
            yeni_nesne.name = script.transform.childCount.ToString();       //üretilen nesneye isim olarak sayısını atadık.
        }         
    }
}
#endif      //koşul bitişi.
