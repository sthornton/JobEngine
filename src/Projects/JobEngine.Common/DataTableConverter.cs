﻿using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

namespace JobEngine.Common
{

    /// <summary>
    /// Converts a DataTable to JSON. Note no support for deserialization
    /// </summary>
    public class DataTableConverter : JsonConverter
    {

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param><returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(System.Type objectType)
        {
            //Return objectType = GetType(DataTable)
            return typeof(DataTable).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the json.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param><returns></returns>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, System.Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            DataTable table = new DataTable();

            if (jObject["TableName"] != null)
            {
                table.TableName = jObject["TableName"].ToString();
            }

            if (jObject["Columns"] == null)
                return table;

            // Loop through the columns in the table and apply any properties provided
            foreach (JObject jColumn in jObject["Columns"])
            {
                DataColumn column = new DataColumn();
                JToken token = default(JToken);

                token = jColumn.SelectToken("AllowDBNull");
                if (token != null)
                {
                    column.AllowDBNull = token.Value<bool>();
                }

                token = jColumn.SelectToken("AutoIncrement");
                if (token != null)
                {
                    column.AutoIncrement = token.Value<bool>();
                }

                token = jColumn.SelectToken("AutoIncrementSeed");
                if (token != null)
                {
                    column.AutoIncrementSeed = token.Value<long>();
                }

                token = jColumn.SelectToken("AutoIncrementStep");
                if (token != null)
                {
                    column.AutoIncrementStep = token.Value<long>();
                }

                token = jColumn.SelectToken("Caption");
                if (token != null)
                {
                    column.Caption = token.Value<string>();
                }

                token = jColumn.SelectToken("ColumnName");
                if (token != null)
                {
                    column.ColumnName = token.Value<string>();
                }

                // Allowed data types: http://msdn.microsoft.com/en-us/library/system.data.datacolumn.datatype.aspx
                token = jColumn.SelectToken("DataType");
                if (token != null)
                {
                    string dataType = token.Value<string>();
                    if (dataType == "Byte[]")
                    {
                        column.DataType = typeof(System.Byte[]);
                    }
                    else
                    {
                        // All allowed data types exist in the System namespace
                        column.DataType = Type.GetType(string.Concat("System.", dataType));
                    }
                }

                token = jColumn.SelectToken("DateTimeMode");
                if (token != null)
                {
                    column.DateTimeMode = (DataSetDateTime)Enum.Parse(typeof(System.Data.DataSetDateTime), token.Value<string>());
                }

                // Can't set default value on auto increment column
                if (!column.AutoIncrement)
                {
                    token = jColumn.SelectToken("DefaultValue");
                    if (token != null)
                    {
                        // If a default value is provided then cast to the columns data type
                        switch (column.DataType.Name)
                        {
                            case "Boolean":
                                bool defaultValueBool = false;
                                if (bool.TryParse(token.ToString(), out defaultValueBool))
                                {
                                    column.DefaultValue = defaultValueBool;
                                }
                                break;
                            case "Byte":
                                byte defaultValueByte = 0;
                                if (byte.TryParse(token.ToString(), out defaultValueByte))
                                {
                                    column.DefaultValue = defaultValueByte;
                                }
                                break;
                            case "Char":
                                char defaultValueChar = '\0';
                                if (char.TryParse(token.ToString(), out defaultValueChar))
                                {
                                    column.DefaultValue = defaultValueChar;
                                }
                                break;
                            case "DateTime":
                                DateTime defaultValueDateTime = default(DateTime);
                                if (DateTime.TryParse(token.ToString(), out defaultValueDateTime))
                                {
                                    column.DefaultValue = defaultValueDateTime;
                                }
                                break;
                            case "Decimal":
                                decimal defaultValueDeciumal = default(decimal);
                                if (decimal.TryParse(token.ToString(), out defaultValueDeciumal))
                                {
                                    column.DefaultValue = defaultValueDeciumal;
                                }
                                break;
                            case "Double":
                                double defaultValueDouble = 0;
                                if (double.TryParse(token.ToString(), out defaultValueDouble))
                                {
                                    column.DefaultValue = defaultValueDouble;
                                }
                                break;
                            case "Guid":
                                Guid defaultValueGuid = default(Guid);
                                if (Guid.TryParse(token.ToString(), out defaultValueGuid))
                                {
                                    column.DefaultValue = defaultValueGuid;
                                }
                                break;
                            case "Int16":
                                Int16 defaultValueInt16 = default(Int16);
                                if (Int16.TryParse(token.ToString(), out defaultValueInt16))
                                {
                                    column.DefaultValue = defaultValueInt16;
                                }
                                break;
                            case "Int32":
                                Int32 defaultValueInt32 = default(Int32);
                                if (Int32.TryParse(token.ToString(), out defaultValueInt32))
                                {
                                    column.DefaultValue = defaultValueInt32;
                                }
                                break;
                            case "Int64":
                                Int64 defaultValueInt64 = default(Int64);
                                if (Int64.TryParse(token.ToString(), out defaultValueInt64))
                                {
                                    column.DefaultValue = defaultValueInt64;
                                }
                                break;
                            case "SByte":
                                sbyte defaultValueSByte = 0;
                                if (sbyte.TryParse(token.ToString(), out defaultValueSByte))
                                {
                                    column.DefaultValue = defaultValueSByte;
                                }
                                break;
                            case "Single":
                                float defaultValueSingle = 0;
                                if (float.TryParse(token.ToString(), out defaultValueSingle))
                                {
                                    column.DefaultValue = defaultValueSingle;
                                }
                                break;
                            case "String":
                                column.DefaultValue = token.ToString();
                                break;
                            case "TimeSpan":
                                TimeSpan defaultValueTimeSpan = default(TimeSpan);
                                if (TimeSpan.TryParse(token.ToString(), out defaultValueTimeSpan))
                                {
                                    column.DefaultValue = defaultValueTimeSpan;
                                }
                                break;
                            case "UInt16":
                                UInt16 defaultValueUInt16 = default(UInt16);
                                if (UInt16.TryParse(token.ToString(), out defaultValueUInt16))
                                {
                                    column.DefaultValue = defaultValueUInt16;
                                }
                                break;
                            case "UInt32":
                                UInt32 defaultValueUInt32 = default(UInt32);
                                if (UInt32.TryParse(token.ToString(), out defaultValueUInt32))
                                {
                                    column.DefaultValue = defaultValueUInt32;
                                }
                                break;
                            case "UInt64":
                                UInt64 defaultValueUInt64 = default(UInt64);
                                if (UInt64.TryParse(token.ToString(), out defaultValueUInt64))
                                {
                                    column.DefaultValue = defaultValueUInt64;
                                }
                                break;
                            case "Byte[]":
                                break;
                            // Leave as null
                        }
                    }
                }

                token = jColumn.SelectToken("MaxLength");
                if (token != null)
                {
                    column.MaxLength = token.Value<int>();
                }

                token = jColumn.SelectToken("ReadOnly");
                if (token != null)
                {
                    column.ReadOnly = token.Value<bool>();
                }

                token = jColumn.SelectToken("Unique");
                if (token != null)
                {
                    column.Unique = token.Value<bool>();
                }

                table.Columns.Add(column);
            }

            // Add the rows to the table
            if (jObject["Rows"] != null)
            {
                foreach (JArray jRow in jObject["Rows"])
                {
                    DataRow row = table.NewRow();
                    // Each row is just an array of objects
                    row.ItemArray = jRow.ToObject<System.Object[]>();
                    table.Rows.Add(row);
                }
            }

            // Add the primary key to the table if supplied
            if (jObject["PrimaryKey"] != null)
            {
                List<DataColumn> primaryKey = new List<DataColumn>();
                foreach (JValue jPrimaryKey in jObject["PrimaryKey"])
                {
                    DataColumn column = table.Columns[jPrimaryKey.ToString()];
                    if (column == null)
                    {
                        throw new ApplicationException("Invalid primary key.");
                    }
                    else
                    {
                        primaryKey.Add(column);
                    }
                }
                table.PrimaryKey = primaryKey.ToArray();
            }

            return table;
        }

        /// <summary>
        /// Writes the json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            DataTable table = value as DataTable;

            writer.WriteStartObject();

            writer.WritePropertyName("TableName");
            writer.WriteValue(table.TableName);

            writer.WritePropertyName("Columns");
            writer.WriteStartArray();

            foreach (DataColumn column in table.Columns)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("AllowDBNull");
                writer.WriteValue(column.AllowDBNull);
                writer.WritePropertyName("AutoIncrement");
                writer.WriteValue(column.AutoIncrement);
                writer.WritePropertyName("AutoIncrementSeed");
                writer.WriteValue(column.AutoIncrementSeed);
                writer.WritePropertyName("AutoIncrementStep");
                writer.WriteValue(column.AutoIncrementStep);
                writer.WritePropertyName("Caption");
                writer.WriteValue(column.Caption);
                writer.WritePropertyName("ColumnName");
                writer.WriteValue(column.ColumnName);
                writer.WritePropertyName("DataType");
                writer.WriteValue(column.DataType.Name);
                writer.WritePropertyName("DateTimeMode");
                writer.WriteValue(column.DateTimeMode.ToString());
                writer.WritePropertyName("DefaultValue");
                writer.WriteValue(column.DefaultValue);
                writer.WritePropertyName("MaxLength");
                writer.WriteValue(column.MaxLength);
                writer.WritePropertyName("Ordinal");
                writer.WriteValue(column.Ordinal);
                writer.WritePropertyName("ReadOnly");
                writer.WriteValue(column.ReadOnly);
                writer.WritePropertyName("Unique");
                writer.WriteValue(column.Unique);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            writer.WritePropertyName("Rows");
            writer.WriteStartArray();

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached)
                {
                    writer.WriteStartArray();

                    for (int index = 0; index <= table.Columns.Count - 1; index++)
                    {
                        writer.WriteValue(row[index]);
                    }

                    writer.WriteEndArray();
                }
            }

            writer.WriteEndArray();

            // Write out primary key if the table has one. This will be useful when deserializing the table.
            // We will write it out as an array of column names
            writer.WritePropertyName("PrimaryKey");
            writer.WriteStartArray();
            if (table.PrimaryKey.Length > 0)
            {
                foreach (DataColumn column in table.PrimaryKey)
                {
                    writer.WriteValue(column.ColumnName);
                }
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

    }

}
