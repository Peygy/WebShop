using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    public class Moderator // Класс Модератора / Moderator class
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string SpecialKey { get; set; }
    }
}
