using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renlen.BaseLibrary
{
  public class JsonDocument
  {
    public JsonDocument() { }
    private static JObject _ErrorObject;
    public static JObject ErrorObject
    {
      get
      {
        if (_ErrorObject == null)
          _ErrorObject = LoadJson(@"{ ""error_code"" : ""52002"" }");
        return _ErrorObject;
      }
    }
    public static JObject Load(string filename, Encoding encoding)
    {
      using (StreamReader file = new StreamReader(filename, encoding))
      using (JsonTextReader reader = new JsonTextReader(file))
      {
        try
        {
          return (JObject)JToken.ReadFrom(reader);
        }
        catch
        {
          return ErrorObject;
        }
      }
    }
    public static JObject LoadJson(string json)
    {
      using (StringReader sr = new StringReader(json))
      using (JsonTextReader reader = new JsonTextReader(sr))
      {
        try
        {
          return (JObject)JToken.ReadFrom(reader);
        }
        catch
        {
          return ErrorObject;
        }
      }
    }
  }
}
