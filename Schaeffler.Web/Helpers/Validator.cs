using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Schaeffler.Helpers
{
    public enum ValidationMethodology
    {
        Custom,
        Required,
        Email,
        Date,
        LessThan,
        GreaterThan,
        Range,
        EqualTo,
        MaxLength,
        MinLength,
        RangeLength,
        RegularExpression,
        Url,
        AlphaOnly,
        NumericOnly,
        AlphaNumericOnly,
        FileExtension
    }

    public class ValidationParam
    {
        public string PropertyName { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
        public Dictionary<ValidationMethodology, string> Methodologies { get; set; }
        public Dictionary<ValidationMethodology, dynamic> Params { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, Func<dynamic, dynamic>> Methods { get; set; }
    }

    public static class Validator
    {
        #region Required
        public static bool Required(ValidationParam param, ModelStateDictionary MSD, string Message)
        {

            bool result = false;

            if (param.Params != null && param.Params.ContainsKey(ValidationMethodology.Required))
            {
                List<bool> objParamResults = new List<bool>();

                for (int i = 0; i < param.Params.Count(); i++)
                {
                    object obj = param.Params[param.Params.ElementAt(i).Key];

                    if (obj is List<object>)
                    {
                        List<object> objParams = (List<object>)obj;
                        foreach (object objParam in objParams)
                        {
                            objParamResults.Add(param.Value != objParam);
                        }
                    }
                }

                result = (objParamResults.Count == 0 || objParamResults.Contains(false));
            }
            else
            {
                result = GetDefault(param.Type) != param.Value; //param != null && (param.Type.GetConstructor(new Type[] { }).Invoke(new object[] { }) != param.Value);
            }

            if (!result)
            {
                param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "This field is required" : Message;

                MSD.AddModelError(param.PropertyName, param.ErrorMessage);
            }

            return result;
        }

        public static object GetDefault(Type t)
        {
            Func<object> f = GetDefault<object>;

            return f.Method.GetGenericMethodDefinition().MakeGenericMethod(t).Invoke(null, null);
        }

        private static T GetDefault<T>()
        {
            return default(T);
        }

        #endregion

        #region Email
        public static bool Email(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            try
            {

                MailAddress m = new MailAddress(Convert.ToString(param.Value));

                return true;
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "Please enter valid email address" : Message;
                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }

                return false;
            }

        }
        #endregion

        #region Date
        public static bool Date(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            DateTime dt;

            object format = string.Empty;

            bool result = param.Value != null
                && param.Params != null
                && param.Params.Count > 0
                && param.Params.TryGetValue(ValidationMethodology.Date, out format)
                && format != null && DateTime.TryParseExact(param.Value.ToString(), format.ToString(), new CultureInfo("en-US"), DateTimeStyles.None, out dt);


            if (!result && string.IsNullOrEmpty(param.ErrorMessage))
            {
                param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "Please enter valid datetime" : Message;
                MSD.AddModelError(param.PropertyName, param.ErrorMessage);
            }

            return result;
        }
        #endregion

        #region EqualTo
        public static void EqualTo(ValidationParam param, ModelStateDictionary MSD, string Message)
        {

            bool result = false;

            if (param.Params != null && param.Params.ContainsKey(ValidationMethodology.EqualTo))
            {
                List<bool> objParamResults = new List<bool>();

                for (int i = 0; i < param.Params.Count(); i++)
                {
                    object obj = param.Params[param.Params.ElementAt(i).Key];

                    if (obj is List<object>)
                    {
                        List<object> objParams = (List<object>)obj;
                        foreach (object objParam in objParams)
                        {
                            objParamResults.Add(Convert.ToString(param.Value) == Convert.ToString(objParam));
                        }
                        result = (objParamResults.Count == 0 || objParamResults.Contains(true));
                    }
                    else
                    {
                        result = Convert.ToString(param.Value) == Convert.ToString(obj);
                    }
                }

            }

            if (!result && string.IsNullOrEmpty(param.ErrorMessage))
            {
                param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "Please enter same value" : Message;
                MSD.AddModelError(param.PropertyName, param.ErrorMessage);
            }

        }
        #endregion

        #region AlphaOnly
        public static void AlphaOnly(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            if (param.Value != null && Convert.ToString(param.Value).ToCharArray().All(c => Char.IsLetter(c)))
            {
                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "Please enter alphabetics only" : Message;
                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region NumericOnly
        public static void NumericOnly(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            long result;
            if (!long.TryParse(Convert.ToString(param.Value), out result))
            {
                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "Please enter numbers only" : Message;
                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region AlphaNumericOnly
        public static void AlphaNumericOnly(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            if (param.Value != null && Convert.ToString(param.Value).ToCharArray().All(c => Char.IsLetter(c) || Char.IsNumber(c)))
            {
                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "Please enter alphanumerics only" : Message;
                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region LessThan
        public static void LessThan(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            dynamic comparisionParam = null;

            if (!(param.Value != null && param.Params != null && param.Params.TryGetValue(ValidationMethodology.LessThan, out comparisionParam) && Convert.ToInt64(param.Value) < Convert.ToInt64(comparisionParam)))
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "Please enter value less than " + Convert.ToString(comparisionParam) : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region GreaterThan
        public static void GreaterThan(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            dynamic comparisionParam = null;

            if (!(param.Value != null && param.Params != null && param.Params.TryGetValue(ValidationMethodology.GreaterThan, out comparisionParam) && Convert.ToInt64(param.Value) > Convert.ToInt64(comparisionParam)))
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? "Please enter value greater than " + Convert.ToString(comparisionParam) : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region Range
        /*Parameters new { Top = 10, Bottom = 0 };*/
        public static void Range(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            dynamic rangeParam = null;

            if (!(param.Value != null && param.Params != null && param.Params.TryGetValue(ValidationMethodology.Range, out rangeParam) && rangeParam != null && Convert.ToInt64(rangeParam.Top) <= Convert.ToInt64(param.Value) <= Convert.ToInt64(rangeParam.Bottom)))
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? string.Format("Please enter value between {0} and {1} ", Convert.ToString(rangeParam.Top), Convert.ToString(rangeParam.Bottom)) : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region MinLength
        /*Parameters 10;*/
        public static void MinLength(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            dynamic lengthParam = null;

            if (!(param.Value != null && param.Params != null && param.Params.TryGetValue(ValidationMethodology.MinLength, out lengthParam) && Convert.ToString(param.Value).Length >= lengthParam))
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? string.Format("Please enter value with minimum length of {0} ", Convert.ToString(lengthParam)) : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region MaxLength
        /*Parameters 10;*/
        public static void MaxLength(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            dynamic lengthParam = null;

            if (!(param.Value != null && param.Params != null && param.Params.TryGetValue(ValidationMethodology.MaxLength, out lengthParam) && Convert.ToString(param.Value).Length <= lengthParam))
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? string.Format("Please enter value with maximum length of {0} ", Convert.ToString(lengthParam)) : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region RangeLength
        /*Parameters new { Top = 10; Bottom = 6};*/
        public static void RangeLength(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            dynamic lengthParam = null;

            if (!(param.Value != null && param.Params != null && param.Params.TryGetValue(ValidationMethodology.Range, out lengthParam) && lengthParam.Bottom <= Convert.ToString(param.Value).Length <= lengthParam.Top))
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? string.Format("Please enter value of length between {0} and {1} ", Convert.ToString(lengthParam.Bottom), Convert.ToString(lengthParam.Top)) : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region RegularExpression
        /*Parameters @"(\w+)\s+(car)";*/
        public static void RegularExpression(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            dynamic regularExpressionParam = null;

            if (!(param.Value != null && param.Params != null && param.Params.TryGetValue(ValidationMethodology.RegularExpression, out regularExpressionParam) && regularExpressionParam != null && new Regex(regularExpressionParam).IsMatch(Convert.ToString(param.Value))))
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? string.Format("Please enter valid input") : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region Url
        public static void Url(ValidationParam param, ModelStateDictionary MSD, string Message)
        {

            if (param.Value != null && !Uri.IsWellFormedUriString(Convert.ToString(param.Value), UriKind.RelativeOrAbsolute))
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? string.Format("Please enter valid url") : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        #region FileExtension
        public static void FileExtension(ValidationParam param, ModelStateDictionary MSD, string Message)
        {
            dynamic fileParam = null;

            bool result = (param.Value != null && param.Params != null && param.Params.TryGetValue(ValidationMethodology.FileExtension, out fileParam) && fileParam != null && fileParam.Length > 0);

            if (result)
            {
                string extension = Path.GetExtension((param.Value as HttpPostedFileBase).FileName);

                result = (fileParam as string[]).OfType<string>().ToList().Contains(extension.Trim('.'));
            }

            if (!result)
            {

                if (string.IsNullOrEmpty(param.ErrorMessage))
                {
                    param.ErrorMessage = (string.IsNullOrEmpty(Message)) ? string.Format("Please select valid file") : Message;

                    MSD.AddModelError(param.PropertyName, param.ErrorMessage);
                }
            }
        }
        #endregion

        public static void Validate(List<ValidationParam> VParams, ModelStateDictionary MSD)
        {
            foreach (ValidationParam vp in VParams)
            {
                foreach (KeyValuePair<ValidationMethodology, string> vm in vp.Methodologies)
                {
                    if (ValidationMethodology.Custom == vm.Key)
                    {

                        foreach (string s in vp.Methods.Keys)
                        {

                            Func<dynamic, dynamic> method = vp.Methods[s];

                            vp.ErrorMessage = method(vp.Params);
                        }
                    }

                    if (ValidationMethodology.Required == vm.Key)
                    {
                        Required(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.Email == vm.Key)
                    {
                        Email(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.Date == vm.Key)
                    {
                        Date(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.EqualTo == vm.Key)
                    {
                        EqualTo(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.AlphaNumericOnly == vm.Key)
                    {
                        AlphaNumericOnly(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.AlphaOnly == vm.Key)
                    {
                        AlphaOnly(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.NumericOnly == vm.Key)
                    {
                        NumericOnly(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.GreaterThan == vm.Key)
                    {
                        GreaterThan(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.LessThan == vm.Key)
                    {
                        LessThan(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.Range == vm.Key)
                    {
                        Range(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.MinLength == vm.Key)
                    {
                        MinLength(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.MaxLength == vm.Key)
                    {
                        MaxLength(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.RangeLength == vm.Key)
                    {
                        RangeLength(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.RegularExpression == vm.Key)
                    {
                        RegularExpression(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.Url == vm.Key)
                    {
                        Url(vp, MSD, vm.Value);
                    }

                    if (ValidationMethodology.FileExtension == vm.Key)
                    {
                        FileExtension(vp, MSD, vm.Value);
                    }

                }
            }
        }

        public static void Validate(object DomainObj, ModelStateDictionary MSD, Dictionary<string, string> requiredFields = null, Dictionary<string, string> emailFields = null, Dictionary<string, string> regularExpressionFields = null)
        {
            string defaultRequiredMessage = "This field is required";

            if (requiredFields != null && requiredFields.Count > 0)
            {
                foreach (KeyValuePair<string, string> requiredField in requiredFields)
                {
                    var prop = DomainObj.GetType().GetProperty(requiredField.Key);
                    var value = prop.GetValue(DomainObj, null);
                    prop.SetValue(DomainObj, null, null);
                    var defaultValue = prop.GetValue(DomainObj, null);
                    if (Convert.ToString(value) == Convert.ToString(defaultValue))
                    {
                        MSD.AddModelError(requiredField.Key, (string.IsNullOrEmpty(requiredField.Value) ? defaultRequiredMessage : requiredField.Value));
                    }
                }
            }
        }

    }
}