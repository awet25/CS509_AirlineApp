import { useState } from 'react';
import axios from 'axios';
import type { components } from '../types/api';
import FlightList from './FlightList';
import './FlightSearch.css';

type CombinedFlightDto = components['schemas']['CombinedFlightDto'];

function FlightSearch() {
  const [departAirport, setDepartAirport] = useState('');
  const [arriveAirport, setArriveAirport] = useState('');
  const [departureDate, setDepartureDate] = useState('');
  const [returnDate, setReturnDate] = useState('');

  const [departingFlights, setDepartingFlights] = useState<CombinedFlightDto[]>([]);
  const [connectingDepartFlights, setConnectingDepartFlights] = useState<CombinedFlightDto[]>([]);
  const [returningFlights, setReturningFlights] = useState<CombinedFlightDto[]>([]);
  const [connectingReturnFlights, setConnectingReturnFlights] = useState<CombinedFlightDto[]>([]);

  const [noRoundTrip, setNoRoundTrip] = useState(false);

  const handleSearch = async () => {
    try {
      const response = await axios.get('http://localhost:5218/api/v1/CombinedFlights/search', {
        params: {
          departAirport,
          arriveAirport,
          departureDate,
          returnDate,
        },
      });

      const { value } = response.data;

      const allDeparting = value?.directDepartFlights?.$values ?? [];
      const allConnectingDepart = value?.connectingDepartFlights?.$values ?? [];
      const allReturning = value?.directReturnFlights?.$values ?? [];
      const allConnectingReturn = value?.connectingReturnFlights?.$values ?? [];

      setDepartingFlights(allDeparting);
      setConnectingDepartFlights(allConnectingDepart);

      if (returnDate) {
        const hasReturns = allReturning.length > 0 || allConnectingReturn.length > 0;
        setNoRoundTrip(!hasReturns);
        setReturningFlights(allReturning);
        setConnectingReturnFlights(allConnectingReturn);
      } else {
        setNoRoundTrip(false);
        setReturningFlights([]);
        setConnectingReturnFlights([]);
      }
    } catch (error) {
      console.error('Error fetching flight data:', error);
    }
  };

  return (
    <div>
      <h1 style={{ textAlign: 'center' }}>Flight Search</h1>
      <form onSubmit={(e) => { e.preventDefault(); handleSearch(); }}>
        <div>
          <label>Depart Airport:</label>
          <input type="text" value={departAirport} onChange={(e) => setDepartAirport(e.target.value)} />
        </div>
        <div>
          <label>Arrive Airport:</label>
          <input type="text" value={arriveAirport} onChange={(e) => setArriveAirport(e.target.value)} />
        </div>
        <div>
          <label>Departure Date:</label>
          <input type="date" value={departureDate} onChange={(e) => setDepartureDate(e.target.value)} />
        </div>
        <div>
          <label>Return Date:</label>
          <input type="date" value={returnDate} onChange={(e) => setReturnDate(e.target.value)} />
        </div>
        <div style={{ textAlign: 'center' }}>
          <button type="submit">Search</button>
        </div>
      </form>

      <br />

      {noRoundTrip ? (
        <div style={{ textAlign: 'center' }}>No round trip flights found</div>
      ) : (
        <div className={`flight-lists-container ${returningFlights.length > 0 || connectingReturnFlights.length > 0 ? '' : 'single-column'}`}>
          <FlightList title="Direct Departing Flights" flights={departingFlights} />
          <FlightList title="Connecting Departing Flights" flights={connectingDepartFlights} />
          {returningFlights.length > 0 && <FlightList title="Direct Returning Flights" flights={returningFlights} />}
          {connectingReturnFlights.length > 0 && <FlightList title="Connecting Returning Flights" flights={connectingReturnFlights} />}
        </div>
      )}
    </div>
  );
}

export default FlightSearch;
