using System;
using Consensus;
using BlockChain.Store;
using Infrastructure;
using System.Collections.Generic;
using System.Linq;
using BlockChain.Data;
using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace BlockChain
{
	public class BlockChainTestsBase
	{
		const string DB = "temp";
		byte[] _GenesisBlockHash;
		IDisposable _TxMessagesListenerScope;
		HashDictionary<TxStateEnum> _TxStates = new HashDictionary<TxStateEnum>();

		protected BlockChain _BlockChain;
		protected Types.Block _GenesisBlock;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Dispose();

			_TxMessagesListenerScope = MessageProducer<BlockChainMessage>.Instance.AddMessageListener(
				new MessageListener<BlockChainMessage>(OnBlockChainMessage));

			_GenesisBlock = Utils.GetGenesisBlock();
			_GenesisBlockHash = Merkle.blockHeaderHasher.Invoke(_GenesisBlock.header);
			_BlockChain = new BlockChain(DB, _GenesisBlockHash);
		}

		[OneTimeTearDown]
		public void Dispose()
		{
			if (_TxMessagesListenerScope != null)
			{
				_TxMessagesListenerScope.Dispose();
			}
			if (_BlockChain != null)
			{
				_BlockChain.Dispose();
			}
			if (Directory.Exists(DB))
			{
				Directory.Delete(DB, true);
			}
		}

		void OnBlockChainMessage(BlockChainMessage m)
		{
			if (m is TxMessage)
			{
				var txMessage = (TxMessage)m;
				_TxStates[txMessage.TxHash] = ((TxMessage)m).State;
			}
			else if (m is BlockMessage)
			{
				foreach (var item in ((BlockMessage)m).PointedTransactions)
				{
					_TxStates[item.Key] = TxStateEnum.Confirmed;
				}
			}
		}

		protected LocationEnum Location(Types.Block block)
		{
			using (var dbTx = _BlockChain.GetDBTransaction())
			{
				var key = Merkle.blockHeaderHasher.Invoke(block.header);
				return _BlockChain.BlockStore.GetLocation(dbTx, key);
			}
		}

		protected TxStateEnum? TxState(Types.Transaction tx)
		{
			var key = Merkle.transactionHasher.Invoke(tx);
			if (_TxStates.ContainsKey(key)) return _TxStates[key];
			return null;
		}

		protected bool CheckUTXOCOntains(Types.Output output)
		{
			HashDictionary<List<Types.Output>> txOutputs;
			HashDictionary<Types.Transaction> txs;
			_BlockChain.GetUTXOSet(null, out txOutputs, out txs);
			           
			foreach (var item in txOutputs)
			{
				if (item.Value.Contains(output))
				{
					return true;
				}
			}

			return false;
		}
	}
}
