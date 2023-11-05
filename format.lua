-- Suformatuoja CSV failo eilutę

done = function(summary, latency, requests)
   io.stderr:write(string.format("%d;%d;%f;%d;%f", (summary.requests/summary.duration)*1e6, latency.min, latency.mean, latency.max, latency.stdev))
end
