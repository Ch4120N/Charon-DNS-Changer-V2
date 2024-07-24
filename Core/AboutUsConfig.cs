using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charon_DNS_Changer.Core
{
    class AboutUsConfig
    {
        public static string about_us_content()
        {
            string programmer_name = "AmirHossein Ghanmi";
            string programmer_nickname = "Ch4120N";
            string programmer_email = "Ch4120ni@Gmail.com";
            string content = $"As a hacker and someone who works in the field of cyber security. I believe that the more you advance in the world of technology and even cyber security, the more you realize that you don't know a lot.\nI, {programmer_name}, With Nickname, {programmer_nickname}, I pool my knowledge of computer and cyber security to create such software to expand the knowledge level of people from the hacker community.\nSo that I can change the digital intellectual and scientific level with all my strength.\nYou can also contact me: {programmer_email}";
            return content;
        }
    }
}
