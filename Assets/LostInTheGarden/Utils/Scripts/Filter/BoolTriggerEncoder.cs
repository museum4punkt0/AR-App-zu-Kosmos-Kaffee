namespace LostInTheGarden.Utils.Filter
{
	public class BoolTriggerEncoder
	{
		private bool state = false;

		// encodes a trigger signal into a a contiuous stream
		// each trigger flips the output signal between true/false
		// input:  ______|___________|____|_______
		// output: ______¯¯¯¯¯¯¯¯¯¯¯¯_____¯¯¯¯¯¯¯¯
		public bool Encode(bool triggerSignal)
		{
			if (triggerSignal)
			{
				state = !state;
			}

			return state;
		}
	}
}
