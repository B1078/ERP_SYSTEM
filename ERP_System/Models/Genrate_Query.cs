using System.Text.RegularExpressions;

namespace ERP_System.Models
{
    public class Genrate_Query
    {
        public string GenerateInsertQuery(object dataObject, string tableName,string RemovebleColumn)
        {
            try
            {
                var OBJ = (object)dataObject;
                var properties = OBJ.GetType().GetProperties();
                var nonNullProperties = properties.Where(p => p.GetValue(OBJ, null) != null);
                string columns = string.Join(", ", nonNullProperties.Select(p => p.Name));
                string values = string.Join(", ", nonNullProperties.Select(p => SanitizeValue(p.GetValue(OBJ, null))));
                return $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        public string GenerateUpdateQuery(object dataObject, string tableName, string conditionColumn, dynamic conditionValue,string removeableColumn)
        {
            try
            {
                var OBJ = (object)dataObject;
                var properties = OBJ.GetType().GetProperties();
                var nonNullProperties = properties.Where(p => p.GetValue(OBJ, null) != null && p.Name != conditionColumn && p.Name != removeableColumn);
                string setClause = string.Join(", ", nonNullProperties.Select(p => $"{p.Name} = {SanitizeValue(p.GetValue(dataObject, null))}"));
                if (string.IsNullOrEmpty(setClause))
                {
                    return "No non-null properties to update.";
                }
                string condition = $"{conditionColumn} = {SanitizeValue(conditionValue)}";
                return $"UPDATE {tableName} SET {setClause} WHERE {condition}";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        static string SanitizeValue(object value)
        {
            if (value == null)
            {
                return "''";
            }
            else if (value is string)
            {
                // Replace non-supported symbols or special characters with an empty string
                string sanitizedValue = Regex.Replace(value.ToString(), "'", "");
                return $"'{sanitizedValue}'";
            }
            else if (value is DateTime)
            {
                return $"'{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}'";
            }
            else if (value is DateOnly)
            {
                return $"'{((DateOnly)value).ToString("yyyy-MM-dd")}'";
            }
            else if (value is TimeOnly)
            {
                TimeOnly timeValue = (TimeOnly)value;
                string formattedTime = $"{timeValue.Hour:D2}:{timeValue.Minute:D2}:{timeValue.Second:D2}.{timeValue.Millisecond:D7}";
                return $"'{formattedTime}'";
            }
            else if (value is TimeSpan)
            {
                TimeSpan timeValue = (TimeSpan)value;
                string formattedTime = $"{timeValue.Hours:D2}:{timeValue.Minutes:D2}:{timeValue.Seconds:D2}.{timeValue.Milliseconds:D7}";
                return $"'{formattedTime}'";
            }
            else
            {
                return value.ToString();
            }
        }
        static T RemoveProperty<T>(T source, string propertyName)
        {
            var propertyToRemove = source.GetType().GetProperty(propertyName);
            if (propertyToRemove == null)
            {
                // Property not found, return the original object
                return source;
            }

            // Create a new object without the specified property
            var newObject = Activator.CreateInstance(source.GetType());
            foreach (var property in source.GetType().GetProperties())
            {
                if (property != propertyToRemove)
                {
                    property.SetValue(newObject, property.GetValue(source));
                }
            }

            return (T)newObject;
        }
    }
}
