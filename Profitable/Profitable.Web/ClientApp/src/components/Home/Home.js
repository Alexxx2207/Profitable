import { Watchlist } from '../Watchlist/Watchlist';
import styles from './Home.module.css';

export const Home = () => {
    return <div>
        <main className={styles.main}>
            <section className={styles.overviewSection}>
                <Watchlist className={styles.watchlist} />
                <div className={styles.overviewText}>
                    <h1>Observe</h1>
                    <h2>Plan</h2>
                    <h3>Execute</h3>
                </div>
            </section>
        </main>
    </div>
}