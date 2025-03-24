using System.Globalization;
using System.IO;
using System.Reflection;

namespace TeamBuilder.Util
{
    public static class Util
    {
        static Util()
        {
            //This is the common util file for Team Builder!
        }


        //Todo read to a database.
        //Todo create algorithm to parse rosters of different shapes.
        public static void readFiles()
        {
            //Todo add config var for each path.
            string directoryName = @"C:\TeamBuilder\Rosters";

            foreach (string fileName in Directory.GetFiles(directoryName))
            {
                readFile(fileName);
                // Do something with the file content
            }
        }
        /// <summary>
        /// Uploads each roster, to a unique guid for userid and teamid, per file.
        /// I suppose that the only unique thing is time slot, as you could not host multiple events at once, therefore they must be the same? Todo.
        /// </summary>
        /// <param name="fileName"></param>
        public static bool readFile(string fileName)
        {
            bool success = false;
            fileName = fileName.Trim();
            if (String.IsNullOrEmpty(fileName))
            {
                return success;
            }
            //Todo, it looks correct, why does the Directory return false then?
            /*else if (Directory.Exists(fileName) == false)
            {
                Console.WriteLine("Error, invalid file or directory. Rosters should go to the Rosters folder at this project level.");
            }*/
            else
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string directory = Path.GetDirectoryName(fileName);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = String.Empty;
                    Console.WriteLine("Reading in roster file name: " + fileName);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    while ((s = sr.ReadLine())
                        != null)
                    {
                        //do minimal amount of work here
                        Console.WriteLine(s);
                    }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                }
                SaveParsedRoster(fileName);
            }
            success = true;
            return success;
        }
        public static void SaveParsedRoster(string fromDir)
        {
            string sourcedirectory = fromDir;//@"C:\source";
            string sourcedirectoryPath = Path.GetDirectoryName(fromDir);
            string destinationdirectoryPath = @"C:\TeamBuilder\Rosters_Parsed\Success";
            string backupdirectory = @"C:\TeamBuilder\Backup";
            try
            {
                if (Directory.Exists(sourcedirectoryPath) == true && File.Exists(fromDir) == true)
                {
                    if (Directory.Exists(destinationdirectoryPath))
                    {
                        //Directory.Delete(destinationdirectory);
                        Directory.Move(sourcedirectoryPath, backupdirectory + DateTime.Now.ToString("_MMMdd_yyyy_HHmmss"));
                        
                        //Todo, simply copy the roster file also.
                        //Directory.Move(sourcedirectory, destinationdirectoryPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(destinationdirectoryPath);
                        Directory.Move(sourcedirectory, destinationdirectoryPath);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="PropertiesToIgnore">an optional list of property names which will NOT be copied</param>
        /// <param name="IgnoreNullProperties">when true will not update properties where the source is null</param>
        /// <param name="CoerceDataType">when true, will attempt to coerce the source property to the destination property (e.g. int to decimal) </param>
        /// <param name="ThrowOnTypeMismatch">when true, will throw a InvalidCastException if the data cannot be coerced</param>
        /// <exception cref="InvalidCastException">if there is a data type mismatch between source/destination and ThrowOnTypeMismatch is enabled and unable to coerce the data type.</exception>
        /// <returns>true if any properties were changed</returns>
        /// References: https://stackoverflow.com/questions/930433/apply-properties-values-from-one-object-to-another-of-the-same-type-automaticall
        public static bool CopyProperties(this object source, object destination, IEnumerable<string>? PropertiesToIgnore = null, bool IgnoreNullProperties = false, bool ThrowOnTypeMismatch = true, bool CoerceDataType = true)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Collect all the valid properties to map
            var results = (from srcProp in typeSrc.GetProperties()
                           let tgtProp = typeDest.GetProperty(srcProp.Name)
                           where srcProp.CanRead
                           && tgtProp != null
                           && tgtProp.GetSetMethod(true) != null && !tgtProp.GetSetMethod(true).IsPrivate
                           && (tgtProp.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                           && !(
                               from i in PropertiesToIgnore ?? Enumerable.Empty<string>()
                               select i
                             ).Contains(srcProp.Name)
                           && (!IgnoreNullProperties || srcProp.GetValue(source, null) != null)
                           select new { sourceProperty = srcProp, targetProperty = tgtProp }).ToList();

            bool PropertyChanged = false;
            //map the properties
            foreach (var props in results)
            {
                object? srcValue = props.sourceProperty.GetValue(source, null);
                object? dstValue = props.targetProperty.GetValue(destination, null);
                if (props.targetProperty.PropertyType.IsAssignableFrom(props.sourceProperty.PropertyType))
                {
                    props.targetProperty.SetValue(destination, srcValue, null);
                }
                else
                {
                    try
                    {
                        if (!CoerceDataType)
                        {
                            throw new InvalidCastException($"Types do not match, source: {props.sourceProperty.PropertyType.FullName}, target: {props.targetProperty.PropertyType.FullName}.");
                        }

                        if (srcValue != null)
                        {
                            // determine if nullable type
                            Type tgtType = Nullable.GetUnderlyingType(props.targetProperty.PropertyType);
                            // if it is, use the underlying type
                            // without this we cannot convert int? -> decimal? when value is not null
                            if (tgtType != null)
                            {
                                props.targetProperty.SetValue(destination, Convert.ChangeType(srcValue, tgtType, CultureInfo.InvariantCulture), null);
                            }
                            else // otherwise use the original type
                            {
                                props.targetProperty.SetValue(destination, Convert.ChangeType(srcValue, props.targetProperty.PropertyType, CultureInfo.InvariantCulture), null);
                            }
                        }
                        else // if null we can just set it as null
                        {
                            props.targetProperty.SetValue(destination, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ThrowOnTypeMismatch)
                        {
                            throw new InvalidCastException($"Unable to copy property {props.sourceProperty.Name} with value {srcValue} from object of type ({typeSrc.FullName}) to type ({typeDest.FullName}), Error: {ex.Message}");
                        }
                        // else ignore update
                    }
                    object? newdstValue = props.targetProperty.GetValue(destination, null);
                    if ((newdstValue == null && dstValue != null) || !newdstValue.Equals(dstValue))
                    {
                        PropertyChanged = true;
                    }
                }
            }

            return PropertyChanged;
        }
    }
}
