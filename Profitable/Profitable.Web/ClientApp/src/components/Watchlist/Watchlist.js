import { useEffect, useState } from 'react';
import { MarketOverview } from "react-tradingview-embed";

export function Watchlist() {
    // eslint-disable-next-line
    const [ignored, forceUpdate] = useState('');

    useEffect(() => {
        document.querySelector('.StockMarketContainer').children[0].style.width = "100%";
        document.querySelector('.StockMarketContainer').children[0].style.height = "100%";
        document.querySelector('.StockMarketContainer').style.width = "100%";
        document.querySelector('.StockMarketContainer').style.height = "100%";
        window.addEventListener('resize', forceUpdate);
    }, []);

    return <div className='StockMarketContainer'>
        <MarketOverview widgetProps={{
            "colorTheme": "dark",
            "dateRange": "1D",
            "showChart": true,
            "locale": "en",
            "width": "100%",
            "height": "99%",
            "largeChartUrl": "",
            "isTransparent": true,
            "showSymbolLogo": true,
            "showFloatingTooltip": false,
            "plotLineColorGrowing": "rgba(255, 255, 255, 1)",
            "plotLineColorFalling": "rgba(255, 255, 255, 1)",
            "gridLineColor": "rgba(0, 0, 0, 0)",
            "scaleFontColor": "rgba(120, 123, 134, 1)",
            "belowLineFillColorGrowing": "rgba(255, 255, 255, 0.2)",
            "belowLineFillColorFalling": "rgba(255, 255, 255, 0.2)",
            "belowLineFillColorGrowingBottom": "rgba(255, 255, 255, 0)",
            "belowLineFillColorFallingBottom": "rgba(255, 255, 255, 0)",
            "symbolActiveColor": "rgba(101, 101, 101, 0.12)",
            "tabs": [
                {
                    "title": "Stocks",
                    "symbols": [
                        {
                            "s": "NASDAQ:AAPL"
                        },
                        {
                            "s": "NASDAQ:META"
                        },
                        {
                            "s": "NASDAQ:GOOG"
                        },
                        {
                            "s": "NASDAQ:MSFT"
                        }
                    ]
                },
                {
                    "title": "Commodities",
                    "symbols": [
                        {
                            "s": "TVC:USOIL"
                        },
                        {
                            "s": "TVC:GOLD"
                        },
                        {
                            "s": "CAPITALCOM:NATURALGAS"
                        },
                        {
                            "s": "PEPPERSTONE:WHEAT"
                        }
                    ]
                },
                {
                    "title": "Forex",
                    "symbols": [
                        {
                            "s": "FX_IDC:EURUSD"
                        },
                        {
                            "s": "FX_IDC:CHFUSD"
                        },
                        {
                            "s": "FX_IDC:BGNUSD"
                        },
                        {
                            "s": "FX_IDC:CNYUSD"
                        }
                    ],
                    "originalTitle": "Forex"
                },
                {
                    "title": "Indices",
                    "symbols": [
                        {
                            "s": "BLACKBULL:SPX500"
                        },
                        {
                            "s": "FOREXCOM:DJI"
                        },
                        {
                            "s": "GLOBALPRIME:GER30"
                        },
                        {
                            "s": "CAPITALCOM:UK100"
                        }
                    ],
                    "originalTitle": "Indices"
                },
                {
                    "title": "Crypto",
                    "symbols": [
                        {
                            "s": "COINBASE:BTCUSD"
                        },
                        {
                            "s": "COINBASE:ETHUSD"
                        },
                        {
                            "s": "COINBASE:SOLUSD"
                        },
                        {
                            "s": "BINANCE:EGLDUSD"
                        }
                    ]
                },
                {
                    "title": "Construction Industry",
                    "symbols": [
                        {
                            "s": "NYSE:CAT"
                        },
                        {
                            "s": "NYSE:VMC"
                        },
                        {
                            "s": "NYSE:MLM"
                        },
                        {
                            "s": "NYSE:EXP"
                        },
                    ]
                }
            ]
        }}
        />
    </div>
}