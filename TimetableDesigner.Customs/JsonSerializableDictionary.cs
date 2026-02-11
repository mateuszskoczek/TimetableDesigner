using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Customs
{
    [JsonArray]
    public class JsonSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
    { }
}
