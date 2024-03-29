﻿namespace Profitable.Models.EntityModels
{
	using Profitable.Common.Enums;
	using Profitable.Models.EntityModels.EntityBaseClass;
	using System.ComponentModel.DataAnnotations.Schema;

	public class FuturesPosition : EntityBase
	{
		public PositionDirection Direction { get; set; }

		[ForeignKey("TradePosition")]
		public Guid TradePositionId { get; set; }

		public TradePosition TradePosition { get; set; }

		[ForeignKey("FuturesContract")]
		public Guid FuturesContractId { get; set; }

		public FuturesContract FuturesContract { get; set; }

	}
}