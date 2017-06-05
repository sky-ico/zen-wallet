﻿using System;
namespace ContractsDiscovery.Web.App_Data
{
    public class ActiveContract
    {
		public string Address { get; set; }
		public string AddressUrl { get; set; }
		public UInt32 LastBlock { get; set; }
		public string Code { get; set; }
		public string AuthorMessage { get; set; }
		public string Expiry { get; set; }
		public string Strike { get; set; }
		public string Oracle { get; set; }
		public string Underlying { get; set; }
		public string Type { get; set; }
        public UInt64 TotalAssets { get; set; }

		public string AssetName { get; set; }
		public string AssetImageUrl { get; set; }
		public string AssetMetadataVersion { get; set; }
	}
}
