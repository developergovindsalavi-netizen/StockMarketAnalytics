import { useEffect, useState } from 'react'
import {
  CartesianGrid,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts'

import { getHistoricalPrices } from '../services/historicalPriceService'
import type { HistoricalPrice } from '../types/historicalPrice'

interface StockPriceChartProps {
  instrumentId: number
  symbol: string
}

function StockPriceChart({
  instrumentId,
  symbol,
}: StockPriceChartProps) {
  const [prices, setPrices] = useState<HistoricalPrice[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false
    const controller = new AbortController();

    async function loadPrices() {
      try {
        setLoading(true)
        setError(null)

        const data = await getHistoricalPrices(instrumentId, controller.signal)

        if (!cancelled) {
          setPrices(data)
        }
      } catch (err) {
        if (err instanceof Error && err.name === 'AbortError') {
          return
        }

        if (!cancelled) {
          setError(
            err instanceof Error
              ? err.message
              : 'Failed to load historical prices.',
          )
        }
      } finally {
        if (!controller.signal.aborted) {
          setLoading(false);
        }
      }

      
    }

    void loadPrices()

    return () => {
        cancelled = true
        controller.abort()
    }
  }, [instrumentId])

  const chartData = prices.map((price) => ({
    ...price,
    date: new Date(price.candleTimeUtc).toLocaleDateString(),
  }))

  if (loading) {
    return <p>Loading price chart...</p>
  }

  if (error) {
    return <p className="error-message">Error: {error}</p>
  }

  if (prices.length === 0) {
    return <p>No historical prices available for {symbol}.</p>
  }

  return (
    <section className="price-chart-section">
      <h2>{symbol} Closing Price</h2>
      <p className="sample-data-note">
        Sample data for development — not live market prices.
      </p>

      <div className="chart-container">
        <ResponsiveContainer width="100%" height="100%">
          <LineChart data={chartData}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="date" />
            <YAxis
              domain={['auto', 'auto']}
              width={75}
            />
            <Tooltip
              formatter={(value) => [
                Number(value).toFixed(2),
                'Closing Price',
              ]}
            />
            <Line
              type="monotone"
              dataKey="close"
              name="Closing Price"
              stroke="#2563eb"
              strokeWidth={2}
              dot={{ r: 4 }}
              activeDot={{ r: 6 }}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>
    </section>
  )
}

export default StockPriceChart