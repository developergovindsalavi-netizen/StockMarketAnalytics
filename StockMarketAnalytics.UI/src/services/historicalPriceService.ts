import type { HistoricalPrice } from '../types/historicalPrice'

const API_BASE_URL = 'http://localhost:5156/api'

export async function getHistoricalPrices(
  instrumentId: number, 
  signal?: AbortSignal
): Promise<HistoricalPrice[]> {
  const response = await fetch(
    `${API_BASE_URL}/instruments/${instrumentId}/historical-prices`,
     { signal }
  )

  if (!response.ok) {
    throw new Error(
      `Failed to fetch historical prices: ${response.status}`,
    )
  }

  return (await response.json()) as HistoricalPrice[]
}