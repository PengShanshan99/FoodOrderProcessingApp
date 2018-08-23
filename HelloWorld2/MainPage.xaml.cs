using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloWorld2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Click_Record(object sender, RoutedEventArgs e)
        {
            // Create an instance of file storage dealer
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            // Create a new file named as "chickenorfish.txt", replace if already exists.
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("chickenOrFish.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //await Windows.Storage.FileIO.WriteTextAsync(sampleFile, "Hello sir what would you like for breakfast chicken please good morning sir what would you like for lunch I want fish noodles thanks");
            // Create an instance of SpeechRecognizer.
            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            // Compile the dictation grammar by default.
            await speechRecognizer.CompileConstraintsAsync();

            // Start recognition.
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();

            // store recognized text as string script0.
            string script0 = speechRecognitionResult.Text;
            // write script0 to the text file "chickenOrFish.txt".
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, script0);
            var messageDialog = new Windows.UI.Popups.MessageDialog("Heard you say: ", speechRecognitionResult.Text);
          
        }

        private async void Click_Script(object sender, RoutedEventArgs e)
        {
            //Create an instance of windows file dealer class
            Windows.Storage.StorageFolder storageFolder2 = Windows.Storage.ApplicationData.Current.LocalFolder;
            // Read from file called "chickenOrFish.txt"
            Windows.Storage.StorageFile sampleFile2 = await storageFolder2.GetFileAsync("chickenOrFish.txt");
            // Store text in a string variable named "text".
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile2);
            // Display the text in the TextBlock named "Output".
            Output.Text = text;
            //MediaElement mediaElement = new MediaElement();
            //var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            //Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);
            //mediaElement.SetSource(stream, stream.ContentType);
            //mediaElement.Play();
        }

        private async void Click_SetupMenu(object sender, RoutedEventArgs e)
        {
            // Creat a new file called "menu.txt"
            Windows.Storage.StorageFolder storageFolderMenu = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleMenu = await storageFolderMenu.CreateFileAsync("menu.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            // Write "Chicken Porridge\nFish Noodle\nVegetarian" into "menu.txt".
            await Windows.Storage.FileIO.WriteTextAsync(sampleMenu, "Chicken Porridge\nFish Noodle\nVegetarian");
            // Read from file "menu.txt" and store it as a string variabel text.
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleMenu);
            // Display the text.
            Menu.Text = text;
        }

        private async void Click_Result(object sender, RoutedEventArgs e)
        {
            Output.Text = "Analyzing...";
            // Read text from "chickenOrFish.txt" for the recorded script, read text from "menu.txt" for the menu.
            Windows.Storage.StorageFolder storageFolder3 = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile3 = await storageFolder3.GetFileAsync("chickenOrFish.txt");
            Windows.Storage.StorageFile sampleMenu = await storageFolder3.GetFileAsync("menu.txt");
            // Store the texts in string variables named "script" and "menu".
            string script = await Windows.Storage.FileIO.ReadTextAsync(sampleFile3);
            string menu = await Windows.Storage.FileIO.ReadTextAsync(sampleMenu);

            // the algorithm to analyze the orders, converted from Xuekai's Java code to c#.
            string[] menuList = menu.Split('\n');
            string[] meal1 = menuList[0].Split(' ');
            string[] meal2 = menuList[1].Split(' ');
            string[] meal3 = menuList[2].Split(' ');
            string[] tokens = script.Split(' ');
            List<string> orders = new List<string>();
            string currentOrder = "";
            for (int i = 0; i < tokens.Count(); i++)
            {
                //Output.Text = i.ToString();
                //the java code if (tokens[i].equalsIgnoreCase(meal1[0]) || tokens[i].equalsIgnoreCase(meal1[1]) || tokens[i].equalsIgnoreCase("first"))
                if ((String.Compare(tokens[i], meal1[0], true) == 0) || (String.Compare(tokens[i], meal1[1], true) == 0) || (String.Compare(tokens[i], "first", true) == 0))
                {
                    currentOrder = meal1[0];
                }
                //the java code else if (tokens[i].equalsIgnoreCase(meal2[0]) || tokens[i].equalsIgnoreCase(meal2[1]) || tokens[i].equalsIgnoreCase("second"))
                else if ((String.Compare(tokens[i], meal2[0], true) == 0) || (String.Compare(tokens[i], meal2[1], true) == 0) || (String.Compare(tokens[i], "second", true) == 0))
                {
                    currentOrder = meal2[0];
                }
                //the java code else if (tokens[i].equalsIgnoreCase(meal3[0]) || tokens[i].equalsIgnoreCase(meal3[1]) || tokens[i].equalsIgnoreCase("third"))
                else if ((String.Compare(tokens[i], meal3[0], true) == 0) || (String.Compare(tokens[i], "third", true) == 0))
                {
                    currentOrder = meal3[0];
                }
                //cue words & phrases: what about you, what would you like, what do you want, thanks, thank you, please
                if (((String.Compare(tokens[i],"about",true)==0) && (String.Compare(tokens[i+1],"you",true)==0)) ||
                        ((String.Compare(tokens[i], "would", true) == 0) && (String.Compare(tokens[i + 1], "you", true) == 0)) ||
                        ((String.Compare(tokens[i], "do", true) == 0) && (String.Compare(tokens[i + 1], "you", true) == 0)) ||
                        (String.Compare(tokens[i], "thanks", true) == 0) || (String.Compare(tokens[i], "thank", true) == 0) || (String.Compare(tokens[i], "please", true) == 0))
                {
                    if ((String.Compare(currentOrder, meal1[0], true) == 0) || (String.Compare(currentOrder, meal2[0], true) == 0) || (String.Compare(currentOrder, meal3[0], true) == 0))
                        orders.Add(currentOrder);
                        currentOrder = "";
                }
                if (i == tokens.Count() - 1)
                    orders.Add(currentOrder);
            }

            //remove any empty order
            for (int i = 0; i < orders.Count(); i++)
            {
                if (String.Compare(orders[i], "", true) == 0)
                    orders.Remove("");
            }
            string output = "The orders are:";
            for (int i = 0; i < orders.Count(); i++)
            {
                output += " \n";
                output += orders[i];
            }
            Output.Text = output;
        }
    }
}
