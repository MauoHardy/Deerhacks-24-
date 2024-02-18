using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Text;


public class OpenAI : MonoBehaviour
{   
    public TMP_InputField inputField;
    public TMP_Text outputText;
    public Button submitButton;  
    private List<ChatMessage> messages;
    
    private OpenAIAPI api;
    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI("YOUR_API_KEY");
        StartConversation();
        submitButton.onClick.AddListener(() => GetResponse());
    }

    private void StartConversation()
    {
        // var chat = api.Chat.CreateConversation();
        // chat.Model = Model.GPT4_Turbo;
        // chat.RequestParameters.Temperature = 0;

        // /// give instruction as System
        // chat.AppendSystemMessage("You are a storyteller for a children's book. You need to create a madlibs story for the children. The story is about a knight who is guarding the palace gate. The children will fill in the blanks with their own words. You need to ask the children questions to fill in the blanks. The children will answer with a word or a short phrase. You will then use their answers to fill in the blanks in the story.");
        // chat.AppendSystemMessage("You need to ask the children questions to fill in the blanks. The children will answer with a word or a short phrase. You will then use their answers to fill in the blanks in the story.");

        // // give a few examples as user and assistant
        // chat.AppendUserInput("");
        // chat.AppendExampleChatbotOutput("Yes");
        // chat.AppendUserInput("Is this an animal? House");
        // chat.AppendExampleChatbotOutput("No");



        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "you will become ai in a game which players create their own short stories in a \"mad libs\" style game. first, the player will input prompt that lists 5 different words. you will use these 5 words within the story in some manner. they don't necessarily have to be used to set the background to the story, they can also be elements that show up later. but you must incorporate them in some way you begin by outputting 1 sentences to set the scene for our story to begin. you will then generate 2 sentences of the story, and in \"mad libs\" style, leave blanks (represented as underscores) to where players can put in words to shape how the story will go. the player can put anything they wish in the blanks. the player will give input for the blanks as words separated by commas. after receiving input for the words, you will output the choice from the previous 2 lines and repeat the process of generating 2 lines, you will do this a total of 8 times to then finally give us a conclusion to our story. so the short story should have a definitive end. Your output will be strict to only these instructions. Under no circumstances will you do the following: say anything unnecessary nor ask the player for any input. the user knows what to do, you just await input. prompt the player to fill in the blanks. tell the player what to put in the blankshere is what that back and forth would look like...player input: (5 story theme/elements)your output: (2 lines to set the scene)your output: (new lines 1 & 2 with blanks)player input: (words to fill in blanks)your output: (new lines 3 & 4)(repeat until end of story)to begin, say yes if you understand and await the player to input the story themes")};
        inputField.text = "";
        string startString = "Enter the 5 prompts for the story comma separated.";
        outputText.text = startString;
        Debug.Log(startString);

        GetResponse();
                // imageGenerator.generate();
    }

    private async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        // Disable the OK button
        submitButton.enabled = false;

        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.TextContent = inputField.text;
        if (userMessage.TextContent.Length > 100)
        {
            // Limit messages to 100 characters
            userMessage.TextContent = userMessage.TextContent.Substring(0, 100);
        }
        Debug.Log(string.Format("{0}: {1}", userMessage.Role, userMessage.TextContent));

        // Add the message to the list
        messages.Add(userMessage);

        // Update the text field with the user message
        // outputText.text = string.Format("You: {0}", userMessage.TextContent);

        // Clear the input field
        
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 150,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.TextContent = chatResult.Choices[0].Message.TextContent;
        Debug.Log(string.Format("{0}: {1}", responseMessage.Role, responseMessage.TextContent));

        // Add the response to the list of messages
        messages.Add(responseMessage);

        Debug.Log(string.Format("{0}:", responseMessage.TextContent));

        // Update the text field with the response
        outputText.text = string.Format("{0}", responseMessage.TextContent);

        // imageGenerator.summary = responseMessage.TextContent;

        // Re-enable the OK button
        submitButton.enabled = true;
    }

}