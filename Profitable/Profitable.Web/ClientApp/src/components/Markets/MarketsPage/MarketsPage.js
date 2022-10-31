import { useState } from "react";
import { MarketWidget } from "../MarketWidget/MarketWidget";
import { MarketsFiltrator } from "../MarketsFiltrator/MarketsFiltrator";
import styles from "./MarketsPage.module.css";
import { getInstrument } from "../../../services/markets/marketsService";

export const MarketsPage = () => {
    const [instrument, setInstrument] = useState({
        tickerSymbol: "AAPL",
        exchangeName: "NASDAQ",
    });

    const onInstrumentChange = (instrument) => {
        getInstrument(instrument).then((result) => {
            setInstrument({
                tickerSymbol: result.tickerSymbol,
                exchangeName: result.exchangeName,
            });
        });
    };

    return (
        <div className={styles.marketsList}>
            <MarketsFiltrator
                instrument={{ ...instrument }}
                onInstrumentChange={onInstrumentChange}
            />
            <MarketWidget instrument={{ ...instrument }} />
        </div>
    );
};
