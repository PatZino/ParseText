using System;
using CsvHelper.Configuration.Attributes;

namespace ParseText
{
	public class ColumnName
	{
        [Index(0)]
        public string Action { get; set; }

        [Index(1)]
        public string Status { get; set; }

        [Index(2)]
        public string Code { get; set; }

        [Index(3)]
        public string ShortCode { get; set; }

        [Index(4)]
        public string LongCode { get; set; }
    }
}
