import { WatchList } from '../Watchlist/Watchlist';
import { MarketWidget } from '../Markets/MarketWidget';

export const Home = () => {
    const marketWidgetStyle = { 
        height: "500px"
    };
    const watchListStyle = { 
        height: "500px",
    };

    return <div>
        <main style={marketWidgetStyle}>
            <MarketWidget style={marketWidgetStyle} />
        </main>
        <aside style={watchListStyle}>
            <WatchList style={watchListStyle} />
        </aside>
    </div>;
}