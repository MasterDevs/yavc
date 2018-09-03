using yavc.Base.Data;

namespace yavc.Base {
	public interface ITileService {
		void CreateOrUpdateTile(Device d);
		void CreateOrUpdateTile(Device device, Zone TheZone);
		void DeleteAllTilesForDevice(Device device);
		void UpdateTileIfExists(Device d);
	}
}
