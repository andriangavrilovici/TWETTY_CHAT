using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWETTY_CHAT.Core
{
    public class MessageApiModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SendBy_Email { get; set; }

        public string SendTo_Email { get; set; }

        public string Message { get; set; }

        public DateTimeOffset MessageSentTime { get; set; }
    }
}
