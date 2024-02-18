using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageTesting : MonoBehaviour
{
    // Start is called before the first frame update

    public Image image;



    public string imageUrl = "https://oaidalleapiprodscus.blob.core.windows.net/private/org-s9zdmVytCjT6odlwwVLB7Kxj/user-XbQTX9of2tjmdTtLIqMr2SHg/img-rCbfKJ5sNHSksvPmCtcIUnml.png?st=2024-02-18T02%3A43%3A06Z&se=2024-02-18T04%3A43%3A06Z&sp=r&sv=2021-08-06&sr=b&rscd=inline&rsct=image/png&skoid=6aaadede-4fb3-4698-a8f6-684d7786b067&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2024-02-17T20%3A08%3A15Z&ske=2024-02-18T20%3A08%3A15Z&sks=b&skv=2021-08-06&sig=V92HDFhYKwESWlTAWX/wN65btWYdRq7EtGFkHNTNxWQ%3D";
    // void Start()
    // {
        
    // }


public ImageTesting(){

}

public void Test(){
    StartCoroutine(LoadImage(imageUrl));
}
        IEnumerator LoadImage(string link){
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
            yield return request.SendWebRequest();

            if(request.isNetworkError || request.isHttpError){
                Debug.Log(request.error);
            }

            else{
                Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite newSprite = Sprite.Create(myTexture, new Rect(0,0,myTexture.width,myTexture.height), new Vector2(0.5f,0.5f));
                image.sprite = newSprite;
            }
        }
}
