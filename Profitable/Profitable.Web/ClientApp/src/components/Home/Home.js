import { WatchList } from '../Watchlist/Watchlist';
import { MarketWidget } from '../Markets/MarketWidget';
import { useEffect, useState } from 'react';

export const Home = () => {
    const requiredInstrument = {
        exchange: 'CAPITALCOM',
        ticker: 'UK100'
    };

    const [instrument, setInstrument] = useState({ ...requiredInstrument });

    const marketWidgetStyle = {
        height: "700px"
    };
    const watchListStyle = {
        height: "800px"
    };

    useEffect(() => {
        fetch('')
        .then(res => res.json())
        .then(responseInstrument => setInstrument(responseInstrument))
    }, []);

    return <div>
        <main style={marketWidgetStyle}>
            <MarketWidget style={marketWidgetStyle} instrument={{ ...instrument }} />
        </main>
        <aside style={watchListStyle}>
            <WatchList style={watchListStyle} />
        </aside>
    </div>;
}