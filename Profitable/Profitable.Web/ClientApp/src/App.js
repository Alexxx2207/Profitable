import { Home } from './components/Home/Home';
import { WatchList } from './components/Watchlist/Watchlist';

export function App() {
    let watchListStyle = {
        height: "800px"
    };

    return (
        <div>
            <aside style={watchListStyle}>
                <WatchList style={watchListStyle} />
            </aside>
            <Home />
        </div>
    );
}
