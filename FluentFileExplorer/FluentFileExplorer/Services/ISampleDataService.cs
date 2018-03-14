using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using FluentFileExplorer.Models;

namespace FluentFileExplorer.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetSampleModelDataAsync();
    }
}
