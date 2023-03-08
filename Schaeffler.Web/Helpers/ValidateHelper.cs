using Schaeffler.Domain;
using System;
using System.Web.Mvc;

namespace Schaeffler.Helpers
{
    public static class ValidateHelper
    {

        public static bool IsValid(this User U, ModelStateDictionary MSD)
        {
            bool isValid = true;
            if (U != null)
            {
                if (string.IsNullOrEmpty(U.Name))
                {
                    MSD.AddModelError(nameof(U.Name), "Please Enter Name");
                    isValid = false;
                }

                if (string.IsNullOrEmpty(U.Email))
                {
                    MSD.AddModelError(nameof(U.Name), "Please Enter Email");
                    isValid = false;
                }
                if (string.IsNullOrEmpty(U.ContactNo))
                {
                    MSD.AddModelError(nameof(U.ContactNo), "Please Enter Contact No");
                    isValid = false;
                }
                if (string.IsNullOrEmpty((U != null && U.role != null ? U.role.Id.ToString() : "")))
                {
                    MSD.AddModelError(nameof(U.role.Id), "Please Select User Role");
                    isValid = false;
                }


            }

            return isValid;
        }
       

    }
}