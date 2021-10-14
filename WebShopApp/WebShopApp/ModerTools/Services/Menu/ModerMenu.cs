using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopApp
{
    class ModerMenu
    {
        Moderator moder;

        public ModerMenu(Moderator moderInput)
        {
            moder = moderInput;
            if(moder.SpecialKey == 01)
            {
                ModerShowcase();
            }
            else if(moder.SpecialKey == 011)
            {
                Admin admin = new Admin { Login = moder.Login, Password = moder.Password, SpecialKey = moder.SpecialKey };
                AdminMenu AdmMenu = new AdminMenu(admin);
            }
        }

        public void ModerShowcase()
        {

        }        
    }
}
