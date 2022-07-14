import { MarketWidget } from '../Markets/MarketWidget';
import { useEffect, useState } from 'react';

export const Home = () => {
    const requiredInstrument = {
        exchangeName: 'CAPITALCOM',
        tickerSymbol: 'UK100'
    };

    const [instrument, setInstrument] = useState({ ...requiredInstrument });

    const marketWidgetStyle = {
        height: "700px"
    };


    useEffect(() => {
        fetch('https://localhost:7048/api/markets/bgnusd')
        .then(res => res.json())
        .then(responseInstrument => {
            setTimeout(() => {
                setInstrument(responseInstrument);
            }, 4000)
        })
    }, []);

    return <div>
        <main style={marketWidgetStyle}>
            <MarketWidget style={marketWidgetStyle} instrument={{ ...instrument }} />
        </main>
        
    </div>;
}