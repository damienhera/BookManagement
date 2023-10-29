using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSystem
{
    class Search : Book//Inherits data from book
    {
        public void getBook(List<Book> bookData, string bookName)//search for book
        {
            var data = bookData.Where(x => x.title.ToLower().Contains(bookName));//selects from list where the title contains similar characters
            data = data.OrderBy(x => bookName).ToList();//orders the names first letter
            if (data.Count() == 0)//if there is no data stored in data
            {
                Console.WriteLine("\nBook not found");//output message
            }
            else if (data.Count() > 0) //if there is data stored
            {
                Console.WriteLine("Index A-Z|| Title || Publisher || Author || Publish Date");
                foreach (var i in data)//loops through data
                {
                    Console.WriteLine("\n{0}|| {1} || {2} || {3} || {4}", i.reference, i.title, i.publisher, i.author, i.pubDate);//outputs each line in data
                }
            }


            Console.WriteLine("\nPress enter to exit.......");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
