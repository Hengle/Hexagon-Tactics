using UnityEngine;
using System.Collections;

public class FullTypeHitRate : HitRate 
{
	public override bool IsAngleBased { get { return false; }}

	public override int Calculate (HexCell target)
	{
		Unit defender = target.Content.GetComponent<Unit>();
		if (AutomaticMiss(defender))
			return Final(100);

		return Final (0);
	}
}