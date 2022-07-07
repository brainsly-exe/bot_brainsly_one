namespace bot_brainsly_one.src.models
{
    public class Task_Payload
    {
        public string id_contaig { get; set; }
        public string id_contayt { get; set; }
        public string id_contakwai { get; set; }
        public string id_contafb { get; set; }
        public string id_contatt { get; set; }
        public string ps { get; set; }
        public string ar { get; set; }
        public string pr { get; set; }
        public string idp { get; set; }
        public string idep { get; set; }
        public string vsys { get; set; }
    }

    public class Task_Received
    {
        public string html { get; set; }
        public string idep { get; set; }
        public string idp { get; set; }
        public string status { get; set; }
        public int? tempo_acao { get; set; }
    }
}
