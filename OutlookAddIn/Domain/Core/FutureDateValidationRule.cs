﻿using System;
using System.Globalization;
using System.Windows.Controls;

namespace OutlookAddin.Domain
{
    public class FutureDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out DateTime time)) return new ValidationResult(false, "Invalid date");

            return time.Date < DateTime.Now.Date
                ? new ValidationResult(false, "Future date required")
                : ValidationResult.ValidResult;
        }
    }
}
