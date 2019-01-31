using ProERP.Core.Data;
using ProERP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Line
{
    public class LineService
    {

        private readonly IRepository<Data.Models.Line> _lineRepository;
        private readonly IRepository<Data.Models.LineMachineActiveHistory> _lineMachineActiveHistoryRepository;

        public LineService(IRepository<Data.Models.Line> lineRepository, IRepository<Data.Models.LineMachineActiveHistory> lineMachineActiveHistoryRepository)
        {
            this._lineRepository = lineRepository;
            this._lineMachineActiveHistoryRepository = lineMachineActiveHistoryRepository;
        }

        public int Add(Data.Models.Line line)
        {
            _lineRepository.Insert(line);
            return line.Id;
        }

        public int Update(Data.Models.Line line,int userId)
        {
            Data.Models.Line oldLine = _lineRepository.Table.Where(w => w.Id == line.Id).FirstOrDefault();

            if (oldLine != null)
            {
                if (line.IsActive != oldLine.IsActive)
                {
                    this._lineMachineActiveHistoryRepository.Insert(new LineMachineActiveHistory
                    {
                        LineId = line.Id,
                        MachineId = null,
                        IsActive = line.IsActive,
                        UpdateBy = userId,
                        UpdateDate = DateTime.UtcNow,
                    });
                }

                oldLine.Name = line.Name;
                oldLine.Location = line.Location;
                oldLine.InCharge = line.InCharge;
                oldLine.Description = line.Description;
                // oldLine.PlantId = line.PlantId;
                oldLine.IsActive = line.IsActive;
                _lineRepository.Update(oldLine);
                return oldLine.Id;
            }
            else
                return 0;
        }
        public void UpdateIsShutdown(int Id, bool IsShutdown)
        {
            Data.Models.Line oldLine = _lineRepository.Table.Where(w => w.Id == Id).FirstOrDefault();
            if (oldLine != null)
            {
                oldLine.IsShutdown = IsShutdown;
                _lineRepository.Update(oldLine);
            }
        }
        public int[] Delete(int[] ids)
        {
            List<int> lineIds = new List<int>();
            foreach (int id in ids)
            {
                if (_lineRepository.Table.Any(a => a.Machines.Any(b => b.LineId == id)))
                    lineIds.Add(id);
                else
                {
                    Data.Models.Line Line = _lineRepository.Table.FirstOrDefault(w => w.Id == id);
                    _lineRepository.Delete(Line);
                }

            }
            return lineIds.ToArray();
        }
        public Data.Models.Line GetLineById(int lineId)
        {
            return _lineRepository.Table.FirstOrDefault(w => w.Id == lineId);
        }

        public List<Data.Models.Line> GetAll(string Name, int SiteId,int PlantId)
        {
            var allData = this._lineRepository.Table;

            if (!string.IsNullOrEmpty(Name))
                allData = allData.Where(w => w.Name.ToLower().Contains(Name.Trim().ToLower()));

            if (SiteId != 0)
                allData = allData.Where(w => w.Plant.Site.Id == SiteId);

            if (PlantId != 0)
                allData = allData.Where(w => w.Plant.Id == PlantId);

            return allData.ToList();
        }
        public List<Data.Models.Line> GetLinesForPlant(int plantId)
        {
            return this._lineRepository.Table.Where(w => w.PlantId == plantId).OrderBy(w =>w.Name).ToList();
        }
        public Data.Models.Line[] GetDashboardForLine(int PlantId)
        {
            return this._lineRepository.Table.Where(w => w.PlantId == PlantId).OrderBy(w => w.Name).ToArray();
        }
        public string[] GetLineName(int PlantId) 
        {
            return _lineRepository.Table.Where(w => w.PlantId == PlantId).Select(s => s.Name).ToArray();
        }

        public List<Data.Models.Line> GetLineList()
        {
            return this._lineRepository.Table.OrderBy(o => o.Name).ToList();
        }
    }
}
