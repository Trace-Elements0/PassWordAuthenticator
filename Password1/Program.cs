using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Password1
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string,string> users = new Dictionary<string, string>();
            string x = new string('=',30);
            string[] messages =
            {
                //0//1st Screen
                "\n\n\t\t\t" + x + "\n\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\t1.Establish Account\n\n\t\t\t2.Authenticate user\n\n\t\t\t3.Exit\n\n\t\t\t" + x + "\n\n\t\t\t Enter Selection: (Selection cannot be empty)\n\n\t\t\t",
                //1//Choice 1
                "\n\n\t\t\t" + x + "\n\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tESTABLISH NEW ACCOUNT\n\n\t\t\tUsername:\n\n\t\t\tPassword:\n\n\t\t\t" + x + "\n\n\t\t\t(Password must contain  min 6 chars,\n\n\t\t\t max 12 chars, no white space, 1 upper\n\n\t\t\t case letter, 1 lower case letter no two\n\n\t\t\t similar chars consecutively at least 1 special char)\n\n\t\t\t (Separate your username and password with a space)\n\n\t\t\tEnter new Username and Password: ",

                //2//Choice 2
                "\n\n\t\t\t" + x + "\n\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tAUTHENTICATE ACCOUNT\n\n\t\t\tUsername:\n\n\t\t\tPassword:\n\n\t\t\t" + x +  "\n\n\t\t\t(Password must contain  min 6 chars,\n\n\t\t\tmax 12 chars, no white space, 1 upper\n\n\t\t\tcase letter, 1 lower case letter no two\n\n\t\t\tsimilar chars consecutively at least 1 special char)\n\n\t\t\t (Separate your username and password with a space)\n\n\t\t\tEnter new Username and Password: ",
                //3//Choice 3
                "\n\n\t\t\t" + x + "\n\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tGoodbye",
                //4//Invalid Selection
                "\n\n\t\t\t" + x + "\n\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tINVALID SELECTION\n\n\t\t\t1.Establish Account\n\n\t\t\t2.Authenticate user\n\n\t\t\t3.Exit\n\n\t\t\t" + x + "\n\n\t\t\tEnter Selection: (Selection cannot be empty)",
                //5//Invalid selection exceed attempts
                "\n\n\t\t\t" + x + "\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tINVALID SELECTION ATTEMPTS EXCEEDED\n\n\t\t\t\tGoodbye\n\n\t\t\t" + x,
                //6//Validation successful
                "\n\n\t\t\t" + x + "\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tUSER SUCCESSFULLY AUTHENTICATED\n\n\t\t\t" + x,
                //7//Invalid Password
                "\n\n\t\t\t" + x + "\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tESTABLISH NEW ACCOUNT\n\n\t\t\tINVALID PASSWORD\n\n\t\t\t" + x +"\n\n\t\t\t(Password must contain  min 6 chars,\n\n\t\t\tmax 12 chars, no white space, 1 upper\n\t\t\t case letter, 1 lower case letter no two\n\t\t\t similar chars consecutively at least 1 special char)\n\n\t\t\t (Separate your username and password with a space)",
                //8//Established successful
                "\n\n\t\t\t" + x + "\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tUSER SUCCESSFULLY ESTABLISHED NEW ACCOUNT\n\n\t\t\t" + x,
                //9//Username does not exist
                "\n\n\t\t\t" + x + "\n\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tUSERNAME DOES NOT EXIST\n\n\t\t\t\t" + x,
                //10//Incorrect password
                "\n\n\t\t\t" + x + "\n\n\t\t\tSYSTEM PASSWORD AUTHENTICATOR\n\n\t\t\tPASSWORD INCORRECT\n\n\t\t\t\t" + x,
            };
            
            int n = 0;
            do
            {
                Console.Clear();
                Console.Write(messages[0]);
                string selection = Console.ReadLine();
                switch (selection)
                {
                    case "1":
                        bool  notEstablished = true;
                        while (notEstablished)
                        {
                            Console.Clear();
                            Console.Write(messages[1]);
                            var newUnPd = Console.ReadLine();
                            string[] newseparatedUnPd = newUnPd.Split(" ");
                            if (CheckPassword(newseparatedUnPd[1]))
                            {
                                string newcheckedEncrypted = Md5Hash(newseparatedUnPd[1]);
                                users.TryAdd(newseparatedUnPd[0], newcheckedEncrypted);
                                Console.Clear();
                                Console.Write(messages[8]);
                                Thread.Sleep(5000);
                                notEstablished = false;
                            }
                            else
                            {
                                Console.Clear();
                                Console.Write(messages[7]);
                                Thread.Sleep(5000);
                            }
                        }
                        break;



                    case "2":
                        bool notAuthenticated = true;
                        while (notAuthenticated)
                        {
                            Console.Clear();
                            Console.Write(messages[2]);
                            string oldUnPd = Console.ReadLine();
                            string[] oldseparatedUnPd = oldUnPd.Split(" ");

                            Dictionary<string, string> usersCopy = new Dictionary<string, string>();

                            usersCopy.Add(oldseparatedUnPd[0], oldseparatedUnPd[1]);
                            foreach (KeyValuePair<string, string> item in users)
                            {
                                if (CheckUser(users, oldseparatedUnPd[0]))
                                {
                                    if (VerifyPassword(users, oldseparatedUnPd[1]))
                                    {
                                        Console.Clear();
                                        Console.Write(messages[6]);
                                        Thread.Sleep(5000);
                                        notAuthenticated = false;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.Write(messages[10]);
                                        Thread.Sleep(5000);
                                    }
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.Write(messages[9]);
                                    Thread.Sleep(5000);
                                }
                            }
                        }
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine(messages[3]);
                        foreach (var i in users)
                        {
                            string username = i.Key;
                            string password = i.Value;
                            Console.WriteLine($"\n\n\t\t\tExisting User:\t\tUsername: {username} Password: {password}");
                            Thread.Sleep(5000);

                        }
                        break;
                    default:
                        Console.Clear();
                        Console.Write(messages[4]);
                        Thread.Sleep(5000);
                        break;
                }
                n++;
            } while (n != 10);
            Console.Clear();
            Console.Write(messages[5]);

            //https://www.csharpstar.com/c-program-to-check-password/
            static bool CheckPassword(string pass)
            {
                //min 6 chars, max 12 chars
                if (pass.Length < 6 || pass.Length > 12)
                    return false;

                //No white space
                if (pass.Contains(" "))
                    return false;

                //At least 1 upper case letter
                if (!pass.Any(char.IsUpper))
                    return false;

                //At least 1 lower case letter
                if (!pass.Any(char.IsLower))
                    return false;

                //No two similar chars consecutively
                for (int i = 0; i < pass.Length - 1; i++)
                {
                    if (pass[i] == pass[i + 1])
                        return false;
                }

                if (string.IsNullOrEmpty(pass))
                {
                    return false;
                }
                //At least 1 special char
                string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
                char[] specialCharactersArray = specialCharacters.ToCharArray();
                foreach (char c in specialCharactersArray)
                {
                    if (pass.Contains(c))
                        return true;
                }
                return false;
            }

            static string Md5Hash(string data)
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder sBuilder = new StringBuilder();
                foreach (byte b in hash)
                {
                    sBuilder.AppendFormat("{0:x2}", b);
                }
                return sBuilder.ToString();
            }
            // Verify a hash against a string.
            static bool CheckUser(Dictionary<string, string> users, string x)
            {
                if (users.ContainsKey(x)) return true;
                return false;
            }
            static bool VerifyPassword(Dictionary<string, string> users, string v)
            {
                if (users.ContainsValue(v)) return true;
                return false;
            }

        }

    }
}
