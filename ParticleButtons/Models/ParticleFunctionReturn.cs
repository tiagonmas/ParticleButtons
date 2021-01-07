
namespace ParticleButtons.Models
{
    public class ParticleFunctionReturn
    {
        //        {
        //    "id": "54ff71066678574917500867",
        //    "connected": true,
        //    "return_value": 1
        //}
        public string Id { get; set; }

        public bool Connected { get; set; }

        public int Return_value { get; set; }

        public bool Error { get; set; }

        public string ErrorDetail { get; set; }
    }
}
