using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

   public class ImageGenerator : MonoBehaviour
    {
    public TMP_InputField inputField;
    public Button submitButton;  

    public string summary;

    public Image image;

    [SerializeField] private ImageTesting imageTesting;

    void Awake(){
        submitButton.onClick.AddListener(()=>{generator(imageTesting, summary+"The blank is filled by"+inputField.text);});
    }



// public void generate(){
//     generator(imageTesting,summary+"The blank is filled by"+inputField.text);
//     inputField.text="";
// }

        static async Task generator(ImageTesting imageTesting, string prompt )
        {
            // Your OpenAI API key
            string apiKey = "YOUR_API_KEY";

            // API URL for image generation
            string apiUrl = "https://api.openai.com/v1/images/generations";

            // Create an HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Set up the authorization header
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                // JSON payload for the request
                string jsonPayload = @"{
                    ""model"": ""dall-e-3"",
                    ""prompt"": """ + prompt + @""",
                    ""n"": 1,
                    ""size"": ""1024x1024""
                }";

                // Prepare the content for the request with the correct Content-Type
                using (StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"))
                {
                    try
                    {
 // Send the POST request
                        HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                        // Ensure we received a successful response
                        response.EnsureSuccessStatusCode();

                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Parse the JSON response
                        JObject jsonResponse = JObject.Parse(responseBody);

                        // Extract the first URL
                       string imageUrl = (string)jsonResponse["data"][0]["url"];

                        // Output the URL to the console
                        Console.WriteLine(imageUrl);
                        Debug.Log(imageUrl);

                        // imageGenerator.LoadImage(imageUrl);
                        imageTesting.imageUrl = imageUrl;
                        imageTesting.Test();
                        
                        
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message: {0} ", e.Message);
                    }

                }
            }
        }

        // IEnumerator LoadImage(string link){
        //     UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
        //     yield return request.SendWebRequest();

        //     if(request.isNetworkError || request.isHttpError){
        //         Debug.Log(request.error);
        //     }

        //     else{
        //         Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        //         Sprite newSprite = Sprite.Create(myTexture, new Rect(0,0,myTexture.width,myTexture.height), new Vector2(0.5f,0.5f));
        //         image.sprite = newSprite;
        //     }
        // }
    }

