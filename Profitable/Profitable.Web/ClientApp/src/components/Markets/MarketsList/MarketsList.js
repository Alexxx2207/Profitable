import { useEffect, useState } from 'react';
import { MarketWidget } from '../MarketWidget/MarketWidget';
import { getAllInstruments, getInstrument } from '../../../services/markets/marketsService';
import styles from './MarketsList.module.css';

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
        <div className={styles.marketsList}>
            <section className={styles.searchSection}>
                <div className={styles.searchInstrumentPart}>
                    <h1 className={styles.chooseInstrumentHeading}>Instrument</h1>
                    <select className={styles.searchDropdown} onChange={(e) => fetchInstrumentData(e.target.value)} value={instrument.tickerSymbol}>
                        {allInstruments.map(instr => <option key={instr.guid} value={instr.tickerSymbol}>{instr.tickerSymbol}</option>)}
                    </select>
                </div>
            </section>
            <MarketWidget instrument={{ ...instrument }} />
        </div>
    );
}