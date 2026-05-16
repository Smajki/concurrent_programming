using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public sealed class NullDiagnosticLogger : IDiagnosticLogger
    {
        public ValueTask logAsync(string ascii_line) => ValueTask.CompletedTask;

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}