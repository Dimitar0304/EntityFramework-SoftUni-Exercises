using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Core.Constants
{
    public static class UserMessageConstants
    {

        public const string Required = "полето {0} е задължително";
        public const string StringLenght = "Полето {0} трябва да е между {1} и {2} символа !";
        public const string UnknownCommand = "Възникна непредвидена грешка";
    }
}
