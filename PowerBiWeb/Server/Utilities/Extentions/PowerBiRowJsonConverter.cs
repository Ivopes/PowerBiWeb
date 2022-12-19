using MetricsAPI.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PowerBiWeb.Server.Utilities.Extentions
{
    public class PowerBiRowJsonConverter : JsonConverter<MetricPortion>
    {
        public override MetricPortion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, MetricPortion value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("rows");

            writer.WriteStartArray();

            foreach (var row in value.Rows)
            {

                writer.WriteStartObject();

                writer.WriteString("Datum", row.Date);

                writer.WriteString("Release", row.Release.ToString());
                
                if (IsValueType(value.AdditionWithSignType))
                    writer.WriteNumber(value.AdditionWithSignName, row.AdditionWithSign);
                else
                    writer.WriteString(value.AdditionWithSignName, row.AdditionWithSign.ToString());

                if (IsValueType(value.AdditionWithoutSignType))
                    writer.WriteNumber(value.AdditionWithoutSignName, row.AdditionWithoutSign);
                else
                    writer.WriteString(value.AdditionWithoutSignName, row.AdditionWithoutSign.ToString());

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
        private bool IsValueType(string type)
        {
            if (type is "System.Int32" or "System.Single" or "System.Double")
                return true;
            return false;
        }
    }
}
