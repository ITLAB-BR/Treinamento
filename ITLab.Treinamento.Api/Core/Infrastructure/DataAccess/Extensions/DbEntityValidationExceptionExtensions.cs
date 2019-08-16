using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

public static class DbEntityValidationExceptionExtensions
{
    public static DbEntityValidationException TransformToThrow(this DbEntityValidationException ex)
    {
        var validationErrorBuilder = new StringBuilder();

        foreach (var entityValidationError in ex.EntityValidationErrors)
        {
            var entry = entityValidationError.Entry.Entity.ToString();

            foreach (var validationError in entityValidationError.ValidationErrors)
            {
                if (!String.IsNullOrEmpty(validationError.PropertyName))
                {
                    validationErrorBuilder.AppendLine(String.Format("Entry {0}: - Property:{1} - Error Message: {2}",
                                             entry,
                                             validationError.PropertyName,
                                             validationError.ErrorMessage));
                }
                else
                {
                    validationErrorBuilder.AppendLine(String.Format("Entry {0}: - Error Message: {1}",
                                             entry,
                                             validationError.ErrorMessage));
                }
            }
        }

        return new DbEntityValidationException(validationErrorBuilder.ToString(), ex);
    }
}