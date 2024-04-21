// Client Class

// Design a class named Client that meets the following requirements:

// A string field to store the client's first name and a corresponding property FirstName with both get and set
// The first name cannot be empty, null, or whitespace; ensure that the stored value is trimmed of leading and trailing whitespace
// A string field to store the client's last name and a corresponding property LastName with both get and set
// The last name cannot be empty, null, or whitespace; ensure that the stored value is trimmed of leading and trailing whitespace
// An int field to store the client's weight in pounds and a corresponding property Weight with both get and set
// The weight must be greater than zero
// An int field to store the client's height in inches and a corresponding property Height with both get and set
// The weight must be greater than zero
// A greedy constructor that requires the first name, last name, weight, and height as parameters
// Use the properties in the constructor for setting the fields to take advantage of any validation checks already coded
// A read-only property named BmiScore that will return as a double the BMI score for the client
// A read-only property named BmiStatus that will return as a string the BMI status for the corresponding BMI score
// A read-only property named FullName that will return as a string the client's full name in the format Lastname, FirstName


using System.Dynamic;

namespace ClientNS
{

    public class Client
    {
        private string _firstName;
        private string _lastName;
        private int _weight;
        private int _height;

        //nongreedy constructor(default)
        //greedy constructor
        public Client()
        {
            FirstName = "XXX";
            LastName = "YYY";
            Weight = 1;
            Height = 1;
        }




        //greedy constructor
        public Client(string firstName, string lastName, int weight, int height)
        {
            FirstName = firstName;
            LastName = lastName;
            Weight = weight;
            Height = height;
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Name is required. Must not be empty or blank.");
                _firstName = value;
            }
        }


        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Name is required. Must not be empty or blank.");
                _lastName = value;
            }
        }
        public int Weight
        {
            get { return _weight; }
            set
            {
                if (value < 0.0)
                    throw new ArgumentException("Age must be a positive value (0 or greater)");
                _weight = value;
            }
        }
        public int Height
        {
            get { return _height; }
            set
            {
                if (value < 0.0)
                    throw new ArgumentException("Age must be a positive value (0 or greater)");
                _height = value;
            }
        }


        public double BmiScore
        {
            get
            {
                double bmiScore = 0.0;
                bmiScore = Weight / Math.Pow(Height, 2) * 703;
                return bmiScore;
            }
        }

        public string BmiStatus
        {
            get
            {
                string bmiStatus = "";
                if (BmiScore <= 18.4)
                    bmiStatus = "Underweight";

                if (BmiScore >= 18.5 && BmiScore <= 24.9)
                    bmiStatus = "Normal";

                if (BmiScore >= 25.0 && BmiScore <= 39.9)
                    bmiStatus = "Overweight";

                if (BmiScore >= 40)
                    bmiStatus = "Obese";

                return bmiStatus;
            }
        }

        // BMI Score	Status
        // <= 18.4	Underweight
        // 18.5 - 24.9	Normal
        // 25.0 - 39.9	Overweight
        // >= 40	Obese
        // Formula: weight / height2 x 703
        // A read-only property named BmiScore that will return as a double the BMI score for the client
        // A read-only property named BmiStatus that will return as a string the BMI status for the corresponding BMI score
        // A read-only property named FullName that will return as a string the client's full name in the format Lastname, FirstName
        public string FullName
        {
            get
            {
                string fullName = LastName + ", " +FirstName;
                return fullName;
            }
        }



        public override string ToString()
        {
            return $"{FirstName},{LastName},{Weight},{Height}";
        }




    }


}