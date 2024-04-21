using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ClientNS;
Client myClient = new Client();
List<Client> listOfClients = [];

LoadFileValuesToMemory(listOfClients);


bool goAgain = true;

while (goAgain)
{
    try
    {
        DisplayMainMenu();
        string mainMenuChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();
        if (mainMenuChoice == "N")
        {
            myClient = NewClient();
            listOfClients.Add(myClient);
        }
        else if (mainMenuChoice == "S")
        {
            ShowClient(myClient);
        }
        else if (mainMenuChoice == "Q")
        {
            SaveMemoryValuesToFile(listOfClients);
            Console.WriteLine($"Bye. See you again");
            goAgain = false;
        }
        else if (mainMenuChoice == "E")
        {
            bool editGoAgain = true;

            while (editGoAgain)
            {
                DisplayEditMenu();
                string editChoice = Prompt("\nChoose what you want to edit: ").ToUpper();
                if (editChoice == "F")
                {
                    GetFirstName(myClient);
                }

                else if (editChoice == "L")
                {
                    GetLastName(myClient);
                }

                else if (editChoice == "W")
                {
                    GetWeight(myClient);
                }

                else if (editChoice == "H")
                {
                    GetHeight(myClient);
                }

                else if (editChoice == "R")
                {
                    editGoAgain = false;
                    throw new Exception($"Going back to main menu.");
                }

                else
                {
                    Console.WriteLine($"Invalid Choice. Try again.");
                }
            }
        }
        else if (mainMenuChoice == "L")
        {
            DisplayListsOfClients(listOfClients);

        }
        else if (mainMenuChoice == "F")
        {
            Console.WriteLine(listOfClients[2]);

        }
        else if (mainMenuChoice == "R")
        {
            Console.WriteLine($"Not implemented yet.");

        }

        else
        {
            throw new Exception($"Invalid Input. Choose the right menu.");
        }
    }

    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}

void DisplayMainMenu()
{
    Console.WriteLine("\n---BMI Calculator---");
    Console.WriteLine("\nMain Menu");
    Console.WriteLine("N) New Client [PartA]");
    Console.WriteLine("S) Show Client BMI Info [PartA]");
    Console.WriteLine("E) Edit Client [PartA]");
    Console.WriteLine("L) List all Clients [PartB]");
    Console.WriteLine("F) Find Client [PartB]");
    Console.WriteLine("R) Remove Client [PartB]");
    Console.WriteLine("Q) Quit & Auto-Save [PartA]");
}

void DisplayEditMenu()
{
    Console.WriteLine("\n---Edit Menu List---\n");
    Console.WriteLine("F) First Name Edit");
    Console.WriteLine("L) Last Name Edit");
    Console.WriteLine("W) Weight Edit");
    Console.WriteLine("H) Height Edit");
    Console.WriteLine("R) Return to Main Menu");
}

void DisplayListsOfClients(List<Client> listOfClients)
{
   
    foreach (Client item in listOfClients)
    {
        ShowClientLists(item);
    }
}

string Prompt(string prompt)
{
    string myString = "";
    while (true)
    {
        try
        {
            Console.Write(prompt);
            myString = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(myString))
                throw new Exception($"Empty Input: Please enter something.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    return myString;
}

Client NewClient()
{
    Client myClient = new("daejin", "kim", 1, 1);
    GetFirstName(myClient);
    GetLastName(myClient);
    GetWeight(myClient);
    GetHeight(myClient);

    return myClient;
}



void ShowClient(Client client)
{
    if (client == null)
    {
        throw new Exception("No Client in Memory");
    }

    // Console.WriteLine($"\n{client.ToString()}");
    Console.WriteLine($"Client's Name : {client.FullName}");
    Console.WriteLine($"BMI score : {client.BmiScore:n2}");
    Console.WriteLine($"BMI status: {client.BmiStatus}");
}

void ShowClientLists(Client client)
{
    // Console.WriteLine($"\n{client.ToString()}");
    Console.WriteLine($"Client's Name : {client.FullName}, {client.BmiScore:n2}, {client.BmiStatus}");
}


void LoadFileValuesToMemory(List<Client> listOfClients)
{
	while(true)
    {
		try
		{
			//string fileName = Prompt("Enter file name including .csv or .txt: ");
			string fileName = "regout.csv";
			string filePath = $"./data/{fileName}";
			if (!File.Exists(filePath))
				throw new Exception($"The file {fileName} does not exist.");
			string[] csvFileInput = File.ReadAllLines(filePath);
			for(int i = 0; i < csvFileInput.Length; i++)
			{
				//Console.WriteLine($"lineIndex: {i}; line: {csvFileInput[i]}");
				string[] items = csvFileInput[i].Split(',');
				for(int j = 0; j < items.Length; j++)
				{
					//Console.WriteLine($"itemIndex: {j}; item: {items[j]}");
				}
				Client myClient = new(items[0], items[1], int.Parse(items[2]), int.Parse(items[3]));
				listOfClients.Add(myClient);
			}
			Console.WriteLine($"Load complete. {fileName} has {listOfClients.Count} data entries");
			break;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"{ex.Message}");
		}
	}
}

void SaveMemoryValuesToFile(List<Client> listOfClients)
{
    string fileName = "regout.csv";
    string directory = "./data";
    string filePath = Path.Combine(directory, fileName);

    if (!Directory.Exists(directory))
    {
        Directory.CreateDirectory(directory);
    }

    HashSet<string> existingEntries = new HashSet<string>(); // Set to keep track of existing entries

    // Check existing entries in the file
    if (File.Exists(filePath))
    {
        string[] existingLines = File.ReadAllLines(filePath);
        foreach (string line in existingLines)
        {
            existingEntries.Add(line); // Store existing entries in the HashSet
        }
    }

    List<string> newEntries = new List<string>();

    foreach (Client client in listOfClients)
    {
        string clientData = client.ToString();

        // Check if clientData (formatted as string) already exists in the HashSet (existingEntries)
        if (!existingEntries.Contains(clientData))
        {
            newEntries.Add(clientData); // Add only if it's not a duplicate
            existingEntries.Add(clientData); // Add to HashSet to avoid future duplicates
        }
    }

    if (newEntries.Count > 0)
    {
        File.AppendAllLines(filePath, newEntries); // Append new entries to the file
        Console.WriteLine($"Auto Save complete. {fileName} has {newEntries.Count} new entries.");
    }
    else
    {
        Console.WriteLine($"No new entries to save. {fileName} remains unchanged.");
    }
}


// void SaveMemoryValuesToFile(List<Client> listOfClients)
// {
//     //string fileName = Prompt("Enter file name including .csv or .txt: ");
//     string fileName = "regout.csv";
//     string directory = $"./data";
//     string filePath = $"./data/{fileName}";
//     if (!Directory.Exists(directory))
//     {
//         Directory.CreateDirectory(directory);
//     }

//     string[] csvLines = new string[listOfClients.Count];
//     for (int i = 0; i < listOfClients.Count; i++)
//     {
//         csvLines[i] = listOfClients[i].ToString();
//     }

//     if (File.Exists(filePath))
//     {
//         File.AppendAllLines(filePath, csvLines);
//         List<string> linesFromFile = File.ReadAllLines(filePath).ToList();
//         Console.WriteLine($"Auto Save complete. {fileName} has {linesFromFile.Count} entries.");
//     }

//     if (!File.Exists(filePath))
//     {
//         File.WriteAllLines(filePath, csvLines);
//         Console.WriteLine($"Auto Save complete. {fileName} has {listOfClients.Count} entries.");
//     }
// }



void GetFirstName(Client client)
{
    string myFirstName = Prompt("Enter the first name of the client: ");
    client.FirstName = myFirstName;
}

void GetLastName(Client client)
{
    string myLastName = Prompt("Enter the last name of the client: ");
    client.LastName = myLastName;
}


void GetWeight(Client client)
{
    int myWeight = PromptIntBetweenMinMax("Enter the weight of the client in POUNDS", 0, 440);
    client.Weight = myWeight;
}


void GetHeight(Client client)
{
    int myHeight = PromptIntBetweenMinMax("Enter the height of the client in INCHES", 0, 120);
    client.Height = myHeight;
}

int PromptIntBetweenMinMax(String msg, int min, int max)
{
    int num = 0;
    while (true)
    {
        try
        {
            Console.Write($"{msg} (greater than {min} and less than or equal to {max}): ");
            num = int.Parse(Console.ReadLine());
            if (num <= min || num > max)
                throw new Exception($"Must be greater than {min} and less than or equal to {max}");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid: {ex.Message}");
        }
    }
    return num;
}