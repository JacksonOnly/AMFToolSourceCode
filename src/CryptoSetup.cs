using System;
using System.Collections.Generic;

public class CryptoSetup
{
	public int? hwcode;

	public int? dxcc_base;

	public int? gcpu_base;

	public int? da_payload_addr;

	public int? sej_base;

	public Func<int, int, List<int>> read32;

	public Func<int, List<int>, bool> write32;

	public Func<int, byte[], bool> writemem;

	public object blacklist;

	public object cqdma_base;

	public object ap_dma_mem;

	public object meid_addr;

	public object socid_addr;

	public object prov_addr;

	public object efuse_base;
}
