using System.ComponentModel.DataAnnotations;
using System.IO.Enumeration;
using System.Runtime.CompilerServices;

int physicalSize = 31;
int logicalSize = 0;
double[] values = new double[physicalSize];
double max = 700.0;
double min = 0.0;
string[] dates = new string[physicalSize];
string[] csvFileInput = new string[physicalSize];
string[] mergedValue = new string[physicalSize];
double valueInput = 0;
string dateInput = "";
string fileName = "";
string newFileName = "";
string filePath = "";
string searchString = "";
int foundIndex = 0;
bool editFinishedBool = false;
double editedValue = 0;
bool loadCompleteforEdit = false;
bool goAgain = true;

while (goAgain)
{
	try
	{
		DisplayMainMenu();
		string mainMenuChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();
		if (string.IsNullOrWhiteSpace(mainMenuChoice))
		{
			throw new Exception($"No Null or White Space please. Please select the proper menu.");
		}
		else if (mainMenuChoice == "L")
			logicalSize = LoadFileValuesToMemory(dates, values);
		else if (mainMenuChoice == "S")
			SaveMemoryValuesToFile(dates, values, logicalSize);
		else if (mainMenuChoice == "D")
			DisplayMemoryValues(dates, values, logicalSize);
		else if (mainMenuChoice == "A")
			logicalSize = AddMemoryValues(dates, values, logicalSize, max, min);
		else if (mainMenuChoice == "E")
			EditMemoryValues(dates, values, logicalSize, min, max);
		else if (mainMenuChoice == "Q")
		{
			goAgain = false;
			throw new Exception("Bye, hope to see you again.");
		}
		else if (mainMenuChoice == "R")
		{
			while (true)
			{
				if (logicalSize == 0)
					throw new Exception("No entries loaded. Please load a file into memory");
				DisplayAnalysisMenu();
				string analysisMenuChoice = Prompt("\nEnter an Analysis Menu Choice: ").ToUpper();
				if (analysisMenuChoice == "A")
					FindAverageOfValuesInMemory(values, logicalSize);
				if (analysisMenuChoice == "H")
					FindHighestValueInMemory(values, logicalSize);
				if (analysisMenuChoice == "L")
					FindLowestValueInMemory(values, logicalSize);
				if (analysisMenuChoice == "G")
					GraphValuesInMemory(dates, values, logicalSize);
				if (analysisMenuChoice == "R")
					throw new Exception("Returning to Main Menu");
			}
		}
		else
		{
			throw new Exception($"Invalid Input. Please choose the proper menu.");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"{ex.Message}");
	}
}

void DisplayMainMenu()
{
	Console.WriteLine("\nMain Menu");
	Console.WriteLine("L) Load Values from File to Memory");
	Console.WriteLine("S) Save Values from Memory to File");
	Console.WriteLine("D) Display Values in Memory");
	Console.WriteLine("A) Add Value in Memory");
	Console.WriteLine("E) Edit Value in Memory");
	Console.WriteLine("R) Analysis Menu");
	Console.WriteLine("Q) Quit");
}

void DisplayAnalysisMenu()
{
	Console.WriteLine("\nAnalysis Menu");
	Console.WriteLine("A) Find Average of Values in Memory");
	Console.WriteLine("H) Find Highest Value in Memory");
	Console.WriteLine("L) Find Lowest Value in Memory");
	Console.WriteLine("G) Graph Values in Memory");
	Console.WriteLine("R) Return to Main Menu");
}

string Prompt(string prompt)
{
	string response = "";
	Console.Write(prompt);
	response = Console.ReadLine();
	return response;
}

string GetFileName()
{
	fileName = "";
	do
	{
		fileName = Prompt("Enter file name including .csv or .txt: ");
	} while (string.IsNullOrWhiteSpace(fileName));
	return fileName;
}

int LoadFileValuesToMemory(string[] dates, double[] values)
{
	string fileName = GetFileName();
	int logicalSize = 0;
	filePath = $"./data/{fileName}";
	if (!File.Exists(filePath))
		throw new Exception($"The file {fileName} does not exist.");
	string[] csvFileInput = File.ReadAllLines(filePath);
	for (int i = 0; i < csvFileInput.Length; i++)
	{
		Console.WriteLine($"lineIndex: {i}; line: {csvFileInput[i]}");
		string[] items = csvFileInput[i].Split(',');
		for (int j = 0; j < items.Length; j++)
		{
			Console.WriteLine($"itemIndex: {j}; item: {items[j]}");
		}
		if (i != 0)
		{
			dates[logicalSize] = items[0];
			values[logicalSize] = double.Parse(items[1]);
			logicalSize++;
		}
	}
	Console.WriteLine($"Load complete. {fileName} has {logicalSize} data entries");
	loadCompleteforEdit = true;
	return logicalSize;
}

void DisplayMemoryValues(string[] dates, double[] values, int logicalSize)
{
	if (logicalSize == 0)
		throw new Exception($"No Entries loaded. Please load a file to memory or add a value in memory");
	Array.Sort(dates, values, 0, logicalSize);
	Console.WriteLine($"\nCurrent Loaded Entries: {logicalSize}");
	Console.WriteLine($"   Date       Value");
	for (int i = 0; i < logicalSize; i++)
		Console.WriteLine($"{dates[i]}   {values[i]:c2}");
}

double FindHighestValueInMemory(double[] values, int logicalSize)
{
	double max = -1.0;

	for (int i = 0; i < logicalSize; i++)
	{
		if (values[i] > max)
			max = values[i];
	}

	Console.WriteLine($"Your maximum value for the loaded file is {max:c2}");

	return max;
}

double FindLowestValueInMemory(double[] values, int logicalSize)
{
	double min = 9999999.0;
	for (int i = 0; i < logicalSize; i++)
	{
		if (values[i] < min)
		{
			min = values[i];
		}
	}
	Console.WriteLine($"Your minimum value from the current memory is : {min:c2}");
	return min;
}

void FindAverageOfValuesInMemory(double[] values, int logicalSize)
{
	double sum = 0;
	for (int i = 0; i < logicalSize; i++)
	{
		sum += values[i];
	}
	double average = sum / logicalSize;
	Console.WriteLine($"The average sales value from the current memory is: {average:c2}");
}

void SaveMemoryValuesToFile(string[] dates, double[] values, int logicalSize)
{
	try
	{
		if (logicalSize == 0)
		{
			throw new Exception($"Nothing to save for now. Load or Add values first.");
		}
		if (logicalSize != 0)
		{
			bool saveFileAgain = true;
			while (saveFileAgain)
			{
				Console.Write($"Enter the file name for saving (including .csv or .txt): ");
				newFileName = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(newFileName))
				{
					Console.WriteLine($"Null value or white space is not allowed for file name. Try again.");
				}
				else if ((newFileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)) || (newFileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)))
				{
					saveFileAgain = false;
				}
				else
				{
					Console.WriteLine($"Please end your filename with .csv or .txt");
				}

			}
			string savePath = $"./data/{newFileName}";

			if (File.Exists(savePath) && editFinishedBool)
			{
				string[] loadedFile = File.ReadAllLines(savePath);
				values[foundIndex] = editedValue;
				File.WriteAllLines(savePath, loadedFile);


			}

			if (File.Exists(savePath) && (editFinishedBool != true))
			{
				List<string> allLines = new List<string>(File.ReadAllLines(savePath));
				List<string> dateList = new List<string>(); // 날짜를 담을 새로운 리스트

				foreach (var line in allLines)
				{
					string[] lineParts = line.Split(',');
					string date = lineParts[0];
					dateList.Add(date);
				}

				bool addingChecker = false;
				bool saveFailChecker = false;

				for (int i = 0; i < logicalSize; i++)
				{
					string newDataDate = dates[i];
					string newData = $"{dates[i]},{values[i]}";

					if (dateList.Contains(newDataDate))
					{
						saveFailChecker = true;
					}
					if (!dateList.Contains(newDataDate))
					{
						allLines.Add(newData);
						addingChecker = true;
					}
				}

				File.WriteAllLines(savePath, allLines);
				if (addingChecker && !saveFailChecker)
				{
					Console.WriteLine($"\nNewly added Entries: {logicalSize}");
					Console.WriteLine($"   Date       Value");
					for (int i = 0; i < logicalSize; i++)
						Console.WriteLine($"{dates[i]}   {values[i]:c2}");
					Console.WriteLine($"File saved successfully at {savePath}");
				}
				if (addingChecker && saveFailChecker)
				{
					string[] savedFile = File.ReadAllLines(savePath);

					for (int i = 0; i < savedFile.Length; i++)
					{
						Console.WriteLine(savedFile[i]);
					}
					Console.WriteLine($"\nData has been saved, except for the values that have the existing dates(if any).\nFor values assigned to existing dates, you can change in the 'Edit' section.");
				}
				if (saveFailChecker && !addingChecker)
				{
					Console.WriteLine($"Data has not been saved due to existing data for the date. Edit first.");
				}
			}
			else
			{
				List<string> allLines = new List<string>();
				allLines.Add("Dates,Values");

				for (int i = 0; i < logicalSize; i++)
				{
					string newData = $"{dates[i]},{values[i]}";

					if (!allLines.Contains(newData))
					{
						allLines.Add(newData);
					}
				}
				File.WriteAllLines(savePath, allLines);
				Console.WriteLine($"\nCurrent Loaded Entries: {logicalSize}");
				Console.WriteLine($"   Date       Value");
				for (int i = 0; i < logicalSize; i++)
					Console.WriteLine($"{dates[i]}   {values[i]:c2}");
				Console.WriteLine($"File saved successfully at {savePath}");
			}
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"{ex.Message}");
	}
}

int AddMemoryValues(string[] dates, double[] values, int logicalSize, double max, double min)
{
	try
	{
		bool addMemorybool = true;
		while (addMemorybool)
		{
			Console.Write($"Make an entry value (Y/N): ");
			string addMemoryYesOrNo = Console.ReadLine().ToUpper();
			if (addMemoryYesOrNo == "Y")
			{
				bool invalidDate = true;
				bool invalidValue = true;
				while (invalidDate)
				{
					Console.Write($"Enter the date for adding values(e.g. MM-dd-yyyy): ");
					dateInput = Console.ReadLine();

					DateTime date;
					if (DateTime.TryParseExact(dateInput, "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out date))
					{
						invalidDate = false;
					}
					else
					{
						Console.WriteLine($"Invalid input for the date. Please follow the EXACT format.\ne.g.)11-3-2023(X)\n     11-03-2023(O)");
					}
					if (logicalSize != 0)
					{
						for (int i = 0; i < logicalSize; i++)
						{
							if (dateInput == dates[i])
							{
								throw new Exception($"There's already an existing data for the same date.\nGo to Edit value menu if you want to change the value for the date.");
							}
						}
					}
				}
				while (invalidValue)
				{
					try
					{
						Console.Write($"Enter the sales value as a double between {min:c2} and {max:c2}: ");
						valueInput = double.Parse(Console.ReadLine());

						if (valueInput >= 0 && valueInput <= 700)
						{
							invalidValue = false;
						}
						else
						{
							Console.WriteLine($"You should enter a value between {min:c2} and {max:c2}");
						}
					}
					catch (Exception)
					{
						Console.WriteLine($"Invalid Input. It should be a number.");
					}
				}
				Console.WriteLine($"\nYou entered {dateInput},{valueInput:c2}");
				if (logicalSize == 0)
				{
					mergedValue[0] = "Dates,Values";
					dates[logicalSize] = dateInput;
					values[logicalSize] = valueInput;
					mergedValue[logicalSize + 1] = $"{dateInput},{valueInput}";
				}
				if (logicalSize != 0)
				{
					Console.WriteLine("You are adding data now for reminder.");
					dates[logicalSize] = dateInput;
					values[logicalSize] = valueInput;
					mergedValue[logicalSize + 1] = $"{dateInput},{valueInput}";
				}

				logicalSize++;
				Console.WriteLine($"\nData has been added to the MEMORY.\nYour logicalSize is {logicalSize}. But it hasn't been saved onto the file yet.");

			}
			else if (addMemoryYesOrNo == "N")
			{
				addMemorybool = false;
			}

			else
			{
				Console.WriteLine($"Invalid Input. Please choose between Y and N");
			}
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
	}
	return logicalSize;
}

void EditMemoryValues(string[] dates, double[] values, int logicalSize, double min, double max)
{
	bool invalidEditDate = true;
	bool invalidEditValue = true;
	if (logicalSize == 0)
	{
		Console.WriteLine($"No entry has been loaded. You should load a file first.");
	}
	if (logicalSize != 0 && !loadCompleteforEdit)
	{
		Console.WriteLine($"Please load first for editing.");
	}
	if (logicalSize != 0 && loadCompleteforEdit)
	{
		while (invalidEditDate)
		{
			Console.WriteLine($"\nIf you want to cancel edit, enter Q");
			Console.Write($"Enter the date of which you want to change the value (e.g MM-dd-yyyy): ");
			searchString = Console.ReadLine();
			if (searchString == "Q" || searchString == "q")
			{
				throw new Exception($"Editing canceled.");
			}
			DateTime date;
			if (DateTime.TryParseExact(searchString, "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out date))
			{
				bool foundbool = false;
				for (int i = 0; i < logicalSize; i++)
				{
					if (dates[i] == searchString)
					{
						foundbool = true;
						foundIndex = i;
						invalidEditDate = false;
						break;
					}
				}
				if (!foundbool)
				{
					Console.WriteLine($"Couldn't find the matching date from the file.");
				}
			}
			else
			{
				Console.WriteLine($"Invalid input for the date. Please follow the EXACT format.\ne.g.)11-3-2023(X)\n     11-03-2023(O)");
			}
		}
		while (invalidEditValue)
		{
			try
			{
				Console.Write($"Enter the value that you want to enter for the date: ");
				editedValue = double.Parse(Console.ReadLine());
				if (editedValue < min || editedValue > max)
				{
					throw new Exception($"please enter value between {min:c2} and {max:c2}");
				}
				invalidEditValue = false;
				editFinishedBool = true;
				values[foundIndex] = editedValue;
				Console.WriteLine($"\nCurrent Entries: {logicalSize}");
				Console.WriteLine($"   Date       Value");
				for (int i = 0; i < logicalSize; i++)
					Console.WriteLine($"{dates[i]}   {values[i]:c2}");
				Console.WriteLine($"\nIt's edited on Memory level. You need to save it into file.\nOtherwise it's gone.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}

void GraphValuesInMemory(string[] dates, double[] values, int logicalSize)
{
	//still working on it..but it's hard. wanted to get the date and give a value to a certain position, but don't know how to yet..
	string dateSpacing = "  ";
	string dayPart = "";
	string anotherDayPart = "";
	bool alreadyAddedBool = false;
	int previousLength=0;
	Array.Sort(dates, values, 0, logicalSize);
	for (int i = 700; i > -50; i = i - 50)
	{
		Console.Write($"{i:c0}");
		for (int j = 0; j < logicalSize; j++)
		{
			if ((values[j] <= i) && (values[j] > i - 50) && alreadyAddedBool)
			{
				anotherDayPart = dates[j].Substring(3, 2);
				Console.Write(values[j].ToString().PadLeft(values[j].ToString().Length - previousLength + (int.Parse(anotherDayPart) - int.Parse(dayPart)) * 3));
				dayPart = dates[j].Substring(3,2);
			}
			if ((values[j] <= i) && (values[j] > i - 50) && (!alreadyAddedBool))
			{
				dayPart = dates[j].Substring(3, 2);
				previousLength = values[j].ToString().Length;
				Console.Write(values[j].ToString().PadLeft(values[j].ToString().Length + int.Parse(dayPart) * 2 + (int.Parse(dayPart) - 2)));
				alreadyAddedBool = true;

			}
		}
		alreadyAddedBool=false;
		Console.WriteLine($"");
	}

	for (int i = 1; i < 32; i++)
	{
		if (i == 1)
		{
			Console.Write($"     {i}{dateSpacing}");
		}
		if (i != 1)
		{
			Console.Write($"{i}{dateSpacing}");
		}
	}
}