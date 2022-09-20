import { useEffect, useState } from 'react';
import {
    getAllMarketTypes,
    getAllInstrumentsByMarketType,
} from '../../../services/markets/marketsService';
import styles from './MarketsFiltrator.module.css';

export const MarketsFiltrator = ({ instrument, onInstrumentChange }) => {

    const [state, setState] = useState({
        markets: [],
        marketSelected: 'stock',
        instruments: [],
        instrumentSelected: instrument
    });

    useEffect(() => {
        (async () => {
            let responseMarkets = await getAllMarketTypes();
            let responseInstruments = await getAllInstrumentsByMarketType(state.marketSelected);

            setState({
                markets: responseMarkets,
                instruments: responseInstruments,
                marketSelected: state.marketSelected,
                instrumentSelected: state.instrumentSelected
            });
        })();
        // eslint-disable-next-line
    }, []);

    const OnChangeMarketType = (marketTypeSelected) => {
        getAllInstrumentsByMarketType(marketTypeSelected)
            .then(responseInstruments => {
                setState({
                    markets: state.markets,
                    instruments: [...responseInstruments],
                    marketSelected: marketTypeSelected,
                    instrumentSelected: state.instrumentSelected
                });
            });
    }

    const OnInstrumentListChangeSetInstrument = (instrument) => {
        setState({
            markets: state.markets,
            instruments: state.instruments,
            marketSelected: state.marketSelected,
            instrumentSelected: instrument
        });
        onInstrumentChange(instrument);
    }

    return (
        <section className={styles.searchSection}>
            <div className={styles.searchMarketTypePart}>
                <h1 className={styles.chooseMarketTypeHeading}>Choose Market</h1>
                <select className={styles.searchDropdown} onChange={(e) => OnChangeMarketType(e.target.value)} value={state.marketSelected}>
                    {state.markets.map(marketType => <option key={marketType.guid} value={marketType.name}>{marketType.name}</option>)}
                </select>
            </div>
            <div className={styles.searchInstrumentPart}>
                <h1 className={styles.chooseInstrumentHeading}>Choose Instrument</h1>
                <select className={styles.searchDropdown} onChange={(e) => OnInstrumentListChangeSetInstrument(e.target.value)} value={state.instrumentSelected.tickerSymbol}>
                    {state.instruments.map(instr => <option key={instr.guid} value={instr.tickerSymbol}>{instr.tickerSymbol}</option>)}
                </select>
            </div>
        </section>
    );
}