import React from "react";
import { AdvancedChart } from "react-tradingview-embed";

export function MarketWidget({ instrument }) {
    return (
        <div style={{ height: "1100px" }}>
            <AdvancedChart
                widgetProps={{
                    width: "100%",
                    height: "100%",
                    symbol: `${instrument.exchangeName}:${instrument.tickerSymbol}`,
                    timezone: "Atlantic/Reykjavik",
                    theme: "dark",
                    locale: "en",
                    toolbar_bg: "#f1f3f6",
                    enable_publishing: false,
                    withdateranges: true,
                    hide_side_toolbar: false,
                    details: true,
                    container_id: "tradingview",
                }}
            />
        </div>
    );
}
