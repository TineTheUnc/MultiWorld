using Terraria.GameContent.UI.Elements;
namespace MultiWorld.Common.UI
{
	public class UINumberBox(int min, int max, int start,string message) : UITextPanel<string>(message + start.ToString())
	{
		public int Number = start;
		public int Min = min;
		public int Max = max;
		public string Message = message;
		public delegate void NumberEvent(UINumberBox self);
		public event NumberEvent OnAdd;
		public event NumberEvent OnReduce;

		public override void OnInitialize()
		{
			base.OnInitialize();
			this.OnLeftClick += (evt, element) =>
			{
				Number++;
				if (Number > Max)
					Number = Max;
				OnAdd(this);
				SetText(Message + Number.ToString());
			};
			this.OnRightClick += (evt, element) =>
			{
				Number--;
				if (Number < Min)
					Number = Min;
				OnReduce(this);
				SetText(Message + Number.ToString());			
			};
		}
	}
}
