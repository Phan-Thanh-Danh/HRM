using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Admin
{
    public interface IAssetService
    {
        Task<List<AssetVM>> GetAllAssetsAsync();
        Task CreateAssetAsync(AssetVM assetVM);
        Task AssignAssetAsync(int assetId, int employeeId, string? notes);
        Task ReturnAssetAsync(int assetId);
    }

    public class AssetService : IAssetService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AssetService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AssignAssetAsync(int assetId, int employeeId, string? notes)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset != null && asset.Status == AssetStatus.Available)
            {
                asset.CurrentHolderId = employeeId;
                asset.Status = AssetStatus.InUse;

                var allocation = new AssetAllocation
                {
                    AssetId = assetId,
                    EmployeeId = employeeId,
                    AssignedDate = DateTime.UtcNow,
                    Notes = notes
                };
                _context.AssetAllocations.Add(allocation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateAssetAsync(AssetVM assetVM)
        {
            var asset = _mapper.Map<Asset>(assetVM);
            asset.Status = AssetStatus.Available; // Default
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AssetVM>> GetAllAssetsAsync()
        {
            var assets = await _context.Assets
                .ToListAsync();
            // Manual check for Holder Name if needed or use separate query
            // Simplification: AutoMapper maps ID/Status
            
            return _mapper.Map<List<AssetVM>>(assets);
        }

        public async Task ReturnAssetAsync(int assetId)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset != null && asset.Status == AssetStatus.InUse)
            {
                // Verify allocation record to close
                var allocation = await _context.AssetAllocations
                    .Where(a => a.AssetId == assetId && a.ReturnedDate == null)
                    .OrderByDescending(a => a.AssignedDate)
                    .FirstOrDefaultAsync();

                if (allocation != null)
                {
                    allocation.ReturnedDate = DateTime.UtcNow;
                }

                asset.CurrentHolderId = null;
                asset.Status = AssetStatus.Available;
                await _context.SaveChangesAsync();
            }
        }
    }
}
