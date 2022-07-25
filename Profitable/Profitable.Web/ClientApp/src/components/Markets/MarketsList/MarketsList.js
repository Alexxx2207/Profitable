import { useEffect, useState } from 'react';
import { MarketWidget } from '../MarketWidget/MarketWidget';

export const MarketsList = () => {
    const requiredInstrument = {
        exchangeName: 'NASDAQ',
        tickerSymbol: 'AAPL'
    };

    const [instrument, setInstrument] = useState({ ...requiredInstrument });
    const [allInstruments, setAllInstruments] = useState([]);

    const fetchInstrumentData = (instrument) => {
        fetch(`${process.env.REACT_APP_API_BASE_URL}/api/markets/${instrument}`)
        .then(response => response.json())
        .then(responseInstrument => {
            setInstrument(responseInstrument);
        })
    }
    
    useEffect(() => {
        fetchInstrumentData('AAPL');
    }, []);
  
    useEffect(() => {
        fetch(`${process.env.REACT_APP_API_BASE_URL}/api/markets/`)
        .then(res => res.json())
        .then(responseInstrument => {
            setAllInstruments(responseInstrument);
        })
    }, []);
    
    return (
        <div style={{height: '81vh'}}>
            <section className="search">
                <select onChange={(e) => fetchInstrumentData(e.target.value)} value={instrument.tickerSymbol}>
                    {allInstruments.map(instr => <option key={instr.guid} value={instr.tickerSymbol}>{instr.tickerSymbol}</option>)}
                </select>
            </section>
            <MarketWidget  instrument={{...instrument}}/>
        </div>
    );
}