using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ParticleButtons.Models
{
    public class ParticleFunction
    {
        DataContractJsonSerializer serializer;

        public ParticleFunction()
        {
            serializer = new DataContractJsonSerializer(typeof(ParticleFunction));
        }

        public bool Validate() {
            if (String.IsNullOrEmpty(FuncName) ||
                String.IsNullOrEmpty(ButtonName) ||
                String.IsNullOrEmpty(Token) ||
                String.IsNullOrEmpty(DeviceId))
                return false;
            else return true;
        }

        public string ButtonName { get; set; }
        public string FuncName { get; set; }
     
        public string Token { get; set; }

        public string Args { get; set; }

        public string DeviceId { get; set; }

        public int Order { get; set; }

        public bool Enabled { get; set; }

        public bool Saved { get; set; }

        internal string ToJson() {

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);

                string json = Encoding.Default.GetString(stream.ToArray());

                return json;
            }
        }
    }
}
