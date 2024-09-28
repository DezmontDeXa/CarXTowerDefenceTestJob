namespace TowerDefence.Towers
{
	public interface ICannonTowerData : ITowerData
	{
		float PredictPrecision { get; }
		float RotationSpeed { get; }
		BalisticsCalculations.Trajectory Trajectory { get; }
	}
}