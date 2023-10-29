using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BookSystem
{
    
    class Program
    {
        //Global Variables
        public static bool dataRead = false;//Stores a global variable to say that the data has not been read.
        public static string filesRead = "";//stores the names of the files read
        public static List<Book> bookData = new List<Book>();//creates a list by passing the class bookData
        public static int counter = 0;

        //Message
        public static void dataNotReadMsg()
        {
            errorCol();
            Console.WriteLine("Book Data has not been read yet");
            Console.WriteLine("Please make sure to read book data before accessing this function");//error message
            colChangeBlue();
            Console.WriteLine("\nPress enter to exit to menu....");
            Console.ReadLine();
        }

        public static void dataAlrRead()//Data is already
        {
            errorCol();
            Console.WriteLine("Data has already been read");
            colChangeBlue();
            Console.WriteLine("\nPress enter to exit to menu....");
            Console.ReadLine();
            Console.Clear();

        }



        //Colour Code - to indicate if it is an input or an output
        public static void errorCol()
        { Console.ForegroundColor = ConsoleColor.Red; }//Changes the error message colour to red for error messages
        public static void colChangeBlue()//Change text colour to blue for system messages
        { Console.ForegroundColor = ConsoleColor.Blue; }
        public static void colChangeWhite() //Change text to white for user input
        { Console.ForegroundColor = ConsoleColor.White; }


        //Functions
        public static int numCheck(string input)//Checks input to see that its a number
        {
            int num;
            bool check;//to check if the parse worked
            while (true)
            {
                check = int.TryParse(input, out num);//tries to convert
                if (check == true) { break; }//break loop
                else //if failed
                {
                    errorCol();
                    Console.WriteLine("\nPlease enter a number for this option...");//re-enter
                    colChangeWhite();
                    input = Console.ReadLine();
                }
            }

            return num;//return num
        }

        public static string getFilename()//gets the default file location
        {

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;//gets the location of the program location in the file
            string[] file = path.Split(@"\".ToCharArray()); //seperates it to the string file

            int binNum = 0;//stores the location of the bin
            int counter = 0;//counter
            foreach (var i in file)
            {
                if (i == "bin")//if bin is located
                {
                    binNum = counter;//stores the counter num as the bin num
                }
                counter++;
            }
            string binPath = "";
            for (int i = 0; i <= binNum; i++)//loops throught files until it reaches the bin
            {
                binPath = binPath + (file[i] + @"\");

            }

            string folderName = binPath + @"BookData";//joins to the location of the folder


            return folderName;
        }




        public static void searchBook()
        {
            if (dataRead == true)//checks to see if the data is read
            {
                string bookName;
                while (true)
                {
                    colChangeBlue();
                    Console.WriteLine("Enter the name of the book you want to search: ");
                    colChangeWhite();
                    bookName = Console.ReadLine();
                    if (bookName.Trim() == "")
                    {
                        errorCol();
                        Console.WriteLine("\nPlease do not leave the search name blank");
                        Console.WriteLine();
                    }
                    else
                    {
                        break;
                    }
                }

                Search books = new Search();
                colChangeBlue();
                Console.Clear();
                books.getBook(bookData, bookName);
            }
            else if (dataRead == false)//if not read outputs this error
            {
                dataNotReadMsg();
            }
            Console.Clear();

        }


        public static void readMenu()
        {
            bool loopCheck = true;
            string readPath;
            colChangeBlue();
            Console.WriteLine("Please select an option" +
                "\n1.Data stored in program" +
                "\n2.Enter your own data location" +
                "\n3.Go back......");

            while (loopCheck)
            {
                colChangeWhite();
                int input = numCheck(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        readPath = getFilename() + @"\BookData.txt";
                        Console.Clear();
                        readData(readPath);
                        loopCheck = false;
                        break;

                    case 2:
                        Console.Clear();
                        colChangeBlue();
                        Console.WriteLine(@"Please enter the location of where you want to read data from(e.g.C:\Users\damien\document.txt)");
                        colChangeWhite();
                        readPath = Console.ReadLine();
                        readData(readPath);
                        loopCheck = false;
                        break;
                    case 3:
                        loopCheck = false;
                        break;
                    default:
                        errorCol();
                        Console.WriteLine("\nPlease make sure you enter an option from 1 to 3");
                        break;
                }
            }
        }

        public static void readData(string readPath)
        {
            bool exist = File.Exists(readPath); //Used to see if the file exists
            if (exist)
            {
                if (filesRead.Contains(readPath)) { dataAlrRead(); }//if the file is already read output error message
                else//else
                {
                    try
                    {
                        string line;//creates string to hold a line from the txt file
                        string[] curr;
                        using (StreamReader r = new StreamReader(readPath))
                        {
                            while ((line = r.ReadLine()) != null)
                            {
                                line = line.Trim();//Gets rid of whitespace
                                if (line != "")//This check to make sure that the line is not blank
                                {
                                    curr = line.Split(',');//splits it to the array

                                    string ref1 = string.Format("Ref {0}", counter);//selects first letter for reference

                                    bookData.Add(new Book { reference = ref1, title = curr[0], publisher = curr[1], author = curr[2], pubDate = curr[3] });//adds to list
                                    counter += 1;
                                }

                            }

                        }
                        dataRead = true;
                        filesRead = filesRead + ", " + readPath;
                        colChangeBlue();
                        Console.WriteLine("Data read!");//outputs success message
                        Console.WriteLine("\nPress enter to exit to menu....");
                        Console.ReadLine();
                        Console.Clear();
                    }


                    catch (Exception)
                    {
                        errorCol();
                        Console.WriteLine("File not in correct format");//error message
                        colChangeBlue();
                        Console.WriteLine("\nPress enter to exit to menu....");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }
            else
            {
                errorCol();
                Console.WriteLine("The file does not exist");//error message
                Console.WriteLine("Please make sure that the file exists in its given location");
                colChangeBlue();
                Console.WriteLine("\nPress enter to exit to menu....");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public static void displayData()
        {
            if (dataRead == false)//If data has not been read
            {
                dataNotReadMsg();
                Console.Clear();
            }
            else
            {
                bookData = bookData.OrderBy(x => x.reference).ToList();//orders the books by the reference created
                colChangeBlue();
                Console.WriteLine("Reference Number || Title || Publisher || Author || Publish Date");
                foreach (var i in bookData)//outputs the data stored in bookData
                {
                    Console.WriteLine("\n{0}|| {1} || {2} || {3} || {4}", i.reference, i.title, i.publisher, i.author, i.pubDate);
                }
                Console.WriteLine("\n Press enter to go back into menu.....");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public static void writeMenu()
        {
            bool loopCheck = true;
            string writePath;//creates a variable to store the write path
            colChangeBlue();
            Console.WriteLine("Please select an option" + //outputs options
                "\n1.Store data in default location" +
                "\n2.Enter your own data location" +
                "\n3.Go back......");

            while (loopCheck)
            {
                colChangeWhite();
                int input = numCheck(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        writePath = getFilename() + @"\NewBookData.txt";//gets default save location
                        Console.Clear();
                        writeData(writePath);//writes to location
                        loopCheck = false;
                        break;
                    case 2:
                        Console.Clear();
                        colChangeBlue();
                        Console.WriteLine(@"Please enter the location of where you want to write to(e.g.C:\Users\damien\document.txt)");
                        colChangeWhite();
                        writePath = Console.ReadLine();//takes in user input
                        writeData(writePath);//writes to location
                        loopCheck = false;
                        break;
                    case 3:
                        loopCheck = false;
                        break;
                    default:
                        errorCol();
                        Console.WriteLine("\nPlease make sure you enter an option from 1 to 3");
                        break;
                }
            }
        }


        public static void writeData(string writePath)
        {

            if (dataRead == false)//If data has not been read
            {
                dataNotReadMsg();
                Console.Clear();
            }
            else
            {
                bool exist = File.Exists(writePath);//checks to see if the file exists
                if (exist == true)
                {
                    try//tries this section of the code first and if it fails, the program switches to catch
                    {

                        using (StreamWriter s = new StreamWriter(writePath))
                        {
                            bookData = bookData.OrderBy(x => x.reference).ToList();//orders the books by reference
                            foreach (var i in bookData)
                            {
                                s.WriteLine("{0}|| {1} || {2} || {3} || {4}", i.reference, i.title, i.publisher, i.author, i.pubDate);//writes to file
                            }
                        }

                        colChangeBlue();
                        Console.WriteLine("The data has been written into the file");
                        Console.WriteLine("The file can be found at: {0}", writePath);
                        Console.WriteLine("\n Press enter to go back into menu.....");
                        Console.ReadLine();
                        Console.Clear();


                    }
                    catch (Exception)//if file not found
                    {
                        errorCol();
                        Console.WriteLine("The file does not exist");//error message
                        Console.WriteLine("Please make sure that the file exists in its given location");

                        colChangeBlue();
                        Console.WriteLine("\nPress enter to exit to menu....");
                        Console.ReadLine();
                        Console.Clear();

                    }

                }

                else//if file does not exist
                {
                    //error message
                    errorCol();
                    Console.WriteLine("The file does not exist");
                    Console.WriteLine("Make sure that there is a file at {0}", writePath);

                    colChangeBlue();
                    Console.WriteLine("\n Press enter to go back into menu.....");
                    Console.ReadLine();
                    Console.Clear();

                }
            }


        }



        public static void help()
        {
            colChangeBlue();
            Console.WriteLine("************Welcome to the book Indexing System************");

            Console.WriteLine("\nThis system allows you to import book data and create a releavent reference. It also alows you to " +
                "\nexport the new data and also search for specific books.");
            Console.WriteLine("\n***Data Handling***");
            Console.WriteLine("\nWhen importing data ensure that the document is a .txt file and the data inside follows the format:" +
                "\nTitle, Publisher, Author, Publish Date ");
            Console.WriteLine("\nTo change the default data you can access the document location at " + @"NewBookSystemDesign\bin\BookData\BookData.txt." +
                "\nWhen inserting a new txt document to be read to the default location, make sure that you name it 'BookData'." +
                "\nIf errors appear when trying to read from the default location, you need to make sure that the txt document" +
                "\nstored there is named BookData as if it is not, it would not work");

            Console.WriteLine("\n\nTo access the default export location, you can find it at " + @"NewBookSystemDesign\bin\BookData\BookData.txt."
                + "\nIf errors appear when exporting to the default location make sure that there is" +
                "\na txt document stored there named 'NewBookData'.");

            Console.WriteLine("\n\nWhen you are exporting and importing to you own documents you need to make sure that you" +
                "\nspecify the entire address of where your document is save otherwise, the program would" +
                "\noutput a message saying that the file does not exist");


            Console.WriteLine("\nPress enter to go back to menu......");
            Console.ReadLine();
            //How to use the app
            //QandA
        }


        public static bool exit()
        {
            bool b = false;
            while (true)
            {
                colChangeBlue();
                Console.WriteLine("Are you sure you want to exit(y or n)");
                colChangeWhite();
                string ans = Console.ReadLine();
                if (ans.ToLower() == "y") { b = true; break; }
                else if (ans.ToLower() == "n") { b = false; break; }
                else
                { errorCol(); Console.WriteLine("\nPlease enter y or n for this option"); }
            }

            return b;
        }



        //Start
        static void Main(string[] args)
        {
            int input = 0;//Stores the user input
            while (input != 6)
            {
                bool loopCheck = true;//Checks if its still possible to loop
                colChangeBlue();
                Console.WriteLine("************Welcome to the book Indexing System************");//menu
                Console.WriteLine("Welcome to the menu. Please select an option:" +
                    "\n1.Read Data" +
                    "\n2.View Data" +
                    "\n3.Export Data" +
                    "\n4.Search for Book" +
                    "\n5.Help" +
                    "\n6.Exit");
                colChangeWhite();
                input = numCheck(Console.ReadLine());
                while (loopCheck == true)
                {
                    switch (input)//Allows program to see what the user inputs and executes code accordingly
                    {
                        case 1:
                            loopCheck = false;
                            Console.Clear();
                            readMenu();
                            break;
                        case 2:
                            loopCheck = false;
                            Console.Clear();
                            displayData();
                            break;
                        case 3:
                            loopCheck = false;
                            Console.Clear();
                            writeMenu();
                            break;
                        case 4:
                            loopCheck = false;
                            Console.Clear();
                            searchBook();
                            break;

                        case 5:
                            loopCheck = false;
                            Console.Clear();
                            help();
                            break;

                        case 6:
                            loopCheck = false;
                            Console.Clear();
                            bool bExit = exit();//stores the answer
                            if (bExit == false) { input = 0; }//if they dont want to exit the answer is set to zero so that they can loop again
                            break;

                        default://error managing
                            errorCol();
                            Console.WriteLine("\nPlease make sure that you enter an option from (1-5)");
                            colChangeWhite();
                            input = numCheck(Console.ReadLine());
                            break;
                    }

                }

                Console.Clear();
            }
        }
    }
}
