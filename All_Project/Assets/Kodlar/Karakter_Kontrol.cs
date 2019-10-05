using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;      //telefonda oynayabilmek için eklediğimiz kütüphane.

public class Karakter_Kontrol : MonoBehaviour       //ana karakterimiz olan JAVA nın içindeyiz.
{

    public Sprite[] bekleme_animasyon;      //animasyon yapmak için arka arkaya fotoğrafları kullancağımızdan dolayı 
    public Sprite[] ziplama_animasyon;      //dizi oluşturduk ve bu fotoğrafları bu dizilere atayacağız. Daha sonra
    public Sprite[] yurume_animasyon;       //bu fotğrafları diziden sırasıyla çağırarak hareket ediyormuş gibi görsel oluşturacağız.

    public Text can_text;        //karakterimizin canını tutacağımız değişken.

    public Text altin_text;     //topladığımız altınların sayısını yazdıracağımız değişken.

    public Image siyah_arka_plan;       //karakter öldükten sonra ekranda gözükecek olan siyah ekran için oluşturduğumuz nesne

    int can = 100;      //karakterimizin canı.    

    SpriteRenderer sprite_renderer;     //komponente ulaşmak için oluşturduğumuz değişken.

    int bekleme_anim_sayac = 0;     //dizimizin elemanlarına ulaşmak için oluşturduğumuz değişken.
    int yurume_anim_sayac = 0;      //dizimizin elemanlarına ulaşmak için oluşturduğumuz değişken.
    int altin_sayac = 0;        //karakterin topladığı altınları tutacak olan deişkenimiz.

    Rigidbody2D fizik;      //nesnemizin rigifbody komponentine ulaşmak için oluşturduğumuz değişken.

    Vector3 vec;        //nesnemize düzlem üzerinde hareket vermek için oluşturduğumuz değişken.

    Vector3 kamera_ilk_pos;     //kameramızın ilk konumunu yakalamak için oluşturduğumuz değişken.
    Vector3 kamera_son_pos;     //kameramızın son konumunu yakalamak için oluşturduğumuz değişken.

    float horizontal = 0;       //nesnemize X ekseninde hareket vermek için oluşturduğumuz değişken.

    float bekleme_animasyon_zamani = 0;     //animasyonun yani diziden çekilen fotoğrafın zaman aralığını belirlemek için oluşturduğumuz değişken.

    float yurume_animasyon_zamani = 0;        //animasyonun yani diziden çekilen fotoğrafın zaman aralığını belirlemek için oluşturduğumuz değişken.

    float siyah_arka_plan_sayaci = 0;       //arka plan renginin şeffaflığını ayarlayacağımız değişkenimiz.

    float ana_menuye_donme_zamani = 0;      //karakter öldüğünde siyah ekrandan sonra menüye dönme zamanını belirlemek için oluşturulan değişken.

    bool bir_kere_zipla = true;

    GameObject kamera;      //kameramıza ulaşmak için oluşturduğumuz nesne.
    

    void Start()
    {
        siyah_arka_plan.gameObject.SetActive(false);        //başlangıçta siyah ekran kapalı yaptık. Çünkü telefon ekranında tuşlar gözükmüyordu.

        sprite_renderer = GetComponent<SpriteRenderer>();       //değişkenimize komponentin özelliklerini atadık.

        fizik = GetComponent<Rigidbody2D>();        //komponentin özelliklerini nesnemize atadık.

        kamera = GameObject.FindGameObjectWithTag("MainCamera");    //kameramıza ulaşıp nesnemize atadık.

        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("kacinci_level"))     //oynanan bölüm kayıtlı bölümden büyükse koşulumuz.
        {
            PlayerPrefs.SetInt("kacinci_level", SceneManager.GetActiveScene().buildIndex);      //oynanan bölümün indexini kaydediyoruz.
        }

        

        kamera_ilk_pos = kamera.transform.position - transform.position;    //kamera ile karakterimizin arasındaki uzaklığı bulup değişkenimize atadık.

        can_text.text = "CAN  " + can;      //karakterimizin canını ekrana yazdırdık.

        altin_text.text = altin_sayac + " / 30";        //oyun başladığında ekrana yazılacak altın miktarı.
    }

    void Update()       //klavyeden girişleri bu fonksiyonun içine yazıyoruz.
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))    // Telefonda çalışsın diye değiştirdik cross la.
                                                                //Input.GetKeyDown(KeyCode.Space) space tuşuna basıldığında çalışacak koşul.
        {
            if (bir_kere_zipla)     //her basıldığında zıplamaması için yazdığımız koşul.
            {
                fizik.AddForce(new Vector2(0, 500));        //nesnemize Y ekseninde kuvvet uygulayacak.

                bir_kere_zipla = false;     //ikinciye zıplamicak.
            }            
        }
    }


    void FixedUpdate()
    {
        Karakter_Hareket();
        Animasyon();

        Time.timeScale = 1;

        if (can <= 0)       //karakterimizin canı sıfır olduğundaki koşulumuz.
        {
            Time.timeScale = 0.4f;      //karakter öldüğünde oyun hızını yavaşlatıyoruz.

            can_text.enabled = false;       //karakter öldüğünce can ekranda gözükmeyecek.

            siyah_arka_plan_sayaci += 0.03f;        //karakter ölünce arka plan rengi yavaş yavaş siyah olcağı için değer atıyoruz.

            siyah_arka_plan.gameObject.SetActive(true);     //arka planımızı burada aktif hale getiriyoruz.

            siyah_arka_plan.color = new Color(0, 0, 0, siyah_arka_plan_sayaci);     //atadığımız değer arka planı bu kodla yavaş yavaş siyah yapıyor.

            ana_menuye_donme_zamani += Time.deltaTime;      //burada geçen zamanı değikenimize atıyoruz.

            if (ana_menuye_donme_zamani > 1)
            {
                SceneManager.LoadScene("Ana_Menu");
            }
        }
    }

    void LateUpdate()       //kamera hareketleri bu fonksiyon içine yazılır.
    {
        Kamera_Kontrol();
    }

    void Karakter_Hareket()
    {
        horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");        //sağ sol tuşlarına basıldığında yataydaki hareketini nesnemize atıyoruz.Ayrıca telefon ve PC de çalışıyor Cross sayesinde.

        vec = new Vector3(horizontal * 10, fizik.velocity.y, 0); //nesnemizin hareket özelliğini veriyoruz.

        fizik.velocity = vec;       //nesnemize hareketini atıyoruz.

    }

    void OnCollisionEnter2D(Collision2D collision)      //geçirgen olmayan yüzeye temas ettiğinde yani nesnemiz bir yere çarptığında çalışacak fonksiyon.
    {
        bir_kere_zipla = true;      //zıplayabilmek için tekrardan kontrolümüzü true yapıyoruz.
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "kursun_tag")       //karakterimize kursun çarptığındaki koşulumuz.
        {
            can--;      //canımız bir azalacak.

            can_text.text = "CAN  " + can;      //canımızı ekrana yazdıracak.
        }

        if (collision.gameObject.tag == "dusman_tag")       //karakterimize düşman çarptığındaki koşulumuz.
        {
            can -= 10;      //canımız bir azalacak.

            can_text.text = "CAN  " + can;      //canımızı ekrana yazdıracak.
        }

        if (collision.gameObject.tag == "testere_tag")       //karakterimize testere çarptığındaki koşulumuz.
        {
            can -= 10;      //canımız bir azalacak.

            can_text.text = "CAN  " + can;      //canımızı ekrana yazdıracak.
        }

        if (collision.gameObject.tag == "level_bitsin_tag")       //karakterimiz level bitsin nesnesine çarptığındaki koşulumuz.
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);      //bir sonraki levele geçme kodu.
        }

        if (can < 100)      //canımız 100 den az ise can alabileceğiz.
        {
            if (collision.gameObject.tag == "can_ver_tag")      //kutuya çarptığımızdaki koşulumuz.
            {
                can += 10;      //canımız 10 artıcak.

                if (can >= 100)     //canımız 100 ü geçerse koşulumuz.
                {
                    can = 100;      //canımızı 100 e eşitliyoruz.
                }

                can_text.text = "CAN " + can;       //canımızı ekrana yazdıracak.

                collision.GetComponent<BoxCollider2D>().enabled = false;        //bir kere can almak için ilk temaston sonra collider ı kapatıyoruz.

                collision.GetComponent<Can_Ver>().enabled = true;

                Destroy(collision.gameObject, 3);       //3 saniye sonra kutu yok olacak.
            }
        }
       

        if (collision.gameObject.tag == "altin_tag")       //karakterimiz level bitsin nesnesine çarptığındaki koşulumuz.
        {
            altin_sayac++;      //her temasta altın sayımız bir artıcak.

            altin_text.text = altin_sayac + " / 30";
            Destroy(collision.gameObject);      //altına temas halinde altın yok olucak.
        }

        if (collision.gameObject.tag == "su_tag")       //karakterimiz suya çarptığındaki koşulumuz.
        {
            can = 0;        //canımız sıfırlanıcak.s
        }
    }

    void Kamera_Kontrol()
    {
        kamera_son_pos = kamera_ilk_pos + transform.position;   //karakterimiz hareket ettiği için o anki konumunu alıp kamerayı aradaki uzaklık mesafesinde tutucak hep.

        kamera.transform.position = Vector3.Lerp(kamera.transform.position, kamera_son_pos, 0.1f);    //kameramız sürekli olarak karakterimizi belli bir mesafeden takip edicek.
                                                                                                      //Lerp fonksiyonu kamera hareketini yumuşatıyor.İlk parametrede kendi konumunu,
                                                                                                      //ikinci olarak gidilecek konumu, son olarakta yumuşaklık değerini veriyoruz.
    }

    void Animasyon()
    {
        if (bir_kere_zipla)     //eğer zıplama başlamamış ise olan koşulumuz.
        {
            if (horizontal == 0)        //karakterimiz X ekseninde hareket etmiyorsa koşulumuz.
            {
                bekleme_animasyon_zamani += Time.deltaTime;     //oyunda geçen zamanımızı değişkenimize atıyoruz.

                if (bekleme_animasyon_zamani > 0.05f)    //değişkenimiz 0.05 saniyeden büyükse koşulumuz.Yani bu zaman aralığında çalışmasını sağlıyoruz.
                {
                    sprite_renderer.sprite = bekleme_animasyon[bekleme_anim_sayac++];   //dizimizin elemanlarını tek tek geziyoruz.

                    if (bekleme_anim_sayac == bekleme_animasyon.Length)     //sayacımız dizimizin uzunluğuna eşitlendiğindeki koşulumuz.
                    {
                        bekleme_anim_sayac = 0;     //sayaca baştan başlamak için sıfırlıyoruz.
                    }

                    bekleme_animasyon_zamani = 0;       //tekrardan koşul sağlanması için sıfırlıyoruz değişkenimizi.
                }
            }

            else if (horizontal > 0)    //karakterimiz X ekseninde artı yönde hareket ediyorsa koşulumuz.
            {
                yurume_animasyon_zamani += Time.deltaTime;     //oyunda geçen zamanımızı değişkenimize atıyoruz.

                if (yurume_animasyon_zamani > 0.01f)    //değişkenimiz 0.01 saniyeden büyükse koşulumuz.Yani bu zaman aralığında çalışmasını sağlıyoruz.
                {
                    sprite_renderer.sprite = yurume_animasyon[yurume_anim_sayac++];   //dizimizin elemanlarını tek tek geziyoruz.

                    if (yurume_anim_sayac == yurume_animasyon.Length)     //sayacımız dizimizin uzunluğuna eşitlendiğindeki koşulumuz.
                    {
                        yurume_anim_sayac = 0;     //sayaca baştan başlamak için sıfırlıyoruz.
                    }

                    yurume_animasyon_zamani = 0;       //tekrardan koşul sağlanması için sıfırlıyoruz değişkenimizi.
                }
                transform.localScale = new Vector3(1, 1, 1);        //transform komponentindeki scale imize ulaştık ve değerlerimizi atadık.
            }

            else if (horizontal < 0)    //karakterimiz X ekseninde eksi yönde hareket ediyorsa koşulumuz.
            {
                yurume_animasyon_zamani += Time.deltaTime;     //oyunda geçen zamanımızı değişkenimize atıyoruz.

                if (yurume_animasyon_zamani > 0.01f)    //değişkenimiz 0.01 saniyeden büyükse koşulumuz.Yani bu zaman aralığında çalışmasını sağlıyoruz.
                {
                    sprite_renderer.sprite = yurume_animasyon[yurume_anim_sayac++];   //dizimizin elemanlarını tek tek geziyoruz.

                    if (yurume_anim_sayac == yurume_animasyon.Length)     //sayacımız dizimizin uzunluğuna eşitlendiğindeki koşulumuz.
                    {
                        yurume_anim_sayac = 0;     //sayaca baştan başlamak için sıfırlıyoruz.
                    }

                    yurume_animasyon_zamani = 0;       //tekrardan koşul sağlanması için sıfırlıyoruz değişkenimizi.
                }
                transform.localScale = new Vector3(-1, 1, 1);        //transform komponentindeki scale imize ulaştık ve değerlerimizi atadık. Ama bu sefer
                                                                     //karakterimiz geri gideceği için X eksenindeki yönünü değiştirdik.
            }
        }

        else
        {
            if (fizik.velocity.y > 0)   //Y ekseninde artı yönde hareket ediyorsa koşulumuz.
            {
                sprite_renderer.sprite = ziplama_animasyon[0];
            }

            else        //Y ekseninde eksi yönde hareket ediyorsa koşulumuz.
            {
                sprite_renderer.sprite = ziplama_animasyon[1];
            }

            if (horizontal > 0)     //X ekseninde artı yönde hareket yapıyorsa koşulumuz.
            {
                transform.localScale = new Vector3(1, 1, 1);    //nesnemizi artı yönüne çevirdik.
            }

            else if (horizontal < 0)    //X ekseninde eksi yönde hareket yapıyorsa koşulumuz
            {
                transform.localScale = new Vector3(-1, 1, 1);   //nesnemizi eksi yönüne çevirdik.
            }
        }        
    }
}
