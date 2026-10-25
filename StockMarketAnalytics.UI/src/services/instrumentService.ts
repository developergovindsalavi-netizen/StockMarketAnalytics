
import type { Instrument } from '../types/instrument'
import type { PagedResponse } from '../types/pagedResponse'

const API_BASE_URL = 'http://localhost:5156/api'

export async function getInstruments(
  pageNumber = 1,
  pageSize = 10,
  search = '',
  exchange = '',
): Promise<PagedResponse<Instrument>> {
  const params = new URLSearchParams({
    pageNumber: String(pageNumber),
    pageSize: String(pageSize),
  })

  if (search.trim()) {
    params.set('search', search.trim())
  }

  if (exchange) {
    params.set('exchange', exchange)
  }

  const response = await fetch(
    `${API_BASE_URL}/instruments?${params.toString()}`,
  )

  if (!response.ok) {
    throw new Error(`Failed to fetch instruments: ${response.status}`)
  }

  return (await response.json()) as PagedResponse<Instrument>
}
