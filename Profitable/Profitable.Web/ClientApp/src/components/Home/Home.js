import { Watchlist } from '../Watchlist/Watchlist';
import { useEffect, useState } from 'react';

export const Home = () => {
    const requiredInstrument = {
        exchangeName: 'CAPITALCOM',
        tickerSymbol: 'UK100'
    };

    const [instrument, setInstrument] = useState({ ...requiredInstrument });

    useEffect(() => {
        fetch(`${process.env.REACT_APP_API_BASE_URL}/api/markets/bgnusd`)
        .then(res => res.json())
        .then(responseInstrument => {
            setTimeout(() => {
                setInstrument(responseInstrument);
            }, 4000)
        })
    }, []);

    const marketWidgetStyle = {
        height: "800px"
    }

    return <div>
        <main style={marketWidgetStyle}>
            <Watchlist style={marketWidgetStyle} instrument={{ ...instrument }} />
        </main>
        
    </div>;
}