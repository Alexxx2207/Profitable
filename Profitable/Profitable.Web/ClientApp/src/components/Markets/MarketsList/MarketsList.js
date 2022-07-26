import { useEffect, useState } from 'react';
import { MarketWidget } from '../MarketWidget/MarketWidget';
import { getAllInstruments, getInstrument } from'../../../services/markets/marketsService';

export const MarketsList = () => {
    const requiredInstrument = {
        exchangeName: 'NASDAQ',
        tickerSymbol: 'AAPL'
    };

    const [instrument, setInstrument] = useState({ ...requiredInstrument });
    const [allInstruments, setAllInstruments] = useState([]);

    const fetchInstrumentData = (instrument) => {
        getInstrument(instrument)
            .then(responseInstrument => {
                setInstrument(responseInstrument);
            })
    }

    useEffect(() => {
        getAllInstruments()
            .then(responseInstrument => {
                setAllInstruments(responseInstrument);
            })
    }, []);

    return (
        <div style={{ height: '81vh' }}>
            <section className="search">
                <select onChange={(e) => fetchInstrumentData(e.target.value)} value={instrument.tickerSymbol}>
                    {allInstruments.map(instr => <option key={instr.guid} value={instr.tickerSymbol}>{instr.tickerSymbol}</option>)}
                </select>
            </section>
            <MarketWidget instrument={{ ...instrument }} />
        </div>
    );
}