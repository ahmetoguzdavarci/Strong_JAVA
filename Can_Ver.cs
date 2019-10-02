using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can_Ver : MonoBehaviour        //can kutusunun içindeyiz.
{
    public Sprite[] animasyon_kareleri;       //can kutusuna açılma görüntüsü vermek için oluşturduğumuz dizi.

    SpriteRenderer sprite_renderer;     //komponenti atamak için oluşturduğumuz değişken.

    float zaman = 0;        //animasyonun çalışma zamanını ayarlamak için kullanacağımız değişken.

    int animasyon_kareleri_sayacı = 0;     //dizimizin elemanlarını gezmek için oluşturulan değişken.

    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();       //komponentimizi nesnemize atıyoruz.
    }

    void Update()
    {
        zaman += Time.deltaTime;        //koşul kodumuzun çalışma zamanını ayarlıyoruz.

        if (zaman > 0.1f)       //0.1 saniyede bir çalışması için verdiğimiz koşul.
        {
            sprite_renderer.sprite = animasyon_kareleri[animasyon_kareleri_sayacı++];       //dizimizin tüm elemanlarını geziyoruz ve hareket ediyomuş gibi görüntü elde ediyoruz.

            if (animasyon_kareleri.Length == animasyon_kareleri_sayacı)     //dizi uzunluğunu aşmamak için kontrol ediyoruz.
            {
                animasyon_kareleri_sayacı = animasyon_kareleri.Length - 1;      //buradaki eksiltmenin sebebide hep son kare döndürülsün diye. Zaten 3 saniye sonra dizi yok edilecek.
            }

            zaman = 0;      //update in hızından bağımsız çalışması için sürekli sıfırlıyoruz zamanı.
        }
    }
}
