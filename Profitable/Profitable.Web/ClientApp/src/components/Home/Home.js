import { Watchlist } from '../Watchlist/Watchlist';
import { About } from './About/About';
import styles from './Home.module.css';

export const Home = () => {
    return <div>
        <main className={styles.main}>
            <section className={styles.overviewSection}>
                <Watchlist className={styles.watchlist} />
                <div className={styles.overviewText}>
                    <div className={styles.observeDiv}>
                        Observe
                    </div>
                    <div className={styles.planDiv}>
                        Plan
                    </div>
                    <div className={styles.executeDiv}>
                        Execute
                    </div>
                </div>
            </section>
            <section className={styles.aboutSection}>
                <About />
            </section>
        </main>
    </div>
}
