import { useEffect } from 'react';
import { MarketOverview } from "react-tradingview-embed";

export function Watchlist(props) {

    useEffect (() => {
        document.querySelector('.StockMarketContainer').children[0].style.height = "100%";
    });

    return <div className='StockMarketContainer' style={props.style}>
        <MarketOverview widgetProps={{
            "colorTheme": "dark",
            "dateRange": "1D",
            "showChart": true,
            "locale": "en",
            "width": "100%",
            "height": "100%",
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
                            "s": "NASDAQ:TSLA"
                        },
                        {
                            "s": "NASDAQ:AMZN"
                        },
                        {
                            "s": "NASDAQ:META"
                        },
                        {
                            "s": "NASDAQ:MSFT"
                        },
                        {
                            "s": "NASDAQ:NFLX"
                        },
                        {
                            "s": "NYSE:TWTR"
                        },
                        {
                            "s": "NASDAQ:GOOG"
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
                            "s": "TVC:SILVER"
                        },
                        {
                            "s": "CAPITALCOM:COPPER"
                        },
                        {
                            "s": "CAPITALCOM:NATURALGAS"
                        },
                        {
                            "s": "PEPPERSTONE:SUGAR"
                        },
                        {
                            "s": "GLOBALPRIME:CORN"
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
                            "s": "FX_IDC:CADUSD"
                        },
                        {
                            "s": "FX_IDC:BGNUSD"
                        },
                        {
                            "s": "FX_IDC:CNYUSD"
                        },
                        {
                            "s": "FX_IDC:RUBUSD"
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
                            "s": "GLOBALPRIME:GER30"
                        },
                        {
                            "s": "CAPITALCOM:UK100"
                        },
                        {
                            "s": "MOEX:RTSI"
                        },
                        {
                            "s": "SSE:000001"
                        },
                        {
                            "s": "FOREXCOM:DJI"
                        },
                        {
                            "s": "CAPITALCOM:DXY"
                        },
                        {
                            "s": "SKILLING:NASDAQ"
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
                        },
                        {
                            "s": "BINANCE:DOTUSD"
                        },
                        {
                            "s": "BINANCE:LINKUSD"
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
                            "s": "NYSE:URI"
                        },
                        {
                            "s": "NYSE:EXP"
                        },
                        {
                            "s": "NYSE:STN"
                        }
                    ]
                }
            ]
        }}
        />;
    </div>
}