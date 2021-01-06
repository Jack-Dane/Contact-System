using MessageApp.Core.Models;
using MessageApp.Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using ContactSystem.Core.Database;

namespace MessageApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            // register the type to create the database connection
            Mvx.IoCProvider.RegisterType<IContactTbl, ContactTbl>();
            RegisterAppStart<MessageViewModel>();
        }
    }
}
