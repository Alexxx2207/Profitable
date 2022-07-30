import { useEffect, useState } from 'react';
import { MarketWidget } from '../MarketWidget/MarketWidget';
import {
    getInstrument,
    getAllMarketTypes,
    getAllInstrumentsByMarketType
} from '../../../services/markets/marketsService';
import styles from './MarketsList.module.css';

export const MarketsList = () => {
    
    const initialMarketType = 'stock';

    const [instrument, setInstrument] = useState({
        tikcerSymbol: '',
        exchangeName: ''
});
    const [marketType, setMarketType] = useState({
        name: initialMarketType
    });
    const [allInstruments, setAllInstruments] = useState([]);
    const [allMarketTypes, setAllMarketTypes] = useState([]);

    useEffect(() => {
        getAllMarketTypes()
        .then(responseMarketTypes => {
            setAllMarketTypes(responseMarketTypes);
        });
    }, []);

    useEffect(() => {
        getAllInstrumentsByMarketType(marketType.name)
            .then(responseInstruments => {
                setAllInstruments(responseInstruments);
            });
    }, [allMarketTypes]);

    useEffect(() => {
        getAllInstrumentsByMarketType(marketType.name)
            .then(responseInstruments => {
                setAllInstruments(responseInstruments);
            });
    }, [marketType]);

    useEffect(() => {
        if(allInstruments.length > 0) {
            OnChangeSetInstrument(allInstruments[0].tickerSymbol)
        }
    }, [allInstruments]);

    const OnChangeSetInstrument = (instrument) => {
        getInstrument(instrument)
            .then(responseInstrument => {
                setInstrument(responseInstrument);
            })
    }

    const OnChangeMarketType = (marketTypeSelected) => {
        setMarketType({ name: marketTypeSelected });
    }

    return (
        <div className={styles.marketsList}>
            <section className={styles.searchSection}>
                <div className={styles.searchMarketTypePart}>
                    <h1 className={styles.chooseMarketTypeHeading}>Market</h1>
                    <select className={styles.searchDropdown} onChange={(e) => OnChangeMarketType(e.target.value)} value={marketType.name}>
                        {allMarketTypes.map(marketType => <option key={marketType.guid} value={marketType.name}>{marketType.name}</option>)}
                    </select>
                </div>
                <div className={styles.searchInstrumentPart}>
                    <h1 className={styles.chooseInstrumentHeading}>Instrument</h1>
                    <select className={styles.searchDropdown} onChange={(e) => OnChangeSetInstrument(e.target.value)} value={instrument.tickerSymbol}>
                        {allInstruments.map(instr => <option key={instr.guid} value={instr.tickerSymbol}>{instr.tickerSymbol}</option>)}
                    </select>
                </div>
            </section>
            <MarketWidget instrument={{ ...instrument }} />
        </div>
    );
}