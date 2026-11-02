
import { useState, useEffect } from 'react';
import './App.css'
import { getInstruments } from './services/instrumentService';
import type { Instrument } from './types/instrument';
import StockPriceChart from './components/StockPriceChart'

function App() {

  const [pageNumber, setPageNumber] = useState(1)
  const [totalCount, setTotalCount] = useState(0)
  const [totalPages, setTotalPages] = useState(0)
  const [pageSize] = useState(5)

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [instruments, setInstruments] = useState<Instrument[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedExchange, setSelectedExchange] = useState('ALL');
  const [debouncedSearch, setDebouncedSearch] = useState(searchTerm);
  const [selectedInstrument, setSelectedInstrument] = useState<Instrument | null>(null);


  useEffect(() => {
    const timerId = setTimeout(() => {
      setDebouncedSearch(searchTerm)
      setPageNumber(1)
    }, 400)

    return () => clearTimeout(timerId)
  }, [searchTerm])

  useEffect(() => {
    async function loadInstruments() {
      try {
        setLoading(true);
        setError(null);

        const result = await getInstruments(
          pageNumber,
          pageSize,
          debouncedSearch,
          selectedExchange === 'ALL' ? '' : selectedExchange,
        )

        setInstruments(result.items)
        setTotalCount(result.totalCount)
        setTotalPages(result.totalPages)
      }
      catch (err) {
        setError(
          err instanceof Error
            ? err.message
            : 'Something went wrong while loading instruments.',
        )
      }
      finally {
        setLoading(false);
      }
    }

    void loadInstruments();
  }, [pageNumber, pageSize, debouncedSearch, selectedExchange]);



  const filteredInstruments = instruments
    .filter((instrument) => {
      const matchesSearch = instrument.symbol
        .toLowerCase()
        .includes(searchTerm.toLowerCase()) ||
        instrument.companyName
          .toLowerCase()
          .includes(searchTerm.toLowerCase())

      const matchesExchange =
        selectedExchange === 'ALL' ||
        instrument.exchange === selectedExchange

      return matchesSearch && matchesExchange
    });

  const handleInstrumentClick = (id: number) => {

    const instrument = instruments.find(
      s => s.instrumentId === id
    );

    if (instrument) {
      setSelectedInstrument(instrument);
    }
  }

  return (
    <div className="app">
      <header className="app-header">
        <h1>MarketPulse</h1>
        <p>Stock Market Analytics & Research Platform</p>
      </header>

      <main className="dashboard">
        <section className="welcome-card">
          <h2>Welcome to MarketPulse</h2>
          <p>
            Track stocks, analyze market trends, and manage your watchlist
            from one dashboard.
          </p>
        </section>

        {(selectedInstrument) && <StockPriceChart
          instrumentId={selectedInstrument.instrumentId}
          symbol={selectedInstrument.symbol}
        />}


        <section className="instruments-section">
          <h2>Stock Instruments</h2>

          <div className="instrument-filters">
            <input
              type="text"
              placeholder="Search by symbol or company..."
              value={searchTerm}
              onChange={(event) => {
                setSearchTerm(event.target.value)
                setPageNumber(1)
              }}
            />

            <select
              value={selectedExchange}
              onChange={(event) => {
                setSelectedExchange(event.target.value)
                setPageNumber(1)
              }}
            >
              <option value="ALL">All Exchanges</option>
              <option value="NSE">NSE</option>
              <option value="BSE">BSE</option>
            </select>
          </div>
          <p className="results-count">
            Showing {instruments.length} records on this page
            {' '}of {totalCount} total instruments
          </p>
          {loading && <p>Loading instruments...</p>}

          {error && <p className="error-message">Error: {error}</p>}

          {!loading && !error && filteredInstruments.length === 0 && (
            <p className="empty-message">
              {instruments.length === 0
                ? 'No instruments available.'
                : 'No instruments match your search.'}
            </p>
          )}

          {!loading && !error && filteredInstruments.length > 0 && (
            <>
              <div className="table-container">
                <table>
                  <thead>
                    <tr>
                      <th>Symbol</th>
                      <th>Company</th>
                      <th>Exchange</th>
                      <th>Status</th>
                    </tr>
                  </thead>

                  <tbody>
                    {filteredInstruments.map((instrument) => (
                      <tr key={instrument.instrumentId}
                        onClick={() => handleInstrumentClick(instrument.instrumentId)}
                        className={
                          selectedInstrument?.instrumentId === instrument.instrumentId
                            ? 'selected-row'
                            : ''
                        }
                      >
                        <td>{instrument.symbol}</td>
                        <td>{instrument.companyName}</td>
                        <td>{instrument.exchange}</td>
                        <td>
                          {instrument.isActive ? 'Active' : 'Inactive'}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <div className="pagination">
                <button
                  type="button"
                  onClick={() => setPageNumber((current) => current - 1)}
                  disabled={pageNumber <= 1 || loading}
                >
                  Previous
                </button>

                <span>
                  Page {pageNumber} of {totalPages}
                </span>

                <button
                  type="button"
                  onClick={() => setPageNumber((current) => current + 1)}
                  disabled={pageNumber >= totalPages || loading}
                >
                  Next
                </button>
              </div></>
          )}
        </section>
      </main>
    </div>
  )
}

export default App
