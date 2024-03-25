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
	Array.Sort(dates,values,0,logicalSize);
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
	Console.WriteLine($"your minimum value for this file is : {min:c2}");
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
	Console.WriteLine($"The average sales value from this file is: {average:c2}");
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
				// 기존 파일이 있는 경우, 기존 내용을 읽어들여 리스트에 저장
				List<string> allLines = new List<string>(File.ReadAllLines(savePath));
				List<string> dateList = new List<string>(); // 날짜를 담을 새로운 리스트

				// allLines 리스트를 반복하여 각 요소에서 날짜를 추출하여 dateList에 추가
				foreach (var line in allLines)
				{
					string[] lineParts = line.Split(','); // 각 줄을 쉼표로 분리하여 배열로 저장
					string date = lineParts[0]; // 첫 번째 요소는 날짜
					dateList.Add(date); // 날짜를 dateList에 추가
				}
				// foreach (var item in allLines)
				// {
				// 	string existingDate = item.Split(',')[0];
				// 	for (int i = 0; i < allLines.Count; i++)
				// 	{
				// 		if (existingDate == dates[i])
				// 		{
				// 			throw new Exception($"Failed. Existing Data for the date.\nIf you want to change the data, go to Edit.");
				// 		}
				// 	}

				// }

				bool addingChecker = false;
				bool saveFailChecker = false;
				// 새로운 데이터를 추가
				for (int i = 0; i < logicalSize; i++)
				{
					string newDataDate = dates[i];
					string newData = $"{dates[i]},{values[i]}";
					// 중복 여부 확인
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
				// 파일에 리스트의 내용을 다시 씀
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
					Console.WriteLine($"\nData has been saved. \nPlease note that the values that belong to the existing dates are not changed through Adding.\nIt should be changed in the 'Edit' section.");
				}
				if (saveFailChecker && !addingChecker)
				{
					Console.WriteLine($"Data has not been saved due to existing data. Edit first.");
				}
			}
			else
			{
				// 파일이 없는 경우, 새로운 파일을 생성하여 데이터를 저장
				List<string> allLines = new List<string>();
				allLines.Add("Dates,Values");

				// 배열 크기를 실제 데이터 크기에 맞게 조절하여 데이터를 추가
				for (int i = 0; i < logicalSize; i++)
				{
					string newData = $"{dates[i]},{values[i]}";
					// 중복 여부 확인
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
						Console.WriteLine($"Invalid input for the date. Please EXACTLY follow the format.");
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
				Console.WriteLine($"Data has been added to the MEMORY.\nYour logicalSize is {logicalSize}.\nBut it hasn't been saved onto the file yet.");

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
			Console.WriteLine($"If you want to cancel edit, enter Q");
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
				Console.WriteLine($"Invalid input for the date. Please EXACTLY follow the format.");
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
	Console.WriteLine("Not Implemented Yet");
	//TODO: Replace this code with yours to implement this function.
}