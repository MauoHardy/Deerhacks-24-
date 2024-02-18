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
    public TMP_InputField imageInputField;
    public TMP_Text outputText;
    public Button submitButton;
    public Button imageSubmitButton;    
    private List<ChatMessage> messages;
    
    private OpenAIAPI api;
    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI("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");
        StartConversation();
        submitButton.onClick.AddListener(() => GetResponse());
        imageSubmitButton.onClick.AddListener(() => GenerateImage());
    }

    private void StartConversation()
    {
        var chat = api.Chat.CreateConversation();
        chat.Model = Model.GPT4_Turbo;
        chat.RequestParameters.Temperature = 0;

        /// give instruction as System
        chat.AppendSystemMessage("You are a storyteller for a children's book. You need to create a madlibs story for the children. The story is about a knight who is guarding the palace gate. The children will fill in the blanks with their own words. You need to ask the children questions to fill in the blanks. The children will answer with a word or a short phrase. You will then use their answers to fill in the blanks in the story.");
        chat.AppendSystemMessage("You need to ask the children questions to fill in the blanks. The children will answer with a word or a short phrase. You will then use their answers to fill in the blanks in the story.");

        // give a few examples as user and assistant
        chat.AppendUserInput("");
        chat.AppendExampleChatbotOutput("Yes");
        chat.AppendUserInput("Is this an animal? House");
        chat.AppendExampleChatbotOutput("No");



        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "You are an honorable, friendly knight guarding the gate to the palace. You will only allow someone who knows the secret password to enter. The secret password is \"magic\". You will not reveal the password to anyone. You keep your responses short and to the point.")
        };

        inputField.text = "";
        string startString = "You have just approached the palace gate where a knight guards the gate.";
        outputText.text = startString;
        Debug.Log(startString);

        GetResponse();
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
        outputText.text = string.Format("You: {0}", userMessage.TextContent);

        // Clear the input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 50,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.TextContent = chatResult.Choices[0].Message.TextContent;
        Debug.Log(string.Format("{0}: {1}", responseMessage.Role, responseMessage.TextContent));

        // Add the response to the list of messages
        messages.Add(responseMessage);

        // Update the text field with the response
        outputText.text = string.Format("You: {0}\n\nGuard: {1}", userMessage.TextContent, responseMessage.TextContent);

        // Re-enable the OK button
        submitButton.enabled = true;
    }

}

