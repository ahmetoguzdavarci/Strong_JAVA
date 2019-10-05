using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR        //bu kodumuz sadece editörde derlencek.
using UnityEditor;      //editör sınıfını oluşturmak için tanımladığımız kütüphane.
#endif      //editör kodunun sonu.

public class Dusman_Kontrol : MonoBehaviour        //düşman nesnemizin içindeyiz.
{
    GameObject[] gidilecek_noktalar;        //çizdirdiğimiz nesneleri dizide tutmak için dizi tanımlıyoruz.

    bool aradaki_mesafeyi_bir_kere_al = true;       //kontrol amaçlı oluşturulan değişken.

    Vector3 aradaki_mesafe;     //nesnelerimizin konumları arasındaki mesafeyi tutmak için oluşturduğumuz değişken.

    int aradaki_mesafe_sayaci = 0;      //testere ilk çizilen nesneye gittikten sonra sırasıyla diğerlerine geçmesi için oluşturulan değişken.

    bool ilerimi_gerimi = true;     //testerenin hareket yönünü kontrol edecek olan kontrol.

    GameObject karakter;        //karakter JAVA ya ulaşmak için oluşturduğumuz nesne.

    RaycastHit2D ray;

    public LayerMask layer_mask;

    int hiz = 7;        //düşmanımızın hızını ayarlamak için kullandığımız değişken.

    public Sprite on_taraf;     //düşmanın bize baktığındaki görüntüsü.
    public Sprite arka_taraf;   //düşmaının bize bakmadığındaki görüntüsü.

    SpriteRenderer sprite_renderer;     //komponente ulaşmak için oluşturduğumuz değişken.

    public GameObject kursun;   //düşmanımızın ateş etmesi için kullanacağımız nesne.

    float ates_zamani = 0;      //düşmanın ateş etme aralığı.

    void Start()
    {
        gidilecek_noktalar = new GameObject[transform.childCount];      //dizimizin sayısını belirliyoruz.

        karakter = GameObject.FindGameObjectWithTag("Player");      //karakter JAVA yı nesnemize atadık.

        sprite_renderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < gidilecek_noktalar.Length; i++)     //dizi üzerinde işlem yapmak için kullandığımız döngü.
        {
            gidilecek_noktalar[i] = transform.GetChild(0).gameObject;       //diziden çıkarılacak nesneyi hep sıfırıncı indise atıyoruz ve aşağıda çıkartıyoruz.

            gidilecek_noktalar[i].transform.SetParent(transform.parent);        //çizilen ve dizide tutulan nesnemizi child olmaktan çıkarıyoruz.

        }

    }


    void FixedUpdate()
    {
        Beni_Gordu_Mu();

        if (ray.collider.tag == "Player")       //düşmanın karakterimizi gördüğü koşul.
        {
            hiz = 15;       //hızını arttıracak.

            sprite_renderer.sprite = on_taraf;      //düşman karakteri gördüğünde ön taraf görseli olucak.

            Ates_Et();
        }

        else       //düşmanımızın karakterimzi görmediği durum.
        {
            hiz = 7;        //hızını yavaşlatıcak.

            sprite_renderer.sprite = arka_taraf;    //düşman karakteri görmediğinde arka taraf görseli olucak.
        }

        Noktalara_Git();
    }

    void Ates_Et()
    {
        ates_zamani += Time.deltaTime;

        if (ates_zamani > Random.Range(0.2f, 1))
        {
            ates_zamani = 0;

            Instantiate(kursun, transform.position, Quaternion.identity);       //kurşun oluşturma kodumuz.
        }

    }

    void Beni_Gordu_Mu()
    {
        Vector3 ray_yonum = karakter.transform.position - transform.position;       //düşmanla karakter arasındaki konum farkını aldık.

        ray = Physics2D.Raycast(transform.position, ray_yonum, 1000, layer_mask);   //düşmanımızdan karakterimize doğru çizgi çizmek için yazdığımız kod.

        Debug.DrawLine(transform.position, ray.point, Color.magenta);       //çizgiyi ve rengini çizdirdiğimiz kod.

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

        transform.position += aradaki_mesafe * Time.deltaTime * hiz;     //testeremiz burada hareketine başlıyor.

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

    public Vector2 get_yon()
    {
        return (karakter.transform.position - transform.position).normalized;       //düşmanla karakter arasındaki konum farkının normalini alarak yön buluyoruz.
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
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);  //çizgi çizme kodumuz. İlk parametre nerden başlicağı, ikincisi nerede biteceği.
        }
    }

#endif      //editör koşul sonu.

}

#if UNITY_EDITOR        //build aşamasında derlenmemesi için koşulumuz.
[CustomEditor(typeof(Dusman_Kontrol))]      //yukarıdaki sınıfla iletişim halinde olmak için yazıdığımız kod.
[System.Serializable]
class Dusman_Kontrol_Editor : Editor
{
    public override void OnInspectorGUI()   //içerisine yazdığımız kodların sadece editörde çalışmasını sağlayan kod.
    {
        Dusman_Kontrol script = (Dusman_Kontrol)target;   //Testere_Kontrol sınıfımızı nesnemize atadık.

        if (GUILayout.Button("ÜRET", GUILayout.MinWidth(100), GUILayout.Width(100)))        //editörümüzde (panelde) üret isminde butonumuzu oluşturduk.
        {
            GameObject yeni_nesne = new GameObject();       //üret tuşuna bastığımzıda nesne üretiyoruz.

            yeni_nesne.transform.parent = script.transform;     //bu nesnemiz testeremizin alt nesnesi oluyor.
            yeni_nesne.transform.position = script.transform.position;      //üretilen alt nesneyi testeremizle aynı konuma atadık.
            yeni_nesne.name = script.transform.childCount.ToString();       //üretilen nesneye isim olarak sayısını atadık.
        }

        EditorGUILayout.Space();        //editördeki nesneler arasına boşluk koyuyoruz.

        EditorGUILayout.PropertyField(serializedObject.FindProperty("layer_mask"));     //editör kodu yazdıysak eğer, inspctor paneline açılmak için bu kod gerekli.

        EditorGUILayout.PropertyField(serializedObject.FindProperty("on_taraf"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("arka_taraf"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("kursun"));

        serializedObject.ApplyModifiedProperties();

        serializedObject.Update();
    }
}
#endif      //koşul bitişi.
