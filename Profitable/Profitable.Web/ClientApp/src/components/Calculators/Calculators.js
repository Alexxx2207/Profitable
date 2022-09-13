import { FuturesCalculator } from './FuturesCalculator/FuturesCalculator';
import styles from './Calculators.module.css';

export const Calcualtors = () => {
    return (
        <div className={styles.calculatorsContainer}>
            <section className={styles.futuresCalculatorContainer}>
                <h3 className={styles.futuresCalculatorHeading}>Futures Calculator</h3>
                <div className={styles.futuresCalculatorWrapper}>
                    <FuturesCalculator />
                </div>
            </section>
        </div>
    );
}