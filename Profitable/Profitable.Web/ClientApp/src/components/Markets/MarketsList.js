import { useEffect, useState } from 'react';
import { MarketWidget } from './MarketWidget';

export const MarketsList = () => {
    const requiredInstrument = {
        exchangeName: 'CAPITALCOM',
        tickerSymbol: 'UK100'
    };

    const [instrument, setInstrument] = useState({ ...requiredInstrument });

    useEffect(() => {
        fetch(`${process.env.REACT_APP_API_BASE_URL}/api/markets/bgnusd`)
        .then(res => res.json())
        .then(responseInstrument => {
            setInstrument(responseInstrument);
        })
    }, []);

    const widgetStyle = {
        height: '700px'
    }
    
    return (
       <MarketWidget style={widgetStyle} instrument={{...instrument}}/>
    );
}