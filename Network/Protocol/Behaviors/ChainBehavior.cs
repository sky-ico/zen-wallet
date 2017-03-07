﻿using System.Threading;
using Consensus;
using System.Linq;
using System;

namespace NBitcoin.Protocol.Behaviors
{
	public class ChainBehavior : NodeBehavior
	{
		private Timer _Refresh;
		private BlockChain.BlockChain _BlockChain;
		private bool IsTipOld;

		public ChainBehavior(BlockChain.BlockChain blockChain)
		{
			_BlockChain = blockChain;
			IsTipOld = _BlockChain.IsTipOld;
		}

		protected override void AttachCore()
		{
			AttachedNode.StateChanged += AttachedNode_StateChanged;
			AttachedNode.MessageReceived += AttachedNode_MessageReceived;
		}

		protected override void DetachCore()
		{
			AttachedNode.StateChanged -= AttachedNode_StateChanged;
			AttachedNode.MessageReceived -= AttachedNode_MessageReceived;
		}


		void AttachedNode_StateChanged(Node node, NodeState oldState)
		{
			//TODO: to check: don't send to self

			if (node.State == NodeState.HandShaked && IsTipOld)
			{
				AttachedNode.SendMessageAsync(new GetTipPayload());
			}
		}


		void AttachedNode_MessageReceived(Node node, IncomingMessage message)
		{
			message.IfPayloadIs<Types.Block>(bk =>
			{
				if (_BlockChain.HandleBlock(bk))
				{
					node.SendMessageAsync(new GetDataPayload(new InventoryVector[] {
						new InventoryVector(InventoryType.MSG_BLOCK, bk.header.parent)
					}));
				}
				else
				{
					node.SendMessageAsync(new RejectPayload()
					{
						Hash = Consensus.Merkle.blockHeaderHasher.Invoke(bk.header),
						Code = RejectCode.INVALID,
						Message = "bk"
					});
				}
			});

			message.IfPayloadIs<GetTipPayload>(getTip =>
			{
				var tip = _BlockChain.Tip;

				if (tip != null)
				{
					NodeServerTrace.Information("Sending tip: " + System.Convert.ToBase64String(tip.Key));
					node.SendMessageAsync(_BlockChain.GetBlock(tip.Key));
				}
				else 
				{
					NodeServerTrace.Information("No tip to send");
				}
			});

			message.IfPayloadIs<GetDataPayload>(getData =>
			{
				foreach (var inventory in getData.Inventory.Where(i => i.Type == InventoryType.MSG_BLOCK))
				{
					var bk = _BlockChain.GetBlock(inventory.Hash);

					if (bk != null)
					{
						NodeServerTrace.Information("Sending block: " + bk.header.blockNumber);
						node.SendMessageAsync(bk);
					}
				}
			});
		}

		#region ICloneable Members

		public override object Clone()
		{
			var behavior = new ChainBehavior(_BlockChain);
			return behavior;
		}

		#endregion
	}
}